using System.Threading.Tasks;

namespace DGIIApi.Services
{
    public interface IComprobanteService
    {
        Task<decimal> CalcularITBIS(decimal monto);
        Task<bool> ValidarContribuyente(string rncCedula);
    }
}
