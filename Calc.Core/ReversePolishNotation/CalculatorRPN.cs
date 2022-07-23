using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace BK_S.Calc.Core.ReversePolishNotation
{
    public class CalculatorRPN : IReversePolishNotationCalculator
    {
        public double Calculate(ICalculator calculator, List<string> tokens, NumberFormatInfo formatInfo)
        {
            var numbers = new Stack<double>();

            foreach (var item in tokens)
            {
                if (Calculator.IsNumber(item))
                {
                    numbers.Push(double.Parse(item, formatInfo));
                }
                if (Calculator.IsOperation(item))
                {
                    var operationType = Calculator.GetOperationType(item);
                    double secondValue = numbers.Pop();
                    double firstValue = numbers.Pop();
                    numbers.Push(calculator.Calculate(firstValue, secondValue, operationType));
                }
            }

            return numbers.Pop();
        }
    }
}
