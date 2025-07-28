namespace FactoryApp.Example
{
    public class AbstractFactory
    {
        public interface IHotdrink
        {
            void consume();
        }

        internal class Tea : IHotdrink
        {
            public void consume() { }
        }

        public interface IHotdrinkFactory
        {
            IHotdrink createHotdrink();
        }
        
        internal class TeaFactory: IHotdrinkFactory
        {
            public IHotdrink createHotdrink()
            {
                return new Tea();
            }
        }

        public class HotdrinkMachine
        {
            public enum AvailableDrink
            {
                Coffe, Tea
            }

            private Dictionary<AvailableDrink, IHotdrinkFactory> factories = 
                new Dictionary<AvailableDrink, IHotdrinkFactory>();
        }
    }
}
