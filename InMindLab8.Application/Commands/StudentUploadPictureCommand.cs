using InMindLab8.Common;
using MediatR;

namespace InMindLab8.Application.Commands;

public class StudentUploadPictureCommand : IRequest<Result<string>>
{
    public int StudentID { get; set; }
    public string PictureName { get; set; }
}