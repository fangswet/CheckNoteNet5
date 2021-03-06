﻿using AutoMapper;
using CheckNoteNet5.Shared.Models;
using CheckNoteNet5.Shared.Services;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheckNoteNet5.Server.Services
{
    public class TagService
    {
        private readonly CheckNoteContext dbContext;
        private readonly IMapper mapper;

        public TagService(CheckNoteContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public async Task<ServiceResult<List<NoteEntry>>> GetNotes(int id)
        {
            var query = dbContext.Tags.Where(t => t.Id == id);
            if (await query.FirstOrDefaultAsync() == null) return ServiceResult<List<NoteEntry>>.MakeError<NotFoundError>();

            var notes = await mapper.ProjectTo<NoteEntry>(query.SelectMany(t => t.Notes)).ToListAsync();
            return ServiceResult.MakeOk(notes);
        }
    }
}
