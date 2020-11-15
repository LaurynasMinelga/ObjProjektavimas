using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Craft_server.Interfaces
{
    interface IObserver
    {
        void Update(ISubject subject);
    }
}
