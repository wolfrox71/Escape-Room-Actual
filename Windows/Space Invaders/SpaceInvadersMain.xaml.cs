using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Escape_Room.Windows.Space_Invaders.Characters;
using Escape_Room.Global;
using Escape_Room.Windows;

namespace Escape_Room.Windows.Space_Invaders
{
    /// <summary>
    /// Interaction logic for SpaceInvadersMain.xaml
    /// </summary>
    /// 

    public partial class SpaceInvadersMain : Window
    {
        List<Player> players = new List<Player>();
        List<Enemy> enemys = new List<Enemy>();

        ProjectileCount projectileCount = new ProjectileCount();
        //List<Enemy> enemys = new List<Enemy>();
        DispatcherTimer timer;
        Game game;
        Textbox healthBox;
        Textbox friendlyProjectiles; 
        Textbox enemyProjectiles;
        bool lost = false;
        bool addedPlayers = false;
        int numberOfWaves = 3;
        int wavesSpawned = 0;

        public SpaceInvadersMain(ref Game E_game)
        {
            InitializeComponent();
            game = E_game;

            timer = new DispatcherTimer();

            timer.Tick += new EventHandler(tick);
            timer.Start();
        }
        public void addPlayers()
        {
            // for some reason this needs to be run outside the constructor
            // for the window size to update
            players.Add(new Player(_Canvas,ref projectileCount, 20, 20));
            players[0].updateColour(Brushes.Red);
            addedPlayers = true;
        }

        public void wave()
        {
            wavesSpawned++;
            int enemiesPerRow = 5;
            int numberOfRows = 3;

            for (int column = 0; column < enemiesPerRow; column++)
            {
                for (int row = 0; row < numberOfRows; row++)
                {
                    enemys.Add(new Enemy(_Canvas, ref projectileCount ,column * 50, row * 30, 20, 20));
                }
            }
        }

        public void addTextboxes()
        {
            healthBox = new Textbox(_Canvas, 50, 50, $"Health: {players[0].health}");
            friendlyProjectiles = new Textbox(_Canvas, 50, 70, $"Friendly: {projectileCount.playerProjectiles}");
            friendlyProjectiles._Box.Width += 10;
            enemyProjectiles = new Textbox(_Canvas, 50, 90, $"Enemy: {projectileCount.enemyProjectiles}");
            enemyProjectiles._Box.Width += 10;
        }

        public void updateTextboxes()
        {
            // health box
            healthBox._Box.Text = $"Health: {players[0].health}";
            healthBox.Draw();

            // friendly projectiles
            friendlyProjectiles._Box.Text = $"Friendly: {projectileCount.playerProjectiles}";
            friendlyProjectiles.Draw();

            // enemy projectiles
            enemyProjectiles._Box.Text = $"Enemy: {projectileCount.enemyProjectiles}";
            enemyProjectiles.Draw();
        }

        public void tick(object sender, EventArgs e)
        {
            int numberOfPlayersDead = 0;
            if (!addedPlayers)
            {
                addPlayers();
                addTextboxes();
                wave();
            }
            if (wavesSpawned < numberOfWaves && enemys.Count == 0)
            {
                wave();
            } 
            if (addedPlayers && enemys.Count == 0 && wavesSpawned == numberOfWaves)
            {
                addedPlayers = false;
                timer.Stop();
                MessageBox.Show("Green Key added");
                // if the user does not have the green key
                if (!game.Inventory.ContainsKey("Green"))
                {
                    // add it
                    game.Inventory.Add("Green",new Global.DataTypes.DoorKey(Brushes.Green));
                }
                game.redirect(new EscapeRoomMain(ref game), this, true);
                this.Close();
            }
            // if the escape key is held down
            if (Keyboard.IsKeyDown(Key.Escape))
            {
                timer.Stop();
                game.redirect(new EscapeRoomMain(ref game), this, false);
                this.Close();
            }

            for (int projectileID = projectileCount.projectiles.Count-1; projectileID >= 0; projectileID--)
            {
               
                Projectile current = projectileCount.projectiles[projectileID];
                current.round();
                // if the projectile is off screen
                if (current.offScreen() || current.timeAlive.ElapsedMilliseconds > current.projectileLifetimeMS)
                {
                    current.kill();
                    continue;
                }

                // if the projectile is fired by a player
                if (current.team == Team.player)
                {
                    // go through each enemy
                    for (int enemyID = enemys.Count-1; enemyID >= 0; enemyID--)
                    {
                        // if the enemy has been hit by the projectile
                        if (enemys[enemyID].hit(current))
                        {
                            current.kill();
                        }
                        continue;
                    }
                }
                // otherwise the projectile is fired by an enemt
                for (int playerID  = players.Count-1; playerID >= 0; playerID--)
                {
                    // if the player has been hit 
                    if (players[playerID].hit(current))
                    {
                        // kill the projectile
                        current.kill();
                    }
                    // move onto the next projectile
                    continue;
                }
            }

            foreach (Player player in players)
            {
                player.round();
                if (!player.alive) { numberOfPlayersDead++; }
            }
            bool flip = false;
            foreach (Enemy enemy in enemys)
            {
                if (enemy.timeToFlip())
                {    
                    flip = true;
                    // break out of the loop to stop unneceserry searching
                    // cannot use break as nested if
                    goto flip;
                }
            }
            flip:
            if (flip)
            {
                foreach (Enemy enemy in enemys)
                {

                    // move towards the player and flip
                    enemy.moveDownandFlip();
                }
            }
            for (int enemyID = enemys.Count - 1; enemyID >= 0; enemyID--)
            {
                Enemy enemy = enemys[enemyID];
                // if the enemy is dead
                if (!enemy.alive)
                {
                    // remove it from the screen
                    _Canvas.Children.Remove(enemy._Rect);
                    // and from the enemy list
                    enemys.RemoveAt(enemyID);
                    // move on to the next enemy
                    continue;
                }
                // move the player and redraw in new position
                enemy.round();
                // if the enemy is inline with the player and/or all players are dead
                if (enemy.y >= players[0].y || players.Count == numberOfPlayersDead)
                {
                    timer.Stop();
                    enemy.y = 0;
                    // this is to stop the message box showing multiple times
                    if (!lost)
                    {
                        lost = true;
                        MessageBox.Show("You loose");
                        game.redirect(new EscapeRoomMain(ref game), this, true);
                        this.Close();
                    }
                }
            }
               

            // update the textboxes
            updateTextboxes();
            
        }
    }
}
