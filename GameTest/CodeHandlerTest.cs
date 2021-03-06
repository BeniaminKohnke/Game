using GameAPI;
using GameAPI.DSL;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GameTest
{
    [TestClass]
    public class CodeHandlerTest
    {
        [TestMethod]
        public void InvokePlayerScriptsTest()
        {
            var handler = new CodeHandler();
            var gameWorld = new GameWorld();
            handler.InvokePlayerScripts(gameWorld);
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
            handler.InvokePlayerScripts(gameWorld);
        }
    }
}