using Zac.DesignPattern.EntityFramework.Repositories;
using Zac.StandardCore.Models;
using Zac.StandardCore.Repositories;

namespace Zac.StandardInfrastructure.EntityFramework.Repositories
{
    public class SystemParameterRepository : StandardRepository<SystemParameter>, ISystemParameterRepository
    {
        public SystemParameterRepository( StandardDbContext context )
            : base( context )
        {
        }
    }
}
