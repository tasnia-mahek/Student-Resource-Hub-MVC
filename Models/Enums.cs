namespace student_resource_hub.Models
{
    public enum Department
    {
        ComputerScience,
        Engineering,
        Mathematics,
        Physics,
        Chemistry,
        Business
    }

    public enum Year
    {
        Year2021 = 2021,
        Year2022 = 2022,
        Year2023 = 2023,
        Year2024 = 2024,
        Year2025 = 2025,
        Year2026 = 2026
    }

    public enum Semester
    {
        Fall,
        Spring,
        Summer
    }

    public enum ResourceStatus
    {
        Pending,
        Approved,
        Rejected
    }
}
