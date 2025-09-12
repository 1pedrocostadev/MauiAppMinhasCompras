using MauiAppMinhasCompras.Models;
using SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MauiAppMinhasCompras.Helpers
{
    public class SQLiteDatabaseHelperBase
    {
        private readonly SQLiteAsyncConnection _conn;

        public SQLiteDatabaseHelperBase(string dbPath)
        {
            _conn = new SQLiteAsyncConnection(dbPath);
            // Cria a tabela se ela não existir
            _conn.CreateTableAsync<Produto>().Wait();
        }

        public async Task<List<Produto>> GetAll()
        {
            return await _conn.Table<Produto>().ToListAsync();
        }

        public async Task<List<Produto>> GetByCategoria(string categoria)
        {
            if (categoria == "Todos")
                return await GetAll();

            return await _conn.Table<Produto>()
                             .Where(p => p.Categoria == categoria)
                             .ToListAsync();
        }

        // Métodos adicionais úteis
        public async Task<int> Insert(Produto produto)
        {
            return await _conn.InsertAsync(produto);
        }

        public async Task<int> Update(Produto produto)
        {
            return await _conn.UpdateAsync(produto);
        }

        public async Task<int> Delete(Produto produto)
        {
            return await _conn.DeleteAsync(produto);
        }

        public async Task<Produto> GetById(int id)
        {
            return await _conn.Table<Produto>()
                             .Where(p => p.Id == id)
                             .FirstOrDefaultAsync();
        }
    }
}