using Asp.Versioning;
using InMindLab8.Application.Commands;
using InMindLab8.Application.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace InMindLab8.API.Controllers;

[ApiVersion( 1.0 )]
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public class UniversityController : ControllerBase
{
    private readonly IMediator _mediator;

    public UniversityController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("[action]/{adminId:int}")]
    public async Task<IActionResult> CreateCourse([FromRoute]int adminId, [FromBody]  CourseDto course)
    {
        AdminCreateCourseCommand command = new AdminCreateCourseCommand
        {
            AdminId = adminId,
            CourseToBeCreated = course
        };
        var createdCourse = await _mediator.Send(command);
        
        return Ok(createdCourse);
        
    }

    [HttpPost("[action]/{teacherId:int}")]
    public async Task<IActionResult> CreateClass([FromRoute] int teacherId, [FromBody] TeacherCourseDto teacherCourse)
    {

        TeacherCreateClassCommand command = new TeacherCreateClassCommand
        {
            TeacherId = teacherId,
            CourseId = teacherCourse.CourseId,
            ClassStart = teacherCourse.ClassStart,
            ClassEnd = teacherCourse.ClassEnd,
        };
        
        var createdClass = await _mediator.Send(command);
        
        return Ok(createdClass);
        
    }

    
    [HttpPost("[action]/{studentId:int}")]
    public async Task<IActionResult> Enroll([FromRoute] int studentId, [FromBody] EnrollDto enroll)
    {
        StudentEnrollClassCommand command = new StudentEnrollClassCommand
        {
            EnrollId = enroll.Id,
            StudentId = studentId,
            CourseId = enroll.CourseId,
            EnrollDate = enroll.EnrollementDate
        };
        var enrolledCourseResult = await _mediator.Send(command);
        if (enrolledCourseResult.IsSuccess)
        {
            return Ok(enrolledCourseResult.Value);
        }
        return BadRequest(enrolledCourseResult.Error);
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> TeacherSetGrade([FromQuery] int teacherId, [FromQuery] int StudentId,
        [FromQuery] int courseId, [FromQuery] float grade)
    {
        TeacherSetGradeCommand command = new TeacherSetGradeCommand
        {
            CourseId = courseId,
            Grade = grade,
            TeacherId = teacherId,
            StudentId = StudentId
        };
        
        var result = await _mediator.Send(command);

        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }
        return BadRequest(result.Error);
        
    }
    
    [HttpPost("[action]")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UploadImage([FromForm] IFormFile file, [FromQuery] int StudentId)
    {
        if (file == null || file.Length == 0)
            return BadRequest("File not good!");

        try
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", file.FileName);

            // Ensure directory exists
            if (!Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images")))
                Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images"));


            StudentUploadPictureCommand command = new StudentUploadPictureCommand
            {
                StudentID = StudentId,
                PictureName = file.FileName
            };
            var result = await _mediator.Send(command);
            if (!result.IsSuccess)
            {
                return BadRequest(result.Error);
            }
            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            return Ok(result.Value);
            
            
            
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
       
    }
}