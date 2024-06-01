using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace LibraryManagementSystemApplication.Models
{
    public class Author
    {
        [Key]
        [JsonIgnore]
        public int AuthorID { get; set; }
        public string Name { get; set; }

        [JsonIgnore]
        public virtual ICollection<Book> Books { get; set; }
    }
}
