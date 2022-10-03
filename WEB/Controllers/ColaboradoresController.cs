using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WEB.Data;
using WEB.Models;
using Newtonsoft.Json;
using WEB.ViewModels;

namespace WEB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ColaboradoresController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ColaboradoresController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Colaboradores
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Colaborador>>> GetColaboradores()
        {
            return await _context.Colaboradores.OrderBy(x=>x.Id_Odoo).ToListAsync();
        }

        // GET: api/Colaboradores/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Colaborador>> GetColaborador(int id)
        {
            var colaborador = await _context.Colaboradores.FindAsync(id);

            if (colaborador == null)
            {
                return NotFound();
            }

            return Ok(colaborador);
        }

        // PUT: api/Colaboradores/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutColaborador(int id, Colaborador colaborador)
        {
            Response response = new Response();
            if (id != colaborador.Id)
            {
                response.success = false;
                response.response = "Los ID no coinciden";
                return BadRequest(response);
            }

            _context.Entry(colaborador).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                if (!ColaboradorExists(id))
                {
                   
                    response.success = false;
                    response.response = "No existe un colaborador con ese ID";
                    return BadRequest(response);
                }
                else
                {
                    response.success = false;
                    response.response = "Error al editar el registro";
                    return BadRequest(response);
                }
            }
            response.success = true;
            response.response = "Registro editado con éxito";
            return Ok(response);

        }

        // POST: api/Colaboradores
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Colaborador>> PostColaborador(Colaborador colaborador)
        {
            Response response = new Response();
            try
            {
                _context.Colaboradores.Add(colaborador);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                response.success = false;
                response.response = $"Error al registrar {ex.Message}";
                return BadRequest(response);
            }
            


            response.success = true;
            response.response = "Registrado con éxito";
            return Ok(response);
        }

        // DELETE: api/Colaboradores/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteColaborador(int id)
        {
            Response response = new Response();
            var colaborador = await _context.Colaboradores.FindAsync(id);
            if (colaborador == null)
            {
                response.success = false;
                response.response = "El registro no existe";
                return NotFound(response);
            }
            try
            {
                _context.Colaboradores.Remove(colaborador);
                await _context.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                response.success = false;
                response.response = $"Error al eliminar el registro {ex.Message}";
                return BadRequest(response);
            }

            response.success = true;
            response.response = "Registro eliminado con éxito";
            return Ok(response);
        }

        private bool ColaboradorExists(int id)
        {
            return _context.Colaboradores.Any(e => e.Id == id);
        }
    }
}
