using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SistemaVenta.BLL.Interfaces;
using Firebase.Auth;
using Firebase.Storage;
using SistemaVenta.Entity;
using SistemaVenta.DAL.Interfaces;


namespace SistemaVenta.BLL.Implementacion
{
    public class FireBaseService : IFireBaseService
    {

        private readonly IGenericRepository<Configuracion> _repository;

        public FireBaseService(IGenericRepository<Configuracion> repository)
        {
            _repository = repository;
        }

        public async Task<string> SubirStorage(Stream StreamArchivo, string CapertaDestino, string NombreArchivo)
        {
            string UrlImagen = "";

            try
            {
                IQueryable<Configuracion> query = await _repository.Consultar(c => c.Recurso.Equals("FireBase_Storage"));

                Dictionary<string, string> Config = query.ToDictionary(keySelector: c => c.Propiedad, elementSelector: c => c.Valor);

                var auth = new FirebaseAuthProvider(new FirebaseConfig(Config["api_key"]));
                var a = await auth.SignInWithEmailAndPasswordAsync(Config["email"], Config["clave"]);

                var cancelarToken = new CancellationTokenSource();

                var task = new FirebaseStorage(
                    Config["ruta"],
                    new FirebaseStorageOptions
                    {
                        AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
                        ThrowOnCancel = true
                    })
                    .Child(Config[CapertaDestino])
                    .Child(NombreArchivo)
                    .PutAsync(StreamArchivo, cancelarToken.Token);

                UrlImagen = await task;
            }
            catch (Exception)
            {

                UrlImagen = "";
            }

            return UrlImagen;
        }
        
                
        public async Task<bool> EliminarStorage(string CapertaDestino, string NombreArchivo)
        {
            try
            {
                IQueryable<Configuracion> query = await _repository.Consultar(c => c.Recurso.Equals("FireBase_Storage"));

                Dictionary<string, string> Config = query.ToDictionary(keySelector: c => c.Propiedad, elementSelector: c => c.Valor);

                var auth = new FirebaseAuthProvider(new FirebaseConfig(Config["api_key"]));
                var a = await auth.SignInWithEmailAndPasswordAsync(Config["email"], Config["clave"]);

                var cancelarToken = new CancellationTokenSource();

                var task = new FirebaseStorage(
                    Config["ruta"],
                    new FirebaseStorageOptions
                    {
                        AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
                        ThrowOnCancel = true
                    })
                    .Child(Config[CapertaDestino])
                    .Child(NombreArchivo)
                    .DeleteAsync();

                await task;
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

    }
}
