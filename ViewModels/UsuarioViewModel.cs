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
                // Camada de serviço que conversa com o SQLite
                UsuarioService usuarioService = new UsuarioService();

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
                        Tipo = TipoUsuario == "Administrador" ? Usuario.TipoUsuario.Administrador : Usuario.TipoUsuario.Cliente
                    };

                    int result = await UsuarioService.SalvarUsuario(usuario);

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
                    var usuario = await usuarioService.ValidarLogin(Email, Senha);

                    if (usuario == null)
                    {
                        await Application.Current.MainPage.DisplayAlert("Erro", "Email ou senha inválidos!", "OK");
                        return;
                    }

                    // Armazenar o usuário logado na Singleton
                    UsuarioAtual.Instancia.DefinirUsuario(usuario);

                    if (usuario.TipoUsuario == "Administrador")
                        await Application.Current.MainPage.Navigation.PushAsync(new AdminMainPage());
                    else
                        await Application.Current.MainPage.Navigation.PushAsync(new ClienteMainPage());
                }
            }
 }

