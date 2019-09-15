

using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class RepositorioBase<T> :IDisposable, IRepository<T> where T : class
    {
        internal Contexto _db;
        public RepositorioBase()
        {
            _db = new Contexto();
        }
        public virtual T Buscar(int id)
        {
            T entity;
            try
            {
                entity = _db.Set<T>().Find(id);
            }
            catch (Exception)
            { throw; }
            return entity;
        }

        public virtual void Dispose()
        {
            _db.Dispose();
        }

        public virtual bool Eliminar(int id)
        {
            bool paso = false;
            try
            {
                T entity = _db.Set<T>().Find(id); ;
                _db.Set<T>().Remove(entity);
                paso = _db.SaveChanges() > 0;
            }
            catch (Exception)
            { throw; }
            return paso;
        }
        public virtual List<T> GetList(Expression<Func<T, bool>> expression)
        {
            List<T> Lista = new List<T>();
            try
            {
                Lista = _db.Set<T>().AsNoTracking().Where(expression).ToList();
            }
            catch (Exception)
            { throw; }
            return Lista;
        }
        public virtual bool Guardar(T entity)
        {
            Contexto _db = new Contexto();
            bool paso = false;
            try
            {
                if (_db.Set<T>().Add(entity) != null)
                    paso = _db.SaveChanges() > 0;
            }
            catch (Exception)
            { throw; }
            return paso;
        }
        public virtual bool Modificar(T entity)
        {
            Contexto _db = new Contexto();
            bool paso = false;
            try
            {
                _db.Entry(entity).State = System.Data.Entity.EntityState.Modified;
                paso = (_db.SaveChanges() > 0);
            }
            catch (Exception)
            { throw; }
            return paso;
        }
    }
}
