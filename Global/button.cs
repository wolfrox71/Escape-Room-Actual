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
    public class button
    {
        public SolidColorBrush colour;
        public Canvas _Canvas { get; protected set; }
        public double x { get; protected set; }
        public double y { get; protected set; }
        public int width { get; protected set; }
        public int height { get; protected set; }

        public Button _Btn;

        // default width and height values
        protected int _Width = 50;
        protected int _Height = 50;
        public button(Canvas E_canvas, double E_x, double E_y, String E_Content)
        {
            _Canvas = E_canvas;
            x = E_x;
            y = E_y;
            width = _Width;
            height = _Height;

            _Btn = new Button
            {
                Width = width,
                Height = height,
                Content = E_Content,
            };
            _Canvas.Children.Add(_Btn);
            Draw();
            
        }

        public void Draw()
        {
            Canvas.SetTop(_Btn, y);
            Canvas.SetLeft(_Btn, x);
        }
        
    }
}
