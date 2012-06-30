using System.Collections.Specialized;

namespace MvcExpense
{
    public class CustomMembershipProvider : MembershipProviderBase
    {
        public override void Initialize( string name, NameValueCollection config )
        {
            base.Initialize( name, config );
        }

        public override bool ValidateUser( string username, string password )
        {
            return true;
        }
    }
}