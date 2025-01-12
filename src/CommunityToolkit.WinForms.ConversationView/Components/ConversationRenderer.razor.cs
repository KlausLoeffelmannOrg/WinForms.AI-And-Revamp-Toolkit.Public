using CommunityToolkit.WinForms.ConversationView.Mvvm;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace CommunityToolkit.WinForms.ConversationView.Components;

/// <summary>
///  MarkDown renderer component - based on Alexander Mutel's MarkDig, Copyright by Alexander Mutel.
/// </summary>
public partial class ConversationRenderer : ComponentBase
{
    private ConversationViewModel _viewModel = null!;
    private ObservableCollection<ConversationItemViewModel>? _conversationItems;

    [Inject]
    private IJSRuntime? JSRuntime { get; set; }

    [Parameter]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public ConversationViewModel ViewModel
    {
        get => _viewModel;
        set
        {
            _viewModel = value;
        }
    }

    [Parameter]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public string BackColor { get; set; } = null!;

    protected async override void OnInitialized()
    {
        base.OnInitialized();

        if (ViewModel is not null)
        {
            _viewModel.PropertyChanged += ViewModel_PropertyChanged;
            SetConversationItems(_viewModel.ConversationItems);
        }

        // Set the background color of the root HTML element:
        if (JSRuntime is not null)
        {
            await JSRuntime.InvokeVoidAsync("setRootBackgroundColor", BackColor);
        }
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (JSRuntime is null)
        {
            return;
        }

        await JSRuntime.InvokeVoidAsync("scrollToBottom");
    }

    private void ViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(ConversationViewModel.ConversationItems))
        {
            SetConversationItems(_viewModel.ConversationItems);
        }

        InvokeAsync(StateHasChanged);
    }

    private void SetConversationItems(ObservableCollection<ConversationItemViewModel>? conversationItems)
    {
        if (_conversationItems is not null)
        {
            _conversationItems.CollectionChanged -= ConversationItems_CollectionChanged;
        }

        _conversationItems = conversationItems;

        if (_conversationItems is not null)
        {
            _conversationItems.CollectionChanged += ConversationItems_CollectionChanged;
        }
    }

    private void ConversationItems_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        InvokeAsync(StateHasChanged);
    }
}
