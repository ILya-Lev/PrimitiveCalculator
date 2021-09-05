using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System;

namespace Calculator
{
    class Program
    {
        static void Main(string[] args)
        {
            var calculator = CreatePrimitiveCalculator_Pure();
            //var calculator = CreatePrimitiveCalculator_ContainerBased();

            ConsoleKey exitKey = ConsoleKey.A;
            //by the way, one loop body is an operation or scope...
            while (exitKey != ConsoleKey.D0 && exitKey != ConsoleKey.NumPad0)
            {
                calculator.Run();

                Console.WriteLine("To exit press 0: ");
                exitKey = Console.ReadKey().Key;
            }
        }

        private static PrimitiveCalculator CreatePrimitiveCalculator_ContainerBased()
        {
            using ServiceProvider serviceProvider = BuildServiceProvider();

            var calculator = serviceProvider.GetRequiredService<PrimitiveCalculator>();
            return calculator;
        }


        /// <summary> DI-container based composition root </summary>
        private static ServiceProvider BuildServiceProvider()
        {
            #region step 1: get configuration
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
            #endregion

            #region step 2: register all components
            var serviceCollection = new ServiceCollection();

            serviceCollection.Configure<DisplayOptions>(configuration.GetSection(nameof(DisplayOptions)));

            serviceCollection.TryAddScoped<IMenu, Menu>();
            serviceCollection.TryAddSingleton<IOperationRunner, OperationRunner>();
            serviceCollection.TryAddSingleton<PrimitiveCalculator>();

            //serviceCollection.TryAddEnumerable(new[]
            //{
            //    ServiceDescriptor.Singleton<ICalculatingOperation, Add>(),
            //    ServiceDescriptor.Singleton<ICalculatingOperation, Subtract>(),
            //    ServiceDescriptor.Singleton<ICalculatingOperation, Multiply>(),
            //    ServiceDescriptor.Singleton<ICalculatingOperation, Divide>(),
            //});

            //using Scrutor https://github.com/khellang/Scrutor
            serviceCollection.Scan(scan => scan
                .FromAssemblyOf<ICalculatingOperation>()
                .AddClasses(classes => classes.AssignableTo(typeof(ICalculatingOperation)))
                .AsImplementedInterfaces()
                .WithSingletonLifetime()
            );
            #endregion

            #region step 3: build service provider
            return serviceCollection.BuildServiceProvider();
            #endregion
        }

        /// <summary> Pure DI composition root </summary>
        private static PrimitiveCalculator CreatePrimitiveCalculator_Pure()
        {
            #region step 1: get configuration
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
            #endregion

            IMenu menu = new Menu();
            var operations = new ICalculatingOperation[]
            {
                new Add(),
                new Subtract(),
                new Multiply(),
                new Divide()
            };
            IOperationRunner operationRunner = new OperationRunner(operations);

            DisplayOptions displayOptions = new();
            configuration.Bind(nameof(DisplayOptions), displayOptions);
            var displaySettings = Options.Create(displayOptions);

            var calculator = new PrimitiveCalculator(menu, operationRunner, displaySettings);

            return calculator;
        }
    }
}
