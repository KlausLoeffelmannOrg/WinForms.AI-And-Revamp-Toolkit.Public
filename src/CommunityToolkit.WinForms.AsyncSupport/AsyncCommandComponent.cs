using System.ComponentModel;
using System.Windows.Input;

namespace CommunityToolkit.WinForms.AsyncSupport;

public partial class AsyncCommandComponent : Component, ICommand
{
    public event EventHandler? CanExecuteChanged;

    public bool CanExecute(object? parameter) 
        => true;

    public void Execute(object? parameter)
    {
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public ICommand? Command { get; set; }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public Component? EventSender { get; set; }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    [TypeConverter(typeof(ComponentEventTypeConverter))]
    public string? Event { get; set; }
}
