using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eSim.Infrastructure.DTOs.Global;

namespace eSim.Infrastructure.DTOs.Client
{
    public class ClientSettingsDTO : EntityBase
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid ClientId { get; set; }

        public string? Name { get; set; }

        [Required(ErrorMessage = "Comission amount is required")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal FixedCommission { get; set; } = 0.00M;

    }
}
