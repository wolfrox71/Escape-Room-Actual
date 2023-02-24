using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Escape_Room.Global.DataTypes
{
    public class DoorKey
    {
        public readonly SolidColorBrush colour;
        public bool canUse = true;
        public DoorKey(SolidColorBrush E_colour)
        {
            colour = E_colour;
        }

    }
}
