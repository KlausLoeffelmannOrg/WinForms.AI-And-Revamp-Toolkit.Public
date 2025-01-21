using System.ComponentModel;
using System.Reflection;
using System.Text.Json;

namespace CommunityToolkit.WinForms.AI;

public static class PromptFromTypeGenerator
{
    private static readonly JsonSerializerOptions s_options = new()
    {
        WriteIndented = true
    };

    public static string GetJSonSchema<T>()
    {
        var targetType = typeof(T);
        var schema = new Dictionary<string, object>
        {
            ["$schema"] = "http://json-schema.org/draft-07/schema#",
            ["type"] = "object",
            ["properties"] = GetPropertiesSchema(targetType),
            ["required"] = GetRequiredProperties(targetType),
            ["additionalProperties"] = false
        };

        return JsonSerializer.Serialize(schema, s_options);
    }

    private static Dictionary<string, object> GetPropertiesSchema(Type type)
    {
        var properties = new Dictionary<string, object>();

        foreach (var property in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            var propertySchema = new Dictionary<string, object>
            {
                ["type"] = GetJsonType(property.PropertyType),
                ["description"] = GetPropertyDescription(property)
            };

            properties[property.Name] = propertySchema;
        }
        return properties;
    }
    private static List<string> GetRequiredProperties(Type type)
    {
        // We consider those properties required, which are attributes with
        // the StructuredReturnDataPropertyAttribute.
        return [.. type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => p.GetCustomAttribute<StructuredReturnDataPropertyAttribute>() is not null)
            .Select(p => p.Name)];
    }

    private static string GetJsonType(Type type) 
        => type switch
        {
            _ when type == typeof(string) => "string",
            _ when type == typeof(int) || type == typeof(long) => "integer",
            _ when type == typeof(float) || type == typeof(double) || type == typeof(decimal) => "number",
            _ when type == typeof(bool) => "boolean",
            _ when type.IsArray || typeof(IEnumerable<>).IsAssignableFrom(type) => "array",
            _ when type.IsClass => "object",
            _ => throw new NotSupportedException($"Unsupported type: {type}")
        };

    private static string GetPropertyDescription(PropertyInfo property)
    {
        var descriptionAttribute = property.GetCustomAttribute<DescriptionAttribute>();
        return descriptionAttribute?.Description ?? string.Empty;
    }

    private static string GetPropertyPrompt(PropertyInfo property)
    {
        var structuredReturnDataAttribute = property.GetCustomAttribute<StructuredReturnDataAttribute>();
        return structuredReturnDataAttribute?.Prompt ?? string.Empty;
    }

    public static StructuredReturnDataAttribute GetTypePromptInfo<T>()
    {
        var targetType = typeof(T);
        StructuredReturnDataAttribute? typePromptInfo = targetType.GetCustomAttribute<StructuredReturnDataAttribute>();

        return typePromptInfo is null
            ? throw new InvalidOperationException($"The type {targetType.Name} does not have a {nameof(StructuredReturnDataAttribute)} attribute.")
            : typePromptInfo;
    }

    public static IEnumerable<string> GetTypePropertyPrompts<T>()
    {
        var targetType = typeof(T);

        // Let's get a list of all the properties with the StructuredReturnDataPropertyAttribute.
        var properties = targetType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => p.GetCustomAttribute<StructuredReturnDataPropertyAttribute>() is not null);

        if (properties is null)
        {
            throw new InvalidOperationException($"The type {targetType.Name} does not have any properties with the {nameof(StructuredReturnDataPropertyAttribute)} attribute.");
        }

        // Iterate through the properties and build the prompt:
        foreach (var property in properties)
        {
            // Get the Attribute for that property:
            var attribute = property.GetCustomAttribute<StructuredReturnDataPropertyAttribute>()!;

            yield return "* " + attribute.Prompt + $" The property name to store the result for this request is {property.Name}.";
        }
    }
}
