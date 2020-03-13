using System;
using System.Collections.Generic;
using System.Text;
using Plugin.FilePicker;

namespace Bluetooth_Xamarin_Client
{
    class PickedFile
    {
        private static Plugin.FilePicker.Abstractions.FileData pickedFile;

        public static Plugin.FilePicker.Abstractions.FileData PickedFile_Selected
        {
            get { return pickedFile; }
            set { pickedFile = value; }
        }
    }
}
