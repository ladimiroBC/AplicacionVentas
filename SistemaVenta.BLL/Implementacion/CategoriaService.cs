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
    public class CategoriaService : ICategoriaService
    {   
        private readonly IGenericRepository<Categoria> _repository;

        public CategoriaService(IGenericRepository<Categoria> repository)
        {
            _repository = repository;
        }

        public async Task<List<Categoria>> Lista()
        {
            IQueryable<Categoria> query = await _repository.Consultar();
            return query.ToList();
        }

        public async Task<Categoria> Crear(Categoria categoria)
        {
            try
            {
                Categoria categoria_creada = await _repository.Crear(categoria);
                if (categoria_creada.IdCategoria == 0)
                {
                    throw new TaskCanceledException("No se pudo crear la categoria");
                }

                return categoria_creada;
            }
            catch
            {

                throw;
            }

            
        }

        public async Task<Categoria> Editar(Categoria categoria)
        {
            try
            {
                Categoria categoria_encontrada = await _repository.Obtener(c=>c.IdCategoria == categoria.IdCategoria);
                categoria_encontrada.Descripcion = categoria.Descripcion;
                categoria_encontrada.EsActivo = categoria.EsActivo;
                bool res = await _repository.Editar(categoria_encontrada);

                if (!res)
                    throw new TaskCanceledException("No se pudo modificar la categoria");

                return categoria_encontrada;
                               
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> Eliminar(int idCategoria)
        {
            try
            {
                Categoria categoria_encontrada = await _repository.Obtener(c => c.IdCategoria == idCategoria);

                if (categoria_encontrada == null)
                    throw new TaskCanceledException("La categoria no existe");

                bool res = await _repository.Eliminar(categoria_encontrada);

                return res;
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
