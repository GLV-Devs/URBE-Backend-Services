using System.Net;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Urbe.Programacion.AppVehiculos.Entities.Data.Entities;
using Urbe.Programacion.AppVehiculos.WebApp.Data.Identity;
using Urbe.Programacion.AppVehiculos.WebApp.Data.Models.VehicleUser;
using Urbe.Programacion.Shared.Common;
using Urbe.Programacion.Shared.API.Common;
using System.Diagnostics;
using Urbe.Programacion.AppVehiculos.WebApp.Data;
using System.Text;

namespace Urbe.Programacion.AppVehiculos.WebApp.Pages.Identity;

public class RegisterModel : LogInModel
{
    protected readonly IUserRepository UserRepository;
    public RegisterModel(IUserRepository userRepository, SignInManager<VehicleUser> signInManager, UserManager<VehicleUser> userManager) : base(signInManager, userManager)
    {
        UserRepository = userRepository;
    }

    public string? Username { get; set; }
    public string? Email { get; set; }
    public string? ConfirmEmail { get; set; }
    public string? RealName { get; set; }

    public override async Task<IActionResult> OnPost()
    {
        HttpStatusCode? code = null;
        var loggedUser = await UserManager.GetUserAsync(User);
        if (loggedUser is not null)
            return ReturnToDestination();

        if (Request.Form.BindThroughConstructor<VehicleUserCreationModel>(out var creation, out _))
        {
            Username = creation.Username;
            RealName = creation.RealName;
            Email = creation.Email;
            ConfirmEmail = creation.ConfirmEmail;

            if (string.IsNullOrWhiteSpace(creation.Username))
                errorList.AddError(ErrorMessages.EmptyProperty("Nombre de Usuario"));

            if (string.IsNullOrWhiteSpace(creation.RealName))
                errorList.AddError(ErrorMessages.EmptyProperty("Nombre Real"));

            if (string.IsNullOrWhiteSpace(creation.Email))
            {
                errorList.AddError(ErrorMessages.EmptyProperty("Correo Electrónico"));

                if(string.IsNullOrWhiteSpace(creation.ConfirmEmail))
                    errorList.AddError(ErrorMessages.EmptyProperty("Confirmación de Correo Electrónico"));

                else if (creation.ConfirmEmail.Equals(creation.Email, StringComparison.OrdinalIgnoreCase) is false)
                    errorList.AddError(ErrorMessages.ConfirmationNotSame("Correo Electrónico"));
            }
            
            if (string.IsNullOrWhiteSpace(creation.ConfirmEmail))
                errorList.AddError(ErrorMessages.EmptyProperty("Confirmación de Correo Electrónico"));
            
            if (string.IsNullOrWhiteSpace(creation.Password))
            {
                errorList.AddError(ErrorMessages.EmptyProperty("Contraseña"));

                if (string.IsNullOrWhiteSpace(creation.ConfirmPassword))
                    errorList.AddError(ErrorMessages.EmptyProperty("Confirmación de Contraseña"));

                else if (creation.ConfirmPassword.Equals(creation.Password, StringComparison.Ordinal) is false)
                    errorList.AddError(ErrorMessages.ConfirmationNotSame("Contraseña"));
            }
            
            if (string.IsNullOrWhiteSpace(creation.ConfirmPassword))
                errorList.AddError(ErrorMessages.EmptyProperty("Confirmación Contraseña"));

            if (errorList.Count > 0)
                code = HttpStatusCode.BadRequest;
            else
            {
                Debug.Assert(creation.Username is not null);
                Debug.Assert(creation.RealName is not null);
                Debug.Assert(creation.Email is not null);
                Debug.Assert(creation.ConfirmEmail is not null);
                Debug.Assert(creation.Password is not null);
                Debug.Assert(creation.ConfirmPassword is not null);

                if (errorList.Count == 0)
                {
                    var result = await UserRepository.Create(null, creation);
                    if (result.IsSuccess && result.TryGetResult(out var user))
                    {
                        await UserRepository.SaveChanges();
                        await SignInManager.PasswordSignInAsync(user, creation.Password, true, false);
                        return ReturnToDestination();
                    }
                    else
                        errorList.AddErrorRange(result.ErrorMessages.Errors);
                }
            }
        }
        else
        {
            errorList.AddError(ErrorMessages.BadLogin());
            code = HttpStatusCode.BadRequest;
        }

        var p = Page();

        if (code is HttpStatusCode c1)
            p.StatusCode = (int)c1;
        else if (errorList.RecommendedCode is HttpStatusCode c2)
            p.StatusCode = (int)c2;

        return p;
    }
}
