using ChatApp.Models.Entities;
using ChatApp.Models.ViewModels;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChatApp.Mapping
{
	public class MappingProfile : Profile
	{
		public MappingProfile()
		{
			CreateMapFromViewModelToEntities();
			CreateMapFromEntitiesToViewModels();
		}

		private void CreateMapFromViewModelToEntities()
		{
			CreateMap<UserViewModel, User>();
			CreateMap<PostViewModel, Post>();
		}

		private void CreateMapFromEntitiesToViewModels()
		{
			CreateMap<User, UserViewModel>();
			CreateMap<Post, PostViewModel>();
		}
	}
}