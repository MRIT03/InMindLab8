using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using InMindLab8.Application.Commands;
using InMindLab8.Application.Queries;
using InMindLab8.Application.ViewModels;
using InMindLab8.Domain.Entities;
using MediatR;

namespace InMindLab8.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TeacherController : ControllerBase
    {
        private readonly IMediator _mediator;
        

        public TeacherController( IMediator mediator)
        {
            _mediator = mediator;
            
        }

       
        [HttpGet("[action]")]
        public async Task<ActionResult<IEnumerable<Student>>> GetAllStudents()
        {
            var teachers = await _mediator.Send(new RetrieveTeachersQuery());
            return Ok(teachers);
        }

        
        [HttpGet("{id}")]
        public async Task<ActionResult<Student>> GetTeachers(int id)
        {
            var teachers = await _mediator.Send(new RetrieveTeachersQuery());
            var teacher = teachers.Where(s => s.Id == id);
            if (teacher == null)
            {
                return NotFound();
            }
            return Ok(teacher);
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

        
    }
}
