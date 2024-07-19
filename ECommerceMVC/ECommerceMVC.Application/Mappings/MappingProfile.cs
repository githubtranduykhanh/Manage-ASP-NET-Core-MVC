using AutoMapper;
using ECommerceMVC.Application.Dtos;
using ECommerceMVC.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceMVC.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<DbProduct, ProductDto>().ReverseMap();
            // Cấu hình các map khác nếu cần
        }
    }
}
