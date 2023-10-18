﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Urbe.BasesDeDatos.AppSocial.Entities;
using Urbe.BasesDeDatos.AppSocial.Entities.Models;

namespace Urbe.BasesDeDatos.AppSocial.API.Services;

public class SocialAppUserStore : UserOnlyStore<SocialAppUser, SocialContext, GuidId<SocialAppUser>>
{
    public SocialAppUserStore(SocialContext context, IdentityErrorDescriber? describer = null) : base(context, describer)
    {
    }

    public override Task<SocialAppUser?> FindByIdAsync(string userId, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();
        var id = ConvertIdFromString(userId);
        return Context.Users.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public override GuidId<SocialAppUser> ConvertIdFromString(string? id) 
        => id is not null ? GuidId<SocialAppUser>.Parse(id, null) : default;

    public override string? ConvertIdToString(GuidId<SocialAppUser> id)
        => id.Value.ToString();
}
