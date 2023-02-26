namespace ClickerLogic
{
    public interface ISaveService
    {
        void Save<T>(T data, string fileName);
        T Load<T>(string fileName);
        bool IsFileExists(string fileName);
    }
}
