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
            RepositorioAnalisis repositorio = new RepositorioAnalisis();
            Analisis analisis = repositorio.Buscar(2);
            repositorio.Dispose();
            Assert.AreEqual(true, !(analisis is null));
        }

        [TestMethod()]
        public void EliminarTest()
        {
            RepositorioAnalisis repositorio = new RepositorioAnalisis();
            bool paso = repositorio.Eliminar(2);
            repositorio.Dispose();
            Assert.IsTrue(paso);
        }

        [TestMethod()]
        public void GetListTest()
        {
            RepositorioAnalisis repositorio = new RepositorioAnalisis();
            List<Analisis> lista = repositorio.GetList(x => true);
            repositorio.Dispose();
            bool paso = (lista.Count > 0);
            Assert.IsTrue(paso);
        }

        [TestMethod()]
        public void GuardarTest()
        {
            Analisis analisis = new Analisis
            {
                PacienteID = 1,
                FechaRegistro = DateTime.Now
            };
            analisis.DetalleAnalisis.Add(new DetalleAnalisis(0, 0, 1, "Positivo"));
            RepositorioAnalisis repositorio = new RepositorioAnalisis();
            bool paso = repositorio.Guardar(analisis);
            repositorio.Dispose();
            Assert.IsTrue(paso);
        }

        [TestMethod()]
        public void ModificarTest()
        {
            RepositorioAnalisis repositorio = new RepositorioAnalisis();
            Analisis analisis = repositorio.Buscar(2);
            analisis.DetalleAnalisis.Add(new DetalleAnalisis(0, 2, 1, "Negativo"));
            bool paso = repositorio.Modificar(analisis);
            repositorio.Dispose();
            Assert.IsTrue(paso);
        }
    }
    //CODIGO PARA MOSTRAR LUEGO;
    //analisis = repositorio.Buscar(
    //    (from c in db.Analisis
    //     orderby c.AnalisisID descending
    //     select c.AnalisisID).First());
}