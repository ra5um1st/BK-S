using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace BK_S.Calc.Core
{
    public class Calculator : ICalculator
    {

        public static readonly Dictionary<OperationType, int> OperationPriorities = new Dictionary<OperationType, int>()
        {
            {OperationType.LeftBracket, 0},
            {OperationType.RightBracket, 0},
            {OperationType.Addition, 1},
            {OperationType.Substraction, 1},
            {OperationType.Multiplication, 2},
            {OperationType.Division, 2},
            {OperationType.Power, 3},

        };
        public static readonly Dictionary<OperationType, string> Operations = new Dictionary<OperationType, string>()
        {
            {OperationType.Addition, "+"},
            {OperationType.Substraction, "-"},
            {OperationType.Multiplication, "*"},
            {OperationType.Division, "/"},
            {OperationType.Power, "^"},
            {OperationType.LeftBracket, "("},
            {OperationType.RightBracket, ")"},
        };

        public double Calculate(double firstValue, double secondValue, OperationType operationType)
        {
            switch (operationType)
            {
                case OperationType.Addition:
                    return firstValue + secondValue;
                case OperationType.Substraction:
                    return firstValue - secondValue;
                case OperationType.Multiplication:
                    return firstValue * secondValue;
                case OperationType.Division:
                    return firstValue / secondValue;
                case OperationType.Power:
                    return Math.Pow(firstValue, secondValue);
                default:
                    return 0;
            }
        }

        public static bool IsNumber(string token)
        {
            return Regex.IsMatch(token, "^[-]?([0-9]*[.])?[0-9]+$");
        }

        public static bool IsOperation(string token)
        {
            return Operations.Any(item => item.Value == token);
        }

        public static OperationType GetOperationType(string token)
        {
            return Operations.FirstOrDefault(item => item.Value == token).Key;
        }

    }
}
