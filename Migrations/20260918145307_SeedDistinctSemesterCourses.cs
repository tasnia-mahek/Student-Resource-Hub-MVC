using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace student_resource_hub.Migrations
{
    /// <inheritdoc />
    public partial class SeedDistinctSemesterCourses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AcademicCourses",
                columns: new[] { "Id", "AcademicSemesterId", "CourseCode", "IsActive", "IsLab", "Name" },
                values: new object[,]
                {
                    { 20000, 11, "1MAT101", true, false, "Calculus I" },
                    { 20001, 11, "1PHY101", true, false, "Physics I" },
                    { 20002, 11, "1CSE101", true, false, "Programming Fundamentals" },
                    { 20003, 11, "1EEE101", true, false, "Electrical Circuits" },
                    { 20004, 11, "1ENG101", true, false, "Academic Writing" },
                    { 20005, 11, "1CSE111", true, true, "Programming Lab" },
                    { 20006, 11, "1PHY111", true, true, "Physics Lab" },
                    { 20007, 11, "1EEE111", true, true, "Circuit Lab" },
                    { 20008, 11, "1ENG111", true, true, "Engineering Graphics Lab" },
                    { 20009, 12, "1MAT102", true, false, "Calculus II" },
                    { 20010, 12, "1CSE102", true, false, "Object Oriented Programming" },
                    { 20011, 12, "1EEE102", true, false, "Digital Logic" },
                    { 20012, 12, "1STA102", true, false, "Probability and Statistics" },
                    { 20013, 12, "1HUM102", true, false, "Professional Ethics" },
                    { 20014, 12, "1CSE112", true, true, "OOP Lab" },
                    { 20015, 12, "1EEE112", true, true, "Digital Logic Lab" },
                    { 20016, 12, "1MAT112", true, true, "Numerical Methods Lab" },
                    { 20017, 12, "1CSE122", true, true, "Technical Skills Lab" },
                    { 20018, 13, "1MAT203", true, false, "Linear Algebra" },
                    { 20019, 13, "1CSE203", true, false, "Data Structures" },
                    { 20020, 13, "1CSE213", true, false, "Computer Organization" },
                    { 20021, 13, "1EEE203", true, false, "Signals and Systems" },
                    { 20022, 13, "1ECO203", true, false, "Engineering Economics" },
                    { 20023, 13, "1CSE223", true, true, "Data Structures Lab" },
                    { 20024, 13, "1CSE233", true, true, "Computer Organization Lab" },
                    { 20025, 13, "1EEE223", true, true, "Signals Lab" },
                    { 20026, 13, "1CSE243", true, true, "Web Development Lab" },
                    { 20027, 14, "1CSE204", true, false, "Algorithms" },
                    { 20028, 14, "1CSE214", true, false, "Database Systems" },
                    { 20029, 14, "1CSE224", true, false, "Operating Systems" },
                    { 20030, 14, "1CSE234", true, false, "Probability for Computing" },
                    { 20031, 14, "1BUS204", true, false, "Management Principles" },
                    { 20032, 14, "1CSE224L", true, true, "Algorithms Lab" },
                    { 20033, 14, "1CSE234L", true, true, "Database Lab" },
                    { 20034, 14, "1CSE244L", true, true, "Operating Systems Lab" },
                    { 20035, 14, "1CSE254L", true, true, "Systems Programming Lab" },
                    { 20036, 15, "1CSE305", true, false, "Computer Networks" },
                    { 20037, 15, "1CSE315", true, false, "Software Engineering" },
                    { 20038, 15, "1CSE325", true, false, "Theory of Computation" },
                    { 20039, 15, "1CSE335", true, false, "Compiler Design" },
                    { 20040, 15, "1CSE345", true, false, "Human Computer Interaction" },
                    { 20041, 15, "1CSE355", true, true, "Networks Lab" },
                    { 20042, 15, "1CSE365", true, true, "Software Engineering Lab" },
                    { 20043, 15, "1CSE375", true, true, "Compiler Lab" },
                    { 20044, 15, "1CSE385", true, true, "UI Design Lab" },
                    { 20045, 16, "1CSE306", true, false, "Artificial Intelligence" },
                    { 20046, 16, "1CSE316", true, false, "Machine Learning" },
                    { 20047, 16, "1CSE326", true, false, "Distributed Systems" },
                    { 20048, 16, "1CSE336", true, false, "Information Security" },
                    { 20049, 16, "1CSE346", true, false, "Data Mining" },
                    { 20050, 16, "1CSE356", true, true, "AI Lab" },
                    { 20051, 16, "1CSE366", true, true, "Machine Learning Lab" },
                    { 20052, 16, "1CSE376", true, true, "Security Lab" },
                    { 20053, 16, "1CSE386", true, true, "Data Mining Lab" },
                    { 20054, 17, "1CSE407", true, false, "Cloud Computing" },
                    { 20055, 17, "1CSE417", true, false, "Mobile Application Development" },
                    { 20056, 17, "1CSE427", true, false, "Computer Graphics" },
                    { 20057, 17, "1CSE437", true, false, "Big Data Analytics" },
                    { 20058, 17, "1CSE447", true, false, "Embedded Systems" },
                    { 20059, 17, "1CSE457", true, true, "Cloud Lab" },
                    { 20060, 17, "1CSE467", true, true, "Mobile Development Lab" },
                    { 20061, 17, "1CSE477", true, true, "Graphics Lab" },
                    { 20062, 17, "1CSE487", true, true, "Embedded Systems Lab" },
                    { 20063, 18, "1CSE408", true, false, "Advanced Algorithms" },
                    { 20064, 18, "1CSE418", true, false, "Natural Language Processing" },
                    { 20065, 18, "1CSE428", true, false, "Deep Learning" },
                    { 20066, 18, "1CSE438", true, false, "Distributed Artificial Intelligence" },
                    { 20067, 18, "1CSE448", true, false, "Project Management" },
                    { 20068, 18, "1CSE458", true, true, "NLP Lab" },
                    { 20069, 18, "1CSE468", true, true, "Deep Learning Lab" },
                    { 20070, 18, "1CSE478", true, true, "Research Lab" },
                    { 20071, 18, "1CSE488", true, true, "Capstone Design Lab" },
                    { 20072, 21, "2MAT101", true, false, "Calculus I" },
                    { 20073, 21, "2PHY101", true, false, "Physics I" },
                    { 20074, 21, "2CSE101", true, false, "Programming Fundamentals" },
                    { 20075, 21, "2EEE101", true, false, "Electrical Circuits" },
                    { 20076, 21, "2ENG101", true, false, "Academic Writing" },
                    { 20077, 21, "2CSE111", true, true, "Programming Lab" },
                    { 20078, 21, "2PHY111", true, true, "Physics Lab" },
                    { 20079, 21, "2EEE111", true, true, "Circuit Lab" },
                    { 20080, 21, "2ENG111", true, true, "Engineering Graphics Lab" },
                    { 20081, 22, "2MAT102", true, false, "Calculus II" },
                    { 20082, 22, "2CSE102", true, false, "Object Oriented Programming" },
                    { 20083, 22, "2EEE102", true, false, "Digital Logic" },
                    { 20084, 22, "2STA102", true, false, "Probability and Statistics" },
                    { 20085, 22, "2HUM102", true, false, "Professional Ethics" },
                    { 20086, 22, "2CSE112", true, true, "OOP Lab" },
                    { 20087, 22, "2EEE112", true, true, "Digital Logic Lab" },
                    { 20088, 22, "2MAT112", true, true, "Numerical Methods Lab" },
                    { 20089, 22, "2CSE122", true, true, "Technical Skills Lab" },
                    { 20090, 23, "2MAT203", true, false, "Linear Algebra" },
                    { 20091, 23, "2CSE203", true, false, "Data Structures" },
                    { 20092, 23, "2CSE213", true, false, "Computer Organization" },
                    { 20093, 23, "2EEE203", true, false, "Signals and Systems" },
                    { 20094, 23, "2ECO203", true, false, "Engineering Economics" },
                    { 20095, 23, "2CSE223", true, true, "Data Structures Lab" },
                    { 20096, 23, "2CSE233", true, true, "Computer Organization Lab" },
                    { 20097, 23, "2EEE223", true, true, "Signals Lab" },
                    { 20098, 23, "2CSE243", true, true, "Web Development Lab" },
                    { 20099, 24, "2CSE204", true, false, "Algorithms" },
                    { 20100, 24, "2CSE214", true, false, "Database Systems" },
                    { 20101, 24, "2CSE224", true, false, "Operating Systems" },
                    { 20102, 24, "2CSE234", true, false, "Probability for Computing" },
                    { 20103, 24, "2BUS204", true, false, "Management Principles" },
                    { 20104, 24, "2CSE224L", true, true, "Algorithms Lab" },
                    { 20105, 24, "2CSE234L", true, true, "Database Lab" },
                    { 20106, 24, "2CSE244L", true, true, "Operating Systems Lab" },
                    { 20107, 24, "2CSE254L", true, true, "Systems Programming Lab" },
                    { 20108, 25, "2CSE305", true, false, "Computer Networks" },
                    { 20109, 25, "2CSE315", true, false, "Software Engineering" },
                    { 20110, 25, "2CSE325", true, false, "Theory of Computation" },
                    { 20111, 25, "2CSE335", true, false, "Compiler Design" },
                    { 20112, 25, "2CSE345", true, false, "Human Computer Interaction" },
                    { 20113, 25, "2CSE355", true, true, "Networks Lab" },
                    { 20114, 25, "2CSE365", true, true, "Software Engineering Lab" },
                    { 20115, 25, "2CSE375", true, true, "Compiler Lab" },
                    { 20116, 25, "2CSE385", true, true, "UI Design Lab" },
                    { 20117, 26, "2CSE306", true, false, "Artificial Intelligence" },
                    { 20118, 26, "2CSE316", true, false, "Machine Learning" },
                    { 20119, 26, "2CSE326", true, false, "Distributed Systems" },
                    { 20120, 26, "2CSE336", true, false, "Information Security" },
                    { 20121, 26, "2CSE346", true, false, "Data Mining" },
                    { 20122, 26, "2CSE356", true, true, "AI Lab" },
                    { 20123, 26, "2CSE366", true, true, "Machine Learning Lab" },
                    { 20124, 26, "2CSE376", true, true, "Security Lab" },
                    { 20125, 26, "2CSE386", true, true, "Data Mining Lab" },
                    { 20126, 27, "2CSE407", true, false, "Cloud Computing" },
                    { 20127, 27, "2CSE417", true, false, "Mobile Application Development" },
                    { 20128, 27, "2CSE427", true, false, "Computer Graphics" },
                    { 20129, 27, "2CSE437", true, false, "Big Data Analytics" },
                    { 20130, 27, "2CSE447", true, false, "Embedded Systems" },
                    { 20131, 27, "2CSE457", true, true, "Cloud Lab" },
                    { 20132, 27, "2CSE467", true, true, "Mobile Development Lab" },
                    { 20133, 27, "2CSE477", true, true, "Graphics Lab" },
                    { 20134, 27, "2CSE487", true, true, "Embedded Systems Lab" },
                    { 20135, 28, "2CSE408", true, false, "Advanced Algorithms" },
                    { 20136, 28, "2CSE418", true, false, "Natural Language Processing" },
                    { 20137, 28, "2CSE428", true, false, "Deep Learning" },
                    { 20138, 28, "2CSE438", true, false, "Distributed Artificial Intelligence" },
                    { 20139, 28, "2CSE448", true, false, "Project Management" },
                    { 20140, 28, "2CSE458", true, true, "NLP Lab" },
                    { 20141, 28, "2CSE468", true, true, "Deep Learning Lab" },
                    { 20142, 28, "2CSE478", true, true, "Research Lab" },
                    { 20143, 28, "2CSE488", true, true, "Capstone Design Lab" },
                    { 20144, 31, "3MAT101", true, false, "Calculus I" },
                    { 20145, 31, "3PHY101", true, false, "Physics I" },
                    { 20146, 31, "3CSE101", true, false, "Programming Fundamentals" },
                    { 20147, 31, "3EEE101", true, false, "Electrical Circuits" },
                    { 20148, 31, "3ENG101", true, false, "Academic Writing" },
                    { 20149, 31, "3CSE111", true, true, "Programming Lab" },
                    { 20150, 31, "3PHY111", true, true, "Physics Lab" },
                    { 20151, 31, "3EEE111", true, true, "Circuit Lab" },
                    { 20152, 31, "3ENG111", true, true, "Engineering Graphics Lab" },
                    { 20153, 32, "3MAT102", true, false, "Calculus II" },
                    { 20154, 32, "3CSE102", true, false, "Object Oriented Programming" },
                    { 20155, 32, "3EEE102", true, false, "Digital Logic" },
                    { 20156, 32, "3STA102", true, false, "Probability and Statistics" },
                    { 20157, 32, "3HUM102", true, false, "Professional Ethics" },
                    { 20158, 32, "3CSE112", true, true, "OOP Lab" },
                    { 20159, 32, "3EEE112", true, true, "Digital Logic Lab" },
                    { 20160, 32, "3MAT112", true, true, "Numerical Methods Lab" },
                    { 20161, 32, "3CSE122", true, true, "Technical Skills Lab" },
                    { 20162, 33, "3MAT203", true, false, "Linear Algebra" },
                    { 20163, 33, "3CSE203", true, false, "Data Structures" },
                    { 20164, 33, "3CSE213", true, false, "Computer Organization" },
                    { 20165, 33, "3EEE203", true, false, "Signals and Systems" },
                    { 20166, 33, "3ECO203", true, false, "Engineering Economics" },
                    { 20167, 33, "3CSE223", true, true, "Data Structures Lab" },
                    { 20168, 33, "3CSE233", true, true, "Computer Organization Lab" },
                    { 20169, 33, "3EEE223", true, true, "Signals Lab" },
                    { 20170, 33, "3CSE243", true, true, "Web Development Lab" },
                    { 20171, 34, "3CSE204", true, false, "Algorithms" },
                    { 20172, 34, "3CSE214", true, false, "Database Systems" },
                    { 20173, 34, "3CSE224", true, false, "Operating Systems" },
                    { 20174, 34, "3CSE234", true, false, "Probability for Computing" },
                    { 20175, 34, "3BUS204", true, false, "Management Principles" },
                    { 20176, 34, "3CSE224L", true, true, "Algorithms Lab" },
                    { 20177, 34, "3CSE234L", true, true, "Database Lab" },
                    { 20178, 34, "3CSE244L", true, true, "Operating Systems Lab" },
                    { 20179, 34, "3CSE254L", true, true, "Systems Programming Lab" },
                    { 20180, 35, "3CSE305", true, false, "Computer Networks" },
                    { 20181, 35, "3CSE315", true, false, "Software Engineering" },
                    { 20182, 35, "3CSE325", true, false, "Theory of Computation" },
                    { 20183, 35, "3CSE335", true, false, "Compiler Design" },
                    { 20184, 35, "3CSE345", true, false, "Human Computer Interaction" },
                    { 20185, 35, "3CSE355", true, true, "Networks Lab" },
                    { 20186, 35, "3CSE365", true, true, "Software Engineering Lab" },
                    { 20187, 35, "3CSE375", true, true, "Compiler Lab" },
                    { 20188, 35, "3CSE385", true, true, "UI Design Lab" },
                    { 20189, 36, "3CSE306", true, false, "Artificial Intelligence" },
                    { 20190, 36, "3CSE316", true, false, "Machine Learning" },
                    { 20191, 36, "3CSE326", true, false, "Distributed Systems" },
                    { 20192, 36, "3CSE336", true, false, "Information Security" },
                    { 20193, 36, "3CSE346", true, false, "Data Mining" },
                    { 20194, 36, "3CSE356", true, true, "AI Lab" },
                    { 20195, 36, "3CSE366", true, true, "Machine Learning Lab" },
                    { 20196, 36, "3CSE376", true, true, "Security Lab" },
                    { 20197, 36, "3CSE386", true, true, "Data Mining Lab" },
                    { 20198, 37, "3CSE407", true, false, "Cloud Computing" },
                    { 20199, 37, "3CSE417", true, false, "Mobile Application Development" },
                    { 20200, 37, "3CSE427", true, false, "Computer Graphics" },
                    { 20201, 37, "3CSE437", true, false, "Big Data Analytics" },
                    { 20202, 37, "3CSE447", true, false, "Embedded Systems" },
                    { 20203, 37, "3CSE457", true, true, "Cloud Lab" },
                    { 20204, 37, "3CSE467", true, true, "Mobile Development Lab" },
                    { 20205, 37, "3CSE477", true, true, "Graphics Lab" },
                    { 20206, 37, "3CSE487", true, true, "Embedded Systems Lab" },
                    { 20207, 38, "3CSE408", true, false, "Advanced Algorithms" },
                    { 20208, 38, "3CSE418", true, false, "Natural Language Processing" },
                    { 20209, 38, "3CSE428", true, false, "Deep Learning" },
                    { 20210, 38, "3CSE438", true, false, "Distributed Artificial Intelligence" },
                    { 20211, 38, "3CSE448", true, false, "Project Management" },
                    { 20212, 38, "3CSE458", true, true, "NLP Lab" },
                    { 20213, 38, "3CSE468", true, true, "Deep Learning Lab" },
                    { 20214, 38, "3CSE478", true, true, "Research Lab" },
                    { 20215, 38, "3CSE488", true, true, "Capstone Design Lab" },
                    { 20216, 41, "4MAT101", true, false, "Calculus I" },
                    { 20217, 41, "4PHY101", true, false, "Physics I" },
                    { 20218, 41, "4CSE101", true, false, "Programming Fundamentals" },
                    { 20219, 41, "4EEE101", true, false, "Electrical Circuits" },
                    { 20220, 41, "4ENG101", true, false, "Academic Writing" },
                    { 20221, 41, "4CSE111", true, true, "Programming Lab" },
                    { 20222, 41, "4PHY111", true, true, "Physics Lab" },
                    { 20223, 41, "4EEE111", true, true, "Circuit Lab" },
                    { 20224, 41, "4ENG111", true, true, "Engineering Graphics Lab" },
                    { 20225, 42, "4MAT102", true, false, "Calculus II" },
                    { 20226, 42, "4CSE102", true, false, "Object Oriented Programming" },
                    { 20227, 42, "4EEE102", true, false, "Digital Logic" },
                    { 20228, 42, "4STA102", true, false, "Probability and Statistics" },
                    { 20229, 42, "4HUM102", true, false, "Professional Ethics" },
                    { 20230, 42, "4CSE112", true, true, "OOP Lab" },
                    { 20231, 42, "4EEE112", true, true, "Digital Logic Lab" },
                    { 20232, 42, "4MAT112", true, true, "Numerical Methods Lab" },
                    { 20233, 42, "4CSE122", true, true, "Technical Skills Lab" },
                    { 20234, 43, "4MAT203", true, false, "Linear Algebra" },
                    { 20235, 43, "4CSE203", true, false, "Data Structures" },
                    { 20236, 43, "4CSE213", true, false, "Computer Organization" },
                    { 20237, 43, "4EEE203", true, false, "Signals and Systems" },
                    { 20238, 43, "4ECO203", true, false, "Engineering Economics" },
                    { 20239, 43, "4CSE223", true, true, "Data Structures Lab" },
                    { 20240, 43, "4CSE233", true, true, "Computer Organization Lab" },
                    { 20241, 43, "4EEE223", true, true, "Signals Lab" },
                    { 20242, 43, "4CSE243", true, true, "Web Development Lab" },
                    { 20243, 44, "4CSE204", true, false, "Algorithms" },
                    { 20244, 44, "4CSE214", true, false, "Database Systems" },
                    { 20245, 44, "4CSE224", true, false, "Operating Systems" },
                    { 20246, 44, "4CSE234", true, false, "Probability for Computing" },
                    { 20247, 44, "4BUS204", true, false, "Management Principles" },
                    { 20248, 44, "4CSE224L", true, true, "Algorithms Lab" },
                    { 20249, 44, "4CSE234L", true, true, "Database Lab" },
                    { 20250, 44, "4CSE244L", true, true, "Operating Systems Lab" },
                    { 20251, 44, "4CSE254L", true, true, "Systems Programming Lab" },
                    { 20252, 45, "4CSE305", true, false, "Computer Networks" },
                    { 20253, 45, "4CSE315", true, false, "Software Engineering" },
                    { 20254, 45, "4CSE325", true, false, "Theory of Computation" },
                    { 20255, 45, "4CSE335", true, false, "Compiler Design" },
                    { 20256, 45, "4CSE345", true, false, "Human Computer Interaction" },
                    { 20257, 45, "4CSE355", true, true, "Networks Lab" },
                    { 20258, 45, "4CSE365", true, true, "Software Engineering Lab" },
                    { 20259, 45, "4CSE375", true, true, "Compiler Lab" },
                    { 20260, 45, "4CSE385", true, true, "UI Design Lab" },
                    { 20261, 46, "4CSE306", true, false, "Artificial Intelligence" },
                    { 20262, 46, "4CSE316", true, false, "Machine Learning" },
                    { 20263, 46, "4CSE326", true, false, "Distributed Systems" },
                    { 20264, 46, "4CSE336", true, false, "Information Security" },
                    { 20265, 46, "4CSE346", true, false, "Data Mining" },
                    { 20266, 46, "4CSE356", true, true, "AI Lab" },
                    { 20267, 46, "4CSE366", true, true, "Machine Learning Lab" },
                    { 20268, 46, "4CSE376", true, true, "Security Lab" },
                    { 20269, 46, "4CSE386", true, true, "Data Mining Lab" },
                    { 20270, 47, "4CSE407", true, false, "Cloud Computing" },
                    { 20271, 47, "4CSE417", true, false, "Mobile Application Development" },
                    { 20272, 47, "4CSE427", true, false, "Computer Graphics" },
                    { 20273, 47, "4CSE437", true, false, "Big Data Analytics" },
                    { 20274, 47, "4CSE447", true, false, "Embedded Systems" },
                    { 20275, 47, "4CSE457", true, true, "Cloud Lab" },
                    { 20276, 47, "4CSE467", true, true, "Mobile Development Lab" },
                    { 20277, 47, "4CSE477", true, true, "Graphics Lab" },
                    { 20278, 47, "4CSE487", true, true, "Embedded Systems Lab" },
                    { 20279, 48, "4CSE408", true, false, "Advanced Algorithms" },
                    { 20280, 48, "4CSE418", true, false, "Natural Language Processing" },
                    { 20281, 48, "4CSE428", true, false, "Deep Learning" },
                    { 20282, 48, "4CSE438", true, false, "Distributed Artificial Intelligence" },
                    { 20283, 48, "4CSE448", true, false, "Project Management" },
                    { 20284, 48, "4CSE458", true, true, "NLP Lab" },
                    { 20285, 48, "4CSE468", true, true, "Deep Learning Lab" },
                    { 20286, 48, "4CSE478", true, true, "Research Lab" },
                    { 20287, 48, "4CSE488", true, true, "Capstone Design Lab" },
                    { 20288, 51, "5MAT101", true, false, "Calculus I" },
                    { 20289, 51, "5PHY101", true, false, "Physics I" },
                    { 20290, 51, "5CSE101", true, false, "Programming Fundamentals" },
                    { 20291, 51, "5EEE101", true, false, "Electrical Circuits" },
                    { 20292, 51, "5ENG101", true, false, "Academic Writing" },
                    { 20293, 51, "5CSE111", true, true, "Programming Lab" },
                    { 20294, 51, "5PHY111", true, true, "Physics Lab" },
                    { 20295, 51, "5EEE111", true, true, "Circuit Lab" },
                    { 20296, 51, "5ENG111", true, true, "Engineering Graphics Lab" },
                    { 20297, 52, "5MAT102", true, false, "Calculus II" },
                    { 20298, 52, "5CSE102", true, false, "Object Oriented Programming" },
                    { 20299, 52, "5EEE102", true, false, "Digital Logic" },
                    { 20300, 52, "5STA102", true, false, "Probability and Statistics" },
                    { 20301, 52, "5HUM102", true, false, "Professional Ethics" },
                    { 20302, 52, "5CSE112", true, true, "OOP Lab" },
                    { 20303, 52, "5EEE112", true, true, "Digital Logic Lab" },
                    { 20304, 52, "5MAT112", true, true, "Numerical Methods Lab" },
                    { 20305, 52, "5CSE122", true, true, "Technical Skills Lab" },
                    { 20306, 53, "5MAT203", true, false, "Linear Algebra" },
                    { 20307, 53, "5CSE203", true, false, "Data Structures" },
                    { 20308, 53, "5CSE213", true, false, "Computer Organization" },
                    { 20309, 53, "5EEE203", true, false, "Signals and Systems" },
                    { 20310, 53, "5ECO203", true, false, "Engineering Economics" },
                    { 20311, 53, "5CSE223", true, true, "Data Structures Lab" },
                    { 20312, 53, "5CSE233", true, true, "Computer Organization Lab" },
                    { 20313, 53, "5EEE223", true, true, "Signals Lab" },
                    { 20314, 53, "5CSE243", true, true, "Web Development Lab" },
                    { 20315, 54, "5CSE204", true, false, "Algorithms" },
                    { 20316, 54, "5CSE214", true, false, "Database Systems" },
                    { 20317, 54, "5CSE224", true, false, "Operating Systems" },
                    { 20318, 54, "5CSE234", true, false, "Probability for Computing" },
                    { 20319, 54, "5BUS204", true, false, "Management Principles" },
                    { 20320, 54, "5CSE224L", true, true, "Algorithms Lab" },
                    { 20321, 54, "5CSE234L", true, true, "Database Lab" },
                    { 20322, 54, "5CSE244L", true, true, "Operating Systems Lab" },
                    { 20323, 54, "5CSE254L", true, true, "Systems Programming Lab" },
                    { 20324, 55, "5CSE305", true, false, "Computer Networks" },
                    { 20325, 55, "5CSE315", true, false, "Software Engineering" },
                    { 20326, 55, "5CSE325", true, false, "Theory of Computation" },
                    { 20327, 55, "5CSE335", true, false, "Compiler Design" },
                    { 20328, 55, "5CSE345", true, false, "Human Computer Interaction" },
                    { 20329, 55, "5CSE355", true, true, "Networks Lab" },
                    { 20330, 55, "5CSE365", true, true, "Software Engineering Lab" },
                    { 20331, 55, "5CSE375", true, true, "Compiler Lab" },
                    { 20332, 55, "5CSE385", true, true, "UI Design Lab" },
                    { 20333, 56, "5CSE306", true, false, "Artificial Intelligence" },
                    { 20334, 56, "5CSE316", true, false, "Machine Learning" },
                    { 20335, 56, "5CSE326", true, false, "Distributed Systems" },
                    { 20336, 56, "5CSE336", true, false, "Information Security" },
                    { 20337, 56, "5CSE346", true, false, "Data Mining" },
                    { 20338, 56, "5CSE356", true, true, "AI Lab" },
                    { 20339, 56, "5CSE366", true, true, "Machine Learning Lab" },
                    { 20340, 56, "5CSE376", true, true, "Security Lab" },
                    { 20341, 56, "5CSE386", true, true, "Data Mining Lab" },
                    { 20342, 57, "5CSE407", true, false, "Cloud Computing" },
                    { 20343, 57, "5CSE417", true, false, "Mobile Application Development" },
                    { 20344, 57, "5CSE427", true, false, "Computer Graphics" },
                    { 20345, 57, "5CSE437", true, false, "Big Data Analytics" },
                    { 20346, 57, "5CSE447", true, false, "Embedded Systems" },
                    { 20347, 57, "5CSE457", true, true, "Cloud Lab" },
                    { 20348, 57, "5CSE467", true, true, "Mobile Development Lab" },
                    { 20349, 57, "5CSE477", true, true, "Graphics Lab" },
                    { 20350, 57, "5CSE487", true, true, "Embedded Systems Lab" },
                    { 20351, 58, "5CSE408", true, false, "Advanced Algorithms" },
                    { 20352, 58, "5CSE418", true, false, "Natural Language Processing" },
                    { 20353, 58, "5CSE428", true, false, "Deep Learning" },
                    { 20354, 58, "5CSE438", true, false, "Distributed Artificial Intelligence" },
                    { 20355, 58, "5CSE448", true, false, "Project Management" },
                    { 20356, 58, "5CSE458", true, true, "NLP Lab" },
                    { 20357, 58, "5CSE468", true, true, "Deep Learning Lab" },
                    { 20358, 58, "5CSE478", true, true, "Research Lab" },
                    { 20359, 58, "5CSE488", true, true, "Capstone Design Lab" },
                    { 20360, 61, "6MAT101", true, false, "Calculus I" },
                    { 20361, 61, "6PHY101", true, false, "Physics I" },
                    { 20362, 61, "6CSE101", true, false, "Programming Fundamentals" },
                    { 20363, 61, "6EEE101", true, false, "Electrical Circuits" },
                    { 20364, 61, "6ENG101", true, false, "Academic Writing" },
                    { 20365, 61, "6CSE111", true, true, "Programming Lab" },
                    { 20366, 61, "6PHY111", true, true, "Physics Lab" },
                    { 20367, 61, "6EEE111", true, true, "Circuit Lab" },
                    { 20368, 61, "6ENG111", true, true, "Engineering Graphics Lab" },
                    { 20369, 62, "6MAT102", true, false, "Calculus II" },
                    { 20370, 62, "6CSE102", true, false, "Object Oriented Programming" },
                    { 20371, 62, "6EEE102", true, false, "Digital Logic" },
                    { 20372, 62, "6STA102", true, false, "Probability and Statistics" },
                    { 20373, 62, "6HUM102", true, false, "Professional Ethics" },
                    { 20374, 62, "6CSE112", true, true, "OOP Lab" },
                    { 20375, 62, "6EEE112", true, true, "Digital Logic Lab" },
                    { 20376, 62, "6MAT112", true, true, "Numerical Methods Lab" },
                    { 20377, 62, "6CSE122", true, true, "Technical Skills Lab" },
                    { 20378, 63, "6MAT203", true, false, "Linear Algebra" },
                    { 20379, 63, "6CSE203", true, false, "Data Structures" },
                    { 20380, 63, "6CSE213", true, false, "Computer Organization" },
                    { 20381, 63, "6EEE203", true, false, "Signals and Systems" },
                    { 20382, 63, "6ECO203", true, false, "Engineering Economics" },
                    { 20383, 63, "6CSE223", true, true, "Data Structures Lab" },
                    { 20384, 63, "6CSE233", true, true, "Computer Organization Lab" },
                    { 20385, 63, "6EEE223", true, true, "Signals Lab" },
                    { 20386, 63, "6CSE243", true, true, "Web Development Lab" },
                    { 20387, 64, "6CSE204", true, false, "Algorithms" },
                    { 20388, 64, "6CSE214", true, false, "Database Systems" },
                    { 20389, 64, "6CSE224", true, false, "Operating Systems" },
                    { 20390, 64, "6CSE234", true, false, "Probability for Computing" },
                    { 20391, 64, "6BUS204", true, false, "Management Principles" },
                    { 20392, 64, "6CSE224L", true, true, "Algorithms Lab" },
                    { 20393, 64, "6CSE234L", true, true, "Database Lab" },
                    { 20394, 64, "6CSE244L", true, true, "Operating Systems Lab" },
                    { 20395, 64, "6CSE254L", true, true, "Systems Programming Lab" },
                    { 20396, 65, "6CSE305", true, false, "Computer Networks" },
                    { 20397, 65, "6CSE315", true, false, "Software Engineering" },
                    { 20398, 65, "6CSE325", true, false, "Theory of Computation" },
                    { 20399, 65, "6CSE335", true, false, "Compiler Design" },
                    { 20400, 65, "6CSE345", true, false, "Human Computer Interaction" },
                    { 20401, 65, "6CSE355", true, true, "Networks Lab" },
                    { 20402, 65, "6CSE365", true, true, "Software Engineering Lab" },
                    { 20403, 65, "6CSE375", true, true, "Compiler Lab" },
                    { 20404, 65, "6CSE385", true, true, "UI Design Lab" },
                    { 20405, 66, "6CSE306", true, false, "Artificial Intelligence" },
                    { 20406, 66, "6CSE316", true, false, "Machine Learning" },
                    { 20407, 66, "6CSE326", true, false, "Distributed Systems" },
                    { 20408, 66, "6CSE336", true, false, "Information Security" },
                    { 20409, 66, "6CSE346", true, false, "Data Mining" },
                    { 20410, 66, "6CSE356", true, true, "AI Lab" },
                    { 20411, 66, "6CSE366", true, true, "Machine Learning Lab" },
                    { 20412, 66, "6CSE376", true, true, "Security Lab" },
                    { 20413, 66, "6CSE386", true, true, "Data Mining Lab" },
                    { 20414, 67, "6CSE407", true, false, "Cloud Computing" },
                    { 20415, 67, "6CSE417", true, false, "Mobile Application Development" },
                    { 20416, 67, "6CSE427", true, false, "Computer Graphics" },
                    { 20417, 67, "6CSE437", true, false, "Big Data Analytics" },
                    { 20418, 67, "6CSE447", true, false, "Embedded Systems" },
                    { 20419, 67, "6CSE457", true, true, "Cloud Lab" },
                    { 20420, 67, "6CSE467", true, true, "Mobile Development Lab" },
                    { 20421, 67, "6CSE477", true, true, "Graphics Lab" },
                    { 20422, 67, "6CSE487", true, true, "Embedded Systems Lab" },
                    { 20423, 68, "6CSE408", true, false, "Advanced Algorithms" },
                    { 20424, 68, "6CSE418", true, false, "Natural Language Processing" },
                    { 20425, 68, "6CSE428", true, false, "Deep Learning" },
                    { 20426, 68, "6CSE438", true, false, "Distributed Artificial Intelligence" },
                    { 20427, 68, "6CSE448", true, false, "Project Management" },
                    { 20428, 68, "6CSE458", true, true, "NLP Lab" },
                    { 20429, 68, "6CSE468", true, true, "Deep Learning Lab" },
                    { 20430, 68, "6CSE478", true, true, "Research Lab" },
                    { 20431, 68, "6CSE488", true, true, "Capstone Design Lab" },
                    { 20432, 71, "7MAT101", true, false, "Calculus I" },
                    { 20433, 71, "7PHY101", true, false, "Physics I" },
                    { 20434, 71, "7CSE101", true, false, "Programming Fundamentals" },
                    { 20435, 71, "7EEE101", true, false, "Electrical Circuits" },
                    { 20436, 71, "7ENG101", true, false, "Academic Writing" },
                    { 20437, 71, "7CSE111", true, true, "Programming Lab" },
                    { 20438, 71, "7PHY111", true, true, "Physics Lab" },
                    { 20439, 71, "7EEE111", true, true, "Circuit Lab" },
                    { 20440, 71, "7ENG111", true, true, "Engineering Graphics Lab" },
                    { 20441, 72, "7MAT102", true, false, "Calculus II" },
                    { 20442, 72, "7CSE102", true, false, "Object Oriented Programming" },
                    { 20443, 72, "7EEE102", true, false, "Digital Logic" },
                    { 20444, 72, "7STA102", true, false, "Probability and Statistics" },
                    { 20445, 72, "7HUM102", true, false, "Professional Ethics" },
                    { 20446, 72, "7CSE112", true, true, "OOP Lab" },
                    { 20447, 72, "7EEE112", true, true, "Digital Logic Lab" },
                    { 20448, 72, "7MAT112", true, true, "Numerical Methods Lab" },
                    { 20449, 72, "7CSE122", true, true, "Technical Skills Lab" },
                    { 20450, 73, "7MAT203", true, false, "Linear Algebra" },
                    { 20451, 73, "7CSE203", true, false, "Data Structures" },
                    { 20452, 73, "7CSE213", true, false, "Computer Organization" },
                    { 20453, 73, "7EEE203", true, false, "Signals and Systems" },
                    { 20454, 73, "7ECO203", true, false, "Engineering Economics" },
                    { 20455, 73, "7CSE223", true, true, "Data Structures Lab" },
                    { 20456, 73, "7CSE233", true, true, "Computer Organization Lab" },
                    { 20457, 73, "7EEE223", true, true, "Signals Lab" },
                    { 20458, 73, "7CSE243", true, true, "Web Development Lab" },
                    { 20459, 74, "7CSE204", true, false, "Algorithms" },
                    { 20460, 74, "7CSE214", true, false, "Database Systems" },
                    { 20461, 74, "7CSE224", true, false, "Operating Systems" },
                    { 20462, 74, "7CSE234", true, false, "Probability for Computing" },
                    { 20463, 74, "7BUS204", true, false, "Management Principles" },
                    { 20464, 74, "7CSE224L", true, true, "Algorithms Lab" },
                    { 20465, 74, "7CSE234L", true, true, "Database Lab" },
                    { 20466, 74, "7CSE244L", true, true, "Operating Systems Lab" },
                    { 20467, 74, "7CSE254L", true, true, "Systems Programming Lab" },
                    { 20468, 75, "7CSE305", true, false, "Computer Networks" },
                    { 20469, 75, "7CSE315", true, false, "Software Engineering" },
                    { 20470, 75, "7CSE325", true, false, "Theory of Computation" },
                    { 20471, 75, "7CSE335", true, false, "Compiler Design" },
                    { 20472, 75, "7CSE345", true, false, "Human Computer Interaction" },
                    { 20473, 75, "7CSE355", true, true, "Networks Lab" },
                    { 20474, 75, "7CSE365", true, true, "Software Engineering Lab" },
                    { 20475, 75, "7CSE375", true, true, "Compiler Lab" },
                    { 20476, 75, "7CSE385", true, true, "UI Design Lab" },
                    { 20477, 76, "7CSE306", true, false, "Artificial Intelligence" },
                    { 20478, 76, "7CSE316", true, false, "Machine Learning" },
                    { 20479, 76, "7CSE326", true, false, "Distributed Systems" },
                    { 20480, 76, "7CSE336", true, false, "Information Security" },
                    { 20481, 76, "7CSE346", true, false, "Data Mining" },
                    { 20482, 76, "7CSE356", true, true, "AI Lab" },
                    { 20483, 76, "7CSE366", true, true, "Machine Learning Lab" },
                    { 20484, 76, "7CSE376", true, true, "Security Lab" },
                    { 20485, 76, "7CSE386", true, true, "Data Mining Lab" },
                    { 20486, 77, "7CSE407", true, false, "Cloud Computing" },
                    { 20487, 77, "7CSE417", true, false, "Mobile Application Development" },
                    { 20488, 77, "7CSE427", true, false, "Computer Graphics" },
                    { 20489, 77, "7CSE437", true, false, "Big Data Analytics" },
                    { 20490, 77, "7CSE447", true, false, "Embedded Systems" },
                    { 20491, 77, "7CSE457", true, true, "Cloud Lab" },
                    { 20492, 77, "7CSE467", true, true, "Mobile Development Lab" },
                    { 20493, 77, "7CSE477", true, true, "Graphics Lab" },
                    { 20494, 77, "7CSE487", true, true, "Embedded Systems Lab" },
                    { 20495, 78, "7CSE408", true, false, "Advanced Algorithms" },
                    { 20496, 78, "7CSE418", true, false, "Natural Language Processing" },
                    { 20497, 78, "7CSE428", true, false, "Deep Learning" },
                    { 20498, 78, "7CSE438", true, false, "Distributed Artificial Intelligence" },
                    { 20499, 78, "7CSE448", true, false, "Project Management" },
                    { 20500, 78, "7CSE458", true, true, "NLP Lab" },
                    { 20501, 78, "7CSE468", true, true, "Deep Learning Lab" },
                    { 20502, 78, "7CSE478", true, true, "Research Lab" },
                    { 20503, 78, "7CSE488", true, true, "Capstone Design Lab" },
                    { 20504, 81, "8MAT101", true, false, "Calculus I" },
                    { 20505, 81, "8PHY101", true, false, "Physics I" },
                    { 20506, 81, "8CSE101", true, false, "Programming Fundamentals" },
                    { 20507, 81, "8EEE101", true, false, "Electrical Circuits" },
                    { 20508, 81, "8ENG101", true, false, "Academic Writing" },
                    { 20509, 81, "8CSE111", true, true, "Programming Lab" },
                    { 20510, 81, "8PHY111", true, true, "Physics Lab" },
                    { 20511, 81, "8EEE111", true, true, "Circuit Lab" },
                    { 20512, 81, "8ENG111", true, true, "Engineering Graphics Lab" },
                    { 20513, 82, "8MAT102", true, false, "Calculus II" },
                    { 20514, 82, "8CSE102", true, false, "Object Oriented Programming" },
                    { 20515, 82, "8EEE102", true, false, "Digital Logic" },
                    { 20516, 82, "8STA102", true, false, "Probability and Statistics" },
                    { 20517, 82, "8HUM102", true, false, "Professional Ethics" },
                    { 20518, 82, "8CSE112", true, true, "OOP Lab" },
                    { 20519, 82, "8EEE112", true, true, "Digital Logic Lab" },
                    { 20520, 82, "8MAT112", true, true, "Numerical Methods Lab" },
                    { 20521, 82, "8CSE122", true, true, "Technical Skills Lab" },
                    { 20522, 83, "8MAT203", true, false, "Linear Algebra" },
                    { 20523, 83, "8CSE203", true, false, "Data Structures" },
                    { 20524, 83, "8CSE213", true, false, "Computer Organization" },
                    { 20525, 83, "8EEE203", true, false, "Signals and Systems" },
                    { 20526, 83, "8ECO203", true, false, "Engineering Economics" },
                    { 20527, 83, "8CSE223", true, true, "Data Structures Lab" },
                    { 20528, 83, "8CSE233", true, true, "Computer Organization Lab" },
                    { 20529, 83, "8EEE223", true, true, "Signals Lab" },
                    { 20530, 83, "8CSE243", true, true, "Web Development Lab" },
                    { 20531, 84, "8CSE204", true, false, "Algorithms" },
                    { 20532, 84, "8CSE214", true, false, "Database Systems" },
                    { 20533, 84, "8CSE224", true, false, "Operating Systems" },
                    { 20534, 84, "8CSE234", true, false, "Probability for Computing" },
                    { 20535, 84, "8BUS204", true, false, "Management Principles" },
                    { 20536, 84, "8CSE224L", true, true, "Algorithms Lab" },
                    { 20537, 84, "8CSE234L", true, true, "Database Lab" },
                    { 20538, 84, "8CSE244L", true, true, "Operating Systems Lab" },
                    { 20539, 84, "8CSE254L", true, true, "Systems Programming Lab" },
                    { 20540, 85, "8CSE305", true, false, "Computer Networks" },
                    { 20541, 85, "8CSE315", true, false, "Software Engineering" },
                    { 20542, 85, "8CSE325", true, false, "Theory of Computation" },
                    { 20543, 85, "8CSE335", true, false, "Compiler Design" },
                    { 20544, 85, "8CSE345", true, false, "Human Computer Interaction" },
                    { 20545, 85, "8CSE355", true, true, "Networks Lab" },
                    { 20546, 85, "8CSE365", true, true, "Software Engineering Lab" },
                    { 20547, 85, "8CSE375", true, true, "Compiler Lab" },
                    { 20548, 85, "8CSE385", true, true, "UI Design Lab" },
                    { 20549, 86, "8CSE306", true, false, "Artificial Intelligence" },
                    { 20550, 86, "8CSE316", true, false, "Machine Learning" },
                    { 20551, 86, "8CSE326", true, false, "Distributed Systems" },
                    { 20552, 86, "8CSE336", true, false, "Information Security" },
                    { 20553, 86, "8CSE346", true, false, "Data Mining" },
                    { 20554, 86, "8CSE356", true, true, "AI Lab" },
                    { 20555, 86, "8CSE366", true, true, "Machine Learning Lab" },
                    { 20556, 86, "8CSE376", true, true, "Security Lab" },
                    { 20557, 86, "8CSE386", true, true, "Data Mining Lab" },
                    { 20558, 87, "8CSE407", true, false, "Cloud Computing" },
                    { 20559, 87, "8CSE417", true, false, "Mobile Application Development" },
                    { 20560, 87, "8CSE427", true, false, "Computer Graphics" },
                    { 20561, 87, "8CSE437", true, false, "Big Data Analytics" },
                    { 20562, 87, "8CSE447", true, false, "Embedded Systems" },
                    { 20563, 87, "8CSE457", true, true, "Cloud Lab" },
                    { 20564, 87, "8CSE467", true, true, "Mobile Development Lab" },
                    { 20565, 87, "8CSE477", true, true, "Graphics Lab" },
                    { 20566, 87, "8CSE487", true, true, "Embedded Systems Lab" },
                    { 20567, 88, "8CSE408", true, false, "Advanced Algorithms" },
                    { 20568, 88, "8CSE418", true, false, "Natural Language Processing" },
                    { 20569, 88, "8CSE428", true, false, "Deep Learning" },
                    { 20570, 88, "8CSE438", true, false, "Distributed Artificial Intelligence" },
                    { 20571, 88, "8CSE448", true, false, "Project Management" },
                    { 20572, 88, "8CSE458", true, true, "NLP Lab" },
                    { 20573, 88, "8CSE468", true, true, "Deep Learning Lab" },
                    { 20574, 88, "8CSE478", true, true, "Research Lab" },
                    { 20575, 88, "8CSE488", true, true, "Capstone Design Lab" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20000);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20001);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20002);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20003);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20004);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20005);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20006);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20007);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20008);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20009);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20010);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20011);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20012);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20013);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20014);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20015);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20016);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20017);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20018);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20019);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20020);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20021);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20022);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20023);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20024);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20025);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20026);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20027);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20028);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20029);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20030);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20031);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20032);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20033);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20034);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20035);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20036);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20037);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20038);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20039);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20040);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20041);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20042);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20043);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20044);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20045);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20046);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20047);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20048);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20049);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20050);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20051);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20052);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20053);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20054);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20055);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20056);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20057);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20058);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20059);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20060);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20061);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20062);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20063);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20064);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20065);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20066);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20067);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20068);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20069);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20070);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20071);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20072);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20073);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20074);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20075);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20076);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20077);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20078);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20079);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20080);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20081);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20082);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20083);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20084);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20085);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20086);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20087);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20088);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20089);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20090);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20091);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20092);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20093);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20094);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20095);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20096);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20097);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20098);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20099);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20100);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20101);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20102);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20103);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20104);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20105);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20106);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20107);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20108);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20109);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20110);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20111);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20112);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20113);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20114);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20115);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20116);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20117);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20118);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20119);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20120);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20121);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20122);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20123);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20124);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20125);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20126);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20127);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20128);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20129);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20130);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20131);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20132);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20133);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20134);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20135);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20136);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20137);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20138);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20139);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20140);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20141);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20142);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20143);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20144);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20145);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20146);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20147);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20148);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20149);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20150);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20151);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20152);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20153);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20154);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20155);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20156);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20157);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20158);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20159);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20160);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20161);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20162);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20163);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20164);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20165);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20166);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20167);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20168);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20169);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20170);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20171);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20172);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20173);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20174);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20175);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20176);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20177);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20178);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20179);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20180);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20181);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20182);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20183);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20184);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20185);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20186);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20187);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20188);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20189);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20190);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20191);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20192);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20193);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20194);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20195);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20196);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20197);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20198);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20199);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20200);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20201);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20202);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20203);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20204);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20205);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20206);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20207);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20208);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20209);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20210);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20211);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20212);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20213);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20214);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20215);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20216);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20217);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20218);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20219);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20220);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20221);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20222);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20223);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20224);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20225);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20226);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20227);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20228);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20229);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20230);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20231);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20232);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20233);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20234);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20235);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20236);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20237);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20238);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20239);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20240);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20241);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20242);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20243);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20244);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20245);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20246);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20247);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20248);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20249);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20250);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20251);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20252);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20253);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20254);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20255);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20256);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20257);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20258);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20259);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20260);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20261);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20262);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20263);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20264);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20265);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20266);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20267);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20268);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20269);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20270);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20271);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20272);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20273);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20274);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20275);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20276);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20277);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20278);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20279);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20280);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20281);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20282);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20283);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20284);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20285);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20286);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20287);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20288);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20289);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20290);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20291);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20292);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20293);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20294);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20295);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20296);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20297);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20298);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20299);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20300);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20301);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20302);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20303);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20304);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20305);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20306);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20307);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20308);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20309);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20310);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20311);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20312);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20313);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20314);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20315);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20316);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20317);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20318);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20319);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20320);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20321);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20322);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20323);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20324);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20325);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20326);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20327);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20328);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20329);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20330);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20331);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20332);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20333);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20334);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20335);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20336);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20337);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20338);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20339);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20340);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20341);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20342);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20343);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20344);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20345);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20346);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20347);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20348);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20349);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20350);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20351);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20352);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20353);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20354);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20355);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20356);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20357);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20358);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20359);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20360);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20361);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20362);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20363);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20364);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20365);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20366);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20367);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20368);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20369);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20370);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20371);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20372);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20373);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20374);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20375);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20376);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20377);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20378);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20379);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20380);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20381);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20382);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20383);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20384);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20385);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20386);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20387);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20388);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20389);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20390);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20391);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20392);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20393);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20394);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20395);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20396);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20397);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20398);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20399);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20400);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20401);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20402);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20403);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20404);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20405);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20406);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20407);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20408);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20409);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20410);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20411);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20412);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20413);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20414);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20415);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20416);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20417);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20418);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20419);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20420);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20421);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20422);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20423);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20424);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20425);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20426);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20427);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20428);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20429);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20430);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20431);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20432);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20433);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20434);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20435);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20436);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20437);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20438);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20439);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20440);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20441);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20442);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20443);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20444);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20445);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20446);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20447);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20448);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20449);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20450);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20451);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20452);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20453);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20454);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20455);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20456);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20457);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20458);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20459);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20460);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20461);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20462);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20463);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20464);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20465);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20466);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20467);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20468);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20469);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20470);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20471);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20472);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20473);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20474);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20475);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20476);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20477);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20478);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20479);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20480);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20481);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20482);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20483);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20484);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20485);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20486);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20487);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20488);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20489);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20490);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20491);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20492);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20493);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20494);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20495);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20496);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20497);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20498);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20499);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20500);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20501);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20502);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20503);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20504);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20505);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20506);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20507);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20508);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20509);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20510);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20511);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20512);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20513);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20514);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20515);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20516);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20517);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20518);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20519);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20520);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20521);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20522);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20523);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20524);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20525);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20526);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20527);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20528);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20529);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20530);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20531);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20532);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20533);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20534);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20535);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20536);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20537);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20538);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20539);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20540);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20541);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20542);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20543);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20544);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20545);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20546);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20547);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20548);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20549);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20550);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20551);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20552);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20553);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20554);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20555);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20556);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20557);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20558);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20559);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20560);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20561);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20562);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20563);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20564);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20565);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20566);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20567);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20568);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20569);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20570);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20571);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20572);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20573);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20574);

            migrationBuilder.DeleteData(
                table: "AcademicCourses",
                keyColumn: "Id",
                keyValue: 20575);
        }
    }
}
