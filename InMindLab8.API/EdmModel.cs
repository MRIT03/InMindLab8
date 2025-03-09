using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using InMindLab8.Application.ViewModels;

namespace InMindLab8.API
{
    public static class EdmModelBuilder
    {
        public static IEdmModel GetEdmModel()
        {
            var builder = new ODataConventionModelBuilder();

            
            builder.EntitySet<StudentDto>("Students");
            builder.EntitySet<CourseDto>("Courses");
            builder.EntityType<StudentDto>().Property(x => x.Name).IsFilterable();
           
            var teacher = builder.EntitySet<TeacherDto>("Teachers").EntityType;
            teacher.HasKey(t => t.Id);
            teacher.Property(t => t.Name);
            
            
            //These are apparently not supported
            teacher.Ignore(t => t.ScheduleStart);
            teacher.Ignore(t => t.ScheduleEnd);
            

            return builder.GetEdmModel();
        }
    }
}