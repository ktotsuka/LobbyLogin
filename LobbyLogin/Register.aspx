<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="Register.aspx.cs" Inherits="LobbyLogin.Register" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server" >
    <meta http-equiv="Refresh" content="300;url=ConfidentialityAgreement.aspx" />
    <asp:Table runat="server">
        <asp:TableRow>
            <asp:TableHeaderCell Font-Size="Large">Returning visitor?</asp:TableHeaderCell>
            <asp:TableCell>
                <asp:DropDownList ID="VisitorsDropDownList" runat="server" Font-Size="Large" OnSelectedIndexChanged="VisitorsOnSelectedIndexChanged" AutoPostBack="True" Width="400">
                </asp:DropDownList>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow >
            <asp:TableHeaderCell Font-Size="Large" Width="300">Last name <span style="COLOR: red">(required)</span></asp:TableHeaderCell>
            <asp:TableCell>
                <asp:TextBox ID="lastName" runat="server" Font-Size="Large" MaxLength="50" AutoCompleteType="Disabled" OnTextChanged="LastNameOnTextChanged" AutoPostBack="True" Width="400" />
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableHeaderCell Font-Size="Large">First name <span style="COLOR: red">(required)</span></asp:TableHeaderCell>
            <asp:TableCell>
                <asp:TextBox ID="firstName" runat="server" Font-Size="Large" MaxLength="50" AutoCompleteType="Disabled" Width="400"/>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableHeaderCell Font-Size="Large">Company name <span style="COLOR: red">(required)</span></asp:TableHeaderCell>
            <asp:TableCell>
                <asp:TextBox ID="companyName" runat="server" Font-Size="Large" MaxLength="50" AutoCompleteType="Disabled" Width="400"/>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableHeaderCell Font-Size="Large">Email address</asp:TableHeaderCell>
            <asp:TableCell>
                <asp:TextBox ID="emailAddress" runat="server" Font-Size="Large" MaxLength="50" AutoCompleteType="Disabled" Width="400"/>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableHeaderCell Font-Size="Large">Phone number</asp:TableHeaderCell>
            <asp:TableCell>
                <asp:TextBox ID="phoneNumber" runat="server" Font-Size="Large" MaxLength="50" AutoCompleteType="Disabled" Width="400"/>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableHeaderCell Font-Size="Large">Employee you are visiting</asp:TableHeaderCell>
            <asp:TableCell>
                <asp:DropDownList ID="EmployeesDropDownList" runat="server" Font-Size="Large" Width="400">
                </asp:DropDownList>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell>
                <asp:Button ID="submitButton" runat="server" Text="Submit" OnClick="SubmitButton_Click" UseSubmitBehavior="false" OnClientClick="this.disabled='true'; this.value='please wait';"  Font-Size="X-Large"/>
            </asp:TableCell>
            <asp:TableHeaderCell ID="submitMessage" runat="server" HorizontalAlign="Left" ForeColor="Red"></asp:TableHeaderCell>
        </asp:TableRow>
    </asp:Table>
</asp:Content>
