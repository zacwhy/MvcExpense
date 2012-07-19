using System.Collections.Generic;
using Zac.StandardCore;
using Zac.StandardCore.Models;
using Zac.StandardCore.Repositories;
using Zac.StandardCore.Services;

namespace Zac.StandardMvc.Services
{
    class ErrorLogService : IErrorLogService
    {
        private IStandardUnitOfWork _standardUnitOfWork;

        private IErrorLogRepository ErrorLogRepository
        {
            get { return _standardUnitOfWork.ErrorLogRepository; }
        }

        public ErrorLogService( IStandardUnitOfWork standardUnitOfWork )
        {
            _standardUnitOfWork = standardUnitOfWork;
        }

        public IEnumerable<ErrorLog> FindAll()
        {
            return ErrorLogRepository.GetAll();
        }

        public ErrorLog FindById( long id )
        {
            return ErrorLogRepository.GetById( id );
        }

        public void Save( ErrorLog entity )
        {
            throw new System.NotImplementedException();
        }

    }
}
