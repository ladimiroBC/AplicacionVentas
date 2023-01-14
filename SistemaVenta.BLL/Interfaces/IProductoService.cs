using SistemaVenta.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaVenta.BLL.Interfaces
{
    public interface IProductoService
    {
        Task<List<Producto>> Lista();
        Task<Producto> Crear(Producto producto, Stream imagen = null, string NombreImagen = "");
        Task<Producto> Editar(Producto producto, Stream imagen = null, string NombreImagen = "");
        Task<bool> Eliminar(int idProducto);
    }
}
