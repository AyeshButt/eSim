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
    public class Client
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




    }
}
