namespace SistemaVenta.AplicacionWeb.Models.ViewModels
{
    public class VMDashBoard
    {
        public int TotalVentas { get; set; }
        public string? TotalIngresos { get; set; }
        public int TotalProductos { get; set; }
        public int TotalCategoria { get; set; }
        public List<VMVentasSemana> VentasUltimaSemanas { get; set; }
        public List<VMProductosSemana> ProductosTopUltimaSeamana { get; set; }
    }
}
