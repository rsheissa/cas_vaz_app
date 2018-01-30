<%@ Page Title="Manual" Language="C#" MasterPageFile="~/Members/Members.master" AutoEventWireup="true" CodeFile="Captura.aspx.cs" Inherits="Captura" %>
<%@ MasterType VirtualPath="~/Members/Members.master" %>

<%-- Agregue aquí los controles de contenido --%>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:Label ID="Titulo1" runat="server" Text="Capture los datos en las casillas correspondientes. Presione el botón para verificar en SAT."
            Font-Bold="True"
            Font-Size="Larger">
    </asp:Label>
    <br />
    <br />
    <div style="margin-left:auto;margin-right:auto;text-align:left">
        <asp:Table ID="EntradaDatos" runat="server"
            CellPadding="10"
            GridLines="Both"
            HorizontalAllign="Center"
            Font-Size="Larger">
            <asp:TableRow>
                <asp:TableCell>
                    RFC Emisor
                </asp:TableCell>
                <asp:TableCell>
                    <asp:TextBox ID="RfcETextBox" runat="server" Width="145" Font-Size="Large" Style="margin-left:auto">
                    </asp:TextBox>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell>
                    RFC Receptor
                </asp:TableCell>
                <asp:TableCell>
                    <asp:TextBox ID="RfcRTextBox" runat="server" Width="145" Font-Size="Large" Style="margin-left:auto">
                    </asp:TextBox>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell>
                    UUID
                </asp:TableCell>
                <asp:TableCell>
                    <asp:TextBox ID="UuidTextBox" runat="server" Width="360" Font-Size="Large" Style="margin-left:auto">
                    </asp:TextBox>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell>
                    Total $
                </asp:TableCell>
                <asp:TableCell>
                    <asp:TextBox ID="TotalTextBox" runat="server" Width="120" Font-Size="Large" Style="margin-left:auto">
                    </asp:TextBox>
                </asp:TableCell>
            </asp:TableRow>
             <asp:TableRow>
                <asp:TableCell>
                    Identificador
                </asp:TableCell>
                <asp:TableCell>
                    <asp:TextBox ID="IdentiTextBox" runat="server" Width="220" Font-Size="Large" Style="margin-left:auto">
                    </asp:TextBox>&nbsp;&nbsp;
                    <asp:Button ID="BorraIdenti" runat="server" Text="Limpiar Campo" Font-Size="Large" OnClick="BorraIdenti_Click" />
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell>
                    QR Code
                </asp:TableCell>
                <asp:TableCell>
                    <asp:TextBox ID="QrTextBox" runat="server" OnTextChanged="Procesar_Click" AutoPostBack="true" Width="965" Font-Size="Large" Style="margin-left:auto">
                    </asp:TextBox>
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
        <br />
        <br />
        <asp:Button ID="Button1" runat="server" Text="Procesar" Font-Size="Larger" OnClick="Procesar_Click" />&nbsp;&nbsp;
        <asp:Button ID="Button2" runat="server" Text="Limpiar Campos" Font-Size="Larger" OnClick="LimpiarButton_Click" />
        
        <hr />
        <div style="margin-left:auto;margin-right:auto;text-align:center;">
            <asp:Label ID="UploadStatusLabel" runat="server" Font-Size="Larger"></asp:Label>
        </div>
        <hr />
   </div>

    <asp:Table ID="Table1" runat="server"
        CellPadding="10"
        GridLines="Both"
        HorizontalAlign="Center" Font-Size="Larger">
        <asp:TableRow>
            <asp:TableCell>
                Total:
            </asp:TableCell>
            <asp:TableCell>
                <asp:Label ID="TotalLabel" runat="server">
                </asp:Label>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell>
                RFC Emisor:
            </asp:TableCell>
            <asp:TableCell>
                <asp:Label ID="RfcEmisorLabel" runat="server">
                </asp:Label>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell>
                RFC Receptor:
            </asp:TableCell>
            <asp:TableCell>
                <asp:Label ID="RfcReceptorLabel" runat="server">
                </asp:Label>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell>
                UUID:
            </asp:TableCell>
            <asp:TableCell>
                <asp:Label ID="UuidLabel" runat="server">
                </asp:Label>
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>
    <br />
    <br />
    <asp:Table ID="Table2" runat="server"
        CellPadding="10"
        GridLines="Both"
        HorizontalAlign="Center" Font-Size="Larger">
        <asp:TableRow>
            <asp:TableCell>
                Estatus:
            </asp:TableCell>
            <asp:TableCell>
                <asp:Label ID="CodigoEstatusLabel" runat="server">
                </asp:Label>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell>
                Estado:
            </asp:TableCell>
            <asp:TableCell>
                <asp:Label ID="EstadoLabel" runat="server">
                </asp:Label>
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>
</asp:Content>
