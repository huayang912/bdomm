<%@ Page Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeFile="Users.aspx.cs" Inherits="Pages_Users"  Title="Users"%>
<asp:Content ID="Content1" ContentPlaceHolderID="PageHeaderContentPlaceHolder" runat="Server">Users</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageContentPlaceHolder" runat="Server">
  <div factory:flow="NewRow" xmlns:factory="urn:codeontime:app-factory">
    <div id="view1" runat="server"></div>
    <aquarium:DataViewExtender id="view1Extender" runat="server" TargetControlID="view1" Controller="Users" view="grid1" ShowInSummary="True" />
  </div>
  <div factory:flow="NewRow" style="padding-top:8px" xmlns:factory="urn:codeontime:app-factory">
    <div factory:activator="Tab|Clients">
      <div id="view2" runat="server"></div>
      <aquarium:DataViewExtender id="view2Extender" runat="server" TargetControlID="view2" Controller="Clients" view="grid1" FilterSource="view1Extender" FilterFields="ChangedByUserID" PageSize="5" AutoHide="Container" ShowModalForms="True" />
    </div>
    <div factory:activator="Tab|Client Contacts">
      <div id="view3" runat="server"></div>
      <aquarium:DataViewExtender id="view3Extender" runat="server" TargetControlID="view3" Controller="ClientContacts" view="grid1" FilterSource="view1Extender" FilterFields="ChangedByUserID" PageSize="5" AutoHide="Container" ShowModalForms="True" />
    </div>
    <div factory:activator="Tab|Projects">
      <div id="view4" runat="server"></div>
      <aquarium:DataViewExtender id="view4Extender" runat="server" TargetControlID="view4" Controller="Projects" view="grid1" FilterSource="view1Extender" FilterFields="CreatedByUserID,ChangedByUserID" PageSize="5" AutoHide="Container" ShowModalForms="True" />
    </div>
    <div factory:activator="Tab|Enquiries">
      <div id="view5" runat="server"></div>
      <aquarium:DataViewExtender id="view5Extender" runat="server" TargetControlID="view5" Controller="Enquiries" view="grid1" FilterSource="view1Extender" FilterFields="CreatedByUserID,ChangedByUserID" PageSize="5" AutoHide="Container" ShowModalForms="True" />
    </div>
    <div factory:activator="Tab|Roles">
      <div id="view6" runat="server"></div>
      <aquarium:DataViewExtender id="view6Extender" runat="server" TargetControlID="view6" Controller="Roles" view="grid1" FilterSource="view1Extender" FilterFields="ChangedByUserID" PageSize="5" AutoHide="Container" ShowModalForms="True" />
    </div>
    <div factory:activator="Tab|Quotations">
      <div id="view7" runat="server"></div>
      <aquarium:DataViewExtender id="view7Extender" runat="server" TargetControlID="view7" Controller="Quotations" view="grid1" FilterSource="view1Extender" FilterFields="CreatedByUserID,ChangedByUserID" PageSize="5" AutoHide="Container" ShowModalForms="True" />
    </div>
    <div factory:activator="Tab|Contacts">
      <div id="view8" runat="server"></div>
      <aquarium:DataViewExtender id="view8Extender" runat="server" TargetControlID="view8" Controller="Contacts" view="grid1" FilterSource="view1Extender" FilterFields="CreatedByUserID,ChangedByUserID" PageSize="5" AutoHide="Container" ShowModalForms="True" />
    </div>
    <div factory:activator="Tab|Bank Details">
      <div id="view9" runat="server"></div>
      <aquarium:DataViewExtender id="view9Extender" runat="server" TargetControlID="view9" Controller="BankDetails" view="grid1" FilterSource="view1Extender" FilterFields="ChangedByUserId" PageSize="5" AutoHide="Container" ShowModalForms="True" />
    </div>
    <div factory:activator="Tab|Certificates">
      <div id="view10" runat="server"></div>
      <aquarium:DataViewExtender id="view10Extender" runat="server" TargetControlID="view10" Controller="Certificates" view="grid1" FilterSource="view1Extender" FilterFields="ChangedByUserID" PageSize="5" AutoHide="Container" ShowModalForms="True" />
    </div>
    <div factory:activator="Tab|Client Contacts Notes">
      <div id="view11" runat="server"></div>
      <aquarium:DataViewExtender id="view11Extender" runat="server" TargetControlID="view11" Controller="ClientContactsNotes" view="grid1" FilterSource="view1Extender" FilterFields="ChangedByUserId" PageSize="5" AutoHide="Container" ShowModalForms="True" />
    </div>
    <div factory:activator="Tab|Client Project Contacts">
      <div id="view12" runat="server"></div>
      <aquarium:DataViewExtender id="view12Extender" runat="server" TargetControlID="view12" Controller="ClientProjectContacts" view="grid1" FilterSource="view1Extender" FilterFields="ChangedByUserID" PageSize="5" AutoHide="Container" ShowModalForms="True" />
    </div>
    <div factory:activator="Tab|Client Purchase Orders">
      <div id="view13" runat="server"></div>
      <aquarium:DataViewExtender id="view13Extender" runat="server" TargetControlID="view13" Controller="ClientPurchaseOrders" view="grid1" FilterSource="view1Extender" FilterFields="CreatedByUserID,ChangedByUserID" PageSize="5" AutoHide="Container" ShowModalForms="True" />
    </div>
    <div factory:activator="Tab|Clients Events">
      <div id="view14" runat="server"></div>
      <aquarium:DataViewExtender id="view14Extender" runat="server" TargetControlID="view14" Controller="ClientsEvents" view="grid1" FilterSource="view1Extender" FilterFields="ChangedByUserID" PageSize="5" AutoHide="Container" ShowModalForms="True" />
    </div>
    <div factory:activator="Tab|Clients Financial">
      <div id="view15" runat="server"></div>
      <aquarium:DataViewExtender id="view15Extender" runat="server" TargetControlID="view15" Controller="ClientsFinancial" view="grid1" FilterSource="view1Extender" FilterFields="ChangedByUserID" PageSize="5" AutoHide="Container" ShowModalForms="True" />
    </div>
    <div factory:activator="Tab|Clients Notes">
      <div id="view16" runat="server"></div>
      <aquarium:DataViewExtender id="view16Extender" runat="server" TargetControlID="view16" Controller="ClientsNotes" view="grid1" FilterSource="view1Extender" FilterFields="ChangedByUserID" PageSize="5" AutoHide="Container" ShowModalForms="True" />
    </div>
    <div factory:activator="Tab|Contact Roles">
      <div id="view17" runat="server"></div>
      <aquarium:DataViewExtender id="view17Extender" runat="server" TargetControlID="view17" Controller="ContactRoles" view="grid1" FilterSource="view1Extender" FilterFields="ChangedByUserID" PageSize="5" AutoHide="Container" ShowModalForms="True" />
    </div>
    <div factory:activator="Tab|Contacts Comms Notes">
      <div id="view18" runat="server"></div>
      <aquarium:DataViewExtender id="view18Extender" runat="server" TargetControlID="view18" Controller="ContactsCommsNotes" view="grid1" FilterSource="view1Extender" FilterFields="ChangedByUserID" PageSize="5" AutoHide="Container" ShowModalForms="True" />
    </div>
    <div factory:activator="Tab|Contacts Next Of Kin">
      <div id="view19" runat="server"></div>
      <aquarium:DataViewExtender id="view19Extender" runat="server" TargetControlID="view19" Controller="ContactsNextOfKin" view="grid1" FilterSource="view1Extender" FilterFields="ChangedByUserID" PageSize="5" AutoHide="Container" ShowModalForms="True" />
    </div>
    <div factory:activator="Tab|Contacts Notes">
      <div id="view20" runat="server"></div>
      <aquarium:DataViewExtender id="view20Extender" runat="server" TargetControlID="view20" Controller="ContactsNotes" view="grid1" FilterSource="view1Extender" FilterFields="ChangedByUserID" PageSize="5" AutoHide="Container" ShowModalForms="True" />
    </div>
    <div factory:activator="Tab|Contacts Travel">
      <div id="view21" runat="server"></div>
      <aquarium:DataViewExtender id="view21Extender" runat="server" TargetControlID="view21" Controller="ContactsTravel" view="grid1" FilterSource="view1Extender" FilterFields="ChangedByUserID" PageSize="5" AutoHide="Container" ShowModalForms="True" />
    </div>
    <div factory:activator="Tab|Email Addresses">
      <div id="view22" runat="server"></div>
      <aquarium:DataViewExtender id="view22Extender" runat="server" TargetControlID="view22" Controller="EmailAddresses" view="grid1" FilterSource="view1Extender" FilterFields="ChangedByUserID" PageSize="5" AutoHide="Container" ShowModalForms="True" />
    </div>
    <div factory:activator="Tab|Employment History">
      <div id="view23" runat="server"></div>
      <aquarium:DataViewExtender id="view23Extender" runat="server" TargetControlID="view23" Controller="EmploymentHistory" view="grid1" FilterSource="view1Extender" FilterFields="ChangedByUserID" PageSize="5" AutoHide="Container" ShowModalForms="True" />
    </div>
    <div factory:activator="Tab|Enquiry Lines">
      <div id="view24" runat="server"></div>
      <aquarium:DataViewExtender id="view24Extender" runat="server" TargetControlID="view24" Controller="EnquiryLines" view="grid1" FilterSource="view1Extender" FilterFields="ChangedByUserID" PageSize="5" AutoHide="Container" ShowModalForms="True" />
    </div>
    <div factory:activator="Tab|Next Of Kin">
      <div id="view25" runat="server"></div>
      <aquarium:DataViewExtender id="view25Extender" runat="server" TargetControlID="view25" Controller="NextOfKin" view="grid1" FilterSource="view1Extender" FilterFields="ChangedByUserID" PageSize="5" AutoHide="Container" ShowModalForms="True" />
    </div>
    <div factory:activator="Tab|Passports">
      <div id="view26" runat="server"></div>
      <aquarium:DataViewExtender id="view26Extender" runat="server" TargetControlID="view26" Controller="Passports" view="grid1" FilterSource="view1Extender" FilterFields="ChangedByUserID" PageSize="5" AutoHide="Container" ShowModalForms="True" />
    </div>
    <div factory:activator="Tab|Telephone Numbers">
      <div id="view27" runat="server"></div>
      <aquarium:DataViewExtender id="view27Extender" runat="server" TargetControlID="view27" Controller="TelephoneNumbers" view="grid1" FilterSource="view1Extender" FilterFields="ChangedByUserID" PageSize="5" AutoHide="Container" ShowModalForms="True" />
    </div>
    <div factory:activator="Tab|User In Role">
      <div id="view28" runat="server"></div>
      <aquarium:DataViewExtender id="view28Extender" runat="server" TargetControlID="view28" Controller="UserInRole" view="grid1" FilterSource="view1Extender" FilterFields="UserID,ModifiedBy" PageSize="5" AutoHide="Container" ShowModalForms="True" />
    </div>
    <div factory:activator="Tab|Visas">
      <div id="view29" runat="server"></div>
      <aquarium:DataViewExtender id="view29Extender" runat="server" TargetControlID="view29" Controller="Visas" view="grid1" FilterSource="view1Extender" FilterFields="ChangedByUserID" PageSize="5" AutoHide="Container" ShowModalForms="True" />
    </div>
  </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="SideBarPlaceHolder" runat="Server">
  <div class="TaskBox About">
    <div class="Inner">
      <div class="Header">About</div>
      <div class="Value">This page allows users management.</div>
    </div>
  </div>
</asp:Content>