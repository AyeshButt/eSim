using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;

namespace eSim.Infrastructure.DTOs.Esim
{
    public class ListBundlesAppliedToESIMRequestDTO
    {
        [Required(ErrorMessage ="iccid is required")]
        public string iccid { get; set; }

        public bool? includeUsed { get; set; }

        [Range(1,200,ErrorMessage = "Must be between 1 and 200.")]
        public int? limit { get; set; }

    }
    public class ListBundlesAppliedToESIMResponseDTO
    {
        public List<BundleDTO> Bundles { get; set; } = new List<BundleDTO>();

    }

    public class BundleDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<AssignmentDTO> Assignments { get; set; } = new List<AssignmentDTO>();

    }

    public class AssignmentDTO
    {
        public string Id { get; set; }
        public string CallTypeGroup { get; set; }
        public long InitialQuantity { get; set; }
        public long RemainingQuantity { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public DateTime AssignmentDateTime { get; set; }
        public string AssignmentReference { get; set; }
        public string BundleState { get; set; }
        public bool Unlimited { get; set; }
    }

}
