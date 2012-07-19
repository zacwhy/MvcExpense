using System.Collections.Generic;
using Zac.DesignPattern.Services;
using Zac.StandardCore.Models;

namespace Zac.StandardCore.Services
{
    public interface IErrorLogService : IStandardService<ErrorLog>
    {
        IEnumerable<ErrorLog> FindAll();
        ErrorLog FindById( long id );
        void Save( ErrorLog entity );
    }
}
