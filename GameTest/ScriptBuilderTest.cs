using GameAPI;
using GameAPI.DSL;
using GameAPI.GameObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Reflection;

namespace GameTest
{
    [TestClass]
    public sealed class ScriptBuilderTest
    {
        private readonly Func<string, string>? _prepareScriptToCompilation;
        private readonly Func<string, string>? _changeFunctionsToCSharpMethods;
        private readonly Func<string, string>? _translateKeywords;
        private readonly Func<string, string>? _changeTabsToBrackets;
        private readonly Func<string, string>? _addSemicolons;
        private readonly Func<string, string>? _translateVariables;
        private readonly Func<string, string>? _translateOtherWords;

        public ScriptBuilderTest()
        {
            var type = typeof(Func<string, string>);
            var methods = typeof(ScriptBuilder).GetMethods(BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.DeclaredOnly);
            _prepareScriptToCompilation = methods?.FirstOrDefault(m => m.Name
                .Equals("PrepareScriptToCompilation"))?
                .CreateDelegate(type) as Func<string, string>;
            _changeFunctionsToCSharpMethods = methods?.FirstOrDefault(m => m.Name
                .Equals("ChangeFunctionsToCSharpMethods"))?
                .CreateDelegate(type) as Func<string, string>;
            _translateKeywords = methods?.FirstOrDefault(m => m.Name
                .Equals("TranslateKeywords"))?
                .CreateDelegate(type) as Func<string, string>;
            _changeTabsToBrackets = methods?.FirstOrDefault(m => m.Name
                .Equals("ChangeTabsToBrackets"))?
                .CreateDelegate(type) as Func<string, string>;
            _addSemicolons = methods?.FirstOrDefault(m => m.Name
                .Equals("AddSemicolons"))?
                .CreateDelegate(type) as Func<string, string>;
            _translateVariables = methods?.FirstOrDefault(m => m.Name
                .Equals("TranslateVariables"))?
                .CreateDelegate(type) as Func<string, string>;
            _translateOtherWords = methods?.FirstOrDefault(m => m.Name
                .Equals("TranslateOtherWords"))?
                .CreateDelegate(type) as Func<string, string>;
        }

        [TestMethod]
        public void PrepareScriptToCompilationTest()
        {
            Assert.AreEqual("[None] SAVE_TO [myObject]", _prepareScriptToCompilation?.Invoke("[None] SAVE TO [myObject]"));
        }

        [TestMethod]
        public void ChangeFunctionsToCSharpMethodsTest()
        { 
            Assert.AreEqual("ScriptFunctions.DistanceBetween([Player],[myObject],gameWorld,parameters,deltaTime)", 
                _changeFunctionsToCSharpMethods?.Invoke("DistanceBetween [Player] [myObject]"));
        }

        [TestMethod]
        public void TranslateKeywordsTest()
        {
            Assert.AreEqual("foreach( var [item] in ([Items] as IEnumerable<object> ?? new[]{[Items]}) )",
                _translateKeywords?.Invoke("FOR_SINGLE [item] FROM [Items] DO"));
        }

        [TestMethod]
        public void ChangeTabsToBracketsTest()
        {
            Assert.AreEqual("foreach (var line in lines)\n{ }", _changeTabsToBrackets?.Invoke("foreach (var line in lines)\n\t"));
        }

        [TestMethod]
        public void AddSemicolonsTest()
        {
            var script = 
            @"foreach (var item in Items)
            {
                if (item is GameObject)
                {
                    storedItem = item
                }
            }".Replace("\r", string.Empty);
            var expected =
            @"foreach (var item in Items)
            {
                if (item is GameObject)
                {
                    storedItem = item;
                }
            }".Replace("\r", string.Empty);
            Assert.AreEqual(expected, _addSemicolons?.Invoke(script));
        }

        [TestMethod]
        public void TranslateVariablesTest()
        {
            Assert.AreEqual("foreach (var item in gameWorld.Player.Items)", _translateVariables?.Invoke("foreach (var [item] in [Items])"));
        }

        [TestMethod]
        public void TranslateOtherWordsTest()
        {
            Assert.AreEqual("ScriptBuilder.MoreThan(value,10)", _translateOtherWords?.Invoke("value MORE_THAN 10"));
        }

        [TestMethod]
        public void OperationsTest()
        {
            Assert.IsTrue(ScriptBuilder.Is(new Player(0, 0), Types.Player, false));
            Assert.IsTrue(ScriptBuilder.LessThan(-1, 10));
            Assert.IsTrue(ScriptBuilder.LessOrEqualThan(0, 0));
            Assert.IsTrue(ScriptBuilder.MoreThan(5, 0));
            Assert.IsTrue(ScriptBuilder.MoreOrEqualThan(10, -10));
        }
    }
}