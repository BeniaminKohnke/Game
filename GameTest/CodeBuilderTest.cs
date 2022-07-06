using GameAPI.DSL;
using GameAPI;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GameTest
{
    [TestClass]
    public class CodeBuilderTest
    {
        [TestMethod]
        public void InvokePlayerScriptsTest()
        {
            var handler = new CodeHandler();
            var gameWorld = new GameWorld();
            var parameters = new Parameters();
            handler.InvokePlayerScripts(gameWorld, parameters);
        }

        [TestMethod]
        public void CompileScriptsTest()
        {
            var handler = new CodeHandler();
            handler.CompileScripts();
        }

        [TestMethod]
        public void LoadScriptsTest()
        {
            var handler = new CodeHandler();
            handler.LoadScripts();

            var gameWorld = new GameWorld();
            var parameters = new Parameters();
            handler.InvokePlayerScripts(gameWorld, parameters);
        }
    }
}