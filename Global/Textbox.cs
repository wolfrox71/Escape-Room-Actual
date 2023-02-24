using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Diagnostics;
namespace Escape_Room.Global
{
    public class Textbox
    {
        public SolidColorBrush colour;
        public Canvas _Canvas { get; protected set; }
        public double x { get; protected set; }
        public double y { get; protected set; }
        public int width { get; protected set; }
        public int height { get; protected set; }

        public TextBlock _Box;

        // default width and height values
        protected int _Width = 50;
        protected int _Height = 50;
        public Textbox(Canvas E_canvas, double E_x, double E_y, String E_Content)
        {
            _Canvas = E_canvas;
            x = E_x;
            y = E_y;
            width = _Width;
            height = _Height;

            _Box = new TextBlock
            {
                Width = width,
                Height = height,
                Text = E_Content,
            };
            _Canvas.Children.Add(_Box);
            Draw();

        }

        public void Draw()
        {
            Canvas.SetTop(_Box, y);
            Canvas.SetLeft(_Box, x);
        }

    }
}
