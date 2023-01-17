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
    public class MenuService : IMenuService
    {
        private readonly IGenericRepository<Menu> _repoMenu;
        private readonly IGenericRepository<RolMenu> _repoRolMenu;
        private readonly IGenericRepository<Usuario> _repoUsuario;

        public MenuService(IGenericRepository<Menu> repoMenu, IGenericRepository<RolMenu> repoRolMenu, IGenericRepository<Usuario> repoUsuario)
        {
            _repoMenu = repoMenu;
            _repoRolMenu = repoRolMenu;
            _repoUsuario = repoUsuario;
        }

        public async Task<List<Menu>> ObtenerMenu(int idUsuario)
        {
            IQueryable<Usuario> tbUsuario = await _repoUsuario.Consultar(u => u.IdUsuario == idUsuario);
            IQueryable<RolMenu> tbRolMenu = await _repoRolMenu.Consultar();
            IQueryable<Menu> tbMenu = await _repoMenu.Consultar();

            IQueryable<Menu> menuPadre = (from u in tbUsuario
                        join rm in tbRolMenu on u.IdRol equals rm.IdRol
                        join m in tbMenu on rm.IdMenu equals m.IdMenu
                        join mpadre in tbMenu on m.IdMenuPadre equals mpadre.IdMenuPadre
                        select mpadre).Distinct().AsQueryable();

            IQueryable<Menu> menusHijos = (from u in tbUsuario
                        join rm in tbRolMenu on u.IdRol equals rm.IdRol
                        join m in tbMenu on rm.IdMenu equals m.IdMenu
                        where m.IdMenu !=m.IdMenuPadre
                        select m).Distinct().AsQueryable();

            List<Menu> listMenu = (from mpadre in menuPadre
                                   select new Menu()
                                   {
                                       Descripcion = mpadre.Descripcion,
                                       Icono = mpadre.Icono,
                                       Controlador = mpadre.Controlador,
                                       PaginaAccion = mpadre.PaginaAccion,
                                       InverseIdMenuPadreNavigation = (from mhijo in menusHijos
                                                                       where mhijo.IdMenuPadre == mpadre.IdMenu
                                                                       select mhijo).ToList()

                                   }).ToList();
            return listMenu;
        }
    }
}
