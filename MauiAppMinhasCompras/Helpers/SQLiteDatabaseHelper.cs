using SQLite;
using MauiAppMinhasCompras.Models;

namespace MauiAppMinhasCompras.Helpers
{
    public class SQLiteDatabaseHelper : SQLiteDatabaseHelperBase
    {
        readonly SQLiteAsyncConnection _conn;
        public SQLiteDatabaseHelper(string path) : base(path)
        {
            _conn = new SQLiteAsyncConnection(path);
            _conn.CreateTableAsync<Produto>().Wait();
        }

        public Task<int> insertProduto(Produto p)
        {
            return _conn.InsertAsync(p);
        }
        public Task<List<Produto>> Update(Produto p)
        {
            string sql = "UPDATE Produto SET Descricao=?, Quantidade=?, Preco=? WHERE Id =?";
            return _conn.QueryAsync<Produto>(
             sql, p.Descricao, p.Quantidade, p.Preco, p.Id
             );
        }
        public Task<int> Delete(int id)
        {
            return _conn.Table<Produto>()
                .Where(p => p.Id == id)
                .DeleteAsync();
        }
        public Task<List<Produto>> GetAll() {
            return _conn.Table<Produto>().ToListAsync();
        }
        public Task<List<Produto>> Search(string q)
        {
            string sql = "SELECT * FROM Produto WHERE Descricao LIKE '%" + q + "%'";
            return _conn.QueryAsync<Produto>(sql);
        }

        public async Task<List<(string Categoria, double Total)>> GetTotalByCategoria()
        {

            var todos = await GetAll();
            return todos
                .GroupBy(p => p.Categoria)
                .Select(g => (Categoria: g.Key, Total: g.Sum(p => p.Total)))
                .ToList();

        }
    }


}

