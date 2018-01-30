<%@ Page Title="Consulta" Language="C#" MasterPageFile="~/Members/Members.master" AutoEventWireup="true" CodeFile="Consulta.aspx.cs" Inherits="Consulta" %>
<%@ MasterType VirtualPath="~/Members/Members.master"%>

<%-- Agregue aquí los controles de contenido --%>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <div>
        <asp:Table ID="CargaDatos" runat="server"
            CellPadding="10"
            GridLines="Both"
            HorizontalAllign="Center"
            Font-Size="Larger">
            <asp:TableRow ID="CargaDB">
                <asp:TableCell>
                    <asp:Label ID="Titulo1" runat="server" Text="Presione el botón para cargar la base de datos actualizada hasta el momento"
                        Font-Bold="True">
                    </asp:Label>
                </asp:TableCell>
                <asp:TableCell>
                    <asp:Button ID="Button1" runat="server" Text="Cargar DB" Font-Size="Large" OnClick="Load_DB" />
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow ID="ActualizaFecha">
                <asp:TableCell>
                    <asp:Label ID="Titulo3" runat="server" Text="Si desea actualizar la fecha de verificación, presione el botón"
                        Font-Bold="True">
                    </asp:Label>
                </asp:TableCell>
                <asp:TableCell>
                    <asp:Button ID="Button3" runat="server" Text="Verificar Datos" Font-Size="Large" OnClick="Verificar_Fecha"/>
                </asp:TableCell>
            </asp:TableRow>
             <asp:TableRow ID="FiltroManual">
                <asp:TableCell>
                    <asp:Label ID="Label2" runat="server" Text="Introduzca el identificador, presione el botón"
                        Font-Bold="true">
                    </asp:Label>
                </asp:TableCell>
                <asp:TableCell>
                    <asp:TextBox ID="IdentiTextBox" runat="server" Width="220" Font-Size="Large" Style="margin-left">
                    </asp:TextBox>
                </asp:TableCell>
                <asp:TableCell>
                    <asp:Button ID="Button4" runat="server" Text="Filtrar" Font-Size="Large" OnClick="Filtrar_Identificador" />
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow ID="ListaFiltro" runat="server">
                <asp:TableCell>
                    <asp:Label ID="Label3" runat="server" Text="Seleccione una opción de la lista"
                        Font-Bold="true">
                    </asp:Label>
                </asp:TableCell>
                <asp:TableCell>
                    <asp:DropDownList ID="ddlFilter" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlFilter_SelectedIndexChanged" Font-Bold="True">
                    </asp:DropDownList>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow ID="BorraRegistro">
                <asp:TableCell>
                    <asp:Label ID="Titulo2" runat="server" Text="Introduzca el número del campo ''doc_id'' que desea eliminar"
                        Font-Bold="true">
                    </asp:Label>
                </asp:TableCell>
                <asp:TableCell>
                    <asp:TextBox ID="DeleteTextBox" runat="server" Width="145" Font-Size="Large" Style="margin-left">
                    </asp:TextBox>
                </asp:TableCell>
                <asp:TableCell>
                    <asp:Button ID="Button2" runat="server" Text="Borrar Campo" Font-Size="Large" OnClick="Button2_Click" />
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
    </div>
    <br />
    <div style="margin-left:auto;margin-right:auto;text-align:center;">
        <asp:Label ID="EstatusLabel" runat="server" Font-Size="Larger" Font-Bold="true">
        </asp:Label>
    </div>
    <br />
    <br />
    <div>
        <asp:Label ID="Label1" runat="server" Text="Página No.:"></asp:Label>&nbsp;
        <asp:DropDownList ID="ddlPageNos" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlPageNos_SelectedIndexChanged"></asp:DropDownList>
    </div>
    <div style="margin:0 auto 0 auto; width:1500px">
        <asp:GridView ID="GridView1" Font-Size="Larger" CellPadding="5" CellSpacing="2" runat="server">
        </asp:GridView>
    </div>
</asp:Content>