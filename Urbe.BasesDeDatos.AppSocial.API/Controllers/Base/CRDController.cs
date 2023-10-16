﻿using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Urbe.BasesDeDatos.AppSocial.DatabaseServices;
using Urbe.BasesDeDatos.AppSocial.Entities.Interfaces;
using Urbe.BasesDeDatos.AppSocial.Entities.Models;

namespace Urbe.BasesDeDatos.AppSocial.API.Controllers.Base;

public abstract class CRDController<TEntity, TKey, TCreationModel> : SocialAppController
    where TEntity : IEntity, IKeyed<TKey>
    where TKey : struct, IEquatable<TKey>
{
    protected IEntityCRDRepository<TEntity, TKey, TCreationModel> EntityCRDRepository { get; }
    protected UserManager<SocialAppUser> UserManager { get; }

    protected CRDController(IEntityCRDRepository<TEntity, TKey, TCreationModel> entityRepository, UserManager<SocialAppUser> userManager)
    {
        EntityCRDRepository = entityRepository ?? throw new ArgumentNullException(nameof(entityRepository));
        UserManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
    }

    [HttpPost]
    public virtual async Task<IActionResult> CreateEntity([FromBody] TCreationModel creationModel)
    {
        var u = await UserManager.GetUserAsync(User);
        var result = await EntityCRDRepository.Create(u, creationModel);

        if (result.TryGetResult(out var created))
        {
            await EntityCRDRepository.SaveChanges();
            var viewresult = await EntityCRDRepository.GetView(u, created);
            return viewresult.TryGetResult(out var view) ? Ok(view) : FailureResult(viewresult);
        }

        return FailureResult(result);
    }

    [HttpGet("{key}")]
    public virtual async Task<IActionResult> ViewEntity(TKey key)
    {
        var u = await UserManager.GetUserAsync(User);
        var foundEntity = await EntityCRDRepository.Find(key);
        if (foundEntity is null)
            return NotFound();

        var viewresult = await EntityCRDRepository.GetView(u, foundEntity);
        return viewresult.TryGetResult(out var view) ? Ok(view) : FailureResult(viewresult);
    }

    [HttpDelete("{key}")]
    public virtual async Task<IActionResult> DeleteEntity(TKey key)
    {
        var u = await UserManager.GetUserAsync(User);
        var foundEntity = await EntityCRDRepository.Find(key);
        if (foundEntity is null)
            return NotFound();

        var result = await EntityCRDRepository.Delete(u, foundEntity);
        if (result.IsSuccess)
        {
            await EntityCRDRepository.SaveChanges();
            return Ok();
        }
        else
            return FailureResult(result);
    }
}
