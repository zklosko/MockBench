using System;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using MockBench.Models;

namespace MockBench.Services
{
    public class UdpTransport
    {
        private readonly UdpClient _udpClient;
        private CancellationTokenSource? _listenCts;

        public event Action<PacketEntry>? PacketReceived;
        
        /// <summary>
        /// Creates UDP server/socket
        /// </summary>
        /// <param name="port">Port server listens on</param>
        public UdpTransport(int port)
        {
            _udpClient = new UdpClient(port);

            // Windows-specific work-around for UDP sockets
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                const int SIO_UDP_CONNRESET = -1744830452;
                _udpClient.Client.IOControl((IOControlCode)SIO_UDP_CONNRESET, new byte[] { 0 }, null);
            }
        }

        public async Task SendAsync(byte[] data, string ip, int port, string? label = null)
        {
            var endpoint = new IPEndPoint(IPAddress.Parse(ip), port);
            await _udpClient.SendAsync(data, data.Length, endpoint);

            PacketReceived?.Invoke(new PacketEntry(PacketEntry.Direction.TX, data, label));
        }

        public void StartListening()
        {
            _listenCts = new CancellationTokenSource();
            _ = ListenLoopAsync(_listenCts.Token);
        }
        
        public void StopListening()
        {
            _listenCts?.Cancel();
        }

        /// <summary>
        /// Main UDP server process loop
        /// </summary>
        /// <returns></returns>
        public async Task ListenLoopAsync(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                try
                {
                    var result = await _udpClient.ReceiveAsync(token);
                    var entry = new PacketEntry(PacketEntry.Direction.RX, result.Buffer);
                    PacketReceived?.Invoke(entry);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error handling packet: {e.Message}");
                }
            }
        }
    }
}
