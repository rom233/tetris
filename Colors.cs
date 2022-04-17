using System.Collections.Generic;
using System.Drawing;

namespace tetris
{
    public class Colors
    {
        public static Brush BORDER_COLOR = new SolidBrush(Color.FromArgb(52, 58, 64));

        public static Color BUTTON_NORMAL = Color.FromArgb(73, 80, 87);
        public static Color BUTTON_HOVER  = Color.FromArgb(84, 91, 98);

        private static readonly List<Color> shape_colors = new List<Color>()
        {
            Color.FromArgb(220, 53, 69),
            Color.FromArgb(40, 167, 69),
            Color.FromArgb(111, 66, 193),
            Color.FromArgb(253, 126, 20),
            Color.FromArgb(0, 123, 255),
            Color.FromArgb(23, 162, 184),
            Color.FromArgb(232, 62, 140),
            Color.FromArgb(255, 192, 7)
        };

        public static Brush GetColor(int id) => new SolidBrush(shape_colors[id - 1]);
    }
}
