using System.Collections.Generic;

namespace System.Web.Mvc
{
    public class SelectListGroupItem
    {
        public string Name { get; set; }
        public List<SelectListItem> Items { get; set; }
    }
}