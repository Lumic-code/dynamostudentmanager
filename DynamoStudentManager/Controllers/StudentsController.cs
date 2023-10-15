using Amazon.DynamoDBv2.DataModel;
using DynamoStudentManager.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DynamoStudentManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly IDynamoDBContext _context;

        public StudentsController(IDynamoDBContext context)
        {
            _context = context;
        }

        [HttpGet("{studentId}")]
        public async Task<IActionResult> GetById(int studentId)
        {
            var student = await _context.LoadAsync<Student>(studentId);
            if (student == null) return NotFound();
            return Ok(student);
        }

        [HttpGet("get-all-students")]
        public async Task<IActionResult> GetAllStudents()
        {
            var studeents = await _context.ScanAsync<Student>(default).GetRemainingAsync();
            return Ok(studeents);
        }

        [HttpPost("add-student")]
        public async Task<IActionResult> CreateStudent(Student studentRequest)
        {
            var student = await _context.LoadAsync<Student>(studentRequest.Id);
            if (student != null) return BadRequest($"Student with Id {studentRequest.Id} already exists.");
            await _context.SaveAsync(studentRequest);
            return Ok(studentRequest);
        }

        [HttpDelete("{studentId}")]
        public async Task<IActionResult> DeleteStudent(int studentId)
        {
            var student = await _context.LoadAsync<Student>(studentId);
            if (student == null) return NotFound();
            await _context.DeleteAsync(student);
            return Ok($"Student with Id {studentId} successfully deleted.");
        }

        [HttpPut("update-student-details")]
        public async Task<IActionResult> UpdateStudent(Student studentRequest)
        {
            var student = await _context.LoadAsync<Student>(studentRequest.Id);
            if (student == null) return NotFound();
            await _context.SaveAsync(studentRequest);
            return Ok(studentRequest);
        }
    }
}
