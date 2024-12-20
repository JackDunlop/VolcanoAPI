using System.ComponentModel.DataAnnotations;

namespace VolcanoAPI.Data
{
    public class UserData
    {
        [Key]
        [Range(1, int.MaxValue)]
        [Display(Name = "ID")]
        public int? id { get; set; }

        [Required]
        [Display(Name = "Username")]
        public string name { get; set; }

        [Required]
        [Display(Name = "Password")]
        public string password { get; set; }

        [Required]
        [Display(Name = "Email")]
        public string email { get; set; }

        [Required]
        [Display(Name = "Date of birth")]
        public DateTime dateOfBirth { get; set; }
    }
}
