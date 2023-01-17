using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using SistemaVenta.AplicacionWeb.Models.ViewModels;
using SistemaVenta.AplicacionWeb.Utilidades.Response;
using SistemaVenta.BLL.Interfaces;

namespace SistemaVenta.AplicacionWeb.Controllers
{
    [Authorize]
    public class DashBoardController : Controller
    {
        private readonly IDashBoardService _dashService;

        public DashBoardController(IDashBoardService dashService)
        {
            _dashService = dashService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerResumen()
        {

            GenericResponse<VMDashBoard> gResponse = new GenericResponse<VMDashBoard>();

            try
            {
                VMDashBoard vmDashBoard = new VMDashBoard();

                vmDashBoard.TotalVentas = await _dashService.TotalVentasUltimaSemana();
                vmDashBoard.TotalIngresos = await _dashService.TotalIngresosUltimaSemana();
                vmDashBoard.TotalProductos = await _dashService.TotalProductos();
                vmDashBoard.TotalCategoria = await _dashService.TotalCategorias();

                List<VMVentasSemana> listaVentasSemana = new List<VMVentasSemana>();
                List<VMProductosSemana> listaProductosSemama = new List<VMProductosSemana>();

                foreach (KeyValuePair<string, int> item in await _dashService.VentasUltimaSemana())
                {
                    listaVentasSemana.Add(new VMVentasSemana()
                    {
                        Fecha = item.Key,
                        Total = item.Value,
                    });
                    
                }

                foreach (KeyValuePair<string, int> item in await _dashService.ProductosTopUltimaSemana())
                {
                    listaProductosSemama.Add(new VMProductosSemana()
                    {
                        Producto = item.Key,
                        Cantidad = item.Value
                    });

                }

                vmDashBoard.VentasUltimaSemana = listaVentasSemana;
                vmDashBoard.ProductosTopUltimaSemana = listaProductosSemama;

                gResponse.Estado = true;
                gResponse.Objeto = vmDashBoard;

            }
            catch (Exception ex)
            {

                gResponse.Estado = false;
                gResponse.Mensaje = ex.Message;
            }

            return StatusCode(StatusCodes.Status200OK,gResponse);
        }
    }
}
