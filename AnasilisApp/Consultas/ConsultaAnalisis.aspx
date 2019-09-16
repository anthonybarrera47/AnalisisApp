<%@ Page
    Title="Consulta de Analisis"
    Language="C#"
    MasterPageFile="~/Site.Master"
    AutoEventWireup="true"
    CodeBehind="ConsultaAnalisis.aspx.cs"
    Inherits="AnasilisApp.Consultas.ConsultaAnalisis" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <script type="text/javascript">
        function ShowPopup(title, body) {
            $("#ModalDetalle .modal-title").html(title);
            $("#ModalDetalle").modal("show");
        }
    </script>

    <div class="container-fluid">
        <div class="card text-center bg-light mb-3">
            <div class="card-header"><%:Page.Title %></div>
            <div class="card-body align-items-center">
                <div class="form-horizontal col-md-12" role="form">
                    <div>
                        <div class="input-group mb-6">
                            <div class="input-group-prepend">
                                <span class="input-group-text" id="FiltroDropDownList">Filtro </span>
                            </div>
                            <div class="input-group-prepend col-md-4" aria-describedby="FiltroDropDownList">
                                <asp:DropDownList ID="BuscarPorDropDownList" runat="server" CssClass="form-control input-sm">
                                    <asp:ListItem>Todos</asp:ListItem>
                                    <asp:ListItem>AnalisisID</asp:ListItem>
                                    <asp:ListItem>PacienteID</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="input-group-prepend">
                                <span class="input-group-text" id="CriterioLB">Criterio </span>
                            </div>
                            <div class="input-group-append" aria-describedby="CriterioLB">
                                <asp:TextBox ID="FiltroTextBox" runat="server" CssClass="form-control col-md-"></asp:TextBox>
                            </div>
                            <div class="input-group-append" aria-describedby="FiltroTextBox">
                                <asp:Button ID="BuscarButton" runat="server" Class="btn btn-success input-sm" Text="Buscar" OnClick="BuscarButton_Click" />
                            </div>
                        </div>
                    </div>
                    <%--CheckBox--%>
                    <div class="input-group mb-3">
                        <div class="input-group-prepend">
                            <asp:CheckBox AutoPostBack="true" Checked="true" OnCheckedChanged="FechaCheckBox_CheckedChanged" ID="FechaCheckBox" runat="server" Text="Filtrar por fecha" />
                        </div>
                    </div>
                    <%--FechaDesde--%>
                    <div class="input-group mb-3">
                        <div class="input-group-prepend">
                            <span class="input-group-text" id="FechaDesde">Desde </span>
                        </div>
                        <div class="input-group-append" aria-describedby="FechaDesdeTextBox">
                            <asp:TextBox ID="FechaDesdeTextBox" TextMode="Date" runat="server" class="form-control input-sm" Visible="true"></asp:TextBox>
                        </div>
                        <div class="input-group-prepend">
                            <span class="input-group-text" id="FechaHasta">Hasta </span>
                        </div>
                        <div class="input-group-append" aria-describedby="FechaHastaTextBox">
                            <asp:TextBox ID="FechaHastaTextBox" TextMode="Date" runat="server" class="form-control input-sm" Visible="true"></asp:TextBox>
                        </div>
                    </div>
                    <%--GRID--%>
                    <asp:ScriptManager runat="server" ID="ScriptManager"></asp:ScriptManager>
                    <asp:UpdatePanel ID="UpdatePanel" runat="server">
                        <ContentTemplate>
                            <div class="row">
                                <div class="table table-responsive">
                                    <asp:GridView ID="DatosGridView"
                                        runat="server"
                                        CssClass="table table-condensed table-bordered table-responsive"
                                        CellPadding="4" ForeColor="#333333"
                                        OnPageIndexChanging="DatosGridView_PageIndexChanging"
                                        AllowPaging="true" PageSize="6"
                                        GridLines="None" AutoGenerateColumns="false">

                                        <AlternatingRowStyle BackColor="LightBlue" />

                                        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                        <RowStyle BackColor="#EFF3FB" />
                                        <Columns>
                                            <asp:HyperLinkField ControlStyle-CssClass="btn btn-info"
                                                DataNavigateUrlFields="AnalisisID"
                                                DataNavigateUrlFormatString="~/Registros/RegistroAnalisis.aspx?AnalisisID={0}"
                                                Text="Editar"></asp:HyperLinkField>
                                            <asp:TemplateField ShowHeader="False" HeaderText="Opciones">
                                                <ItemTemplate>
                                                    <asp:Button ID="VerDetalleButton" runat="server" CausesValidation="false" CommandName="Select"
                                                        Text="Ver Detalle" class="btn btn-danger" OnClick="VerDetalleButton_Click" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderText="AnalisisID" DataField="AnalisisID"/>
                                            <asp:BoundField HeaderText="PacienteID" DataField="PacienteID"/>
                                            <asp:BoundField HeaderText="Paciente" DataField="Paciente"/>
                                            <asp:BoundField HeaderText="Balance" DataField="Balance"/>
                                            <asp:BoundField HeaderText="Monto" DataField="Monto"/>
                                            <asp:BoundField HeaderText="Fecha de Registro" DataField="FechaRegistro"/>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="DatosGridView" />
                        </Triggers>
                    </asp:UpdatePanel>

                    <div class="input-group mb-2">
                        <div class="input-group-prepend">
                            <span class="input-group-text" id="Cantidad">Cantidad </span>
                        </div>
                        <div aria-describedby="Cantidad">
                            <asp:TextBox ID="CAntidadTextBox" TextMode="Number" ReadOnly="true" MaxLength="9" runat="server" Text="0" class="form-control input-sm"></asp:TextBox>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="modal fade" id="ModalDetalle" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog ml-sm-auto" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="AgregarPacientesLB">Agregar Pacientes Rapido!!</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <%--GRID--%>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <div class="row">
                                <div class="table table-responsive">
                                    <asp:GridView ID="DetalleDatosGridView"
                                        runat="server"
                                        CssClass="table table-condensed table-bordered table-responsive"
                                        CellPadding="4" ForeColor="#333333"
                                        OnPageIndexChanging="DatosGridView_PageIndexChanging"
                                        AllowPaging="true" PageSize="6"
                                        GridLines="None" AutoGenerateColumns="false">
                                        <Columns>
                                            <asp:BoundField HeaderText="Detalle ID" DataField="DetalleAnalisisID"/>
                                            <asp:BoundField HeaderText="AnalisisID" DataField="AnalisisID"/>
                                            <asp:BoundField HeaderText="TipoAnalisisID" DataField="TipoAnalisisID"/>
                                            <asp:BoundField HeaderText="Tipo de analisis" DataField="TipoAnalisis"/>
                                            <asp:BoundField HeaderText="Precio" DataField="Precio"/>
                                            <asp:BoundField HeaderText="Resultado" DataField="Resultado"/>
                                        </Columns>
                                        <AlternatingRowStyle BackColor="LightBlue" />

                                        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                        <RowStyle BackColor="#EFF3FB" />
                                    </asp:GridView>
                                </div>
                            </div>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="DatosGridView" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-dismiss="modal">Cerrar</button>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
