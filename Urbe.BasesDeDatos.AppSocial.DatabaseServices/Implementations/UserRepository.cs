using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Client;
using Urbe.BasesDeDatos.AppSocial.Common;
using Urbe.BasesDeDatos.AppSocial.DatabaseServices.DTOs;
using Urbe.BasesDeDatos.AppSocial.Entities;
using Urbe.BasesDeDatos.AppSocial.Entities.Interfaces;
using Urbe.BasesDeDatos.AppSocial.Entities.Models;
using Urbe.BasesDeDatos.AppSocial.Services.Attributes;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Urbe.BasesDeDatos.AppSocial.DatabaseServices.Implementations;

[RegisterService(typeof(IEntityCRUDRepository<SocialAppUser, Guid, UserCreationModel, UserUpdateModel>))]
[RegisterService(typeof(IUserRepository))]
public class UserRepository : EntityCRUDRepository<SocialAppUser, Guid, UserCreationModel, UserUpdateModel>, IUserRepository
{
    protected readonly UserManager<SocialAppUser> userManager;

    public UserRepository(SocialContext context, UserManager<SocialAppUser> userManager, IServiceProvider provider) : base(context, provider)
    {
        this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
    }

    public override async ValueTask<ErrorList> Update(SocialAppUser? requester, SocialAppUser entity, UserUpdateModel update)
    {
        var errors = new ErrorList();

        if (requester is null || requester.Id != entity.Id)
        {
            errors.AddError(ErrorMessages.NoPermission());
            errors.RecommendedCode = System.Net.HttpStatusCode.Unauthorized;
        }

        if (Helper.IsUpdating(entity.Email, update.Email)
                && Helper.IsTooLong(ref errors, update.Email, SocialAppUser.EmailMaxLength, "Correo electronico") is false)
        {
            errors.AddError(ErrorMessages.NotSupported("Correo Electrónico", "Cambiar"));
            //var match = Regexes.Email().Match(update.Email);
            //if (match.Success is false || match.Length != update.Email.Length)
            //    errors.AddError(ErrorMessages.BadEmail(update.Email));
            //else
            //{
            //    userManager.Email
            //    entity.EmailConfirmed = false;
            //    entity.Email = update.Email;
            //}
        }

        if (Helper.IsUpdating(entity.UserName, update.Username)
                && Helper.IsTooLong(ref errors, update.Username, SocialAppUser.UserNameMaxLength, "Nombre de Usuario") is false)
        {
            if (await userManager.FindByNameAsync(update.Username) is not null)
                errors.AddError(ErrorMessages.AlreadyInUseByOtherUser("Nombre de Usuario", update.Username));
            else
            {
                var result = await userManager.SetUserNameAsync(entity, update.Username);
                if (result.Succeeded is false)
                    errors.AddError(ErrorMessages.BadUsername(update.Username));
            }
        }

        if (Helper.IsUpdating(entity.RealName, update.RealName, StringComparison.Ordinal)
                && Helper.IsTooLong(ref errors, update.RealName, SocialAppUser.RealNameMaxLength, "Nombre Real") is false)
            entity.RealName = update.RealName;

        if (Helper.IsUpdating(entity.Pronouns, update.Pronouns, StringComparison.Ordinal)
                && Helper.IsTooLong(ref errors, update.Pronouns, SocialAppUser.PronounsMaxLength, "Pronombres") is false)
            entity.Pronouns = update.Pronouns;

        if (Helper.IsUpdating(entity.ProfileMessage, update.ProfileMessage, StringComparison.Ordinal)
                && Helper.IsTooLong(ref errors, update.ProfileMessage, SocialAppUser.ProfileMessageMaxLength, "Mensaje de Perfil") is false)
            entity.ProfileMessage = update.ProfileMessage;

        if (Helper.IsUpdating(entity.ProfilePictureUrl, update.ProfilePictureUrl, StringComparison.Ordinal)
                && Helper.IsTooLong(ref errors, update.ProfilePictureUrl, SocialAppUser.ProfilePictureUrlMaxLength, "URL de Foto de Perfil") is false)
            entity.ProfilePictureUrl = update.ProfilePictureUrl;

        return errors;
    }

    public override async ValueTask<SuccessResult<SocialAppUser>> Create(SocialAppUser? requester, UserCreationModel model)
    {
        var errors = new ErrorList();
        
        if (Helper.IsTooLong(ref errors, model.Username, SocialAppUser.UserNameMaxLength, "Nombre de Usuario") is false
            && await userManager.FindByNameAsync(model.Username) is not null)
        {
            errors.AddError(ErrorMessages.AlreadyInUseByOtherUser("Nombre de Usuario", model.Username));
            return new SuccessResult<SocialAppUser>(errors);
        }

        if (Helper.IsTooLong(ref errors, model.Email, SocialAppUser.EmailMaxLength, "Correo Electrónico") is false
            && await userManager.FindByEmailAsync(model.Email) is not null)
        {
            errors.AddError(ErrorMessages.AlreadyInUseByOtherUser("Correo Electrónico", model.Email));
            return new SuccessResult<SocialAppUser>(errors);
        }

        var newuser = new SocialAppUser()
        {
            Id = new(Guid.NewGuid()),
            RealName = model.RealName
        };

        await userManager.CreateAsync(newuser);

        if ((await userManager.SetUserNameAsync(newuser, model.Username)).Succeeded is false)
        {
            await userManager.DeleteAsync(newuser);
            errors.AddError(ErrorMessages.BadUsername(model.Username));
            return new SuccessResult<SocialAppUser>(errors);
        } 
        else if ((await userManager.AddPasswordAsync(newuser, model.Password)).Succeeded is false)
        {
            await userManager.DeleteAsync(newuser);
            errors.AddError(ErrorMessages.BadPassword());
            return new SuccessResult<SocialAppUser>(errors);
        }
        else
        {
            var token = await userManager.GenerateChangeEmailTokenAsync(newuser, model.Email);
            if ((await userManager.ChangeEmailAsync(newuser, model.Email, token)).Succeeded is false)
            {
                await userManager.DeleteAsync(newuser);
                errors.AddError(ErrorMessages.BadPassword());
                return new SuccessResult<SocialAppUser>(errors);
            }
        }

        await context.SaveChangesAsync();
        return new SuccessResult<SocialAppUser>(newuser);
    }

    public override async ValueTask<SuccessResult<object>> GetView(SocialAppUser? requester, SocialAppUser entity)
    {
        if (requester is null || requester.Id != entity.Id)
        {
            if (await CanView(requester, entity))
                return new SuccessResult<object>(UserViewModel.FromUser(entity));
            else
            {
                var errors = new ErrorList();
                errors.AddError(ErrorMessages.NoPermission());
                errors.RecommendedCode = System.Net.HttpStatusCode.Unauthorized;
                return new SuccessResult<object>(errors);
            }
        }

        return new SuccessResult<object>(UserSelfViewModel.FromUser(entity));
    }

    public async ValueTask<SocialAppUser?> FindByUsername(string username)
        => await userManager.FindByNameAsync(username);

    public async ValueTask<bool> IsFollowing(SocialAppUser requester, SocialAppUser followed)
        => requester.Follows?.Contains(followed) is true || await context.Users.Where(x => x.Id == requester.Id).AnyAsync(x => x.Follows!.Contains(followed));

    public ValueTask<IQueryable<SocialAppUser>> GetFollowers(SocialAppUser requester)
        => ValueTask.FromResult(context.Users.Where(x => x.Follows!.Contains(requester)));

    public ValueTask<IQueryable<SocialAppUser>> GetFollowing(SocialAppUser requester)
        => ValueTask.FromResult(context.Users.Where(x => requester.Follows!.Contains(x)));

    public ValueTask<IQueryable<SocialAppUser>> GetMutuals(SocialAppUser requester)
        => ValueTask.FromResult(context.Users.Where(x => x.Follows!.Contains(requester) && requester.Follows!.Contains(x)));

    public async ValueTask<bool> IsFollowing(SocialAppUser requester, SocialAppUser follower, SocialAppUser followed)
        => await CanView(requester, follower) && await IsFollowing(follower, followed);

    public async ValueTask<IQueryable<SocialAppUser>?> GetFollowers(SocialAppUser requester, SocialAppUser user)
        => await CanView(requester, user) ? await GetFollowers(user) : null;

    public async ValueTask<IQueryable<SocialAppUser>?> GetFollowing(SocialAppUser requester, SocialAppUser user)
        => await CanView(requester, user) ? await GetFollowing(user) : null;

    public async ValueTask<IQueryable<SocialAppUser>?> GetMutuals(SocialAppUser requester, SocialAppUser user)
        => await CanView(requester, user) ? await GetMutuals(user) : null;

    public async ValueTask<bool> FollowUser(SocialAppUser requester, SocialAppUser followed)
    {
        if (await IsFollowing(requester, followed))
            return false;

        (requester.Follows ??= new()).Add(followed);
        return true;
    }

    private async ValueTask<bool> CanView(SocialAppUser? requester, SocialAppUser entity)
        => (requester?.Id.Equals(entity.Id) is true)
        || ((entity.Settings.HasFlag(UserSettings.AllowAnonymousViews) || requester is not null)
            && (entity.Settings.HasFlag(UserSettings.AllowNonFollowerViews) || (requester is not null && await IsFollowing(requester, entity))));
}
