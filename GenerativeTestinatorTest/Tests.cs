using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GenerativeTestinatorTest
{
    [TestClass]
    public class Tests
    {
        [Generator(times: 100)]
        [DataRow(1, 1)] //need to have to make method able to receive parameters, thanks for this MsTest ¯\_(ツ)_/¯
        public void Should_sum_two_values(int x, int y)
        {
            Assert.AreEqual(x + y, Sum(x, y));
        }

        public int Sum(int x, int y)
        {
            if (x < 50000) return x + y;

            return 10; //¯\_(ツ)_/¯
        }
    }
}
