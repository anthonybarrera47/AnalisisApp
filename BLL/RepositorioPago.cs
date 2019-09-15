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
        public override Pagos Buscar(int id)
        {
            Pagos Pagos = new Pagos();
            Contexto db = new Contexto();
            try
            {
                Pagos = db.Pagos.Include(x=>x.DetallesPagos)
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
            foreach(var item in pagos.DetallesPagos.ToList())
            {
                var Analisis = repositorio.Buscar(item.AnalisisID);
                Analisis.Monto += item.Monto;
                db.Entry(Analisis).State = System.Data.Entity.EntityState.Modified;
            }
            bool paso = db.SaveChanges() > 0;
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
        public override bool Guardar(Pagos entity)
        {
            RepositorioAnalisis repositorio = new RepositorioAnalisis();
            Contexto db = new Contexto();
            foreach(var item in entity.DetallesPagos.ToList())
            {
                var Analisis = repositorio.Buscar(item.AnalisisID);
                Analisis.Monto += item.Monto;
                db.Entry(Analisis).State = System.Data.Entity.EntityState.Modified;
            }
            bool paso = db.SaveChanges() > 0;
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
            //bool paso = false;
            //RepositorioAnalisis repositorio = new RepositorioAnalisis();
            //Analisis Analisi = repositorio.Buscar(entity.AnalisisID);
            //repositorio.Dispose();
            //try
            //{
            //    foreach (var item in entity.DetallesPagos.ToList())
            //    {
            //        var estado = EntityState.Unchanged;
            //        if (item.DetallePagoID == 0)
            //        {
            //            Analisi.Balance -= item.Monto;
            //            estado = EntityState.Added;
            //            _db.Entry(Analisi).State = EntityState.Modified;
            //        }
            //        _db.Entry(item).State = estado;
            //    }
            //    _db.Entry(entity).State = EntityState.Modified;
            //    paso = (_db.SaveChanges() > 0);
            //}
            //catch(Exception)
            //{ throw; }
            //return paso;
            return false;
        }
    }
}
