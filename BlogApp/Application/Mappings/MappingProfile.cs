using System.ComponentModel;
using AutoMapper;
using BlogApp.Application.DTOs.Comment;
using BlogApp.Application.DTOs.Author;
using BlogApp.Application.DTOs.Post;
using BlogApp.Domain.Entities;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BlogApp.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Author, AuthorDto>()
                 .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.GetFullName()))
                 .ForMember(dest => dest.PostCount, opt => opt.MapFrom(src => src.Posts != null ? src.Posts.Count : 0));


            CreateMap<CreateAuthorDto, Author>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.Now));
            CreateMap<UpdateAuthorDto, Author>()
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());

            CreateMap<Post, PostDto>()
                 .ForMember(dest => dest.DaysSincePublished, opt => opt.MapFrom(src => src.GetDaysSincePublished()))
                 .ForMember(dest => dest.AuthorName, opt => opt.MapFrom(src => src.Author != null ? src.Author.GetFullName() : string.Empty));

            CreateMap<CreatePostDto, Post>()
             .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.Now));

            CreateMap<UpdatePostDto, Post>()
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());


            CreateMap<Comment, CommentDto>()
                .ForMember(dest => dest.CanBeDeleted, opt => opt.MapFrom(src => src.CanBeDeleted()))
                .ForMember(dest => dest.PostTitle, opt => opt.MapFrom(src => src.Post != null ? src.Post.Title : string.Empty))
                .ForMember(dest => dest.AuthorName, opt => opt.MapFrom(src => src.Post != null && src.Post.Author != null ? src.Post.Author.GetFullName() : string.Empty));

            CreateMap<CreateCommentDto, Comment>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => "Pending"))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.Now));

            CreateMap<UpdateCommentDto, Comment>()
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());

        }
    }
}