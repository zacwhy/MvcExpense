using Zac.DesignPattern.EntityFramework;
using Zac.DesignPattern.EntityFramework.Repositories;
using Zac.StandardCore.Models;
using Zac.StandardCore.Repositories;

namespace Zac.StandardInfrastructure.EntityFramework.Repositories
{
    public class ErrorLogRepository : StandardRepository<ErrorLog>, IErrorLogRepository
    {
        public ErrorLogRepository( EnhancedDbContext context )
            : base( context )
        {
        }
    }
}
