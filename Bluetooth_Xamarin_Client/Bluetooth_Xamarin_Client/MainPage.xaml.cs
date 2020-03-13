using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Plugin.FilePicker; //Added from here
using System.IO;
using Xamarin.Forms.Xaml;
using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions; //To here
//UTType List : https://developer.apple.com/library/archive/documentation/Miscellaneous/Reference/UTIRef/Articles/System-DeclaredUniformTypeIdentifiers.html


namespace Bluetooth_Xamarin_Client
{
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        private Plugin.FilePicker.Abstractions.FileData pickedFile;
        public MainPage()
        {
            InitializeComponent();
        }

        public Plugin.FilePicker.Abstractions.FileData SelectedFile()
        {
            var _selectedFile = pickedFile;
            return _selectedFile;
        }

        private async void PickFile_Clicked(object sender, EventArgs args)
        {
            string[] fileTypes = null;

            if (Device.RuntimePlatform == Device.Android)
            {
                fileTypes = new string[] { "files/json", "image/jpeg", "application/zip" };
            }

            if (Device.RuntimePlatform == Device.iOS)
            {
                fileTypes = new string[] { "public.json", "public.jpeg", "com.pkware.zip-archive" }; // same as iOS constant UTType.Image
            }

            pickedFile = await CrossFilePicker.Current.PickFile(fileTypes);
            if (pickedFile != null)
            {
                SelectedFileLabel.Text = pickedFile.FileName;
                SendButton.IsVisible = true;
            }
        }

        private async void SendFileButton_Clicked(object sender, EventArgs e)
        {
            
            
            await Navigation.PushAsync(new BluetoothDevicePage());
        }
    }
}
