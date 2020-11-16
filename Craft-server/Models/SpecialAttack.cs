using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Craft_server.Builder;

namespace Craft_server.Models
{
    public class SpecialAttack
    {
        public void Construct(SpecialAttackBuilder builder)
        {
            builder.BuildFrontGun();
            builder.BuildReadGun();
        }
    }
}
