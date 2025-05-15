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


    [Table("Settings", Schema = "client")]
    public class ClientSettings : EntityBase
    {
        [Key]
        public Guid Id { get; set; }

       
        public Guid ClientId { get; set; }


        [ForeignKey(nameof(ClientId))]
        public Client Client { get; set; } = null!;


        [Column(TypeName = "decimal(18,2)")]
        public decimal FixedCommission { get; set; }

    }
}
