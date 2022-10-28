using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrilhaApiDesafio.Context;
using TrilhaApiDesafio.Models;

namespace TrilhaApiDesafio.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TarefaController : ControllerBase
    {
        private readonly OrganizadorContext _context;

        public TarefaController(OrganizadorContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public IActionResult ObterPorId(int id)
        {
            
            var tarefa = _context.Tarefas.Find(id);
            
            if(tarefa == null)
                return NotFound();
            
            return Ok(tarefa);
        }

        [HttpGet("ObterTodos")]
        public async Task<ActionResult<IEnumerable<Tarefa>>> GetCategorias()
        {
            return await _context.Tarefas.ToListAsync();
        }

        [HttpGet("ObterPorTitulo/{titulo}")]
        public IActionResult ObterPorTitulo(string titulo)
        {
            
            var tarefas = _context.Tarefas.Where(x => x.Titulo.Contains(titulo));
            if ( tarefas == null)
            {
                return NotFound();
            }
            return Ok(tarefas);
        }

        [HttpGet("ObterPorData")]
        public IActionResult ObterPorData(DateTime data)
        {
            var tarefas = _context.Tarefas.Where(x => x.Data.Date == data.Date);
             if ( tarefas == null)
            {
                return NotFound();
            }
            return Ok(tarefas);
        }

        [HttpGet("ObterPorStatus/{status}")]
        public IActionResult ObterPorStatus(EnumStatusTarefa status)
        {
            var tarefas = _context.Tarefas.Where(x => x.Status == status);
             if ( tarefas == null)
            {
                return NotFound();
            }
            return Ok(tarefas);
        }

        [HttpPost]
        public IActionResult Criar(Tarefa tarefa)
        {

            if (tarefa.Data == DateTime.MinValue)
                return BadRequest(new { Erro = "A data da tarefa não pode ser vazia" });
            _context.Add(tarefa);
            _context.SaveChanges();
            return CreatedAtAction(nameof(ObterPorId), new { id = tarefa.Id }, tarefa);
        }

        [HttpPut("{id}")]
        public IActionResult Atualizar(int id, Tarefa tarefa)
        {
            var tarefaBanco = _context.Tarefas.Find(id);

            if (tarefaBanco == null)
                return NotFound();

            if (tarefa.Data == DateTime.MinValue)
                return BadRequest(new { Erro = "A data da tarefa não pode ser vazia" });

            tarefaBanco.Titulo = tarefa.Titulo;
            tarefaBanco.Status = tarefa.Status;
            tarefaBanco.Descricao = tarefa.Descricao;
            tarefaBanco.Data = tarefa.Data;
            _context.Tarefas.Update(tarefaBanco);
            _context.SaveChanges();
            return Ok(tarefaBanco);    
        }

        [HttpDelete("{id}")]
        public IActionResult Deletar(int id)
        {
            var tarefaBanco = _context.Tarefas.Find(id);

            if (tarefaBanco == null)
                return NotFound();

            // TODO: Remover a tarefa encontrada através do EF e salvar as mudanças (save changes)
            _context.Tarefas.Remove(tarefaBanco);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
