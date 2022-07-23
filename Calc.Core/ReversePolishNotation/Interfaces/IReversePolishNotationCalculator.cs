using BK_S.Calc.Core;
using System.Collections.Generic;
using System.Globalization;

namespace BK_S.Calc.Core.ReversePolishNotation
{
    public interface IReversePolishNotationCalculator
    {
        public double Calculate(ICalculator calculator, List<string> tokens, NumberFormatInfo formatInfo);
    }
}
