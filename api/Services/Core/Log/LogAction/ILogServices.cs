using Common;
using System.Threading.Tasks;
namespace Services.Core.Interfaces
{
    public interface ILogServices
    {
        Task WriteLogAction (string path, string method, string? body = null);
        Task WriteLogException(string function, string message, string stackTrace);
    }
}