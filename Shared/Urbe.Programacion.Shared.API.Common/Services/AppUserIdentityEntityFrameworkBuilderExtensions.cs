// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Urbe.Programacion.Shared.Entities.Models;

namespace Urbe.Programacion.Shared.API.Common.Services;

/// <summary>
/// Contains extension methods to <see cref="IdentityBuilder"/> for adding entity framework stores.
/// </summary>
public static class AppUserIdentityEntityFrameworkBuilderExtensions
{
    /// <summary>
    /// Adds an Entity Framework implementation of identity information stores.
    /// </summary>
    /// <typeparam name="TContext">The Entity Framework database context to use.</typeparam>
    /// <param name="builder">The <see cref="IdentityBuilder"/> instance this method extends.</param>
    /// <returns>The <see cref="IdentityBuilder"/> instance this method extends.</returns>
    public static IdentityBuilder AddEntityFrameworkDbContextStores<TAppUser, TDbContext>(this IdentityBuilder builder)
        where TAppUser : BaseAppUser
        where TDbContext : DbContext
    {
        builder.Services.AddScoped<IUserStore<TAppUser>>(x => new AppUserStore<TAppUser, TDbContext>(x.GetRequiredService<TDbContext>()));
        builder.Services.AddScoped<IUserStore<TAppUser>>(x => new AppUserStore<TAppUser, TDbContext>(x.GetRequiredService<TDbContext>()));
        return builder;
    }
}
