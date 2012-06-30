using System.Data.Entity;
using Zac.DesignPattern.EntityFramework.Repositories;
using Zac.StandardCore.Models;
using Zac.StandardCore.Repositories;

namespace Zac.StandardInfrastructure.EntityFramework.Repositories
{
    public class SystemParameterRepository : StandardRepository<SystemParameter>, ISystemParameterRepository
    {
        public SystemParameterRepository( DbContext context )
            : base( context )
        {
        }
    }
}
