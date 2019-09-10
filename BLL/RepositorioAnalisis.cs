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
        public override bool Modificar(Analisis entity)
        {
            bool paso = false;
            Analisis Anterior = Buscar(entity.AnalisisID);
            Contexto db = new Contexto();
            try
            {
                foreach (var item in Anterior.DetalleAnalisis.ToList())
                {
                    if (!entity.DetalleAnalisis.Exists(x => x.DetalleAnalisisID == item.DetalleAnalisisID))
                            db.Entry(item).State = EntityState.Deleted;
                }
                foreach (var item in entity.DetalleAnalisis)
                {
                    var estado = EntityState.Unchanged;
                    estado = item.DetalleAnalisisID > 0 ? EntityState.Modified : EntityState.Added;
                    db.Entry(item).State = estado;
                }
                db.Entry(entity).State = EntityState.Modified;
                paso = db.SaveChanges() > 0;
            }
            catch (Exception)
            { throw; }
            finally
            { db.Dispose(); }
            return paso;
        }
        public override Analisis Buscar(int id)
        {
            Contexto db = new Contexto();
            Analisis analisis = new Analisis();
            try
            {
                analisis = db.Analisis.AsNoTracking().Include(x => x.DetalleAnalisis).Where(x => x.AnalisisID == id).FirstOrDefault();
            }
            catch (Exception)
            { throw; }
            finally
            { db.Dispose(); }

            return analisis;
        }
        public override List<Analisis> GetList(Expression<Func<Analisis, bool>> expression)
        {
            Contexto db = new Contexto();
            List<Analisis> Lista = new List<Analisis>();
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
