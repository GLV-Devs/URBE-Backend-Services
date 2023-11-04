using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Urbe.Programacion.AppSocial.Entities;
using Urbe.Programacion.AppSocial.Entities.Models;
using Urbe.Programacion.AppSocial.ModelServices.API.Responses;
using Urbe.Programacion.AppSocial.ModelServices.DTOs;

namespace Urbe.Programacion.AppSocial.ModelServices.DTOs.Responses;

public class PostViewModel : IResponseModel
{
    public required long Id { get; init; }
    public required UserViewModel? Poster { get; init; }
    public required Guid PosterId { get; init; }
    public required string Content { get; init; }
    public required string PosterThenUsername { get; init; }
    public required DateTimeOffset DatePosted { get; init; }
    public required long? InResponseTo { get; init; }
    public required HashSet<long>? Responses { get; init; }

    public static PostViewModel FromPost(Post post)
        => new()
        {
            Id = post.Id.AsLong(),
            Poster = post.Poster is not null ? UserViewModel.FromHiddenUser(post.Poster) : null,
            PosterId = post.PosterId,
            Content = post.Content,
            PosterThenUsername = post.PosterThenUsername,
            DatePosted = post.DatePosted,
            InResponseTo = post.InResponseToId?.AsLong(),
            Responses = post.Responses?.Select(x => x.Id.AsLong()).ToHashSet()
        };

    APIResponseCode IResponseModel.APIResponseCode => APIResponseCodeEnum.PostView;
}
