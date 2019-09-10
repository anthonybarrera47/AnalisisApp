﻿<%@ Page
    Title="Consulta de Analisis"
    Language="C#"
    MasterPageFile="~/Site.Master"
    AutoEventWireup="true"
    CodeBehind="ConsultaAnalisis.aspx.cs"
    Inherits="AnasilisApp.Consultas.ConsultaAnalisis" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container-fluid">
        <div class="card text-center bg-light mb-3">
            <div class="card-header"><%:Page.Title %></div>
            <div class="card-body">
                <div class="form-horizontal col-md-12" role="form">
                    <div>
                        <div class="input-group mb-3">
                            <div class="input-group-prepend">
                                <asp:DropDownList ID="BuscarPorDropDownList" runat="server" CssClass="form-control input-sm">
                                <asp:ListItem>Todos</asp:ListItem>
                                <asp:ListItem>AnalisisID</asp:ListItem>
                                <asp:ListItem>PacienteID</asp:ListItem>
                                <asp:ListItem>Tipo de Analisis</asp:ListItem>
                            </asp:DropDownList>
                            </div>
                        </div>
                        <div class="input-group-append">
                            <asp:TextBox ID="FiltroTextBox" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                        </div>
                        <div class="input-group-append" aria-describedby="FiltroTextBox">
                            <asp:Button ID="BuscarButton" runat="server" Class="btn btn-success input-sm" Text="Buscar" OnClick="BuscarButton_Click" />
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
                    <div class="table table-condensed table-bordered table-responsive">
                        <asp:GridView ID="DatosGridView"
                            runat="server"
                            CssClass="table table-condensed table-bordered table-responsive"
                            CellPadding="4" ForeColor="#333333" GridLines="None">

                            <AlternatingRowStyle BackColor="LightBlue" />

                            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                            <RowStyle BackColor="#EFF3FB" />
                            <Columns>
                                <asp:TemplateField ShowHeader="False" HeaderText="Opciones">
                                    <ItemTemplate>
                                        <asp:Button ID="EditarAnalisis" runat="server" CausesValidation="false" CommandName="Select"
                                            Text="Editar" class="btn btn-danger btn-sm" OnClientClick="window.open('~/Registros/RegistroAnalisis.aspx?Id={0}', '');" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </div>
    </div>

</asp:Content>