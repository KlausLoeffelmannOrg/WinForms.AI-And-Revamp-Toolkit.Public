using CommunityToolkit.DesktopGeneric.Mvvm;
using System.ComponentModel;
using DialogResult = CommunityToolkit.DesktopGeneric.Mvvm.DialogResult;

namespace CommunityToolkit.WinForms.Extensions;

public class ModalDialogResult<T>(T? result, DialogResult dialogResult) : IModalDialogResult<T>
    where T : class, INotifyPropertyChanged
{
    public T? ReturnValue => result;

    DialogResult IModalDialogResult<T>.DialogResult => dialogResult;
}
