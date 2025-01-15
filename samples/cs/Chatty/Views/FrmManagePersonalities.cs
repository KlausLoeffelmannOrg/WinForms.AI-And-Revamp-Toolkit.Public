using Chatty.ViewModels;

namespace Chatty.Views;

internal partial class FrmManagePersonalities: Form
{
    private PersonalityViewModel _personalityViewModel;

    public FrmManagePersonalities()
    {
        InitializeComponent();
        _personalityViewModel = PersonalityViewModel.GetTemplates();
    }

    public FrmManagePersonalities(PersonalityViewModel personalities)
    {
        InitializeComponent();
        _personalityViewModel = personalities;
    }

    public PersonalityViewModel Personalities => _personalityViewModel;
}
