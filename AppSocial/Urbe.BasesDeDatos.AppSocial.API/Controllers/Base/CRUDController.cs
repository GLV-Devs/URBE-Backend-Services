using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Urbe.BasesDeDatos.AppSocial.Entities.Interfaces;
using Urbe.BasesDeDatos.AppSocial.Entities.Models;
using Urbe.BasesDeDatos.AppSocial.ModelServices;

namespace Urbe.BasesDeDatos.AppSocial.API.Controllers.Base;

public abstract class CRUDController<TEntity, TKey, TCreationModel, TUpdateModel> : CRDController<TEntity, TKey, TCreationModel>
    where TEntity : IEntity, IKeyed<TKey>
    where TKey : struct, IEquatable<TKey>
{
    protected IEntityCRUDRepository<TEntity, TKey, TCreationModel, TUpdateModel> EntityCRUDRepository { get; }

    protected CRUDController(IEntityCRUDRepository<TEntity, TKey, TCreationModel, TUpdateModel> entityRepository, UserManager<SocialAppUser> userManager) 
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
