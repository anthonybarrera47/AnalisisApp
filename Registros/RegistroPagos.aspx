<%@ Page
    Title="Pagos de Analisis"
    Language="C#"
    MasterPageFile="~/Site.Master"
    AutoEventWireup="true"
    CodeBehind="RegistroPagos.aspx.cs"
    Inherits="AnasilisApp.Registros.RegistroPagos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container-fluid">
        <div class="card text-center bg-light mb-3">
            <div class="card-header"><%:Page.Title %></div>
            <div class="card-body">
                <%--PagosID--%>
                <div class="input-group mb-2">
                    <div class="input-group-prepend">
                        <span class="input-group-text" id="PagosID">PagosID </span>
                    </div>
                    <div aria-describedby="PagosID">
                        <asp:TextBox ID="PagosIdTextBox" TextMode="Number" MaxLength="9" runat="server" Text="0" CssClass="form-control input-sm"></asp:TextBox>
                    </div>
                    <div class="input-group-append">
                        <asp:Button Text="Buscar" CssClass="btn btn-info" runat="server" ID="BuscarButton" OnClick="BuscarButton_Click" />
                    </div>
                </div>
                <%--Fecha--%>
                <div class="input-group mb-3">
                    <div class="input-group-append">
                        <span class="input-group-text" id="Fecha">Fecha </span>
                    </div>
                    <div class="input-group-append" aria-describedby="Fecha">
                        <asp:TextBox ID="FechaTextBox" TextMode="Date" runat="server" CssClass="form-control input-sm" Visible="true"></asp:TextBox>
                    </div>
                </div>
                <%--Analisis--%>
                <div class="input-group mb-2">
                    <%--Paciente--%>
                    <div class="input-group-append">
                        <span class="input-group-text" id="PacienteNombre">Paciente </span>
                    </div>
                    <div aria-describedby="PacienteNombre">
                        <asp:TextBox AutoPostBack="true" OnTextChanged="PacienteTextBox_TextChanged" ID="PacienteTextBox" runat="server" CssClass="form-control input-sm" Visible="true">
                        </asp:TextBox>
                    </div>
                    <div class="input-group-append">
                        <asp:Button Text="Buscar" CssClass="btn btn-info" runat="server" ID="BuscarPaciente" OnClick="BuscarPaciente_Click" />
                    </div>
                </div>
                <%--AnalisisDropWDown--%>
                <div class="input-group mb-2">
                    <div class="input-group-prepend">
                        <span class="input-group-text" id="Analisis">AnalisisID </span>
                    </div>
                    <div aria-describedby="Analisis">
                        <asp:DropDownList ID="AnalisisDropDownList" AutoPostBack="true" OnSelectedIndexChanged="AnalisisDropDownList_SelectedIndexChanged" CssClass=" form-control dropdown-item" AppendDataBoundItems="true" runat="server" Height="2.5em">
                        </asp:DropDownList>
                    </div>

                    <%--Balance--%>
                    <div class="input-group-append">
                        <span class="input-group-text" id="BalanceAnalisis">Balance</span>
                    </div>
                    <div aria-describedby="BalanceAnalisis">
                        <asp:TextBox ID="BalanceTextBox" AutoPostBack="true" ReadOnly="true" runat="server" Text="0" CssClass="form-control input-sm"></asp:TextBox>
                    </div>
                </div>
                <%--Monto a Pagar--%>
                <div class="input-group mb-2">
                    <div class="input-group-prepend">
                        <span class="input-group-text" id="MontoPagar">Monto A Pagar</span>
                    </div>
                    <div aria-describedby="MontoPagar">
                        <asp:TextBox ID="MontoPagarTextBox" TextMode="Number" MaxLength="9" runat="server" Text="0" CssClass="form-control input-sm"></asp:TextBox>
                    </div>
                    <div class="input-group-append">
                        <asp:Button Text="Agregar" CssClass="btn btn-info" runat="server" ID="AgregarPagoButton" OnClick="AgregarPagoButton_Click" />
                    </div>
                </div>

                <asp:ScriptManager ID="ScriptManger" runat="server"></asp:ScriptManager>
                <asp:UpdatePanel ID="UpdatePanel" runat="server">
                    <ContentTemplate>
                        <div class="row">
                            <div class="table table-responsive">
                                <asp:GridView ID="DetalleGridView"
                                    runat="server"
                                    CssClass="table table-condensed table-bordered table-responsive"
                                    CellPadding="4" ForeColor="#333333" GridLines="None"
                                    OnPageIndexChanging="DetalleGridView_PageIndexChanging"
                                    AllowPaging="true" PageSize="6" AutoGenerateColumns="false">
                                    <Columns>
                                        <asp:TemplateField ShowHeader="False" HeaderText="Opciones">
                                            <ItemTemplate>
                                                <asp:Button ID="RemoverDetalleClick" runat="server" CausesValidation="false" CommandName="Select"
                                                    Text="Remover" CssClass="btn btn-danger btn-sm" OnClick="RemoverDetalleClick_Click" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="DetallePagoID" DataField="DetallePagoID" Visible="false" />
                                        <asp:BoundField HeaderText="PagosID" DataField="PagosID" Visible="false" />
                                        <asp:BoundField HeaderText="AnalisisID" DataField="AnalisisID" />
                                        <asp:BoundField HeaderText="Fecha" DataField="FechaRegistro" DataFormatString="{0:dd-MM-yyyy}"  />
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
                        <asp:AsyncPostBackTrigger ControlID="DetalleGridView" />
                    </Triggers>
                </asp:UpdatePanel>
                <%--GRID--%>
                <div class="panel-footer">
                    <div class="text-center">
                        <div class="form-group" display: inline-block>
                            <asp:Button Text="Nuevo" CssClass="btn btn-warning btn-lg" runat="server" ID="NuevoButton" OnClick="NuevoButton_Click" />
                            <asp:Button Text="Guardar" CssClass="btn btn-success btn-lg" runat="server" ID="GuadarButton" OnClick="GuadarButton_Click" />
                            <asp:Button Text="Eliminar" CssClass="btn btn-danger btn-lg" runat="server" ID="EliminarButton" OnClick="EliminarButton_Click" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
