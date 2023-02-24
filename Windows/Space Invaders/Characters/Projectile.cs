using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Escape_Room.Windows.Space_Invaders.Characters
{
    class Projectile : Character
    {
        static int projectile_width = 5;
        static int projectile_height = 15;
        public long projectileLifetimeMS = 5000;
        protected double speed;
        Character _Owner;
        public Projectile(Character Owner, ref ProjectileCount projectiles) : base(Owner._Canvas, ref projectiles ,Owner.width/2 + Owner.x - projectile_width/2, Owner.height/2 + Owner.y - projectile_height/2, projectile_width, projectile_height, Owner.team)
        {
            _Owner = Owner;
            setDefaultValues();
        }
        protected void setDefaultValues()
        {
            keepOnScreen = false;
            speed = _Owner.projectileSpeed;
            updateColour(Brushes.Blue);
            Draw();
        }
        
        public void kill()
        {
            // remove this from the screen
            _Canvas.Children.Remove(_Rect);
            // and the projectiles list
            projectileCount.projectiles.Remove(this);
            // remove 1 from the number of projectiles fired by this team
            if (team==Team.player) { projectileCount.playerProjectiles--; } else { projectileCount.enemyProjectiles--; }
        }

        public void round()
        {
            move();
            Draw();
        }
        public void move()
        {
            long timeSinceLastMove = timeAlive.ElapsedMilliseconds - lastMoveMS;
            if (timeSinceLastMove < moveDelayMS) { return; }
            double Speed = timeSinceLastMove * speed;
            lastMoveMS = timeAlive.ElapsedMilliseconds;

            // move up if it is fired by the player
            // and down if not
            y -= (team == Team.player) ? Speed : -Speed;
        }
    }
}