using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft_client.objects;

namespace Craft_client.IteratorPattern
{
    /// <summary>
    /// The 'Iterator' interface
    /// </summary>
    interface IAbstractIterator
    {
        Coordinate First();
        Coordinate Next();
        bool IsDone { get; }
        Coordinate CurrentItem { get; }
    }
}
