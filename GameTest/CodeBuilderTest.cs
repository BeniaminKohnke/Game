using Game.DSL;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GameTest
{
    [TestClass]
    public class CodeBuilderTest
    {
        [TestMethod]
        public void CompileToCSharpTest()
        {
            CodeBuilder.CompileToCSharp("TEST");
        }
    }
}