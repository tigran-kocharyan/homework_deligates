using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;

namespace Deligates_HW
{
    class Program
    {
        /// <summary>
        /// Объявление делегата и словаря для использования в конструкторе.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public delegate double MathOperation(double a, double b);
        public static Dictionary<string, MathOperation> operations;
        /// <summary>
        /// Статический конструктор необходим для того, чтобы не перезаполнять всю таблицу после того, 
        /// как программа будет перезапущена через ENTER.
        /// </summary>
        static Program()
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
        static double Calculate(string expr)
        {
            string[] operators = expr.Split(' ');
            try
            {
                double firstOperand = double.Parse(operators[0]);
                double secondOperand = double.Parse(operators[2]);

                return operations[operators[1]](firstOperand, secondOperand);
            }
            catch (Exception)
            {
                Console.WriteLine("Строка должна иметь вид \"{operand_1} {operation} {operand_2}\"");
                return double.NaN;
            }
        }

        /// <summary>
        /// Метод считывает операнды из pathRead и заполняет результат их операций в файл по пути pathWrite.
        /// </summary>
        /// <param name="pathRead"></param>
        /// <param name="pathWrite"></param>
        static void WriteAnswers(string pathRead, string pathWrite)
        {
            try
            {
                Console.WriteLine("Создание файла answers.txt..." + Environment.NewLine);
                File.WriteAllText(pathWrite, "");
                string[] expressions = File.ReadAllLines(pathRead);
                try
                {
                    foreach (string expression in expressions)
                    {
                        double answer = Calculate(expression);
                        File.AppendAllText(pathWrite, $"{answer:F3}" + Environment.NewLine);
                    }
                    Console.WriteLine("Запись в файл answers.txt успешно завершена..." + Environment.NewLine);
                }
                catch (Exception)
                {
                    Console.WriteLine("Произошла ошибка в работе с файлом!");
                }
            }
            // Необходимо ловить все исключения, связанные с файлами.
            catch (FileNotFoundException)
            {
                Console.WriteLine("Данного файла не существует.");
            }
            catch (IOException)
            {
                Console.WriteLine("Ошибка работы с файлом.");
            }
            catch (UnauthorizedAccessException)
            {
                Console.WriteLine("Ошибка доступа.");
            }
            catch (System.Security.SecurityException)
            {
                Console.WriteLine("Ошибка безопасности.");
            }
        }

        /// <summary>
        /// Метод сравнивает значения из pathExpression и pathAnswer, и если они равны, то записывает в pathResults либо
        /// OK, либо Error, в зависимости от результат сравнения. Затем добавляет к файлу на пути pathResults кол-во ошибок.
        /// </summary>
        /// <param name="pathExpression"></param>
        /// <param name="pathAnswer"></param>
        /// <param name="pathResults"></param>
        static void CheckAnswers(string pathExpression, string pathAnswer, string pathResults)
        {
            try
            {
                Console.WriteLine("Создание файла results.txt..."+Environment.NewLine);
                File.WriteAllText(pathResults, "");

                string[] expressions = File.ReadAllLines(pathExpression);
                string[] answers = File.ReadAllLines(pathAnswer);
                int errors = 0;


                // Так как говорится, что данные будут валидные, то можем использовать обычный double.Parse, но все равно покроем его в 
                // Try...Catch.
                try
                {
                    Console.WriteLine("Процесс проверки начался..."+Environment.NewLine);
                    for (int i = 0; i < expressions.Length; i++)
                    {
                        double answer = double.Parse($"{answers[i]:F3}");
                        double expression = double.Parse($"{expressions[i]:F3}");
                        if (answer == expression)
                        {
                            File.AppendAllText(pathResults,"OK"+Environment.NewLine);
                        }
                        else
                        {
                            File.AppendAllText(pathResults, "Error" + Environment.NewLine);
                            errors++;
                        }
                    }
                    File.AppendAllText(pathResults, "Найдено ошибок: "+errors.ToString());
                    Console.WriteLine("Файл results.txt успешно создан!"+Environment.NewLine);
                }
                catch (Exception)
                {
                    Console.WriteLine("Произошла ошибка в работе с файлом!");
                }
            }
            // Необходимо ловить все исключения, связанные с файлами.
            catch (FileNotFoundException)
            {
                Console.WriteLine("Данного файла не существует.");
            }
            catch (IOException)
            {
                Console.WriteLine("Ошибка работы с файлом.");
            }
            catch (UnauthorizedAccessException)
            {
                Console.WriteLine("Ошибка доступа.");
            }
            catch (System.Security.SecurityException)
            {
                Console.WriteLine("Ошибка безопасности.");
            }
        }

        /// <summary>
        /// Декомпозированный метод Main для запуска, который отдельно выполняет все методы.
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            do
            {
                try
                {
                    Console.WriteLine("Считывание из файла expression.txt для последующего подсчета..." + Environment.NewLine);
                    WriteAnswers("../../../expressions.txt", "../../../answers.txt");
                    Console.WriteLine("Начинается процесс проверки..." + Environment.NewLine);
                    CheckAnswers("../../../expressions_checker.txt", "../../../answers.txt", "../../../results.txt");
                }
                catch (Exception)
                {
                    Console.WriteLine("Произошла ошибка!");
                }

                //Для повтора решения необходимо будет нажать в конце ENTER!
                Console.WriteLine("Программа успешно закончила работу! Для повтора решения введите ENTER...");
            } while (Console.ReadKey(true).Key == ConsoleKey.Enter);
        }
    }
}
