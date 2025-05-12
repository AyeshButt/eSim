using System.ComponentModel.DataAnnotations;

namespace eSim.EF.Entities
{
    public class SystemClaims
    {
        [Key]
        public string Id { get; set; }
        [Required]
        public string Type { get; set; }
        [Required]
        public string Value { get; set; }
        public int ParentId { get; set; }
    }
}
