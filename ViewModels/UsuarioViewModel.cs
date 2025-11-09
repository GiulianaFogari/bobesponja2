using bobesponja2._0.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using bobesponja2._0.Services;
using SQLite;
using System.Collections.ObjectModel;

namespace bobesponja2._0.ViewModels
{
    public class UsuarioViewModel : BaseViewModel
    {
        DataBaseService dataBaseService = new DataBaseService();

        public ObservableCollection<string> TiposUsuario { get; } =
                new ObservableCollection<string>(
                    Enum.GetNames(typeof(Usuario.TipoUsuario)) // "Administrador", "Cliente"
                );

        // Propriedades da tela


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

        private string _cpf;
        public string Cpf
        {
            get => _cpf;
            set
            {
                _cpf = value;
                OnPropertyChanged();
            }
        }

        private string _email;
        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged();
            }
        }

        private string _senha;
        public string Senha
        {
            get => _senha;
            set
            {
                _senha = value;
                OnPropertyChanged();
            }
        }

        private string _tipoUsuario; // "Administrador" ou "Cliente"
        public string TipoUsuario
        {
            get => _tipoUsuario;
            set
            {
                _tipoUsuario = value;
                OnPropertyChanged();
            }
        }

        // Commands
        public ICommand CadastrarCommand { get; set; }
        public ICommand LoginCommand { get; set; }
        public ICommand LogoutCommand { get; set; }

        public ICommand AbrirCadastro { get; set; }

        // Construtor
        public UsuarioViewModel()
        {
            CadastrarCommand = new Command(async () => await Cadastrar());
            LoginCommand = new Command(async () => await Login());
            LogoutCommand = new Command(Logout);
            AbrirCadastro = new Command(() => AbrirView(new Views.UsuarioCadastroView()));
        }

        // Método de cadastro
        private async Task Cadastrar()
        {
            if (string.IsNullOrWhiteSpace(Nome) ||
                string.IsNullOrWhiteSpace(Cpf) ||
                string.IsNullOrWhiteSpace(Email) ||
                string.IsNullOrWhiteSpace(Senha) ||
                string.IsNullOrWhiteSpace(TipoUsuario))
            {
                await Application.Current.MainPage.DisplayAlert("Erro", "Preencha todos os campos obrigatórios!", "OK");
                return;
            }

            var ok = Enum.TryParse<Usuario.TipoUsuario>(TipoUsuario, out var tipoEnum);
            if (!ok) tipoEnum = Usuario.TipoUsuario.Cliente;

            Usuario usuario = new Usuario
            {
                Nome = Nome,
                CPF = Cpf,
                Email = Email,
                Senha = Senha,
                DataCadastro = DateTime.Now,
                Tipo = tipoEnum
            };

            int result = await dataBaseService.AddUsuarioAsync(usuario);

            if (result > 0)
            {
                await Application.Current.MainPage.DisplayAlert("Sucesso", "Usuário cadastrado com sucesso!", "OK");
                await Application.Current.MainPage.Navigation.PopAsync();
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Erro", "Falha ao cadastrar o usuário!", "OK");
            }
        }

        // Método de login
        private async Task Login()
        {
            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Senha))
            {
                await Application.Current.MainPage.DisplayAlert("Erro", "Preencha o email e a senha!", "OK");
                return;
            }

            try
            {
                var usuario = await dataBaseService.GetUsuarioByEmailSenhaAsync(Email, Senha);

                if (usuario == null)
                {
                    await Application.Current.MainPage.DisplayAlert("Erro", "Email ou senha inválidos!", "OK");
                    return;
                }

                //Salva o usuário logado no singleton
                UsuarioAtual.Instance.SetUsuario(usuario);

                // Navegação baseada no tipo do usuário
                if (UsuarioAtual.Instance.IsAdmin)
                {
                    await Application.Current.MainPage.DisplayAlert("Sucesso", $"Bem-vindo(a), {usuario.Nome}! (Administrador)", "OK");
                    AbrirView(new Views.AdminView());
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Sucesso", $"Bem-vindo(a), {usuario.Nome}! (Cliente)", "OK");
                    AbrirView(new Views.ClienteView());
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Erro", $"Erro ao realizar login: {ex.Message}", "OK");
            }
        }

        // Método de logout
        private void Logout()
        {
            UsuarioAtual.Instance.Logout();
        }
    }
}

