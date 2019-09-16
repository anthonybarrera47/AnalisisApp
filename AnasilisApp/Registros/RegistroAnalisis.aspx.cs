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
    public partial class RegistroAnalisis : System.Web.UI.Page
    {
        readonly string KeyViewState = "Analisis";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                FechaTextBox.Text = DateTime.Now.ToFormatDate();
                ViewState[KeyViewState] = new Analisis();
                LlenarCombo();
                int id = Request.QueryString["AnalisisID"].ToInt();
                if (id > 0)
                {
                    var Analisis = new RepositorioAnalisis().Buscar(id);
                    if (Analisis.EsNulo())
                        Extensores.Extensores.Alerta(this, TipoTitulo.Informacion, TiposMensajes.RegistroNoEncontrado, IconType.info);
                    else
                        LlenarCampos(Analisis);
                }
            }
        }
        private void LlenarCombo()
        {
            TipoAnalisisDropdonwList.Items.Clear();
            PacientesDropdownList.Items.Clear();
            RepositorioBase<TipoAnalisis> repositorio = new RepositorioBase<TipoAnalisis>();
            TipoAnalisisDropdonwList.DataSource = repositorio.GetList(x => true);
            TipoAnalisisDropdonwList.DataValueField = "TipoAnalisisID";
            TipoAnalisisDropdonwList.DataTextField = "Descripcion";
            TipoAnalisisDropdonwList.DataBind();

            RepositorioBase<Pacientes> repositorioPacientes = new RepositorioBase<Pacientes>();
            PacientesDropdownList.DataSource = repositorioPacientes.GetList(x => true);
            PacientesDropdownList.DataValueField = "PacienteID";
            PacientesDropdownList.DataTextField = "Nombre";
            PacientesDropdownList.DataBind();
        }
        protected void BindGrid()
        {
            Analisis analisis = ViewState[KeyViewState].ToAnalisis();
            DetalleGridView.DataSource = analisis.DetalleAnalisis;
            DetalleGridView.DataBind();
        }
        public Analisis LLenaClase()
        {
            Analisis Analisis = ViewState[KeyViewState].ToAnalisis();
            Analisis.AnalisisID = AnalisisIdTextBox.Text.ToInt();
            Analisis.PacienteID = TipoAnalisisDropdonwList.SelectedValue.ToInt();
            Analisis.FechaRegistro = FechaTextBox.Text.ToDatetime();
            Analisis.Monto = 0;
            Analisis.DetalleAnalisis.ForEach(x => Analisis.Monto += new RepositorioBase<TipoAnalisis>().Buscar(x.TipoAnalisisID).Monto);
            return Analisis;
        }
        public void LlenarCampos(Analisis analisis)
        {
            Limpiar();
            AnalisisIdTextBox.Text = analisis.AnalisisID.ToString();
            PacientesDropdownList.SelectedValue = analisis.PacienteID.ToString();
            FechaTextBox.Text = analisis.FechaRegistro.ToFormatDate();
            MontoTextBox.Text = analisis.Monto.ToString();
            BalanceTextBox.Text = analisis.Balance.ToString();
            ViewState[KeyViewState] = analisis;
            Calcular();
            this.BindGrid();
        }
        private void Limpiar()
        {
            AnalisisIdTextBox.Text = "0";
            PacientesDropdownList.ClearSelection();
            TipoAnalisisDropdonwList.ClearSelection();
            ResultadoAnalisisTextBox.Text = string.Empty;
            FechaTextBox.Text = DateTime.Now.ToFormatDate();
            MontoTextBox.Text = 0.ToString();
            BalanceTextBox.Text = 0.ToString();
            ViewState[KeyViewState] = new Analisis();
            this.BindGrid();
        }
        protected void AgregarAnaliss_Click(object sender, EventArgs e)
        {
            RepositorioBase<TipoAnalisis> repositorio = new RepositorioBase<TipoAnalisis>();

            if (!string.IsNullOrEmpty(DescripcionAnalisisTextBox.Text) && PrecioAnalisisTexBox.Text.ToDecimal() > 0)
                repositorio.Guardar(new TipoAnalisis(0, DescripcionAnalisisTextBox.Text, PrecioAnalisisTexBox.Text.ToDecimal(), DateTime.Now.ToDatetime()));
            
            LlenarCombo();
        }
        protected void AgregarPacientesButton_Click(object sender, EventArgs e)
        {
            RepositorioBase<Pacientes> repositorio = new RepositorioBase<Pacientes>();

            if (!string.IsNullOrEmpty(NombrePacienteTextBox.Text))
                repositorio.Guardar(new Pacientes(0, NombrePacienteTextBox.Text, DateTime.Now));
            else
                Extensores.Extensores.Alerta(this,TipoTitulo.OperacionFallida,TiposMensajes.RegistroNoGuardado,IconType.error);
            
            LlenarCombo();
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
            RepositorioAnalisis repositorio = new RepositorioAnalisis();
            Analisis analisis = LLenaClase();
            TipoTitulo tipoTitulo = TipoTitulo.OperacionFallida;
            TiposMensajes tiposMensajes = TiposMensajes.RegistroNoGuardado;
            IconType iconType = IconType.error;

            if (analisis.AnalisisID == 0)
                paso = repositorio.Guardar(analisis);
            else
            {
                if (ExisteEnLaBaseDeDatos())
                {
                    Extensores.Extensores.ToastSweet(this, IconType.info, TiposMensajes.RegistroNoEncontrado);
                    return;
                }
                paso = repositorio.Modificar(analisis);
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
            RepositorioAnalisis repositorio = new RepositorioAnalisis();
            Analisis Analisis = repositorio.Buscar(AnalisisIdTextBox.Text.ToInt());
            if (!Analisis.EsNulo())
            {
                Limpiar();
                LlenarCampos(Analisis);
            }
            else
                Extensores.Extensores.ToastSweet(this, IconType.info, TiposMensajes.RegistroNoEncontrado);
        }
        protected void EliminarButton_Click(object sender, EventArgs e)
        {
            RepositorioBase<Analisis> repositorio = new RepositorioBase<Analisis>();
            int id = AnalisisIdTextBox.Text.ToInt();
            if (ExisteEnLaBaseDeDatos())
            {
                Extensores.Extensores.Alerta(this,TipoTitulo.OperacionFallida,TiposMensajes.RegistroInexistente,IconType.error);
                return;
            }
            else
            {
                if (repositorio.Eliminar(id))
                {
                    Extensores.Extensores.Alerta(this, TipoTitulo.OperacionExitosa,TiposMensajes.RegistroEliminado,IconType.success);
                    Limpiar();
                }
            }
        }
        protected void AgregarDetalleButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ResultadoAnalisisTextBox.Text))
                return;
            Analisis analisis = ViewState[KeyViewState].ToAnalisis();
            analisis.AgregarDetalle(0, analisis.AnalisisID, TipoAnalisisDropdonwList.SelectedValue.ToInt(), ResultadoAnalisisTextBox.Text);
            ViewState[KeyViewState] = analisis;
            this.BindGrid();
            Calcular();
            ResultadoAnalisisTextBox.Text = string.Empty;
        }
        protected void RemoverDetalleClick_Click(object sender, EventArgs e)
        {
            Analisis analisis = ViewState[KeyViewState].ToAnalisis();
            GridViewRow row = (sender as Button).NamingContainer as GridViewRow;
            analisis.RemoverDetalle(row.RowIndex);
            ViewState[KeyViewState] = analisis;
            this.BindGrid();
            Calcular();
        }
        public void Calcular()
        {
            decimal Monto = 0;
            Analisis analisis = ViewState[KeyViewState].ToAnalisis();
            foreach (var item in analisis.DetalleAnalisis.ToList())
            {
                TipoAnalisis tipo = new RepositorioBase<TipoAnalisis>().Buscar(item.TipoAnalisisID);
                Monto += tipo.EsNulo() ? 0 : tipo.Monto;
            }
            analisis.Monto = Monto;
            ViewState[KeyViewState] = analisis;
            this.BindGrid();
        }
        
        protected void DetalleGridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Analisis analisis = ViewState[KeyViewState].ToAnalisis();
            DetalleGridView.DataSource = analisis.DetalleAnalisis;
            DetalleGridView.PageIndex = e.NewPageIndex;
            DetalleGridView.DataBind();
        }
        private bool Validar()
        {
            bool paso = true;
            if (TipoAnalisisDropdonwList.Items.Count == 0)
                paso = false;
            if (PacientesDropdownList.Items.Count == 0)
                paso = false;
            if (DetalleGridView.Rows.Count == 0)
                paso = false;
            return paso;
        }
        private bool ExisteEnLaBaseDeDatos()
        {
            RepositorioBase<Analisis> repositorio = new RepositorioBase<Analisis>();
            Analisis analisis = repositorio.Buscar(AnalisisIdTextBox.Text.ToInt());
            repositorio.Dispose();
            return analisis.EsNulo();
        }
       
        
    }
}