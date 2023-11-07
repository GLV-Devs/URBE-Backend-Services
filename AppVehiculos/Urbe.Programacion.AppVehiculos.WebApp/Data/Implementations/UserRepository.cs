using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Urbe.Programacion.AppVehiculos.Entities.Data;
using Urbe.Programacion.AppVehiculos.Entities.Data.Entities;
using Urbe.Programacion.AppVehiculos.WebApp.Data.Models.VehicleUser;
using Urbe.Programacion.Shared.Common;
using Urbe.Programacion.Shared.Entities.Models;
using Urbe.Programacion.Shared.ModelServices;
using Urbe.Programacion.Shared.ModelServices.Implementations;
using Urbe.Programacion.Shared.Services.Attributes;

namespace Urbe.Programacion.AppVehiculos.WebApp.Data.Implementations;

[RegisterService(typeof(IUserRepository))]
[RegisterService(typeof(IEntityCRUDRepository<VehicleUser, Guid, VehicleUserCreationModel, VehicleUserUpdateModel>))]
public class UserRepository : EntityCRUDRepository<VehicleUser, Guid, VehicleUserCreationModel, VehicleUserUpdateModel>, IUserRepository
{
    protected readonly UserManager<VehicleUser> userManager;
    protected readonly new VehicleContext context;

    public UserRepository(VehicleContext context, UserManager<VehicleUser> userManager, IServiceProvider provider) : base(context, provider)
    {
        this.context = context;
        this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
    }

    public override async ValueTask<SuccessResult> Update(BaseAppUser? r, VehicleUser entity, VehicleUserUpdateModel update)
    {
        var requester = (VehicleUser?)r;

        var errors = new ErrorList();

        if (requester is null || requester.Id != entity.Id)
        {
            errors.AddError(ErrorMessages.NoPermission());
            errors.RecommendedCode = System.Net.HttpStatusCode.Unauthorized;
            return new(errors);
        }

        if (Helper.IsUpdatingString(entity.Email, update.Email)
                && Helper.IsTooLong(ref errors, update.Email, BaseAppUser.EmailMaxLength, "Correo electronico") is false)
        {
            var match = Regexes.Email().Match(update.Email);
            if (match.Success is false || match.Length != update.Email.Length)
                errors.AddError(ErrorMessages.BadEmail(update.Email));
            else
            {
                var tk = await userManager.GenerateChangeEmailTokenAsync(requester, update.Email);
                await userManager.ChangeEmailAsync(requester, update.Email, tk);
            }
        }

        if (Helper.IsUpdatingString(entity.UserName, update.Username)
                && Helper.IsTooLong(ref errors, update.Username, BaseAppUser.UserNameMaxLength, "Nombre de Usuario") is false)
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

        if (Helper.IsUpdatingString(entity.RealName, update.RealName, StringComparison.Ordinal)
                && Helper.IsTooLong(ref errors, update.RealName, BaseAppUser.RealNameMaxLength, "Nombre Real") is false)
            entity.RealName = update.RealName;

        return new(errors);
    }

    public override async ValueTask<SuccessResult<VehicleUser>> Create(BaseAppUser? requester, VehicleUserCreationModel model)
    {
        var errors = new ErrorList();

        if (Helper.IsEmpty(ref errors, model.Username, "Nombre de Usuario")  is false
            && Helper.IsTooLong(ref errors, model.Username, BaseAppUser.UserNameMaxLength, "Nombre de Usuario") is false
            && await userManager.FindByNameAsync(model.Username) is not null)
        {
            errors.AddError(ErrorMessages.UsernameAlreadyInUse(model.Username));
            errors.RecommendedCode = System.Net.HttpStatusCode.Conflict;
        }

        if (Helper.IsEmpty(ref errors, model.Email, "Correo Electrónico") is false
            && Helper.IsTooLong(ref errors, model.Email, BaseAppUser.EmailMaxLength, "Correo Electrónico") is false
            && await userManager.FindByEmailAsync(model.Email) is not null)
        {
            errors.AddError(ErrorMessages.EmailAlreadyInUse(model.Email));
            errors.RecommendedCode = System.Net.HttpStatusCode.Conflict;
        }

        Helper.IsEmpty(ref errors, model.Password, "Contraseña");

        if (errors.Count > 0)
            return new SuccessResult<VehicleUser>(errors);

        Debug.Assert(model.Password is not null);

        var newuser = new VehicleUser()
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
                errors.AddError(ErrorMessages.TryBindError(error.Code, error.Description));

            return new SuccessResult<VehicleUser>(errors);
        }

        var passresults = await userManager.AddPasswordAsync(newuser, model.Password);
        if (passresults.Succeeded is false)
        {
            await userManager.DeleteAsync(newuser);
            errors.AddError(ErrorMessages.BadPassword());
            foreach (var error in passresults.Errors)
                errors.AddError(ErrorMessages.TryBindError(error.Code, error.Description));

            return new SuccessResult<VehicleUser>(errors);
        }

        await context.SaveChangesAsync();
        return new SuccessResult<VehicleUser>(newuser);
    }

    public override ValueTask<SuccessResult<object>> GetView(BaseAppUser? requester, VehicleUser entity) 
        => requester?.Id is not Guid id
            ? default
            : ValueTask.FromResult<SuccessResult<object>>(new(
                id != entity.Id
                ? new VehicleUserView(entity.UserName, null, null)
                : new VehicleUserView(entity.UserName, entity.Email, entity.RealName)
            ));

    public override ValueTask<IQueryable<object>?> GetViews(BaseAppUser? requester, IQueryable<VehicleUser>? users) 
        => ValueTask.FromResult<IQueryable<object>?>(users is null || requester?.Id is not Guid id
            ? null
            : users.Select(x => new VehicleUserView(
                x.UserName,
                id == x.Id ? x.Email : null,
                id == x.Id ? x.RealName : null
            )
        ));
}
