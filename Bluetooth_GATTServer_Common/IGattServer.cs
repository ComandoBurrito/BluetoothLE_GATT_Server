using System;
using System.Threading.Tasks;
using static Bluetooth_GATTServer_Common.GattServer;

namespace Bluetooth_GATTServer_Common
{
    public interface IGattServer
    {
        event GattChararteristicHandler OnChararteristicWrite;
        Task Initialize();
        Task<bool> AddReadCharacteristicAsync(Guid characteristicId, string characteristicValue, string userDescription);
        Task<bool> AddWriteCharacteristicAsync(Guid characteristicId, string userDescription);
        Task<bool> AddReadWriteCharacteristicAsync(Guid characteristicId, string userDescription);
        void Start();
        void Stop();
    }
}