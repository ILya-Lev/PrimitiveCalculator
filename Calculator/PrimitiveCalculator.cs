using Microsoft.Extensions.Options;
using System;

namespace Calculator
{
    internal class PrimitiveCalculator
    {
        private readonly IMenu _menu;
        private readonly IOperationsProvider _operationsProvider;
        private readonly IOptions<DisplayOptions> _config;

        public PrimitiveCalculator(
            IMenu menu
            , IOperationsProvider operationsProvider
            , IOptions<DisplayOptions> config)
        {
            _menu = menu;
            _operationsProvider = operationsProvider;
            _config = config;
        }

        public void Run()
        {
            var operationNames = _operationsProvider.GetAllOperationNames();
            
            var operationIndex = _menu.SelectOneOperationIndex(operationNames);
            var (lhs, rhs) = _menu.GetOperands();

            var result = _operationsProvider.Calculate(operationIndex, lhs, rhs);
            var roundedResult = Math.Round(result, _config.Value.DecimalDigits);

            _menu.DisplayResult(roundedResult);
        }
    }
}