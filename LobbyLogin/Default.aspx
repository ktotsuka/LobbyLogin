<%@ Page Title="BAV Lobby Login" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="LobbyLogin._Default" %>


<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <p></p>
    <asp:Image ID="Image1" Height="200" Width="400" runat="server" ImageUrl="Images/Bastian-Solutions-TAL.png" />
    <div>
        <h1>BAV Guest Sign-in</h1>
    </div>
    <hr>
    <asp:Table ID="cardGameTable" runat="server">
        <asp:TableRow>
            <asp:TableHeaderCell>First name <span style="COLOR: red">(required)</span></asp:TableHeaderCell>
            <asp:TableCell>
                <asp:TextBox ID="firstName" runat="server" />
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableHeaderCell>Last name <span style="COLOR: red">(required)</span></asp:TableHeaderCell><asp:TableCell>
                <asp:TextBox ID="lastName" runat="server" />
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableHeaderCell>Company name <span style="COLOR: red">(required)</span></asp:TableHeaderCell><asp:TableCell>
                <asp:TextBox ID="companyName" runat="server" />
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableHeaderCell>Email address</asp:TableHeaderCell><asp:TableCell>
                <asp:TextBox ID="emailAddress" runat="server" />
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableHeaderCell>Phone number</asp:TableHeaderCell><asp:TableCell>
                <asp:TextBox ID="phoneNumber" runat="server" />
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableHeaderCell>Person you are visiting</asp:TableHeaderCell><asp:TableCell>
                <asp:DropDownList ID="employees" runat="server">
                    <asp:ListItem Text="Kenji Totsuka"></asp:ListItem>
                    <asp:ListItem Text="Trung Hoang"></asp:ListItem>
                </asp:DropDownList>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell>
                <asp:Button ID="submitButton" runat="server" Text="Submit" OnClick="SubmitButton_Click" />
            </asp:TableCell>
            <asp:TableHeaderCell ID="submitErrorMessage" runat="server" HorizontalAlign="Left" ForeColor="Red"></asp:TableHeaderCell>
        </asp:TableRow>
    </asp:Table>
    <br />
</asp:Content>
