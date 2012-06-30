using MvcExpense.MvcExpenseHelper;

namespace MvcExpense
{
    public class CustomRoleProvider : RoleProviderBase
    {
        public override string[] GetRolesForUser( string username )
        {
            if ( username == "zy" )
            {
                return new string[] { Role.User, Role.ApplicationAdministrator };
            }

            return new string[] { Role.User };
        }
    }
}