using Microsoft.VisualStudio.TestTools.UnitTesting;
using BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entidades;
using DAL;

namespace BLL.Tests
{
    [TestClass()]
    public class RepositorioAnalisisTests
    {
        [TestMethod()]
        public void BuscarTest()
        {
            Analisis analisis = new RepositorioAnalisis().Buscar(2);
            Assert.AreEqual(true, !(analisis is null));
        }

        [TestMethod()]
        public void EliminarTest()
        {
            bool paso = new RepositorioAnalisis().Eliminar(2);
            Assert.IsTrue(paso);
        }

        [TestMethod()]
        public void GetListTest()
        {
            List<Analisis> lista = new RepositorioAnalisis().GetList(x => true);
            bool paso = (lista.Count > 0);
            Assert.IsTrue(paso);
        }

        [TestMethod()]
        public void GuardarTest()
        {
            Analisis analisis = new Analisis();
            analisis.PacienteID = 1;
            analisis.FechaRegistro = DateTime.Now;
            analisis.DetalleAnalisis.Add(new DetalleAnalisis(0, 0, 1, "Positivo"));
            Assert.IsTrue(new RepositorioAnalisis().Guardar(analisis));
        }

        [TestMethod()]
        public void ModificarTest()
        {
            Analisis analisis = new Analisis();
            RepositorioAnalisis repositorio = new RepositorioAnalisis();
            analisis = repositorio.Buscar(2);
            analisis.DetalleAnalisis.Add(new DetalleAnalisis(0, 2, 1, "Negativo"));
           //analisis.DetalleAnalisis.RemoveAt(0);
            
            bool paso = repositorio.Modificar(analisis);
            Assert.IsTrue(paso);
        }
    }
    //CODIGO PARA MOSTRAR LUEGO;
    //analisis = repositorio.Buscar(
    //    (from c in db.Analisis
    //     orderby c.AnalisisID descending
    //     select c.AnalisisID).First());
}