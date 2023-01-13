using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SistemaVenta.BLL.Interfaces;
using SistemaVenta.DAL.Interfaces;
using SistemaVenta.Entity;

namespace SistemaVenta.BLL.Implementacion
{
    public class NegocioService : INegocioService
    {

        private readonly IGenericRepository<Negocio> _repository;
        private readonly IFireBaseService _firebaseService;

        public NegocioService(
            IGenericRepository<Negocio> repository,
            IFireBaseService firebaseService)
        {
            _firebaseService = firebaseService;
            _repository = repository;
        }

        public async Task<Negocio> Obtener()
        {
            try
            {
                Negocio negocio_encontrado = await _repository.Obtener(n => n.IdNegocio == 1);
                return negocio_encontrado;
            }
            catch
            {

                throw;
            }
        }

               
        public async Task<Negocio> GuardarCambios(Negocio negocio, Stream logo = null, string NombreLogo = "")
        {
            try
            {
                Negocio negocio_encontrado = await _repository.Obtener(n => n.IdNegocio == 1);

                negocio_encontrado.NumeroDocumento = negocio.NumeroDocumento;
                negocio_encontrado.Nombre = negocio.Nombre;
                negocio_encontrado.Correo = negocio.Correo;
                negocio_encontrado.Direccion = negocio.Direccion;
                negocio_encontrado.Telefono = negocio.Telefono;
                negocio_encontrado.PorcentajeImpuesto = negocio.PorcentajeImpuesto;
                negocio_encontrado.SimboloMoneda = negocio.SimboloMoneda;

                negocio_encontrado.NombreLogo = negocio_encontrado.NombreLogo == "" ? NombreLogo : negocio_encontrado.NombreLogo;

                if (logo != null)
                {
                    string urllogo = await _firebaseService.SubirStorage(logo, "carpeta_logo", negocio_encontrado.NombreLogo);
                    negocio_encontrado.UrlLogo = urllogo;
                }

                await _repository.Editar(negocio_encontrado);
                return negocio_encontrado;

            }
            catch
            {

                throw;
            }
        }

    }
}
