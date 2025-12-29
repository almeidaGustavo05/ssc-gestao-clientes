using AutoMapper;
using GestaoClientes.Application.Features.Clientes.Queries.ObtemClientePorId;
using GestaoClientes.Domain.Clientes;

namespace GestaoClientes.Application.Mappings
{
    public class ClienteProfile : Profile
    {
        public ClienteProfile()
        {
            CreateMap<Cliente, ClienteDto>()
                .ForMember(dest => dest.Cnpj, opt => opt.MapFrom(src => src.Cnpj.Valor));
        }
    }
}