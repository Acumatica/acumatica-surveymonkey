<%@ Page Language="C#" MasterPageFile="~/MasterPages/ListView.master" AutoEventWireup="true" ValidateRequest="false" CodeFile="MK502000.aspx.cs" Inherits="Page_MK502000" Title="Untitled Page" %>
<%@ MasterType VirtualPath="~/MasterPages/ListView.master" %>

<asp:Content ID="cont1" ContentPlaceHolderID="phDS" Runat="Server">
	<px:PXDataSource ID="ds" runat="server" Visible="True" Width="100%" PrimaryView="SurveyList" TypeName="PXSurveyMonkeyMKExt.CaseSurveyResponseProcessing">
	</px:PXDataSource>
</asp:Content>
<asp:Content ID="cont2" ContentPlaceHolderID="phL" Runat="Server">
	<px:PXGrid ID="grid" runat="server" DataSourceID="ds" Style="z-index: 100" 
		Width="100%" Height="150px" SkinID="PrimaryInquire" TabIndex="2300" SyncPosition="True">
		<Levels>
			<px:PXGridLevel DataMember="SurveyList">
                <Columns>
                    <px:PXGridColumn AllowNull="False" DataField="Selected" TextAlign="Center" Type="CheckBox" Width="40px" AllowCheckAll="true" >
                    </px:PXGridColumn>
                    <px:PXGridColumn DataField="CaseClassID" Width="300px" DisplayMode="Hint">
                    </px:PXGridColumn>
                    <px:PXGridColumn DataField="UsrSurveyID">
                    </px:PXGridColumn>
                    <px:PXGridColumn DataField="UsrLastSurveySyncDate" Width="200px" DisplayFormat="g">
                    </px:PXGridColumn>
                </Columns>
			</px:PXGridLevel>
		</Levels>
		<AutoSize Container="Window" Enabled="True" MinHeight="150" />
	</px:PXGrid>
</asp:Content>