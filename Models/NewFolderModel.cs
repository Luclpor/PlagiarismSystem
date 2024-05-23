using System.ComponentModel.DataAnnotations;

namespace PlagiarismSystem.Models
{
    public class NewFolderModel
    {
        [Required(ErrorMessage ="please entre folder name")]

        // [Required(ErrorMessage ="error")]
        [StringLength(15)]
        [Display(Name ="Введите название новой папки")]
        public string FolderName {get;set;}
    }
}