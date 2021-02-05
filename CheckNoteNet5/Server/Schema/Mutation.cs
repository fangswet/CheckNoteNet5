using CheckNoteNet5.Server.Schema.Types;
using CheckNoteNet5.Shared.Models.Inputs;
using CheckNoteNet5.Shared.Services;
using GraphQL;
using GraphQL.Types;

namespace CheckNoteNet5.Server.Schema
{
    public class Mutation : ObjectGraphType<object>
    {
        public Mutation(INoteService noteService)
        {
            Field<NoteType>("addNote",
                            arguments: new QueryArguments(new QueryArgument<NonNullGraphType<NoteInputType>> { Name = "note" }),
                            resolve: ctx => noteService.Add(ctx.GetArgument<NoteInput>("note")));
        }
    }
}
