using bobesponja2._0.ViewModels;

namespace bobesponja2._0.Views;

public partial class UsuarioLoginView : ContentPage
{
	public UsuarioLoginView()
	{
		InitializeComponent();
        BindingContext = new UsuarioViewModel();
    }
}