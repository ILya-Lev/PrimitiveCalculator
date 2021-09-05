using System;
using System.Collections.Generic;
using System.Linq;

namespace Calculator
{
    internal interface IOperationRunner
    {
        IReadOnlyList<string> GetAllOperationNames();
        double Calculate(int operationIndex, double lhs, double rhs);
    }

    internal class OperationRunner : IOperationRunner
    {
        private readonly IReadOnlyList<ICalculatingOperation> _operations;

        public OperationRunner(IEnumerable<ICalculatingOperation> operations)
        {
            _operations = operations?.ToArray() ?? throw new ArgumentNullException(nameof(operations));
        }

        public IReadOnlyList<string> GetAllOperationNames() => _operations
            .Select(op => op.GetType().Name)
            .ToArray();

        public double Calculate(int operationIndex, double lhs, double rhs) => 
            GetOperation(operationIndex)
                .Calculate(lhs, rhs);

        private ICalculatingOperation GetOperation(int index) => _operations[index];
    }
}
