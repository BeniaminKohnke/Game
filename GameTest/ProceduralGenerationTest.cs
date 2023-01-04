using GameAPI;
using GameAPI.GameObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace GameTest
{
    [TestClass]
    public sealed class ProceduralGenerationTest
    {
        private readonly ProceduralGeneration _generator = new(0);

        [TestMethod]
        public void CreateObjectTest()
        {
            var gameObjects = new List<GameObject>();
            for (var i = 0; i < 100; i++)
            {
                for (var j = 0; j < 100; j++)
                {
                    var go = _generator.CreateObject(i, j);
                    if (go != null)
                    {
                        gameObjects.Add(go);
                    }
                }
            }

            Assert.IsTrue(gameObjects.Any());
        }

        [TestMethod]
        public void ProcedureTest()
        {
            var firstRun = new List<GameObject>();
            for (var i = 0; i < 100; i++)
            {
                for (var j = 0; j < 100; j++)
                {
                    var go = _generator.CreateObject(i, j);
                    if (go != null)
                    {
                        firstRun.Add(go);
                    }
                }
            }

            var secondRun = new List<GameObject>();
            for (var i = 0; i < 100; i++)
            {
                for (var j = 0; j < 100; j++)
                {
                    var go = _generator.CreateObject(i, j);
                    if (go != null)
                    {
                        secondRun.Add(go);
                    }
                }
            }

            Assert.AreEqual(firstRun.Count, secondRun.Count);
            for (var i = 0; i < firstRun.Count; i++)
            {
                Assert.AreEqual(firstRun[i].State, secondRun[i].State);
                Assert.AreEqual(firstRun[i].ObjectType, secondRun[i].ObjectType);
                Assert.AreEqual(firstRun[i].Grid, secondRun[i].Grid);
                Assert.AreEqual(firstRun[i].Position, secondRun[i].Position);
            }
        }
    }
}