using Microsoft.Extensions.Options;
using System;

namespace Calculator
{
    internal class PrimitiveCalculator
    {
        private readonly IMenu _menu;
        private readonly IOperationRunner _operationRunner;
        private readonly IOptions<DisplayOptions> _config;

        public PrimitiveCalculator(
            IMenu menu
            , IOperationRunner operationRunner
            , IOptions<DisplayOptions> config)
        {
            _menu = menu;
            _operationRunner = operationRunner;
            _config = config;
        }

        public void Run()
        {
            var operationNames = _operationRunner.GetAllOperationNames();
            
            var operationIndex = _menu.SelectOneOperationIndex(operationNames);
            var (lhs, rhs) = _menu.GetOperands();

            var result = _operationRunner.Calculate(operationIndex, lhs, rhs);
            var roundedResult = Math.Round(result, _config.Value.DecimalDigits);

            _menu.DisplayResult(roundedResult);
        }
    }
}