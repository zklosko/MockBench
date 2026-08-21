using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;
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
        public ICommand ClearLogCommand { get; }
        public ICommand LoadSessionCommand { get; }
        public ICommand SaveSessionCommand { get; }

        /// <summary>
        /// Main runtime loop for GUI view
        /// </summary>
        public MainViewModel()
        {
            _transport = new UdpTransport(9990);
            _transport.PacketReceived += OnPacketReceived; // += is a subscribe call
            _transport.StartListening();

            SendCommand = new RelayCommand(async () => await SendAsync());
            ClearLogCommand = new RelayCommand(() => Log.Clear());
            LoadSessionCommand = new RelayCommand(LoadSession);
            SaveSessionCommand = new RelayCommand(SaveSession);
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

        private void LoadSession()
        {
            var dialog = new OpenFileDialog
            {
                Filter = "JSON files (*.json)|*.json"
            };

            if (dialog.ShowDialog() == true)
            {
                var json = File.ReadAllText(dialog.FileName);
                var entries = JsonSerializer.Deserialize<List<PacketEntry>>(json);

                if (entries != null)
                {
                    Log.Clear();
                    foreach (var entry in entries)
                        Log.Add(entry);
                }
            }
        }

        private void SaveSession()
        {
            var dialog = new SaveFileDialog
            {
                Filter = "JSON files (*.json)|*.json",
                FileName = $"session_{DateTime.Now:yyyyMMdd_HHmmss}.json"
            };
            if (dialog.ShowDialog() == true)
            {
                var json = JsonSerializer.Serialize(Log, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(dialog.FileName, json);
            }
        }
    }
}
