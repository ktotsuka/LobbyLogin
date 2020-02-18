<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="ThankYou.aspx.cs" Inherits="LobbyLogin.ThankYou" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Image ID="Image1" Height="200" width="400" runat="server" ImageUrl="Images/Bastian-Solutions-TAL.png" />
    <hr>
    <div>
        <h1>Thank you for signing in.  Your escort has been notified and should be on the way.</h1>        
    </div>
    <hr>
    <asp:Table ID="cardGameTable" runat="server">
        <asp:TableRow>
            <asp:TableCell>
                <asp:Button ID="backButton" runat="server" Text="Back to guest sign-in" OnClick="BackButton_Click" />
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>
</asp:Content>