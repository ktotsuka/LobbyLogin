<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="WaitList.aspx.cs" Inherits="LobbyLogin.WaitList" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server" >
    <meta http-equiv="Refresh" content="30;url=ConfidentialityAgreement.aspx" />
    <asp:Table runat="server" >
        <asp:TableRow >
            <asp:TableHeaderCell Font-Size="Large">List of registered visitors to be acknowledged</asp:TableHeaderCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell>
                <asp:DropDownList ID="WaitingVisitDropDownList" runat="server" ></asp:DropDownList>
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>
    <p>&nbsp</p>
    <asp:Table runat="server" >
        <asp:TableRow>
            <asp:TableCell>
                <asp:Button ID="removeWaitingVisitButton" runat="server" Text="Remove" Width="150" Height="50" OnClick="RemoveWaitingVisitButton_Click" />
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>
</asp:Content>
