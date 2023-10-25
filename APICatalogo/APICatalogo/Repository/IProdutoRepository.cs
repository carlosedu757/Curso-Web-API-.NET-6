using APICatalogo.Models;

namespace APICatalogo.Repository;

public interface IProdutoRepository : IRepository<Produto>
{
    Task<IEnumerable<Produto>> GetProdutosPorPreco();
}
