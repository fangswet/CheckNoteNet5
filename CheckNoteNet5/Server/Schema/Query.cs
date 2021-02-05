using CheckNoteNet5.Server.Schema.Types;
using CheckNoteNet5.Server.Services.Extensions;
using CheckNoteNet5.Shared.Services;
using GraphQL;
using GraphQL.Types;

namespace CheckNoteNet5.Server.Schema
{
    public class Query : ObjectGraphType
    {
        public Query(INoteService noteService)
        {
            Field<NoteType>("note",
                            arguments: new QueryArguments(new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "id" }),
                            resolve: ctx => noteService.Get(ctx.GetArgument<int>("id")).MapToValue());

            Field<ListGraphType<NoteEntryType>>("notes",
                                           arguments: new QueryArguments(new QueryArgument<StringGraphType> { Name = "title" }),
                                           resolve: ctx => noteService.List(ctx.GetArgument<string>("title")).MapToValue());
        }
    }
}
