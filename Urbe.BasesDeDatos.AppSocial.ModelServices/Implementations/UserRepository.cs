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
using Urbe.BasesDeDatos.AppSocial.Entities;
using Urbe.BasesDeDatos.AppSocial.Entities.Interfaces;
using Urbe.BasesDeDatos.AppSocial.Entities.Models;
using Urbe.BasesDeDatos.AppSocial.ModelServices;
using Urbe.BasesDeDatos.AppSocial.ModelServices.DTOs.Requests;
using Urbe.BasesDeDatos.AppSocial.ModelServices.DTOs.Responses;
using Urbe.BasesDeDatos.AppSocial.Services.Attributes;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Urbe.BasesDeDatos.AppSocial.ModelServices.Implementations;

[RegisterService(typeof(IEntityCRDRepository<SocialAppUser, Guid, UserCreationModel>))]
[RegisterService(typeof(IEntityCRUDRepository<SocialAppUser, Guid, UserCreationModel, UserUpdateModel>))]
[RegisterService(typeof(IUserRepository))]
public class UserRepository : EntityCRUDRepository<SocialAppUser, Guid, UserCreationModel, UserUpdateModel>, IUserRepository
{
    protected readonly UserManager<SocialAppUser> userManager;

    public UserRepository(SocialContext context, UserManager<SocialAppUser> userManager, IServiceProvider provider) : base(context, provider)
    {
        this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
    }

    public override async ValueTask<SuccessResult> Update(SocialAppUser? requester, SocialAppUser entity, UserUpdateModel update)
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
                errors.AddError(ErrorMessages.UsernameAlreadyInUse(update.Username));
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

        return new(errors);
    }

    public override async ValueTask<SuccessResult<SocialAppUser>> Create(SocialAppUser? requester, UserCreationModel model)
    {
        var errors = new ErrorList();

        if (Helper.IsTooLong(ref errors, model.Username, SocialAppUser.UserNameMaxLength, "Nombre de Usuario") is false
            && await userManager.FindByNameAsync(model.Username) is not null)
        {
            errors.AddError(ErrorMessages.UsernameAlreadyInUse(model.Username));
            return new SuccessResult<SocialAppUser>(errors);
        }

        if (Helper.IsTooLong(ref errors, model.Email, SocialAppUser.EmailMaxLength, "Correo Electrónico") is false
            && await userManager.FindByEmailAsync(model.Email) is not null)
        {
            errors.AddError(ErrorMessages.EmailAlreadyInUse(model.Email));
            return new SuccessResult<SocialAppUser>(errors);
        }

        var newuser = new SocialAppUser()
        {
            Id = Guid.NewGuid(),
            UserName = model.Username,
            Email = model.Email,
            RealName = model.RealName
        };

        var createresult = await userManager.CreateAsync(newuser);
        if (createresult.Succeeded is false)
        {
            foreach (var error in createresult.Errors)
                errors.AddError(new ErrorMessage(
                    error.Description,
                    error.Code,
                    null
                ));

            return new SuccessResult<SocialAppUser>(errors);
        }

        var passresults = await userManager.AddPasswordAsync(newuser, model.Password);
        if (passresults.Succeeded is false)
        {
            await userManager.DeleteAsync(newuser);
            errors.AddError(ErrorMessages.BadPassword());
            foreach (var error in passresults.Errors)
                errors.AddError(new ErrorMessage(
                    error.Description,
                    error.Code,
                    null
                ));

            return new SuccessResult<SocialAppUser>(errors);
        }
        //else
        //{
        //    var token = await userManager.GenerateChangeEmailTokenAsync(newuser, model.Email);
        //    if ((await userManager.ChangeEmailAsync(newuser, model.Email, token)).Succeeded is false)
        //    {
        //        await userManager.DeleteAsync(newuser);
        //        errors.AddError(ErrorMessages.BadPassword());
        //        return new SuccessResult<SocialAppUser>(errors);
        //    }
        //}

        await context.SaveChangesAsync();
        return new SuccessResult<SocialAppUser>(newuser);
    }

    public override async ValueTask<SuccessResult<object>> GetView(SocialAppUser? requester, SocialAppUser entity)
    {
        if (requester is null || requester.Id != entity.Id)
        {
            UserViewModel view;
            if (await CanView(requester, entity))
            {
                view = UserViewModel.FromUser(entity);
                if (requester is not null)
                    view.FollowsRequester = await IsFollowing(requester, entity);
            }
            else
                view = UserViewModel.FromHiddenUser(entity);
            return new SuccessResult<object>(view);
        }

        return new SuccessResult<object>(UserSelfViewModel.FromUser(entity));
    }

    public async ValueTask<SocialAppUser?> FindByUsername(string username)
        => await userManager.FindByNameAsync(username);

    public async ValueTask<bool> IsFollowing(SocialAppUser requester, SocialAppUser followed)
        => requester.FollowedUsers?.Contains(followed) is true || await context.SocialAppUserFollows.AnyAsync(x => x.FollowerId == requester.Id && x.FollowedId == followed.Id);

    public ValueTask<IQueryable<SocialAppUser>> GetFollowers(SocialAppUser requester)
        => ValueTask.FromResult(context.SocialAppUserFollows.Where(x => x.FollowedId == requester.Id).Select(x => x.Follower!));

    public ValueTask<IQueryable<SocialAppUser>> GetFollowing(SocialAppUser requester)
        => ValueTask.FromResult(context.SocialAppUserFollows.Where(x => x.FollowerId == requester.Id).Select(x => x.Followed!));

    public ValueTask<IQueryable<SocialAppUser>> GetMutuals(SocialAppUser requester)
        => ValueTask.FromResult(context.SocialAppUsers.Where(x => requester.FollowedUsers != null && requester.FollowedUsers.Contains(x) && x.FollowedUsers != null && x.FollowedUsers.Contains(requester)));

    public async ValueTask<bool> IsFollowing(SocialAppUser requester, SocialAppUser follower, SocialAppUser followed)
        => await CanView(requester, follower) && await IsFollowing(follower, followed);

    public async ValueTask<IQueryable<SocialAppUser>?> GetFollowers(SocialAppUser? requester, SocialAppUser user)
        => await CanView(requester, user) ? await GetFollowers(user) : null;

    public async ValueTask<IQueryable<SocialAppUser>?> GetFollowing(SocialAppUser? requester, SocialAppUser user)
        => await CanView(requester, user) ? await GetFollowing(user) : null;

    public async ValueTask<IQueryable<SocialAppUser>?> GetMutuals(SocialAppUser? requester, SocialAppUser user)
        => await CanView(requester, user) ? await GetMutuals(user) : null;

    public async ValueTask<bool> FollowUser(SocialAppUser requester, SocialAppUser followed)
    {
        if (await IsFollowing(requester, followed))
            return false;

        context.SocialAppUserFollows.Add(new SocialAppUserFollow()
        {
            Followed = followed,
            Follower = requester
        });

        return true;
    }

    public async ValueTask<bool> UnfollowUser(SocialAppUser requester, SocialAppUser followed)
        => await context.SocialAppUserFollows.Where(x => x.Follower == requester && x.Followed == followed).ExecuteDeleteAsync() > 0;

    public override ValueTask<IQueryable<object>?> GetViews(SocialAppUser? requester, IQueryable<SocialAppUser>? users)
        => ValueTask.FromResult<IQueryable<object>?>(
            users is null
            ? null
            : requester is null
            ? users.AsNoTracking().Select(x => x.Settings.HasFlag(UserSettings.AllowAnonymousViews)
                ? new UserViewModel()
                {
                    UserId = x.Id,
                    ProfileMessage = x.ProfileMessage,
                    ProfilePictureUrl = x.ProfilePictureUrl,
                    Pronouns = x.Pronouns,
                    RealName = x.RealName,
                    Username = x.UserName!
                }
                : new UserViewModel()
                {
                    UserId = x.Id,
                    Username = x.UserName!,
                    ProfilePictureUrl = null,
                    Pronouns = x.Pronouns,
                })
            : users.AsNoTracking().Select(x => x.Settings.HasFlag(UserSettings.AllowNonFollowerViews) || x.FollowedUsers != null && x.FollowedUsers.Contains(requester)
                ? new UserViewModel()
                {
                    UserId = x.Id,
                    ProfileMessage = x.ProfileMessage,
                    ProfilePictureUrl = x.ProfilePictureUrl,
                    Pronouns = x.Pronouns,
                    RealName = x.RealName,
                    Username = x.UserName!,
                    FollowsRequester = x.FollowedUsers != null && x.FollowedUsers.Contains(requester)
                }
                : new UserViewModel()
                {
                    UserId = x.Id,
                    Username = x.UserName!,
                    Pronouns = x.Pronouns,
                    ProfilePictureUrl = x.ProfilePictureUrl,
                    FollowsRequester = x.FollowedUsers != null && x.FollowedUsers.Contains(requester)
                })
           );

    public override IQueryable<SocialAppUser> Query(SocialAppUser? Requester)
        => Requester is not null
            ? Query().Where(x => x.Id != Requester.Id && (x.Settings.HasFlag(UserSettings.AllowNonFollowerViews) || Requester.FollowedUsers != null && Requester.FollowedUsers.Contains(x)))
            : Query().Where(x => x.Settings.HasFlag(UserSettings.AllowAnonymousViews));

    private async ValueTask<bool> CanView(SocialAppUser? requester, SocialAppUser entity)
        => requester?.Id.Equals(entity.Id) is true
        || (entity.Settings.HasFlag(UserSettings.AllowAnonymousViews) || requester is not null)
            && (entity.Settings.HasFlag(UserSettings.AllowNonFollowerViews) || requester is not null && await IsFollowing(requester, entity));
}
