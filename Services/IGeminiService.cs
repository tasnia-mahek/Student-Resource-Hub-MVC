namespace student_resource_hub.Services
{
    public interface IGeminiService
    {
        /// <summary>
        /// Gets a value indicating whether a valid Gemini API key is configured.
        /// </summary>
        bool IsConfigured { get; }

        /// <summary>
        /// Returns a formatted message guiding the user on how and where to configure the Gemini API key.
        /// </summary>
        string GetMissingConfigurationMessage();

        /// <summary>
        /// Generates an AI response from Gemini for conversational or academic queries.
        /// </summary>
        /// <param name="prompt">User's prompt or question.</param>
        /// <param name="systemContext">Optional context regarding course materials or instructions.</param>
        /// <returns>Formatted HTML reply.</returns>
        Task<string> GenerateAnswerAsync(string prompt, string? systemContext = null);
    }
}
