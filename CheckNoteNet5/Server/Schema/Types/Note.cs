using CheckNoteNet5.Shared.Models;
using GraphQL.Types;

namespace CheckNoteNet5.Server.Schema.Types
{
    public class NoteType : ObjectGraphType<NoteModel>
    {
        public NoteType()
        {
            Field(n => n.Id);
            Field(n => n.Title);
            Field(n => n.Description);
            Field(n => n.Text);
            Field<NoteType>("parent", resolve: ctx => ctx.Source.Parent);
        }
    }

    public class NoteEntryType : ObjectGraphType<NoteEntry>
    {
        public NoteEntryType()
        {
            Field(n => n.Id);
            Field(n => n.Title);
            Field(n => n.Description);
        }
    }

    public class NoteInputType : InputObjectGraphType
    {
        public NoteInputType()
        {
            Field<NonNullGraphType<StringGraphType>>("title");
            Field<StringGraphType>("description");
            Field<NonNullGraphType<StringGraphType>>("text");
        }
    }
}
