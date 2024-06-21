using NotesApiNext.ApiTypes;
using NotesApiNext.Interfaces;
using NotesApiNext.Models.Note;
using AutoMapper;

namespace NotesApiNext.Mapping
{
    public class NoteMappingProfile : Profile
    {
        public NoteMappingProfile(IDateTimeProvider dateTimeProvider)
        {
            CreateMap<Note, ListNoteVm>()
                .ForMember(note => note.Id, opt => opt.MapFrom(note => note.Id))
                .ForMember(note => note.Title, opt => opt.MapFrom(note => note.Title))
                .ForMember(note => note.IsCompleted, opt => opt.MapFrom(note => note.IsCompleted));
                
            CreateMap<Note, DetailedNoteVm>()
                .ForCtorParam(nameof(DetailedNoteVm.Id), opt => opt.MapFrom(note => note.Id))
                .ForCtorParam(nameof(DetailedNoteVm.Title), opt => opt.MapFrom(note => note.Title))
                .ForCtorParam(nameof(DetailedNoteVm.Description), opt => opt.MapFrom(note => note.Description))
                .ForCtorParam(nameof(DetailedNoteVm.IsCompleted), opt => opt.MapFrom(note => note.IsCompleted))
                .ForCtorParam(nameof(DetailedNoteVm.Priority), opt => opt.MapFrom(note => note.Priority));

            CreateMap<CreateNoteDto, Note>()
                .ForMember(note => note.Id, opt => opt.MapFrom( _ => Guid.NewGuid()))
                .ForMember(note => note.CreationDateTime, opt => opt.MapFrom( _ => dateTimeProvider.UtcNow))
                .ForMember(note => note.UpdatedDateTime, opt => opt.MapFrom(_ => dateTimeProvider.UtcNow))
                .ForMember(note => note.Priority, opt => opt.MapFrom(noteDto => StringToPriorityConvertor(noteDto.Priority)));
        }

        private static Priority StringToPriorityConvertor(string priorityStr) =>
    Enum.TryParse<Priority>(priorityStr, true, out var priority)
    ? priority
    : default;
    }
}
