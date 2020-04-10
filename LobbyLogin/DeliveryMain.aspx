<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="DeliveryMain.aspx.cs" Inherits="LobbyLogin.DeliveryMain" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server" >
    <style>
        div.big {
            font-size: 75px;
        }
    </style>
    <div>
        <div class="big">Delivery Notice</div>
    </div>
    <h1>Pick-up or drop-off</h1>
    <br>
    <asp:RadioButton ID="PickUpRadioButton" runat="server" GroupName="pickup-dropoff" Text="Pick-up" Font-Size="X-Large" />
    <br>
    <asp:RadioButton ID="DropOffRadioButton" runat="server" GroupName="pickup-dropoff" Text="Drop-off" Font-Size="X-Large"  />
    <br>

    <h1>Delivery method</h1>
    <br>
    <asp:RadioButton ID="ForkLiftRequiredRadioButton" runat="server" GroupName="forklift-required" Text="LTL/Dedicated (fork-lift required)" Font-Size="X-Large" />
    <br>
    <asp:RadioButton ID="ForkLiftNotRequiredRadioButton" runat="server" GroupName="forklift-required" Text="Other" Font-Size="X-Large"  />
    <br>
    <br>

    <asp:Table runat="server">
        <asp:TableRow>
            <asp:TableHeaderCell Font-Size="Large">&nbsp &nbsp &nbsp</asp:TableHeaderCell>
            <asp:TableCell>
                <asp:Button ID="notifyButton" runat="server" Text="Notify" Width="200" Height="100" OnClick="NotifyButton_Click" Font-Size="XX-Large" UseSubmitBehavior="false" OnClientClick="this.disabled='true'; this.value='please wait';" />
            </asp:TableCell>
            <asp:TableHeaderCell ID="notifyMessage" runat="server" HorizontalAlign="Left" ForeColor="Red" Font-Size="Large"></asp:TableHeaderCell>
        </asp:TableRow>
    </asp:Table>
</asp:Content>
