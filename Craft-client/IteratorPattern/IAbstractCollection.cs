using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Craft_client.IteratorPattern
{
    /// <summary>
    /// The 'Aggregate' interface
    /// </summary>
    interface IAbstractCollection
    {
        Iterator CreateIterator();
    }
}
