namespace BarberReservation.Blazor.UI.Message;

public sealed class MessageService
{
    public UiMessage? Current { get; private set; }
    public event Action? OnChange;

    private int _version;

    public void ShowSuccess(string text) => Show(MessageType.Success, text);
    public void ShowError(string text) => Show(MessageType.Error, text);
    public void ShowInfo(string text) => Show(MessageType.Info, text);

    private void Show(MessageType type, string text)
    {
        Current = new UiMessage(type, text);

        _version++;
        var currentVersion = _version;

        OnChange?.Invoke();

        _ = AutoClearAsync(currentVersion);
    }

    private async Task AutoClearAsync(int version)
    {
        try
        {
            await Task.Delay(3000);

            if (version != _version)
                return;

            Clear();
        }
        catch
        {
        }
    }

    public void Clear()
    {
        Current = null;
        OnChange?.Invoke();
    }
}
