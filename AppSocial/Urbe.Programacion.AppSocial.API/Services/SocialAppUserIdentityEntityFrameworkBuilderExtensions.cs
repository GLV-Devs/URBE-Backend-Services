// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Identity;
using Urbe.Programacion.AppSocial.Entities;
using Urbe.Programacion.AppSocial.Entities.Models;

namespace Urbe.Programacion.AppSocial.API.Services;

/// <summary>
/// Contains extension methods to <see cref="IdentityBuilder"/> for adding entity framework stores.
/// </summary>
public static class SocialAppUserIdentityEntityFrameworkBuilderExtensions
{
    /// <summary>
    /// Adds an Entity Framework implementation of identity information stores.
    /// </summary>
    /// <typeparam name="TContext">The Entity Framework database context to use.</typeparam>
    /// <param name="builder">The <see cref="IdentityBuilder"/> instance this method extends.</param>
    /// <returns>The <see cref="IdentityBuilder"/> instance this method extends.</returns>
    public static IdentityBuilder AddEntityFrameworkSocialContextStores(this IdentityBuilder builder)
    {
        builder.Services.AddScoped<IUserStore<SocialAppUser>>(x => new SocialAppUserStore(x.GetRequiredService<SocialContext>()));
        return builder;
    }
}
