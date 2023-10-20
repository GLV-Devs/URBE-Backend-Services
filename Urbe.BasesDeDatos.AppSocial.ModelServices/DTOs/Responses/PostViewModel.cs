using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Urbe.BasesDeDatos.AppSocial.Entities;
using Urbe.BasesDeDatos.AppSocial.Entities.Models;
using Urbe.BasesDeDatos.AppSocial.ModelServices.API.Responses;

namespace Urbe.BasesDeDatos.AppSocial.ModelServices.DTOs;

public class PostViewModel : IResponseModel
{
    public required long Id { get; init; }
    public required Guid Poster { get; init; }
    public required string Content { get; init; }
    public required string PosterThenUsername { get; init; }
    public required DateTimeOffset DatePosted { get; init; }
    public required long ? InResponseTo { get; init; }
    public required HashSet<long>? Responses { get; init; }

    public static PostViewModel FromPost(Post post)
        => new()
        {
            Id = post.Id.AsLong(),
            Poster = post.PosterId,
            Content = post.Content,
            PosterThenUsername = post.PosterThenUsername,
            DatePosted = post.DatePosted,
            InResponseTo = post.InResponseToId?.AsLong(),
            Responses = post.Responses?.Select(x => x.Id.AsLong()).ToHashSet()
        };

    APIResponseCode IResponseModel.APIResponseCode => APIResponseCodeEnum.PostView;
}
