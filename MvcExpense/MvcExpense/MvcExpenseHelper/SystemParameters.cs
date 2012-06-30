using System.Configuration;

namespace MvcExpense.MvcExpenseHelper
{
    public class SystemParameters
    {
        public const string DateFormat = "dd MMM yyyy ddd";
        public const string DropDownListNullDisplay = "[ Select One ]";

        public static string DefaultController
        {
            get
            {
                const string defaultValue = "Home";
                return GetAppSettingValueOrDefaultValue( "DefaultController", defaultValue );
            }
        }

        public static string DefaultAction
        {
            get
            {
                const string defaultValue = "Index";
                return GetAppSettingValueOrDefaultValue( "DefaultAction", defaultValue );
            }
        }

        private static string GetAppSettingValueOrDefaultValue( string appSettingName, string defaultValue )
        {
            string value = ConfigurationManager.AppSettings[appSettingName];
            return GetValueOrDefaultValue( value, defaultValue );
        }

        private static string GetValueOrDefaultValue( string value, string defaultValue )
        {
            if ( string.IsNullOrEmpty( value ) )
            {
                return defaultValue;
            }
            return value;
        }

    }
}