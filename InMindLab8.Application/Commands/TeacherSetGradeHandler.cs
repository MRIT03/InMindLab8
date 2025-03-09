using System.Runtime.InteropServices.JavaScript;
using InMindLab8.Domain.Entities;
using InMindLab5.Persistence.Data.Repositories;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using InMindLab8.Common;

namespace InMindLab8.Application.Commands;

public class TeacherSetGradeHandler : IRequestHandler<TeacherSetGradeCommand, Result<String>>
{
    private readonly IRepository<TeacherCourse> _teacherCourseRepository;
    private readonly IRepository<Student> _studentRepository;
    private readonly IRepository<Enroll> _enrollRepository;

    public TeacherSetGradeHandler(IRepository<Enroll> enrollRepository, IRepository<Student> studentRepository,
        IRepository<TeacherCourse> teacherCourseRepository)
    {
        _enrollRepository = enrollRepository;
        _studentRepository = studentRepository;
        _teacherCourseRepository = teacherCourseRepository;
    }

    public async Task<Result<String>> Handle(TeacherSetGradeCommand request, CancellationToken cancellationToken)
    {
        var teacherCourse = await _teacherCourseRepository.GetAllAsync();
        bool courseIsTaughtByTeacher = teacherCourse.Where(x => x.CourseId == request.CourseId && x.TeacherId== request.TeacherId)
            .ToList()
            .Count > 0;

        if (!courseIsTaughtByTeacher)
        {
            return Result<String>.Failure("Teacher not teaching this course");
        }

        if (request.Grade > 20.0 || request.Grade < 0.0)
        {
            return Result<String>.Failure("Invalid grade");
        }
        
        var students = await _studentRepository.GetAllAsync();
        
        Student student = students.FirstOrDefault(x => x.StudentId == request.StudentId);
        if (student == null)
        {
            return Result<String>.Failure("Student not found");
        }
        
        var enrolls = await _enrollRepository.GetAllAsync();
        enrolls = enrolls.Where(x => request.StudentId == x.StudentId).ToList();
        Enroll enroll = enrolls.FirstOrDefault(x => x.CourseId == request.CourseId, null);
        if (enroll == null)
        {
            return Result<String>.Failure("Student not enrolled in this course");
        }
        enroll.Grade = request.Grade;
        float gpa = 0;
        
        foreach( var ens in enrolls)
        {
            gpa += ens.Grade.Value;
        }
        gpa /= enrolls.Count();
        student.GradePointAverage = gpa;
        if (gpa > 15.0)
        {
            student.canApplyToFrance = true;
        }
        else
        {
            student.canApplyToFrance = false;
        }

        await _enrollRepository.SaveAsync();
        await _studentRepository.SaveAsync();
        
        return Result<String>.Success("Grade added successfully");
    }
}