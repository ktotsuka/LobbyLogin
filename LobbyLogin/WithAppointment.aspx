<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="WithAppointment.aspx.cs" Inherits="LobbyLogin.WithAppointment" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server" >
    <script type="text/javascript">
        function ShowWaitMessage() { setTimeout('document.getElementById("WaitMessage").style.display ="inline";', 500); }
    </script>
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
            <asp:TableHeaderCell Font-Size="Large" Width="300">Last name <span style="COLOR: red">(required)</span></asp:TableHeaderCell>
            <asp:TableCell>
                <asp:TextBox ID="lastName" runat="server" Font-Size="Large" MaxLength="50" AutoCompleteType="Disabled" OnTextChanged="LastNameOnTextChanged" AutoPostBack="True" Width="400" />
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableHeaderCell Font-Size="Large">Returning visitor?</asp:TableHeaderCell>
            <asp:TableCell>
                <asp:DropDownList ID="VisitorsDropDownList" runat="server" Font-Size="Large" OnSelectedIndexChanged="VisitorsOnSelectedIndexChanged" AutoPostBack="True" Width="400">
                </asp:DropDownList>
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
            <asp:TableHeaderCell Font-Size="Large">Person you are visiting</asp:TableHeaderCell>
            <asp:TableCell>
                <asp:DropDownList ID="EmployeesDropDownList" runat="server" Font-Size="Large" Width="400">
                </asp:DropDownList>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell>
                <asp:Button ID="submitButton" runat="server" Text="Submit" OnClick="SubmitButton_Click" OnClientClick="ShowWaitMessage()"  Font-Size="X-Large"/>
                <div style="background-color: Red; display: none;" id="WaitMessage"> Please wait...  </div>
            </asp:TableCell>
            <asp:TableHeaderCell ID="submitMessage" runat="server" HorizontalAlign="Left"></asp:TableHeaderCell>
        </asp:TableRow>
    </asp:Table>
</asp:Content>