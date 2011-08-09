<%@ Page Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeFile="Contacts.aspx.cs" Inherits="Pages_Contacts"  Title="Contacts"%>
<asp:Content ID="Content1" ContentPlaceHolderID="PageHeaderContentPlaceHolder" runat="Server">Contacts</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageContentPlaceHolder" runat="Server">
  <div factory:flow="NewRow" xmlns:factory="urn:codeontime:app-factory">
    <div id="view1" runat="server"></div>
    <aquarium:DataViewExtender id="view1Extender" runat="server" TargetControlID="view1" Controller="Contacts" view="grid1" ShowInSummary="True" />
  </div>
  <div factory:flow="NewRow" style="padding-top:8px" xmlns:factory="urn:codeontime:app-factory">
    <div factory:activator="Tab|Bank Details">
      <div id="view2" runat="server"></div>
      <aquarium:DataViewExtender id="view2Extender" runat="server" TargetControlID="view2" Controller="BankDetails" view="grid1" FilterSource="view1Extender" FilterFields="ContactID" PageSize="5" AutoHide="Container" ShowModalForms="True" />
    </div>
    <div factory:activator="Tab|Certificates">
      <div id="view3" runat="server"></div>
      <aquarium:DataViewExtender id="view3Extender" runat="server" TargetControlID="view3" Controller="Certificates" view="grid1" FilterSource="view1Extender" FilterFields="ContactID" PageSize="5" AutoHide="Container" ShowModalForms="True" />
    </div>
    <div factory:activator="Tab|Contact Roles">
      <div id="view4" runat="server"></div>
      <aquarium:DataViewExtender id="view4Extender" runat="server" TargetControlID="view4" Controller="ContactRoles" view="grid1" FilterSource="view1Extender" FilterFields="ContactID" PageSize="5" AutoHide="Container" ShowModalForms="True" />
    </div>
    <div factory:activator="Tab|Contacts Comms Notes">
      <div id="view5" runat="server"></div>
      <aquarium:DataViewExtender id="view5Extender" runat="server" TargetControlID="view5" Controller="ContactsCommsNotes" view="grid1" FilterSource="view1Extender" FilterFields="ContactID" PageSize="5" AutoHide="Container" ShowModalForms="True" />
    </div>
    <div factory:activator="Tab|Contacts Next Of Kin">
      <div id="view6" runat="server"></div>
      <aquarium:DataViewExtender id="view6Extender" runat="server" TargetControlID="view6" Controller="ContactsNextOfKin" view="grid1" FilterSource="view1Extender" FilterFields="ContactID" PageSize="5" AutoHide="Container" ShowModalForms="True" />
    </div>
    <div factory:activator="Tab|Contacts Notes">
      <div id="view7" runat="server"></div>
      <aquarium:DataViewExtender id="view7Extender" runat="server" TargetControlID="view7" Controller="ContactsNotes" view="grid1" FilterSource="view1Extender" FilterFields="ContactID" PageSize="5" AutoHide="Container" ShowModalForms="True" />
    </div>
    <div factory:activator="Tab|Contacts Travel">
      <div id="view8" runat="server"></div>
      <aquarium:DataViewExtender id="view8Extender" runat="server" TargetControlID="view8" Controller="ContactsTravel" view="grid1" FilterSource="view1Extender" FilterFields="ContactID" PageSize="5" AutoHide="Container" ShowModalForms="True" />
    </div>
    <div factory:activator="Tab|Email Addresses">
      <div id="view9" runat="server"></div>
      <aquarium:DataViewExtender id="view9Extender" runat="server" TargetControlID="view9" Controller="EmailAddresses" view="grid1" FilterSource="view1Extender" FilterFields="ContactID" PageSize="5" AutoHide="Container" ShowModalForms="True" />
    </div>
    <div factory:activator="Tab|Employment History">
      <div id="view10" runat="server"></div>
      <aquarium:DataViewExtender id="view10Extender" runat="server" TargetControlID="view10" Controller="EmploymentHistory" view="grid1" FilterSource="view1Extender" FilterFields="ContactID" PageSize="5" AutoHide="Container" ShowModalForms="True" />
    </div>
    <div factory:activator="Tab|Next Of Kin">
      <div id="view11" runat="server"></div>
      <aquarium:DataViewExtender id="view11Extender" runat="server" TargetControlID="view11" Controller="NextOfKin" view="grid1" FilterSource="view1Extender" FilterFields="ContactID" PageSize="5" AutoHide="Container" ShowModalForms="True" />
    </div>
    <div factory:activator="Tab|Passports">
      <div id="view12" runat="server"></div>
      <aquarium:DataViewExtender id="view12Extender" runat="server" TargetControlID="view12" Controller="Passports" view="grid1" FilterSource="view1Extender" FilterFields="ContactID" PageSize="5" AutoHide="Container" ShowModalForms="True" />
    </div>
    <div factory:activator="Tab|Telephone Numbers">
      <div id="view13" runat="server"></div>
      <aquarium:DataViewExtender id="view13Extender" runat="server" TargetControlID="view13" Controller="TelephoneNumbers" view="grid1" FilterSource="view1Extender" FilterFields="ContactID" PageSize="5" AutoHide="Container" ShowModalForms="True" />
    </div>
    <div factory:activator="Tab|Visas">
      <div id="view14" runat="server"></div>
      <aquarium:DataViewExtender id="view14Extender" runat="server" TargetControlID="view14" Controller="Visas" view="grid1" FilterSource="view1Extender" FilterFields="ContactID" PageSize="5" AutoHide="Container" ShowModalForms="True" />
    </div>
  </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="SideBarPlaceHolder" runat="Server">
  <div class="TaskBox About">
    <div class="Inner">
      <div class="Header">About</div>
      <div class="Value">This page allows contacts management.</div>
    </div>
  </div>
</asp:Content>