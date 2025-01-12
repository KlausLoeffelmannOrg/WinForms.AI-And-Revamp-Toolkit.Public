using System.ComponentModel;

namespace CommunityToolkit.DesktopGeneric.Mvvm;

public interface IModalDialogResult<TViewModel>
    where TViewModel : class, INotifyPropertyChanged
{
    TViewModel? ReturnValue { get; }

    DialogResult DialogResult { get; }
}
