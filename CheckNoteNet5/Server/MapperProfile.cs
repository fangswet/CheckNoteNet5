using AutoMapper;
using CheckNoteNet5.Shared.Models;
using CheckNoteNet5.Shared.Models.Auth;

namespace CheckNote.Server
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<User, User.Model>();
            CreateMap<Note, Note.Model>().ForMember(m => m.Text, o => o.MapFrom(n => n.Content.Text));
            CreateMap<Note, Note.Entry>();
            CreateMap<Course, Course.Entry>();
            CreateMap<Note.Source, Note.Source.Model>();
            CreateMap<Question, Question.BinaryModel>();
            CreateMap<Question, Question.UnaryModel>();
            CreateMap<Question.Answer, Question.Answer.Model>();
            CreateMap<Tag, Tag.Model>();
            CreateMap<Note, Note.Entry>();
        }
    }
}
