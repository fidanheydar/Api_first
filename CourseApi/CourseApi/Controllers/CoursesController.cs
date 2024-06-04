using CourseApi.Data;
using CourseApi.Data.Entities;
using CourseApi.Dtos.CourseDtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CourseApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CoursesController(AppDbContext context)
        {
            _context = context;
        }


        [HttpGet("")]
        public ActionResult<List<CourseGetAllDto>> GetAll(int page = 1, int pageSize = 2)
        {
            List<CourseGetAllDto> data = _context.Courses.Skip((page - 1) * pageSize).Take(pageSize).Select(x => new CourseGetAllDto
            {
               Name=x.Name
            }).ToList();

            return StatusCode(200, data);
        }

        [HttpGet("{id}")]
        public ActionResult<CourseGetByIdDto> GetById(int id)
        {
            var data = _context.Courses.Find(id);

            if (data == null)
            {
                return StatusCode(404, data);
            }
            CourseGetByIdDto courseGetByIdDto = new CourseGetByIdDto()
            {
                Id=data.Id,
                Name = data.Name
            };

            return StatusCode(200, courseGetByIdDto);
        }

        [HttpPost("")]
        public ActionResult Create(CourseCreateDto courseDto)
        {
            Course course = new Course
            {
                Name = courseDto.Name
            };

            _context.Courses.Add(course);
            _context.SaveChanges();

            return StatusCode(201);
        }


        [HttpPut("{id}")]
        public ActionResult Update(CourseUpdateDto courseUpdateDto)
        {
            var existCourse = _context.Courses.Find(courseUpdateDto.Id);
            if (existCourse == null)
            {
                return StatusCode(404, courseUpdateDto);
            }

            existCourse.Name = courseUpdateDto.Name;
            _context.Courses.Update(existCourse);
            _context.SaveChanges();

            return StatusCode(200);
        }


        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var  existCourse = _context.Courses.FirstOrDefault(x => x.Id == id);
            if (existCourse == null)
            {
                return NotFound();
            }
            _context.Courses.Remove(existCourse);
            _context.SaveChanges();
            return StatusCode(204);
        }

    }
}
