
using SistemaVenta.AplicacionWeb.Models.ViewModels;
using SistemaVenta.Entity;
using System.Globalization;
using AutoMapper;

namespace SistemaVenta.AplicacionWeb.Utilidades.Automapper
{
    public class AutoMapperProfile: Profile
    {
        public AutoMapperProfile()
        {
            #region Rol
            CreateMap<Rol, VMRol>().ReverseMap();
            #endregion

            #region Usuario
            CreateMap<Usuario, VMUsuario>()
                .ForMember(destiny => destiny.EsActivo,
                opt => opt.MapFrom(origen => origen.EsActivo == true ? 1 : 0)
                )
                .ForMember(destiny => destiny.NombreRol,
                opt => opt.MapFrom(origen => origen.IdRolNavigation.Descripcion)
                );

            CreateMap<VMUsuario, Usuario>()
                .ForMember(destiny => destiny.EsActivo,
                opt => opt.MapFrom(origen => origen.EsActivo == 1 ? true : false)
                )
                .ForMember(destiny => destiny.IdRolNavigation,
                opt => opt.Ignore()
                );
            #endregion

            #region Negocio
            CreateMap<Negocio, VMNegocio>()
                .ForMember(d => d.PorcentajeImpuesto,
                opt => opt.MapFrom(o => Convert.ToString(o.PorcentajeImpuesto.Value, new CultureInfo("es-CO")))
                );

            CreateMap<VMNegocio, Negocio>()
                .ForMember(d => d.PorcentajeImpuesto,
                opt => opt.MapFrom(o => Convert.ToDecimal(o.PorcentajeImpuesto, new CultureInfo("es-CO")))
                );
            #endregion

            #region Categoria
            CreateMap<Categoria, VMCategoria>()
                .ForMember(d => d.EsActivo,
                opt => opt.MapFrom(o => o.EsActivo == true ? 1 : 0)
                );
            CreateMap<VMCategoria, Categoria>()
                .ForMember(d => d.EsActivo,
                opt => opt.MapFrom(o => o.EsActivo == 1 ? true :false)
                );
            #endregion

            #region Producto
            CreateMap<Producto, VMProducto>()
                .ForMember(d => d.EsActivo,
                opt => opt.MapFrom(o => o.EsActivo == true ? 1 : 0)
                )
                .ForMember(d => d.Precio,
                opt => opt.MapFrom(o => Convert.ToString(o.Precio.Value, new CultureInfo("es-CO")))
                );

            CreateMap<VMProducto, Producto>()
                .ForMember(d => d.EsActivo,
                opt => opt.MapFrom(o => o.EsActivo == 1 ? true : false)
                )
                .ForMember(d => d.Precio,
                opt => opt.MapFrom(o => Convert.ToDecimal(o.Precio, new CultureInfo("es-CO")))
                );
            #endregion

            #region TipoDocumentoVenta
            CreateMap<TipoDocumentoVenta, VMTipoDocumentoVenta>().ReverseMap();
            #endregion

            #region Venta
            CreateMap<Venta, VMVenta>()
                .ForMember(d => d.TipoDocumentoVenta,
                opt => opt.MapFrom(o => o.IdTipoDocumentoVentaNavigation.Descripcion)
                )
                .ForMember(d => d.Usuario,
                opt => opt.MapFrom(o => o.IdUsuarioNavigation.Nombre)
                )
                .ForMember(d => d.SubTotal,
                opt => opt.MapFrom(o => Convert.ToString(o.SubTotal.Value, new CultureInfo("es-CO")))
                )
                .ForMember(d => d.ImpuestoTotal,
                opt => opt.MapFrom(o => Convert.ToString(o.ImpuestoTotal.Value, new CultureInfo("es-CO")))
                )
                .ForMember(d => d.Total,
                opt => opt.MapFrom(o => Convert.ToString(o.Total.Value, new CultureInfo("es-CO")))
                )
                .ForMember(d => d.FechaRegistro,
                opt => opt.MapFrom(o => o.FechaRegistro.Value.ToString("dd/MM/yyyy"))
                );

            CreateMap<VMVenta, Venta>()
                .ForMember(d => d.SubTotal,
                opt => opt.MapFrom(o => Convert.ToDecimal(o.SubTotal, new CultureInfo("es-CO")))
                )
                .ForMember(d => d.ImpuestoTotal,
                opt => opt.MapFrom(o => Convert.ToDecimal(o.ImpuestoTotal, new CultureInfo("es-CO")))
                )
                .ForMember(d => d.Total,
                opt => opt.MapFrom(o => Convert.ToDecimal(o.Total, new CultureInfo("es-CO")))
                );
            #endregion

            #region DetalleVenta
            CreateMap<DetalleVenta, VMDetalleVenta>()
                .ForMember(d => d.Precio,
                opt => opt.MapFrom(o => Convert.ToString(o.Precio.Value, new CultureInfo("es-CO")))
                )
                .ForMember(d => d.Total,
                opt => opt.MapFrom(o => Convert.ToString(o.Total.Value, new CultureInfo("es-CO")))
                );

            CreateMap<VMDetalleVenta, DetalleVenta>()
                .ForMember(d => d.Precio,
                opt => opt.MapFrom(o => Convert.ToDecimal(o.Precio, new CultureInfo("es-CO")))
                )
                .ForMember(d => d.Total,
                opt => opt.MapFrom(o => Convert.ToDecimal(o.Total, new CultureInfo("es-CO")))
                );

            CreateMap<DetalleVenta, VMReporteVenta>()
                .ForMember(d => d.FechaRegistro,
                opt => opt.MapFrom(o => o.IdVentaNavigation.FechaRegistro.Value.ToString("dd/MM/yyyy"))
                )
                .ForMember(d => d.NumeroVenta,
                opt => opt.MapFrom(o => o.IdVentaNavigation.NumeroVenta)
                )
                .ForMember(d => d.TipoDocumento,
                opt => opt.MapFrom(o => o.IdVentaNavigation.IdTipoDocumentoVentaNavigation.Descripcion)
                )
                .ForMember(d => d.DocumentoCliente,
                opt => opt.MapFrom(o => o.IdVentaNavigation.DocumentoCliente)
                )
                .ForMember(d => d.NombreCliente,
                opt => opt.MapFrom(o => o.IdVentaNavigation.NombreCliente)
                )
                .ForMember(d => d.SubTotalVenta,
                opt => opt.MapFrom(o => Convert.ToString(o.IdVentaNavigation.SubTotal.Value, new CultureInfo("es-CO")))
                )
                .ForMember(d => d.ImpuestoTotalVenta,
                opt => opt.MapFrom(o => Convert.ToString(o.IdVentaNavigation.ImpuestoTotal.Value, new CultureInfo("es-CO")))
                )
                .ForMember(d => d.TotalVenta,
                opt => opt.MapFrom(o => Convert.ToString(o.IdVentaNavigation.Total.Value, new CultureInfo("es-CO")))
                )
                .ForMember(d => d.Producto,
                opt => opt.MapFrom(o => o.DescripcionProducto)
                )
                .ForMember(d => d.Precio,
                opt => opt.MapFrom(o => Convert.ToString(o.Precio.Value, new CultureInfo("es-CO")))
                )
                .ForMember(d => d.Total,
                opt => opt.MapFrom(o => Convert.ToString(o.Total.Value, new CultureInfo("es-CO")))
                );

            #endregion

            #region Menu
            CreateMap<Menu, VMMenu>()
                .ForMember(d => d.SubMenus,
                opt => opt.MapFrom(o => o.InverseIdMenuPadreNavigation)
                );
            #endregion



        }
    }
}
