using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSim.EF.Entities
{

    [Table("Settings", Schema = "master")]
    public class GlobalSetting
    {
        [Key]
        public Guid Id { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal FixedCommission { get; set; }

        [Range(0,100)]
        public int PercentageCommission { get; set; }
    }
}
