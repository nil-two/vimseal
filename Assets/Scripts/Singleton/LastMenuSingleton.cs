namespace Singleton
{
    public class LastMenuSingleton
    {
        private static LastMenuSingleton _instance;
        public int Index { get; set; }
        
        public static LastMenuSingleton GetInstance()
        {
            _instance ??= new LastMenuSingleton();
            return _instance;
        }
    }
}