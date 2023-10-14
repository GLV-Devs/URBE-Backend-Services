using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Urbe.BasesDeDatos.AppSocial.Entities;
using Urbe.BasesDeDatos.AppSocial.Entities.Models;

namespace Urbe.BasesDeDatos.AppSocial.DatabaseServices.DTOs;

public class PostViewModel
{
    public required SnowflakeId<Post> Id { get; init; }
    public required GuidId<SocialAppUser> Poster { get; init; }
    public required string Content { get; init; }
    public required string PosterThenUsername { get; init; }
    public required DateTimeOffset DatePosted { get; init; }
    public required SnowflakeId<Post>? InResponseTo { get; init; }
    public required HashSet<SnowflakeId<Post>>? Responses { get; init; }

    public static PostViewModel FromPost(Post post)
        => new()
        {
            Id = post.Id,
            Poster = post.PosterId,
            Content = post.Content,
            PosterThenUsername = post.PosterThenUsername,
            DatePosted = post.DatePosted,
            InResponseTo = post.InResponseToId,
            Responses = post.Responses?.Select(x => x.Id).ToHashSet()
        };
}
