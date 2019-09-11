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
        internal Contexto _db;
        public RepositorioAnalisis()
        {
            _db = new Contexto();
        }
        public override void Dispose()
        {
            _db.Dispose();
        }
        public override bool Modificar(Analisis entity)
        {
            bool paso = false;
            bool paso2 = false;
            var Anterior = Buscar(entity.AnalisisID);
            Contexto db = new Contexto();
            try
            {
                foreach (var item in Anterior.DetalleAnalisis.ToList())
                {
                    if (!entity.DetalleAnalisis.Exists(x => x.DetalleAnalisisID == item.DetalleAnalisisID))
                        db.Entry(item).State = EntityState.Deleted;
                }
                foreach (var item in entity.DetalleAnalisis.ToList())
                {
                    var estado = EntityState.Unchanged;
                    estado = item.DetalleAnalisisID > 0 ? EntityState.Modified : EntityState.Added;
                    _db.Entry(item).State = estado;
                }
                _db.Entry(entity).State = EntityState.Modified;
        
                paso2 = (db.SaveChanges() > 0);
                paso = (_db.SaveChanges() > 0);
            }
            catch (Exception)
            { throw; }
            finally
            { db.Dispose(); }
            return paso||paso2;
        }
        public override Analisis Buscar(int id)
        {
            //Contexto db = new Contexto();
            Analisis analisis = new Analisis();
            try
            {
                analisis = _db.Analisis.Include(x => x.DetalleAnalisis).Where(x => x.AnalisisID == id).FirstOrDefault();
                //analisis = db.Analisis.Find(id);
                //if (!(analisis is null))
                //    analisis.DetalleAnalisis.Count();
            }
            catch (Exception)
            { throw; }
            finally
            { _db.Dispose(); }
            _db = new Contexto();
            return analisis;
        }
        public override List<Analisis> GetList(Expression<Func<Analisis, bool>> expression)
        {
            List<Analisis> Lista = new List<Analisis>();
            try
            {
                Lista = _db.Set<Analisis>().AsNoTracking().Where(expression).ToList();
            }
            catch (Exception)
            { throw; }
            return Lista;
        }

    }
}
