using System.Collections.Generic;

namespace Zac.RepositorySiteMapProvider
{
    internal class Node<T> where T : class
    {
        public Node<T> Parent { get; private set; }
        public ICollection<Node<T>> Children { get; private set; }
        public T Value { get; private set; }

        public Node( T value )
            : this( value, null )
        {
        }

        public Node( T value, Node<T> parent )
        {
            if ( parent != null )
            {
                Parent = parent;
                parent.Children.Add( this );
            }

            Value = value;
            Children = new List<Node<T>>();
        }

        protected Node<T> GetRoot()
        {
            if ( Parent == null )
            {
                return this;
            }

            return Parent.GetRoot();
        }

    }
}