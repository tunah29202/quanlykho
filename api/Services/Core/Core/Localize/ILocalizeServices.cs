namespace Services.Core.Interfaces
{
    public interface ILocalizeServices
    {
        string Get(string module, string screen, string key);
        Task<String> GetAsync(string module, string screen, string key);
    }
}