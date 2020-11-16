using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Craft_server.Models;

namespace Craft_server.Builder
{
    public class AirCarrierBuilder : SpecialAttackBuilder
    {
        public AirCarrierBuilder()
        {
            attack = new GamePanel
            {
                SessionId = 0,
                Level = Levels.Desert,
                Gun1 = "",
                Gun2 = ""
            };
        }
        
        public override void BuildFrontGun()
        {
            attack.Gun1 = "Nuclear";
        }
        public override void BuildReadGun()
        {
            attack.Gun2 = "Bomberplane";
        }
    }
}
