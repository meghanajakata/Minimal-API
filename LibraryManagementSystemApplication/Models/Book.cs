using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace LibraryManagementSystemApplication.Models
{
    public class Book
    {
        [Key]
        [JsonIgnore]
        public int BookId { get; set; }
        public string Title { get; set; }
        public DateTime publicationDate { get; set; }
        public string ISBN { get; set; }

        [ForeignKey("Author")]
        public int AuthorID { get; set; }

        [JsonIgnore]
        public virtual Author Author { get; set; }


    }
}
