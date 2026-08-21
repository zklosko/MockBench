using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace MockBench.Models
{
    public class PacketEntry
    {
        public enum Direction { RX, TX}

        public Direction Dir { get; set; } = Direction.RX;
        public DateTime Timestamp { get; set; }
        public byte[] Bytes { get; set; } = Array.Empty<byte>();
        public string? Label { get; set; }

        public string MessageHex => BitConverter.ToString(Bytes).Replace("-", " ");
        public string MessageAscii => System.Text.Encoding.ASCII.GetString(Bytes);

        [JsonConstructor]
        public PacketEntry(Direction dir, byte[] bytes, DateTime timestamp, string? label = null)
        {
            Dir = dir;
            Bytes = bytes;
            Label = label;
            Timestamp = timestamp;
        }

        public PacketEntry(Direction dir, byte[] bytes, string? label = null)
            : this(dir, bytes, DateTime.Now, label)
        {
        }
    }
}
