using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Craft_server.Models;

namespace Craft_server.Builder
{
    public abstract class SpecialAttackBuilder
    {
        protected GamePanel attack;

        // Gets vehicle instance
        public GamePanel Attack
        {
            get { return attack; }
        }

        // Abstract build methods
        public abstract void BuildFrontGun();
        public abstract void BuildReadGun();
    }
}
