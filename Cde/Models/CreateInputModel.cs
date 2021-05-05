using System.ComponentModel.DataAnnotations;

namespace Cde.Models
{
    public interface ICreateFileInputModel
    {
        public string ProjectName { get; set; }
    }
    
    public record CreateFileInputModel : FileInputModel, ICreateFileInputModel
    {
        [Required]
        [StringLength(50)]
        public string ProjectName { get; set; }
    }

    public record CreateFileTextInputModel : FileTextInputModel, ICreateFileInputModel
    {
        [Required]
        [StringLength(50)]
        public string ProjectName { get; set; }
    }
}
