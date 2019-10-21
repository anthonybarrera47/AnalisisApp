<%@ Page Title="Registro de Tipos de Analisis" 
    Language="C#" 
    MasterPageFile="~/Site.Master" 
    AutoEventWireup="true" 
    CodeBehind="RegistroTipoAnalisis.aspx.cs" 
    Inherits="AnasilisApp.Registros.RegistroTipoAnalisis" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container-fluid ">
        <div class="card align-items-center text-center bg-light">
            <div class="card-header"><%:Page.Title %></div>
            <div class="card-body ">
                <%--TipoAnalisisID--%>
                <div class="input-group">
                    <div class="input-group-prepend">
                        <span class="input-group-text">TipoAnalisis </span>
                    </div>
                    <asp:TextBox ID="TipoIdTextBox" TextMode="Number" MaxLength="9" runat="server" Text="0" class="form-control input-sm "></asp:TextBox>
                    <asp:Button Text="Buscar" class="btn btn-info" runat="server" ID="BuscarButton" OnClick="BuscarButton_Click" />
                </div>
                <%--Descripción--%>
                <div class="input-group">
                    <div class="input-group-prepend">
                        <span class="input-group-text">Descripción </span>
                    </div>
                    <asp:TextBox ID="DescripcionTextBox" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                </div>
                <%--Monto--%>
                <div class="input-group">
                    <div class="input-group-prepend">
                        <span class="input-group-text">Monto </span>
                    </div>
                    <asp:TextBox ID="MontoTextBox" TextMode="Number" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                </div>
                <%--Fecha--%>
                <div class="input-group">
                    <div class="input-group-prepend">
                        <span class="input-group-text" id="Fecha">Fecha </span>
                    </div>
                    <asp:TextBox ID="FechaTextBox" TextMode="Date" runat="server" class="form-control input-sm " Visible="true"></asp:TextBox>
                </div>
                <div class="card-footer">
                    <div class="text-center">
                        <div class="form-group" display: inline-block>
                            <asp:Button Text="Nuevo" class="btn btn-warning btn-lg" runat="server" ID="NuevoButton" OnClick="NuevoButton_Click" />
                            <asp:Button Text="Guardar" class="btn btn-success btn-lg" runat="server" ID="GuadarButton" OnClick="GuadarButton_Click"/>
                            <asp:Button Text="Eliminar" class="btn btn-danger btn-lg" runat="server" ID="EliminarButton" OnClick="EliminarButton_Click" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
