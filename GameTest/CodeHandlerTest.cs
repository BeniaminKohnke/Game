using Game.DSL;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GameTest
{
    [TestClass]
    public class CodeHandlerTest
    {
        [TestMethod]
        public void CompileToCSharpTest()
        {
            CodeBuilder.CompileToCSharp("Test");
        }
    }
}