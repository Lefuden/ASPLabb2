using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ASPLabb2.Models;

public class Student
{
    public int ID { get; set; }

    [StringLength(30, MinimumLength = 3, ErrorMessage = "Name must be within 3 - 30 in length")]
    public string Name { get; set; }
    public int ClassId { get; set; }
    public Class Class { get; set; }

    [DisplayName("Student Courses")]
    public ICollection<StudentCourse> StudentCourses { get; set; } = [];

    //[DisplayName("Student Teachers")]
    //public ICollection<StudentTeacher> StudentTeachers { get; set; } = [];
}
