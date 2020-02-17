<%@ Page Title="BAV Lobby Login" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="LobbyLogin._Default" %>


<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div>
        <h1>BAV Guest Sign-in</h1>        
    </div>
    <hr>
    <asp:Table ID="cardGameTable" runat="server">
        <asp:TableRow>
            <asp:TableHeaderCell>Guest name</asp:TableHeaderCell>
            <asp:TableCell>
                <asp:TextBox ID="guestName" runat="server" />
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell>
               <asp:RequiredFieldValidator 
                    ID="RequiredFieldValidatorGuestName" 
                    runat="server" style="color:Red;"
                    ErrorMessage="A guest name is required." 
                    ControlToValidate="guestName">
                </asp:RequiredFieldValidator>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableHeaderCell>Person you are visiting</asp:TableHeaderCell>
            <asp:TableCell>
                <asp:DropDownList ID="Employees" runat="server">
                    <asp:ListItem Text="Kenji Totsuka"></asp:ListItem>
                    <asp:ListItem Text="Trung Hoang"></asp:ListItem>
                </asp:DropDownList>
            </asp:TableCell>
        </asp:TableRow>

    </asp:Table>
    <br />
    <asp:Button ID="dealHandButton" runat="server" Text="Submit" 
        OnClick="dealHandButton_Click" />
    <br />
    <asp:Label ID="dealtHandLabel" runat="server" Visible="false" 
        Text="Here are the cards." />
    <asp:Table ID="dealtHandsTable" runat="server" Visible="false" />
</asp:Content>
