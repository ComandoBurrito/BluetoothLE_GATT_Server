using System.Threading.Tasks;

namespace Bluetooth_GATTServer_Common
{
    public interface ILogger
    {
        Task LogMessageAsync(string message);
    }
}