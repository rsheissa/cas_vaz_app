<%@ Page Title="Home" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <h1>Bienvenido al sistema de captura, verificación y almacenaje de facturas CFDI</h1>

    <p>Este sitio permite las siguientes operaciones:</p>

    <ul>
        <li>Captura manual o código QR de una factura</li>
        <li>Captura automatizada de un grupo de facturas</li>
        <li>Consulta y validación de las facturas en el SAT</li>
        <li>Revisión de consultas anteriores</li>
        <li>Exportar la base de datos en formato EXCEL</li>
        <li>Importar y exportar la base de datos</li>
    </ul>

    <asp:Login ID="Login1" runat="server"></asp:Login>

</asp:Content>

