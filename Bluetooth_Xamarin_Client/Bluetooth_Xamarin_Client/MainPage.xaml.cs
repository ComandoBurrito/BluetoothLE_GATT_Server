using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Plugin.FilePicker; //Added
using System.IO; //Added
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

            //await PickAndProcessFile(fileTypes); //File Picker Menu
            pickedFile = await CrossFilePicker.Current.PickFile(fileTypes);
            //string selectedFilePath;
            if (pickedFile != null)
            {
                SelectedFileLabel.Text = pickedFile.FileName;
                //selectedFilePath = pickedFile.FilePath;
                SendButton.IsVisible = true;
            }
        }

        private async void SendFileButton_Clicked(object sender, EventArgs e)
        {
            
            
            await Navigation.PushAsync(new BluetoothDevicePage());
        }



        /* private async Task PickAndProcessFile(string[] fileTypes)
         {
             try
             {
                 //var pickedFile = await CrossFilePicker.Current.PickFile(fileTypes);
                 string selectedFilePath, stringSourceFile;
                 if (pickedFile != null)
                 {
                     SelectedFileLabel.Text = pickedFile.FileName;
                     selectedFilePath = pickedFile.FilePath;

                     if (pickedFile.FileName.EndsWith("json") || pickedFile.FileName.EndsWith("jpeg", StringComparison.OrdinalIgnoreCase)
                        || pickedFile.FileName.EndsWith("png", StringComparison.OrdinalIgnoreCase) || pickedFile.FileName.EndsWith("zip"));
                     {
                         stringSourceFile = FileToBase64(selectedFilePath);
                         splitPorcess(stringSourceFile);
                     }

                 }
             }
             catch(Exception ex)
             {
                 SelectedFileLabel.Text = ex.ToString();
             }
         }*/



        /// <summary>
        /// CONVERSION FUNCTIONS
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        /*public string FileToBase64(string path)
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

        private void splitProcess(string str512)
        {
            int chunckSize = 512;
            int stringLength = str512.Length;

        }*/

    }
}
