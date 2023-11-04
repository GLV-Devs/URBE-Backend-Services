using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Urbe.Programacion.Shared.Entities.Interfaces;
using Urbe.Programacion.Shared.Entities.Models;
using Urbe.Programacion.Shared.ModelServices;

namespace Urbe.Programacion.Shared.API.Backend.Controllers;

public abstract class CRUDController<TAppUser, TEntity, TKey, TCreationModel, TUpdateModel> : CRDController<TAppUser, TEntity, TKey, TCreationModel>
    where TAppUser : BaseAppUser
    where TEntity : IEntity, IKeyed<TKey>
    where TKey : struct, IEquatable<TKey>
{
    protected IEntityCRUDRepository<TEntity, TKey, TCreationModel, TUpdateModel> EntityCRUDRepository { get; }

    protected CRUDController(IEntityCRUDRepository<TEntity, TKey, TCreationModel, TUpdateModel> entityRepository, UserManager<TAppUser> userManager)
        : base(entityRepository, userManager)
    {
        EntityCRUDRepository = entityRepository;
    }

    [HttpPut("{key}")]
    public async Task<IActionResult> Update([FromBody] TUpdateModel update, TKey key)
    {
        var u = await UserManager.GetUserAsync(User);
        var foundEntity = await EntityCRUDRepository.Find(key);
        if (foundEntity is null)
            return NotFound();

        var result = await EntityCRUDRepository.Update(u, foundEntity, update);

        if (result.IsSuccess)
        {
            await EntityCRUDRepository.SaveChanges();
            var viewresult = await EntityCRUDRepository.GetView(u, foundEntity);

            return viewresult.TryGetResult(out var view) ? Ok(view) : FailureResult(viewresult);
        }

        return FailureResult(result);
    }
}
