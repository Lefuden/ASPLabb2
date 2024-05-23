using System.ComponentModel;

namespace ASPLabb2.Models;

public class ViewTeacherCourseModel
{
	public List<Teacher> Teachers { get; set; }

	[DisplayName("Course name")]
	public string CourseName { get; set; }
	public int CourseId { get; set; }
}
