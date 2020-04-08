<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ConfidentialityAgreement.aspx.cs" Inherits="LobbyLogin.ConfidentialityAgreement" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server" >
    <div>
        <h1>&nbsp &nbsp &nbsp &nbsp &nbsp &nbsp &nbsp &nbsp &nbsp &nbsp &nbsp BAV Guest Sign-in</h1>
    </div>
    <hr>
    <div>
        <h1>VISITOR CONFIDENTIALITY AGREEMENT</h1>
    </div>
    <div>
        <h4>Visitor hereby agrees as follows:</h4>
    </div>
    <div>
        <h4>1. I understand, acknowledge and agree that Bastian Solutions Confidential Information is extremely valuable to it and is not generally known by others, including Bastian Solutions’ competitors. Bastian Solutions’ Confidential Information shall mean all information and/or products which are directly or indirectly disclosed to me under or in connection with my visit in whatever form (including verbally and electronically) that is not generally known to the public, and which includes, but is not limited to: information which concerns Bastian Solutions’ business or affairs, programs, software, products, processes, designs, inventions, discoveries, intellectual property and any other similarly legally protectible information. Bastian Solutions Confidential Information shall not include any information which (i) was in the public domain, or known by Visitor, at the time of such disclosure; or (ii) becomes part of the public domain after disclosure to Visitor, through no fault of Visitor.</h4>
    </div>
    <div>
        <h4>2. In consideration of being admitted into Bastian Solutions’ facility, I hereby agree to hold in the strictest confidence, and not use or disclose to others, the Confidential Information to which I may have access and/or which is disclosed to me. I will not photograph, videotape, or otherwise make any record of or preserve any information to which I may have access during my visit.</h4>
    </div>
    <div>
        <h4>3. This Agreement shall be governed by, construed, and interpreted under the laws of the State of Indiana.</h4>
    </div>
    <asp:Table runat="server">
        <asp:TableRow>
            <asp:TableCell>
                <asp:Button ID="AgreeButton" runat="server" Text="Agree" Width="150" Height="75" OnClick="AgreeButton_Click" Font-Size="X-Large" UseSubmitBehavior="false" OnClientClick="this.disabled='true'; this.value='please wait';" />
            </asp:TableCell>
            <asp:TableHeaderCell Font-Size="Large">&nbsp &nbsp</asp:TableHeaderCell>
            <asp:TableCell>
                <asp:Button ID="DisagreeButton" runat="server" Text="Disagree" Width="150" Height="75" OnClick="DisagreeButton_Click" Font-Size="X-Large" UseSubmitBehavior="false" OnClientClick="this.disabled='true'; this.value='please wait';"/>
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>
</asp:Content>
