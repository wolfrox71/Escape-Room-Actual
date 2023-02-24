using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;

namespace Escape_Room.Windows.Space_Invaders.Characters
{
    class Player : Character
    {
       

        List<Key> LeftKeys = new List<Key>();
        List<Key> RightKeys = new List<Key>();
        List<Key> FireKeys = new List<Key>();

        public Player(Canvas E_Canvas, ref ProjectileCount projectiles, double E_x, double E_y, int E_width, int E_height) : base(E_Canvas, ref projectiles, E_x, E_y, E_width, E_height, Team.player)
        {
            setDefaultValues();
        }
        public Player(Canvas E_Canvas, ref ProjectileCount projectiles, int E_width, int E_height) : base(E_Canvas, ref projectiles, 50, 50 ,E_width, E_height, Team.player)
        {
            x = E_Canvas.ActualWidth/2 + E_width;
            y = E_Canvas.ActualHeight - E_height - .1 * E_Canvas.ActualHeight;
            setDefaultValues();
        }
        protected void setDefaultValues()
        {
            canTakeDamage = true;
            health = 5;
            keepOnScreen = true;
            _L_Speed_PixPerMS = 0.3;
            _R_Speed_PixPerMS = 0.3;
            _U_Speed_PixPerMS = 0.3;
            _D_Speed_PixPerMS = 0.3;

            fireDelayMS = 400;
            maxNumberOfProjectiles = 10;
            projectileSpeed = 0.3;


            LeftKeys.Add(Key.A);
            LeftKeys.Add(Key.Left);
            RightKeys.Add(Key.D);
            RightKeys.Add(Key.Right);
            FireKeys.Add(Key.Space);

            Draw();
        }
        public void updateMovement()
        {
            moveingLeft = false;
            moveingRight = false;
            foreach (Key key in LeftKeys)
            {
                // if that key is pressed down
                if (Keyboard.IsKeyDown(key))
                {
                    moveingLeft = true;
                }
            }
            foreach (Key key in RightKeys)
            {
                if (Keyboard.IsKeyDown (key))
                {
                    moveingRight = true;
                }
            }
        }

        public void updateKeys()
        {
            foreach (Key key in FireKeys)
            {
                if (Keyboard.IsKeyDown(key))
                {
                    fire();
                }
            }
        }

        

        public void round()
        {
            updateMovement();
            updateKeys();
            move();
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
