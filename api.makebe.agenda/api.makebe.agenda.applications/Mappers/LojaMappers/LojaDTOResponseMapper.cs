using api.makebe.agenda.applications;
using api.makebe.agenda.domain.DTO;
using AutoMapper;

public class LojaDTOResponseMapper : Profile
{
    public LojaDTOResponseMapper()
    {

        CreateMap<LojaEnderecoDTO, LojaResponse>()
            .ReverseMap();
    }
}
