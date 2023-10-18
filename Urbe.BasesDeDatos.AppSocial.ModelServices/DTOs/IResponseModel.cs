using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Urbe.BasesDeDatos.AppSocial.ModelServices.API.Responses;

namespace Urbe.BasesDeDatos.AppSocial.ModelServices.DTOs;

public interface IResponseModel
{
    public APIResponseCode APIResponseCode { get; }
}
