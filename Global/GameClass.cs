using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Diagnostics;

using Escape_Room.Windows;
using Escape_Room.User_Management;
using Escape_Room.Global.DataTypes;

namespace Escape_Room.Global
{
    public class Game
    {
        public Dictionary<string, DoorKey> Inventory = new Dictionary<string, DoorKey>();

        public bool loggedIn { get { return !(currentAccount is null); } }
        public Window previousWindow;
        public Account currentAccount;

        protected int redirect_count = 0;
        protected int max_redirects = 5;

        public Stopwatch timeAlive = new Stopwatch();

        protected List<Account> accounts { get; set; }
        = new List<Account>
        {
            new Account("Jamie", "56C6EAFBE65D79069D6AC79E7560EB42FA7A941E8AC3C0FA259BDA2FBD2EE5DB", false),
            new Account("",""),
        };
        public Game()
        {
            timeAlive.Start();
        }

        public int timeAliveToSec()
        {
            return (int)timeAlive.Elapsed.TotalSeconds;
        }

        public bool validAccount(Account E_Account)
        {
            // go through each account
            foreach (Account account in accounts)
            {
                // if that account is the account entered
                if (E_Account == account)
                {
                    // return true as the given account was valid
                    return true;
                }
            }
            // return false as non of the accounts were valid
            return false;
        }
        public void addAccount(Account E_account)
        {
            // add the account to the list of accounts
            
            // if the account is a duplicate dont add it
            if (validAccount(E_account)) { return; }
            // if it is unique add it
            accounts.Add(E_account);
        }
        public void redirect(Window address, Window current_Window, bool auto_redirect = false)
        {
            if (auto_redirect)
            {
                redirect_count++;
            }
            
            // if the user has auto redirected to many times
            if (redirect_count > max_redirects)
            {
                // send them to the loggin window
                logginWindow(current_Window);
            }

            address.Show();
            current_Window.Close();
        }
        public void logginWindow(Window current_window)
        {
            // create a new instance of the login window
            LoginWindow login = new LoginWindow(this);
            // dont have a previous window
            previousWindow = null;
            // show the login window
            login.Show();
            // close the currently in use window
            current_window.Close();
        }
    }
}
