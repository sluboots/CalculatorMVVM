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
                        exp[i + 2] = (decimal.Parse(exp[i], NumberStyles.Float, new CultureInfo("en-US")) + decimal.Parse(exp[i + 2], NumberStyles.Float, new CultureInfo("en-US"))).ToString();
                        break;
                    case "-":
                        exp[i + 2] = (decimal.Parse(exp[i], NumberStyles.Float, new CultureInfo("en-US")) - decimal.Parse(exp[i + 2], NumberStyles.Float, new CultureInfo("en-US"))).ToString();
                        break;
                    case "*":
                        exp[i + 2] = (decimal.Parse(exp[i], NumberStyles.Float, new CultureInfo("en-US")) * decimal.Parse(exp[i + 2], NumberStyles.Float, new CultureInfo("en-US"))).ToString();
                        break;
                    case "/":
                        exp[i + 2] = (decimal.Parse(exp[i], NumberStyles.Float, new CultureInfo("en-US")) / decimal.Parse(exp[i + 2], NumberStyles.Float, new CultureInfo("en-US"))).ToString();
                        break;
                    default:
                        break;

                }

            }


           
            return exp[^1];
        }

    }

    //public class Expression
    //{
    //    public Expression(string expression, ICalculator calc)
    //    {
    //        MathExpression = expression;
    //        Steps = MathExpression;
    //        if (calc is null)
    //        {
    //            Result = 0;
    //            ErrorMessage = "Null Calculate";
    //        }
    //        else
    //        {
    //            var res = calc.Calculate(this);
    //            Result = res.Result;
    //            ErrorMessage = res.ErrorMessage;
    //        }
    //    }

    //    public Expression()
    //    {

    //    }
    //    public string Steps { get; set; }
    //    public string MathExpression { get; }
    //    public string Action { get; set; }
    //    public double Result { get; set; }
    //    public bool HasError => string.IsNullOrWhiteSpace(ErrorMessage) == false;
    //    public string ErrorMessage { get; set; }
    //}
}




/*
 
    // Получаем текст кнопки
            string s = (string)((Button)e.OriginalSource).Content;
            // Добавляем его в текстовое поле
            textBlock.Text += s;
            int num;
            // Пытаемся преобразовать его в число
            bool result = Int32.TryParse(s, out num);
            // Если текст - это число
            if (result == true)
            {
                // Если операция не задана
                if (operation == "")
                {
                    // Добавляем к левому операнду
                    leftop += s;
                }
                else
                {
                    // Иначе к правому операнду
                    rightop += s;
                }
            }
            // Если было введено не число
            else
            {
                // Если равно, то выводим результат операции
                if (s == "=")
                {
                    Update_RightOp();
                    textBlock.Text += rightop;
                    operation = "";
                }
                // Очищаем поле и переменные
                else if (s == "CLEAR")
                {
                    leftop = "";
                    rightop = "";
                    operation = "";
                    textBlock.Text = "";
                }
                // Получаем операцию
                else
                {
                    // Если правый операнд уже имеется, то присваиваем его значение левому
                    // операнду, а правый операнд очищаем
                    if (rightop != "")
                    {
                        Update_RightOp();
                        leftop = rightop;
                        rightop = "";
                    }
                    operation = s;
                }
            }
        }
        // Обновляем значение правого операнда
        private void Update_RightOp()
        {
            int num1 = Int32.Parse(leftop);
            int num2 = Int32.Parse(rightop);
            // И выполняем операцию
            switch (operation)
            {
                case "+":
                    rightop = (num1 + num2).ToString();
                    break;
                case "-":
                    rightop = (num1 - num2).ToString();
                    break;
                case "*":
                    rightop = (num1 * num2).ToString();
                    break;
                case "/":
                    rightop = (num1 / num2).ToString();
                    break;
            }
        }
 *
 */
