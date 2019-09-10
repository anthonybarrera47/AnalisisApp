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
        <div class="card text-center bg-light mb-3">
            <div class="card-header"><%:Page.Title %></div>
            <div class="card-body">
                <div class="form-horizontal col-md-12" role="form">
                    <%--UsuarioID--%>
                    <div class="input-group mb-2">
                        <div class="input-group-prepend">
                            <span class="input-group-text" id="AnalisisID">AnalisisID </span>
                        </div>
                        <div aria-describedby="AnalisisID">
                            <asp:TextBox ID="AnalisisIdTextBox" TextMode="Number" MaxLength="9" runat="server" Text="0" class="form-control input-sm"></asp:TextBox>
                        </div>
                        <div class="input-group-append">
                            <asp:Button Text="Buscar" class="btn btn-info" runat="server" ID="BuscarButton" OnClick="BuscarButton_Click" />
                        </div>
                    </div>
                    <%--Paciente--%>
                    <div class="input-group mb-2">
                        <div class="input-group-prepend">
                            <span class="input-group-text" id="Paciente">Paciente </span>
                        </div>
                        <div aria-describedby="PacientesDropdownList">
                            <asp:DropDownList ID="PacientesDropdownList" CssClass=" form-control dropdown-item" AppendDataBoundItems="true" runat="server" Height="2.5em">
                            </asp:DropDownList>
                        </div>
                        <%--AgregarPaciente--%>
                        <div class="input-group-append">
                            <!-- Button trigger modal -->
                            <div class="col-md-4 input-group-append" aria-describedby="TipoAnalisisDropdonwList">
                                <button aria-describedby="TipoAnalisisDropdonwList" type="button" class="btn btn-info" data-toggle="modal" data-target="#ModalPacientes" runat="server">+</button>
                            </div>
                        </div>
                    </div>
                    <%--TipoAnalisis--%>
                    <div class="input-group mb-2">
                        <div class="input-group-prepend">
                            <span class="input-group-text" id="TipoAnalisis">Tipo de analisis </span>
                        </div>
                        <%--TipoAnalisisDropdonwList--%>
                        <div aria-describedby="TipoAnalisisDropdonwList">
                            <asp:DropDownList ID="TipoAnalisisDropdonwList" CssClass=" form-control dropdown-toggle-split" AppendDataBoundItems="true" runat="server" Height="2.5em">
                            </asp:DropDownList>
                        </div>
                        <%--AgregarAnalisis--%>
                        <div class="input-group-append">
                            <!-- Button trigger modal -->
                            <div class="col-md-4 input-group-append" aria-describedby="TipoAnalisisDropdonwList">
                                <button aria-describedby="TipoAnalisisDropdonwList" type="button" class="btn btn-info" data-toggle="modal" data-target="#myModal" runat="server">+</button>
                                <%--<asp:Button Text="Agregar" class="btn btn-info" runat="server" ID="Button1" data-toggle="modal" data-target="#myModal" />--%>
                            </div>
                        </div>

                        <%--Resultados--%>
                        <div class="input-group-prepend">
                            <span class="input-group-text" id="ResultadoAnalisis">Resultado </span>
                        </div>
                        <div class="input-group-append" aria-describedby="ResultadoAnalisisTextBox">
                            <asp:TextBox ID="ResultadoAnalisisTextBox" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                        <%--AgregarDetalle--%>
                        <div class="col-md-4 input-group-append" aria-describedby="AgregarDetalleButton">
                            <asp:Button Text="Agregar" class="btn btn-info" runat="server" ID="AgregarDetalleButton" OnClick="AgregarDetalleButton_Click" />
                        </div>
                    </div>
                    <%--Fecha--%>
                    <div class="input-group mb-3">
                        <div class="input-group-prepend">
                            <span class="input-group-text" id="Fecha">Fecha </span>
                        </div>
                        <div class="input-group-append" aria-describedby="FechaTextBox">
                            <asp:TextBox ID="FechaTextBox" TextMode="Date" runat="server" class="form-control input-sm" Visible="true"></asp:TextBox>
                        </div>
                    </div>
                    <%--GRID--%>
                    <div class="table table-condensed table-bordered table-responsive">
                        <asp:GridView ID="DetalleGridView"
                            runat="server"
                            CssClass="table table-condensed table-bordered table-responsive"
                            CellPadding="4" ForeColor="#333333" GridLines="None">
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
            </div>
        </div>
    </div>
    <div class="panel-footer">
        <div class="text-center">
            <div class="form-group" display: inline-block>
                <asp:Button Text="Nuevo" class="btn btn-warning btn-sm" runat="server" ID="NuevoButton" OnClick="NuevoButton_Click" />
                <asp:Button Text="Guardar" class="btn btn-success btn-sm" runat="server" ID="GuadarButton" OnClick="GuadarButton_Click" />
                <asp:Button Text="Eliminar" class="btn btn-danger btn-sm" runat="server" ID="EliminarButton" OnClick="EliminarButton_Click" />
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
                    <div class="input-group mb-3">
                        <div class="input-group-prepend">
                            <span class="input-group-text" id="DescripcionLb">Descripción </span>
                        </div>
                        <div aria-describedby="DescripcionLb">
                            <asp:TextBox ID="DescripcionAnalisisTextBox" runat="server" class="form-control input-sm"></asp:TextBox>
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
