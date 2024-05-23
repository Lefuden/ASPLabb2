using Microsoft.AspNetCore.Mvc.Rendering;

namespace ASPLabb2.Models;

public class UpdateStudentTeacherModel
{
	public int? SelectStudentID { get; set; }
	public Student Student { get; set; }
	public int? SelectCourseID { get; set; }
	public Course? Course { get; set; }
	public int? OldTeacherID { get; set; }
	public Teacher? OldTeacher { get; set; }
	public int? SelectNewTeacherID { get; set; }
	public Teacher? SelectNewTeacher { get; set; }
	public SelectList? CourseList { get; set; }
	public SelectList? TeacherList { get; set; }
}

//I wanted to have this big model that gets updated with info for each step of updating teacher and student, but ran into issues so it's not working as intended.