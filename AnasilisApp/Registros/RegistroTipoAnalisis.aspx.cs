using BLL;
using Entidades;
using Extensores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AnasilisApp.Registros
{
    public partial class RegistroTipoAnalisis : System.Web.UI.Page
    {
        readonly string KeyViewState = "TipoAnalisis";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                FechaTextBox.Text = DateTime.Now.ToFormatDate();
                ViewState[KeyViewState] = new TipoAnalisis();
                int id = Request.QueryString["TipoAnalisisId"].ToInt();
                if (id > 0)
                {
                    var TipoAnalisis = new RepositorioBase<TipoAnalisis>().Buscar(id);
                    if (TipoAnalisis.EsNulo())
                        Extensores.Extensores.Alerta(this, TipoTitulo.Informacion, TiposMensajes.RegistroNoEncontrado, IconType.info);
                    else
                        LlenarCampos(TipoAnalisis);
                }
            }
        }
        private TipoAnalisis LlenaClase()
        {
            TipoAnalisis tipo = ViewState[KeyViewState].ToTipoAnalisis();
            tipo.TipoAnalisisID = TipoIdTextBox.Text.ToInt();
            tipo.Descripcion = DescripcionTextBox.Text;
            tipo.Monto = MontoTextBox.Text.ToDecimal();
            tipo.FechaRegistro = FechaTextBox.Text.ToDatetime();
            return tipo;
        }
        private void LlenarCampos(TipoAnalisis tipo)
        {
            TipoIdTextBox.Text = tipo.TipoAnalisisID.ToString();
            DescripcionTextBox.Text = tipo.Descripcion;
            MontoTextBox.Text = tipo.Monto.ToString();
            FechaTextBox.Text = tipo.FechaRegistro.ToFormatDate();
            ViewState[KeyViewState] = tipo;
        }
        private void Limpiar()
        {
            TipoIdTextBox.Text = "0";
            DescripcionTextBox.Text = string.Empty;
            MontoTextBox.Text = "0";
            FechaTextBox.Text = DateTime.Now.ToFormatDate();
            ViewState[KeyViewState] = new TipoAnalisis();
        }
        protected void NuevoButton_Click(object sender, EventArgs e)
        {
            Limpiar();
        }

        protected void GuadarButton_Click(object sender, EventArgs e)
        {
            if (!Validar())
                return;
            bool paso = false;
            RepositorioBase<TipoAnalisis> repositorio = new RepositorioBase<TipoAnalisis>();
            TipoTitulo tipoTitulo = TipoTitulo.OperacionFallida;
            TiposMensajes tiposMensajes = TiposMensajes.RegistroNoGuardado;
            IconType iconType = IconType.error;
            TipoAnalisis tipoAnalisis = LlenaClase();
            if (tipoAnalisis.TipoAnalisisID == 0)
                paso = repositorio.Guardar(tipoAnalisis);
            else
            {
                if (!ExisteEnLaBaseDeDatos())
                {
                    Extensores.Extensores.ToastSweet(this, IconType.info, TiposMensajes.RegistroNoEncontrado);
                    return;
                }
                paso = repositorio.Modificar(tipoAnalisis);
            }


            if (paso)
            {
                Limpiar();
                tipoTitulo = TipoTitulo.OperacionExitosa;
                tiposMensajes = TiposMensajes.RegistroGuardado;
                iconType = IconType.success;
            }
            Extensores.Extensores.Alerta(this, tipoTitulo, tiposMensajes, iconType);
        }
        protected void BuscarButton_Click(object sender, EventArgs e)
        {
            RepositorioBase<TipoAnalisis> repositorio = new RepositorioBase<TipoAnalisis>();
            TipoAnalisis tipoAnalisis = repositorio.Buscar(TipoIdTextBox.Text.ToInt());
            if (!tipoAnalisis.EsNulo())
            {
                Limpiar();
                LlenarCampos(tipoAnalisis);
            }
            else
                Extensores.Extensores.ToastSweet(this, IconType.info, TiposMensajes.RegistroNoEncontrado);
        }

        protected void EliminarButton_Click(object sender, EventArgs e)
        {
            RepositorioBase<TipoAnalisis> repositorio = new RepositorioBase<TipoAnalisis>();
            int id = TipoIdTextBox.Text.ToInt();
            if (!ExisteEnLaBaseDeDatos())
            {
                Extensores.Extensores.Alerta(this, TipoTitulo.OperacionFallida, TiposMensajes.RegistroInexistente, IconType.error);
                return;
            }
            else
            {
                if (repositorio.Eliminar(id))
                {
                    Extensores.Extensores.Alerta(this, TipoTitulo.OperacionExitosa, TiposMensajes.RegistroEliminado, IconType.success);
                    Limpiar();
                }
            }
        }
        private bool Validar()
        {
            bool paso = true;
            if (string.IsNullOrEmpty(DescripcionTextBox.Text))
                paso = false;
            if (string.IsNullOrEmpty(MontoTextBox.Text) || MontoTextBox.Text.ToDecimal() <= 0)
                paso = false;
            return paso;
        }
        private bool ExisteEnLaBaseDeDatos()
        {
            TipoAnalisis tipoAnalisis = new TipoAnalisis();
            RepositorioBase<TipoAnalisis> repositorio = new RepositorioBase<TipoAnalisis>();
            tipoAnalisis = repositorio.Buscar(TipoIdTextBox.Text.ToInt());
            return tipoAnalisis != null;
        }

    }
}