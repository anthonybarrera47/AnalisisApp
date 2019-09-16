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
            Contexto db = new Contexto();
            foreach (var item in entity.DetallesPagos.ToList())
            {
                var Analisis = repositorio.Buscar(item.AnalisisID);
                Analisis.Balance -= item.Monto;
                db.Entry(Analisis).State = System.Data.Entity.EntityState.Modified;
            }

            bool paso = db.SaveChanges() > 0;
            repositorio.Dispose();
            if (paso)
            {
                db.Dispose();
                return base.Guardar(entity);
            }
            db.Dispose();
            return false;
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
                            var Analisis = repositorio.Buscar(item.AnalisisID);
                            Analisis.Balance += item.Monto;
                            contexto.Entry(item).State = EntityState.Deleted;
                            contexto.Entry(Analisis).State = EntityState.Modified;
                            flag = true;
                            repositorio.Dispose();
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
                        var Analisis = repositorio.Buscar(item.AnalisisID);
                        Analisis.Balance -= item.Monto;
                        estado = EntityState.Added;
                        db.Entry(Analisis).State = EntityState.Modified;
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
            RepositorioAnalisis repositorio = new RepositorioAnalisis();
            Pagos pagos = Buscar(id);
            Contexto db = new Contexto();
            foreach (var item in pagos.DetallesPagos.ToList())
            {
                var Analisis = repositorio.Buscar(item.AnalisisID);
                Analisis.Monto += item.Monto;
                db.Entry(Analisis).State = System.Data.Entity.EntityState.Modified;
            }
            bool paso = db.SaveChanges() > 0;
            repositorio.Dispose();
            if (paso)
            {
                db.Dispose();
                return base.Eliminar(pagos.PagosID);
            }
            db.Dispose();
            return false;
        }
        public override List<Pagos> GetList(Expression<Func<Pagos, bool>> expression)
        {
            return base.GetList(expression);
        }
       
    }
}
