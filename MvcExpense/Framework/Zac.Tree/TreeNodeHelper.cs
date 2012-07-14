using System.Collections.Generic;
using System.Linq;

namespace Zac.Tree
{
    public static class TreeNodeHelper
    {
        public static TreeNode<T> FindNodeById<T>( TreeNode<T> node, long id )
        {
            var nodeData = (ITreeNodeData) node.Data;
            long nodeId = nodeData.Id;

            if ( nodeId == id )
            {
                return node;
            }

            foreach ( TreeNode<T> child in node.Children )
            {
                TreeNode<T> treeNode = FindNodeById( child, id );

                if ( treeNode != null )
                {
                    return treeNode;
                }
            }

            return null;
        }

        public static TreeNode<T> CreateTree<T>( IEnumerable<T> dataList ) where T : class, ITreeNodeData
        {
            return BuildTree( dataList );
        }

        private static T GetDataRoot<T>( IEnumerable<T> dataList ) where T : ITreeNodeData
        {
            return dataList.Single( x => x.Lft == 1 );
        }

        private static bool HasChildren( ITreeNodeData data )
        {
            return data.Rgt > ( data.Lft + 1 );
        }

        private static T FirstChild<T>( ITreeNodeData parentData, IEnumerable<T> children ) where T : ITreeNodeData
        {
            return children.Single( x => x.Lft == parentData.Lft + 1 );
        }

        private static T NextSibling<T>( T child, IEnumerable<T> children ) where T : ITreeNodeData
        {
            return children.SingleOrDefault( x => x.Lft == child.Rgt + 1 );
        }

        private static TreeNode<T> BuildTree<T>( IEnumerable<T> dataList, TreeNode<T> parent = null ) where T : class, ITreeNodeData
        {
            IList<T> list = dataList.ToList();

            if ( parent == null )
            {
                T data = GetDataRoot( list );
                parent = new TreeNode<T>( data );
            }

            if ( HasChildren( parent.Data ) )
            {
                T child = FirstChild( parent.Data, list );

                while ( child != null )
                {
                    var childTreeNode = new TreeNode<T>( child, parent );
                    parent.Children.Add( childTreeNode );
                    BuildTree( list, childTreeNode );
                    child = NextSibling( child, list );
                }
            }

            return parent;
        }

    }
}