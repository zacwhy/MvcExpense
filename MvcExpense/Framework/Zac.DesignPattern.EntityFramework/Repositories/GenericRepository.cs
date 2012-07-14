using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Zac.DesignPattern.Repositories;

namespace Zac.DesignPattern.EntityFramework.Repositories
{
    public class GenericRepository<TEntity, TId> : IRepository<TEntity, TId> where TEntity : class
    {
        private readonly EnhancedDbContext _context;
        private readonly DbSet<TEntity> _dbSet;

        public EnhancedDbContext Context
        {
            get { return _context; }
        }

        public DbSet<TEntity> DbSet
        {
            get { return _dbSet; }
        }

        public GenericRepository( EnhancedDbContext context )
        {
            // added 2012-06-01
            if ( context == null )
            {
                throw new ArgumentNullException( "context" );
            }

            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        // added 2012-06-02
        public IQueryable<TEntity> GetQueryable()
        {
            //return _dbSet.AsQueryable();
            return _dbSet.AsNoTracking().AsQueryable();
        }

        // added 2012-06-02
        public IEnumerable<TEntity> GetAll()
        {
            return Get();
        }

        public virtual IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "" )
        {
            IQueryable<TEntity> query = _dbSet;

            if ( filter != null )
            {
                query = query.Where( filter );
            }

            string[] includeProperties2 = includeProperties.Split( new char[] { ',' },
                                                                   StringSplitOptions.RemoveEmptyEntries );

            foreach ( var includeProperty in includeProperties2 )
            {
                query = query.Include( includeProperty );
            }

            if ( orderBy != null )
            {
                return orderBy( query ).ToList();
            }

            return query.ToList();
        }

        public virtual TEntity GetById( TId id )
        {
            return _dbSet.Find( id );
        }

        public virtual void Insert( TEntity entity )
        {
            _dbSet.Add( entity );
        }

        public virtual void Delete( TId id )
        {
            TEntity entityToDelete = _dbSet.Find( id );
            Delete( entityToDelete );
        }

        public virtual void Delete( TEntity entityToDelete )
        {
            if ( _context.Entry( entityToDelete ).State == EntityState.Detached )
            {
                _dbSet.Attach( entityToDelete );
            }
            _dbSet.Remove( entityToDelete );
        }

        public virtual void Update( TEntity entityToUpdate )
        {
            _dbSet.Attach( entityToUpdate );
            _context.Entry( entityToUpdate ).State = EntityState.Modified;
        }
    }
}