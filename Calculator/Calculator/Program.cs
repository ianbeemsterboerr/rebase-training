using JetBrains.Annotations;
using System;
using System.Globalization;

namespace Calculator
{
    internal static class Program
    {
        private enum Operator
        {
            Add,
            Subtract,
            Multiply,
            Divide,
        }

        private static void Main()
        {
            Console.WriteLine("Welcome to Calculator. Please enter your calculation.");
            Console.WriteLine();

            decimal operand1 = ReadNumber("Number 1");
            Operator @operator = ReadOperator("Operator");
            decimal operand2 = ReadNumber("Number 2");

            decimal? result = @operator switch
            {
                Operator.Add => RunAdd(operand1, operand2),
                Operator.Multiply => RunMultiply(operand1, operand2),
                _ => LogNonExistingOperator(@operator)
            };

            if (result.HasValue)
            {
                Console.WriteLine();
                Console.WriteLine("Result: " + result.Value.ToString(CultureInfo.InvariantCulture));
            }

            static decimal? LogNonExistingOperator(Operator @operator)
            {
                Console.WriteLine();
                Console.WriteLine($"ERROR: Operator '{@operator}' is not yet implemented.");
                return null;
            }
        }

        [Pure]
        private static decimal RunAdd(decimal operand1, decimal operand2)
        {
            return operand1 + operand2;
        }

        [Pure]
        private static decimal RunMultiply(decimal operand1, decimal operand2)
        {
            return operand1 - operand2;
        }

        [Pure]
        private static decimal ReadNumber([NotNull] string prompt)
        {
            while (true)
            {
                Console.Write($"{prompt}: ");
                string line = Console.ReadLine();
                if (decimal.TryParse(line, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal number))
                {
                    return number;
                }

                Console.WriteLine($"ERROR: '{line}' is not a number.");
            }
        }

        [Pure]
        private static Operator ReadOperator([NotNull] string prompt)
        {
            while (true)
            {
                Console.Write($"{prompt}: ");
                string line = Console.ReadLine();

                Operator? @operator = line?.Trim() switch
                {
                    "+" => Operator.Add,
                    "-" => Operator.Subtract,
                    "*" => Operator.Multiply,
                    "/" => Operator.Divide,
                    _ => null
                };

                if (@operator.HasValue)
                {
                    return @operator.Value;
                }

                Console.WriteLine($"ERROR: '{line}' is not a valid operation. Only + - * / are supported.");
            }
        }

    }
}
