namespace DocumentFlowing.Interfaces.Services;

public interface IDialogService
{
    event Action<bool> DialogClosed;
}