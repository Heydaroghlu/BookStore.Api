using AutoMapper;
using BookStore.Api.Apps.AdminApi.DTOs.AccountDTOs;
using BookStore.Api.DTOs.AuthorDTOs;
using BookStore.Core.Entities;
using System.Collections.Generic;

namespace BookStore.Api.Profiles
{
    public class AppProfile:Profile
    {
        public AppProfile()
        {
            CreateMap<Author, AuthorGetDTO>();
            CreateMap<Author, AuthorListItemDTO>()
                .ForMember(desc => desc.BooksCount, m => m.MapFrom(src => src.Books.Count));//custom 
            CreateMap<AppUser, AccountGetDTO>();
            CreateMap<AccountGetDTO,AppUser >();
            CreateMap<AccountPostDTO, AppUser>();

        }
    }
}
