using SQLite;
using PCLExt.FileStorage.Folders;
using bobesponja2._0.Models;

namespace bobesponja2._0.Services
{
    public class DataBaseService
    {

        private SQLiteAsyncConnection _db;

        public DataBaseService()
        {
            var folder = new LocalRootFolder();
            var file = folder.CreateFile("bobburg.db",
                PCLExt.FileStorage.CreationCollisionOption.OpenIfExists);

            _db = new SQLiteAsyncConnection(file.Path);
        }

        // Inicializa as tabelas
        public async Task InitAsync()
        {
            await _db.CreateTableAsync<Usuario>();
            await _db.CreateTableAsync<Item>();
            await _db.CreateTableAsync<Pedido>();
            await _db.CreateTableAsync<Historico>();
        }

        public async Task<int> AddUsuarioAsync(Usuario usuario)
        {
            return await _db.InsertAsync(usuario);
        }

        public async Task<Usuario> GetUsuarioByEmailSenhaAsync(string email, string senha)
        {
            return await _db.Table<Usuario>()
                .Where(usuario => usuario.Email == email && usuario.Senha == senha)
                .FirstOrDefaultAsync();
        }

        public async Task<List<Usuario>> GetUsuariosAsync()
        {
            return await _db.Table<Usuario>().ToListAsync();
        }

        public async Task<int> AddItemAsync(Item item)
        {
            return await _db.InsertAsync(item);
        }

        public async Task<int> UpdateItemAsync(Item item)
        {
            return await _db.UpdateAsync(item);
        }

        public async Task<int> DeleteItemAsync(Item item)
        {
            return await _db.DeleteAsync(item);
        }

        public async Task<List<Item>> GetItensAsync()
        {
            return await _db.Table<Item>().ToListAsync();
        }

        public async Task<int> AddPedidoAsync(Pedido pedido)
        {
            return await _db.InsertAsync(pedido);
        }

        public async Task<List<Pedido>> GetPedidosAsync()
        {
            return await _db.Table<Pedido>().ToListAsync();
        }

        public async Task<List<Pedido>> GetPedidosByUsuarioAsync(int usuarioId)
        {
            return await _db.Table<Pedido>()
                .Where(pedido => pedido.UsuarioId == usuarioId)
                .ToListAsync();
        }

        // Métodos para o Histórico
        public async Task<int> AddHistoricoAsync(Historico historico)
        {
            return await _db.InsertAsync(historico);
        }

        public async Task<List<Historico>> GetHistoricoAsync()
        {
            return await _db.Table<Historico>()
                .OrderByDescending(h => h.DataHora)
                .ToListAsync();
        }

        public async Task<List<Historico>> GetHistoricoByUsuarioAsync(int usuarioId)
        {
            return await _db.Table<Historico>()
                .Where(h => h.UsuarioId == usuarioId)
                .OrderByDescending(h => h.DataHora)
                .ToListAsync();
        }
    }
}

