using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Escape_Room.Windows.Space_Invaders.Characters
{
    class Enemy : Character
    {

        int boundary = 50;
        double downDistance = 50;

        public Enemy(Canvas E_Canvas, ref ProjectileCount projectiles, double E_x, double E_y, int E_width, int E_height) : base(E_Canvas, ref projectiles ,E_x, E_y, E_width, E_height, Team.enemy)
        {
            setDefaultValues();
        }
        public Enemy(Canvas E_Canvas,  ref ProjectileCount projectiles,int E_width, int E_height) : base(E_Canvas, ref projectiles, 50, 50, E_width, E_height, Team.enemy)
        {
            x = E_Canvas.ActualWidth / 2 + E_width;
            y = E_Canvas.ActualHeight - E_height - .1 * E_Canvas.ActualHeight;
            setDefaultValues();
        }
        protected void setDefaultValues()
        {
            keepOnScreen = true;
            _L_Speed_PixPerMS = 0.1;
            _R_Speed_PixPerMS = 0.1;
            _U_Speed_PixPerMS = 0.1;
            _D_Speed_PixPerMS = 0.1;

            projectileSpeed = 0.1;

            fireDelayMS = 1000;
            maxNumberOfProjectiles = 40;
            // 1.00 is 100%
            chanceToFire = 0.25;

            updateColour(Brushes.Green);
            Draw();

        }
        public bool timeToFlip()
        {
            // if currently moveing Right
            if (moveingRight)
            {
                // if past the boundry
                if (_Canvas.ActualWidth - x - width/2 < boundary)
                {
                    // return that it is time to move towards the player
                    // and flip the direction
                    return true;
                }
                
                return false;
            }
            // past the boundry on the left side of the screen
            if (x - width/2 < boundary)
            {
                return true;
            }
            return false;
        }
        public void moveDownandFlip()
        {
            if (moveingLeft)
            {
                // move towards the player
                y += downDistance;

                // flip the direction traveling
                moveingRight = true;
                moveingLeft = false;
                return;
            }
            // move towards the player
            y += downDistance;

            // flip the direction traveliing
            moveingLeft = true;
            moveingRight = false;
            return;
        }
       

        public void round()
        {
            move();
            fire();
            Draw();
        }
        public void move()
        {
            long timeSinceLastMove = timeAlive.ElapsedMilliseconds - lastMoveMS;
            if (timeSinceLastMove < moveDelayMS) { return; }
            double _L_Speed = timeSinceLastMove * _L_Speed_PixPerMS;
            double _R_Speed = timeSinceLastMove * _R_Speed_PixPerMS;
            double _U_Speed = timeSinceLastMove * _U_Speed_PixPerMS;
            double _D_Speed = timeSinceLastMove * _D_Speed_PixPerMS;
            lastMoveMS = timeAlive.ElapsedMilliseconds;

            if (moveingLeft)
            {
                x -= _L_Speed;
                if (keepOnScreen && offScreen())
                {
                    x = 0;
                }
            }
            if (moveingRight)
            {
                x += _R_Speed;
                if (keepOnScreen && offScreen())
                {
                    x = _Canvas.ActualWidth - width;
                }
            }
        }
    }
}
