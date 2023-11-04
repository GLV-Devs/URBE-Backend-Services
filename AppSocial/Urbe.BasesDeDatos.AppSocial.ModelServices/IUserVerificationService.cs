using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Urbe.BasesDeDatos.AppSocial.Entities;
using Urbe.BasesDeDatos.AppSocial.Entities.Models;

namespace Urbe.Programacion.AppSocial.ModelServices;

public interface IUserVerificationService
{
    public ValueTask<VerificationResult> VerifyUserEmail(PendingMailConfirmation confirmation);

    public ValueTask<bool> CreateMailConfirmation(SocialAppUser user);

    public ValueTask<PendingMailConfirmation?> FindById(RandomKey key);

    public ValueTask<PendingMailConfirmation?> FindByUser(SocialAppUser user);

    public ValueTask<PendingMailConfirmation?> FindByToken(string token);
}
