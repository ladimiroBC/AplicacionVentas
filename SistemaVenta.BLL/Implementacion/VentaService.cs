using Microsoft.EntityFrameworkCore;
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
    public class VentaService : IVentaService
    {
        private readonly IGenericRepository<Producto> _repository;
        private readonly IVentaRepository _ventaRepository;

        public VentaService(IGenericRepository<Producto> repository, IVentaRepository ventaRepository)
        {
            _repository= repository;
            _ventaRepository= ventaRepository;
        }

        public async Task<List<Producto>> ObtenerProductos(string busqueda)
        {
            IQueryable<Producto> query = await _repository.Consultar(
                p => p.EsActivo == true && p.Stock > 0 && 
                string.Concat(p.CodigoBarra, p.Marca, p.Descripcion).Contains(busqueda)
                );
                
                return query.Include(c=>c.IdCategoriaNavigation).ToList();
        }

        public async Task<Venta> Detalle(string numeroVenta)
        {
            IQueryable<Venta> query = await _ventaRepository.Consultar(v=>v.NumeroVenta == numeroVenta);

            return query                
                        .Include(tdv => tdv.IdTipoDocumentoVentaNavigation)
                        .Include(u => u.IdUsuarioNavigation)
                        .Include(dv => dv.DetalleVenta)
                        .First();
        
        }

        public async Task<List<Venta>> Historial(string numeroVenta, string fechaInicio, string fechaFin)
        {
            IQueryable<Venta> query = await _ventaRepository.Consultar();
            fechaInicio = fechaInicio is null ? "" : fechaInicio;
            fechaFin = fechaFin is null ? "" : fechaFin;

            if (fechaInicio != "" && fechaFin != "")
            {
                DateTime fecha_inicio = DateTime.ParseExact(fechaInicio, "dd/MM/yyyy", new CultureInfo("es-CO"));
                DateTime fecha_fin = DateTime.ParseExact(fechaFin, "dd/MM/yyyy", new CultureInfo("es-CO"));

                return query.Where(v=>
                    v.FechaRegistro.Value.Date >= fecha_inicio.Date &&
                    v.FechaRegistro.Value.Date <= fecha_fin.Date
                )
                    .Include(tdv=>tdv.IdTipoDocumentoVentaNavigation)
                    .Include(u=>u.IdUsuarioNavigation)
                    .Include(dv=>dv.DetalleVenta)
                    .ToList();
            }
            else
            {
                return query.Where(v => v.NumeroVenta == numeroVenta
                )
                    .Include(tdv => tdv.IdTipoDocumentoVentaNavigation)
                    .Include(u => u.IdUsuarioNavigation)
                    .Include(dv => dv.DetalleVenta)
                    .ToList();
            }
        }


        public async Task<Venta> Registrar(Venta venta)
        {
            try
            {
                return await _ventaRepository.Registrar(venta);
            }
            catch 
            {

                throw;
            }
        }

        public async Task<List<DetalleVenta>> Reporte(string fechaInicio, string fechaFin)
        {
            DateTime fecha_inicio = DateTime.ParseExact(fechaInicio, "dd/MM/yyyy", new CultureInfo("es-CO"));
            DateTime fecha_fin = DateTime.ParseExact(fechaFin, "dd/MM/yyyy", new CultureInfo("es-CO"));

            List<DetalleVenta> list = await _ventaRepository.Reporte(fecha_inicio,fecha_fin);

            return list;
        }
    }
}
