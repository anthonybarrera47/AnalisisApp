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
            RepositorioBase<Pacientes> repositorio = new RepositorioBase<Pacientes>();
            Pacientes paciente = repositorio.Buscar(entity.PacienteID);
            paciente.Balance += entity.Balance;
            repositorio.Dispose();
            RepositorioBase<Pacientes> RepositorioModificar = new RepositorioBase<Pacientes>();
            RepositorioModificar.Modificar(paciente);
            RepositorioModificar.Dispose();
            return base.Guardar(entity);
        }
        public override bool Modificar(Analisis entity)
        {
            bool paso = false;
            Analisis Anterior = Buscar(entity.AnalisisID);
            RepositorioBase<Pacientes> repositorioPaciente = new RepositorioBase<Pacientes>();
            Pacientes Paciente = repositorioPaciente.Buscar(entity.PacienteID);
            Paciente.Balance -= Anterior.Balance;
            Contexto db = new Contexto();
            try
            {
                using (Contexto contexto = new Contexto())
                {
                    foreach (var item in Anterior.DetalleAnalisis.ToList())
                    {
                        if (!entity.DetalleAnalisis.Exists(x => x.DetalleAnalisisID == item.DetalleAnalisisID))
                        {
                            RepositorioBase<TipoAnalisis> repositorioBase = new RepositorioBase<TipoAnalisis>();
                            TipoAnalisis TipoAnalisis = repositorioBase.Buscar(item.TipoAnalisisID);
                            entity.Balance -= TipoAnalisis.Monto;
                            contexto.Entry(item).State = EntityState.Deleted;
                            entity.DetalleAnalisis.Remove(item);
                            repositorioBase.Dispose();
                        }
                    }
                    contexto.SaveChanges();                        
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
                Paciente.Balance += entity.Balance;
                repositorioPaciente.Modificar(Paciente);
                db.Entry(entity).State = EntityState.Modified;
                paso = (db.SaveChanges() > 0);
            }
            catch (Exception)
            { throw; }
            finally
            { db.Dispose(); }

            return paso;
        }
        public override bool Eliminar(int id)
        {
            Analisis analisis = Buscar(id);
            RepositorioBase<Pacientes> repositorio = new RepositorioBase<Pacientes>();
            Pacientes paciente = repositorio.Buscar(analisis.PacienteID);
            paciente.Balance -= analisis.Balance;
            repositorio.Modificar(paciente);
            repositorio.Dispose();
            return base.Eliminar(id);
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
