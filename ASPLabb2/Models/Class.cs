using System.ComponentModel.DataAnnotations;

namespace ASPLabb2.Models;

public class Class
{
    public int ID { get; set; }

    [StringLength(30, MinimumLength = 3, ErrorMessage = "Name must be within 3 - 30 in length")]
    public string Name { get; set; }
    public ICollection<Student> Students { get; set; } = [];
}
