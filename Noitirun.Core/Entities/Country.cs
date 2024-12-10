using System.ComponentModel.DataAnnotations;

namespace Noitirun.Core.Entities
{
    public class Country : BaseAuditableEntity
    {
        [MaxLength(50)]
        public string? Name { get; set; }
        [MaxLength(50)]
        public string? NameFr { get; set; }

        //[RegularExpression(@"^\+(\d{1,4})$", ErrorMessage = "Invalid phone prefix.")]
        public string PhonePrefixe { get; set; }
    }
}
