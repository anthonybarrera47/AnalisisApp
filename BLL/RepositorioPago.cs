using DAL;
using Entidades;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

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

                foreach (var item in entity.DetallesPagos.ToList())
                {
                    if (item.DetallePagoID == 0)
                    {
                        RepositorioAnalisis repositorio = new RepositorioAnalisis();
                        Analisis Analisis = repositorio.Buscar(item.AnalisisID);
                        Analisis.Balance -= item.Monto;
                        repositorio.Modificar(Analisis);
                        repositorio.Dispose();
                        db.Entry(item).State = EntityState.Added;
                    }
                }
                foreach (var item in entity.DetallesPagos.ToList())
                {
                    if (item.DetallePagoID != 0)
                        db.Entry(item).State = EntityState.Modified;
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
                RepositorioBase<Pacientes> repositorio = new RepositorioBase<Pacientes>();
                Pagos = db.Pagos.Include(x => x.DetallesPagos)
                    .Where(x => x.PagosID == id)
                    .FirstOrDefault();
                if (Pagos != null)
                    Pagos.NombrePaciente = repositorio.Buscar(Pagos.PacienteID).Nombre;
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
            List<Pagos> Lista = new List<Pagos>();
            RepositorioBase<Pacientes> repositorio = new RepositorioBase<Pacientes>();
            Contexto db = new Contexto();
            try
            {
                Lista = db.Set<Pagos>().AsNoTracking().Where(expression).ToList();
                if (Lista.Count > 0)
                {
                    foreach (var item in Lista)
                    {
                        item.NombrePaciente = repositorio.Buscar(item.PacienteID).Nombre;
                        item.DetallesPagos.ForEach(x => item.TotalPagado += x.Monto);
                    }
                }
            }
            catch (Exception)
            { throw; }
            finally
            { db.Dispose(); }
            return Lista;
        }
        public string GetNombrePaciente(int id)
        {
            RepositorioBase<Pacientes> repositorio = new RepositorioBase<Pacientes>();
            Pacientes pacientes = repositorio.Buscar(id);
            return pacientes.Nombre;
        }
    }
}
