<%@ Page Title="Automático" Language="C#" MasterPageFile="~/Members/Members.master" AutoEventWireup="true" CodeFile="Captura2.aspx.cs" Inherits="Captura2" %>
<%@ MasterType VirtualPath="~/Members/Members.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <div>
        <asp:Label ID="Titulo1" runat="server" Text="Se procesarán archivos XML y archivos ZIP que contengan XML. Otro tipo de archivos serán ignorados."
            Font-Bold="True"
            Font-Size="Larger">
        </asp:Label>
        <br />
        <br />
        <asp:Table ID="IdentiTable" runat="server"
            CellPadding="10"
            GridLines="None"
            HorizontalAllign="Center"
            Font-Size="Larger">
             <asp:TableRow>
                <asp:TableCell>
                    Identificador
                </asp:TableCell>
                <asp:TableCell>
                    <asp:TextBox ID="IdentificadorTextBox" runat="server" Width="220" Font-Size="Large" Style="margin-left">
                    </asp:TextBox>
                </asp:TableCell>
                 <asp:TableCell>
                    <asp:Button ID="LimpiarButton" runat="server" Text="Limpiar Campo" Font-Size="Large" OnClick="LimpiarButton_Click" />
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
        <br />
        <asp:FileUpload ID="FileUpload1"
            runat="server"
            AllowMultiple="true"
            Font-Size="Large"></asp:FileUpload>
        <br />
        <br />
        <asp:Button ID="UploadBtn"
            Text="Subir Archivo"
            OnClick="UploadBtn_Click"
            Font-Size="Large"
            runat="server"></asp:Button>
        <br />
        <br />
        <hr />
        <div style="margin-left: auto; margin-right: auto; text-align: center;">
            <asp:Label ID="UploadStatusLabel"
                Font-Size="Larger"
                runat="server">
            </asp:Label>

        </div>
    </div>
    <hr />
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
        <asp:TableRow>
            <asp:TableCell>
                Nombre Archivo:
            </asp:TableCell>
            <asp:TableCell>
                <asp:Label ID="NombreArchLabel" runat="server">
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

