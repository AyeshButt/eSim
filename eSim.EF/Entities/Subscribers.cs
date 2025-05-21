using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eSim.Infrastructure.DTOs.Global;

namespace eSim.EF.Entities
{
    [Table("Subscribers", Schema = "client")]
    public class Subscribers
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; } = null!;
        [MaxLength(50)]
        public string LastName { get; set; } = null!;

        [MaxLength(500)]
        [DataType(DataType.EmailAddress)]
        public string Email{ get; set; } = null!;

        
        [MaxLength(250)]
        [Required]
        public string Hash{ get; set; } = null!;
        public bool Active{ get; set; }

        [Required]
        public Guid ClientId { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime ModifiedAt { get; set; }


        [Required]
        [MaxLength(2)]
        public string Country { get; set; }
        public bool IsEmailVerifired { get; set; } = false;
        public bool TermsAndConditions { get; set; }

    }
}
