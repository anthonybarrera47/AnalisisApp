﻿using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BLL;
using Entidades;
using Extensores;

namespace BLLTests
{
    /// <summary>
    /// Summary description for RepositorioPagosTest
    /// </summary>
    [TestClass]
    public class RepositorioPagosTest
    {
        [TestMethod]
        public void GuardaPagos()
        {
            RepositorioPago repositorio = new RepositorioPago();
            Pagos pagos = new Pagos
            {
                FechaRegistro = DateTime.Now
            };
            pagos.AgregarDetalle(0, 0, 1, 200);
            Assert.IsTrue(repositorio.Guardar(pagos));
        }
        [TestMethod]
        public void ModificarPagos()
        {
            RepositorioPago repositorio = new RepositorioPago();
            Pagos pagos = repositorio.Buscar(1);
            pagos.FechaRegistro = DateTime.Now;
            pagos.AgregarDetalle(0, pagos.PagosID, 1, 200);
            Assert.IsTrue(repositorio.Modificar(pagos));
        }
        [TestMethod]
        public void BuscarPagos()
        {
            RepositorioPago repositorio = new RepositorioPago();
            Pagos pagos = repositorio.Buscar(2);
            Assert.IsTrue(!pagos.EsNulo());
        }
        [TestMethod]
        public void GetListPagos()
        {
            RepositorioPago repositorio = new RepositorioPago();
            Assert.IsTrue((repositorio.GetList(x=>true).Count>0));
        }
        [TestMethod]
        public void EliminarPagos()
        {
            RepositorioPago repositorio = new RepositorioPago();
            Assert.IsTrue(repositorio.Eliminar(2));
        }
    }
}
