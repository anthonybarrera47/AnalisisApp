<%@ Page Title="Registro de Analisis"
    Language="C#"
    MasterPageFile="~/Site.Master"
    AutoEventWireup="true"
    CodeBehind="RegistroAnalisis.aspx.cs"
    Inherits="AnasilisApp.Registros.RegistroAnalisis" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <style>
        .modal-lg {
            max-width: 80% !important;
        }
    </style>
    <script></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container-fluid">
        <div class="card text-center bg-light">
            <div class="card-header"><%:Page.Title %></div>
            <div class="card-body text-center">
                <%--AnalisisID--%>
                <div class="input-group col-md-12">
                    <div class="input-group-prepend">
                        <span class="input-group-text">AnalisisID </span>
                    </div>
                    <asp:TextBox ID="AnalisisIdTextBox" TextMode="Number" MaxLength="9" runat="server" Text="0" class="form-control input-sm col-md-3"></asp:TextBox>
                    <asp:Button Text="Buscar" class="btn btn-info" runat="server" ID="BuscarButton" OnClick="BuscarButton_Click" />
                </div>
                <%--Fecha--%>
                <div class="input-group col-md-12">
                    <div class="input-group-prepend">
                        <span class="input-group-text" id="Fecha">Fecha </span>
                    </div>
                    <asp:TextBox ID="FechaTextBox" TextMode="Date" runat="server" class="form-control input-sm col-md-3" Visible="true"></asp:TextBox>
                </div>

                <%--Paciente--%>
                <div class="input-group col-md-12">
                    <div class="input-group-prepend">
                        <span class="input-group-text">Paciente </span>
                    </div>
                    <asp:DropDownList ID="PacientesDropdownList" CssClass=" form-control dropdown-item col-md-3" AppendDataBoundItems="true" runat="server" Height="2.5em">
                    </asp:DropDownList>
                    <%--AgregarPaciente--%>
                    <button type="button" class="btn btn-info" data-toggle="modal" data-target="#ModalPacientes" runat="server">Nuevo</button>
                </div>

                <%--TipoAnalisis--%>
                <div class="input-group col-md-12">
                    <div class="input-group-prepend">
                        <span class="input-group-text">Tipo de analisis </span>
                    </div>
                    <%--TipoAnalisisDropdonwList--%>
                    <asp:DropDownList ID="TipoAnalisisDropdonwList" CssClass=" form-control dropdown-item col-md-3" AppendDataBoundItems="true" runat="server" Height="2.5em">
                    </asp:DropDownList>
                    <%--AgregarAnalisis--%>
                    <button type="button" class="btn btn-info" data-toggle="modal" data-target="#myModal" runat="server">Nuevo</button>
                </div>

                <%--Resultados--%>
                <div class="input-group col-md-12">
                    <div class="input-group-prepend">
                        <span class="input-group-text" id="ResultadoAnalisis">Resultado </span>
                    </div>
                    <asp:TextBox ID="ResultadoAnalisisTextBox" runat="server" CssClass="form-control col-md-3"></asp:TextBox>

                    <%--AgregarDetalle--%>
                    <asp:Button Text="Agregar" class="btn btn-info" runat="server" ID="AgregarDetalleButton" OnClick="AgregarDetalleButton_Click" />
                </div>

                
                <%--GRID--%>
                <asp:ScriptManager ID="ScriptManger" runat="server"></asp:ScriptManager>
                <asp:UpdatePanel ID="UpdatePanel" runat="server">
                    <ContentTemplate>
                        <div class="row">
                            <div class="table table-responsive col-md-12">
                                <asp:GridView ID="DetalleGridView"
                                    runat="server"
                                    CssClass="table table-condensed table-bordered table-responsive"
                                    CellPadding="4" ForeColor="#333333" GridLines="None"
                                    OnPageIndexChanging="DetalleGridView_PageIndexChanging"
                                    AllowPaging="true" PageSize="5">
                                    <AlternatingRowStyle BackColor="LightBlue" />
                                    <Columns>
                                        <asp:TemplateField ShowHeader="False" HeaderText="Opciones">
                                            <ItemTemplate>
                                                <asp:Button ID="RemoverDetalleClick" runat="server" CausesValidation="false" CommandName="Select"
                                                    Text="Remover" class="btn btn-danger btn-sm" OnClick="RemoverDetalleClick_Click" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                    <RowStyle BackColor="#EFF3FB" />
                                </asp:GridView>
                            </div>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="DetalleGridView" />
                    </Triggers>
                </asp:UpdatePanel>

                <%--Monto--%>
                <div class="input-group col-md-12">
                    <div class="input-group-prepend">
                        <span class="input-group-text" id="MontoLB">Monto </span>
                    </div>
                    <asp:TextBox AutoPostBack="true" ID="MontoTextBox" ReadOnly="true" runat="server" class="form-control input-sm col-md-6"></asp:TextBox>

                    <%--Balance--%>
                    <div class="input-group-append">
                        <span class="input-group-text" id="BalanceLB">Balance </span>
                    </div>
                    <asp:TextBox ID="BalanceTextBox" ReadOnly="true" runat="server" class="form-control input-sm col-md-6"></asp:TextBox>

                </div>
            </div>
        </div>
    </div>
    <div class="panel-footer">
        <div class="text-center">
            <div class="form-group" display: inline-block>
                <asp:Button Text="Nuevo" class="btn btn-warning btn-lg" runat="server" ID="NuevoButton" OnClick="NuevoButton_Click" />
                <asp:Button Text="Guardar" class="btn btn-success btn-lg" runat="server" ID="GuadarButton" OnClick="GuadarButton_Click" />
                <asp:Button Text="Eliminar" class="btn btn-danger btn-lg" runat="server" ID="EliminarButton" OnClick="EliminarButton_Click" />
            </div>
        </div>
    </div>

    <!-- Modal para tipo Analisis -->
    <div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog ml-sm-auto" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="AgregarAnalisisLB">Agregar Analisis Rapido!!</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <%--Descripcion--%>
                    <div class="input-group mb-3">
                        <div class="input-group-prepend">

                            <span class="input-group-text" id="DescripcionLb">Descripción </span>
                        </div>
                        <div aria-describedby="DescripcionLb">
                            <asp:TextBox ID="DescripcionAnalisisTextBox" runat="server" class="form-control input-sm"></asp:TextBox>
                        </div>
                    </div>
                    <%--Precio--%>
                    <div class="input-group mb-3">
                        <div class="input-group-append">
                            <span class="input-group-text" id="PrecioLB">Precio </span>
                        </div>
                        <div aria-describedby="DescripcionLb">
                            <asp:TextBox ID="PrecioAnalisisTexBox" TextMode="Number" runat="server" class="form-control input-sm"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-dismiss="modal">Cerrar</button>
                    <asp:Button ID="AgregarAnaliss" class="btn btn-success" Text="Guardar" runat="server" data-dismiss="modal" UseSubmitBehavior="false" OnClick="AgregarAnaliss_Click" />
                </div>
            </div>
        </div>
    </div>
    <!-- Modal para Pacientes Analisis -->
    <div class="modal fade" id="ModalPacientes" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog ml-sm-auto" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="AgregarPacientesLB">Agregar Pacientes Rapido!!</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <%--NombrePacienteTextBox--%>
                    <div class="input-group mb-3">
                        <div class="input-group-prepend">
                            <span class="input-group-text" id="Nombre">Nombre </span>
                        </div>
                        <div aria-describedby="DescripcionLb">
                            <asp:TextBox ID="NombrePacienteTextBox" runat="server" class="form-control input-sm"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-dismiss="modal">Cerrar</button>
                    <asp:Button ID="AgregarPacientesButton" class="btn btn-success" Text="Guardar" runat="server" data-dismiss="modal" UseSubmitBehavior="false" OnClick="AgregarPacientesButton_Click" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
