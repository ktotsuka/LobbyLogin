﻿<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="WaitList.aspx.cs" Inherits="LobbyLogin.WaitList" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server" >
    <meta http-equiv="Refresh" content="30;url=ConfidentialityAgreement.aspx" />
    <div>
        <h1>&nbsp &nbsp &nbsp &nbsp &nbsp &nbsp &nbsp &nbsp &nbsp &nbsp &nbsp BAV Guest Sign-in</h1>
    </div>
    <hr>
    <asp:Table runat="server" >
        <asp:TableRow >
            <asp:TableHeaderCell Font-Size="Large">List of visitors to be acknowledged</asp:TableHeaderCell>
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
