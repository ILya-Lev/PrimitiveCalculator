using System;
using System.Collections.Generic;
using System.Linq;

namespace Calculator
{
    public interface IMenu
    {
        int SelectOneOperationIndex(IReadOnlyList<string> operationNames);
        void DisplayResult(double result);
        (double lhs, double rhs) GetOperands();
    }

    public class Menu : IMenu
    {
        public int SelectOneOperationIndex(IReadOnlyList<string> operationNames)
        {
            Console.Clear();
            var message = $@"
Please select an operation entering its number:
{string.Join(Environment.NewLine, operationNames.Select((n, idx) => $"{idx:D2}. {n}"))}
For example: {operationNames.Count / 2}
";
            Console.WriteLine(message);

            var input = Console.ReadKey().KeyChar.ToString();

            Console.WriteLine();

            if (int.TryParse(input, out var selection) && IsInRange(selection))
            {
                Console.WriteLine($"{operationNames[selection]} is selected");
                return selection;
            }
            return SelectOneOperationIndex(operationNames);

            bool IsInRange(in int s) => s >= 0 && s < operationNames.Count;
        }

        public void DisplayResult(double result) => Console.WriteLine($"The result is: {result}");

        public (double lhs, double rhs) GetOperands() => (GetNumber("first"), GetNumber("second"));

        private static double GetNumber(string name)
        {
            string input = null;
            double number;
            do
            {
                Console.WriteLine($"Please enter the {name} number: ");
                input = Console.ReadLine();
            } while (!double.TryParse(input, out number));

            return number;
        }
    }
}
