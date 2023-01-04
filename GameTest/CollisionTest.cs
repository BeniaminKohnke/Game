using GameAPI;
using GameAPI.GameObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GameTest
{
    [TestClass]
    public sealed class CollisionTest
    {
        private readonly GameObject _first = new Player(0, 0);
        private readonly GameObject _second = new Enemy(0,0, Grids.Enemy1);
        private readonly GameObject _third = new(500, 500, Types.Tree, Grids.Tree1);

        [TestMethod]
        public void CheckCollisionTest()
        {
            Assert.IsTrue(_first.CheckCollision(_second));
            Assert.IsFalse(_second.CheckCollision(_third));
        }
    }
}
