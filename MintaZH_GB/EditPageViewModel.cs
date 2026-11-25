using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MintaZH_GB
{
    [QueryProperty(nameof(EditedStudent),"student")]
    public partial class EditPageViewModel:ObservableObject
    {
        [ObservableProperty]
        Student editedStudent;

        [RelayCommand]
        async Task SaveAsync()
        {
            await Shell.Current.GoToAsync("..", new ShellNavigationQueryParameters() { { "editedStudent", EditedStudent } });
        }
    }
}
