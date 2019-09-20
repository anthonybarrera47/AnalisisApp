using DAL;
using Entidades;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class RepositorioPago : RepositorioBase<Pagos>
    {
        public override bool Guardar(Pagos entity)
        {
            RepositorioAnalisis repositorio = new RepositorioAnalisis();
            bool paso = false;
            foreach (var item in entity.DetallesPagos.ToList())
            {
                var Analisis = repositorio.Buscar(item.AnalisisID);
                Analisis.Balance -= item.Monto;
                paso = repositorio.Modificar(Analisis);
            }
            repositorio.Dispose();
            if (paso)
                return base.Guardar(entity);
            return paso;
        }
        public override bool Modificar(Pagos entity)
        {
            bool paso = false;
            var Anterior = Buscar(entity.PagosID);
            Contexto db = new Contexto();
            try
            {
                using (Contexto contexto = new Contexto())
                {
                    bool flag = false;
                    foreach (var item in Anterior.DetallesPagos.ToList())
                    {
                        if (!entity.DetallesPagos.Exists(x => x.DetallePagoID == item.DetallePagoID))
                        {
                            RepositorioAnalisis repositorio = new RepositorioAnalisis();
                            Analisis Analisis = repositorio.Buscar(item.AnalisisID);
                            Analisis.Balance += item.Monto;

                            contexto.Entry(item).State = EntityState.Deleted;
                            repositorio.Modificar(Analisis);
                            repositorio.Dispose();
                            flag = true;
                        }
                    }

                    if (flag)
                        contexto.SaveChanges();
                    contexto.Dispose();
                }

                foreach (var item in entity.DetallesPagos)
                {
                    var estado = EntityState.Unchanged;
                    if (item.DetallePagoID == 0)
                    {
                        RepositorioAnalisis repositorio = new RepositorioAnalisis();
                        Analisis Analisis = repositorio.Buscar(item.AnalisisID);
                        Analisis.Balance -= item.Monto;
                        estado = EntityState.Added;
                        repositorio.Modificar(Analisis);
                        repositorio.Dispose();
                    }
                    db.Entry(item).State = estado;
                }
                db.Entry(entity).State = EntityState.Modified;
                paso = (db.SaveChanges() > 0);
            }
            catch (Exception)
            { throw; }
            finally
            { db.Dispose(); }
            return paso;
        }
        public override Pagos Buscar(int id)
        {
            Pagos Pagos = new Pagos();
            Contexto db = new Contexto();
            try
            {
                Pagos = db.Pagos.Include(x => x.DetallesPagos)
                    .Where(x => x.PagosID == id)
                    .FirstOrDefault();
            }
            catch (Exception)
            { throw; }
            finally
            { db.Dispose(); }
            return Pagos;
        }
        public override bool Eliminar(int id)
        {
            Pagos pagos = Buscar(id);
            bool paso = false;
            RepositorioAnalisis repositorio = new RepositorioAnalisis();
            foreach (var item in pagos.DetallesPagos)
            {
                var Analisis = repositorio.Buscar(item.AnalisisID);
                Analisis.Balance += item.Monto;
                paso = repositorio.Modificar(Analisis);
            }
            if (paso)
                return base.Eliminar(pagos.PagosID);
            return paso;
        }
        public override List<Pagos> GetList(Expression<Func<Pagos, bool>> expression)
        {
            return base.GetList(expression);
        }

    }
}
