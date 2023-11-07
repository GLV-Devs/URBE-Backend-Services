using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Urbe.Programacion.Shared.API.Common;
public static class FormCollectionExtensions
{
    public static bool BindThroughConstructor<T>(
        this IFormCollection model, 
        [NotNullWhen(true)] out T? result,
        [NotNullWhen(false)] out ModelErrorCollection? errors
    )
    {
        errors = null;

        var ctors = typeof(T).GetConstructors();
        if (ctors.Length > 1)
            throw new InvalidOperationException($"It's not possible to bind {typeof(T).FullName} because it has multiple constructors");

        var ctor = ctors[0];
        var parameters = ctor.GetParameters();
        var paramArray = new object[parameters.Length];
        if (parameters.Length > 0)
        {
            for (int i = 0; i < parameters.Length; i++) 
            {
                object? converted;
                var p = parameters[i];

                if (p.ParameterType == typeof(bool))
                    converted = model.ContainsKey(p.Name!);
                else if (model.TryGetValue(p.Name!, out var value) is false)
                {
                    (errors ??= new()).Add($"NoEntry.{p.Name}");
                    continue;
                }
                else if (TryConvertValue(p.ParameterType, value[0]!, p.Name, out converted, out var exception) is false)
                {
                    if (exception is null)
                        (errors ??= new()).Add($"CannotConvert.{p.Name}");
                    else
                        (errors ??= new()).Add(new ModelError(exception, $"CannotConvert.{p.Name}"));
                    continue;
                }

                paramArray[i] = converted;
            }
        }

        if (errors?.Count > 0)
        {
            result = default;
            return false;
        }

        result = (T)ctor.Invoke(paramArray);
        return true;
    }

    private static bool TryConvertValue(
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)]
            Type type,
            string value, 
            string? property, 
            [NotNullWhen(true)] out object? result, 
            out Exception? error)
    {
        error = null;
        result = null;
        if (type == typeof(object))
        {
            result = value;
            return true;
        }

        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
        {
            if (string.IsNullOrEmpty(value))
            {
                return false;
            }
            return TryConvertValue(Nullable.GetUnderlyingType(type)!, value, property, out result, out error);
        }

        TypeConverter converter = TypeDescriptor.GetConverter(type);
        if (converter.CanConvertFrom(typeof(string)))
        {
            try
            {
                result = converter.ConvertFromInvariantString(value)!;
            }
            catch (Exception ex)
            {
                error = new InvalidOperationException($"Failed to bind {property} to {type}", ex);
                return false;
            }
            return true;
        }

        if (type == typeof(byte[]))
        {
            try
            {
                result = Convert.FromBase64String(value)!;
            }
            catch (FormatException ex)
            {
                error = new InvalidOperationException($"Failed to bind {property} to {type}", ex);
                return false;
            }
            return true;
        }

        return false;
    }
}
