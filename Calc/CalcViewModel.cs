using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using Calc.Models;
using System.Globalization;

namespace Calc
{
    class CalcViewModel : INotifyPropertyChanged, IDataErrorInfo
    {

        #region propertychaged
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChange([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
        #region IDataErrorInfo
        public string Error => null;

        public string this[string columnName]
        {
            get
            {
                string error = null;

                switch (columnName)
                {
                    case nameof(Expression):

                        if (Expression == string.Empty)
                        {
                            error = "Empty";
                        }
                        else if (Expression.Length > 2 && (Expression[^2] == '+' || Expression[^2] == '-' || Expression[^2] == '*' || Expression[^2] == '/'))
                        {
                            error = "Not correct";
                        }
                        break;

                }

                return error;
            }
        }
        #endregion

        private string _lastOperation;
        public string LastOperation
        {
            get
            {
                return _lastOperation;
            }
            set
            {
                _lastOperation = value;
                OnPropertyChange(nameof(LastOperation));
            }
        }
        private string _result;
        public string Result
        {
            get
            {
                return _result;
            }
            set
            {
                _result = value;
                OnPropertyChange(nameof(Result));
            }
        }

        private string _expression;
        public string Expression
        {
            get
            {
                return _expression;
            }
            set
            {
                _expression = value;
                OnPropertyChange(nameof(Expression));
            }
        }

        private int _bracketsCount;
        public int BracketsCount
        {
            get
            {
                return _bracketsCount;
            }
            set
            {
                _bracketsCount = value;
                OnPropertyChange(nameof(BracketsCount));
            }
        }

        public JournalCollectionTxt Journal
        {
            set;
            get;
        }

        public MemoryCollectionTxt Memory
        {
            set;
            get;
        }

      

        public CalcViewModel()
        {
            Expression = "0";
            Result = string.Empty;
            LastOperation = "=";

            Journal = new JournalCollectionTxt("Journal");
            if(Journal.TryLoad())
            {
                Journal.Collection = Journal.Load();
            }
            Memory = new MemoryCollectionTxt("Memory");
            if (Memory.TryLoad())
            {
                Memory.Collection = Memory.Load();
            }
        }

        private ICommand _digitButtonCommand;
        public ICommand DigitButtonCommand
        {
            get
            {
                return _digitButtonCommand ??= new RelayCommand<string>(DigitButton, digit => true);
            }
        }
        public void DigitButton(string digit)
        {
            if (Expression == "0")
            {
                Expression = $"{digit} ";
            }
            else
            {
                var exp = Expression.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if(exp[^1] == "0")
                {
                    Expression = Expression.Remove(Expression.Length - 2);
                    Expression += $"{digit}";
                }
                else if (decimal.TryParse(exp[^1], out _))
                {
                    
                    Expression = Expression.Remove(Expression.Length - 1);
                    Expression += $"{digit} ";

                }
                //Expression += $"{digit}";
            }
            
        }

        private ICommand _operationButtonCommand;
        public ICommand OperationButtonCommand
        {
            get
            {
                return _operationButtonCommand ??= new RelayCommand<string>(OperationButton, operation => true);
            }
        }
        public void OperationButton(string operation)
        {
            
            switch (operation)
            {
                case "=":
                    while (BracketsCount > 0)
                    {
                        Expression += " )";
                        BracketsCount--;
                    }

                    Result = CalcModel.Calculate(Expression);
                    Expression = $"{Expression} = {Result}";
                    Journal.Add(new JournalItem(Expression));
                    Expression = $"{Result}";
                    break;
                default:
                    if (Expression == string.Empty)
                    {
                        break;
                    }
                    else if (Expression.Length == 1)
                    {
                        Expression += $" {operation} ";
                        break;
                    }
                    else if (Expression[^2] == '+' || Expression[^2] == '/' || Expression[^2] == '*' || Expression[^2] == '-')
                    {
                        Expression = Expression.Remove(Expression.Length - 2, 2);
                        Expression += $"{operation} ";
                        break;
                    }
                    else
                    {
                        Expression += $" {operation} ";
                        break;
                    }
            }
            LastOperation = $"{operation}";
        }

        private ICommand _clearButtonCommand;
        public ICommand ClearButtonCommand
        {
            get
            {
                return _clearButtonCommand ??= new RelayCommand<string>(ClearButton, x => true);
            }
        }
        public void ClearButton(string x)
        {
            if (x == "C")
            {
                Expression = "0";
                LastOperation = "";
                BracketsCount = 0;
            }

            else if (x == "CE")
            {
                Expression = "0";
                LastOperation = "";
                BracketsCount = 0;

            }

        }

        private ICommand _pointButtonCommand;
        public ICommand PointButtonCommand
        {
            get
            {
                return _pointButtonCommand ??= new RelayCommand(PointButton);
            }
        }

        public void PointButton()
        {
            if (Expression != string.Empty)
            {
                string[] exp = Expression.Split(' ');
                if (exp[^1] == " ")
                {
                    Expression += "0.";
                }
                if (!exp[^1].Contains("."))
                {
                    Expression += ".";
                }
            }
        }

        private ICommand _bracketButtonCommand;
        public ICommand BracketButtonCommand
        {
            get
            {
                return _bracketButtonCommand ??= new RelayCommand<string>(BracketButton, bracket => true);
            }
        }

        public void BracketButton(string bracket)
        {

            switch (bracket)
            {
                case "(":
                    var exp = Expression.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    if (decimal.TryParse(exp[^1], out _))
                    {
                        //exp[^2] = bracket;
                        var tmp = exp[^1];
                        exp[^1] = bracket;
                        exp = exp.Append(tmp).ToArray();

                        Expression = string.Empty;
                        foreach (var item in exp)
                        {
                            Expression += $"{item} ";
                        }
                        BracketsCount++;
                        LastOperation = "BracketOpen";
                    }
                    else if (BracketsCount < 25 && LastOperation != "BracketClose")
                    {
                        Expression += "( ";

                        BracketsCount++;
                        LastOperation = "BracketOpen";
                    }
                    if (LastOperation == "=")
                    {
                        Expression = string.Empty;
                    }
                    
                    break;
                case ")":
                    if (BracketsCount > 0)
                    {
                        Expression += " )";
                        BracketsCount--;
                        LastOperation = "BracketClose";
                    }
                    break;
            }

        }

        private ICommand _memorySave;
        public ICommand MemorySave
        {
            get
            {
                return _memorySave ??= new RelayCommand(MemorySaveButton, () => Expression != string.Empty);
            }
        }
        public void MemorySaveButton()
        {
            Result = CalcModel.Calculate(Expression);
            Memory.Add(new MemoryItem(Result));
        }

        private ICommand _memoryDecrease;
        public ICommand MemoryDecrease
        {
            get
            {
                return _memoryDecrease ??= new RelayCommand(MemoryDecreaseButton, () => Memory.Collection.Count != 0);
            }
        }
        public void MemoryDecreaseButton()
        {

            Memory.ChangeValue(CalcModel.Calculate($"{Memory.Collection[0].Result} - {Expression}"));
        }

        private ICommand _memoryIncrease;
        public ICommand MemoryIncrease
        {
            get
            {
                return _memoryIncrease ??= new RelayCommand(MemoryIncreaseButton, () => Memory.Collection.Count != 0);
            }
        }
        public void MemoryIncreaseButton()
        {
            Memory.ChangeValue(CalcModel.Calculate($"{Memory.Collection[0].Result} + {Expression}"));

        }

        private ICommand _memoryRecall;
        public ICommand MemoryRecall
        {
            get
            {
                return _memoryRecall ??= new RelayCommand(MemoryReCallButton, () => Memory.Collection.Count != 0);
            }
        }
        public void MemoryReCallButton()
        {
            if (Expression != string.Empty && (Expression[^1] == '+' || Expression[^1] == '-' || Expression[^1] == '*' || Expression[^1] == '/'))
                Expression += Memory.Collection[0].Result;
            else
                Expression = Memory.Collection[0].Result;

        }



    }

}
