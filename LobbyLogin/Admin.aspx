﻿<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="Admin.aspx.cs" Inherits="LobbyLogin.AdminTool" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <hr>
    <asp:Table ID="AdminPasswordTable" runat="server" CellSpacing="10">
        <asp:TableRow>
            <asp:TableHeaderCell ID="adminPasswordLabel">Password</asp:TableHeaderCell>
            <asp:TableCell>
                <asp:TextBox ID="adminPassword" runat="server" AutoCompleteType="Disabled" />
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell>
                <asp:Button ID="submitButton" runat="server" Text="Submit" OnClick="AdminPasswordSubmitButton_Click" />
            </asp:TableCell><asp:TableHeaderCell ID="submitErrorMessage" runat="server" HorizontalAlign="Left" ForeColor="Red"></asp:TableHeaderCell>
        </asp:TableRow>
    </asp:Table>
    <asp:Table ID="AddEmployeeTable" runat="server" CellSpacing="10" Visible="false">
        <asp:TableRow>
            <asp:TableHeaderCell ID="lastNameLabel">Last name <span style="COLOR: red">(required)</span></asp:TableHeaderCell><asp:TableCell>
                <asp:TextBox ID="lastName" runat="server" MaxLength="50" />
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableHeaderCell ID="firstNameLabel">First name <span style="COLOR: red">(required)</span></asp:TableHeaderCell><asp:TableCell>
                <asp:TextBox ID="firstName" runat="server" MaxLength="50" />
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableHeaderCell ID="emailAddressLabel">Email address <span style="COLOR: red">(required)</span></asp:TableHeaderCell><asp:TableCell>
                <asp:TextBox ID="emailAddress" runat="server" MaxLength="50" />
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableHeaderCell ID="cellPhoneNumberLabel">Cell phone number <span style="COLOR: red">(required)</span></asp:TableHeaderCell><asp:TableCell>
                <asp:TextBox ID="cellPhoneNumber" runat="server" MaxLength="50" />
                <ajaxToolkit:MaskedEditExtender ID="PhoneNumberMaskedEditExtender" runat="server"
                    TargetControlID="cellPhoneNumber"
                    ClearMaskOnLostFocus="false"
                    MaskType="None"
                    Mask="(999)999-9999"
                    MessageValidatorTip="true"
                    InputDirection="LeftToRight"
                    ErrorTooltipEnabled="True" />
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell>
                <asp:Button ID="addEmployeeButton" runat="server" Text="Add an employee" OnClick="AddEmployeeButton_Click" />
            </asp:TableCell><asp:TableHeaderCell ID="addEmployeeErrorMessage" runat="server" HorizontalAlign="Left" ForeColor="Red"></asp:TableHeaderCell>
        </asp:TableRow>
    </asp:Table>
    <hr />
    <asp:Table ID="RemoveEmployeeTable" runat="server" CellSpacing="10" Visible="false">
        <asp:TableRow>
            <asp:TableCell>
                <asp:DropDownList ID="EmployeesDropDownList" runat="server"></asp:DropDownList>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell>
                <asp:Button ID="removeEmployeeButton" runat="server" Text="Remove an employee" OnClick="RemoveEmployeeButton_Click" />
            </asp:TableCell>
            <asp:TableHeaderCell ID="removeEmployeeMessage" runat="server" HorizontalAlign="Left" ForeColor="Red"></asp:TableHeaderCell>
        </asp:TableRow>
    </asp:Table>
    <hr />
        <asp:Table ID="RemoveVisitorTable" runat="server" CellSpacing="10" Visible="false">
        <asp:TableRow>
            <asp:TableCell>
                <asp:DropDownList ID="VisitorsDropDownList" runat="server"></asp:DropDownList>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell>
                <asp:Button ID="removeVisitorButton" runat="server" Text="Remove a visitor" OnClick="RemoveVisitorButton_Click" />
            </asp:TableCell>
            <asp:TableHeaderCell ID="removeVisitorMessage" runat="server" HorizontalAlign="Left" ForeColor="Red"></asp:TableHeaderCell>
        </asp:TableRow>
    </asp:Table>
    <hr />
    <asp:Table ID="RemoveVisitTable" runat="server" CellSpacing="10" Visible="false">
        <asp:TableRow>
            <asp:TableCell>
                <asp:DropDownList ID="VisitsDropDownList" runat="server"></asp:DropDownList>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell>
                <asp:Button ID="removeVisitButton" runat="server" Text="Remove a visit" OnClick="RemoveVisitButton_Click" />
            </asp:TableCell><asp:TableHeaderCell ID="removeVisitMessage" runat="server" HorizontalAlign="Left" ForeColor="Red"></asp:TableHeaderCell>
        </asp:TableRow>
    </asp:Table>
    <hr />
    <asp:Table ID="ExportTable" runat="server" CellSpacing="10" Visible="false">
        <asp:TableRow>
            <asp:TableCell>
                <asp:Button ID="ExportEmployeeButton" runat="server" Text="Export employees" OnClick="ExportEmployeesButton_Click" />
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell>
                <asp:Button ID="ExportVisitorButton" runat="server" Text="Export visitors" OnClick="ExportVisitorsButton_Click" />
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell>
                <asp:Button ID="ExportVisitButton" runat="server" Text="Export visits" OnClick="ExportVisitsButton_Click" />
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>
    <hr />
    <asp:Table ID="ImportTable" runat="server" CellSpacing="10" Visible="false">
        <asp:TableRow>
            <asp:TableCell>
                <asp:FileUpload id="ImportEmployeesFileUploadControl" runat="server" />
                <asp:Button ID="ImportEmployeesButton" runat="server" Text="Import employees" OnClick="ImportEmployeesButton_Click" />
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell>
                <asp:FileUpload id="ImportVisitorsFileUploadControl" runat="server" />
                <asp:Button ID="ImportVisitorsButton" runat="server" Text="Import visitors" OnClick="ImportVisitorsButton_Click" />
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell>
                <asp:FileUpload id="ImportVisitsFileUploadControl" runat="server" />
                <asp:Button ID="ImportVisitsButton" runat="server" Text="Import visits" OnClick="ImportVisitsButton_Click" />
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>
</asp:Content>
