using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DGIIApi.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DGIIApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComprobantesFiscalesController : ControllerBase
    {
        private readonly DgiiContext _context;

        public ComprobantesFiscalesController(DgiiContext context)
        {
            _context = context;
        }

        // GET: api/ComprobantesFiscales/{rncCedula}
        [HttpGet("{rncCedula}")]
        public async Task<ActionResult<IEnumerable<ComprobantesFiscale>>> GetComprobantesByRncCedula(string rncCedula)
        {
            var comprobantes = await _context.ComprobantesFiscales
                .Where(c => c.RncCedula == rncCedula)
                .ToListAsync();

            if (comprobantes == null || !comprobantes.Any())
            {
                return NotFound();
            }

            return Ok(comprobantes);
        }

        // POST: api/ComprobantesFiscales
        [HttpPost]
        public async Task<IActionResult> CreateComprobante([FromBody] CreateComprobanteRequest request)
        {
            if (request == null)
            {
                return BadRequest("Solicitud no válida.");
            }

            // Validar si la cédula existe en la tabla ListadoContribuyente
            var contribuyenteExists = await _context.ListadoContribuyentes
                .AnyAsync(c => c.RncCedula == request.RncCedula);

            if (!contribuyenteExists)
            {
                return BadRequest("La cédula no existe en la lista de contribuyentes.");
            }

            // Calcular el ITBIS
            decimal itbis18 = request.Monto * 0.18M;

            // Insertar el comprobante
            await _context.InsertComprobanteAsync(request.RncCedula, request.Monto, itbis18);

            // Crear una respuesta de comprobante sin NCF
            var comprobante = new
            {
                RncCedula = request.RncCedula,
                Monto = request.Monto,
                ITBIS18 = itbis18 // ITBIS calculado automáticamente
            };

            return CreatedAtAction(nameof(GetComprobantesByRncCedula), new { rncCedula = request.RncCedula }, comprobante);
        }
    }
}
