using DGIIApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DGIIApi.Services
{
    public class ComprobanteService : IComprobanteService
    {
        private readonly DgiiContext _context;

        public ComprobanteService(DgiiContext context)
        {
            _context = context;
        }

        public Task<decimal> CalcularITBIS(decimal monto) => Task.FromResult(monto * 0.18M);

        public async Task<bool> ValidarContribuyente(string rncCedula)
        {
            return await _context.ListadoContribuyentes.AnyAsync(c => c.RncCedula == rncCedula);
        }
    }
}
