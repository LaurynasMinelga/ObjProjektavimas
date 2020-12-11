using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Craft_client.Composite
{
    /// <summary>
    /// Composite
    /// </summary>
    class CompositeAttack : Technology
    {
        private List<Technology> elements = new List<Technology>();

        // Constructor

        public CompositeAttack(string name): base(name)
        {
        }

        public override void Add(Technology d)
        {
            elements.Add(d);
        }

        public override void Remove(Technology d)
        {
            elements.Remove(d);
        }

        public override void Display(int depth)
        {
            Console.WriteLine(new String('-', depth) + "+ " + _name);

            // Display each child element on this node

            foreach (Technology d in elements)
            {
                d.Display(depth + 2);
            }
        }
    }
}
