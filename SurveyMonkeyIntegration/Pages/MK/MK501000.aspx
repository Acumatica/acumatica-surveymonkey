<%@ Page Language="C#" MasterPageFile="~/MasterPages/FormDetail.master" AutoEventWireup="true" ValidateRequest="false" CodeFile="MK501000.aspx.cs" Inherits="Page_MK501000" Title="Untitled Page" %>
<%@ MasterType VirtualPath="~/MasterPages/FormDetail.master" %>

<asp:Content ID="cont1" ContentPlaceHolderID="phDS" Runat="Server">
	<px:PXDataSource ID="ds" runat="server" Visible="True" Width="100%" 
        PrimaryView="Filter" 
        TypeName="PXSurveyMonkeyMKExt.CaseSurveyProcessing">
	</px:PXDataSource>
</asp:Content>
<asp:Content ID="cont2" ContentPlaceHolderID="phF" Runat="Server">
	<px:PXFormView ID="form" runat="server" DataSourceID="ds" Style="z-index: 100" 
		Width="100%" DataMember="Filter" TabIndex="100">
        <Template>
            <px:PXLayoutRule runat="server" StartRow="True" LabelsWidth="XM" 
                ControlSize="SM">
            </px:PXLayoutRule>
            <px:PXNumberEdit ID="edNumberOfDaysMostRecentCaseCreatedIn" runat="server" 
                DataField="NumberOfDaysMostRecentCaseCreatedIn" CommitChanges="True">
            </px:PXNumberEdit>
            <px:PXNumberEdit runat="server" DataField="NumberOfDays" ID="edNumberOfDays" 
                CommitChanges="True">
            </px:PXNumberEdit>
        </Template>
	</px:PXFormView>
</asp:Content>
<asp:Content ID="cont3" ContentPlaceHolderID="phG" Runat="Server">
	<px:PXGrid ID="grid" runat="server" DataSourceID="ds" Style="z-index: 100" 
		Width="100%" Height="150px" SkinID="Details" TabIndex="1100">		
		<Levels>
<px:PXGridLevel DataKeyNames="ContactID,CaseOwnerID" DataMember="Records">
    <RowTemplate>
        <px:PXCheckBox ID="edSelected" runat="server" DataField="Selected" 
            Text="Selected">
        </px:PXCheckBox>
        <px:PXTextEdit ID="edFullName" runat="server" DataField="FullName">
        </px:PXTextEdit>
        <px:PXDateTimeEdit ID="edUsrLastSurveyed" runat="server" 
            DataField="UsrLastSurveyed">
        </px:PXDateTimeEdit>
        <px:PXTextEdit ID="edUserFullName" runat="server" DataField="UserFullName">
        </px:PXTextEdit>
        <px:PXDropDown ID="edTitle" runat="server" DataField="Title">
        </px:PXDropDown>
        <px:PXTextEdit ID="edDisplayName" runat="server" DataField="DisplayName">
        </px:PXTextEdit>
        <px:PXTextEdit ID="edEMail" runat="server" DataField="EMail">
        </px:PXTextEdit>
        <px:PXTextEdit ID="edPhone1" runat="server" DataField="Phone1">
        </px:PXTextEdit>
        <px:PXDropDown ID="edPhone1Type" runat="server" DataField="Phone1Type">
        </px:PXDropDown>
        <px:PXTextEdit ID="edPhone2" runat="server" DataField="Phone2">
        </px:PXTextEdit>
        <px:PXDropDown ID="edPhone2Type" runat="server" DataField="Phone2Type">
        </px:PXDropDown>
        <px:PXTextEdit ID="edPhone3" runat="server" DataField="Phone3">
        </px:PXTextEdit>
        <px:PXDropDown ID="edPhone3Type" runat="server" DataField="Phone3Type">
        </px:PXDropDown>
        <px:PXNumberEdit ID="edSurveyCaseCount" runat="server" 
            DataField="SurveyCaseCount">
        </px:PXNumberEdit>
        <px:PXTextEdit ID="edCaseCD" runat="server" DataField="CaseCD">
        </px:PXTextEdit>
        <px:PXNumberEdit ID="edContactID" runat="server" DataField="ContactID">
        </px:PXNumberEdit>
        <px:PXDateTimeEdit ID="edRecentCaseCreationDate" runat="server" 
            DataField="RecentCaseCreationDate">
        </px:PXDateTimeEdit>
        <px:PXDateTimeEdit ID="edRecentCaseResolutionDate" runat="server" 
            DataField="RecentCaseResolutionDate">
        </px:PXDateTimeEdit>
        <px:PXTextEdit ID="edTechUserName" runat="server" DataField="TechUserName">
        </px:PXTextEdit>
        <px:PXTextEdit ID="edTechFirstName" runat="server" DataField="TechFirstName">
        </px:PXTextEdit>
        <px:PXTextEdit ID="edTechLastName" runat="server" DataField="TechLastName">
        </px:PXTextEdit>
        <px:PXTextEdit ID="edCaseClassID" runat="server" DataField="CaseClassID">
        </px:PXTextEdit>
        <px:PXTextEdit ID="edPartnerGroup" runat="server" DataField="PartnerGroup">
        </px:PXTextEdit>
        <px:PXTextEdit ID="edSurveyUrl" runat="server" AlreadyLocalized="False" DataField="SurveyUrl">
        </px:PXTextEdit>
    </RowTemplate>
    <Columns>
        <px:PXGridColumn DataField="Selected" TextAlign="Center" Type="CheckBox" 
            Width="80px">
        </px:PXGridColumn>
        <px:PXGridColumn DataField="FullName" Width="200px">
        </px:PXGridColumn>
        <px:PXGridColumn DataField="UsrLastSurveyed" Width="90px">
        </px:PXGridColumn>
        <px:PXGridColumn DataField="UserFullName" Width="120px">
        </px:PXGridColumn>
        <px:PXGridColumn DataField="Title" Width="200px">
        </px:PXGridColumn>
        <px:PXGridColumn DataField="DisplayName" Width="120px">
        </px:PXGridColumn>
        <px:PXGridColumn DataField="EMail" Width="200px">
        </px:PXGridColumn>
        <px:PXGridColumn DataField="Phone1" Width="200px">
        </px:PXGridColumn>
        <px:PXGridColumn DataField="Phone1Type">
        </px:PXGridColumn>
        <px:PXGridColumn DataField="Phone2" Width="200px">
        </px:PXGridColumn>
        <px:PXGridColumn DataField="Phone2Type">
        </px:PXGridColumn>
        <px:PXGridColumn DataField="Phone3" Width="200px">
        </px:PXGridColumn>
        <px:PXGridColumn DataField="Phone3Type">
        </px:PXGridColumn>
        <px:PXGridColumn DataField="SurveyCaseCount" TextAlign="Right">
        </px:PXGridColumn>
        <px:PXGridColumn DataField="CaseCD" Width="200px">
        </px:PXGridColumn>
        <px:PXGridColumn DataField="ContactID" TextAlign="Right">
        </px:PXGridColumn>
        <px:PXGridColumn DataField="RecentCaseCreationDate" Width="90px">
        </px:PXGridColumn>
        <px:PXGridColumn DataField="RecentCaseResolutionDate" Width="90px">
        </px:PXGridColumn>
        <px:PXGridColumn DataField="TechUserName">
        </px:PXGridColumn>
        <px:PXGridColumn DataField="TechFirstName">
        </px:PXGridColumn>
        <px:PXGridColumn DataField="TechLastName">
        </px:PXGridColumn>
        <px:PXGridColumn DataField="PartnerGroup" Width="100px">
        </px:PXGridColumn>
        <px:PXGridColumn DataField="CaseClassID" Width="100px">
        </px:PXGridColumn>
        <px:PXGridColumn DataField="SurveyUrl" Width="150px">
        </px:PXGridColumn>
    </Columns>
            </px:PXGridLevel>
</Levels>

		<AutoSize Container="Window" Enabled="True" MinHeight="150" />
		<ActionBar ActionsText="True">
		</ActionBar>
	</px:PXGrid>
</asp:Content>
