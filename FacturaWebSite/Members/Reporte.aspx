<%@ Page Title="Reporte" Language="C#" MasterPageFile="~/Members/Members.master" AutoEventWireup="true" CodeFile="Reporte.aspx.cs" Inherits="Reporte" %>
<%@ MasterType VirtualPath="~/Members/Members.master"%>

<%-- Agregue aquí los controles de contenido --%>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <asp:Label ID="Titulo1" runat="server" Text="La base de datos será descargada en un archivo Excel. Las facturas XML no serán incluídas."
            Font-Bold="True"
            Font-Size="Larger">
    </asp:Label>
    <br />
    <br />
    <div>
        <asp:Label ID="NomArchLabel" runat="server" Text="Nombre del archivo (Opcional):" Font-Size="Larger">
        </asp:Label>
        <asp:TextBox ID="NomArchText" runat="server" Font-Size="Larger" Width="250">
        </asp:TextBox>
    </div>
    <br />
    <div>
        <asp:Button ID="Button1" runat="server" Text="Exportar a Excel" Font-Size="Larger" OnClick="Load_DB" />
    </div>
    <br />
    <hr />
    <div>
        <asp:Label ID="StatusLineLabel" runat="server" Font-Size="Larger"></asp:Label>
    </div>
    <hr />
</asp:Content>