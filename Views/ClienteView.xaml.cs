using bobesponja2._0.ViewModels;

namespace bobesponja2._0.Views;

public partial class ClienteView : ContentPage
{
    public ClienteView()
    {
        InitializeComponent();
        BindingContext = new ItemViewModel();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        
        // Executar comando para carregar itens
        if (BindingContext is ItemViewModel viewModel)
        {
            viewModel.CarregarItensCommand?.Execute(null);
        }
    }
}