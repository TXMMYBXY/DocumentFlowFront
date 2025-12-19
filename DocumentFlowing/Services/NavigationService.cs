using DocumentFlowing.Interfaces.Client;
using DocumentFlowing.Interfaces.Client.Services;
using DocumentFlowing.Interfaces.Services;
using DocumentFlowing.Views.Admin;
using DocumentFlowing.Views.Boss;
using DocumentFlowing.Views.Purchaser;
using DocumentFlowing.Views.User;
using Microsoft.Extensions.Configuration;
using System.Windows;

namespace DocumentFlowing.Services;

public class NavigationService : INavigationService
{
    public void NavigateToRole(int roleId)
    {
        switch (roleId)
        {
            case 1:
                new AdminMainView().Show();
                break;
            case 2:
                new BossMainView().Show();
                break;
            case 3:
                new PurchaserMainView().Show();
                break;
            case 4:
                new UserMainView().Show();
                break;
        }
    }
}