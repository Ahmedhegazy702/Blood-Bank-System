using System.ComponentModel.DataAnnotations;

namespace Dashboard.Models
{
    public class RoleFormViewModel
    {
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Name is Required")]
        [StringLength(256)]
        public string Name { get; set; }
    }
}
