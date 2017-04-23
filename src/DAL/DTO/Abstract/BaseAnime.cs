using System.ComponentModel.DataAnnotations;

namespace DAL_Database.DTO.Abstract
{
    public abstract class BaseAnime
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string OriginalTitle { get; set; }
        public string EnglishTitle { get; set; }
    }
}
