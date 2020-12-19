using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;


namespace Calc.Models
{
    public class CalcModel
    {

        public CalcModel()
        {

        }
        public static string Calculate(string expression)
        {

            string[] exp = expression.Split(' ');
            for (int i = 0; i < exp.Length - 1; i += 2)
            {
                switch (exp[i + 1])
                {
                    case "+":
                        exp[i + 2] = (decimal.Parse(exp[i], NumberStyles.Float, new CultureInfo("en-US")) + decimal.Parse(exp[i + 2], NumberStyles.Float, new CultureInfo("en-US"))).ToString(new CultureInfo("en-US"));
                        break;
                    case "-":
                        exp[i + 2] = (decimal.Parse(exp[i], NumberStyles.Float, new CultureInfo("en-US")) - decimal.Parse(exp[i + 2], NumberStyles.Float, new CultureInfo("en-US"))).ToString(new CultureInfo("en-US"));
                        break;
                    case "*":
                        exp[i + 2] = (decimal.Parse(exp[i], NumberStyles.Float, new CultureInfo("en-US")) * decimal.Parse(exp[i + 2], NumberStyles.Float, new CultureInfo("en-US"))).ToString(new CultureInfo("en-US"));
                        break;
                    case "/":
                        exp[i + 2] = (decimal.Parse(exp[i], NumberStyles.Float, new CultureInfo("en-US")) / decimal.Parse(exp[i + 2], NumberStyles.Float, new CultureInfo("en-US"))).ToString(new CultureInfo("en-US"));
                        break;
                    default:
                        break;

                }

            }



            return exp[^1];
        }

    }
}