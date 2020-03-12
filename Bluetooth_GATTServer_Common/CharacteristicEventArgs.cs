using System;

namespace Bluetooth_GATTServer_Common
{
    public class CharacteristicEventArgs : EventArgs
    {
        public CharacteristicEventArgs(Guid characteristicId, object value)
        {
            Characteristic = characteristicId;
            Value = value;
        }

        public Guid Characteristic { get; set; }

        public object Value { get; set; }
    }
}