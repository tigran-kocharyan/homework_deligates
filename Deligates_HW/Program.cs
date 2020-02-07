using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;

namespace Deligates_HW
{
    /// <summary>
    /// Делегат типа void, принимающий на вход строку message.
    /// </summary>
    /// <param name="message"></param>
    public delegate void ErrorNotificationType(string message);

    class Program
    {
        /// <summary>
        /// Создаем StringBuilder для заполнения его ошибками.
        /// </summary>
        public static StringBuilder errorMessages = new StringBuilder();

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
                foreach (string expression in expressions)
                {
                    try
                    {
                        double answer = Calculator.Calculate(expression);
                        File.AppendAllText(pathWrite, $"{answer:F3}" + Environment.NewLine);
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                }
                Console.WriteLine("Запись в файл answers.txt успешно завершена..." + Environment.NewLine);
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
                Console.WriteLine("Создание файла results.txt..." + Environment.NewLine);
                File.WriteAllText(pathResults, "");

                string[] expressions = File.ReadAllLines(pathExpression);
                string[] answers = File.ReadAllLines(pathAnswer);
                int errors = 0;


                // Так как говорится, что данные будут валидные, то можем использовать обычный double.Parse, но все равно покроем его в 
                // Try...Catch.
                try
                {
                    Console.WriteLine("Процесс проверки начался..." + Environment.NewLine);
                    for (int i = 0; i < expressions.Length; i++)
                    {
                        double answer = double.Parse($"{answers[i]:F3}");
                        double expression = double.Parse($"{expressions[i]:F3}");
                        if (answer == expression)
                        {
                            File.AppendAllText(pathResults, "OK" + Environment.NewLine);
                        }
                        else
                        {
                            File.AppendAllText(pathResults, "Error" + Environment.NewLine);
                            errors++;
                        }
                    }
                    File.AppendAllText(pathResults, "Найдено ошибок: " + errors.ToString());
                    Console.WriteLine("Файл results.txt успешно создан!" + Environment.NewLine);
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
        /// Метод для вывода на консоль ошибок.
        /// </summary>
        /// <param name="message"></param>
        public static void ConsoleErrorHandler(string message)
        {
            Console.WriteLine(message + " " + DateTime.Now);
        }

        /// <summary>
        /// Заполнитель файла с ошибками.
        /// </summary>
        /// <param name="message"></param>
        public static void ResultErrorHandler(string message)
        {
            if (message == "Выражение не является числом.")
            {
                errorMessages.Append("не число\n");
                return;
            }
            else if (message == "Значение было недопустимо малым или недопустимо большим для Double.")
            {
                errorMessages.Append("∞\n");
                return;
            }
            else if (message == "Выражение не является числом.")
            {
                errorMessages.Append("не число\n");
                return;
            }
            else if (message == "Вызванного оператора нет в доступных.")
            {
                errorMessages.Append("неверный оператор\n");
                return;
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
                    Calculator.ErrorNotification += ConsoleErrorHandler;
                    Calculator.ErrorNotification += ResultErrorHandler;

                    Console.WriteLine("Считывание из файла expression.txt для последующего подсчета..." + Environment.NewLine);
                    WriteAnswers("../../../expressions.txt", "../../../answers.txt");
                    Console.WriteLine("Начинается процесс проверки..." + Environment.NewLine);
                    //CheckAnswers("../../../expressions_checker.txt", "../../../answers.txt", "../../../results.txt");
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
