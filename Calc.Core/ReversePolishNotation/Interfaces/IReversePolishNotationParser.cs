using System;
using System.Collections.Generic;
using System.Text;

namespace BK_S.Calc.Core.ReversePolishNotation
{
    public interface IReversePolishNotationParser
    {
        public List<string> ParseExpression(string expression);
    }
}
