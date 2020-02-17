<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="Admin.aspx.cs" Inherits="LobbyLogin.LogIn" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <hr>
    <asp:Table ID="cardGameTable" runat="server">
        <asp:TableRow>
            <asp:TableHeaderCell>Password</asp:TableHeaderCell>
            <asp:TableCell>
                <asp:TextBox ID="adminPassword" runat="server" />
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>
    <asp:Button ID="submitButton" runat="server" Text="Submit" OnClick="AdminPasswordSubmitButton_Click" />
</asp:Content>
