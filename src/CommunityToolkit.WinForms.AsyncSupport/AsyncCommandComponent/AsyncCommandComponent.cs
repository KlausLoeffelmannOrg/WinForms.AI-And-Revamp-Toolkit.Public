using System.ComponentModel;
using System.Windows.Input;

namespace CommunityToolkit.WinForms.AsyncSupport;

/// <summary>
/// Represents the method that will handle the asynchronous events which can be awaited.
/// </summary>
/// <param name="sender">The source of the event.</param>
/// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
/// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
public delegate Task AsyncEventHandler(object? sender, EventArgs e);

public partial class AsyncCommandComponent : Component, ICommand
{
    /// <summary>
    /// Occurs when changes occur that affect whether or not the command should execute.
    /// </summary>
    public event EventHandler? CanExecuteChanged;

    /// <summary>
    /// Defines the method that determines whether the command can execute in its current state.
    /// </summary>
    /// <param name="parameter">Data used by the command. If the command does not require data to be passed, this object can be set to null.</param>
    /// <returns>true if this command can be executed; otherwise, false.</returns>
    public bool CanExecute(object? parameter)
        => true;

    /// <summary>
    /// Defines the method to be called when the command is invoked.
    /// </summary>
    /// <param name="parameter">Data used by the command. If the command does not require data to be passed, this object can be set to null.</param>
    public void Execute(object? parameter)
    {
    }

    /// <summary>
    /// Gets or sets the command that will be executed.
    /// </summary>
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public ICommand? Command { get; set; }

    /// <summary>
    /// Gets or sets the component that will be the source of the event.
    /// </summary>
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public Component? EventSender { get; set; }

    /// <summary>
    /// Gets or sets the name of the event that will be handled.
    /// </summary>
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    [TypeConverter(typeof(ComponentEventTypeConverter))]
    public string? Event { get; set; }
}
