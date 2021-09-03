using System;

namespace Calculator
{
    internal interface ICalculatingOperation
    {
        double Calculate(double lhs, double rhs);
    }

    internal class Add : ICalculatingOperation
    {
        public double Calculate(double lhs, double rhs) => lhs + rhs;
    }

    internal class Subtract : ICalculatingOperation
    {
        public double Calculate(double lhs, double rhs) => lhs - rhs;
    }

    internal class Multiply : ICalculatingOperation
    {
        public double Calculate(double lhs, double rhs) => lhs * rhs;
    }

    internal class Divide : ICalculatingOperation
    {
        public double Calculate(double lhs, double rhs) => rhs == 0.0 ? double.NaN : lhs / rhs;
    }

    //internal class Power : ICalculatingOperation
    //{
    //    public double Calculate(double aBase, double power) => Math.Pow(aBase, power);
    //}

    //internal class Root : ICalculatingOperation
    //{
    //    public double Calculate(double number, double power) => power == 0.0 
    //        ? double.NaN 
    //        : Math.Pow(number, 1 / power);
    //}
}