using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Craft_server.Models;

namespace Craft_server.Interfaces
{
    interface ISubject
    {
        void Attach(IObserver observer);
        void Notify();
    }
}
