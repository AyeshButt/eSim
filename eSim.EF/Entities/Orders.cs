    using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSim.EF.Entities
{
    public class Orders
    {
        [Key]
        public Guid Id { get; set; }
        public Guid SubscriberId { get; set; }
        public string OrderReferenceId { get; set; }
        public double? Total { get; set; }
        public string? Currency { get; set; }
        public string? Status { get; set; }
        public string? StatusMessage { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool? Assigned { get; set; }
        public string? SourceIP { get; set; }

    }
}
