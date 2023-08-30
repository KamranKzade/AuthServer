using AutoMapper;
using AuthServer.Core.Dtos;
using AuthServer.Core.Models;

namespace AuthServer.Service;

class DtoMapper : Profile
{
	public DtoMapper()
	{
		CreateMap<ProductDto, Product>().ReverseMap();
		CreateMap<UserAppDto, UserApp>().ReverseMap();
	}
}
