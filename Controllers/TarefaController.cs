using Microsoft.AspNetCore.Mvc;
using TrilhaApiDesafio.Context;
using TrilhaApiDesafio.Models;

namespace TrilhaApiDesafio.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class TarefaController(OrganizadorContext context) : ControllerBase
	{
		private readonly OrganizadorContext _context = context;


		// METHOD GET 
		[HttpGet("ObterTodos")]
		public IActionResult ObterTodos()
		{
			var tasks = _context.Tarefas.ToList();
			if (tasks.Count == 0) return NotFound();

			return Ok(tasks);
		}


		// METHOD GET - ID
		[HttpGet("ObterPorId")]
		public IActionResult ObterPorId(int id)
		{
			var task = _context.Tarefas.Find(id);
			if (task == null) return NotFound();

			return Ok(task);
		}


		// METHOD GET - TÍTULO
		[HttpGet("ObterPorTitulo")]
		public IActionResult ObterPorTitulo(string titulo)
		{
			var tasks = _context.Tarefas.Where(x => x.Titulo == titulo).ToList();
			if (tasks.Count == 0) return NotFound();

			return Ok(tasks);
		}


		// METHOD GET - DATA
		[HttpGet("ObterPorData")]
		public IActionResult ObterPorData(DateTime data)
		{
			var tasks = _context.Tarefas.Where(x => x.Data.Date == data.Date).ToList();
			if (tasks.Count == 0) return NotFound();

			return Ok(tasks);
		}


		// METHOD GET - STATUS
		[HttpGet("ObterPorStatus")]
		public IActionResult ObterPorStatus(EnumStatusTarefa status)
		{
			var tasks = _context.Tarefas.Where(x => x.Status == status).ToList();
			if (tasks.Count == 0) return NotFound();

			return Ok(tasks);
		}


		// METHOD POST
		[HttpPost]
		public IActionResult Criar(Tarefa tarefa)
		{
			if (tarefa.Titulo.Trim() == "" || tarefa.Titulo == null) return BadRequest(new { Erro = "O título da tarefa não pode ser vazio" });
			if (tarefa.Data == DateTime.MinValue) return BadRequest(new { Erro = "A data da tarefa não pode ser vazia" });

			_context.Tarefas.Add(tarefa);
			_context.SaveChanges();
			return CreatedAtAction(nameof(ObterPorId), new { id = tarefa.Id }, tarefa);
		}


		// METHOD GET - UPDATE
		[HttpPut("{id}")]
		public IActionResult Atualizar(int id, Tarefa tarefa)
		{
			var task = _context.Tarefas.Find(id);
			if (task == null) return NotFound();
			if (tarefa.Titulo.Trim() == "" || tarefa.Titulo == null) return BadRequest(new { Erro = "O título da tarefa não pode ser vazio" });
			if (tarefa.Data == DateTime.MinValue) return BadRequest(new { Erro = "A data da tarefa não pode ser vazia" });

			task.Titulo = tarefa.Titulo;
			task.Descricao = tarefa.Descricao;
			task.Data = tarefa.Data;
			task.Status = tarefa.Status;

			_context.Tarefas.Update(task);
			_context.SaveChanges();
			return Ok(task);
		}


		// METHOD DELETE
		[HttpDelete("{id}")]
		public IActionResult Deletar(int id)
		{
			var task = _context.Tarefas.Find(id);
			if (task == null) return NotFound();

			_context.Tarefas.Remove(task);
			_context.SaveChanges();
			return NoContent();
		}


		// METHOD DELETE - TODOS
		[HttpDelete("DeletarTodos")]
		public IActionResult DeletarTodos()
		{
			var tasks = _context.Tarefas.ToList();
			if (tasks.Count == 0) return NotFound();

			foreach (var task in tasks) _context.Tarefas.Remove(task);
			_context.SaveChanges();
			return NoContent();
		}
	}
}
