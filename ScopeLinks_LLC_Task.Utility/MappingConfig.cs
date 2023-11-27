using AutoMapper;
using Microsoft.AspNetCore.Http;
using ScopeLinks_LLC_Task.DataAccess.Dtos;
using ScopeLinks_LLC_Task.DataAccess.Entities;
using System;
using System.IO;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Http;

namespace ScopeLinks_LLC_Task.Utility
{
	public class MappingConfig : Profile
	{
		public MappingConfig() 
		{
			/// <summary>
			/// Class Tasks Mapping
			/// </summary>
			CreateMap<Tasks, TaskGetDto>().ReverseMap();
			CreateMap<Tasks, TaskAddDto>().ReverseMap();

			/// <summary>
			/// Class UserTasks Mapping
			/// </summary>
			CreateMap<UserTasks, TaskUserDto>().ReverseMap();
			CreateMap<UserTasks, UserTaskGetDto>().ReverseMap();

			/// <summary>
			/// Class CartItem Mapping
			/// </summary

			CreateMap<CartItem, CartItemGet>().ReverseMap();
			CreateMap<CartItem, CartItemAddDto>().ReverseMap();

			/// <summary>
			/// Class Product Mapping
			/// </summary

			CreateMap<Product, productGetDto>().ReverseMap();
			CreateMap<Product, ProductOneDto>().ReverseMap()
		   .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images));
			CreateMap<Product, ProductAddDto>().ReverseMap();
			CreateMap<Product, ProductUpdateDto>().ReverseMap().
				ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images));




			/// <summary>
			/// Class Product Image Mapping
			/// </summary
			CreateMap<ProductImage, ProductImageDto>().ReverseMap();
			CreateMap<ProductImage, ImageDto>().ReverseMap();


			CreateMap<Order, OrderGetDto>().ReverseMap();




		}
	}
}
