using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eSim.Infrastructure.DTOs.Esim;

namespace eSim.Infrastructure.DTOs
{
    public class GetAppliedBundleStatusRequestDTO : EsimCompatibilityRequestDto

    {
    }
    public class GetAppliedBundleStatus
    {
        public string Id { get; set; }
        public string CallTypeGroup { get; set; }
        public int InitialQuantity { get; set; }
        public int RemainingQuantity { get; set; }
        public DateTime AssignmentDateTime { get; set; }
        public string AssignmentReference { get; set; }
        public string BundleState { get; set; }
        public bool Unlimited { get; set; }

    }

    public class GetAppliedBundleStatusResponseDTO
    {
        public List<GetAppliedBundleStatus> Assignments { get; set; }
        public string Message { get; set; }
    }

}
