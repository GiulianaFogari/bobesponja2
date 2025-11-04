using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bobesponja2._0.Services
{
   public class UsuarioService
    {
            private readonly SQLiteAsyncConnection _database;

            public UsuarioService()
            {
                string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "bobburg.db3");
                _database = new SQLiteAsyncConnection(dbPath);
                _database.CreateTableAsync<Usuario>().Wait();
            }

            public Task<List<Usuario>> GetUsuarios()
            {
                return _database.Table<Usuario>().ToListAsync();
            }

            public Task<Usuario> GetUsuarioById(int id)
            {
                return _database.Table<Usuario>().Where(u => u.Id == id).FirstOrDefaultAsync();
            }

            public Task<int> SalvarUsuario(Usuario usuario)
            {
                if (usuario.Id != 0)
                    return _database.UpdateAsync(usuario);
                else
                    return _database.InsertAsync(usuario);
            }

            public Task<int> DeletarUsuario(Usuario usuario)
            {
                return _database.DeleteAsync(usuario);
            }

            public async Task<Usuario> ValidarLogin(string email, string senha)
            {
                return await _database.Table<Usuario>()
                    .Where(u => u.Email == email && u.Senha == senha)
                    .FirstOrDefaultAsync();
            }
        }
    }
