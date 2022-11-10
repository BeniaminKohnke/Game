using GameAPI.DSL;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Reflection;

namespace GameTest
{
    [TestClass]
    public sealed class CodeBuilderTest
    {
        private readonly Func<string, string>? _prepareCodeToCompilation;
        private readonly Func<string, string>? _changeFunctionsToCSharpMethods;
        private readonly Func<string, string>? _changeTabsToBrackets;

        public CodeBuilderTest()
        {
            var methods = typeof(ScriptBuilder).GetMethods(BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.DeclaredOnly);
            _prepareCodeToCompilation = methods?.FirstOrDefault(m => m.Name
                .Equals("PrepareCodeToCompilation"))?
                .CreateDelegate(typeof(Func<string, string>)) as Func<string, string>;
            _changeFunctionsToCSharpMethods = methods?.FirstOrDefault(m => m.Name
                .Equals("ChangeFunctionsToCSharpMethods"))?
                .CreateDelegate(typeof(Func<string, string>)) as Func<string, string>;
            _changeTabsToBrackets = methods?.FirstOrDefault(m => m.Name
                .Equals("ChangeTabsToBrackets"))?
                .CreateDelegate(typeof(Func<string, string>)) as Func<string, string>;
        }

        [TestMethod]
        public void ChangeFunctionsToCSharpMethodsTest()
        { 
            var code = _prepareCodeToCompilation?.Invoke("DistanceBetween [Player] [myObject]");
            Assert.AreEqual("DistanceBetween([Player],[myObject],GameWorld gameWorld,Dictionary<string, object> parameters,float deltaTime)", 
                _changeFunctionsToCSharpMethods?.Invoke(code ?? string.Empty));
        }

        [TestMethod]
        public void ChangeTabsToBracketsTest()
        {
            var code = "FOR_EACH LINE IN LINES\n\t";
            Assert.AreEqual("FOR_EACH LINE IN LINES\n{}", _changeTabsToBrackets?.Invoke(code));
        }
    }
}