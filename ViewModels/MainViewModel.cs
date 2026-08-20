using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Input;
using MockBench.Models;
using MockBench.Services;

namespace MockBench.ViewModels
{
    public class MainViewModel
    {
        public ObservableCollection<PacketEntry> Log { get; } = [];

        private readonly UdpTransport _transport;

        public string TargetIp { get; set; } = "192.164.1.42";
        public int TargetPort { get; set; } = 9990;
        public string HexInput { get; set; } = string.Empty;
        public string LabelInput { get; set; } = string.Empty;

        public ICommand SendCommand { get; }

        /// <summary>
        /// Main runtime loop for GUI view
        /// </summary>
        public MainViewModel()
        {
            _transport = new UdpTransport(9990);
            _transport.PacketReceived += OnPacketReceived; // += is a subscribe call
            _transport.StartListening();

            SendCommand = new RelayCommand(async () => await SendAsync());
        }

        private async Task SendAsync()
        {
            var bytes = ParseHex(HexInput);
            await _transport.SendAsync(bytes, TargetIp, TargetPort, LabelInput);
        }

        private void OnPacketReceived(PacketEntry entry)
        {
            Application.Current.Dispatcher.Invoke(() => Log.Add(entry));
        }

        /// <summary>
        /// Helper to turn hex input into a byte[]
        /// </summary>
        /// <param name="hex"></param>
        /// <returns></returns>
        private byte[] ParseHex(string hex)
        {
            var parts = hex.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var bytes = new byte[parts.Length];
            for (int i = 0; i < parts.Length; i++)
                bytes[i] = Convert.ToByte(parts[i], 16);
            return bytes;
        }
    }
}
