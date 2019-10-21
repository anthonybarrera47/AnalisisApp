<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ConsultaPagos.aspx.cs" Inherits="AnasilisApp.Consultas.ConsultaPagos" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <style type="text/css">
        .bigModal {
            width: 1080px;
            height:800px;
            margin-left: -200px;
        }
    </style>
    <script type="text/javascript">
        function ShowPopup(title) {
            $("#ModalDetalle .modal-title").html(title);
            $("#ModalDetalle").modal("show");
        }
    </script>
    <script type="text/javascript">
        function ShowReporte(title) {
            $("#ModalReporte .modal-title").html(title);
            $("#ModalReporte").modal("show");
        }
    </script>

    <%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91"
        Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
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
                                <asp:Button ID="BuscarButton" runat="server" CssClass="btn btn-success input-sm" Text="Buscar" OnClick="BuscarButton_Click" />
                            </div>
                        </div>
                    </div>
                    <%--CheckBox--%>
                    <div class="form-row">
                        <div class="custom-control custom-checkbox mr-sm-2">
                            <asp:CheckBox CssClass="custom-checkbox" Checked="true" ID="FechaCheckBox" runat="server" Text="Filtrar por fecha" />
                        </div>
                    </div>
                    <%--FechaDesde--%>
                    <div class="input-group mb-3">
                        <div class="input-group-prepend">
                            <span class="input-group-text" id="FechaDesde">Desde </span>
                        </div>
                        <div class="input-group-append" aria-describedby="FechaDesdeTextBox">
                            <asp:TextBox ID="FechaDesdeTextBox" TextMode="Date" runat="server" CssClass="form-control input-sm" Visible="true"></asp:TextBox>
                        </div>
                        <div class="input-group-prepend">
                            <span class="input-group-text" id="FechaHasta">Hasta </span>
                        </div>
                        <div class="input-group-append" aria-describedby="FechaHastaTextBox">
                            <asp:TextBox ID="FechaHastaTextBox" TextMode="Date" runat="server" CssClass="form-control input-sm" Visible="true"></asp:TextBox>
                        </div>
                    </div>
                    <%--GRID--%>
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
                                                DataNavigateUrlFields="PagosID"
                                                DataNavigateUrlFormatString="~/Registros/RegistroPagos.aspx?PagosID={0}"
                                                Text="Editar"></asp:HyperLinkField>
                                            <asp:TemplateField ShowHeader="False" HeaderText="Opciones">
                                                <ItemTemplate>
                                                    <asp:Button ID="VerDetalleButton" runat="server" CausesValidation="false" CommandName="Select"
                                                        Text="Ver Detalle" CssClass="btn btn-danger" OnClick="VerDetalleButton_Click" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderText="PagosID" DataField="PagosID" />
                                            <asp:BoundField HeaderText="Fecha" DataField="FechaRegistro" DataFormatString="{0:dd-MM-yyyy}" />
                                            <asp:BoundField HeaderText="PacienteID" DataField="PacienteID" Visible="false" />
                                            <asp:BoundField HeaderText="Paciente" DataField="NombrePaciente" />
                                            <asp:BoundField HeaderText="Total Pagado" DataField="TotalPagado" />
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="DatosGridView" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="card-footer">
                <asp:Button ID="ImprimirButton" runat="server" CssClass="btn btn-success input-sm" Text="Imprimir" OnClick="ImprimirButton_Click" />
            </div>
        </div>
    </div>
    <div class="modal fade" id="ModalDetalle" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog ml-sm-auto" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="AgregarPacientesLB"></h5>
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
                                            <asp:BoundField HeaderText="Detalle ID" DataField="DetallePagoID" Visible="false" />
                                            <asp:BoundField HeaderText="PagosID" DataField="PagosID" Visible="false" />
                                            <asp:BoundField HeaderText="AnalisisID" DataField="AnalisisID" />
                                            <asp:BoundField HeaderText="Monto" DataField="Monto" />
                                            <asp:BoundField HeaderText="Estado" DataField="Estado" />
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
    <%-- El  Modal para el Reporte--%>
    <div class="modal fade" id="ModalReporte" tabindex="-1" role="dialog" aria-hidden="true">
        <div class="modal-dialog modal-lg" role="document">
            <div class="modal-content bigModal">
                <div class="modal-header">
                    <h5 class="modal-title" id="ModalLebel">Reporte de Analisis</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <%--Viewer--%>
                    <rsweb:ReportViewer ID="PagosReportViewer" runat="server" ProcessingMode="Remote" Height="100%" Width="100%">
                    </rsweb:ReportViewer>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
