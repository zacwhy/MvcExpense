using System;
using System.Collections.ObjectModel;

namespace Zac.Tree
{
    public class TreeNode<T> : IDisposable
    {
        public TreeNode( T data )
            : this( data, null )
        {
        }

        public TreeNode( T data, TreeNode<T> parent )
        {
            Data = data;

            if ( parent != null )
            {
                Parent = parent;
            }

            Children = new Collection<TreeNode<T>>();
        }

        public void Dispose()
        {
            ClearChildren();

            if ( Parent != null )
            {
                Children = null;
            }

            Parent = null;

            var disposable = Data as IDisposable;

            if ( disposable != null )
            {
                disposable.Dispose();
            }
        }

        public void ClearChildren()
        {
            foreach ( TreeNode<T> child in Children )
            {
                child.Dispose();
            }

            Children.Clear();
        }

        public TreeNode<T> Parent { get; private set; }
        public virtual Collection<TreeNode<T>> Children { get; private set; }
        public T Data { get; set; }

        public bool IsRoot
        {
            get { return Parent == null; }
        }

        public bool HasChildren
        {
            get { return Children.Count > 0; }
        }

        //public TreeNode<T> FirstChild
        //{
        //    get { return Children.First(); }
        //}

        public Collection<TreeNode<T>> Siblings
        {
            get { return Parent == null ? new Collection<TreeNode<T>> { this } : Parent.Children; }
        }

        //public TreeNode<T> FirstSibling
        //{
        //    get { return Siblings.First(); }
        //}

        public int SiblingIndex
        {
            get
            {
                for ( int i = 0; i < Siblings.Count; i++ )
                {
                    TreeNode<T> sibling = Siblings[i];

                    if ( sibling == this )
                    {
                        return i;
                    }
                }

                throw new Exception( "This node does not exist within siblings." );
            }
        }

        public bool IsFirstChild
        {
            get { return SiblingIndex == 0; }
        }

        public TreeNode<T> PreviousSibling
        {
            get { return IsFirstChild ? null : Siblings[SiblingIndex - 1]; }
        }

        public void RemoveSelfFromTree()
        {
            ClearChildren();
            Siblings.Remove( this );
            Dispose();
        }

        public void RemoveNode( TreeNode<T> node )
        {
            node.RemoveSelfFromTree();
        }

    }
}