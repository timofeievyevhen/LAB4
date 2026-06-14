using Microsoft.VisualStudio.TestPlatform.TestHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;
// Підключаємо простір імен вашого головного проєкту, щоб тест бачив клас Program
using StudentDataLab;

namespace TestProject1
{
    [TestClass]
    public class StudentTests
    {
        [TestMethod]
        public void Test_ParseGrade_NormalGrade_ReturnsSameNumber()
        {
            // Перевіряємо, чи звичайна текстова оцінка "5" правильно конвертується в число 5.0
            double result = Program.ParseGradeForProcessing("5");

            Assert.AreEqual(5.0, result);
        }

        [TestMethod]
        public void Test_ParseGrade_AbsentSign_ReturnsTwo()
        {
            // Головна фішка лаби: перевіряємо, чи неявка "-" перетворюється на 2.0 під час обробки
            double result = Program.ParseGradeForProcessing("-");

            Assert.AreEqual(2.0, result);
        }
    }
}