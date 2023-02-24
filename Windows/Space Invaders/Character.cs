using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Diagnostics;
using Escape_Room.Windows.Space_Invaders.Characters;

namespace Escape_Room.Windows.Space_Invaders
{
    enum Team
    {
        player,
        enemy,
    }
    class Character
    {
        public SolidColorBrush colour;
        public Canvas _Canvas { get; protected set; }
        public double x { get; protected set; }
        public double y { get; set; }
        public int width { get; protected set; }
        public int height { get; protected set; }

        public bool canTakeDamage = true;
        public int maxHealth { get; protected set; }
        public int health { get; set; }
        protected long lastDamageMS;
        protected long damageDelayMS = 500;
        public bool alive { get; protected set; } = true;
        public int damage { get; protected set; } = 1;

        public Team team;

        public Stopwatch timeAlive = new Stopwatch();
        public Rectangle _Rect;
        Random rand = new Random();

        //                  movement
        protected bool moveingLeft = false;
        protected bool moveingRight = false;
        protected double _D_Speed_PixPerMS { get; set; }
        protected double _U_Speed_PixPerMS { get; set; }
        protected double _L_Speed_PixPerMS { get; set; }
        protected double _R_Speed_PixPerMS { get; set; }
        protected bool keepOnScreen;

        protected long lastMoveMS;
        protected long moveDelayMS = 1;

        //                  fire
        protected long lastFiredMS;
        protected long fireDelayMS;
        protected int maxNumberOfProjectiles;
        protected double chanceToFire = 100.0;
        public ProjectileCount projectileCount;
        public double projectileSpeed = 0.1;

        public Character(Canvas E_canvas, ref ProjectileCount projectiles, double E_x, double E_y, int E_width, int E_height, Team E_team)
        {
            _Canvas = E_canvas;
            projectileCount = projectiles;
            x = E_x;
            y = E_y;
            width = E_width;
            height = E_height;
            team = E_team;
            _Rect = new Rectangle
            {
                Width = width,
                Height = height,
            };
            _Canvas.Children.Add(_Rect);
            timeAlive.Start();
            Draw();
        }

        public void Draw()
        {
            Canvas.SetTop(_Rect, y);
            Canvas.SetLeft(_Rect, x);
        }
        public bool offScreen()
        {
            return (x < 0 || y < 0 || x > _Canvas.ActualWidth - width || y > _Canvas.ActualHeight - height);
        }
        public bool inHitbox(Character other)
        {
            double left = x;
            double right = x + width;
            double top = y;
            double bottom = y + height;

            double otherLeft = other.x;
            double otherRight = other.x + other.width;
            double otherTop = other.y;
            double otherBottom = other.y + other.height;
            // if the left or right of the other person is in the hitbox
            if ((otherLeft >= left && otherLeft <= right) || (otherRight >= left && otherRight <= right))
            {
                if ((otherTop >= top && otherTop <= bottom) || (otherBottom >= top && otherBottom <= bottom))
                {
                    return true;
                }
            }
            return false;
        }

        public void fire()
        {
            // find the team of the current character
            // and if that team had shots remaining
            if (team == Team.player && projectileCount.playerProjectiles < maxNumberOfProjectiles || 
                team == Team.enemy && projectileCount.enemyProjectiles < maxNumberOfProjectiles) 
            {
                if (timeAlive.ElapsedMilliseconds - lastFiredMS < fireDelayMS)
                {
                    // not enough time passed since last fire
                    return;
                }
                
                // random chance to fire is %, higher is morelikely to fire
                if (rand.NextDouble() < chanceToFire)
                {
                    projectileCount.projectiles.Add(new Projectile(this, ref projectileCount));
                    // add one to the number of projectiles fired by this team
                    if (team == Team.player) { projectileCount.playerProjectiles++; } else { projectileCount.enemyProjectiles++; }
                    lastFiredMS = timeAlive.ElapsedMilliseconds;
                }
                // if they failed to fire
                else
                {
                    //
                    lastFiredMS = timeAlive.ElapsedMilliseconds + (long)(((double)rand.Next(50, 100))/100 * (double)fireDelayMS);
                }
                //lastFiredMS = timeAlive.ElapsedMilliseconds;
                return;
            }
        }
        public bool hit(Character other)
        {
            // this is for when the hit funtion has been called on a thing that cannot take damage so it just returns haveing not done anything

            if (!canTakeDamage) { return false; }

            // if not enougth time has passed since the last hit
            // return to not get hit
            if (timeAlive.ElapsedMilliseconds - lastDamageMS < damageDelayMS) { return false; }

            // if the two items are on the same team, return false
            if (this.team == other.team)
            {
                return false;
            }

            // if the other thing is not in the hitbox of this
            // return and dont take damage
            if (!this.inHitbox(other)) { return false; }


            // if you can take damage
            // take damage
            takeDamage(other);
            return true;
        }
        public void updateColour(SolidColorBrush newColour)
        {
            colour = newColour;
            _Rect.Fill = colour;
        }
        protected void takeDamage(Character other)
        {
            health -= other.damage;
            // set this time as the last time damage was delt
            lastDamageMS = timeAlive.ElapsedMilliseconds;
            if (health <= 0)
            {
                this.alive = false;
                this.timeAlive.Stop();
            }
        }
        public void updateProjectiles()
        {
            // so that if a projectile is removed it removes it from the end so len of list unchecked
            // doesnt change
            for (int projectileID = projectileCount.projectiles.Count - 1; projectileID >= 0; projectileID--)
            {
                Projectile current = projectileCount.projectiles[projectileID];
                // if the projectile is off screen
                if (current.offScreen() || current.timeAlive.ElapsedMilliseconds > current.projectileLifetimeMS)
                {
                    // remove it from the screen
                    _Canvas.Children.Remove(current._Rect);

                    // remove it from the list
                    projectileCount.projectiles.RemoveAt(projectileID);

                    // move to the next projectile
                    continue;
                }

                current.round();

            }
        }
    }
}