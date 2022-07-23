using System;
using System.Collections.Generic;
using System.Text;

namespace BK_S.Calc.Core
{
    public enum OperationType
    {
        Addition,
        Substraction,
        Multiplication,
        Division,
        Power,
        LeftBracket,
        RightBracket
    }

    public interface ICalculator
    {
        public double Calculate(double firstValue, double secondValue, OperationType operationType);
    }
}
