using APICatalogo.Context;
using APICatalogo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {

        private readonly AppDbContext _context;

        public ProdutosController(AppDbContext context) 
        {
        
         _context = context;
        }
        [HttpGet] //extrair dados do Banco de dados// retorna para API todos os produtos incluidos no banco de dados
        public ActionResult<IEnumerable<Produto>> Get() //trás uma lista de produto, equivale ao mesmo que list, mas IEenumerable é mais otimizado
        {
           var produtos = _context.Produtos.ToList();
           
            if(produtos is null) 
            {
                return NotFound("Produtos não encontrados");
            }
            else
            return produtos;

        }
        // retorna um produto pelo ID
        [HttpGet("{id:int}", Name = "ObterProduto")]
        public ActionResult<Produto> Get(int id) 
        {
            var produto = _context.Produtos.FirstOrDefault(p => p.ProdutoId == id);
            if(produto is null) 
            {
                return NotFound("Produto não encontrado!");
            }

            return produto;

        }

        [HttpPost]

        // criar um produo via JSON
        public ActionResult Post(Produto produto) 
        {
            if (produto is null)
            {
                return BadRequest();
            }
            _context.Produtos.Add(produto);
            _context.SaveChanges(); //manda para o banco de dados
            
            return new CreatedAtRouteResult("ObterProduto", new { id = produto.ProdutoId }, produto);
        }

        // ATUALIZAÇÃO

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Produto produto) 
        {
            if (id != produto.ProdutoId) 
            {
                return BadRequest();
            }
        
                _context.Entry(produto).State = EntityState.Modified;
                _context.SaveChanges();

            return Ok(produto);
        }



        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id) 
        {
        
            var produto = _context.Produtos.FirstOrDefault(p => p.ProdutoId==id);

            if(produto is null) 
            {
                return NotFound("Produto não encontrado!");
             
            }
            _context.Produtos.Remove(produto);
            _context.SaveChanges();


            return Ok(produto);

        }


    }
}
