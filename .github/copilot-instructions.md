- **C# Version:**  
  Assume the latest release of C# is in use (minimum C# 12) unless a newer version (e.g., C# 13+) is available after your cut‐off date.

- **Nullable Reference Types (NRT):**  
  Assume NRT is enabled by default—no need to add a `#nullable` directive.

- **Null Checks:**  
  Always use `is null` or `is not null` instead of `== null` or `!= null`.

- **Code Block Formatting:**  
  Insert a newline before the opening curly brace of any code block (e.g., after `if`, `for`, `while`, `foreach`, `using`, `try`, etc.).  
  Also, ensure that the final return statement of a method is on its own line.

- **Variable Declarations:**  
  Never use `var` for primitive types. Use `var` only when the type is obvious from context. When in doubt, opt for an explicit type declaration.

- **Modern Language Features:**  
  Use pattern matching and switch expressions wherever possible.

- **Performance Considerations:**  
  For processing bytes, characters, or strings, consider using `Span<T>`.

- **Member Names:**  
  Use `nameof` instead of string literals when referring to member names.

- **Collection Initializers:**  
  Use the new collection initializer syntax when possible. For example:  
  - Prefer  
    ```csharp
    List<int> list = [1, 2, 3];
    ```  
    over  
    ```csharp
    List<int> list = new List<int> { 1, 2, 3 };
    ```  
  - Prefer  
    ```csharp
    Dictionary<string, int> dict = [ ["key1", 1], ["key2", 2] ];
    ```  
    over  
    ```csharp
    new Dictionary<string, int> { ["key1"] = 1, ["key2"] = 2 };
    ```

- **Namespaces and Usings:**  
  Prefer file-scoped namespace declarations and single-line using directives.

- **Expression-Bodied Members:**  
  Use expression-bodied members for methods, properties, and indexers where it makes sense.

- **XML Documentation:**  
  When asked to add XML comments, follow these guidelines:
  - **For an entire class:**  
    - Include XML comments for the class, its constructor, all public properties, all public methods, events, and protected virtual members (but not for fields).
  - **Structure:**  
    - Include `<see cref="..."/>` tags where appropriate.
    - Wrap lines at 100 characters.
    - In `<remarks>` sections, always use `<para>` tags.
    - Ensure XML-compatible character encoding.
    - Use a one-space indentation in the XML documentation. For example:
      ```csharp
      /// <summary>
      ///  Summary text.
      /// </summary>
      /// <remarks>
      ///  <para>
      ///   This is a sample paragraph to guide an LLM in structuring doc tags.
      ///  </para>
      ///  <para>
      ///   It also shows how to handle multiple paragraphs and code listings.
      ///  </para>
      /// </remarks>
      ```
