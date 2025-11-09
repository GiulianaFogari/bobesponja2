using bobesponja2._0.ViewModels;

namespace bobesponja2._0.Views;

public partial class UsuarioCadastroView : ContentPage
{
	public UsuarioCadastroView()
	{
		InitializeComponent();
        BindingContext = new UsuarioViewModel();

    }
}