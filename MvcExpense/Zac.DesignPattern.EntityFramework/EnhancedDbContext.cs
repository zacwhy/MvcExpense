using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;

namespace Zac.DesignPattern.EntityFramework
{
    public class EnhancedDbContext : DbContext
    {
        public EnhancedDbContext( string nameOrConnectionString )
            : base( nameOrConnectionString )
        {
        }

        private ICollection<SqlCommand> _sqlCommandsBeforeSaveChanges;

        public ICollection<SqlCommand> SqlCommandsBeforeSaveChanges
        {
            get { return _sqlCommandsBeforeSaveChanges ?? ( _sqlCommandsBeforeSaveChanges = new List<SqlCommand>() ); }
        }

    }
}
