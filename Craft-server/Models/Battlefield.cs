using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Craft_server.Interfaces;

namespace Craft_server.Models
{
    public class Battlefield
    {
        private List<IObserver> _observers;
        public bool ship_destroyed { get; set; }

        public Battlefield()
        {
            _observers = new List<IObserver>();
        }

       

        public void Notify() { }
    }
}
