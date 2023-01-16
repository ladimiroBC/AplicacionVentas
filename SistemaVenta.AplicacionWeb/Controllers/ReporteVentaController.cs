using Microsoft.AspNetCore.Mvc;

using AutoMapper;
using SistemaVenta.AplicacionWeb.Models.ViewModels;
using SistemaVenta.BLL.Interfaces;
using System.Net.NetworkInformation;

namespace SistemaVenta.AplicacionWeb.Controllers
{
    public class ReporteVentaController : Controller
    {   
        private readonly IMapper _mapper;
        private readonly IVentaService _ventaService;

        public ReporteVentaController(IMapper mapper, IVentaService ventaService)
        {
            _mapper = mapper;
            _ventaService = ventaService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> VentaReporte(string fechaInicio, string fechaFin)
        {
            List<VMReporteVenta> vmLista = _mapper.Map<List<VMReporteVenta>>(await _ventaService.Reporte(fechaInicio, fechaFin));

            return StatusCode(StatusCodes.Status200OK, new { data = vmLista });
        }
    }
}
