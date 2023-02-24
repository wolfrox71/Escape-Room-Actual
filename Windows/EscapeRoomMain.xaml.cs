using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using Escape_Room.Global;
using Escape_Room.Windows;

namespace Escape_Room.Windows
{
    /// <summary>
    /// Interaction logic for EscapeRoomMain.xaml
    /// </summary>
    public partial class EscapeRoomMain : Window
    {
        Game game;
        int numberOfKeysNeeded = 2;
        public EscapeRoomMain(ref Game E_game)
        {
            InitializeComponent();
            game = E_game;

            int x = 20;
            // go through each item in the inventory
            foreach (var key in game.Inventory.Values)
           {
                // and add it to the screen
                Item item = new Item(_Canvas, x, 10, 10, 10);
                item.updateColour(key.colour);
                x += 20;

           }

            //if (!game.loggedIn) { game.logginWindow(this); }
        }

        private void SpaceInvaders_Click(object sender, RoutedEventArgs e)
        {
            game.redirect(new Space_Invaders.SpaceInvadersMain(ref game), this, false);
        }

        private void DoorBtn_Click(object sender, RoutedEventArgs e)
        {
            // when the door is clicked
            if (game.Inventory.Keys.Count < numberOfKeysNeeded)
            {
                MessageBox.Show($"You need {numberOfKeysNeeded} key{(numberOfKeysNeeded == 1 ? "" : "s")} to open this door");
                return;
            }
            game.timeAlive.Stop();
            MessageBox.Show($"You escaped in {game.timeAliveToSec()} seconds");
            this.Close();
            return;
        }

        private void MathsQuizBtn_Click(object sender, RoutedEventArgs e)
        {
            game.redirect(new Maths_Quiz.MathsQuizMain(ref game), this, false);
        }

        private void MasterMindBtn_Click(object sender, RoutedEventArgs e)
        {
            game.redirect(new MasterMind.MasterMindMain(ref game), this, false);
        }
    }
}
