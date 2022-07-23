using BK_S.Calc.Core.ReversePolishNotation;
using Calc.WPF.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Tests
{
    [TestClass]
    public class ParseExpression_Test
    {
        [TestMethod]
        public void ParseExpression_CorrectExpression()
        {
            ParserRPN parser = new ParserRPN();
            string expression = "(0.5*5+5)/3";
            var tokens = new List<string>()
            {
                "0.5",
                "5",
                "*",
                "5",
                "+",
                "3",
                "/",
            };

            var result = parser.ParseExpression(expression);
            CollectionAssert.AreEqual(tokens, result);
        }

        [TestMethod]
        public void ParseExpression_CorrectShortExpression()
        {
            ParserRPN parser = new ParserRPN();
            string expression = "0.5*(5+2)";
            var tokens = new List<string>()
            {
                "0.5",
                "5",
                "2",
                "+",
                "*",
            };

            var result = parser.ParseExpression(expression);
            CollectionAssert.AreEqual(tokens, result);
        }

        [TestMethod]
        public void ParseExpression_CorrectLongExpression()
        {
            ParserRPN parser = new ParserRPN();
            string expression = "1+5*(2-6/(2+1))";
            var tokens = new List<string>()
            {
                "1",
                "5",
                "2",
                "6",
                "2",
                "1",
                "+",
                "/",
                "-",
                "*",
                "+",
            };

            var result = parser.ParseExpression(expression);
            CollectionAssert.AreEqual(tokens, result);
        }
    }
}
