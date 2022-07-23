using System.Collections.Generic;
using System.Globalization;
using System.Windows.Input;
using WPF.Core.ViewModels;
using WPF.Core.Commands;
using Calc.Core;
using System.Linq;
using System;
using BK_S.Calc.Core;
using BK_S.Calc.Core.ReversePolishNotation;

namespace Calc.WPF.ViewModels
{
    public class MainViewModel : ObservableObject
    {
        public MainViewModel()
        {
            _format = new NumberFormatInfo()
            {
                NegativeSign = "-",
                NumberDecimalSeparator = ".",
            };

            _calculator = new ReversePolishNotationCalculator(new Calculator(), new CalculatorRPN(), new ParserRPN());
            _currentNumber = new Number();

            CurrentValue = "0";

            EqualsCommand = new DelegateCommand(OnEqualsExecute);
            AddCommand = new DelegateCommand(OnAddCommandExecute);
            SubstractCommand = new DelegateCommand(OnSubstractCommandExecute);
            MultiplyCommand = new DelegateCommand(OnMultiplyCommandExecute);
            DivideCommand = new DelegateCommand(OnDivideCommandExecute);
            PercentCommand = new DelegateCommand(OnPercentCommandExecute);
            SquareRootCommand = new DelegateCommand(OnSquareRootCommandExecute);
            PowerCommand = new DelegateCommand(OnPowerCommandExecute);
            ClearCommand = new DelegateCommand(OnClearCommandExecute);
            ClearExistingCommand = new DelegateCommand(OnClearExistingCommandExecute);
            LeftBracketCommand = new DelegateCommand(OnLeftBracketCommandExecute);
            RightBracketCommand = new DelegateCommand(OnRightBracketCommandExecute);
            SignCommand = new DelegateCommand(OnSignCommandExecute);
            FloatCommand = new DelegateCommand(OnFloatCommandExecute);

            AddDigitCommand = new DelegateCommand(OnAddDigitCommandExecute);
        }

        #region Private Fields

        private NumberFormatInfo _format;
        private Number _currentNumber;
        private ReversePolishNotationCalculator _calculator;
        private bool _isFloatingStage;
        private bool _isRightBraketStage;
        private bool _isCalculationStage;
        private bool _isTempResultStage;
        private int _openBracketsCount;

        #endregion Private Fields

        #region Properties

        private string _expression;

        public string Expression
        {
            get => _expression;
            set => Set(ref _expression, value);
        }

        private string _currentValue;

        public string CurrentValue
        {
            get => _currentValue;
            set => Set(ref _currentValue, value);
        }

        private string _operation;

        public string Operation
        {
            get => _operation;
            set => Set(ref _operation, value);
        }

        #endregion Properties

        #region Operations Commands

        private void SetOperation(string operation)
        {
            if (_isCalculationStage)
            {
                OnClearCommandExecute(null);
            }
            if (_isFloatingStage)
            {
                CurrentValue = CurrentValue.Substring(0, CurrentValue.Length - 1);
                _isFloatingStage = false;
            }
            if(_isRightBraketStage)
            {
                Expression += operation;
                _isRightBraketStage = false;
            }
            else
            {
                if (Operation == operation)
                {
                    Expression += CurrentValue;
                    CalculateExpression();
                    Expression += operation;
                    _isTempResultStage = true;
                }
                else
                {
                    Expression += CurrentValue + operation;
                    _currentNumber.Reset();
                    CurrentValue = _currentNumber;
                    _isTempResultStage = false;
                }
            }

            Operation = operation;
        }

        public ICommand EqualsCommand { get; set; }

        private void OnEqualsExecute(object obj)
        {
            _isCalculationStage = true;

            if (Operation != string.Empty)
            {
                Expression += CurrentValue;
            }
            while (_openBracketsCount > 0)
            {
                Expression += ")";
                _openBracketsCount--;
            }
            if(!Expression.Any(item => char.IsDigit(item)))
            {
                OnClearCommandExecute(null);
                return;
            }

            Expression += "=";

            CalculateExpression();
        }

        private void CalculateExpression()
        {
            var result = _calculator.Calcucate(Expression);
            _currentNumber.Reset(result, _format);
            CurrentValue = _currentNumber;

            Operation = string.Empty;
        }

        public ICommand AddCommand { get; set; }
        private void OnAddCommandExecute(object obj)
        {
            SetOperation(obj.ToString());
        }

        public ICommand SubstractCommand { get; set; }
        private void OnSubstractCommandExecute(object obj)
        {
            SetOperation(obj.ToString());
        }

        public ICommand MultiplyCommand { get; set; }
        private void OnMultiplyCommandExecute(object obj)
        {
            SetOperation("*");
        }

        public ICommand DivideCommand { get; set; }
        private void OnDivideCommandExecute(object obj)
        {
            SetOperation("/");
        }

        public ICommand PercentCommand { get; set; }
        private void OnPercentCommandExecute(object obj)
        {
            SetOperation("/");
            CurrentValue = "100";
            OnEqualsExecute(null);
        }

        public ICommand SquareRootCommand { get; set; }
        private void OnSquareRootCommandExecute(object obj)
        {
            SetOperation("^");
            CurrentValue = "0.5";
            OnEqualsExecute(null);
        }

        public ICommand PowerCommand { get; set; }
        private void OnPowerCommandExecute(object obj)
        {
            SetOperation("^");
        }

        public ICommand ClearCommand { get; set; }
        private void OnClearCommandExecute(object obj)
        {
            _isCalculationStage = false;
            _isRightBraketStage = false;
            _isFloatingStage = false;
            _isTempResultStage = false;
            Operation = string.Empty;
            Expression = string.Empty;
            CurrentValue = _currentNumber.Reset();
        }

        public ICommand ClearExistingCommand { get; set; }
        private void OnClearExistingCommandExecute(object obj)
        {
            _isFloatingStage = false;
            Operation = string.Empty;
            CurrentValue = _currentNumber.Reset();
        }

        public ICommand LeftBracketCommand { get; set; }
        private void OnLeftBracketCommandExecute(object obj)
        {
            Expression += "(";
            _openBracketsCount++;
        }

        public ICommand RightBracketCommand { get; set; }
        private void OnRightBracketCommandExecute(object obj)
        {
            if(_openBracketsCount > 0)
            {
                _isRightBraketStage = true;

                Expression += CurrentValue + ")";

                Operation = string.Empty;
                CurrentValue = _currentNumber.Reset();
                _openBracketsCount--;
            }
        }

        public ICommand SignCommand { get; set; }
        private void OnSignCommandExecute(object obj)
        {
            _currentNumber.InvertSign();
            CurrentValue = _currentNumber;
        }

        public ICommand FloatCommand { get; set; }

        private void OnFloatCommandExecute(object obj)
        {
            if (!CurrentValue.EndsWith(".") && !CurrentValue.Contains("."))
            {
                CurrentValue += ".";
                _isFloatingStage = true;
            }
        }

        #endregion

        #region Digits Commands

        public ICommand AddDigitCommand { get; set; }

        private void OnAddDigitCommandExecute(object obj)
        {
            var digit = Number.Digits.FirstOrDefault(item => item.Value == obj.ToString()).Key;

            if (_isTempResultStage)
            {
                _currentNumber.Reset();
            }
            if (_isCalculationStage)
            {
                OnClearCommandExecute(null);
            }
            if (_isFloatingStage)
            {
                _currentNumber.MakeFloating(digit);
                _isFloatingStage = false;
            }
            else
            {
                _currentNumber.AddDigit(digit);
            }

            CurrentValue = _currentNumber;
        }

        #endregion
    }
}
