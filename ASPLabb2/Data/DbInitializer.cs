using ASPLabb2.Models;

namespace ASPLabb2.Data;

public class DbInitializer(ApplicationDbContext context)
{
	public async Task InitDb()
	{
		await context.Database.EnsureCreatedAsync();

		//if DB is not empty
		if (context.Students.Any())
		{
			return;
		}

		//run methods in order to initialize and add data
		await InitTeachers();
		await InitClasses();
		await InitCourses();
		await InitStudents();
		await InitStudentCourses();
		await InitTeacherCourses();
	}

	private async Task InitTeachers()
	{
		var teachers = new List<Teacher>
		{
			new() { Name = "Teachy McTeachFace" },
			new() { Name = "Professor Snuggles" },
			new() { Name = "Dr Doolittle" },
			new() { Name = "King Kong Sr" },
			new() { Name = "Lisbeth Lisplip" },
			new() { Name = "Nils Carlsson Pyssling" }
		};

		foreach (var teacher in teachers)
		{
			await context.Teachers.AddAsync(teacher);
		}

		await context.SaveChangesAsync();
	}

	private async Task InitClasses()
	{
		var classes = new List<Class>
		{
			new() { Name = "NET23" },
			new() { Name = "NET24" }
		};

		foreach (var cass in classes)
		{
			await context.Classes.AddAsync(cass);
		}

		await context.SaveChangesAsync();
	}

	private async Task InitCourses()
	{
		var courses = new List<Course>
		{
			new() { Name = "Programming 1" },
			new() { Name = "Programming 2" },
			new() { Name = "Procrastination" },
			new() { Name = "Deadline sprinting" },
			new() { Name = "Tree climbing" },
			new() { Name = "Advanced knitting" }
		};
		foreach (var course in courses)
		{
			await context.Courses.AddAsync(course);
		}

		await context.SaveChangesAsync();
	}

	private async Task InitStudents()
	{
		var students = new List<Student>
		{
			new() { Name = "Monke Bananaman", ClassId = 1 },
			new() { Name = "Gorilla Strongarm", ClassId = 1 },
			new() { Name = "Macaque McQuack", ClassId = 1 },
			new() { Name = "Ape Apeson", ClassId = 1 },
			new() { Name = "Carl Svenson", ClassId = 1 },
			new() { Name = "King Kong Jr", ClassId = 1 },
			new() { Name = "Bob Bobsled", ClassId = 2 },
			new() { Name = "Leif Wingman", ClassId = 2 },
			new() { Name = "Baboon Babsan", ClassId = 2 },
			new() { Name = "Baboon Jr", ClassId = 2 },
			new() { Name = "Macaque Jr", ClassId = 2 }
		};

		foreach (var student in students)
		{
			await context.Students.AddAsync(student);
		}

		await context.SaveChangesAsync();
	}

	private async Task InitStudentCourses()
	{
		var studentCourse = new List<StudentCourse>
		{
			new() { StudentId = 1, CourseId = 1 },
			new() { StudentId = 1, CourseId = 2 },
			new() { StudentId = 1, CourseId = 3 },
			new() { StudentId = 2, CourseId = 4 },
			new() { StudentId = 2, CourseId = 5 },
			new() { StudentId = 2, CourseId = 6 },
			new() { StudentId = 3, CourseId = 1 },
			new() { StudentId = 3, CourseId = 2 },
			new() { StudentId = 3, CourseId = 3 },
			new() { StudentId = 4, CourseId = 4 },
			new() { StudentId = 4, CourseId = 5 },
			new() { StudentId = 5, CourseId = 6 },
			new() { StudentId = 5, CourseId = 1 },
			new() { StudentId = 5, CourseId = 2 },
			new() { StudentId = 5, CourseId = 3 },
			new() { StudentId = 6, CourseId = 4 },
			new() { StudentId = 6, CourseId = 5 },
			new() { StudentId = 6, CourseId = 6 },
			new() { StudentId = 6, CourseId = 1 },
			new() { StudentId = 6, CourseId = 2 },
			new() { StudentId = 7, CourseId = 3 },
			new() { StudentId = 8, CourseId = 4 },
			new() { StudentId = 9, CourseId = 5 },
			new() { StudentId = 9, CourseId = 6 },
			new() { StudentId = 9, CourseId = 1 },
			new() { StudentId = 10, CourseId = 2 },
			new() { StudentId = 10, CourseId = 3 },
			new() { StudentId = 10, CourseId = 4 },
			new() { StudentId = 10, CourseId = 5 },
			new() { StudentId = 11, CourseId = 6 }
		};
		foreach (var studcour in studentCourse)
		{
			await context.StudentCourses.AddAsync(studcour);
		}

		await context.SaveChangesAsync();
	}

	private async Task InitTeacherCourses()
	{
		var teacherCourse = new List<TeacherCourse>
		{
			new() { TeacherId = 1, CourseId = 1 },
			new() { TeacherId = 1, CourseId = 2 },
			new() { TeacherId = 2, CourseId = 3 },
			new() { TeacherId = 2, CourseId = 4 },
			new() { TeacherId = 3, CourseId = 5 },
			new() { TeacherId = 3, CourseId = 6 },
			new() { TeacherId = 4, CourseId = 1 },
			new() { TeacherId = 4, CourseId = 2 },
			new() { TeacherId = 5, CourseId = 3 },
			new() { TeacherId = 5, CourseId = 4 },
			new() { TeacherId = 6, CourseId = 5 },
			new() { TeacherId = 6, CourseId = 6 }
		};
		foreach (var teachcour in teacherCourse)
		{
			await context.TeachersCourses.AddAsync(teachcour);
		}
		await context.SaveChangesAsync();
	}
}