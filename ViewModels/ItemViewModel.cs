using bobesponja2._0.Models;
using bobesponja2._0.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace bobesponja2._0.ViewModels
{
    public class ItemViewModel : BaseViewModel
    {
        DataBaseService dataBaseService = new DataBaseService();

        public ObservableCollection<string> StatusItem { get; } =
                new ObservableCollection<string>(
                    Enum.GetNames(typeof(Item.StatusItem)) 
                );

        // Propriedades da tela
        private int _id;
        public int Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged();
            }
        }

        private string _nome;
        public string Nome
        {
            get => _nome;
            set
            {
                _nome = value;
                OnPropertyChanged();
            }
        }

        private string _descricao;
        public string Descricao
        {
            get => _descricao;
            set
            {
                _descricao = value;
                OnPropertyChanged();
            }
        }

        private string _status;
        public string Status
        {
            get => _status;
            set
            {
                _status = value;
                OnPropertyChanged();
            }
        }

        private string _preco;
        public string Preco
        {
            get => _preco;
            set
            {
                _preco = value;
                OnPropertyChanged();
            }
        }

        // Propriedade para mostrar nome do usuário logado
        public string NomeUsuarioLogado => UsuarioAtual.Instance.Usuario?.Nome ?? "Não logado";

        // Propriedade para verificar se o usuário é admin
        public bool IsAdmin => UsuarioAtual.Instance.IsAdmin;

        // Lista de itens
        public ObservableCollection<Item> Itens { get; set; }

        // Lista de histórico
        public ObservableCollection<Historico> Historico { get; set; }

        // Item selecionado
        private Item _itemSelecionado;
        public Item ItemSelecionado
        {
            get => _itemSelecionado;
            set
            {

                _itemSelecionado = value;
                OnPropertyChanged();
                if (value != null)
                {
                    Id = value.Id;
                    Nome = value.Nome;
                    Descricao = value.Descricao;
                    Status = value.Status.ToString();
                    Preco = value.Preco;
                }
            }
        }

        // Commands
        public ICommand AdicionarCommand { get; set; }
        public ICommand AtualizarCommand { get; set; }
        public ICommand ExcluirCommand { get; set; }
        public ICommand CarregarItensCommand { get; set; }
        public ICommand SelecionarItemCommand { get; set; } // NOVO
        public ICommand CarregarHistoricoCommand { get; set; } // NOVO

        // Construtor
        public ItemViewModel()
        {
            Itens = new ObservableCollection<Item>();
            Historico = new ObservableCollection<Historico>(); // NOVA LISTA
            
            AdicionarCommand = new Command(async () => await Adicionar());
            AtualizarCommand = new Command(async () => await Atualizar());
            ExcluirCommand = new Command(async () => await Excluir());
            CarregarItensCommand = new Command(async () => await CarregarItens());
            SelecionarItemCommand = new Command<Item>(async (item) => await SelecionarItem(item)); // NOVO
            CarregarHistoricoCommand = new Command(async () => await CarregarHistorico()); // NOVO

            Task.Run(async () => await CarregarItens());
        }

        //  Método Selecionar Itens (para clientes)
        private async Task SelecionarItem(Item item)
        {
            if (UsuarioAtual.Instance.Usuario == null)
            {
                await Application.Current.MainPage.DisplayAlert("Erro", "Você precisa estar logado!", "OK");
                return;
            }

            if (item == null) return;

            // Criar registro no histórico
            var historico = new Historico
            {
                UsuarioId = UsuarioAtual.Instance.Usuario.Id,
                ItemId = item.Id,
                DataHora = DateTime.Now,
                NomeUsuario = UsuarioAtual.Instance.Usuario.Nome,
                NomeItem = item.Nome
            };

            await dataBaseService.AddHistoricoAsync(historico);
            
            await Application.Current.MainPage.DisplayAlert("Sucesso", 
                $"Item '{item.Nome}' selecionado com sucesso!", "OK");

            // Atualizar histórico
            await CarregarHistorico();
        }

        // Método para carregar histórico
        private async Task CarregarHistorico()
        {
            try
            {
                List<Historico> historicos;

                if (IsAdmin)
                {
                    // Admin vê todo o histórico
                    historicos = await dataBaseService.GetHistoricoAsync();
                }
                else
                {
                    // Cliente vê só o próprio histórico
                    if (UsuarioAtual.Instance.Usuario != null)
                    {
                        historicos = await dataBaseService.GetHistoricoByUsuarioAsync(
                            UsuarioAtual.Instance.Usuario.Id);
                    }
                    else
                    {
                        historicos = new List<Historico>();
                    }
                }

                Historico.Clear();
                foreach (var hist in historicos)
                {
                    Historico.Add(hist);
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Erro", 
                    $"Erro ao carregar histórico: {ex.Message}", "OK");
            }
        }

        // Método para adicionar item - Verifica se é admin
        private async Task Adicionar()
        {
            if (!IsAdmin)
            {
                await Application.Current.MainPage.DisplayAlert("Erro", "Apenas administradores podem adicionar itens!", "OK");
                return;
            }

            if (string.IsNullOrWhiteSpace(Nome) ||
                string.IsNullOrWhiteSpace(Descricao) ||
                string.IsNullOrWhiteSpace(Status) ||
                string.IsNullOrWhiteSpace(Preco))
            {
                await Application.Current.MainPage.DisplayAlert("Erro", "Preencha todos os campos obrigatórios!", "OK");
                return;
            }
            var ok = Enum.TryParse<Item.StatusItem>(Status, out var tipoEnum);
            if (!ok) tipoEnum = Item.StatusItem.Disponível;

            Item item = new Item
            {
                Nome = Nome,
                Descricao = Descricao,
                Status = tipoEnum,
                Preco = Preco
            };

            int result = await dataBaseService.AddItemAsync(item);

            if (result > 0)
            {
                await Application.Current.MainPage.DisplayAlert("Sucesso", "Item adicionado com sucesso!", "OK");
                await CarregarItens();
                LimparCampos();
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Erro", "Falha ao adicionar o item!", "OK");
            }
        }

        // Método para atualizar item - Verifica se é admin
        private async Task Atualizar()
        {
            if (!IsAdmin)
            {
                await Application.Current.MainPage.DisplayAlert("Erro", "Apenas administradores podem atualizar itens!", "OK");
                return;
            }

            if (Id <= 0)
            {
                await Application.Current.MainPage.DisplayAlert("Erro", "Selecione um item para atualizar!", "OK");
                return;
            }

            if (string.IsNullOrWhiteSpace(Nome) ||
                string.IsNullOrWhiteSpace(Descricao) ||
                string.IsNullOrWhiteSpace(Status) ||
                string.IsNullOrWhiteSpace(Preco))
            {
                await Application.Current.MainPage.DisplayAlert("Erro", "Preencha todos os campos obrigatórios!", "OK");
                return;
            }

            try
            {

                var ok = Enum.TryParse<Item.StatusItem>(Status, out var tipoEnum);
                if (!ok) tipoEnum = Item.StatusItem.Disponível;
                Item item = new Item
                {
                    Id = Id,
                    Nome = Nome,
                    Descricao = Descricao,
                    Status = tipoEnum,
                    Preco = Preco
                };

                int result = await dataBaseService.UpdateItemAsync(item);

                if (result > 0)
                {
                    await Application.Current.MainPage.DisplayAlert("Sucesso", "Item atualizado com sucesso!", "OK");
                    await CarregarItens();
                    LimparCampos();
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Erro", "Falha ao atualizar o item!", "OK");
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Erro", $"Erro ao atualizar item: {ex.Message}", "OK");
            }
        }

        // Método para excluir item - Verifica se é admin
        private async Task Excluir()
        {
            if (!IsAdmin)
            {
                await Application.Current.MainPage.DisplayAlert("Erro", "Apenas administradores podem excluir itens!", "OK");
                return;
            }

            if (ItemSelecionado == null)
            {
                await Application.Current.MainPage.DisplayAlert("Erro", "Selecione um item para excluir!", "OK");
                return;
            }

            try
            {
                int result = await dataBaseService.DeleteItemAsync(ItemSelecionado);

                if (result > 0)
                {
                    await Application.Current.MainPage.DisplayAlert("Sucesso", "Item excluído com sucesso!", "OK");
                    await CarregarItens();
                    LimparCampos();
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Erro", "Falha ao excluir o item!", "OK");
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Erro", $"Erro ao excluir item: {ex.Message}", "OK");
            }
        }

        // Método para carregar itens
        private async Task CarregarItens()
        {
            try
            {
                var itens = await dataBaseService.GetItensAsync();
                Itens.Clear();
                
                foreach (var item in itens)
                {
                    Itens.Add(item);
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Erro", $"Erro ao carregar itens: {ex.Message}", "OK");
            }
        }

        // Método para limpar campos
        private void LimparCampos()
        {
            Id = 0;
            Nome = string.Empty;
            Descricao = string.Empty;
            Status = string.Empty;
            Preco = string.Empty;
            ItemSelecionado = null;
        }
    }
}
