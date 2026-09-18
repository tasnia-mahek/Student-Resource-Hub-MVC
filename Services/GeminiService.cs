using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace student_resource_hub.Services
{
    public class GeminiService : IGeminiService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<GeminiService> _logger;

        private const string DefaultModel = "gemini-3.8-flash";
        private const string TemporaryServiceFallbackModel = "gemini-3.7-flash";

        public GeminiService(
            HttpClient httpClient,
            IConfiguration configuration,
            ILogger<GeminiService> logger)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _logger = logger;
            _httpClient.Timeout = TimeSpan.FromSeconds(30);
        }

        private string? GetApiKey()
        {
            var key = _configuration["Gemini:ApiKey"];
            if (string.IsNullOrWhiteSpace(key))
            {
                key = Environment.GetEnvironmentVariable("GEMINI_API_KEY");
            }
            if (string.IsNullOrWhiteSpace(key))
            {
                key = _configuration["GEMINI_API_KEY"];
            }

            return key?.Trim();
        }

        private string GetNormalizedModelName()
        {
            var model = _configuration["Gemini:Model"]?.Trim();
            if (string.IsNullOrWhiteSpace(model))
            {
                return DefaultModel;
            }

            return model;
        }

        public bool IsConfigured
        {
            get
            {
                var key = GetApiKey();
                return !string.IsNullOrWhiteSpace(key) &&
                       !key.Equals("YOUR_GEMINI_API_KEY", StringComparison.OrdinalIgnoreCase) &&
                       !key.Equals("YOUR_API_KEY_HERE", StringComparison.OrdinalIgnoreCase);
            }
        }

        public string GetMissingConfigurationMessage()
        {
            return @"<div style=""background: rgba(245, 158, 11, 0.1); border: 1px solid rgba(245, 158, 11, 0.4); border-radius: 12px; padding: 1rem 1.25rem; margin: 0.5rem 0 1rem 0;"">
    <h4 style=""color: #f59e0b; margin: 0 0 0.5rem 0; font-size: 0.95rem; display: flex; align-items: center; gap: 6px;"">
        <span>⚠️</span> <span>Gemini AI API Key Not Configured</span>
    </h4>
    <p style=""margin: 0 0 0.6rem 0; font-size: 0.88rem; color: #e5e7eb; line-height: 1.5;"">
        To enable general conversation and academic explanations, you need to configure a Gemini API key. Please add your key in any of the following places:
    </p>
    <ol style=""margin: 0.25rem 0 0.75rem 1.25rem; padding: 0; font-size: 0.84rem; color: #9ca3af; line-height: 1.6;"">
        <li style=""margin-bottom: 0.5rem;"">
            <strong>In <code>appsettings.json</code>:</strong><br/>
            <pre style=""background: #090a0f; border: 1px solid rgba(255, 255, 255, 0.1); padding: 0.4rem 0.6rem; border-radius: 6px; margin: 0.25rem 0; color: #00ff66; font-size: 0.8rem;"">""Gemini"": {
  ""ApiKey"": ""YOUR_GEMINI_API_KEY""
}</pre>
        </li>
        <li style=""margin-bottom: 0.5rem;"">
            <strong>Or as an Environment Variable:</strong><br/>
            <code style=""color: #38bdf8; background: #090a0f; padding: 2px 6px; border-radius: 4px;"">GEMINI_API_KEY=YOUR_GEMINI_API_KEY</code>
        </li>
        <li>
            <strong>Or using .NET User Secrets (Development):</strong><br/>
            <code style=""color: #38bdf8; background: #090a0f; padding: 2px 6px; border-radius: 4px;"">dotnet user-secrets set ""Gemini:ApiKey"" ""YOUR_GEMINI_API_KEY""</code>
        </li>
    </ol>
    <p style=""margin: 0; font-size: 0.82rem; color: #9ca3af;"">
        Get your free API key at <a href=""https://aistudio.google.com/"" target=""_blank"" style=""color: #00ff66; font-weight: 600; text-decoration: underline;"">Google AI Studio &rarr;</a>
    </p>
</div>";
        }

        public async Task<string> GenerateAnswerAsync(string prompt, string? systemContext = null)
        {
            var apiKey = GetApiKey();
            if (string.IsNullOrWhiteSpace(apiKey) ||
                apiKey.Equals("YOUR_GEMINI_API_KEY", StringComparison.OrdinalIgnoreCase) ||
                apiKey.Equals("YOUR_API_KEY_HERE", StringComparison.OrdinalIgnoreCase))
            {
                return GetMissingConfigurationMessage();
            }

            var primaryModel = GetNormalizedModelName();

            var systemInstructionText = "You are StudyHub AI Copilot, an expert university academic assistant. " +
                                       "Help students understand concepts, solve problems, prepare for exams, and answer questions thoroughly yet concisely. " +
                                       "Use clean, formatted HTML (such as <p>, <strong>, <em>, <code>, <pre>, <ul>, <ol>, <li>) for clarity. " +
                                       "Do not wrap your overall response in a markdown code fence like ```html.";

            if (!string.IsNullOrWhiteSpace(systemContext))
            {
                systemInstructionText += $" Context: {systemContext}";
            }

            var requestPayload = new
            {
                contents = new[]
                {
                    new
                    {
                        role = "user",
                        parts = new[]
                        {
                            new { text = prompt }
                        }
                    }
                },
                systemInstruction = new
                {
                    parts = new[]
                    {
                        new { text = systemInstructionText }
                    }
                },
                generationConfig = new
                {
                    temperature = 0.7,
                    maxOutputTokens = 2048
                }
            };

            try
            {
                var requestJson = JsonSerializer.Serialize(requestPayload);
                var modelUsed = primaryModel;
                var (statusCode, responseBody) = await GenerateContentAsync(primaryModel, apiKey, requestJson);

                // Only retry on the temporary service-unavailable response. Other failures are
                // returned to the caller unchanged and never trigger a database search.
                if (statusCode == HttpStatusCode.ServiceUnavailable &&
                    !primaryModel.Equals(TemporaryServiceFallbackModel, StringComparison.OrdinalIgnoreCase))
                {
                    _logger.LogWarning(
                        "Gemini model {Model} returned HTTP {StatusCode}; retrying once with {FallbackModel}.",
                        primaryModel,
                        (int)statusCode,
                        TemporaryServiceFallbackModel);

                    modelUsed = TemporaryServiceFallbackModel;
                    (statusCode, responseBody) = await GenerateContentAsync(modelUsed, apiKey, requestJson);
                }

                _logger.LogInformation(
                    "Gemini request completed using model {Model} with HTTP {StatusCode}.",
                    modelUsed,
                    (int)statusCode);

                if ((int)statusCode < 200 || (int)statusCode > 299)
                {
                    _logger.LogWarning("Gemini API call failed using model {Model} with HTTP {StatusCode}.", modelUsed, (int)statusCode);

                    try
                    {
                        using var doc = JsonDocument.Parse(responseBody);
                        if (doc.RootElement.TryGetProperty("error", out var errorEl) &&
                            errorEl.TryGetProperty("message", out var msgEl))
                        {
                            return $"<p style=\"color: #f43f5e;\">Gemini AI Error: {System.Net.WebUtility.HtmlEncode(msgEl.GetString())}</p>";
                        }
                    }
                    catch
                    {
                        // Fallback to generic message
                    }

                    return "<p style=\"color: #f43f5e;\">Sorry, I encountered an issue connecting to Gemini AI. Please check your API key or try again in a moment.</p>";
                }

                using var responseDoc = JsonDocument.Parse(responseBody);
                if (responseDoc.RootElement.TryGetProperty("candidates", out var candidates) &&
                    candidates.GetArrayLength() > 0)
                {
                    var firstCandidate = candidates[0];
                    if (firstCandidate.TryGetProperty("content", out var content) &&
                        content.TryGetProperty("parts", out var parts) &&
                        parts.GetArrayLength() > 0)
                    {
                        var text = parts[0].GetProperty("text").GetString() ?? string.Empty;
                        return FormatGeminiOutput(text);
                    }
                }

                return "<p>I could not generate a response for that inquiry. Please try rephrasing your question.</p>";
            }
            catch (TaskCanceledException)
            {
                _logger.LogWarning("Gemini API request timed out for prompt: {Prompt}", prompt);
                return "<p style=\"color: #f43f5e;\">The request to Gemini AI timed out. Please try again shortly.</p>";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error communicating with Gemini AI.");
                return "<p style=\"color: #f43f5e;\">An unexpected error occurred while communicating with Gemini AI.</p>";
            }
        }

        private async Task<(HttpStatusCode StatusCode, string ResponseBody)> GenerateContentAsync(
            string model,
            string apiKey,
            string requestJson)
        {
            var requestUrl = $"https://generativelanguage.googleapis.com/v1beta/models/{model}:generateContent?key={apiKey}";
            using var requestMessage = new HttpRequestMessage(HttpMethod.Post, requestUrl)
            {
                Content = new StringContent(requestJson, Encoding.UTF8, "application/json")
            };
            requestMessage.Headers.Add("x-goog-api-key", apiKey);

            using var response = await _httpClient.SendAsync(requestMessage);
            return (response.StatusCode, await response.Content.ReadAsStringAsync());
        }

        private static string FormatGeminiOutput(string rawText)
        {
            if (string.IsNullOrWhiteSpace(rawText))
            {
                return string.Empty;
            }

            var text = rawText.Trim();

            // If already formatted HTML with root paragraph or tags, sanitize fences if present
            if (text.StartsWith("```html", StringComparison.OrdinalIgnoreCase))
            {
                text = text[7..];
                if (text.EndsWith("```"))
                {
                    text = text[..^3];
                }
                return text.Trim();
            }

            if (text.StartsWith("```", StringComparison.OrdinalIgnoreCase) && text.EndsWith("```"))
            {
                text = text[3..^3].Trim();
            }

            // Convert common markdown patterns to HTML if markdown is present
            return MarkdownToHtml(text);
        }

        private static string MarkdownToHtml(string markdown)
        {
            var sb = new StringBuilder();

            // 1. Extract code blocks and replace with placeholders
            var codeBlocks = new List<string>();
            var text = Regex.Replace(markdown, @"```(\w*)\n([\s\S]*?)```", m =>
            {
                var lang = m.Groups[1].Value.Trim();
                var code = System.Net.WebUtility.HtmlEncode(m.Groups[2].Value.TrimEnd());
                var langBadge = string.IsNullOrEmpty(lang) ? "" : $"<div style=\"font-size: 0.72rem; color: #9ca3af; text-transform: uppercase; margin-bottom: 4px;\">{lang}</div>";
                var placeholder = $"___CODE_BLOCK_{codeBlocks.Count}___";
                codeBlocks.Add($"{langBadge}<pre style=\"background: #090a0f; border: 1px solid rgba(255,255,255,0.1); border-radius: 8px; padding: 0.75rem 1rem; overflow-x: auto; color: #00ff66; font-family: monospace; font-size: 0.88rem;\"><code>{code}</code></pre>");
                return placeholder;
            });

            // 2. Convert inline markdown
            // Headers
            text = Regex.Replace(text, @"^### (.*)$", "<h4 style=\"color: #00ff66; margin: 0.75rem 0 0.35rem; font-size: 0.95rem;\">$1</h4>", RegexOptions.Multiline);
            text = Regex.Replace(text, @"^## (.*)$", "<h3 style=\"color: #ffffff; margin: 0.85rem 0 0.4rem; font-size: 1.05rem;\">$1</h3>", RegexOptions.Multiline);
            text = Regex.Replace(text, @"^# (.*)$", "<h2 style=\"color: #ffffff; margin: 1rem 0 0.5rem; font-size: 1.15rem;\">$1</h2>", RegexOptions.Multiline);

            // Bold and italic
            text = Regex.Replace(text, @"\*\*([^*]+)\*\*", "<strong>$1</strong>");
            text = Regex.Replace(text, @"\*([^*]+)\*", "<em>$1</em>");

            // Inline code
            text = Regex.Replace(text, @"`([^`]+)`", "<code style=\"background: rgba(255,255,255,0.08); padding: 2px 5px; border-radius: 4px; color: #38bdf8; font-family: monospace; font-size: 0.88em;\">$1</code>");

            // 3. Process paragraphs and lists
            var lines = text.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            bool inList = false;
            bool isOrderedList = false;

            for (int i = 0; i < lines.Length; i++)
            {
                var line = lines[i].Trim();

                if (string.IsNullOrEmpty(line))
                {
                    if (inList)
                    {
                        sb.Append(isOrderedList ? "</ol>" : "</ul>");
                        inList = false;
                    }
                    continue;
                }

                // Check list items
                var ulMatch = Regex.Match(line, @"^[*-]\s+(.*)$");
                var olMatch = Regex.Match(line, @"^\d+\.\s+(.*)$");

                if (ulMatch.Success)
                {
                    if (!inList || isOrderedList)
                    {
                        if (inList) sb.Append(isOrderedList ? "</ol>" : "</ul>");
                        sb.Append("<ul style=\"margin: 0.4rem 0 0.6rem 1.25rem; padding: 0;\">");
                        inList = true;
                        isOrderedList = false;
                    }
                    sb.Append($"<li style=\"margin-bottom: 0.25rem;\">{ulMatch.Groups[1].Value}</li>");
                    continue;
                }

                if (olMatch.Success)
                {
                    if (!inList || !isOrderedList)
                    {
                        if (inList) sb.Append(isOrderedList ? "</ol>" : "</ul>");
                        sb.Append("<ol style=\"margin: 0.4rem 0 0.6rem 1.25rem; padding: 0;\">");
                        inList = true;
                        isOrderedList = true;
                    }
                    sb.Append($"<li style=\"margin-bottom: 0.25rem;\">{olMatch.Groups[1].Value}</li>");
                    continue;
                }

                if (inList)
                {
                    sb.Append(isOrderedList ? "</ol>" : "</ul>");
                    inList = false;
                }

                if (line.StartsWith("<h") || line.StartsWith("___CODE_BLOCK_"))
                {
                    sb.Append(line);
                }
                else
                {
                    sb.Append($"<p style=\"margin-bottom: 0.5rem;\">{line}</p>");
                }
            }

            if (inList)
            {
                sb.Append(isOrderedList ? "</ol>" : "</ul>");
            }

            var result = sb.ToString();

            // Restore code blocks
            for (int i = 0; i < codeBlocks.Count; i++)
            {
                result = result.Replace($"___CODE_BLOCK_{i}___", codeBlocks[i]);
            }

            return string.IsNullOrWhiteSpace(result) ? markdown : result;
        }
    }
}
