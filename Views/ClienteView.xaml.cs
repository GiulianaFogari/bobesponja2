using bobesponja2._0.ViewModels;

namespace bobesponja2._0.Views;

public partial class ClienteView : ContentPage
{
	public ClienteView()
	{
		InitializeComponent();
        BindingContext = new UsuarioViewModel();
    }
}