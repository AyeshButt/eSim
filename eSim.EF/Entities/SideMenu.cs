using System.ComponentModel.DataAnnotations;

namespace eSim.EF.Entities
{
    public class SideMenu
    {
        [Key]
        public int Id { get; set; }
        public string? ControllerName { get; set; }
        public string? ActionName { get; set; }
        public string Title { get; set; }
        public int? ParentId { get; set; }
    }
}
