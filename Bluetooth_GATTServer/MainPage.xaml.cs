using Bluetooth_GATTServer_Common; //GattServer.cs
using Bluetooth_Common; //GattCharacteristicIdentifier.cs
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Bluetooth_GATTServer
{
    public sealed partial class MainPage : Page
    {
        private ILogger _logger; //All the messages showed in screen
        private IGattServer _gattServer; //All the actions captured by the gatt device (gatt server in this case)

        public MainPage()
        {
            InitializeComponent();
            InitializeLogger();
            InitializeGattServer();
        }

        private void InitializeLogger()
        {
            _logger = new ControlLogger(LogTextBox);
        }

        private void InitializeGattServer()
        {
            _gattServer = new Bluetooth_GATTServer_Common.GattServer(GattCharacteristicIdentifiers.ServiceId, _logger);//Common.GattServer(GattCharacteristicIdentifiers.ServiceId, _logger);
            _gattServer.OnChararteristicWrite += _gattServer_OnChararteristicWrite; ;
        }

        private async void _gattServer_OnChararteristicWrite(object myObject, CharacteristicEventArgs myArgs)
        {
            await _logger.LogMessageAsync($"Characteristic with Guid: {myArgs.Characteristic.ToString()} changed: {myArgs.Value.ToString()}");
        }

        private async void StartGattServer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await _gattServer.Initialize();
            }
            catch
            {
                return;
            }

            await _gattServer.AddReadWriteCharacteristicAsync(GattCharacteristicIdentifiers.DataExchange, "Data exchange");
            //NOTE : In case you want other characteristics, you must unlock the following variables.
            //       Also the "GattCharacteristicIdentifiers.cs" variables
            ///await _gattServer.AddReadCharacteristicAsync(GattCharacteristicIdentifiers.FirmwareVersion, "1.0.0.1", "Firmware Version");
            ///await _gattServer.AddWriteCharacteristicAsync(GattCharacteristicIdentifiers.InitData, "Init info");
            ///await _gattServer.AddReadCharacteristicAsync(GattCharacteristicIdentifiers.ManufacturerName, "Jenx.si", "Manufacturer");

            _gattServer.Start();
        }

        private void StopGattServer_Click(object sender, RoutedEventArgs e)
        {
            _gattServer.Stop();
        }
    }
}
