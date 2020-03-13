using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System.IO;

namespace Bluetooth_Xamarin_Client
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BluetoothDevicePage : ContentPage
    {
        private readonly IAdapter _bluetoothAdapter;
        private List<IDevice> _gattDevices = new List<IDevice>();

        private readonly Plugin.FilePicker.Abstractions.FileData _pickedFile; //Para recuperar
        public Plugin.FilePicker.Abstractions.FileData pickedFile;

        public BluetoothDevicePage() //Stream selectedFile
        {
            InitializeComponent();
            
            _bluetoothAdapter = CrossBluetoothLE.Current.Adapter;
            _bluetoothAdapter.DeviceDiscovered += (s, a) =>
            {
                _gattDevices.Add(a.Device);
            };

        }

        /// <summary>
        /// SCAN BUTTON
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void ScanButton_Clicked(object sender, EventArgs e)
        {
            ScanButton.IsEnabled = false;

            var locationPermissionStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Location);

            if (locationPermissionStatus != PermissionStatus.Granted)
            {
                if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Location))
                {
                    await DisplayAlert("Permission required", "Application needs location permission", "OK");
                }

                var requestLocationPermissionStatus = await CrossPermissions.Current.RequestPermissionsAsync(new[] { Permission.Location });

                var locationPermissionPersent = requestLocationPermissionStatus.FirstOrDefault(x => x.Key == Permission.Location);
                if (locationPermissionPersent.Value != null)
                    if (locationPermissionPersent.Value == PermissionStatus.Granted) locationPermissionStatus = PermissionStatus.Granted;
            }

            if (locationPermissionStatus != PermissionStatus.Granted)
            {
                await DisplayAlert("Permission required", "Application needs location permission", "OK");
                return;
            }

            _gattDevices.Clear();
            await _bluetoothAdapter.StartScanningForDevicesAsync();
            listView.ItemsSource = _gattDevices.ToArray();
            
            //Falta mostar los que no tiene nombre como "N/A" y no solo ""
            
            ScanButton.IsEnabled = true;
        }

        /// <summary>
        /// FOUND BLUETOOTH - SELECT ITEM IN THE LISTVIEW, THEN SEND THE SELECTED FILE
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void FoundBluetoothDevicesListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            IDevice selectedItem = e.SelectedItem as IDevice;
            MainPage selectedFile = new MainPage(); //'selectedFile' es la var que toma de la clase Main
            pickedFile = selectedFile.SelectedFile(); //Dentro de Main class se encuentra este metodo

            if (selectedItem.State == Plugin.BLE.Abstractions.DeviceState.Connected) //If the Client is already connected
            {
                await PickAndSendFile(pickedFile, selectedItem);                                       //Send the selected file
                //await Navigation.PushAsync(new BluetoothDataPage(selectedItem)); 
            }
            else //If it's not connected yet
            {
                try
                {
                    await _bluetoothAdapter.ConnectToDeviceAsync(selectedItem);      //Connect to the selected device
                    await PickAndSendFile(pickedFile, selectedItem);                                   //And then send the selected file
                    //await Navigation.PushAsync(new BluetoothDataPage(selectedItem)); 
                }
                catch
                {
                    // ... could not connect to device
                    //Idea for improvement : A message box saying "Could not connect to the selected device, try again"
                }
            }
        }

        private async Task PickAndSendFile(Plugin.FilePicker.Abstractions.FileData pickedFile, IDevice _connectedDevice)
        {
            try
            {
                string selectedFilePath , strSourceFile;
                if (pickedFile != null) //Double verification
                {
                    selectedFilePath = pickedFile.FilePath;
                    if (pickedFile.FileName.EndsWith("json") || pickedFile.FileName.EndsWith("jpeg", StringComparison.OrdinalIgnoreCase)
                        || pickedFile.FileName.EndsWith("png", StringComparison.OrdinalIgnoreCase) || pickedFile.FileName.EndsWith("zip")) ;
                    {
                        strSourceFile = FileToBase64(selectedFilePath);
                        
                        var service = await _connectedDevice.GetServiceAsync(GattCharacteristicIdentifiers.ServiceId);
                        if (service != null)
                        {
                            var characteristic = await service.GetCharacteristicAsync(GattCharacteristicIdentifiers.DataExchange); //Send to the DataExchange identifier
                            if (characteristic != null)
                            {
                                splitProcess(strSourceFile, characteristic);
                            }
                        }
                        //splitPorcess(strSourceFile);
                    }
                }
            }
            catch
            {

            }
        }

        /// <summary>
        /// CONVERSION FUNCTIONS
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public string FileToBase64(string path)
        {
            //Provide read acces to the file
            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            //Create a byte array of the file stream length
            byte[] Data = new byte[fs.Length];
            //Read block of bytes from stream into the byte array
            fs.Read(Data, 0, System.Convert.ToInt32(fs.Length));
            //Close the File Stream
            fs.Close();
            string base64String = Convert.ToBase64String(Data);
            return base64String;
        }

        private async Task splitProcess(string str512, ICharacteristic _characteristic)
        {
            int chunkSize = 512;
            int stringLength = str512.Length;

            byte[] senddata = Encoding.UTF8.GetBytes("start");
            var bytes = await _characteristic.WriteAsync(senddata);
            //receiveStr("start");

            for (int i = 0; i < stringLength; i += chunkSize)
            {
                if (i + chunkSize > stringLength)
                {
                    chunkSize = stringLength - i;
                }
                senddata = Encoding.UTF8.GetBytes(str512.Substring(i, chunkSize));
                bytes = await _characteristic.WriteAsync(senddata);
                //receiveStr(str.512.Substring(i, chunkSize));
            }
            //receiveStr("end");
        }

        
        /*/// <summary>
        /// Gatt server string Reception Process [iOS OFFLINE PROTOTYPE]
        /// ====== Only reconstruction so far ======
        /// </summary>
        /// <param name="recepStr"></param>
        //14h17 : Hace falta agregar un reseteo al "strConstruct.Append" al final de la funcion
        //15h15 : Fixed, utilizando .Clear();
        StringBuilder strConstruct = new System.Text.StringBuilder(); //Probablemente aqui se deba de poner 
        string fileString; //Resultado de concatenarlo todo
        bool currentState; //Booleano que me permititra seguir concatenando/*
        private void receiveStr(string recepStr)
        {
            if (recepStr == "start")
            {
                currentState = true;
            }
            else if (recepStr == "end")
            {
                currentState = false;
            }

            if (currentState == true) //Proceso normal
            {
                if (recepStr != "start")
                {
                    strConstruct.Append(recepStr); //Inserta un pedazo mas al string buffer
                }
            }
            else if (currentState == false)
            {
                fileString = strConstruct.ToString(); //Se iguala el string final al string buffer
                strConstruct.Clear(); //Se vacia el string buffer. == fileString recepta toda el contenido al final ==
            }
        }*/


    }
}