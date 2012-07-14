using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Zac.DesignPattern.EntityFramework;
using Zac.DesignPattern.EntityFramework.Repositories;
using Zac.StandardCore.Models;
using Zac.StandardCore.Repositories;

namespace Zac.StandardInfrastructure.EntityFramework.Repositories
{
    public class SiteMapNodeRepository : StandardRepository<SiteMapNode>, ISiteMapNodeRepository
    {
        public SiteMapNodeRepository( EnhancedDbContext context )
            : base( context )
        {
        }

        public IEnumerable<SiteMapNode> GetChildrenByParentId( long id )
        {
            return GetQueryable().Where( x => x.ParentId == id ).AsEnumerable(); // todo don't use GetQueryable()
        }

        const string TableName = "App_SiteMapNode"; // todo remove hardcode

        public void InsertNode( SiteMapNode entity, long parentId, long? previousSiblingId )
        {
            const long size = 2; // right - left + 1

            var parameterCollection = new List<SqlParameter>();
            var sb = new StringBuilder();

            sb.Append( "declare @left bigint, @right bigint, @size bigint;" );

            if ( previousSiblingId.HasValue )
            {
                sb.AppendFormat( "select @left = Rgt + 1 from {0} where Id = @PreviousSiblingId;", TableName );
                parameterCollection.Add( new SqlParameter( "PreviousSiblingId", previousSiblingId.Value ) );
            }
            else
            {
                sb.AppendFormat( "select @left = Lft + 1 from {0} where Id = @ParentId;", TableName );
                parameterCollection.Add( new SqlParameter( "ParentId", parentId ) );
            }

            sb.AppendFormat( "set @size = {0};", size );
            sb.Append( "set @right = @left + @size - 1;" );
            sb.AppendFormat( "update {0} set Lft = Lft + @size where Lft >= @left;", TableName );
            sb.AppendFormat( "update {0} set Rgt = Rgt + @size where Rgt >= @left;", TableName );

            sb.Append( "set @newLeft = @left;" );
            var pLeft = new SqlParameter( "newLeft", SqlDbType.BigInt ) { Direction = ParameterDirection.Output };
            parameterCollection.Add( pLeft );

            string sql = sb.ToString();
            object[] parameters = parameterCollection.Cast<object>().ToArray();
            Context.Database.ExecuteSqlCommand( sql, parameters );

            long left = (long) pLeft.Value;

            entity.Lft = left;
            entity.Rgt = left + size - 1;
        }

        public void UpdateNode( SiteMapNode entity, long parentId, long? previousSiblingId )
        {
            var parameterCollection = new List<SqlParameter>();
            var sb = new StringBuilder();

            //
            // convert NodeId to NodeLft

            sb.Append( "declare @NodeLft bigint;" );

            sb.AppendFormat( "select @NodeLft = Lft from {0} where Id = @NodeId;", TableName );
            parameterCollection.Add( new SqlParameter( "NodeId", entity.Id ) );

            if ( previousSiblingId.HasValue )
            {
                sb.Append( "declare @PreviousSiblingLft bigint;" );
                sb.AppendFormat( "select @PreviousSiblingLft = Lft from {0} where Id = @PreviousSiblingId;", TableName );
                parameterCollection.Add( new SqlParameter( "PreviousSiblingId", previousSiblingId.Value ) );
            }
            else
            {
                sb.Append( "declare @ParentLft bigint;" );
                sb.AppendFormat( "select @ParentLft = Lft from {0} where Id = @ParentId;", TableName );
                parameterCollection.Add( new SqlParameter( "ParentId", parentId ) );
            }

            //
            // retrieve data

            sb.Append( "declare @NodeRgt bigint, @RootRgt bigint;" );
            sb.AppendFormat( "select @NodeRgt = Rgt from {0} where Lft = @NodeLft;", TableName );
            sb.AppendFormat( "select @RootRgt = Rgt from {0} where Lft = 1;", TableName );

            if ( previousSiblingId.HasValue )
            {
                sb.Append( "declare @PreviousSiblingRgt bigint;" );
                sb.AppendFormat( "select @PreviousSiblingRgt = Rgt from {0} where Lft = @PreviousSiblingLft;", TableName );
            }

            //
            // calculations

            sb.Append( "declare @SubtreeSize bigint;" );
            sb.Append( "set @SubtreeSize = @NodeRgt - @NodeLft + 1;" );

            sb.Append( "declare @NewNodeLft bigint;" );

            if ( previousSiblingId.HasValue )
            {
                sb.Append( "if (@NodeLft < @PreviousSiblingLft)" );
                sb.Append( " set @NewNodeLft = @PreviousSiblingRgt - @SubtreeSize + 1;" );
                sb.Append( "else" );
                sb.Append( " set @NewNodeLft = @PreviousSiblingRgt + 1;" );
            }
            else
            {
                sb.Append( "if (@NodeLft < @ParentLft)" );
                sb.Append( " set @NewNodeLft = @ParentLft - @SubtreeSize + 1;" );
                sb.Append( "else" );
                sb.Append( " set @NewNodeLft = @ParentLft + 1;" );
            }

            // move subtree out of main tree
            sb.Append( "declare @d1 bigint;" );
            sb.Append( "set @d1 = @RootRgt - @NodeLft + 1;" );
            sb.AppendFormat( "update {0} set Lft = Lft + @d1, Rgt = Rgt + @d1 where Lft >= @NodeLft and Rgt <= @NodeRgt;", TableName );

            // close up gap of subtree
            sb.AppendFormat( "update {0} set Lft = Lft - @SubtreeSize where Lft >= @NodeLft;", TableName );
            sb.AppendFormat( "update {0} set Rgt = Rgt - @SubtreeSize where Rgt >= @NodeLft;", TableName );

            // create space for subtree
            sb.AppendFormat( "update {0} set Lft = Lft + @SubtreeSize where Lft >= @NewNodeLft;", TableName );
            sb.AppendFormat( "update {0} set Rgt = Rgt + @SubtreeSize where Rgt >= @NewNodeLft;", TableName );

            // move subtree back into main tree
            sb.Append( "set @d1 = @RootRgt - @NewNodeLft + 1;" );
            sb.AppendFormat( "update {0} set Lft = Lft - @d1, Rgt = Rgt - @d1 where Rgt > @RootRgt;", TableName );

            sb.Append( "set @newLeft = @NewNodeLft;" );
            var pLeft = new SqlParameter( "newLeft", SqlDbType.BigInt ) { Direction = ParameterDirection.Output };
            parameterCollection.Add( pLeft );

            sb.Append( "set @newRight = @NewNodeLft + @SubtreeSize - 1;" );
            var pRight = new SqlParameter( "newRight", SqlDbType.BigInt ) { Direction = ParameterDirection.Output };
            parameterCollection.Add( pRight );

            string sql = sb.ToString();
            object[] parameters = parameterCollection.Cast<object>().ToArray();
            Context.Database.ExecuteSqlCommand( sql, parameters );

            // cannot use this because output parameter cannot be returned immediately
            //var sqlCommand = new SqlCommand( sql );
            //sqlCommand.Parameters.AddRange( parameterCollection.ToArray() );
            //Context.SqlCommandsBeforeSaveChanges.Add( sqlCommand );

            entity.Lft = (long) pLeft.Value;
            entity.Rgt = (long) pRight.Value;
        }

        public void DeleteNode( long id )
        {
            var sb = new StringBuilder();
            sb.Append( "declare @left bigint, @right bigint, @size bigint;" );

            sb.AppendFormat( "select @left = Lft, @right = Rgt, @size = Rgt - Lft + 1 from {0} where Id = @id;", TableName );
            var pId = new SqlParameter( "id", SqlDbType.BigInt ) { SqlValue = id };

            sb.AppendFormat( "delete from {0} where Lft >= @left and Rgt <= @right;", TableName );
            sb.AppendFormat( "update {0} set Lft = Lft - @size where Lft >= @left;", TableName );
            sb.AppendFormat( "update {0} set Rgt = Rgt - @size where Rgt >= @left;", TableName );

            string sql = sb.ToString();

            //Context.Database.ExecuteSqlCommand( sql, pId );

            var sqlCommand = new SqlCommand( sql );
            sqlCommand.Parameters.Add( pId );
            Context.SqlCommandsBeforeSaveChanges.Add( sqlCommand );
        }

    }
}
