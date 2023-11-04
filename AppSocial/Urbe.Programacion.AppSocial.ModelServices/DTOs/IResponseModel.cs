using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Urbe.Programacion.AppSocial.ModelServices.API.Responses;

namespace Urbe.Programacion.AppSocial.ModelServices.DTOs;

public interface IResponseModel
{
    public APIResponseCode APIResponseCode { get; }
}
