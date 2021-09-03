using System;
using System.Collections.Generic;
using System.Linq;

namespace Calculator
{
    internal interface IOperationsProvider
    {
        IReadOnlyList<string> GetAllOperationNames();
        ICalculatingOperation GetOperation(int index);
    }

    internal class OperationsProvider : IOperationsProvider
    {
        private readonly IReadOnlyList<ICalculatingOperation> _operations;

        public OperationsProvider(IEnumerable<ICalculatingOperation> operations)
        {
            _operations = operations?.ToArray() ?? throw new ArgumentNullException(nameof(operations));
        }

        public IReadOnlyList<string> GetAllOperationNames() => _operations
            .Select(op => op.GetType().Name)
            .ToArray();

        public ICalculatingOperation GetOperation(int index) => _operations[index];
    }
}
