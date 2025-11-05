using bobesponja2._0.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using bobesponja2._0.Services;
using SQLite;

namespace bobesponja2._0.ViewModels
{
    public class UsuarioViewModel : BaseViewModel
    {
        DataBaseService dataBaseService = new DataBaseService();

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

        // Construtor
        public UsuarioViewModel()
        {
            CadastrarCommand = new Command(async () => await Cadastrar());
            LoginCommand = new Command(async () => await Login());
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

            // Ensure the correct namespace is used for the Usuario type
            Usuario usuario = new Usuario
            {
                Nome = Nome,
                CPF = Cpf,
                Email = Email,
                Senha = Senha,
                DataCadastro = DateTime.Now,
                Tipo = TipoUsuario == "Administrador" ? Usuario.TipoUsuario.Administrador : Usuario.TipoUsuario.Cliente
            };

            int result = await dataBaseService.AddUsuarioAsync(usuario);

            if (result > 0)
            {
                await Application.Current.MainPage.DisplayAlert("Sucesso", "Usuário cadastrado com sucesso!", "OK");
                await Application.Current.MainPage.Navigation.PopAsync(); // Volta para a tela de login
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Erro", "Falha ao cadastrar o usuário!", "OK");
            }
        }

        // Método de login
        private async Task Login()
        {
            // Validar campos de login
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

                // Navegação baseada no tipo do usuário
                if (usuario.Tipo == Usuario.TipoUsuario.Administrador)
                {
                    // TODO: Implementar navegação para página de administrador
                    await Application.Current.MainPage.DisplayAlert("Sucesso", "Login realizado como Administrador!", "OK");
                }
                else
                {
                    // TODO: Implementar navegação para página de cliente
                    await Application.Current.MainPage.DisplayAlert("Sucesso", "Login realizado como Cliente!", "OK");
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Erro", $"Erro ao realizar login: {ex.Message}", "OK");
            }
        }
    }
}

