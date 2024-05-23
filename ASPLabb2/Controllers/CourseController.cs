using ASPLabb2.Data;
using ASPLabb2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ASPLabb2.Controllers;
public class CourseController : Controller
{
	private readonly ApplicationDbContext _context;
	public CourseController(ApplicationDbContext context)
	{
		_context = context;
	}

	public IActionResult Index()
	{
		return View();
	}

	//Hämta alla lärare som undervisar i “programmering 1”
	[HttpGet]
	public IActionResult FindTeacherCourse()
	{
		ViewData["CourseId"] = new SelectList(_context.Courses, "ID", "Name");
		return View();
	}

	[HttpPost]
	public async Task<IActionResult> FindTeacherCourse(int? id)
	{
		if (id == null) return View();
		var viewTeacherCourseModel = await _context.Courses
			.Where(c => c.ID == id)
			.Include(c => c.TeacherCourses)
			.ThenInclude(tc => tc.Teacher)
			.Select(c => new ViewTeacherCourseModel
			{
				CourseId = c.ID,
				CourseName = c.Name,
				Teachers = c.TeacherCourses.Select(tc => tc.Teacher).ToList()
			}).FirstOrDefaultAsync();

		return View("ViewTeacherCourse", viewTeacherCourseModel);
	}

	public IActionResult ViewTeacherCourse(ViewTeacherCourseModel viewTeacherCourseModel)
	{
		return View(viewTeacherCourseModel);
	}

	//Hämta alla elever med deras lärare, skriv ut både elevernas namn och namnet på alla lärare de har
	[HttpGet]
	public async Task<IActionResult> GetStudentsAndTeachers()
	{
		var viewStudentTeacherModelList = await _context.Students
			.Include(s => s.StudentCourses)
			.ThenInclude(sc => sc.Course)
			.ThenInclude(c => c.TeacherCourses)
			.ThenInclude(tc => tc.Teacher)
			.Select(s => new ViewStudentTeacherModel
			{
				Student = s,
				Teachers = s.StudentCourses
					.SelectMany(sc => sc.Course.TeacherCourses.Select(tc => tc.Teacher))
					.Distinct()
					.ToList()
			}).ToListAsync();

		return View(viewStudentTeacherModelList);
	}

	//Hämta alla elever som läser “programmering 1” och skriv ut deras namn samt vilka lärare de har i den kursen
	[HttpGet]
	public IActionResult FindStudentTeacherCourse()
	{
		ViewData["CourseId"] = new SelectList(_context.Courses, "ID", "Name");
		return View();
	}

	[HttpPost]
	public async Task<IActionResult> FindStudentTeacherCourse(int? id)
	{
		if (id == null) return View();

		var viewStudentTeacherCourseModel = await _context.Courses
			.Where(c => c.ID == id)
			.Include(c => c.TeacherCourses)
			.Include(c => c.StudentCourses)
			.Select(c => new ViewStudentTeacherCourseModel
			{
				CourseId = c.ID,
				CourseName = c.Name,
				Teachers = c.TeacherCourses.Select(tc => tc.Teacher).ToList(),
				Students = c.StudentCourses.Select(sc => sc.Student).ToList()
			}).FirstOrDefaultAsync();

		return View("ViewStudentTeacherCourse", viewStudentTeacherCourseModel);
	}

	public IActionResult ViewStudentTeacherCourse(ViewStudentTeacherCourseModel viewStudentTeacherCourseModel)
	{
		return View(viewStudentTeacherCourseModel);
	}

	//Editera ett ämne från “programmering 2” till “OOP”
	[HttpGet]
	public async Task<IActionResult> FindEditCourse()
	{
		return View(await _context.Courses.ToListAsync());
	}

	[HttpGet]
	public async Task<IActionResult> EditCourse(int? id)
	{
		if (id == null) return NotFound();
		var course = await _context.Courses.FindAsync(id);
		if (course == null) return NotFound();

		return View(course);
	}

	[HttpPost]
	public async Task<IActionResult> EditCourse(int id, [Bind("ID,Name")] Course course)
	{
		if (id != course.ID) return NotFound();
		if (!ModelState.IsValid) return View(course);

		try
		{
			_context.Update(course);
			await _context.SaveChangesAsync();
		}
		catch (Exception e)
		{
			Console.WriteLine(e);
			throw;
		}

		return RedirectToAction(nameof(FindEditCourse));
	}

	//Uppdatera en elevs lärare i “programmering1” från Reidar till Tobias.
	[HttpGet]
	public async Task<IActionResult> FindStudent() //step 1 - get student ID
	{
		ViewData["StudentId"] = new SelectList(await _context.Students.ToListAsync(), "ID", "Name");
		UpdateStudentTeacherModel model = new();
		return View(model);
	}

	[HttpPost]
	public async Task<IActionResult> FindStudentPost(int? selectStudentID) //step 1.5 - get courses related to student ID
	{
		if (selectStudentID == null) return RedirectToAction(nameof(FindStudent));

		//var student = await _context.StudentCourses
		//	.Select(sc => sc.Student)
		//	.FirstOrDefaultAsync(sc => sc.ID == studentId);

		var model = await _context.StudentCourses
			.Where(sc => sc.StudentId == selectStudentID)
			//.Include(sc => sc.Course)
			.Include(sc => sc.Student)
			.Select(sc => new UpdateStudentTeacherModel
			{
				SelectStudentID = selectStudentID,
				Student = sc.Student,
				Course = null,
				CourseList = null,
				TeacherList = null
			}).FirstOrDefaultAsync();
		if (model.Student == null)
		{
			return NotFound();
		}
		model.CourseList = new SelectList(await _context.StudentCourses
			.Where(s => s.StudentId == selectStudentID)
			//.Include(s => s.Course)
			.Select(s => s.Course)
			.ToListAsync(), "ID", "Name");

		//return RedirectToAction("FindStudentCourse", new
		//{
		//	model = new UpdateStudentTeacherModel()
		//	{
		//		Student = student,
		//		CourseList = courseList,
		//	}
		//});
		return View("FindStudentCourse", model);
	}
	[HttpGet]
	public IActionResult FindStudentCourse(UpdateStudentTeacherModel model) //step 2 - get course ID related to student
	{
		return View(model);
	}

	[HttpPost]
	public async Task<IActionResult> FindStudentCoursePost(UpdateStudentTeacherModel model) //step 2.5 - add selected Course, add list of teachers related to course
	{
		if (model.SelectCourseID == null) return View("FindStudentCourse", model);

		var newModel = await _context.TeachersCourses
			.Where(tc => tc.CourseId == model.SelectCourseID)
			.Include(tc => tc.Course)
			.Include(tc => tc.Teacher)
			.Select(tc => new UpdateStudentTeacherModel
			{
				SelectStudentID = model.SelectStudentID,
				Student = model.Student,
				SelectCourseID = model.SelectCourseID,
				Course = tc.Course,
				OldTeacherID = tc.TeacherId,
				OldTeacher = tc.Teacher,
				CourseList = model.CourseList,
				TeacherList = null
			}).FirstOrDefaultAsync();

		//newModel.Teacher = await _context.TeachersCourses
		//	.Where(tc => tc.CourseId == model.SelectCourseID)
		//	.Include(tc => tc.Teacher)
		//	.Select(tc => tc.Teacher)
		//	.FirstOrDefaultAsync();
		//newModel.OldTeacherID = model.Teacher.ID;

		if (newModel.OldTeacher == null) return View("FindStudentCourse", model);

		newModel.TeacherList = new SelectList(_context.Teachers
			.Where(t => _context.TeachersCourses
				.Any(tc => tc.CourseId == newModel.SelectCourseID && tc.TeacherId == t.ID) && t.ID != newModel.OldTeacher.ID)
			.ToList(), "ID", "Name");

		return View("ChangeTeacher", newModel);
	}

	[HttpGet]
	public IActionResult ChangeTeacher(UpdateStudentTeacherModel model) //step 3 - select teacher ID related to student AND course to CHANGE to
	{
		return View(model);
	}

	//[HttpPost]
	//public async Task<IActionResult> ChangeTeacherPost2(UpdateStudentTeacherModel model) //in SC, change CourseId to CourseId related to "new" teacherId in TC
	//{
	//	if (model.SelectNewTeacherID == null) return View("ChangeTeacher", model);

	//	var currentTeacherCourse = await _context.StudentCourses
	//		.Where(sc => sc.CourseId == model.SelectCourseID && sc.StudentId == model.SelectStudentID)
	//		.FirstOrDefaultAsync();

	//	var newTeacherCourse = await _context.TeachersCourses
	//		.Where(tc => tc.TeacherId == model.SelectNewTeacherID && tc.CourseId == model.SelectCourseID)
	//		.FirstOrDefaultAsync();

	//	if (currentTeacherCourse == null || newTeacherCourse == null) return NotFound();

	//	try
	//	{
	//		_context.TeachersCourses.Remove(currentTeacherCourse);
	//		await _context.SaveChangesAsync();

	//		StudentCourse updatedStudentCourse = new()
	//		{
	//			StudentId = currentTeacherCourse.StudentId,
	//			CourseId = newTeacherCourse.CourseId
	//		};

	//		_context.StudentCourses.Add(updatedStudentCourse);
	//		await _context.SaveChangesAsync();
	//	}
	//	catch (Exception e)
	//	{
	//		Console.WriteLine(e);
	//		throw;
	//	}

	//	return View("Index");
	//}

	[HttpPost]
	public async Task<IActionResult> ChangeTeacherPost(UpdateStudentTeacherModel model)
	{
		var existingTeacherCourse = _context.TeachersCourses
			.FirstOrDefault(tc => tc.TeacherId == model.OldTeacherID && tc.CourseId == model.SelectCourseID);

		var newTeacher = _context.Teachers.FirstOrDefault(t => t.ID == model.SelectNewTeacherID);
		if (existingTeacherCourse == null || newTeacher == null)
		{
			return NotFound();
		}

		try
		{
			_context.TeachersCourses.Remove(existingTeacherCourse);
			await _context.SaveChangesAsync();

			TeacherCourse newTeacherCourse = new()
			{
				TeacherId = (int)model.SelectNewTeacherID!,
				CourseId = (int)model.SelectCourseID!
			};
			if (!TeacherCourseExists(newTeacherCourse.TeacherId, newTeacherCourse.CourseId))
			{
				_context.TeachersCourses.Add(newTeacherCourse);
				await _context.SaveChangesAsync();
			}
			await Console.Out.WriteLineAsync("alert vi kan inte lägga till denna i databasen");
		}
		catch (DbUpdateConcurrencyException)
		{
			if (!TeacherCourseExists(existingTeacherCourse.TeacherId, existingTeacherCourse.CourseId))
			{
				return NotFound();
			}
			else
			{
				throw;
			}
		}
		//pga felplanerad struktur i databasen kommer alla elever i vald kurs byta lärare,
		//men logiken är korrekt. det skulle krävas en fullständig omstrukturering av databasen för att korrigera.
		return View("Index");
	}
	private bool TeacherCourseExists(int teacherId, int courseId)
	{
		return _context.TeachersCourses.Any(tc => tc.TeacherId == teacherId && tc.CourseId == courseId);
	}

	//public async Task<IActionResult> ChangeTeacherPost(UpdateStudentTeacherModel model) //in SC, change CourseId to CourseId related to "new" teacherId in TC
	//{
	//	if (model.SelectNewTeacherID == null) return View("ChangeTeacher", model);

	//	var currentStudentCourse = await _context.StudentCourses
	//		.Where(sc => sc.CourseId == model.SelectCourseID && sc.StudentId == model.SelectStudentID)
	//		.FirstOrDefaultAsync();

	//	var newStudentCourse = await _context.TeachersCourses
	//		.Where(tc => tc.TeacherId == model.SelectNewTeacherID && tc.CourseId == model.SelectCourseID)
	//		.FirstOrDefaultAsync();

	//	if (currentStudentCourse == null || newStudentCourse == null) return NotFound();

	//	try
	//	{
	//		_context.StudentCourses.Remove(currentStudentCourse);
	//		await _context.SaveChangesAsync();

	//		StudentCourse updatedStudentCourse = new()
	//		{
	//			StudentId = currentStudentCourse.StudentId,
	//			CourseId = newStudentCourse.CourseId
	//		};

	//		_context.StudentCourses.Add(updatedStudentCourse);
	//		await _context.SaveChangesAsync();
	//	}
	//	catch (Exception e)
	//	{
	//		Console.WriteLine(e);
	//		throw;
	//	}

	//	return View("Index");
	//}
}
