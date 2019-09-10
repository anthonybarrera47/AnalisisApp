using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class RepositorioBase<T> : IRepository<T> where T : class
    {
        //internal Contexto _db;
        //public RepositorioBase()
        //{
        //    _db = new Contexto();
        //}
        public RepositorioBase()
        {

        }
        public virtual T Buscar(int id)
        {
            Contexto db = new Contexto();
            T entity;
            try
            {
                entity = db.Set<T>().Find(id);
            }catch(Exception)
            { throw; }
            finally
            { db.Dispose(); }
            return entity;
        }
        public virtual bool Eliminar(int id)
        {
            Contexto db = new Contexto();
            bool paso = false;
            try
            {
                T entity = db.Set<T>().Find(id); ;
                db.Set<T>().Remove(entity);
                paso = db.SaveChanges() > 0;
            }catch(Exception)
            {throw;}
            finally
            { db.Dispose(); }
            return paso;
        }
        public virtual List<T> GetList(Expression<Func<T, bool>> expression)
        {
            Contexto db = new Contexto();
            List<T> Lista = new List<T>();
            try
            {
                Lista = db.Set<T>().AsNoTracking().Where(expression).ToList();
            }catch(Exception)
            { throw; }
            finally
            { db.Dispose(); }
            return Lista;
        }
        public virtual bool Guardar(T entity)
        {
            Contexto db = new Contexto();
            bool paso = false;
            try
            {
                if (db.Set<T>().Add(entity) != null)
                    paso = db.SaveChanges() > 0;
            }
            catch(Exception)
            {throw;}
            finally
            { db.Dispose(); }
            return paso;
        }
        public virtual bool Modificar(T entity)
        {
            Contexto db = new Contexto();
            bool paso = false;
            try
            {
                db.Entry(entity).State = System.Data.Entity.EntityState.Modified;
                if (db.SaveChanges() > 0)
                    paso = true;

            }
            catch(Exception)
            {throw; }
            finally
            { db.Dispose(); }
            return paso;
        }
    }
}
