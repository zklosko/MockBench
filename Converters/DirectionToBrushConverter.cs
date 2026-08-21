using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using MockBench.Models;

namespace MockBench.Converters
{
    public class DirectionToBrushConverter : IValueConverter
    {
        private static readonly SolidColorBrush TxBrush = new(Color.FromRgb(0x4A, 0x90, 0xD9)); // Blue
        private static readonly SolidColorBrush RxBrush = new(Color.FromRgb(0x5F, 0xB0, 0xA3)); // Teal

        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is PacketEntry.Direction dir)
                return dir == PacketEntry.Direction.TX ? TxBrush : RxBrush;

            return Brushes.Gray;
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
