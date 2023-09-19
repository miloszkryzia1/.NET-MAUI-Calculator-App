using org.mariuszgromada.math.mxparser;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Calculator
{
    [AddINotifyPropertyChangedInterface]
    class MainViewModel
    {

        private bool computed;
        public string Expression { get; set; }
        public double Result { get; set; }
        public ICommand ButtonClick => new Command(x => ButtonAction((string)x));

        private void Evaluate() 
        {
            Expression e = new Expression(Expression);
            Result = (double)e.calculate();
        }

        private void ButtonAction(string symbol)
        {
            if ((symbol.Equals("+") || (symbol.Equals("-") || (symbol.Equals("*") || (symbol.Equals("/"))))))
            {
                if (computed)
                {
                    Expression = "" + Result;
                    computed = false;
                }
                Expression += $" {symbol} ";
            }
            else if (Regex.IsMatch(symbol, @"\d"))
            {
                if ((Expression.Length == 1) && (Expression[Expression.Length - 1] == '0'))
                {
                    Expression = ReplaceEndZero(Expression, symbol.ToCharArray()[0]);
                }
                else
                {
                    Expression += symbol;
                }
            }
            else if (symbol.Equals("AC"))
            {
                Expression = "0";
                Result = 0;
                computed = false;
            }
            else if (symbol.Equals("BS"))
            {
                computed = false;
                if (Expression.Length == 1)
                {
                    Expression = "0";
                }
                else if (Regex.IsMatch(Expression.Substring(Expression.Length - 1, 1), @"\d") || (Expression[Expression.Length-1] == '.') || (Expression[Expression.Length-1] == '%'))
                {
                    Expression = Expression.Substring(0, Expression.Length - 1);
                }
                else
                {
                    Expression = Expression.Substring(0, Expression.Length - 3);
                }
            }
            else if (symbol == ".")
            {
                Func<bool> decimalPresent = () =>
                {
                    try
                    {
                        var lastOperand = Expression.Substring(Expression.LastIndexOf(' '));
                        return Regex.IsMatch(lastOperand, @"\d*\.\d*");
                    }
                    catch (ArgumentOutOfRangeException e)
                    {
                        return Regex.IsMatch(Expression, @"\d*\.\d*");
                    }
                };
                if (computed)
                {
                    Expression = "" + Result;
                    computed = false;
                }
                if (!decimalPresent())
                {
                    Expression += symbol;
                }
            }
            else if ((symbol.Equals("=")))
            {
                Evaluate();
                computed = true;
            }
            else if (symbol.Equals("%"))
            {
                if (Regex.IsMatch(Expression[Expression.Length - 1]+"", @"\d"))
                {
                    Expression += "%";
                }
            }
        }

        private string ReplaceEndZero(string exp, char symbol)
        {
            char[] chars = exp.ToCharArray();
            chars[chars.Length - 1] = symbol;
            StringBuilder sb = new StringBuilder();
            foreach (char  c in chars)
            {
                sb.Append(c);
            }
            return sb.ToString();
        }
    }
}
