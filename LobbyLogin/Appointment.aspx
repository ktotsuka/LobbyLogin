﻿<%@ Page Title="BAV Lobby Login" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Appointment.aspx.cs" Inherits="LobbyLogin.Appointment" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server" >
    <asp:Table runat="server">
        <asp:TableRow >
            <asp:TableHeaderCell Font-Size="Large">Do you have an appointment with a specific employee? </asp:TableHeaderCell>
        </asp:TableRow>
    </asp:Table>
    <asp:Table runat="server">
        <asp:TableRow>
            <asp:TableCell>
                <asp:Button ID="YesButton" runat="server" Text="Yes" Width="150" Height="75" OnClick="YesButton_Click" Font-Size="X-Large" />
            </asp:TableCell>
            <asp:TableHeaderCell Font-Size="Large">&nbsp &nbsp</asp:TableHeaderCell>
            <asp:TableCell>
                <asp:Button ID="NoButton" runat="server" Text="No" Width="150" Height="75" OnClick="NoButton_Click" Font-Size="X-Large" />
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>
</asp:Content>
