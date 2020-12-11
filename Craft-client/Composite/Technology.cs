using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Craft_client.Composite
{
    /// <summary>
    /// Component
    /// </summary>
    abstract class Technology
    {
        protected string _name;

        // Constructor

        public Technology(string name)
        {
            this._name = name;
        }

        public abstract void Add(Technology d);
        public abstract void Remove(Technology d);
        public abstract void Display(int depth);
    }
}
