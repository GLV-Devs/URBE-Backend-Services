using System.Drawing;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Urbe.Programacion.AppVehiculos.Entities.Data.Entities;
using Urbe.Programacion.AppVehiculos.WebApp.Data.Models.VehicleReport;
using Urbe.Programacion.Shared.Entities.Localization;

namespace Urbe.Programacion.AppVehiculos.WebApp.Data;

public static class SharedPageData
{
    static SharedPageData()
    {
        ColorDictionary = Colors.ToDictionary(x => x.Value);
        ColorListESP = Colors.Select(x => new SelectListItem()
        {
            Text = x.NameESP,
            Value = x.Value.ToString()
        }).ToList();
    }

    public readonly static object FormControlModel
        = new
        {
            @class = "form-control"
        };

    public readonly static IEnumerable<SelectListItem> CountrySelectList
        = CountryInfo.GetCountries().Select(x => new SelectListItem()
        {
            Text = GetCountryName(x),
            Value = x.Alpha3Code
        });

    public static IEnumerable<SelectListItem> GetCountrySelectListFor(string? selected = null)
    {
        lock (CountrySelectList)
        {
            foreach (var c in CountrySelectList)
            {
                c.Selected = selected is not null && selected == c.Value;
                yield return c;
            }
        }
    }

    public static string GetCountryName(VehicleReportView view) 
        => GetCountryName(CountryInfo.GetByAlpha3Code(view.VehicleCountryAlpha3Code));

    public static string GetCountryName(CountryInfo ci) 
        => $"{ci.NameESP} ({ci.Alpha3Code})";

    public readonly static IEnumerable<SelectListItem> ColorListESP;

    public static IEnumerable<SelectListItem> GetColorSelectListFor(uint? selected = null)
    {
        var cstr = selected?.ToString();
        lock (ColorListESP)
        {
            foreach (var c in ColorListESP)
            {
                c.Selected = cstr is not null && cstr == c.Value;
                yield return c;
            }
        }
    }

    public static string GetColorName(uint color)
        => ColorDictionary.TryGetValue(color, out var cd) ? cd.NameESP : color.ToString();

    public static string ToHex(uint color)
    {
        var c = Color.FromArgb((int)color);
        return $"#{c.R:X2}{c.G:X2}{c.B:X2}";
    }

    public readonly static IDictionary<uint, ColorData> ColorDictionary;

    public readonly static IEnumerable<ColorData> Colors = new ColorData[]
    {
        new ColorData("Azúl Alicia", 4293982463),
        new ColorData("Blanco Antiguo", 4294634455),
        new ColorData("Aguamarina", 4286578644),
        new ColorData("Azure", 4293984255),
        new ColorData("Beige", 4294309340),
        new ColorData("Bisque", 4294960324),
        new ColorData("Negro", 4278190080),
        new ColorData("Almendra Blanqueada", 4294962125),
        new ColorData("Azul", 4278190335),
        new ColorData("Violeta Azulado", 4287245282),
        new ColorData("Marron", 4289014314),
        new ColorData("Madera", 4292786311),
        new ColorData("Azul Cadete", 4284456608),
        new ColorData("Monasterio", 4286578432),
        new ColorData("Chocolate", 4291979550),
        new ColorData("Coral", 4294934352),
        new ColorData("Azul Aciano", 4284782061),
        new ColorData("Seda de Maiz", 4294965468),
        new ColorData("Carmesi", 4292613180),
        new ColorData("Cian", 4278255615),
        new ColorData("Azul Oscuro", 4278190219),
        new ColorData("Cian Oscuro", 4278225803),
        new ColorData("Vara de Oro Oscuro", 4290283019),
        new ColorData("Gris Oscuro", 4289309097),
        new ColorData("Verde Oscuro", 4278215680),
        new ColorData("Khaki Oscuro", 4290623339),
        new ColorData("Magenta Oscuro", 4287299723),
        new ColorData("Verde Olivo Oscuro", 4283788079),
        new ColorData("Naranja Oscuro", 4294937600),
        new ColorData("Orquidea Oscuro", 4288230092),
        new ColorData("Rojo Oscuro", 4287299584),
        new ColorData("Salmon Oscuro", 4293498490),
        new ColorData("Verde Mar Oscuro", 4287609999),
        new ColorData("Azul Pizarra Oscuro", 4282924427),
        new ColorData("Gris Pizarra Oscuro", 4281290575),
        new ColorData("Turquesa Oscuro", 4278243025),
        new ColorData("Violeta Oscuro", 4287889619),
        new ColorData("Rosado Profundo", 4294907027),
        new ColorData("Azul Cielo Profundo", 4278239231),
        new ColorData("Gris Tenue", 4285098345),
        new ColorData("Azul Dodger", 4280193279),
        new ColorData("Ladrillo", 4289864226),
        new ColorData("Blanco Floral", 4294966000),
        new ColorData("Verde Bosque", 4280453922),
        new ColorData("Gainsboro", 4292664540),
        new ColorData("Blanco Fantasma", 4294506751),
        new ColorData("Dorado", 4294956800),
        new ColorData("Vara de Oro", 4292519200),
        new ColorData("Gris", 4286611584),
        new ColorData("Verde", 4278222848),
        new ColorData("Verde Amarillento", 4289593135),
        new ColorData("Miel", 4293984240),
        new ColorData("Rosado Caliente", 4294928820),
        new ColorData("Rojo Indio", 4291648604),
        new ColorData("Indigo", 4283105410),
        new ColorData("Marfil", 4294967280),
        new ColorData("Khaki", 4293977740),
        new ColorData("Lavanda", 4293322490),
        new ColorData("Rubor Lavanda", 4294963445),
        new ColorData("Verde Jardin", 4286381056),
        new ColorData("Gasa Limon", 4294965965),
        new ColorData("Azul Claro", 4289583334),
        new ColorData("Coral Claro", 4293951616),
        new ColorData("Cian Claro", 4292935679),
        new ColorData("Vara de Oro Amarillento Claro", 4294638290),
        new ColorData("Verde Claro", 4287688336),
        new ColorData("Gris Claro", 4292072403),
        new ColorData("Rosado Claro", 4294948545),
        new ColorData("Salmon Claro", 4294942842),
        new ColorData("Verde Mar Claro", 4280332970),
        new ColorData("Azul Cielo Claro", 4287090426),
        new ColorData("Gris Pizarra Claro", 4286023833),
        new ColorData("Azul Acero Claro", 4289774814),
        new ColorData("Amarillo Claro", 4294967264),
        new ColorData("Lima", 4278255360),
        new ColorData("Lima Verde", 4281519410),
        new ColorData("Lino", 4294635750),
        new ColorData("Magenta", 4294902015),
        new ColorData("Granate", 4286578688),
        new ColorData("Aguamarina Medio", 4284927402),
        new ColorData("Azul Medio", 4278190285),
        new ColorData("Orquidea Medio", 4290401747),
        new ColorData("Purpura Medio", 4287852763),
        new ColorData("Verde Mar Medio", 4282168177),
        new ColorData("Azul Pizarra  Medio", 4286277870),
        new ColorData("Verde Primavera Medio", 4278254234),
        new ColorData("Turquesa Medio", 4282962380),
        new ColorData("Rojo Violeta Medio", 4291237253),
        new ColorData("Azul Medianoche", 4279834992),
        new ColorData("Crema de Menta", 4294311930),
        new ColorData("Rosa Brumosa", 4294960353),
        new ColorData("Mocasin", 4294960309),
        new ColorData("Blanco Navajo", 4294958765),
        new ColorData("Azul Marino", 4278190208),
        new ColorData("Encaje Viejo", 4294833638),
        new ColorData("Olivo", 4286611456),
        new ColorData("Verde Olivo", 4285238819),
        new ColorData("Naranja", 4294944000),
        new ColorData("Naranja Rojizo", 4294919424),
        new ColorData("Orquidea", 4292505814),
        new ColorData("Vara de Oro Palido", 4293847210),
        new ColorData("Verde Palido", 4288215960),
        new ColorData("Turquesa Palido", 4289720046),
        new ColorData("Violeta Rojizo Palido", 4292571283),
        new ColorData("Rosado", 4294951115),
        new ColorData("Purpura", 4286578816),
        new ColorData("Rojo", 4294901760),
        new ColorData("Azul Real", 4282477025),
        new ColorData("Salmon", 4294606962),
        new ColorData("Verde Mar", 4281240407),
        new ColorData("Plateado", 4290822336),
        new ColorData("Azul Cielo", 4287090411),
        new ColorData("Azul Pizarra", 4285160141),
        new ColorData("Gris Pizarra", 4285563024),
        new ColorData("Nieve", 4294966010),
        new ColorData("Verde Primavera", 4278255487),
        new ColorData("Azul Acero", 4282811060),
        new ColorData("Verde Azulado", 4278222976),
        new ColorData("Cardo", 4292394968),
        new ColorData("Tomate", 4294927175),
        new ColorData("Turquesa", 4282441936),
        new ColorData("Violeta", 4293821166),
        new ColorData("Blanco", 4294967295),
        new ColorData("Amarillo", 4294967040),
        new ColorData("Verde Amarillento", 4288335154),
    };
}

public readonly record struct ColorData(string NameESP, uint Value)
{
    public Color AsColor() => Color.FromArgb((int)Value);
}