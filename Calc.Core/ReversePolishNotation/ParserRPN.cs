using System;
using System.Collections.Generic;
using System.Text;

namespace BK_S.Calc.Core.ReversePolishNotation
{
    public class ParserRPN : IReversePolishNotationParser
    {
        public List<string> ParseExpression(string expression)
        {
            var operations = new Stack<OperationType>();
            var tokens = new List<string>();
            var currentNumber = string.Empty;

            foreach (var item in expression)
            {
                if (char.IsDigit(item) || item == '.')
                {
                    currentNumber += item;
                }
                if (Calculator.IsOperation(item.ToString()))
                {
                    if (currentNumber != string.Empty)
                    {
                        tokens.Add(currentNumber);
                        currentNumber = string.Empty;
                    }

                    var currentOperationType = Calculator.GetOperationType(item.ToString());

                    if (operations.Count == 0 ||
                        currentOperationType == OperationType.LeftBracket)
                    {
                        operations.Push(currentOperationType);
                        continue;
                    }

                    var prevOperationPriority = Calculator.OperationPriorities[operations.Peek()];
                    var currentOperationPriority = Calculator.OperationPriorities[currentOperationType];

                    while (operations.Count > 0 && prevOperationPriority >= currentOperationPriority)
                    {
                        if (currentOperationType == OperationType.RightBracket &&
                            operations.Peek() == OperationType.LeftBracket)
                        {
                            operations.Pop();
                            break;
                        }
                        else
                        {
                            var operation = Calculator.Operations[operations.Pop()];
                            tokens.Add(operation);
                        }
                        if (operations.Count > 0)
                        {
                            prevOperationPriority = Calculator.OperationPriorities[operations.Peek()];
                        }
                    }

                    if (currentOperationType != OperationType.RightBracket)
                    {
                        operations.Push(currentOperationType);
                    }
                }
            }

            if (currentNumber != string.Empty)
            {
                tokens.Add(currentNumber);
            }
            foreach (var item in operations)
            {
                tokens.Add(Calculator.Operations[item]);
            }

            return tokens;
        }
    }
}
