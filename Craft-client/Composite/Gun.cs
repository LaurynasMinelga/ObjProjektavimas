using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Craft_client.Composite
{
    /// <summary>
    /// Leaf
    /// </summary>
    class Gun : Technology
    {
        // Constructor
        public Gun(string name): base(name)
        {
        }

        public override void Add(Technology c)
        {
            Console.WriteLine(
              "Cannot add to a PrimitiveElement");
        }

        public override void Remove(Technology c)
        {
            Console.WriteLine(
              "Cannot remove from a PrimitiveElement");
        }

        public override void Display(int depth)
        {
            Console.WriteLine(
              new String('-', depth) + " " + _name);
        }
    }
}
