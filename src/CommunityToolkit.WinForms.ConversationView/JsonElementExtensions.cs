using System.Text.Json;

namespace CommunityToolkit.WinForms.ConversationView.Extensions;

/// <summary>
///  Provides extension methods for <see cref="JsonElement"/>.
/// </summary>
public static class JsonElementExtensions
{
    /// <summary>
    ///  Gets the value of a specified property as a string, 
    ///  or a default value if the property does not exist.
    /// </summary>
    /// <param name="element">The <see cref="JsonElement"/> to search.</param>
    /// <param name="propertyName">The name of the property to get.</param>
    /// <param name="defaultValue">The default value to return if the property does not exist.</param>
    /// <returns>The value of the property as a string, or the default value if the property does not exist.</returns>
    public static string GetPropertyOrDefault(this JsonElement element, string propertyName, string defaultValue = "")
        => element.TryGetProperty(propertyName, out JsonElement property)
            ? property.GetString() ?? defaultValue
            : defaultValue;

    /// <summary>
    ///  Gets the value of a specified property as an integer, 
    ///  or a default value if the property does not exist.
    /// </summary>
    /// <param name="element">The <see cref="JsonElement"/> to search.</param>
    /// <param name="propertyName">The name of the property to get.</param>
    /// <param name="defaultValue">The default value to return if the property does not exist.</param>
    /// <returns>The value of the property as an integer, or the default value if the property does not exist.</returns>
    public static int GetPropertyOrDefault(this JsonElement element, string propertyName, int defaultValue = 0)
        => element.TryGetProperty(propertyName, out JsonElement property)
            ? property.GetInt32()
            : defaultValue;

    /// <summary>
    ///  Gets the value of a specified property as a <see cref="DateTimeOffset"/>, 
    ///  or a default value if the property does not exist.
    /// </summary>
    /// <param name="element">The <see cref="JsonElement"/> to search.</param>
    /// <param name="propertyName">The name of the property to get.</param>
    /// <param name="defaultValue">The default value to return if the property does not exist.</param>
    /// <returns>The value of the property as a <see cref="DateTimeOffset"/>, or the default value if the property does not exist.</returns>
    public static DateTimeOffset GetPropertyOrDefault(this JsonElement element, string propertyName, DateTimeOffset defaultValue)
        => element.TryGetProperty(propertyName, out JsonElement property)
            ? property.GetDateTimeOffset()
            : defaultValue;

    /// <summary>
    ///  Gets the value of a specified property as a <see cref="Guid"/>, 
    ///  or a default value if the property does not exist.
    /// </summary>
    /// <param name="element">The <see cref="JsonElement"/> to search.</param>
    /// <param name="propertyName">The name of the property to get.</param>
    /// <param name="defaultValue">The default value to return if the property does not exist.</param>
    /// <returns>The value of the property as a <see cref="Guid"/>, or the default value if the property does not exist.</returns>
    public static Guid GetPropertyOrDefault(this JsonElement element, string propertyName, Guid defaultValue)
        => element.TryGetProperty(propertyName, out JsonElement property)
            ? property.GetGuid()
            : defaultValue;
}
