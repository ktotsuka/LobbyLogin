<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="DeliveryMain.aspx.cs" Inherits="LobbyLogin.DeliveryMain" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server" >
    <style>
        div.big {
            font-size: 75px;
        }
    </style>
    <div>
        <div class="big">For delivery, push</div>
        <div class="big">the button to notify</div>
    </div>
    <asp:Table runat="server">
        <asp:TableRow>
            <asp:TableHeaderCell Font-Size="Large">&nbsp &nbsp &nbsp &nbsp &nbsp &nbsp &nbsp &nbsp &nbsp &nbsp &nbsp &nbsp &nbsp &nbsp &nbsp &nbsp &nbsp &nbsp &nbsp &nbsp &nbsp</asp:TableHeaderCell>
            <asp:TableCell>
                <asp:Button ID="notifyButton" runat="server" Text="Notify" Width="300" Height="300" OnClick="NotifyButton_Click" Font-Size="XX-Large" UseSubmitBehavior="false" OnClientClick="this.disabled='true'; this.value='please wait';" />
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>
</asp:Content>
