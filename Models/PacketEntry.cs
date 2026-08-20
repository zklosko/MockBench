using System;
using System.Collections.Generic;
using System.Text;

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

        public PacketEntry(Direction direction, byte[] bytes, string? label = null)
        {
            Dir = direction;
            Bytes = bytes;
            Label = label;
            Timestamp = DateTime.Now;
        }
    }
}
