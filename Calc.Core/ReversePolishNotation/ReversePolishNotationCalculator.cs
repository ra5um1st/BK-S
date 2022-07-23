using BK_S.Calc.Core;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace BK_S.Calc.Core.ReversePolishNotation
{
    public class ReversePolishNotationCalculator
    {
        public ICalculator Calculator { get; set; }
        public IReversePolishNotationCalculator CalculatorRPN { get; set; }
        public IReversePolishNotationParser ParserRPN { get; set; }

        public ReversePolishNotationCalculator(
            ICalculator calculator,
            IReversePolishNotationCalculator rpnCalculator,
            IReversePolishNotationParser rpnParser)
        {
            Calculator = calculator;
            CalculatorRPN = rpnCalculator;
            ParserRPN = rpnParser;
        }

        public double Calcucate(string expression)
        {
            var tokens = ParserRPN.ParseExpression(expression);
            return CalculatorRPN.Calculate(Calculator, tokens, new NumberFormatInfo() { NumberDecimalSeparator = ".", NegativeSign = "-" });
        }
    }
}
