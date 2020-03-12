using Bluetooth_GATTServer_Common;
using System;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;

namespace Bluetooth_GATTServer
{
    public class ControlLogger : ILogger
    {
        private readonly TextBlock _textBox;

        public ControlLogger(TextBlock textBox)
        {
            _textBox = textBox;
        }

        public async Task LogMessageAsync(string message)
        {
            await _textBox.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                _textBox.Text += message + Environment.NewLine;
            });
        }
    }
}