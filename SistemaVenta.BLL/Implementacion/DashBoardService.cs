using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using SistemaVenta.BLL.Interfaces;
using SistemaVenta.DAL.Interfaces;
using SistemaVenta.Entity;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace SistemaVenta.BLL.Implementacion
{
    public class DashBoardService : IDashBoardService
    {

        private readonly IVentaRepository _repositoryVenta;
        private readonly IGenericRepository<DetalleVenta> _repositoryDetalle;
        private readonly IGenericRepository<Categoria> _repositoryCategoria;
        private readonly IGenericRepository<Producto> _repositoryProducto;
        private DateTime FechaInicio = DateTime.Now;

        public DashBoardService
            (
            IVentaRepository repositoryVenta,
            IGenericRepository<DetalleVenta> repositoryDetalle, 
            IGenericRepository<Categoria> repositoryCategoria, 
            IGenericRepository<Producto> repositoryProducto 
            
            )
        {
            _repositoryVenta = repositoryVenta;
            _repositoryDetalle = repositoryDetalle;
            _repositoryCategoria = repositoryCategoria;
            _repositoryProducto = repositoryProducto;
            
            FechaInicio = FechaInicio.AddDays( -7 );
        }

        public async Task<int> TotalVentasUltimaSemana()
        {
            try
            {
                IQueryable<Venta> query = await _repositoryVenta.Consultar(v => v.FechaRegistro.Value.Date >= FechaInicio.Date);
                int total = query.Count();
                return total;
            }
            catch
            {

                throw;
            }
        }

        public async Task<string> TotalIngresosUltimaSemana()
        {
            try
            {
                IQueryable<Venta> query = await _repositoryVenta.Consultar(v => v.FechaRegistro.Value.Date >= FechaInicio.Date);
                decimal resultado = query
                    .Select(v => v.Total)
                    .Sum(v => v.Value);

                return Convert.ToString(resultado,new CultureInfo("es-CO"));
            }
            catch
            {

                throw;
            }
        }

        public async Task<int> TotalProductos()
        {
            try
            {
                IQueryable<Producto> query = await _repositoryProducto.Consultar();
                int total = query.Count();
                return total;
            }
            catch
            {

                throw;
            }
        }        

        public async Task<int> TotalCategorias()
        {
            try
            {
                IQueryable<Categoria> query = await _repositoryCategoria.Consultar();
                int total = query.Count();
                return total;
            }
            catch
            {

                throw; 
            }
        }
        
        public async Task<Dictionary<string, int>> VentasUltimaSemana()
        {
            try
            {
                IQueryable<Venta> query = await _repositoryVenta.Consultar(v => v.FechaRegistro.Value.Date >= FechaInicio.Date);

                Dictionary<string, int> resultado = query
                    .GroupBy(v => v.FechaRegistro.Value.Date).OrderByDescending(g => g.Key)
                    .Select(dv => new { Fecha = dv.Key.ToString("dd/MM/yyyy"), total = dv.Count() })
                    .ToDictionary(keySelector: r => r.Fecha, elementSelector: r => r.total);

                return resultado;
            }
            catch 
            {

                throw;
            }
        }

        public async Task<Dictionary<string, int>> ProductosTopUltimaSemana()
        {
            try
            {
                IQueryable<DetalleVenta> query = await _repositoryDetalle.Consultar();

                Dictionary<string, int> resultado = query
                    .Include(v => v.IdVentaNavigation)
                    .Where(dv => dv.IdVentaNavigation.FechaRegistro.Value.Date >= FechaInicio.Date)
                    .GroupBy(dv => dv.DescripcionProducto).OrderByDescending(g => g.Count())
                    .Select(dv => new { producto = dv.Key, total = dv.Count() }).Take(4)
                    .ToDictionary(keySelector: r => r.producto, elementSelector: r => r.total);

                return resultado;
            }
            catch
            {

                throw;
            }
        }

    }
}
