using System.ComponentModel.DataAnnotations;

namespace Zac.Mvc.Models.Input
{
    public class SiteMapNodeCreateInput
    {
        [Required]
        public string Url { get; set; }

        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        [Required]
        public long ParentId { get; set; }

        public long? PreviousSiblingId { get; set; }
    }
}