using eSim.Infrastructure.DTOs.Global;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSim.Infrastructure.DTOs.Client
{
    public class ClientDTO : EntityBase
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage ="Name is required")]
        [MaxLength(100)]
        public string Name { get; set; } = null!;

        [MaxLength(30)]
        public string? Kid { get; set; }

        [MaxLength(50)]
        public string? Secret { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
