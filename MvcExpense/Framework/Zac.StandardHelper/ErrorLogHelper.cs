using System;
using System.Diagnostics.Contracts;
using Zac.StandardCore;
using Zac.StandardCore.Models;

namespace Zac.StandardHelper
{
    public class ErrorLogHelper
    {
        private IStandardUnitOfWork _standardUnitOfWork;

        private string _userId;
        private string _hostIP;
        private string _clientIP;

        public ErrorLogHelper( IStandardUnitOfWork standardUnitOfWork, string userId, string hostIP, string clientIP )
        {
            _standardUnitOfWork = standardUnitOfWork;

            _userId = userId;
            _hostIP = hostIP;
            _clientIP = clientIP;
        }

        public void Log( ErrorLog errorLog )
        {
            errorLog.ErrorDate = DateTime.Now;
            errorLog.UserId = _userId;
            errorLog.HostIP = _hostIP;
            errorLog.ClientIP = _clientIP;

            Save( errorLog );
        }

        public void Log( Exception exception, string category = null )
        {
            var errorLog = new ErrorLog
            {
                ErrorCategory = category,
                ExceptionType = exception.GetType().FullName,
                Message = exception.Message,
                BaseMessage = exception.GetBaseException().Message,
                StackTrace = exception.StackTrace
            };

            Log( errorLog );
        }

        private void Save( ErrorLog entity )
        {
            Contract.Requires( entity.UserId != null );

            _standardUnitOfWork.ErrorLogRepository.Insert( entity );
            _standardUnitOfWork.Save();
        }

    }
}
