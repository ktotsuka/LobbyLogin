<%@ Page Title="BAV Lobby Login" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="LobbyLogin._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server" >
    <script type="text/javascript">
        function ShowWaitMessage() { setTimeout('document.getElementById("WaitMessage").style.display ="inline";', 500); }
    </script>
    <p></p>
    <asp:Image Height="200" Width="400" runat="server" ImageUrl="Images/Bastian-Solutions-TAL.png" />
    <div>
        <h1>BAV Guest Sign-in</h1>
    </div>
    <hr>
    <asp:Table runat="server">
        <asp:TableRow>
            <asp:TableHeaderCell>First name <span style="COLOR: red">(required)</span></asp:TableHeaderCell>
            <asp:TableCell>
                <asp:TextBox ID="firstName" runat="server" MaxLength="50" />
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableHeaderCell>Last name <span style="COLOR: red">(required)</span></asp:TableHeaderCell><asp:TableCell>
                <asp:TextBox ID="lastName" runat="server" MaxLength="50" />
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableHeaderCell>Company name <span style="COLOR: red">(required)</span></asp:TableHeaderCell><asp:TableCell>
                <asp:TextBox ID="companyName" runat="server" MaxLength="50" />
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableHeaderCell>Email address</asp:TableHeaderCell><asp:TableCell>
                <asp:TextBox ID="emailAddress" runat="server" MaxLength="50" />
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableHeaderCell>Phone number</asp:TableHeaderCell><asp:TableCell>
                <asp:TextBox ID="phoneNumber" runat="server" MaxLength="50" />
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableHeaderCell>Person you are visiting</asp:TableHeaderCell><asp:TableCell>
                <asp:DropDownList ID="EmployeesDropDownList" runat="server">
                </asp:DropDownList>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell>
                <asp:Button ID="submitButton" runat="server" Text="Submit" OnClick="SubmitButton_Click" OnClientClick="ShowWaitMessage()" />
                <div style="background-color: Red; display: none;" id="WaitMessage"> Please wait...  </div>
            </asp:TableCell>
            <asp:TableHeaderCell ID="submitMessage" runat="server" HorizontalAlign="Left"></asp:TableHeaderCell>
        </asp:TableRow>
    </asp:Table>
    <br />
</asp:Content>
