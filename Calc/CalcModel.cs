using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Collections.ObjectModel;

namespace Calc.Models
{
    public class CalcModel
    {

        public CalcModel()
        {

        }
        public static string Calculate(string expression)
        {
            ReadOnlyDictionary<string, int> prec = new ReadOnlyDictionary<string, int>(
                new Dictionary<string, int>
                {
                    {"*", 3},
                    {"/", 3},
                    {"+", 2},
                    {"-", 2},
                    {"(", 1},
                });
            Stack<string> operations = new Stack<string>();
            string[] exp = expression.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            expression = string.Empty;
            string topToken;

            foreach (var item in exp)
            {
                if (decimal.TryParse(item, NumberStyles.Float, new CultureInfo("en-US"), out _))
                {
                    expression += $"{item} ";
                }
                else if (item == "(")
                {
                    operations.Push(item);
                }
                else if(item == ")")
                {
                    topToken = operations.Pop();
                    while(topToken != "(")
                    {
                        expression += topToken;
                        topToken = operations.Pop();
                    }
                }
                else if (prec.ContainsKey(item))
                {
                    if(!operations.Count.Equals(0) && prec[item] <= prec[operations.Peek()])
                    {
                        expression += $"{operations.Pop()} ";
                    }
                    operations.Push(item);
                }
            }
            while (!operations.Count.Equals(0))
            {
                expression += $"{operations.Pop()} ";
            }
            Stack<string> operand= new Stack<string>();
            exp = expression.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            foreach (var item in exp)
            {
                if (decimal.TryParse(item, NumberStyles.Float, new CultureInfo("en-US"), out _))
                {
                    operand.Push(item);
                }
                else
                {
                    string operand2 = operand.Pop();
                    string operand1 = operand.Pop();
                    string result = doMath(item, operand1, operand2);
                    operand.Push(result);
                }
            }
            return operand.Pop();
           
        }

        public static string doMath(string item, string op1, string op2)
        {
            if(item == "*")
            {
                return (decimal.Parse(op1, NumberStyles.Float, new CultureInfo("en-US")) *
                                              decimal.Parse(op2, NumberStyles.Float, new CultureInfo("en-US"))).ToString("G29", new CultureInfo("en-US"));
            }
            else if(item == "/")
            {
                return (decimal.Parse(op1, NumberStyles.Float, new CultureInfo("en-US")) /
                                              decimal.Parse(op2, NumberStyles.Float, new CultureInfo("en-US"))).ToString("G29", new CultureInfo("en-US"));
            }
            else if(item == "+")
            {
                return (decimal.Parse(op1, NumberStyles.Float, new CultureInfo("en-US")) +
                                              decimal.Parse(op2, NumberStyles.Float, new CultureInfo("en-US"))).ToString("G29", new CultureInfo("en-US"));
            }
            else
            {
                return (decimal.Parse(op1, NumberStyles.Float, new CultureInfo("en-US")) -
                                              decimal.Parse(op2, NumberStyles.Float, new CultureInfo("en-US"))).ToString("G29", new CultureInfo("en-US"));
            }
        }

        public static string NormalForm(string expression)
        {
            for (int i = 0; i < expression.Length; i++)
            {
                if (expression[i] == '.')
                {

                }
            }

            return expression;
        }

    }

    
}