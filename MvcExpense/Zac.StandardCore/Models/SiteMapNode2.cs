using Zac.Tree;

namespace Zac.StandardCore.Models
{
    public partial class SiteMapNode : /*ICloneable,*/ ITreeNodeData
    {
        //public object Clone()
        //{
        //    return ShallowCopy();
        //    //return MemberwiseClone(); // cannot use this as proxy is used
        //}

        //public SiteMapNode ShallowCopy()
        //{
        //    // todo use AutoMapper

        //    var entity = new SiteMapNode
        //    {
        //        ParentId = ParentId,
        //        Id = Id,
        //        Lft = Lft,
        //        Rgt = Rgt,
        //        Url = Url,
        //        Title = Title,
        //        Description = Description,
        //        ImageUrl = ImageUrl
        //    };

        //    return entity;
        //}

        //public bool HasChildren()
        //{
        //    return Children.Count > 0;
        //}

        //public SiteMapNode FindNodeById( long id )
        //{
        //    return SiteMapNodeHelper.FindNodeById( this, id );
        //}

        //private SiteMapNode _tree;

        //public SiteMapNode Tree
        //{
        //    get
        //    {
        //        if ( _tree == null )
        //        {
        //            throw new Exception( "SiteMapNode.Tree is not set." );
        //        }
        //        return Tree;
        //    }
        //    set { _tree = value; }
        //}

        //public SiteMapNode GetParent( SiteMapNode tree )
        //{
        //    if ( !ParentId.HasValue )
        //    {
        //        return null;
        //    }

        //    SiteMapNode node = tree.FindNodeById( ParentId.Value );
        //    return node;
        //}

        //public bool IsFirstChild( SiteMapNode tree )
        //{
        //    SiteMapNode parent = GetParent( tree );
        //    return Lft == parent.Lft + 1;
        //}

        //public ICollection<SiteMapNode> GetSiblings( SiteMapNode tree )
        //{
        //    SiteMapNode parent = GetParent( tree );
        //    return parent.Children;
        //}

        //public SiteMapNode PreviousSibling( SiteMapNode tree )
        //{
        //    if ( IsFirstChild( tree ) )
        //    {
        //        return null;
        //    }

        //    ICollection<SiteMapNode> siblings = GetSiblings( tree );
        //    return siblings.Single( x => x.Rgt == Lft - 1 );
        //}

    }
}
