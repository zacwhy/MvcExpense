using Zac.DesignPattern.EntityFramework;
using Zac.DesignPattern.EntityFramework.Repositories;
using Zac.StandardCore.Models;
using Zac.StandardCore.Repositories;

namespace Zac.StandardInfrastructure.EntityFramework.Repositories
{
    public class SystemParameterRepository : StandardRepository<SystemParameter>, ISystemParameterRepository
    {
        public SystemParameterRepository( EnhancedDbContext context )
            : base( context )
        {
        }
    }
}
