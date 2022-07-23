using BK_S.Calc.Core;
using BK_S.Calc.Core.ReversePolishNotation;
using Calc.WPF.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class CalculatePostfixNotation_Test
    {
        [TestMethod]
        public void CalculatePostfixNotation_CorrectShortExpression()
        {
            var calculator = new ReversePolishNotationCalculator(new Calculator(), new CalculatorRPN(), new ParserRPN());
            string expression = "(2*5+5)/3";

            Assert.AreEqual(5.0, calculator.Calcucate(expression));
        }

        [TestMethod]
        public void CalculatePostfixNotation_CorrectLongExpression()
        {
            var calculator = new ReversePolishNotationCalculator(new Calculator(), new CalculatorRPN(), new ParserRPN());
            string expression = "1+5*(2-6/(2+1))";

            Assert.AreEqual(1.0, calculator.Calcucate(expression));
        }
    }
}
