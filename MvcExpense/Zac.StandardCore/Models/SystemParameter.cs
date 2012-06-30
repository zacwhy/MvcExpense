using Zac.DesignPattern.Models;

namespace Zac.StandardCore.Models
{
    public class SystemParameter : StandardPersistentObject
    {
        public string Code { get; set; }
        public string Value { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
    }
}
