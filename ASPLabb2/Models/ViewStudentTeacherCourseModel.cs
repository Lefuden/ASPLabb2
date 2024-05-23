using System.ComponentModel;

namespace ASPLabb2.Models;

public class ViewStudentTeacherCourseModel
{
	public List<Student> Students { get; set; }
	public List<Teacher> Teachers { get; set; }

	[DisplayName("Course name")]
	public string CourseName { get; set; }
	public int CourseId { get; set; }
}
