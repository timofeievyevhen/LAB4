using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace LabTest
{
    [TestClass]
    public sealed class Test1
    {

        private string CreateTempFile(string content)
        {
            string path = Path.GetTempFileName();
            File.WriteAllText(path, content, Encoding.UTF8);
            return path;
        }

        private Lab4_2.Student MakeStudent(string math, string physics, string inform, string lastName = "Тест")
            => new Lab4_2.Student
            {
                LastName = lastName,
                FirstName = "Ім'я",
                Patronymic = "По батькові",
                Gender = 'M',
                BirthDate = "01.01.2000",
                MathGrade = math,
                PhysicsGrade = physics,
                InformGrade = inform,
                Scholarship = 0
            };

        public TestContext TestContext { get; set; }

        private string CaptureVariant11(List<Lab4_2.Student> students)
        {
            var sb = new StringBuilder();
            var writer = new StringWriter(sb);
            var old = System.Console.Out;
            System.Console.SetOut(writer);
            try
            {
                Lab4_2.Program.Variant11(students);
            }
            finally
            {
                writer.Flush();
                System.Console.SetOut(old);
            }
            string result = sb.ToString();
            TestContext?.WriteLine("=== captured output ===\n" + result);
            return result;
        }

        [TestMethod]
        public void ParseGrade_Dash_Returns2()
        {
            double result = Lab4_2.Program.ParseGradeForProcessing("-");
            Assert.AreEqual(2.0, result);
        }

        [TestMethod]
        public void ParseGrade_ValidInteger_ReturnsParsedValue()
        {
            double result = Lab4_2.Program.ParseGradeForProcessing("5");
            Assert.AreEqual(5.0, result);
        }

        [TestMethod]
        public void ParseGrade_InvalidString_Returns2()
        {
            double result = Lab4_2.Program.ParseGradeForProcessing("abc");
            Assert.AreEqual(2.0, result);
        }

        [TestMethod]
        public void ParseGrade_EmptyString_Returns2()
        {
            double result = Lab4_2.Program.ParseGradeForProcessing("");
            Assert.AreEqual(2.0, result);
        }

        [TestMethod]
        public void ParseGrade_Zero_ReturnsZero()
        {
            double result = Lab4_2.Program.ParseGradeForProcessing("0");
            Assert.AreEqual(0.0, result);
        }

        [TestMethod]
        public void ReadStudents_ValidLine_ParsesAllFields()
        {
            string path = CreateTempFile("Aaa Bbb Ccc M 01.02.2003 5 - 4 0\n");
            try
            {
                List<Lab4_2.Student> students = Lab4_2.Program.ReadStudentsFromFile(path);

                Assert.AreEqual(1, students.Count);
                Lab4_2.Student s = students[0];
                Assert.AreEqual("Aaa", s.LastName);
                Assert.AreEqual("Bbb", s.FirstName);
                Assert.AreEqual("Ccc", s.Patronymic);
                Assert.AreEqual('M', s.Gender);
                Assert.AreEqual("01.02.2003", s.BirthDate);
                Assert.AreEqual("5", s.MathGrade);
                Assert.AreEqual("-", s.PhysicsGrade);
                Assert.AreEqual("4", s.InformGrade);
                Assert.AreEqual(0, s.Scholarship);
            }
            finally { File.Delete(path); }
        }

        [TestMethod]
        public void ReadStudents_MultipleLines_ReturnsAll()
        {
            string content =
                "Aaa Bbb Ccc M 01.02.2003 5 - 4 0\n" +
                "Z X Y F 08.03.1999 5 5 2 0\n";
            string path = CreateTempFile(content);
            try
            {
                List<Lab4_2.Student> students = Lab4_2.Program.ReadStudentsFromFile(path);
                Assert.AreEqual(2, students.Count);
            }
            finally { File.Delete(path); }
        }

        [TestMethod]
        public void ReadStudents_ShortLine_IsSkipped()
        {
            string path = CreateTempFile("Aaa Bbb Ccc M 01.02.2003\n");
            try
            {
                List<Lab4_2.Student> students = Lab4_2.Program.ReadStudentsFromFile(path);
                Assert.AreEqual(0, students.Count);
            }
            finally { File.Delete(path); }
        }

        [TestMethod]
        public void ReadStudents_EmptyFile_ReturnsEmptyList()
        {
            string path = CreateTempFile("");
            try
            {
                List<Lab4_2.Student> students = Lab4_2.Program.ReadStudentsFromFile(path);
                Assert.AreEqual(0, students.Count);
            }
            finally { File.Delete(path); }
        }

        [TestMethod]
        public void ReadStudents_TabSeparatedLine_ParsedCorrectly()
        {
            string path = CreateTempFile("Qw\tEr\tTy\tM\t31.12.2005\t4\t4\t4\t444\n");
            try
            {
                List<Lab4_2.Student> students = Lab4_2.Program.ReadStudentsFromFile(path);
                Assert.AreEqual(1, students.Count);
                Assert.AreEqual(444, students[0].Scholarship);
            }
            finally { File.Delete(path); }
        }

        [TestMethod]
        public void ReadStudents_UnicodeLastName_ParsedCorrectly()
        {
            string path = CreateTempFile(
                "Переплигнѣгопченко Каленикъ Ѳеофанович M 05.05.1555 5 5 4 0\n");
            try
            {
                List<Lab4_2.Student> students = Lab4_2.Program.ReadStudentsFromFile(path);
                Assert.AreEqual(1, students.Count);
                Assert.AreEqual("Переплигнѣгопченко", students[0].LastName);
            }
            finally { File.Delete(path); }
        }


        [TestMethod]
        public void Variant11_StudentAbove4_5_IsListed()
        {
            var students = new List<Lab4_2.Student> { MakeStudent("5", "5", "5", "Відмінник") };
            string output = CaptureVariant11(students);
            StringAssert.Contains(output, "Відмінник");
        }

        [TestMethod]
        public void Variant11_StudentBelow4_5_IsNotListed()
        {
            var students = new List<Lab4_2.Student> { MakeStudent("3", "3", "3", "Трієчник") };
            string output = CaptureVariant11(students);
            Assert.IsFalse(output.Contains("Трієчник"));
        }

        [TestMethod]
        public void Variant11_DashGrade_TreatedAs2_BringsAverageDown()
        {
            var students = new List<Lab4_2.Student> { MakeStudent("5", "-", "5", "Прочерк") };
            string output = CaptureVariant11(students);
            StringAssert.Contains(output, "не знайдено");
        }


        [TestMethod]
        public void Variant11_MixedStudents_OnlyHighAverageShown()
        {
            var good = MakeStudent("5", "5", "4", "Відмінник");
            var bad = MakeStudent("3", "3", "3", "Трієчник");

            string output = CaptureVariant11(new List<Lab4_2.Student> { good, bad });
            StringAssert.Contains(output, "Відмінник");
            Assert.IsFalse(output.Contains("Трієчник"));
        }

        [TestMethod]
        public void Variant11_OutputContainsPhysicsGrade()
        {
            var s = MakeStudent("5", "5", "5", "Іванов");
            string output = CaptureVariant11(new List<Lab4_2.Student> { s });
            StringAssert.Contains(output, "5");
        }
    }
}
