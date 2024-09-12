using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DGIIApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DGIIApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContribuyentesController : ControllerBase
    {
        private readonly DgiiContext _context;

        public ContribuyentesController(DgiiContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ListadoContribuyente>>> GetContribuyentes()
        {
            return await _context.ListadoContribuyentes.ToListAsync();
        }

        // Método para crear un nuevo contribuyente
        [HttpPost]
        public async Task<ActionResult<ListadoContribuyente>> CreateContribuyente(ListadoContribuyente contribuyente)
        {
            // Validar longitud de la cédula
            if (contribuyente.RncCedula.Length != 11 || !long.TryParse(contribuyente.RncCedula, out _))
            {
                return BadRequest(new { message = "La cédula debe tener exactamente 11 dígitos." });
            }

            // Validar si ya existe un contribuyente con el mismo RNC o Cédula
            var exists = await _context.ListadoContribuyentes
                .AnyAsync(c => c.RncCedula == contribuyente.RncCedula);

            if (exists)
            {
                // Retornar un error si ya existe
                return Conflict(new { message = "Ya existe un contribuyente con este RNC o Cédula." });
            }

            // Agregar el nuevo contribuyente al contexto y guardar cambios
            _context.ListadoContribuyentes.Add(contribuyente);
            await _context.SaveChangesAsync();

            // Retornar el contribuyente creado con un código de éxito 201 (Created)
            return CreatedAtAction(nameof(GetContribuyentes), new { id = contribuyente.RncCedula }, contribuyente);
        }
    }
}
