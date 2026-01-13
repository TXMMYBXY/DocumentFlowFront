using DocumentFlowing.Client.Admin.Dtos;
using DocumentFlowing.ViewModels.Base;

namespace DocumentFlowing.ViewModels.Controls;

public class UserItemViewModel : BaseViewModel
{
    private readonly GetUserDto _userDto;
    
    public UserItemViewModel(GetUserDto userDto)
    {
        _userDto = userDto;
    }
    
    public int Id => _userDto.Id;
    public string Email => _userDto.Email;
    public string FullName => _userDto.FullName;
    public string Role => _userDto.Role;
    public string Department => _userDto.Department;
    
    public bool IsActive
    {
        get => _userDto.IsActive;
        set
        {
            _userDto.IsActive = value;
            OnPropertyChanged(nameof(IsActive));
        }
    }
}