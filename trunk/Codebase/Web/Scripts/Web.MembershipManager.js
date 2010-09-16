/// <reference name="MicrosoftAjax.debug.js" />
/// <reference name="AjaxControlToolkit.ExtenderBase.BaseScripts.js" assembly="AjaxControlToolkit" />
/// <reference name="AjaxControlToolkit.Common.Common.js" assembly="AjaxControlToolkit" />
/// <reference name="AjaxControlToolkit.DropDown.DropDownBehavior.js" assembly="AjaxControlToolkit"/>
/// <reference name="AjaxControlToolkit.ModalPopup.ModalPopupBehavior.js" assembly="AjaxControlToolkit"/>
/// <reference name="AjaxControlToolkit.AlwaysVisibleControl.AlwaysVisibleControlBehavior.js" assembly="AjaxControlToolkit"/>
/// <reference name="AjaxControlToolkit.PopupControl.PopupControlBehavior.js" assembly="AjaxControlToolkit"/>
/// <reference name="AjaxControlToolkit.Calendar.CalendarBehavior.js" assembly="AjaxControlToolkit"/>
/// <reference name="AjaxControlToolkit.Tabs.Tabs.js" assembly="AjaxControlToolkit"/>
/// <reference path="Web.DataViewResources.js"/>
/// <reference path="Web.MembershipResources.js" />

Type.registerNamespace("Web");

Web.MembershipManager = function(element) {
    Web.MembershipManager.initializeBase(this, [element]);
}

Web.MembershipManager.prototype = {
    get_servicePath: function() {
        return this._servicePath;
    },
    set_servicePath: function set_servicePath(value) {
        this._servicePath = value;
    },
    get_baseUrl: function() {
        return this._baseUrl;
    },
    set_baseUrl: function(value) {
        this._baseUrl = value;
    },
    initialize: function() {
        Web.MembershipManager.callBaseMethod(this, 'initialize');
    },
    dispose: function() {
        Web.MembershipManager.callBaseMethod(this, 'dispose');
    },
    updated: function() {
        Web.MembershipManager.callBaseMethod(this, 'updated');
        var elem = this.get_element();
        Sys.UI.DomElement.addCssClass(elem, 'MembershipManager');

        var sb = new Sys.StringBuilder();
        sb.append('<div class="TabContainer" id="LoginView1_SecurityTabs" style="visibility:hidden;">');
        sb.append('<div id="LoginView1_SecurityTabs_header">');
        sb.append(String.format('<span id="__tab_LoginView1_SecurityTabs_UsersTab">{0}</span><span id="__tab_LoginView1_SecurityTabs_RolesTab">{1}</span>', Web.MembershipResources.Manager.UsersTab, Web.MembershipResources.Manager.RolesTab));
        sb.append('</div><div id="LoginView1_SecurityTabs_body">');
        sb.append('<div id="LoginView1_SecurityTabs_UsersTab" style="display:none;visibility:hidden;">');
        sb.append('<div id="LoginView1_SecurityTabs_UsersTab_Users"></div>');
        sb.append('</div><div id="LoginView1_SecurityTabs_RolesTab" style="display:none;visibility:hidden;">');
        sb.append('<div id="LoginView1_SecurityTabs_RolesTab_Roles"></div>');
        sb.append('<div id="LoginView1_SecurityTabs_RolesTab_UsersInRoles" style="margin-top: 8px"></div>');
        sb.append('</div>');
        sb.append('</div>');
        sb.append('</div>');
        elem.innerHTML = sb.toString();

        $create(Web.DataView, { "baseUrl": this.get_baseUrl(), "controller": "aspnet_Membership", "cookie": "85d34761-6069-46a2-ad7a-e7f15ae61889", "id": "LoginView1_SecurityTabs_UsersTab_UsersExtender", "pageSize": 10, "servicePath": this.get_servicePath(), "showActionBar": true, "viewId": null }, null, null, $get("LoginView1_SecurityTabs_UsersTab_Users", elem));
        $create(AjaxControlToolkit.TabPanel, { "headerTab": $get("__tab_LoginView1_SecurityTabs_UsersTab", elem) }, null, { "owner": "LoginView1_SecurityTabs" }, $get("LoginView1_SecurityTabs_UsersTab", elem));
        $create(Web.DataView, { "baseUrl": this.get_baseUrl(), "controller": "aspnet_Roles", "cookie": "34fdf582-de5b-4672-8722-5df4924c12cc", "id": "LoginView1_SecurityTabs_RolesTab_RolesExtender", "pageSize": 5, "servicePath": this.get_servicePath(), "showActionBar": true, "viewId": null }, null, null, $get("LoginView1_SecurityTabs_RolesTab_Roles", elem));
        $create(Web.DataView, { "baseUrl": this.get_baseUrl(), "controller": "aspnet_Membership", "cookie": "07b98b9c-8aa7-4276-b935-76170c65ffdf", "filterFields": "RoleId", "filterSource": "LoginView1_SecurityTabs_RolesTab_RolesExtender", "id": "LoginView1_SecurityTabs_RolesTab_UsersInRolesExtender", "pageSize": 5, "servicePath": this.get_servicePath(), "showActionBar": true, "viewId": "usersInRolesGrid" }, null, null, $get("LoginView1_SecurityTabs_RolesTab_UsersInRoles", elem));
        $create(AjaxControlToolkit.TabPanel, { "headerTab": $get("__tab_LoginView1_SecurityTabs_RolesTab", elem) }, null, { "owner": "LoginView1_SecurityTabs" }, $get("LoginView1_SecurityTabs_RolesTab", elem));
        $create(AjaxControlToolkit.TabContainer, { "activeTabIndex": 0, "clientStateField": $get("LoginView1_SecurityTabs_ClientState", elem) }, null, null, $get("LoginView1_SecurityTabs", elem));
    }
}

Web.MembershipManager.registerClass('Web.MembershipManager', Sys.UI.Behavior);

if (Sys.Extended && typeof(AjaxControlToolkit) == "undefined") AjaxControlToolkit = Sys.Extended.UI;

if (typeof (Sys) !== 'undefined') Sys.Application.notifyScriptLoaded();
