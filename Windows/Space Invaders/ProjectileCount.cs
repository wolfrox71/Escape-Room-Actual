using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Escape_Room.Windows.Space_Invaders.Characters;

namespace Escape_Room.Windows.Space_Invaders
{
    class ProjectileCount
    {
        public List<Projectile> projectiles;
        public int enemyProjectiles = 0;
        public int playerProjectiles = 0;
        public ProjectileCount()
        {
            projectiles = new List<Projectile>();

        }
    }
}
