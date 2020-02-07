using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deligates_HW
{
    public class Calculator
    {
        /// <summary>
        /// Объявление делегата и словаря для использования в конструкторе.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public delegate double MathOperation(double a, double b);

        /// <summary>
        /// Публичное событие типа ErrorNotificationType.
        /// </summary>
        public static event ErrorNotificationType ErrorNotification;

        public static Dictionary<string, MathOperation> operations;
        /// <summary>
        /// Статический конструктор необходим для того, чтобы не перезаполнять всю таблицу после того, 
        /// как программа будет перезапущена через ENTER.
        /// </summary>
        static Calculator()
        {
            operations = new Dictionary<string, MathOperation>();

            MathOperation plus = (x, y) => x + y;
            MathOperation multiply = (x, y) => x * y;
            MathOperation divide = (x, y) => x / y;
            MathOperation substract = (x, y) => x - y;
            MathOperation pow = (x, y) => Math.Pow(x, y);
            operations.Add("+", plus);
            operations.Add("*", multiply);
            operations.Add("/", divide);
            operations.Add("-", substract);
            operations.Add("^", pow);
        }

        /// <summary>
        /// Метод возвращает вещественное число - результат операции между двумя операндами и самой операцией.
        /// </summary>
        /// <param name="expr"></param>
        /// <returns></returns>
        public static double Calculate(string expr)
        {
            string[] operators = expr.Split(' ');
            try
            {
                double firstOperand = double.Parse(operators[0]);
                double secondOperand = double.Parse(operators[2]);
                if(operators[1]=="/" && secondOperand==0)
                {
                    throw new Exception("Делить на ноль нельзя!");
                }
                else if(double.IsNaN(Math.Round(operations[operators[1]](firstOperand, secondOperand), 3)))
                {
                    throw new Exception("Выражение не является числом.");
                }
                else if(!operations.ContainsKey(operators[1]))
                {
                    throw new Exception("Вызванного оператора нет в доступных.");
                }
                return Math.Round(operations[operators[1]](firstOperand, secondOperand),3);
            }
            catch (Exception e)
            {
                ErrorNotification(e.Message);
                // Программе нужно что-то вернуть, чтобы продолжить, пусть это будет NaN 
                // или проброить исключение выше.
                throw;
            }
        }

    }
}
