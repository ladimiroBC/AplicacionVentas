using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using SistemaVenta.BLL.Interfaces;
using SistemaVenta.DAL.Interfaces;
using SistemaVenta.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaVenta.BLL.Implementacion
{
    public class ProductoService : IProductoService
    {   
        private readonly IGenericRepository<Producto> _repository;
        private readonly IFireBaseService _firebaseService;
        private readonly IUtilidadesService _utilidadesService;

        public ProductoService(IGenericRepository<Producto> repository, IFireBaseService firebaseService)
        {
            _repository = repository;
            _firebaseService = firebaseService;
            
        }

        public async Task<List<Producto>> Lista()
        {
            IQueryable<Producto> query = await _repository.Consultar();
            return query.Include(c=>c.IdCategoriaNavigation).ToList();
        }

        public async Task<Producto> Crear(Producto producto, Stream imagen = null, string NombreImagen = "")
        {
            Producto producto_existe = await _repository.Obtener(p => p.CodigoBarra == producto.CodigoBarra);

            if (producto_existe != null)
                throw new TaskCanceledException("El codigo de barra ya existe");

            try
            {
                producto.NombreImagen = NombreImagen;
                if (imagen != null)
                {
                    string urlImagen = await _firebaseService.SubirStorage(imagen, "carpeta_producto", NombreImagen);
                    producto.UrlImagen = urlImagen;
                }

                Producto productoCreado = await _repository.Crear(producto);

                if (productoCreado.IdProducto == 0)
                    throw new TaskCanceledException("No se pudo crear el prodcuto");

                IQueryable<Producto> query = await _repository.Consultar(p => p.IdProducto == productoCreado.IdProducto);
                
                productoCreado = query.Include(c=>c.IdCategoriaNavigation).First();
                return productoCreado;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<Producto> Editar(Producto producto, Stream imagen = null, string NombreImagen = "")
        {
           Producto producto_existe = await _repository.Obtener(p=>p.CodigoBarra == producto.CodigoBarra && p.IdProducto != producto.IdProducto);
            
           if(producto_existe != null)
                throw new TaskCanceledException("El codigo de barra ya existe");

            try
            {
                IQueryable<Producto> queryProducto = await _repository.Consultar(p => p.IdProducto == producto.IdProducto);

                Producto productoEditar = queryProducto.First();

                productoEditar.CodigoBarra = producto.CodigoBarra;
                productoEditar.Marca = producto.Marca;
                productoEditar.Descripcion = producto.Descripcion;
                productoEditar.IdCategoria = producto.IdCategoria;
                productoEditar.Stock = producto.Stock;
                productoEditar.Precio = producto.Precio;
                productoEditar.EsActivo = producto.EsActivo;

                if (productoEditar.NombreImagen == "")
                {
                    productoEditar.NombreImagen = NombreImagen;
                }

                if(imagen != null) {
                    string urlImagen = await _firebaseService.SubirStorage(imagen, "carpeta_producto", productoEditar.NombreImagen);
                    productoEditar.UrlImagen = urlImagen;    
                }

                bool res = await _repository.Editar(productoEditar);

                if (!res)
                {
                    throw new TaskCanceledException("No se pudo editar el producto");
                }

                Producto producto_editado = queryProducto.Include(c=>c.IdCategoriaNavigation).First();
                
                return producto_editado;
            }
            catch 
            {

                throw;
            }
        }

        public async Task<bool> Eliminar(int idProducto)
        {
            try
            {
                Producto producto_encontrado = await _repository.Obtener(p => p.IdProducto == idProducto);

                if (producto_encontrado == null)
                    throw new TaskCanceledException("El producto no existe");

                string nombreImagen = producto_encontrado.NombreImagen;
                bool res = await _repository.Eliminar(producto_encontrado);

                if (res)
                    await _firebaseService.EliminarStorage("carpeta_producto",nombreImagen);

                return true;
                
                
            }
            catch
            {

                throw;
            }
        }

    }
}
