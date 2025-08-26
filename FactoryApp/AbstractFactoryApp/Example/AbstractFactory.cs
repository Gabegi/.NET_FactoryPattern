using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FactoryApp.Example
{
    // 1. Product Interface
    public interface IHotDrink
    {
        void Consume();
    }

    // 2. Concrete Products
    public class Tea : IHotDrink
    {
        public void Consume() =>
            Console.WriteLine("Enjoying a calming cup of tea.");
    }

    public class Coffee : IHotDrink
    {
        public void Consume() =>
            Console.WriteLine("Sipping a strong cup of coffee.");
    }

    // 3. Factory Interface
    public interface IHotDrinkFactory
    {
        IHotDrink Prepare(int amount);
    }

    // 4. Concrete Factories
    public class TeaFactory : IHotDrinkFactory
    {
        public IHotDrink Prepare(int amount)
        {
            Console.WriteLine($"Preparing {amount}ml of tea...");
            return new Tea();
        }
    }

    public class CoffeeFactory : IHotDrinkFactory
    {
        public IHotDrink Prepare(int amount)
        {
            Console.WriteLine($"Brewing {amount}ml of coffee...");
            return new Coffee();
        }
    }

    // 5. Machine to Manage Factories
    public class HotDrinkMachine
    {
        private readonly Dictionary<string, IHotDrinkFactory> _factories = new();

        public HotDrinkMachine()
        {
            // Automatically register all factories implementing IHotDrinkFactory
            var factoryTypes = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => typeof(IHotDrinkFactory).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

            foreach (var type in factoryTypes)
            {
                var drinkName = type.Name.Replace("Factory", ""); // e.g., TeaFactory → Tea
                _factories[drinkName] = (IHotDrinkFactory)Activator.CreateInstance(type);
            }
        }

        public IHotDrink MakeDrink(string drink, int amount)
        {
            if (!_factories.TryGetValue(drink, out var factory))
                throw new ArgumentException($"Drink '{drink}' is not available.");

            return factory.Prepare(amount);
        }

        public IEnumerable<string> ListAvailableDrinks() => _factories.Keys;
    }

    // 6. Example Usage
    public class Program
    {
        public static void Main()
        {
            var machine = new HotDrinkMachine();

            Console.WriteLine("Available drinks:");
            foreach (var drink in machine.ListAvailableDrinks())
                Console.WriteLine($"- {drink}");

            var selectedDrink = "Tea";
            var drinkObj = machine.MakeDrink(selectedDrink, 250);
            drinkObj.Consume();
        }
    }
}
