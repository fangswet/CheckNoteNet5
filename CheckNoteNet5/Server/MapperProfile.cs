using AutoMapper;
using CheckNoteNet5.Shared.Models;
using CheckNoteNet5.Shared.Models.Dtos;
using CheckNoteNet5.Shared.Models.Inputs;

namespace CheckNote.Server
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<User, UserModel>();

            CreateMap<NoteInput, Note>().ForMember(n => n.Content, o => o.MapFrom(i => new Content { Text = i.Text }));
            CreateMap<Note, NoteModel>().ForMember(m => m.Text, o => o.MapFrom(n => n.Content.Text));
            CreateMap<Note, NoteEntry>();

            CreateMap<Course, CourseEntry>();

            CreateMap<Source, SourceModel>();

            CreateMap<QuestionInput, Question>();
            CreateMap<Question, BinaryQuestionModel>();
            CreateMap<Question, UnaryQuestionModel>();

            CreateMap<AnswerInput, Answer>();
            CreateMap<Answer, AnswerModel>();

            CreateMap<Tag, TagModel>();
        }
    }
}
