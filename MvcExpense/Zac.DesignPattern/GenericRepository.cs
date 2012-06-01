using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace Zac.DesignPattern
{
    public class GenericRepository<TEntity> where TEntity : class
    {
        internal DbContext context;
        internal DbSet<TEntity> dbSet;

        public DbSet<TEntity> DbSet
        {
            get
            {
                return dbSet;
            }
        }

        public GenericRepository( DbContext context )
        {
            // added 2012-06-01
            if ( context == null )
            {
                throw new ArgumentNullException( "context" );
            }

            this.context = context;
            this.dbSet = context.Set<TEntity>();
        }

        //public virtual IQueryable<TEntity> GetQueryable(
        //    Expression<Func<TEntity, bool>> filter = null,
        //    Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
        //    string includeProperties = "" )
        //{
        //    //Expression<Func<TEntity, int, TResult>> selector;
        //    throw new NotImplementedException();
        //}

        public virtual IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "" )
        {
            IQueryable<TEntity> query = dbSet;

            if ( filter != null )
            {
                query = query.Where( filter );
            }

            foreach ( var includeProperty in includeProperties.Split
                ( new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries ) )
            {
                query = query.Include( includeProperty );
            }

            if ( orderBy != null )
            {
                return orderBy( query ).ToList();
            }
            else
            {
                return query.ToList();
            }
        }

        public virtual TEntity GetById( object id )
        {
            return dbSet.Find( id );
        }

        public virtual void Insert( TEntity entity )
        {
            dbSet.Add( entity );
        }

        public virtual void Delete( object id )
        {
            TEntity entityToDelete = dbSet.Find( id );
            Delete( entityToDelete );
        }

        public virtual void Delete( TEntity entityToDelete )
        {
            if ( context.Entry( entityToDelete ).State == EntityState.Detached )
            {
                dbSet.Attach( entityToDelete );
            }
            dbSet.Remove( entityToDelete );
        }

        public virtual void Update( TEntity entityToUpdate )
        {
            dbSet.Attach( entityToUpdate );
            context.Entry( entityToUpdate ).State = EntityState.Modified;
        }
    }
}