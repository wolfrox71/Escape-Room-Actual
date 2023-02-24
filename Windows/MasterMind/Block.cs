using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Diagnostics;
namespace Escape_Room.Windows.MasterMind
{
    public class Block
    {
        public SolidColorBrush colour;
        public Canvas _Canvas { get; protected set; }
        public double x { get; protected set; }
        public double y { get; protected set; }
        public int width { get; protected set; }
        public int height { get; protected set; }

        public Rectangle _Rect;

        // default width and height values
        protected int _Width = 10;
        protected int _Height = 10;
        public Block(Canvas E_canvas, double E_x, double E_y, SolidColorBrush colour)
        {
            _Canvas = E_canvas;
            x = E_x;
            y = E_y;
            width = _Width;
            height = _Height;

            _Rect = new Rectangle
            {
                Width = width,
                Height = height,
            };
            _Canvas.Children.Add(_Rect);
            Draw();
            updateColour(colour);
        }

        public void Draw()
        {
            Canvas.SetTop(_Rect, y);
            Canvas.SetLeft(_Rect, x);
        }
        public void updateColour(SolidColorBrush newColour)
        {
            colour = newColour;
            _Rect.Fill = colour;
        }
    }
}
