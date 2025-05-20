using eSim.Infrastructure.DTOs.Global;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSim.EF.Entities
{
    [Table("Clients", Schema = "master")]
    public class Client : EntityBase
    {
        public Guid Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(30)]
        public string Kid { get; set; } = null!;

        [Required]
        [MaxLength(50)]
        public string Secret { get; set; } = null!;

        public bool IsActive { get; set; } = true;

        [Required]
        [EmailAddress]
        [MaxLength(500)]
        public string PrimaryEmail { get; set; }



    }
}
