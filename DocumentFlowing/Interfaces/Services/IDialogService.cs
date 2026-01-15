namespace DocumentFlowing.Interfaces.Services;

public interface IDialogService
{
    /// <summary>
    /// Событие для закрытия собственных модальных окон
    /// </summary>
    event Action<bool> DialogClosed;
}