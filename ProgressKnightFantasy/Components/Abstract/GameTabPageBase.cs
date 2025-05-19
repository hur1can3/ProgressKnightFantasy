using Microsoft.AspNetCore.Components;
using ProgressKnightFantasy.Services;

namespace ProgressKnightFantasy.Components.Abstract;

public abstract class GameTabPageBase : ComponentBase, IDisposable
{
    [Inject]
    protected GameStateService GameState { get; set; } = default!;

    protected override void OnInitialized()
    {
        base.OnInitialized();
        if (GameState != null)
        {
            GameState.OnChange += HandleGameStateChanged;
        }
    }

    protected async void HandleGameStateChanged()
    {
        // Ensure UI updates are marshalled to the correct synchronization context.
        // This is crucial when the OnChange event is invoked from a background thread (like a Timer).
        await InvokeAsync(StateHasChanged);
    }

    public virtual void Dispose()
    {
        if (GameState != null)
        {
            GameState.OnChange -= HandleGameStateChanged;
        }
        GC.SuppressFinalize(this);
    }
}
