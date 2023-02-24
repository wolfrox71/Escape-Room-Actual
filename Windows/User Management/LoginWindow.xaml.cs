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
using Escape_Room.Global.DataTypes;
using Escape_Room.Windows;

namespace Escape_Room.User_Management
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        Game game;
        public LoginWindow(Game E_game)
        {
            InitializeComponent();
            game = E_game;
            // if the user has already logged in
            if (game.loggedIn)
            {
                // if there is a previous page
                game.redirect(new EscapeRoomMain(ref game), this, true);
            }
        }

        private void LogginButton_Click(object sender, RoutedEventArgs e)
        {
            Account entered_Account =  new Account(Username_Box.Text, Password_Box.Password);
            // if the entered account was an accepted account
            if (game.validAccount(entered_Account))
            {
                // set that account as the current user
                game.currentAccount = entered_Account;
            }
            else
            {
                ErrorBox.Text = "Invalid Details";
                // if it wasnt a valid account
                // exit the function
                return;
            }
            // show the current account
            
            game.redirect(new EscapeRoomMain(ref game),this);
            
        }
    }
}
