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
            bool paso2 = false;
            var Anterior = Buscar(entity.AnalisisID);
            Contexto db = new Contexto();
            try
            {
                foreach (var item in Anterior.DetalleAnalisis.ToList())
                {
                    if (!entity.DetalleAnalisis.Exists(x => x.DetalleAnalisisID == item.DetalleAnalisisID))
                    {
                        entity.Balance -= new RepositorioBase<TipoAnalisis>().Buscar(item.TipoAnalisisID).Monto;
                        db.Entry(item).State = EntityState.Deleted;
                    }
                }
                foreach (var item in entity.DetalleAnalisis.ToList())
                {
                    var estado = EntityState.Unchanged;
                    if (item.DetalleAnalisisID==0)
                    {
                        entity.Balance += db.TipoAnalisis.Find(item.TipoAnalisisID).Monto;
                        estado = EntityState.Added;
                    }
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
            return paso || paso2;
        }
        public override Analisis Buscar(int id)
        {
            Analisis analisis = new Analisis();
            try
            {
                analisis = _db.Analisis.Include(x => x.DetalleAnalisis).Where(x => x.AnalisisID == id).FirstOrDefault();
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
