using GameAPI;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GameTest
{
    [TestClass]
    public class PositionTest
    {
        [TestMethod]
        public void GetDistanceTest()
        {
            var p1 = new Position(0, 0);
            var p2 = new Position(3, 4);
            var p3 = new Position(-3, -4);

            Assert.AreEqual(5f, p1.GetDistance(p2));
            Assert.AreEqual(5f, p1.GetDistance(p3));
        }
    }
}
