using System.Collections.Generic;
using System.Data.SqlClient;

namespace Zac.DesignPattern.EntityFramework.UnitOfWork
{
    internal static class UnitOfWorkBaseHelper
    {
        internal static IEnumerable<SqlParameter> CopyParameters( SqlParameterCollection sqlParameterCollection )
        {
            foreach ( SqlParameter sqlParameter in sqlParameterCollection )
            {
                yield return new SqlParameter( sqlParameter.ParameterName, sqlParameter.SqlValue )
                {
                    SqlValue = sqlParameter.SqlValue
                    //, Direction = sqlParameter.Direction
                };
            }
        }

    }
}
