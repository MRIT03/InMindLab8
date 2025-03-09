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
    public class StudentController : ControllerBase
    {
        private readonly IMediator _mediator;
        

        public StudentController( IMediator mediator)
        {
            _mediator = mediator;
            
        }

        // GET: api/student
        [HttpGet("[action]")]
        public async Task<ActionResult<IEnumerable<Student>>> GetAllStudents()
        {
            var students = await _mediator.Send(new RetrieveStudentQuery());
            return Ok(students);
        }

        // GET: api/student/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Student>> GetStudent(int id)
        {
            var students = await _mediator.Send(new RetrieveStudentQuery());
            var student = students.Where(s => s.Id == id);
            if (student == null)
            {
                return NotFound();
            }
            return Ok(student);
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

        
    }
}
