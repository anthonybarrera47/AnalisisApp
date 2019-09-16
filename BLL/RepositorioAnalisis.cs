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
    public class RepositorioAnalisis : RepositorioBase<Analisis>
    {
        public RepositorioAnalisis() : base()
        {

        }
        public override bool Guardar(Analisis entity)
        {
            entity.Balance = entity.Monto;
            return base.Guardar(entity);
        }
        public override bool Modificar(Analisis entity)
        {
            bool paso = false;
            var Anterior = Buscar(entity.AnalisisID);
            Contexto db = new Contexto();
            try
            {
                using (Contexto contexto = new Contexto())
                {
                    foreach (var item in Anterior.DetalleAnalisis.ToList())
                    {
                        if (!entity.DetalleAnalisis.Exists(x => x.DetalleAnalisisID == item.DetalleAnalisisID))
                        {
                            entity.Balance -= new RepositorioBase<TipoAnalisis>().Buscar(item.TipoAnalisisID).Monto;
                            contexto.Entry(item).State = EntityState.Deleted;
                        }
                    }
                    if (!(contexto.SaveChanges() > 0))
                        return false;
                }
                foreach (var item in entity.DetalleAnalisis.ToList())
                {
                    var estado = EntityState.Unchanged;
                    if (item.DetalleAnalisisID == 0)
                    {
                        entity.Balance += db.TipoAnalisis.Find(item.TipoAnalisisID).Monto;
                        estado = EntityState.Added;
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
        public override Analisis Buscar(int id)
        {
            Analisis analisis = new Analisis();
            Contexto db = new Contexto();
            try
            {
                analisis = db.Analisis.AsNoTracking().Include(x => x.DetalleAnalisis).Where(x => x.AnalisisID == id).FirstOrDefault();
            }
            catch (Exception)
            { throw; }
            finally
            { db.Dispose(); }
            db = new Contexto();
            return analisis;
        }
        public override List<Analisis> GetList(Expression<Func<Analisis, bool>> expression)
        {
            List<Analisis> Lista = new List<Analisis>();
            Contexto db = new Contexto();
            try
            {
                Lista = db.Set<Analisis>().AsNoTracking().Where(expression).ToList();
            }
            catch (Exception)
            { throw; }
            finally
            { db.Dispose(); }
            return Lista;
        }

    }
}
