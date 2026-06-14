using ConsoleApp8;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestProject
{
    [TestClass]
    public class LabTests
    {
        [TestMethod]
        public void Test_NormalGrade_ParsesCorrectly()
        {
            // Перевіряємо звичайну оцінку
            double result = Program.ParseGradeForProcessing("5");
            Assert.AreEqual(5.0, result);
        }

        [TestMethod]
        public void Test_AbsentGrade_ReturnsTwo()
        {
            // Перевіряємо вимогу лаби: неявка "-" має ставати двійкою
            double result = Program.ParseGradeForProcessing("-");
            Assert.AreEqual(2.0, result);
        }
    }
}
