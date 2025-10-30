using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace bobesponja2._0.ViewModels
{
    public class BaseViewModel : BaseNotifyViewModel
    {
        public ICommand VoltarCommand { get; set; }

        private async void Voltar(object obj)
        {
            await Application.Current.MainPage.Navigation.PopAsync();
        }

        public async void AbrirView(ContentPage view)
        {
            await Application.Current.MainPage.Navigation.PushAsync(view);
        }
        public BaseViewModel()
        {
            VoltarCommand = new Command(Voltar);
        }
    }
}
