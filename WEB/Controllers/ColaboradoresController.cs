﻿using System;
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
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace WEB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ColaboradoresController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;



        public ColaboradoresController(ApplicationDbContext context, RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        // GET: api/Colaboradores
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Colaborador>>> GetColaboradores()
        {
            var x = await _userManager.GetUsersInRoleAsync("Desarrollador");

            var colaboradores = await _context.Colaboradores.Where(x => x.Id != 1).ToListAsync();
            //var rolDesarrollador = _roleManager.FindByNameAsync("Desarrollador");
            //var colaboradores = await _context.Colaboradores.Where(x =>x.IdentityUser.Roles.Any(y => y.RoleId == rolDesarrollador.Result.Id)).OrderBy(x => x.Id_Odoo).ToListAsync();
            return colaboradores;
        }

        // GET: api/Colaboradores/5
        [HttpGet("{id}")]
        [Authorize]
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
        [Authorize]
        public async Task<IActionResult> PutColaborador(int id, Colaborador colaborador)
        {
            Response response = new Response();
            if (id != colaborador.Id)
            {
                response.success = false;
                response.response = "Los ID no coinciden";
                return Ok(response);
            }

            try
            {
                if (_context.Colaboradores.Any(x => x.CURP.ToUpper().Trim() == colaborador.CURP.ToUpper().Trim() && x.Id != colaborador.Id))
                {
                    response.success = false;
                    response.response = $"Ya existe un contacto con ese CURP";
                    return Ok(response);
                }
                if (_context.Colaboradores.Any(x => x.Id_Odoo.ToUpper().Trim() == colaborador.Id_Odoo.ToUpper().Trim() && x.Id != colaborador.Id))
                {
                    response.success = false;
                    response.response = $"Ya existe un contacto con esa clave Odoo";
                    return Ok(response);
                }

                Colaborador updateColaborador = new Colaborador();
                updateColaborador.Id = colaborador.Id;
                updateColaborador.Nombres = colaborador.Nombres.Trim();
                updateColaborador.Apellidos = colaborador.Apellidos.Trim();
                updateColaborador.CURP = colaborador.CURP.ToUpper().Trim();
                updateColaborador.Id_Odoo = colaborador.Id_Odoo.Trim();


                _context.Entry(updateColaborador).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                if (!ColaboradorExists(id))
                {
                   
                    response.success = false;
                    response.response = "No existe un colaborador con ese ID";
                    return Ok(response);
                }
                else
                {
                    response.success = false;
                    response.response = "Error al editar el registro";
                    return Ok(response);
                }
            }
            response.success = true;
            response.response = "Registro editado con éxito";
            return Ok(response);

        }

        // POST: api/Colaboradores
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> PostColaborador(ColaboradorPost colaborador)
        {
            Response response = new Response();
            if (_context.Colaboradores.Any(x => x.CURP.ToUpper().Trim() == colaborador.CURP.ToUpper().Trim()))
            {
                response.success = false;
                response.response = $"Ya existe un contacto con ese CURP";
                return Ok(response);
            }
            if (_context.Colaboradores.Any(x => x.Id_Odoo.ToUpper().Trim() == colaborador.Id_Odoo.ToUpper().Trim()))
            {
                response.success = false;
                response.response = $"Ya existe un contacto con esa clave Odoo";
                return Ok(response);
            }

            var c = await _userManager.FindByEmailAsync(colaborador.Email);

            if (c!=null)
            {
                response.success = false;
                response.response = $"Ya existe un contacto con ese Email";
                return Ok(response);
            }

            using (var transaction = _context.Database.BeginTransaction())
            {
                
                try
                {
                    
                    Colaborador addColaborador = new Colaborador();
                    addColaborador.Nombres = colaborador.Nombres.Trim();
                    addColaborador.Apellidos = colaborador.Apellidos.Trim();
                    addColaborador.CURP = colaborador.CURP.ToUpper().Trim();
                    addColaborador.Id_Odoo = colaborador.Id_Odoo.Trim();
                    _context.Colaboradores.Add(addColaborador);


                    ApplicationUser user = new ApplicationUser();
                    user.Email = colaborador.Email;
                    user.NormalizedEmail = colaborador.Email.Trim().ToUpper();
                    user.Colaborador = addColaborador;
                    user.EmailConfirmed = true;
                    user.UserName = colaborador.Id_Odoo;

                    var x = await _userManager.CreateAsync(user, "Pa$word1");
                    await _context.SaveChangesAsync();
                    if (x.Succeeded)
                    {
                        var y = await _userManager.AddToRoleAsync(user, "Desarrollador");
                        if (y.Succeeded)
                        {
                            //Enviar Correo
                        }
                        else
                        {
                            transaction.Rollback();
                            response.success = false;
                            response.response = $"No se pudo asignar un rol";
                            return Ok(response);
                        }
                    }
                    else
                    {
                        transaction.Rollback();
                        response.success = false;
                        response.response = $"No se pudo crear el usuario";
                        return Ok(response);
                    }

                    await _context.SaveChangesAsync();
                   
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    response.success = false;
                    response.response = $"Error al registrar";
                    return Ok(response);
                }

            }

            response.success = true;
            response.response = "Registrado con éxito";
            return Ok(response);
        }

        // DELETE: api/Colaboradores/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteColaborador(int id)
        {
            Response response = new Response();
            using (var transaction = _context.Database.BeginTransaction())
            {
                var colaborador = await _context.Colaboradores.FindAsync(id);
                if (colaborador == null)
                {
                    response.success = false;
                    response.response = "El registro no existe";
                    return Ok(response);
                }
                try
                {
                    _context.Colaboradores.Remove(colaborador);
                    await _context.SaveChangesAsync();
                }
                catch (Exception)
                {
                    response.success = false;
                    response.response = $"Error al eliminar el registro";
                    return Ok(response);
                }
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
