<%@ Page Title="Admin" Language="C#" MasterPageFile="~/Members/Members.master" AutoEventWireup="true" CodeFile="Admin.aspx.cs" Inherits="Admin" %>
<%@ MasterType VirtualPath="~/Members/Members.master"%>

<%-- Agregue aquí los controles de contenido --%>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <asp:Label ID="Titulo1" runat="server" Text="Seleccione la opción deseada para cargar una base de datos en le servidor o guardar la base de datos actual. <br> Es recomendable realizar una copia de seguridad antes de borrar el contenido de datos.<br> Una vez borrados los datos no es posible volver a recupearlos."
        Font-Bold="true"
        Font-Size="Larger">
    </asp:Label>
    <br />
    <br />
    <div>
        <asp:Table ID="CargaDatos" runat="server"
            CellPadding="10"
            GridLines="Both"
            HorizontalAllign="Center"
            Font-Size="Larger">
            <asp:TableRow>
                <asp:TableCell>
                    <asp:Label ID="Titulo2" runat="server" Text="Presione el botón para salvar la base de datos en un archivo"
                        Font-Bold="True">
                    </asp:Label>
                </asp:TableCell>
                <asp:TableCell>
                    <asp:Button ID="Button1" runat="server" Text="Guarda DB" Font-Size="Large" OnClick="GuardaButton_Click" />
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell>
                    <asp:Label ID="Titulo3" runat="server" Text="Si desea cargar una nueva base de datos, presione el botón"
                        Font-Bold="True">
                    </asp:Label>
                </asp:TableCell>
                <asp:TableCell>
                    <asp:FileUpload ID="FileUpload1" runat="server" Font-Size="Large" Height="33px" />&nbsp;&nbsp;
                    <asp:Button ID="Button2" runat="server" Text="Sube DB" Font-Size="Large" OnClick="SubeButton_Click"/>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell>
                    <asp:Label ID="Titulo4" runat="server" Text="Presione el botón para borrar el contenido de la base de datos"
                        Font-Bold="true">
                    </asp:Label>
                </asp:TableCell>
                <asp:TableCell>
                    <asp:Button ID="Button3" runat="server" Text="Borra DB" Font-Size="Large" OnClick="BorraDbButton_Click" />
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
    </div>
    <br />
    <div style="margin-left:auto;margin-right:auto;text-align:center;">
        <asp:Label ID="InfoLabel" runat="server" Font-Size="Larger" Font-Bold="true">
        </asp:Label>
    </div>
    <br />
    <br />
</asp:Content>