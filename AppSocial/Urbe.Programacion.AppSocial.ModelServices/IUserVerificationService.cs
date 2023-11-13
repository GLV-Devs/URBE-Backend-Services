using Urbe.Programacion.AppSocial.Entities.Models;
using Urbe.Programacion.Shared.Common;

namespace Urbe.Programacion.AppSocial.ModelServices;

public interface IUserVerificationService
{
    public ValueTask<VerificationResult> VerifyUserEmail(PendingMailConfirmation confirmation);

    public ValueTask<bool> CreateMailConfirmation(SocialAppUser user);

    public ValueTask<PendingMailConfirmation?> FindById(RandomKey key);

    public ValueTask<PendingMailConfirmation?> FindByUser(SocialAppUser user);

    public ValueTask<PendingMailConfirmation?> FindByToken(string token);
}
