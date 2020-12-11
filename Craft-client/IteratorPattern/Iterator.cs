using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft_client.objects;

namespace Craft_client.IteratorPattern
{
    class Iterator : IAbstractIterator
    {
        private Collection _collection;
        private int _current = 0;
        private int _step = 1;

        // Constructor

        public Iterator(Collection collection)
        {
            this._collection = collection;
        }

        // Gets first item

        public Coordinate First()
        {
            _current = 0;
            return _collection[_current] as Coordinate;
        }

        // Gets next item

        public Coordinate Next()
        {
            _current += _step;
            if (!IsDone)
                return _collection[_current] as Coordinate;
            else

                return null;
        }

        // Gets or sets stepsize

        public int Step
        {
            get { return _step; }
            set { _step = value; }
        }

        // Gets current iterator item

        public Coordinate CurrentItem
        {
            get { return _collection[_current] as Coordinate; }
        }

        // Gets whether iteration is complete

        public bool IsDone
        {
            get { return _current >= _collection.Count; }
        }
    }
}
