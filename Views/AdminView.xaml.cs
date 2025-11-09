using bobesponja2._0.ViewModels;

namespace bobesponja2._0.Views;

public partial class AdminView : ContentPage
{
	public AdminView()
	{
		InitializeComponent();
        BindingContext = new UsuarioViewModel();
    }
}