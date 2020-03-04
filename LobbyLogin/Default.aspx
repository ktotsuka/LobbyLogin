<%@ Page Title="BAV Lobby Login" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="LobbyLogin._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server" >
    <p></p>
    <h1>
        &nbsp &nbsp &nbsp &nbsp &nbsp &nbsp &nbsp &nbsp
        <asp:Image Height="180" Width="360" runat="server" ImageUrl="Images/Bastian-Solutions-TAL.png" />
    </h1>
    <div>
        <h1>&nbsp &nbsp &nbsp &nbsp &nbsp &nbsp &nbsp &nbsp BAV Guest Sign-in</h1>
    </div>
    <hr>
    <asp:Table runat="server">
        <asp:TableRow >
            <asp:TableHeaderCell Font-Size="Large">Do you have an appointment? </asp:TableHeaderCell>
        </asp:TableRow>
    </asp:Table>
    <asp:Table runat="server">
        <asp:TableRow>
            <asp:TableCell>
                <asp:Button ID="YesButton" runat="server" Text="Yes" Width="75" Height="75" OnClick="YesButton_Click" Font-Size="X-Large" />
            </asp:TableCell>
            <asp:TableHeaderCell Font-Size="Large">&nbsp &nbsp</asp:TableHeaderCell>
            <asp:TableCell>
                <asp:Button ID="NoButton" runat="server" Text="No" Width="75" Height="75" OnClick="NoButton_Click" Font-Size="X-Large" />
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>
</asp:Content>
