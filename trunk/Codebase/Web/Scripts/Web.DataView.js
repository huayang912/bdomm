/// <reference name="MicrosoftAjax.debug.js" />
/// <reference name="AjaxControlToolkit.ExtenderBase.BaseScripts.js" assembly="AjaxControlToolkit" />
/// <reference name="AjaxControlToolkit.Common.Common.js" assembly="AjaxControlToolkit" />
/// <reference name="AjaxControlToolkit.ModalPopup.ModalPopupBehavior.js" assembly="AjaxControlToolkit"/>
/// <reference name="AjaxControlToolkit.AlwaysVisibleControl.AlwaysVisibleControlBehavior.js" assembly="AjaxControlToolkit"/>
/// <reference name="AjaxControlToolkit.PopupControl.PopupControlBehavior.js" assembly="AjaxControlToolkit"/>
/// <reference name="AjaxControlToolkit.Calendar.CalendarBehavior.js" assembly="AjaxControlToolkit"/>
/// <reference path="Web.DataViewResources.js"/>
/// <reference path="Web.Menu.js"/>
Type.registerNamespace("Web");

Web.DataViewMode = { View: 'View', Lookup: 'Lookup' };
Web.DataViewSelectionMode = { Single: 'Single', Multiple: 'Multiple' };
Web.DynamicExpressionScope = { Field: 0, ViewRowStyle: 1, CategoryVisibility: 2, DataFieldVisibility: 3, DefaultValues: 4 };
Web.AutoHideMode = { Nothing: 0, Self: 1, Container: 2 };
Web.DynamicExpressionType = { RegularExpression: 0, ClientScript: 1, ServerExpression: 2, CSharp: 3, VisualBasic: 4, Any: 4 };
Web.DataViewAggregates = ['None', 'Sum', 'Count', 'Avg', 'Max', 'Min'];

Sys.StringBuilder.prototype.appendFormat = function(fmt, args) {
    this.append(String._toFormattedString(false, arguments));
}

String.isNullOrEmpty = function(s) {
    return s == null || s.length == 0;
}

String.isBlank = function(s) {
    return s == null || typeof (s) == 'string' && s.match(Web.DataView._blankRegex) != null;
}

Web.DataView = function(element) {
    Web.DataView.initializeBase(this, [element]);
    this._controller = null;
    this._viewId = null;
    this._servicePath = null;
    this._showActionBar = true;
    this._showDescription = true;
    this._showViewSelector = true;
    this._baseUrl = null;
    this._pageIndex = -1;
    this._pageSize = Web.DataViewResources.Pager.PageSizes[0];
    this._sortExpression = null;
    this._filter = [];
    this._externalFilter = [];
    this._categories = null;
    this._fields = null;
    this._allFields = null;
    this._rows = null;
    this._totalRowCount = 0;
    this._firstPageButtonIndex = 0;
    this._pageCount = 0;
    this._views = [];
    this._actionGroups = [];
    this._selectedKey = [];
    this._selectedKeyFilter = []
    this._lastCommandName = null;
    this._lastViewId = null;
    this._lookupField = null;
    this._filterFields = null;
    this._filterSource = null;
    this._mode = Web.DataViewMode.View;
    this._lookupPostBackExpression = null;
    this._domFilterSource = null;
    this._selectedKeyList = [];
    this._pageSizes = Web.DataViewResources.Pager.PageSizes;
}

Web.DataView.htmlEncode = function(s) { if (s != null && typeof (s) != 'string') s = s.toString(); return s ? s.replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;') : s; }

Web.DataView.htmlAttributeEncode = function(s) { return s != null && typeof (s) == 'string' ? s.replace(/\x27/g, '&#39;').replace(/\x22/g, '&quot;') : s; }

Web.DataView.isIE6 = this._ie6 = Sys.Browser.agent == Sys.Browser.InternetExplorer && Sys.Browser.version < 7;

Web.DataView.prototype = {
    get_controller: function() {
        return this._controller;
    },
    set_controller: function(value) {
        this._controller = value;
    },
    get_viewId: function() {
        if (!this._viewId && this._views.length > 0)
            this._viewId = this._views[0].Id;
        return this._viewId;
    },
    set_viewId: function(value) {
        this._viewId = value;
    },
    get_newViewId: function() {
        return this._newViewId;
    },
    set_newViewId: function(value) {
        this._newViewId = value;
    },
    get_servicePath: function() {
        return this._servicePath;
    },
    set_servicePath: function set_servicePath(value) {
        this._servicePath = this.resolveClientUrl(value);
        if (!Web.DataView._servicePath) Web.DataView._servicePath = value;
    },
    get_baseUrl: function() {
        return this._baseUrl;
    },
    set_baseUrl: function(value) {
        if (value == '~') value = '/';
        this._baseUrl = value;
        if (!Web.DataView._baseUrl) Web.DataView._baseUrl = value;
    },
    get_siteUrl: function() {
        var servicePath = this.get_servicePath();
        var m = servicePath.match(/(^.+?\/)\w+\/\w+\.asmx/);
        return m ? m[1] : '';
    },
    resolveClientUrl: function(url) {
        return url ? url.replace(/~\x2f/g, this.get_baseUrl()) : null;
    },
    get_hideExternalFilterFields: function() {
        return this._hideExternalFilterFields != false;
    },
    set_hideExternalFilterFields: function(value) {
        this._hideExternalFilterFields = value;
    },
    get_backOnCancel: function() {
        return this._backOnCancel == true;
    },
    set_backOnCancel: function(value) {
        this._backOnCancel = value;
    },
    get_startCommandName: function() {
        return this._startCommandName;
    },
    set_startCommandName: function(value) {
        this._startCommandName = value;
    },
    get_startCommandArgument: function() {
        return this._startCommandArgument;
    },
    set_startCommandArgument: function(value) {
        this._startCommandArgument = value;
    },
    get_exitModalStateCommands: function() {
        return this._exitModalStateCommands;
    },
    set_exitModalStateCommands: function(value) {
        this._exitModalStateCommands = value;
    },
    get_showActionBar: function() {
        return this._showActionBar;
    },
    set_showActionBar: function(value) {
        this._showActionBar = value;
    },
    get_showDescription: function() {
        return this._showDescription;
    },
    set_showDescription: function(value) {
        this._showDescription = value;
    },
    get_showViewSelector: function() {
        return this._showViewSelector;
    },
    set_showViewSelector: function(value) {
        this._showViewSelector = value;
    },
    get_selectionMode: function() {
        return this._selectionMode;
    },
    set_selectionMode: function(value) {
        this._selectionMode = value;
    },
    get_cookie: function() {
        return this._cookie;
    },
    set_cookie: function(value) {
        this._cookie = value;
    },
    get_pageIndex: function() {
        return this._pageIndex;
    },
    set_pageIndex: function(value) {
        this._pageIndex = value;
        if (value >= 0) {
            if (value >= this._firstPageButtonIndex + Web.DataViewResources.Pager.PageButtonCount)
                this._firstPageButtonIndex = value;
            else if (value < this._firstPageButtonIndex) {
                this._firstPageButtonIndex -= Web.DataViewResources.Pager.PageButtonCount;
                if (value < this._firstPageButtonIndex)
                    this._firstPageButtonIndex = value;
            }
            if (this._firstPageButtonIndex < 0)
                this._firstPageButtonIndex = 0;
            if (this.get_pageCount() - this._firstPageButtonIndex < Web.DataViewResources.Pager.PageButtonCount)
                this._firstPageButtonIndex = this.get_pageCount() - Web.DataViewResources.Pager.PageButtonCount;
            if (this._firstPageButtonIndex < 0)
                this._firstPageButtonIndex = 0;
        }
    },
    get_categoryTabIndex: function() {
        return this._categoryTabIndex;
    },
    set_categoryTabIndex: function(value) {
        if (value != this._categoryTabIndex) {
            this._categoryTabIndex = value;
            this._updateTabbedCategoryVisibility();
            if (value != -1) _body_performResize();
        }
    },
    get_enabled: function() {
        return this._enabled == null ? true : this._enabled;
    },
    set_enabled: function(value) {
        this._enabled = value;
    },
    get_showInSummary: function() {
        return this._showInSummary;
    },
    set_showInSummary: function(value) {
        this._showInSummary = value;
    },
    get_summaryFieldCount: function() {
        return this._summaryFieldCount;
    },
    set_summaryFieldCount: function(value) {
        this._summaryFieldCount = value;
    },
    get_tag: function() {
        return this._tag;
    },
    set_tag: function(value) {
        this._tag = value;
    },
    get_pageSize: function() {
        return this._pageSize;
    },
    set_pageSize: function(value) {
        this._pageSize = value;
        if (Array.indexOf(this._pageSizes, value) == -1) {
            this._pageSizes = Array.clone(this._pageSizes);
            Array.insert(this._pageSizes, 0, value);
        }
        if (this._fields != null) {
            this.set_pageIndex(-2);
            this._loadPage();
        }
    },
    get_sortExpression: function() {
        return this._sortExpression;
    },
    set_sortExpression: function(value) {
        if (!value || value.length == 0)
            this._sortExpression = null;
        else {
            var expression = value.match(/^(\w+)\s*((asc|desc)|$)/)
            if (expression[2].length == 0)
                if (String.isNullOrEmpty(this._sortExpression) || this._sortExpression.match(/^(\w+)\s*/)[1] != expression[1])
                this._sortExpression = value + ' asc';
            else if (this._sortExpression.endsWith(' asc'))
                this._sortExpression = value + ' desc';
            else
                this._sortExpression = value + ' asc';
            else
                this._sortExpression = value;
        }
    },
    get_filterSource: function() {
        return this._filterSource;
    },
    set_filterSource: function(value) {
        this._filterSource = value;
    },
    get_filterFields: function() {
        return this._filterFields;
    },
    set_filterFields: function(value) {
        this._filterFields = value;
    },
    get_showQuickFind: function() {
        return this._showQuickFind != false;
    },
    get_quickFindText: function() {
        return String.isNullOrEmpty(this._quickFindText) ? Web.DataViewResources.Grid.QuickFindText : this._quickFindText;
    },
    set_quickFindText: function(value) {
        this._quickFindText = value;
    },
    get_quickFindElement: function() {
        return $get(this.get_id() + '_QuickFind');
    },
    set_showQuickFind: function(value) {
        this._showQuickFind = value;
    },
    get_filter: function() {
        if (this.get_lookupField() == null && (this.get_pageIndex() == -1 && !this._allFields || this._externalFilter.length > 0 && this._filter.length == 0)) {
            if (this.get_domFilterSource()) {
                this._externalFilter = [];
                Array.add(this._externalFilter, { Name: this.get_filterFields(), Value: this.get_domFilterSource().value });
            }
            else {
                var params = Web.DataView._commandLine.match(/\?([\s\S]+)/);
                if (params && (this.get_filterSource() != 'None' && this.get_filterSource() == null)) {
                    this._externalFilter = [];
                    var iterator = /(\w+)=([\S\s]*?)(&|$)/g;
                    var m = iterator.exec(params[1]);
                    while (m) {
                        if (m[1] != 'ReturnUrl') Array.add(this._externalFilter, { Name: m[1], Value: m[2].length == 0 ? 'null' : decodeURIComponent(m[2]) });
                        m = iterator.exec(params[1]);
                    }
                }
            }
            this.applyExternalFilter();
        }
        else if (this.get_filterSource() == 'Context' && this._externalFilter.length > 0)
            this.applyExternalFilter(true);
        return this._filter;
    },
    set_filter: function(value) {
        this._filter = value;
    },
    get_externalFilter: function() {
        return this._externalFilter;
    },
    set_externalFilter: function(value) {
        this._externalFilter = value ? value : [];
    },
    get_modalAnchor: function() {
        return this._modalAnchor;
    },
    set_modalAnchor: function(value) {
        this._modalAnchor = value;
    },
    get_isModal: function() {
        return this._modalPopup != null;
    },
    get_categories: function() {
        return this._categories;
    },
    get_fields: function() {
        return this._fields;
    },
    get_rows: function() {
        return this._rows;
    },
    get_selectedRow: function() {
        return this._rows[this._selectedRowIndex != null ? this._selectedRowIndex : 0];
    },
    get_pageCount: function() {
        return this._pageCount;
    },
    get_aggregates: function() {
        return this._aggregates;
    },
    get_views: function() {
        return this._views;
    },
    get_actionGroups: function(scope, all) {
        var groups = [];
        for (var i = 0; i < this._actionGroups.length; i++) {
            if (this._actionGroups[i].Scope == scope) {
                var group = this._actionGroups[i];
                var current = all ? group : null;
                if (!all) {
                    for (var j = 0; j < group.Actions.length; j++) {
                        if (this._isActionAvailable(group.Actions[j])) {
                            current = this._actionGroups[i]
                            break;
                        }
                    }
                }
                if (current) Array.add(groups, current);
            }
        }
        return groups;
    },
    get_actions: function(scope) {
        for (var i = 0; i < this._actionGroups.length; i++)
            if (this._actionGroups[i].Scope == scope)
            return this._actionGroups[i].Actions;
        return [];
    },
    get_selectedKey: function() {
        return this._selectedKey;
    },
    get_selectedKeyFilter: function() {
        return this._selectedKeyFilter;
    },
    _get_selectedValueElement: function() {
        return $get(String.format('{0}_{1}_SelectedValue', this.get_id(), this.get_controller()));
    },
    get_selectedValue: function() {
        var sv = this._get_selectedValueElement();
        return sv ? sv.value : '';
    },
    set_selectedValue: function(value) {
        var selectedValue = this._get_selectedValueElement();
        if (selectedValue)
            selectedValue.value = value != null ? value : '';
    },
    get_keyRef: function() {
        var key = this.get_selectedKey();
        if (!key) return null;
        var ref = '';
        for (var i = 0; i < this._keyFields.length; i++) {
            if (i > 0) ref += '&';
            ref = String.format('{0}{1}={2}', ref, this._keyFields[i].Name, key[i]);
        }
        return ref;
    },
    get_showIcons: function() {
        return this._icons != null && this._lookupField == null;
    },
    _createRowKey: function(index) {
        var r = this._rows[index];
        var v = '';
        for (var i = 0; i < this._keyFields.length; i++) {
            var f = this._keyFields[i];
            if (v.length > 0) v += ',';
            v += r[f.Index].toString()
        }
        return v;
    },
    toggleSelectedRow: function(index) {
        var startIndex = index != null ? index : 0;
        var endIndex = index != null ? index : this._rows.length - 1;
        var btn = $get(this.get_id() + '_ToggleButton');
        for (var i = startIndex; i <= endIndex; i++) {
            var key = this._createRowKey(i);
            var j = Array.indexOf(this._selectedKeyList, key);
            if (j != -1)
                Array.removeAt(this._selectedKeyList, j);
            var checked = index == null ? btn.checked : j == -1;
            if (checked)
                Array.add(this._selectedKeyList, key);
            var cb = $get(this.get_id() + '_CheckBox' + i);
            if (cb) {
                cb.checked = checked;
                if (checked)
                    Sys.UI.DomElement.addCssClass(cb, 'Selected');
                else
                    Sys.UI.DomElement.removeCssClass(cb, 'Selected');
                var elem = cb;
                while (elem && elem.tagName != 'TR')
                    elem = elem.parentNode;
                if (checked)
                    Sys.UI.DomElement.addCssClass(elem, 'MultiSelectedRow');
                else
                    Sys.UI.DomElement.removeCssClass(elem, 'MultiSelectedRow');
            }
        }
        this.set_selectedValue(this._selectedKeyList.join(';'));
        var si = $get(this.get_id() + '$SelectionInfo');
        if (si) si.innerHTML = this._selectedKeyList.length == 0 ? '' : String.format(Web.DataViewResources.Pager.SelectionInfo, this._selectedKeyList.length);
    },
    get_view: function() {
        if (!this._view || this._view.Id != this.get_viewId())
            for (var i = 0; i < this._views.length; i++)
            if (this._views[i].Id == this.get_viewId()) {
            if (!this._view && this._views[i].Type == 'Form') {
                this._lastCommandName = 'Select';
                if (this._rows.length > 0) this._selectKeyByRowIndex(this._selectedRowIndex = 0);
            }
            this._view = this._views[i];
            break;
        }
        return this._view;
    },
    get_lastCommandName: function() {
        return this._lastCommandName;
    },
    set_lastCommandName: function(value) {
        this._lastCommandName = value;
        this._lastCommandArgument = null;
        $closeHovers();
    },
    get_lastCommandArgument: function() {
        return this._lastCommandArgument;
    },
    set_lastCommandArgument: function(value) {
        this._lastCommandArgument = value;
    },
    get_isEditing: function() {
        return this._lastCommandName == 'New' || this._lastCommandName == 'Edit' || this._lastCommandName == 'BatchEdit' || this._lastCommandName == 'Duplicate';
    },
    get_isInserting: function() {
        return this._lastCommandName == 'New' || this._lastCommandName == 'Duplicate';
    },
    get_lookupField: function() {
        return this.get_mode() == Web.DataViewMode.View ? this._lookupField : this._fields[0];
    },
    set_lookupField: function(value) {
        this._lookupField = value;
    },
    get_lookupContext: function() {
        var f = this.get_lookupField();
        return f ? { 'FieldName': f.Name, 'Controller': f._dataView.get_controller(), 'View': f._dataView.get_viewId()} : null;
    },
    get_mode: function() {
        return this._mode;
    },
    set_mode: function(value) {
        this._mode = value;
    },
    get_lookupValue: function() {
        return this._lookupValue;
    },
    set_lookupValue: function(value) {
        this._lookupValue = value;
    },
    get_lookupText: function() {
        return this._lookupText;
    },
    set_lookupText: function(value) {
        this._lookupText = value;
    },
    get_lookupPostBackExpression: function() {
        return this._lookupPostBackExpression;
    },
    set_lookupPostBackExpression: function(value) {
        this._lookupPostBackExpression = value;
    },
    get_domFilterSource: function() {
        return this._domFilterSource;
    },
    set_domFilterSource: function(value) {
        this._domFilterSource = value;
    },
    get_autoHide: function() {
        return this._autoHide == null ? Web.AutoHideMode.Nothing : this._autoHide;
    },
    set_autoHide: function(value) {
        this._autoHide = value;
    },
    initialize: function() {
        Web.DataView.callBaseMethod(this, 'initialize');
        this._bodyKeydownHandler = Function.createDelegate(this, this._bodyKeydown);
        this._filterSourceSelectedHandler = Function.createDelegate(this, this._filterSourceSelected);
        this._quickFindHandlers = {
            'focus': this._quickFind_focus,
            'blur': this._quickFind_blur,
            'keydown': this._quickFind_keydown
        }
    },
    dispose: function() {
        if (!Sys.Application._disposing) this._detachBehaviors();
        this._disposeFields();
        this._lookupField = null;
        this._bodyKeydownHandler = null;
        this._filterSourceSelectedHandler = null;
        delete this._container;
        Web.DataView.callBaseMethod(this, 'dispose');
    },
    updated: function() {
        Web.DataView.callBaseMethod(this, 'updated');
        if (!Web.DataView._commandLine) {
            Web.DataView._commandLine = typeof (Web.Membership) != "undefined" ? Web.Membership._instance.get_commandLine() : null;
            Web.DataView._commandLine = !Web.DataView._commandLine ? location.href : location.pathname + '?' + Web.DataView._commandLine;
        }
        if (this.get_servicePath().startsWith('http'))
            this.set_baseUrl(this.get_siteUrl());
        var selectedValue = this.get_selectedValue();
        if (selectedValue.length > 0) {
            if (this.get_selectionMode() == Web.DataViewSelectionMode.Multiple)
                this._selectedKeyList = selectedValue.split(';');
            else {
                this._selectedKey = selectedValue.split(',');
                this._pendingSelectedEvent = true;
            }
        }
        if (this._container == null) {
            this.get_element().innerHTML = '';
            this._container = document.createElement('div');
            this.get_element().appendChild(this._container);
            Sys.UI.DomElement.addCssClass(this._container, 'DataViewContainer');
            if (!this.get_showActionBar())
                Sys.UI.DomElement.addCssClass(this._container, 'ActionBarHidden');
            if (!this.get_showDescription())
                Sys.UI.DomElement.addCssClass(this._container, 'DescriptionHidden');
        }
        if (this.get_filterSource() && this.get_filterSource() != 'Context') {
            var source = Web.DataView.find(this.get_filterSource());
            if (source) {
                source.add_selected(this._filterSourceSelectedHandler);
                if (source._pendingSelectedEvent) {
                    this._source = source;
                    this._afterUpdateTimerId = window.setInterval(String.format('$find("{0}")._afterUpdate()', this.get_id()), 250);
                }
            }
            else {
                source = $get(this.get_filterSource());
                if (source) $addHandler(source, 'change', this._filterSourceSelectedHandler);
                this.set_domFilterSource(source);
            }
            this._createExternalFilter();
            if (!this._source) this.applyExternalFilter();
        }
        if (this.get_modalAnchor() && !this.get_isModal())
            this._initializeModalPopup();
        if (source != null && this.get_autoHide() != Web.AutoHideMode.Nothing)
            this._updateLayoutContainerVisibility(false);
        var commandLine = Web.DataView._commandLine;
        var command = commandLine.match(/_commandName=(.+?)&_commandArgument=(.*?)(&|$)/);
        if (command && String.isNullOrEmpty(this.get_startCommandName()) && !this.get_filterSource() && !this.get_isModal()) {
            var tc = commandLine.match(/_controller=(\w+)/);
            var tv = commandLine.match(/_view=(\w+)/);
            if ((!tc || tc[1] == this.get_controller()) && (!tv || tv[1] == this.get_view())) {
                this.set_startCommandName(command[1]);
                this.set_startCommandArgument(command[2]);
                if (!String.isNullOrEmpty(this._viewId)) this._replaceTriggerViewId = this._viewId;
                this._skipRender = true;
                this._skipTriggeredView = true;
            }
        }
        this.loadPage();
    },
    _afterUpdate: function() {
        if (this._delayedLoading && this._source._pendingSelectedEvent || this._source._isBusy) return;
        window.clearInterval(this._afterUpdateTimerId);
        var source = this._source;
        this._source = null;
        this._filterSourceSelected(source, Sys.EventArgs.Empty, true);
    },
    _updateLayoutContainerVisibility: function(visible) {
        var c = this._element.parentNode;
        if (String.isNullOrEmpty(c.getAttribute('factory:visibilityController')))
            c.setAttribute('factory:visibilityController', this.get_id());
        var activator = !String.isNullOrEmpty(c.getAttribute('factory:activator')) ? c.getAttribute('factory:activator').match(/^\s*(\w+)\s*\|\s*(.+)\s*$/) : null;
        if (this.get_autoHide() == Web.AutoHideMode.Self) {
            this._element.style.display = visible ? '' : 'none';
            var tabBar = c.parentNode.childNodes[c.parentNode.childNodes[0].className ? 0 : 1];
            if (activator && activator[1] == 'Tab' && Sys.UI.DomElement.containsCssClass(tabBar, 'TabBar')) {
                if (c.getAttribute('factory:visibilityController') == this.get_id()) {
                    var tabIndex = -1;
                    for (var i = 0; i < c.parentNode.childNodes.length; i++) {
                        var n = c.parentNode.childNodes[i];
                        if (n.className && Sys.UI.DomElement.containsCssClass(n, 'TabBody')) tabIndex++;
                        if (c == n) break;

                    }
                    if (tabIndex != -1) {
                        var menuCells = tabBar.getElementsByTagName('td');
                        menuCells[tabIndex].style.display = visible ? '' : 'none';
                    }
                }
            }
            else
                c.setAttribute('factory:hidden', !visible);
        }
        else {
            if (activator && activator[1] == 'SideBarTask')
                c.setAttribute('factory:hidden', true);
            while (c) {
                if (!String.isNullOrEmpty(c.getAttribute('factory:flow'))) {
                    c.style.display = visible ? '' : 'none';
                    break;
                }
                c = c.parentNode;
            }
        }
        var sideBar = $get('PageContentSideBar');
        if (sideBar && activator) {
            var tasks = sideBar.getElementsByTagName('a');
            for (i = 0; i < tasks.length; i++) {
                var l = tasks[i];
                if (Sys.UI.DomElement.containsCssClass(l.parentNode, 'Task') && l.innerHTML == activator[2]) {
                    l.parentNode.style.display = visible ? '' : 'none';
                    break;
                }
            }
        }
        if (Web.DataView._loaded) _body_performResize();
    },
    loadPage: function(showWait) {
        var displayed = this.get_isDisplayed();
        this._showWait(!displayed);
        if (this.get_mode() != Web.DataViewMode.View || (this.get_lookupField() || !(this._delayedLoading = !displayed))) {
            if (!this._source) this._loadPage();
        }
        else if (!Array.contains(Web.DataView._delayedLoadingViews, this)) {
            Array.add(Web.DataView._delayedLoadingViews, this);
            Web.DataView._startDelayedLoading();
        }
    },
    get_isDisplayed: function() {
        var node = this.get_element().parentNode;
        while (node != null) {
            if (node.getAttribute && node.tagName != 'TABLE' && node.getAttribute('factory:activator') && !node._activated)
                return false;
            if (node.style && node.style.display == 'none')
                return false;
            node = node.parentNode;
        }
        return true;
    },
    goToPage: function(pageIndex) {
        this.set_pageIndex(pageIndex);
        this._loadPage();
    },
    sort: function(sortExpression) {
        if (this.get_sortExpression() == sortExpression) sortExpression = '';
        this.set_sortExpression(sortExpression);
        this.set_pageIndex(0);
        this._loadPage();
    },
    applyFilterByIndex: function(fieldIndex, valueIndex) {
        var filterField = this._allFields[fieldIndex];
        var field = this.findFieldUnderAlias(filterField);
        this.applyFilter(filterField, valueIndex >= 0 ? '=' : null, valueIndex >= 0 ? field._listOfValues[valueIndex] : null);
    },
    findFieldUnderAlias: function(aliasField) {
        if (aliasField.Hidden)
            for (var i = 0; i < this._allFields.length; i++)
            if (this._allFields[i].AliasIndex == aliasField.Index) return this._allFields[i];
        return aliasField;
    },
    removeFromFilter: function(field) {
        for (var i = 0; i < this._filter.length; i++) {
            if (this._filter[i].match(/^(\w+):/)[1] == field.Name) {
                Array.removeAt(this._filter, i);
                break;
            }
        }
    },
    clearFilter: function() {
        for (var i = 0; i < this._allFields.length; i++) {
            var f = this._allFields[i];
            if (this.filterOf(f) != null) this.removeFromFilter(f);
        }
        var qfe = this.get_quickFindElement();
        if (qfe != null) {
            qfe.value = '';
            this.quickFind();
        }
    },
    beginFilter: function() {
        this._filtering = true;
    },
    endFilter: function() {
        this._filtering = false;
        this.set_pageIndex(-2);
        this._loadPage();
    },
    applyFilter: function(field, operator, value) {
        this.removeFromFilter(field);
        if (operator == ':') {
            if (value) Array.add(this._filter, field.Name + ':' + value);
        }
        else if (operator) Array.add(this._filter, field.Name + ':' + operator + this.convertFieldValueToString(field, value));
        var keepFieldValues = (this._filter.length == 1 && this._filter[0].match(/(\w+):/)[1] == field.Name);
        field = this.findFieldUnderAlias(field);
        for (i = 0; i < this._allFields.length; i++)
            if (!keepFieldValues || this._allFields[i].Name != field.Name) this._allFields[i]._listOfValues = null;
        if (this._filtering != true) {
            this.set_pageIndex(-2);
            this._loadPage();
        }
    },
    applyExternalFilter: function(preserveFilter) {
        if (!preserveFilter) this._filter = [];
        this._selectedRowIndex = null;
        for (var i = 0; i < this._externalFilter.length; i++) {
            var filterItem = this._externalFilter[i];
            if (preserveFilter) {
                for (var j = 0; j < this._filter.length; j++) {
                    if (this._filter[j].startsWith(filterItem.Name + ':=')) {
                        Array.removeAt(this._filter, j);
                        break;
                    }
                }
            }
            Array.add(this._filter, filterItem.Name + ':=' + filterItem.Value);
        }
    },
    showCustomFilter: function(fieldIndex) {
        var field = this._allFields[fieldIndex];
        var customFilter = null;
        for (var i = 0; i < this._filter.length; i++) {
            var m = this._filter[i].match(/(\w+):([\s\S]*)/);
            if (m[1] == field.Name) {
                customFilter = m[2];
                break;
            }
        }
        if (customFilter && customFilter.indexOf('\0') == -1) customFilter += '\0';
        var panel = this._customFilterPanel = document.createElement('div');
        this.get_element().appendChild(panel);
        panel.className = 'CustomFilterDialog';
        panel.id = this.get_id() + '_CustomFilterPanel';
        var sb = new Sys.StringBuilder();
        sb.appendFormat('<div>{0}</div>', String.format(Web.DataViewResources.HeaderFilter.CustomFilterPrompt, this.get_view().Label, field.Label));
        sb.append('<table cellpadding="0" cellspacing="0">');
        sb.appendFormat('<tr><td colspan="2"><input type="text" id="{0}_Query" size="70" value="{1}"></input></td></tr>', this.get_id(), customFilter ? Web.DataView.htmlAttributeEncode(customFilter.replace(/\0/g, ', ').replace(/\*|[^<>]=|^=|, $/g, '')) : '');
        sb.appendFormat('<tr><td></td><td align="right"><button id="{0}Ok">{1}</button> <button id="{0}Cancel">{2}</button></td></tr>', this.get_id(), Web.DataViewResources.ModalPopup.OkButton, Web.DataViewResources.ModalPopup.CancelButton);
        sb.append('</table>');
        panel.innerHTML = sb.toString();
        sb.clear();
        this._customFilterField = field;
        this._customFilterModalPopupBehavior = $create(AjaxControlToolkit.ModalPopupBehavior, {
            OkControlID: this.get_id() + 'Ok', CancelControlID: this.get_id() + 'Cancel', OnOkScript: String.format('$find("{0}").applyCustomFilter()', this.get_id()), OnCancelScript: String.format('$find("{0}").closeCustomFilter()', this.get_id()),
            PopupControlID: panel.id, DropShadow: true, BackgroundCssClass: 'ModalBackground'
        }, null, null, this._container.getElementsByTagName('a')[0]);
        this._customFilterModalPopupBehavior.show();
        $addHandler(document.body, 'keydown', this._bodyKeydownHandler);
        var q = $get(this.get_id() + '_Query');
        q.focus();
        q.select();
    },
    applyCustomFilter: function() {
        this.removeFromFilter(this._customFilterField);
        var iterator = /\s*(=|<={0,1}|>={0,1}|)\s*([\S\s]+?)\s*(,|$)/g;
        var filter = this._customFilterField.Name + ':';
        var s = $get(this.get_id() + '_Query').value;
        var match = iterator.exec(s);
        while (match) {
            filter += (match[1] ? match[1] : (this._customFilterField.Type == 'String' ? '*' : '=')) + match[2] + '\0';
            match = iterator.exec(s);
        }
        if (filter.indexOf('\0') > 0) Array.add(this._filter, filter);
        this.set_pageIndex(-2);
        this._loadPage();
        this.closeCustomFilter();
    },
    closeCustomFilter: function() {
        if (this._customFilterModalPopupBehavior) {
            this._customFilterModalPopupBehavior.dispose();
            this._customFilterModalPopupBehavior = null;
            this._customFilterField = null;
        }
        if (this._customFilterPanel) {
            this._customFilterPanel.parentNode.removeChild(this._customFilterPanel);
            delete this._customFilterPanel;
        }
        $removeHandler(document.body, 'keydown', this._bodyKeydownHandler);
    },
    convertFieldValueToString: function(field, value) {
        return field.Type.startsWith('DateTime') && field.DataFormatString && field.DataFormatString.length > 0 ? (value == null ? 'null' : String.localeFormat(field.DataFormatString, value)) : value;
    },
    goToView: function(viewId) {
        if (!String.isNullOrEmpty(this._replaceTriggerViewId) && this._replaceTriggerViewId == viewId) {
            if (this._skipTriggeredView) this._skipTriggeredView = false;
            location.replace(location.href)
            return;
        }
        if (viewId == 'form')
            for (var i = 0; i < this.get_views().length; i++)
            if (this.get_views()[i].Type == 'Form') {
            viewId = this.get_views()[i].Id;
            break;
        }
        this._detachBehaviors();
        if (this.get_view().Type != 'Form') {
            this._lastViewId = this.get_viewId();
            this._selectedRowIndex = 0;
        }
        if (this.get_view().Id != viewId) this._focusedFieldName = null;
        this.set_viewId(viewId);
        if (this.get_view().Type != 'Form') this._lastViewId = viewId;
        this.set_pageIndex(-1);
        this.set_filter(this.get_view().Type == 'Form' ? this.get_selectedKeyFilter() : []);
        this.set_sortExpression(null);
        this._loadPage();
        this._raiseSelectedDelayed = true;
        this._scrollIntoView = true;
    },
    filterOf: function(field) {
        var header = (field.AliasName && field.AliasName.length > 0 ? field.AliasName : field.Name) + ':';
        for (var i = 0; i < this._filter.length; i++) {
            var s = this._filter[i];
            if (s.startsWith(header) && !s.match(':~'))
                return this._filter[i].substr(header.length);
        }
        return null;
    },
    findField: function(query) {
        for (var i = 0; i < this._allFields.length; i++) {
            var field = this._allFields[i];
            if (field.Name == query) return field;
        }
        return null;
    },
    findCategory: function(query) {
        for (var i = 0; i < this._categories.length; i++) {
            var c = this._categories[i];
            if (c.HeaderText == query) return c;
        }
        return null;
    },
    _isInInstantDetailsMode: function() {
        return window.location.href.match(/(\?|&)l=.+(&|$)/);
    },
    _closeInstantDetails: function() {
        if (this._isInInstantDetailsMode()) {
            if (Web.DataViewResources.Lookup.ShowDetailsInPopup) {
                window.close();
                return true;
            }
        }
        return false;
    },
    executeAction: function(scope, actionIndex, rowIndex, groupIndex) {
        if (this._isBusy) return;
        Web.DataView.hideMessage();
        var actions = this.get_lookupField() ? null : (scope == 'ActionBar' ? this.get_actionGroups(scope)[groupIndex].Actions : this.get_actions(scope));
        if (actionIndex < 0 && actions) {
            for (var i = 0; i < actions.length; i++)
                if (this._isActionAvailable(actions[i], rowIndex)) {
                actionIndex = i;
                break;
            }
            if (actionIndex < 0) return;
        }
        var action = actions ? actions[actionIndex] : null;
        if (action && action.Confirmation && action.Confirmation.length > 0 && !confirm(action.Confirmation)) return;
        var command = action ? action.CommandName : 'Select';
        var argument = action ? action.CommandArgument : null;
        var causesValidation = action ? action.CausesValidation : true;
        this.executeRowCommand(rowIndex, command, argument, causesValidation);
    },
    executeRowCommand: function(rowIndex, command, argument, causesValidation) {
        if (rowIndex != null && rowIndex >= 0) {
            this._selectedRowIndex = rowIndex;
            this._raiseSelectedDelayed = !(command == 'Select' && String.isNullOrEmpty(argument));
            this._selectKeyByRowIndex(rowIndex);
        }
        this.executeCommand({ commandName: command, commandArgument: argument ? argument : '', causesValidation: causesValidation ? true : false });
        if (command == 'ClientScript') window.setTimeout(String.format('$find("{0}").refresh(true);', this.get_id()), 10);
        else if (command == 'Select' && argument == null && this.get_view().Type != 'Grid') this._render();
    },
    _get_dataRequestForm: function() {
        var f = $get('_dataRequest_form');
        if (!f) {
            f = document.createElement('form');
            f.id = '_dataRequest_form';
            f.method = 'post';
            f.innerHTML = '<input type="hidden" name="q" id="q"/><input type="hidden" name="c" id="c"/><input type="hidden" name="a" id="a"/>';
            document.body.appendChild(f);
        }
        return f;
    },
    executeReport: function(args) {
        var f = this._get_dataRequestForm();
        f.action = this.resolveClientUrl('~/Report.ashx');
        $get('c', f).value = args.commandName;
        $get('a', f).value = args.commandArgument;
        var q = this._createParams();
        q.Controller = this.get_controller();
        q.View = this.get_viewId();
        $get('q', f).value = Sys.Serialization.JavaScriptSerializer.serialize(q);
        f.submit();
    },
    executeExport: function(args) {
        var f = this._get_dataRequestForm();
        f.target = args.commandName == 'ExportRss' ? '_blank' : '';
        f.action = this.resolveClientUrl('~/Export.ashx');
        var q = this._createParams();
        args.Controller = this.get_controller();
        args.View = this.get_viewId();
        args.Filter = q.Filter;
        args.SortExpression = q.SortExpression;
        $get('c', f).value = args.commandName;
        $get('a', f).value = args.commandArgument;
        $get('q', f).value = Sys.Serialization.JavaScriptSerializer.serialize(args);
        f.submit();
    },
    _clearDynamicItems: function() {
        for (var i = 0; i < this._allFields.length; i++) {
            var f = this._allFields[i];
            if (f.DynamicItems) f.DynamicItems = null;
        }
    },
    _copyLookupValues: function(r, lf, nv, outputValues) {
        if (String.isNullOrEmpty(lf.Copy)) return;
        var values = outputValues ? outputValues : [];
        var iterator = /(\w+)=(\w+)/g;
        var m = iterator.exec(lf.Copy);
        while (m) {
            if (lf._dataView.findField(m[1])) {
                if (r) {
                    var f = this.findField(m[2]);
                    if (f) Array.add(values, { 'name': m[1], 'value': r[f.Index] });
                }
                else if (nv) {
                    for (var i = 0; i < nv.length; i++) {
                        if (nv[i].Name == m[2]) {
                            Array.add(values, { 'name': m[1], 'value': nv[i].NewValue });
                            break;
                        }
                    }
                }
            }
            m = iterator.exec(lf.Copy);
        }
        if (outputValues) return;
        lf._dataView.refresh(true, values);
        lf._dataView._focus();
    },
    _copyExternalLookupValues: function() {
        if (this.get_filterSource() && this.get_filterSource() != 'Context') {
            var master = Web.DataView.find(this.get_filterSource());
            if (master) {
                var ditto = [];
                var filter = this.get_externalFilter();
                for (var i = 0; i < filter.length; i++) {
                    var f = this.findField(filter[i].Name);
                    if (f && !String.isNullOrEmpty(f.Copy)) {
                        master._copyLookupValues(master.get_currentRow(), f, null, ditto);
                    }
                }
                this._ditto = ditto;
            }
        }
    },
    _processSelectedLookupValues: function() {
        var values = [];
        var labels = [];
        var lf = this.get_lookupField();
        var dataValueField = this.findField(lf.ItemsDataValueField);
        var dataTextField = this.findField(lf.ItemsDataTextField);
        var r = this.get_selectedRow();
        if (dataValueField != null) Array.add(values, r[dataValueField.Index]);
        if (dataTextField != null) Array.add(labels, r[dataTextField.Index]);
        if (!dataValueField) {
            for (var i = 0; i < this._allFields.length; i++) {
                if (this._allFields[i].IsPrimaryKey)
                    Array.add(values, r[this._allFields[i].Index]);
            }
        }
        if (!dataTextField) {
            for (i = 0; i < this.get_fields().length; i++) {
                f = this.get_fields()[i];
                if (!f.Hidden && f.Type == 'String') {
                    Array.add(labels, r[f.AliasIndex]);
                    break;
                }
            }
            if (labels.length == 0) {
                for (i = 0; i < values.length; i++) {
                    var f = this.get_fields()[i];
                    if (!f.Hidden) {
                        Array.add(labels, r[f.AliasIndex]);
                        break;
                    }
                }
            }
        }
        this._copyLookupValues(r, lf);
        lf._dataView.changeLookupValue(lf.Index, values.length == 1 ? values[0] : values, labels.join(';'));
    },
    executeCommand: function(args) {
        if (this._isBusy) return;
        switch (args.commandName) {
            case 'Select':
                this.set_lastCommandName(args.commandName);
                this.set_lastCommandArgument(args.commandArgument);
                if (this.get_lookupField() && args.commandArgument == '') this._processSelectedLookupValues();
                else {
                    if (args.commandArgument.length > 0)
                        this.goToView(args.commandArgument);
                    else
                        this._render();
                }
                break;
            case 'Edit':
            case 'BatchEdit':
            case 'New':
            case 'Duplicate':
                if (args.commandName == 'Duplicate') {
                    var r = this.get_selectedRow();
                    if (r) {
                        var dv = []
                        for (i = 0; i < this._allFields.length; i++) {
                            var f = this._allFields[i];
                            Array.add(dv, { 'name': f.Name, 'value': r[f.Index] });
                        }
                        this._ditto = dv;
                    }
                }
                if (args.commandName == 'New' || args.commandName == 'Duplicate') {
                    this._lastSelectedRowIndex = this._selectedKey.length > 0 ? this._selectedRowIndex : -1;
                    Array.clear(this._selectedKey);
                    this.updateSummary();
                    if (args.commandName == 'New') this._copyExternalLookupValues();
                }
                this.set_lastCommandName(args.commandName);
                this.set_lastCommandArgument(args.commandArgument);
                this._clearDynamicItems();
                if (args.commandArgument.length > 0)
                    this.goToView(args.commandArgument);
                else
                    this._render();
                break;
            case 'Navigate':
                this.navigate(args.commandArgument);
                break;
            case 'Cancel':
                if (this._closeInstantDetails()) { }
                else if (this.endModalState('Cancel')) return;
                else if (this.get_backOnCancel() || !String.isNullOrEmpty(this._replaceTriggerViewId)) {
                    history.go(-1);
                    Web.DataView._navigated = true;
                    window.setTimeout('location.replace(location.href)', 500);
                }
                else {
                    var inserting = this.get_isInserting();
                    this.set_lastCommandName('Cancel');
                    if (this.get_view().Type == 'Form' || inserting)
                        this.goToView(this._lastViewId);
                    else {
                        this._clearDynamicItems();
                        this._render();
                        _body_performResize();
                    }
                }
                break;
            case 'Back':
                history.go(!String.isNullOrEmpty(args.commandArgument) ? parseInt(args.commandArgument) : -1);
                break;
            case 'Report':
            case 'ReportAsPdf':
            case 'ReportAsImage':
            case 'ReportAsExcel':
                this.executeReport(args);
                break;
            case 'ExportCsv':
            case 'ExportRowset':
            case 'ExportRss':
                this.executeExport(args);
                break;
            case '_ViewDetails':
                f = this.findField(args.commandArgument);
                if (f) {
                    var keyFieldName = f.Name;
                    if (f.ItemsDataController == this.get_controller())
                        for (i = 0; i < this._allFields.length; i++) {
                        if (this._allFields[i].IsPrimaryKey) {
                            keyFieldName = this._allFields[i].Name;
                            break;
                        }
                    }
                    var link = String.format('{0}&{1}={2}', f.ItemsDataController, f.ItemsDataValueField && f.ItemsDataValueField.length > 0 ? f.ItemsDataValueField : keyFieldName, this.get_selectedRow()[f.Index]);
                    if (Web.DataViewResources.Lookup.ShowDetailsInPopup)
                        window.open(this.resolveClientUrl(String.format('~/{0}?l={1}', Web.DataView.DetailsHandler, encodeURIComponent(link))), '_blank', 'scrollbars=yes,height=100,resizable=yes');
                    else
                        window.location.href = this.resolveClientUrl(String.format('~/{0}?l={1}', Web.DataView.DetailsHandler, encodeURIComponent(link)));
                }
                break;
            case 'ClientScript':
                Web.HoverMonitor._instance.close();
                eval(args.commandArgument);
                break;
            case 'SelectModal':
            case 'EditModal':
                this.set_lastCommandName(null);
                this.set_lastCommandArgument(null);
                this._render();
                var modalCmd = args.commandName.match(/^(\w+)Modal$/);
                this.set_lastCommandName(modalCmd[1]);
                this.set_lastCommandArgument(args.commandArgument);
                var modalArg = args.commandArgument.split(',');
                var modalController = modalArg.length == 1 ? this.get_controller() : modalArg[0];
                var modalView = modalArg.length == 1 ? args.commandArgument : modalArg[1];
                var filter = [];
                for (i = 0; i < this.get_selectedKey().length; i++)
                    Array.add(filter, { Name: this._keyFields[i].Name, Value: this.get_selectedKey()[i] });
                Web.DataView.showModal(null, modalController, modalView, modalCmd[1], modalView, this.get_baseUrl(), this.get_servicePath(), filter);
                break;
            default:
                var view = this.get_viewId();
                var m = args.commandArgument.match(/^view:(.+)$/);
                if (args.commandName == 'Insert' && m) {
                    view = m[1];
                    Array.clear(this._selectedKey);
                    this.updateSummary();
                    this.set_lastCommandName('New');
                    this.set_lastCommandArgument(view);
                }
                values = this._collectFieldValues();
                if (this._validateFieldValues(values, args.causesValidation == null || args.causesValidation))
                    this._execute({ CommandName: args.commandName, CommandArgument: args.commandArgument, LastCommandName: this.get_lastCommandName(), Values: values, ContextKey: this.get_id(), Cookie: this.get_cookie(), Controller: this.get_controller(), View: view });
                break;
        }
    },
    _parseLocation: function(location, row, values) {
        if (!row) row = this.get_selectedRow();
        if (location) {
            location = this.resolveClientUrl(location);
            var iterator = /([\s\S]*?)\{(\w+)?\}/g;
            var formattedLocation = '';
            var lastIndex = -1;
            var match = iterator.exec(location);
            while (match) {
                formattedLocation += match[1];
                if (values && this._lastArgs) {
                    for (var i = 0; i < values.length; i++) {
                        var v = values[i];
                        if (v.Name == match[2]) {
                            formattedLocation += this._lastArgs.CommandName.match(/Insert/i) ? v.NewValue : v.OldValue;
                            break;
                        }
                    }
                }
                else {
                    var field = match[2].match(/^\d+$/) ? this.get_fields()[parseInt(match[2])] : this.findField(match[2]);
                    if (field) {
                        var fv = row[field.Index];
                        if (fv != null) formattedLocation += encodeURIComponent(fv);
                    }
                }
                lastIndex = iterator.lastIndex;
                match = iterator.exec(location);
            }
            if (lastIndex != -1) location = formattedLocation + (lastIndex < location.length ? location.substr(lastIndex) : '');
        }
        return location;
    },
    navigate: function(location, values) {
        this.set_selectedValue(this.get_selectedKey());
        Web.HoverMonitor._instance.close();
        location = this._parseLocation(location, null, values);
        var targetView = null;
        for (i = 0; i < this.get_views().length; i++)
            if (this.get_views()[i].Id == location) {
            targetView = this.get_views()[i];
            break;
        }
        if (targetView)
            this.goToView(location);
        else {
            Web.DataView._navigated = true;
            var m = location.match(Web.DataView.LocationRegex);
            if (m)
                window.open(m[2], m[1]);
            else
                window.location.href = location;
        }
    },
    get_contextFilter: function(field) {
        var contextFilter = [];
        if (!String.isNullOrEmpty(field.ContextFields)) {
            var contextValues = this._collectFieldValues();
            var iterator = /(\w+)(=(.+?)){0,1}(,|$)/g;
            var m = iterator.exec(field.ContextFields);
            while (m) {
                var n = !String.isNullOrEmpty(m[3]) ? m[3] : m[1];
                var m2 = n.match(/^\'(.+)\'$/);
                if (m2) {
                    for (var i = 0; i < contextFilter.length; i++) {
                        if (contextFilter[i].Name == m[1]) {
                            contextFilter[i].Value += '\0=' + m2[1];
                            m2 = null;
                            break;
                        }
                    }
                    if (m2) Array.add(contextFilter, { 'Name': m[1], 'Value': m2[1] });
                }
                else {
                    var f = this.findField(n);
                    if (f) {
                        for (i = 0; i < contextValues.length; i++) {
                            if (contextValues[i].Name == f.Name) {
                                var v = contextValues[i];
                                var fieldValue = v.Modified ? v.NewValue : v.OldValue;
                                Array.add(contextFilter, { 'Name': m[1], 'Value': fieldValue });
                                break;
                            }
                        }
                    }
                }
                m = iterator.exec(field.ContextFields);
            }
        }
        return contextFilter;
    },
    showLookup: function(fieldIndex) {
        if (!this.get_enabled()) return;
        var field = this._allFields[fieldIndex];
        if (!field._lookupModalBehavior) {
            var showLink = $get(this.get_id() + '_Item' + field.Index + '_ShowLookupLink');
            if (showLink) {
                var panel = field._lookupModalPanel = document.createElement('div');
                document.body.appendChild(panel);
                panel.className = 'ModalPanel';
                panel.id = this.get_id() + '_ItemLookupPanel' + field.Index;
                panel.innerHTML = String.format('<table style="width:100%;height:100%"><tr><td valign="middle" align="center"><table cellpadding="0" cellspacing="0"><tr><td class="ModalTop"><div style="height:1px;font-size:1px"></div></td><td><div style="height:1px;font-size:1px"></div></td></tr><tr><td align="left" valign="top" id="{0}_ItemLookupPlaceholder{1}"  class="ModalPlaceholder"></td><td class="RightSideShadow"></td></tr><tr><td colspan="2"><div class="BottomShadow"></div></td></tr></table></td></tr></table>', this.get_id(), field.Index);
                field._lookupModalBehavior = $create(AjaxControlToolkit.ModalPopupBehavior, { id: this.get_id() + "_ItemLookup" + field.Index, PopupControlID: panel.id, BackgroundCssClass: 'ModalBackground' }, null, null, showLink);
            }
        }
        else
            field._lookupDataControllerBehavior._render();
        var contextFilter = this.get_contextFilter(field);
        var focusQF = true;
        if (!field._lookupDataControllerBehavior) {
            focusQF = false;
            field._lookupDataControllerBehavior = $create(Web.DataView, { id: this.get_id() + '_LookupView' + fieldIndex, baseUrl: this.get_baseUrl(), pageSize: Web.DataViewResources.Pager.PageSizes[0], servicePath: this.get_servicePath(), controller: field.ItemsDataController, showActionBar: Web.DataViewResources.Lookup.ShowActionBar, lookupField: field, externalFilter: contextFilter, filterSource: contextFilter.length > 0 ? 'Context' : null }, null, null, $get(this.get_id() + '_ItemLookupPlaceholder' + field.Index));
        }
        else if (contextFilter.length > 0) {
            field._lookupDataControllerBehavior.set_externalFilter(contextFilter);
            field._lookupDataControllerBehavior.goToPage(-1);
            focusQF = true;
        }
        this._saveTabIndexes();
        field._lookupModalBehavior.show();
        if (focusQF) field._lookupDataControllerBehavior._focusQuickFind(true);
        $addHandler(document.body, 'keydown', field._lookupDataControllerBehavior._bodyKeydownHandler);
        field._lookupDataControllerBehavior._adjustLookupSize();
    },
    changeLookupValue: function(fieldIndex, value, text) {
        var field = this._allFields[fieldIndex];
        this._closeLookup(field);
        this._restoreTabIndexes();
        var itemId = this.get_id() + '_Item' + fieldIndex;
        var itemTextId = this.get_id() + '_Item' + field.AliasIndex;
        Sys.UI.DomElement.setVisible($get(itemId + '_ClearLookupLink'), true);
        $get(itemId + '_ShowLookupLink').innerHTML = this.htmlEncode(field, text);
        $get(itemId + '_ShowLookupLink').focus();
        $get(itemId).value = value;
        if (itemId != itemTextId) $get(itemTextId).value = text;
        this._updateLookupInfo(value, text);
        this._valueChanged(fieldIndex);
    },
    clearLookupValue: function(fieldIndex) {
        var field = this._allFields[fieldIndex];
        var itemId = this.get_id() + '_Item' + fieldIndex;
        var itemTextId = this.get_id() + '_Item' + field.AliasIndex;
        Sys.UI.DomElement.setVisible($get(itemId + '_ClearLookupLink'), false);
        $get(itemId + '_ShowLookupLink').innerHTML = Web.DataViewResources.Lookup.SelectLink;
        $get(itemId + '_ShowLookupLink').focus();
        $get(itemId).value = '';
        $get(itemTextId).value = '';
        this._updateLookupInfo('', Web.DataViewResources.Lookup.SelectLink);
        this._valueChanged(fieldIndex);
    },
    _updateLookupInfo: function(value, text) {
        var lookupText = $get(this.get_id() + '_Text0');
        if (lookupText) {
            lookupText.value = text;
            lookupText.name = lookupText.id;
            var lookupValue = $get(this.get_id() + '_Item0');
            lookupValue.value = value;
            lookupValue.name = lookupValue.id;
            if (this.get_lookupPostBackExpression()) {
                var prm = Sys.WebForms.PageRequestManager.getInstance();
                if (prm)
                    eval("Sys.WebForms.PageRequestManager.getInstance()._doPostBack" + this.get_lookupPostBackExpression().match(/\w+(.+)/)[1]);
                else
                    eval(this.get_lookupPostBackExpression());
            }
        }
    },
    createNewLookupValue: function(fieldIndex) {
        var field = this._newLookupValueField = this._allFields[fieldIndex];
        this._createNewView = Web.DataView.showModal($get(String.format('{0}_Item{1}_CreateNewLookupLink', this.get_id(), field.Index)), field.ItemsDataController, field.ItemsNewDataView, 'New', field.ItemsNewDataView, this.get_baseUrl(), this.get_servicePath(), this.get_contextFilter(field));
        this._createNewView.add_executed(Function.createDelegate(this, this._saveNewLookupValueCompleted));
    },
    _saveNewLookupValueCompleted: function(sender, args) {
        if (args.result.Errors.length > 0) return;
        args.handled = true;
        Web.DataView.hideMessage();
        var v = null;
        if (args.result.Values.length == 0) args.result.Values = sender._lastArgs.Values;
        for (var j = 0; j < args.result.Values.length; j++)
            if (args.result.Values[j].Name == sender._keyFields[0].Name) {
            v = args.result.Values[j].NewValue;
            break;
        }
        var t = null;
        for (i = 0; i < sender._lastArgs.Values.length; i++) {
            if (sender._lastArgs.Values[i].Name == sender._fields[0].Name) {
                t = sender._lastArgs.Values[i].NewValue;
                break;
            }
        }
        //if (this._newLookupValueField._lookupDataControllerBehavior) this._newLookupValueField._lookupDataControllerBehavior.goToPage(-1);
        this._createNewView.endModalState('Cancel');
        this._copyLookupValues(null, this._newLookupValueField, sender._lastArgs.Values);
        this.changeLookupValue(this._newLookupValueField.Index, v, t);
    },
    hideLookup: function(fieldIndex) {
        var field = fieldIndex ? this._allFields[fieldIndex] : this.get_lookupField();
        var dv = this.get_lookupField()._dataView;
        dv._closeLookup(field);
        dv._restoreTabIndexes();
        $get(dv.get_id() + '_Item' + field.Index + '_ShowLookupLink').focus();
    },
    closeLookupAndCreateNew: function() {
        this.hideLookup();
        var field = this.get_lookupField();
        field._dataView.createNewLookupValue(field.Index);
    },
    htmlEncode: function(field, s) { var f = this._allFields[field.AliasIndex]; return f.HtmlEncode ? (f.Type == 'String' ? Web.DataView.htmlEncode(s) : s) : s; },
    filterIsExternal: function() {
        if (this._externalFilter.length == 0) return false;
        for (var i = 0; i < this._filter.length; i++) {
            var name = this._filter[i].match(/(\w+):/)[1];
            var found = false;
            for (var j = 0; j < this._externalFilter.length; j++)
                if (this._externalFilter[j].Name == name) {
                found = true;
                break;
            }
            if (!found) return false;
        }
        return true;
    },
    updateSummary: function() {
        if (!this.get_showInSummary()) return;
        var summaryBox = null;
        if (!this._summaryId) {
            var sideBar = $getSideBar();
            if (!sideBar) this._summaryId = '';
            else {
                this._summaryId = 'PageSummary_' + this.get_id();
                summaryBox = $get('PageSummaryBox');
                if (!summaryBox) {
                    summaryBox = document.createElement('div');
                    summaryBox.id = 'PageSummaryBox';
                    summaryBox.className = 'TaskBox';
                    summaryBox.innerHTML = String.format('<div class="Inner"><div class="Summary">{0}</div></div>', Web.DataViewResources.Menu.Summary);
                    sideBar.insertBefore(summaryBox, sideBar.childNodes[sideBar._hasActivators ? 1 : 0]);
                    summaryBox._numberOfVisibleSummaries = 0;
                }
                var viewSummary = $get(this._summaryId);
                if (!viewSummary) {
                    viewSummary = document.createElement('div');
                    viewSummary.id = this._summaryId;
                    summaryBox.childNodes[0].appendChild(viewSummary);
                }
            }
        }
        if (this._summaryId.length > 0) {
            if (!summaryBox) summaryBox = $get('PageSummaryBox');
            if (!this._rows || this._rows.length == 0) {
                Sys.UI.DomElement.setVisible(summaryBox, false);
                return;
            }
            var row = this.get_selectedRow();
            viewSummary = $get(this._summaryId);
            var sb = new Sys.StringBuilder();
            var saveLastCommandName = this._lastCommandName;
            var saveViewType = this.get_view().Type;
            this.get_view().Type = 'Grid';
            this._lastCommandName = null;
            var empty = true;
            if (this._selectedKey.length > 0) {
                var first = true;
                var count = 0;
                for (var i = 0; i < this._allFields.length; i++) {
                    var f = this._allFields[i];
                    if (f.ShowInSummary && !f.Hidden) {
                        empty = false;
                        sb.append('<div class="Field">');
                        if (first)
                            first = false;
                        else
                            sb.append('<div class="Divider"></div>');
                        sb.appendFormat('<div class="Label">{0}</div>', this._allFields[f.AliasIndex].Label);
                        sb.append('<div class="Value">');
                        this._renderItem(sb, f, row, false, false, false, false);
                        sb.append('</div></div>');
                        count++;
                        if (this.get_summaryFieldCount() > 0 && count >= this.get_summaryFieldCount()) break;
                    }
                }
            }
            Sys.UI.DomElement.setVisible(viewSummary, !empty);
            if (empty && this._summaryIsVisible)
                summaryBox._numberOfVisibleSummaries--;
            else if (!empty && !this._summaryIsVisible || !empty && this._summaryIsVisible == null)
                summaryBox._numberOfVisibleSummaries++;
            this._summaryIsVisible = !empty;
            Sys.UI.DomElement.setVisible(summaryBox, summaryBox._numberOfVisibleSummaries > 0);
            var s = sb.toString();
            viewSummary.innerHTML = s;
            var clearPermalink = this._lastArgs != this._lastClearArgs && this._lastArgs.CommandName == 'Delete';
            if ((!empty || clearPermalink) && this.get_filterSource() == null && typeof (Web.Membership) != "undefined") {
                if (clearPermalink) this._lastClearArgs = this._lastArgs;
                Web.Membership._instance.addPermalink(String.format('{0}&_controller={1}&_commandName=Select&_commandArgument=editForm1', this.get_keyRef(), this.get_controller()), clearPermalink ? null : String.format('<div class="TaskBox" style="width:{2}px"><div class="Inner"><div class="Summary">{0}</div>{1}</div></div>', document.title, s, viewSummary.offsetWidth));
            }
            sb.clear();
            this._lastCommandName = saveLastCommandName;
            this.get_view().Type = saveViewType;
        }
    },
    add_selected: function(handler) {
        this.get_events().addHandler('selected', handler);
    },
    remove_selected: function(handler) {
        this.get_events().removeHandler('selected', handler);
    },
    raiseSelected: function(eventArgs) {
        if (Web.DataView._navigated) return;
        var handler = this.get_events().getHandler('selected');
        if (handler) handler(this, Sys.EventArgs.Empty);
        if (this.get_selectionMode() != Web.DataViewSelectionMode.Multiple)
            this.set_selectedValue(this.get_selectedKey());
    },
    add_executed: function(handler) {
        this.get_events().addHandler('executed', handler);
    },
    remove_executed: function(handler) {
        this.get_events().removeHandler('executed', handler);
    },
    raiseExecuted: function(eventArgs) {
        var handler = this.get_events().getHandler('executed');
        if (handler) handler(this, eventArgs);
    },
    _closeLookup: function(field) {
        $closeHovers();
        if (field && field._lookupModalBehavior) {
            field._lookupModalBehavior.hide();
            $removeHandler(document.body, 'keydown', field._lookupDataControllerBehavior._bodyKeydownHandler);
        }
    },
    _collectFieldValues: function(all) {
        if (all == null) all = false;
        var values = new Array();
        var selectedRow = this.get_selectedRow();
        if (!selectedRow && !this.get_isInserting()) return values;
        for (var i = 0; i < this._allFields.length; i++) {
            var field = this._allFields[i];
            var element = $get(this.get_id() + '_Item' + i);
            if (field.ItemsStyle == 'RadioButtonList') {
                var j = 0;
                var option = $get(this.get_id() + '_Item' + i + '_' + j);
                while (option) {
                    if (option.checked) {
                        element = option;
                        break;
                    }
                    j++;
                    option = $get(this.get_id() + '_Item' + i + '_' + j);
                }
            }
            else if (field.ItemsStyle == 'CheckBoxList' && element) {
                element.value = '';
                j = 0;
                option = $get(this.get_id() + '_Item' + i + '_' + j);
                while (option) {
                    if (option.checked) {
                        if (element.value.length > 0) element.value += ',';
                        element.value += option.value;
                    }
                    j++;
                    option = $get(this.get_id() + '_Item' + i + '_' + j);
                }
            }
            if (!field.OnDemand && (element || field.IsPrimaryKey || (!field.ReadOnly || all))) {
                var add = true;
                if (this._lastCommandName == 'BatchEdit') {
                    var cb = $get(String.format('{0}$BatchSelect{1}', this.get_id(), field.Index));
                    add = field.IsPrimaryKey || cb && cb.checked;
                }
                if (add) Array.add(values, { Name: field.Name, OldValue: this.get_isInserting() ? (/*this._newRow ? this._newRow[field.Index] : */null) : selectedRow[field.Index], NewValue: element && element.value ? (field.Type == 'Boolean' ? element.value == 'true' : element.value) : null, Modified: element != null });
            }
        }
        for (i = 0; i < this._externalFilter.length; i++) {
            var filterItem = this._externalFilter[i];
            for (j = 0; j < values.length; j++) {
                if (values[j].Name.toLowerCase() == filterItem.Name.toLowerCase() && values[j].NewValue == null) {
                    values[j].NewValue = filterItem.Value;
                    values[j].Modified = true;
                    break;
                }
            }
        }
        return values;
    },
    _enumerateExpressions: function(type, scope, target) {
        var l = [];
        if (this._expressions) {
            for (var i = 0; i < this._expressions.length; i++) {
                var e = this._expressions[i];
                if (e.Scope == scope && (type == Web.DynamicExpressionType.Any || e.Type == Web.DynamicExpressionType.RegularExpression) && e.Target == target)
                    Array.add(l, e);
            }
        }
        return l;
    },
    _prepareJavaScriptExpression: function(expression) {
        if (!expression._variables) {
            var vars = [];
            var re = /\[(\w+)\]/g;
            var m = re.exec(expression.Test);
            while (m) {
                var found = false;
                for (var i = 0; i < vars.length; i++) {
                    if (vars[i].name == m[1]) {
                        found = true;
                        break;
                    }
                }
                if (!found) Array.add(vars, { 'name': m[1], 'regex': new RegExp('\\[' + m[1] + '\\]', 'g') });
                m = re.exec(expression.Test);
            }
            expression._variables = vars;
        }
    },
    _evaluateJavaScriptExpressions: function(expressions, row, concatenateResult) {
        var result = concatenateResult ? '' : null;
        for (var i = 0; i < expressions.length; i++) {
            var exp = expressions[i];
            if (exp.Type == Web.DynamicExpressionType.ClientScript) {
                this._prepareJavaScriptExpression(exp);
                var script = exp.Test;
                for (var j = 0; j < exp._variables.length; j++) {
                    var v = exp._variables[j];
                    var f = this.findField(v.name);
                    if (f) {
                        var o = row[f.Index];
                        if (String.isInstanceOfType(o))
                            o = '\'' + o.toString().replace('\'', '\\\'') + '\'';
                        else if (Date.isInstanceOfType(o))
                            o = 'parseDate(\'' + o + '\')';
                        script = script.replace(v.regex, o);
                    }
                    else {
                        script = null;
                        break;
                    }
                }
                if (script) {
                    try {
                        var r = eval(script);
                        if (concatenateResult) {
                            if (r) {
                                if (result == null) result = exp.Result;
                                else result += ' ' + exp.Result;
                            }
                        }
                        else
                            return exp.Result == null ? r : exp.Result;
                    }
                    catch (er) {
                    }
                }
            }
        }
        return result;
    },
    _validateFieldValues: function(values, displayErrors) {
        var valid = true;
        var sb = new Sys.StringBuilder();
        for (var i = 0; i < values.length; i++) {
            var v = values[i];
            var field = this.findField(v.Name);
            if (field.Type == 'DateTime' && v.OldValue != null && v.OldValue.getTimezoneOffset) v.OldValue = new Date(v.OldValue - v.OldValue.getTimezoneOffset() * 60 * 1000);
            var error = null;
            if (v.Modified) {
                // see if the field is blank
                if (!field.AllowNulls && (!field.HasDefaultValue || Web.DataViewResources.Validator.EnforceRequiredFieldsWithDefaultValue)) {
                    if (String.isBlank(v.NewValue) && !field.Hidden)
                        error = Web.DataViewResources.Validator.RequiredField;
                }
                // convert blank values to "null"
                if (!error && String.isBlank(v.NewValue))
                    v.NewValue = null;
                // convert to the "typed" value
                if (!error && v.NewValue != null && !field.IsMirror && !field.Hidden) {
                    switch (field.Type) {
                        case 'SByte':
                        case 'Byte':
                        case 'Int16':
                        case 'Int32':
                        case 'UInt32':
                        case 'Int64':
                        case 'Single':
                        case 'Double':
                        case 'Decimal':
                        case 'Currency':
                            var newValue = v.NewValue && typeof (v.NewValue) == 'string' ? Number.parseLocale(v.NewValue.replace(Sys.CultureInfo.CurrentCulture.numberFormat.CurrencySymbol, '')) : v.NewValue;
                            if (String.format('{0}', newValue) == 'NaN')
                                error = Web.DataViewResources.Validator.NumberIsExpected;
                            else
                                v.NewValue = newValue;
                            break;
                        case 'Boolean':
                            try {
                                v.NewValue = String.isInstanceOfType(v.NewValue) ? Boolean.parse(v.NewValue) : v.NewValue;
                            }
                            catch (e) {
                                error = Web.DataViewResources.Validator.BooleanIsExpected;
                            }
                            break;
                        case 'DateTime':
                            //case 'DateTimeOffset':
                            newValue = field.DataFormatString && field.DataFormatString.length ? Date.parseLocale(v.NewValue, field.DataFormatString.match(/\{0:([\s\S]*?)\}/)[1]) : Date.parse(v.NewValue);
                            if (!newValue)
                                error = Web.DataViewResources.Validator.DateIsExpected;
                            else
                                v.NewValue = newValue;
                            if (!error && v.NewValue != null) v.NewValue = new Date(v.NewValue - v.NewValue.getTimezoneOffset() * 60 * 1000);
                            break;
                    }
                }
                if (!error) {
                    var expressions = this._enumerateExpressions(Web.DynamicExpressionType.RegularExpression, Web.DynamicExpressionScope.Field, v.Name);
                    for (var j = 0; j < expressions.length; j++) {
                        var exp = expressions[j];
                        var s = v.NewValue ? v.NewValue : '';
                        try {
                            var re = new RegExp(exp.Test);
                            var m = re.exec(s);
                            if (exp.Result.match(/\$(\d|\'\`)/)) {
                                if (m) v.NewValue = s.replace(re, exp.Result);
                            }
                            else {
                                if (!m) error = error ? error += exp.Result : exp.Result;
                            }
                        }
                        catch (ex) {
                        }
                    }
                }
                // see if the value has been modified
                v.Modified = field.Type.startsWith('DateTime') ? ((v.NewValue == null ? null : v.NewValue.toString()) != (v.OldValue == null ? null : v.OldValue.toString())) : v.NewValue != v.OldValue;
            }
            // display/hide the error as needed
            var errorElement = $get(this.get_id() + '_Item' + field.Index + '_Error');
            if (errorElement && displayErrors) {
                Sys.UI.DomElement.setVisible(errorElement, error != null);
                errorElement.innerHTML = error;
            }
            if (error && displayErrors) {
                if (valid) this._focus(field.Name);
                valid = false;
                if (!errorElement) sb.append(Web.DataView.formatMessage('Attention', field.Label + ": " + error));
            }
        }
        if (!displayErrors) valid = true;
        if (!valid) Web.DataView.showMessage(sb.toString());
        sb.clear();
        return valid;
    },
    _fieldIsInExternalFilter: function(field) {
        return this._findExternalFilterItem(field) != null;
    },
    _findExternalFilterItem: function(field) {
        for (var i = 0; i < this._externalFilter.length; i++) {
            var filterItem = this._externalFilter[i];
            if (field.Name.toLowerCase() == filterItem.Name.toLowerCase())
                return filterItem;
        }
        return null;
    },
    _focus: function(fieldName, message) {
        if (String.isNullOrEmpty(fieldName) && !String.isNullOrEmpty(this._focusedFieldName)) {
            var field = this.findField(this._focusedFieldName);
            if (field) fieldName = field.Name;
        }
        if (message) {
            for (var i = 0; i < this.get_fields().length; i++) {
                field = this.get_fields()[i];
                var error = $get(this.get_id() + '_Item' + field.Index + '_Error');
                if (error) Sys.UI.DomElement.setVisible(error, false);
            }
        }
        for (i = 0; i < this.get_fields().length; i++) {
            field = this.get_fields()[i];
            if (!field.ReadOnly && (fieldName == null || field.Name == fieldName)) {
                var elemId = this.get_id() + '_Item' + field.Index;
                switch (field.ItemsStyle) {
                    case 'RadioButtonList':
                    case 'CheckBoxList':
                        elemId += '_0';
                        break;
                    case 'Lookup':
                        elemId += '_ShowLookupLink';
                        break;
                }
                var element = $get(elemId);
                var c = $get(String.format('{0}_ItemContainer{1}', this.get_id(), field.Index));
                var cat = this._categories[field.CategoryIndex];
                var categoryTabIndex = Array.indexOf(this._tabs, cat.Tab);
                if (fieldName && categoryTabIndex >= 0) this.set_categoryTabIndex(categoryTabIndex);
                if (element && (!c || Sys.UI.DomElement.getVisible(c))) {
                    if (fieldName || (categoryTabIndex == this.get_categoryTabIndex() || this._tabs.length == 0)) {
                        if (categoryTabIndex >= 0) this.set_categoryTabIndex(categoryTabIndex);
                        try {
                            if (message) {
                                error = $get(this.get_id() + '_Item' + field.Index + '_Error');
                                if (error) {
                                    error.innerHTML = message;
                                    Sys.UI.DomElement.setVisible(error, true);
                                }
                            }
                            if (element.select) element.select();
                            element.focus();
                        } catch (ex) { }
                        break;
                    }
                }
            }
        }
    },
    _saveTabIndexes: function() {
        this._lastSavedTabIndexes = this._savedTabIndexes;
        this._savedTabIndexes = [];
        for (var i = 0; i < Web.DataView._tagsWithIndexes.length; i++) {
            var tags = document.getElementsByTagName(Web.DataView._tagsWithIndexes[i]);
            for (var j = 0; j < tags.length; j++) {
                var elem = tags[j];
                if (elem)
                    Array.add(this._savedTabIndexes, { element: elem, tabIndex: elem.tabIndex });
            }
        }
    },
    _restoreTabIndexes: function() {
        if (this._savedTabIndexes) {
            for (var i = 0; i < this._savedTabIndexes.length; i++) {
                this._savedTabIndexes[i].element.tabIndex = this._savedTabIndexes[i].tabIndex;
                delete this._savedTabIndexes[i].element;
            }
            Array.clear(this._savedTabIndexes);
        }
        this._savedTabIndexes = this._lastSavedTabIndexes;
        this._lastSavedTabIndexes = null;
    },
    _selectKeyByRowIndex: function(rowIndex) {
        var oldKey = this._selectedKey;
        this._selectedKey = [];
        this._selectedKeyFilter = []
        for (var i = 0; i < this._keyFields.length; i++) {
            var field = this._keyFields[i];
            Array.add(this._selectedKey, this._rows[rowIndex][field.Index]);
            Array.add(this._selectedKeyFilter, field.Name + ':=' + this.convertFieldValueToString(field, this._rows[rowIndex][field.Index]));
            if (oldKey && (oldKey.length < i || (oldKey[i] != this._selectedKey[i]))) oldKey = null;
        }
        this.updateSummary();
        if (!oldKey && !this._raiseSelectedDelayed) this.raiseSelected();
    },
    _showWait: function(force) {
        if (this.get_fields() == null || force)
            this._container.innerHTML = Web.DataViewResources.Common.WaitHtml;
        else {
            var wait = $get(this.get_id() + '_Wait');
            if (wait) {
                this._oldWaitHTML = wait.innerHTML;
                wait.innerHTML = Web.DataViewResources.Common.WaitHtml;
            }
        }
    },
    _hideWait: function() {
        if (this._oldWaitHTML) {
            var waitElement = $get(this.get_id() + '_Wait');
            if (waitElement) waitElement.innerHTML = this._oldWaitHTML;
        }
    },
    _get_colSpan: function() {
        return this.get_view().Type == 'Form' ? 2 : this.get_fields().length + (this._selectionMode == Web.DataViewSelectionMode.Multiple ? 1 : 0) + (this.get_showIcons() ? 1 : 0);
    },
    _renderCreateNewBegin: function(sb, field) { if (!String.isNullOrEmpty(field.ItemsNewDataView)) sb.append('<table cellpadding="0" cellspacing="0"><tr><td>'); },
    _renderCreateNewEnd: function(sb, field) {
        if (!String.isNullOrEmpty(field.ItemsNewDataView)) {
            sb.append('</td><td>');
            if (this.get_enabled())
                sb.appendFormat('<a href="#" class="CreateNew" onclick="$find(\'{0}\').createNewLookupValue({1});return false" id="{0}_Item{1}_CreateNewLookupLink" title="{2}"{3}>&nbsp;</a>',
                    this.get_id(), field.Index, String.format(Web.DataViewResources.Lookup.NewToolTip, field.Label), String.format(' tabindex="{0}"', $nextTabIndex()));
            sb.append('</td></tr></table>');
        }
    },
    _raisePopulateDynamicLookups: function() {
        if (this._hasDynamicLookups && this.get_isEditing() && this._skipPopulateDynamicLookups != true)
            this.executeCommand({ 'commandName': 'PopulateDynamicLookups', 'commandArgument': '', 'causesValidation': false });
        this._skipPopulateDynamicLookups = false;
    },
    _raiseCalculate: function(field) {
        this.executeCommand({ 'commandName': 'Calculate', 'commandArgument': field.Name, 'causesValidation': false });
    },
    get_currentRow: function() {
        return this.get_isInserting() ? (this._newRow ? this._newRow : []) : this.get_selectedRow()
    },
    _useLEVs: function(row) {
        if (row && this._allowLEVs) {
            var r = this._get_LEVs();
            if (r.length > 0) {
                for (i = 0; i < r[0].length; i++) {
                    var v = r[0][i];
                    f = this.findField(v.Name);
                    if (f) {
                        if (this._lastCommandName == 'New' && v.Modified && v.NewValue != null) row[f.Index] = v.NewValue;
                        f._LEV = v.Modified ? v.NewValue : null;
                    }
                }
            }
        }
    },
    _configure: function(row) {
        if (!this._requiresConfiguration) return;
        if (!row) row = this.get_currentRow();
        if (!row) return;
        for (var i = 0; i < this._allFields.length; i++) {
            var f = this._allFields[i];
            if (!String.isNullOrEmpty(f.Configuration)) {
                var iterator = /\s*(\w+)=(\w+)\s*?($|\n)/g;
                var m = iterator.exec(f.Configuration);
                while (m) {
                    var sourceField = this.findField(m[2]);
                    if (sourceField) {
                        var v = row[sourceField.Index];
                        if (v) f[m[1]] = v;
                    }
                    m = iterator.exec(f.Configuration);
                }
            }
        }
    },
    _focusQuickFind: function(force) {
        if (!this._quickFindFocused || force == true) {
            try {
                this.get_quickFindElement().select();
                this.get_quickFindElement().focus();
            }
            catch (ex) {
            }
            this._quickFindFocused = true;
        }
    },
    _render: function() {
        this._detachBehaviors();
        var checkWidth = this.get_view() && this.get_view().Type == 'Form' && this._numberOfColumns > 1;
        var width = this.get_element().offsetWidth;
        this._useLEVs();
        this._configure();
        this._internalRender();
        if (checkWidth && width < this.get_element().offsetWidth) {
            var oldNumberOrColumns = this._numberOfColumns;
            this._numberOfColumns = 1;
            this._ignoreColumnIndex = true;
            this._internalRender(sb);
            this._numberOfColumns = oldNumberOrColumns;
            this._ignoreColumnIndex = false;
        }
        this._raisePopulateDynamicLookups();
        if (!this.get_modalAnchor() && this.get_lookupField())
            this._focusQuickFind();
        if (this._raiseSelectedDelayed) {
            this._raiseSelectedDelayed = false;
            this.raiseSelected();
        }
        if (this._scrollIntoView) {
            this._scrollIntoView = false;
            var bounds = $common.getBounds(this._element);
            var scrolling = $common.getScrolling();
            if (bounds.y < scrolling.y)
                this._element.scrollIntoView(true);
        }
    },
    _mergeRowUpdates: function(row) {
        this._originalRow = null;
        if (!this._ditto) this._useLEVs(row);
        if (this.get_isEditing() && this._ditto) {
            this._originalRow = Array.clone(row);
            for (var i = 0; i < this._ditto.length; i++) {
                var d = this._ditto[i];
                var f = this.findField(d.name);
                if (f && !(f.ReadOnly && f.IsPrimaryKey))
                    row[f.Index] = d.value;
            }
            delete this._ditto;
        }
        this._configure(row);
        this._mergedRow = row;
    },
    _removeRowUpdates: function() {
        var row = this._mergedRow;
        if (!row) return;
        if (this._originalRow) {
            for (var i = 0; i < this._originalRow.length; i++)
                row[i] = this._originalRow[i];
        }
        this._mergedRow = null;
    },
    _internalRender: function() {
        this._multipleSelection = null;
        var sb = new Sys.StringBuilder();
        if (this.get_mode() == Web.DataViewMode.Lookup) {
            var field = this._fields[0];
            var v = this.get_lookupText();
            if (v == null) v = Web.DataViewResources.Lookup.SelectLink;
            var s = field.format(v);
            this._renderCreateNewBegin(sb, field);
            sb.appendFormat('<table cellpadding="0" cellspacing="0" class="DataViewLookup"><tr><td><a href="#" onclick="$find(\'{0}\').showLookup({1});return false" id="{0}_Item{1}_ShowLookupLink" title="{3}" tabindex="{7}"{8}>{2}</a><a href="#" class="Clear" onclick="$find(\'{0}\').clearLookupValue({1});return false" id="{0}_Item{1}_ClearLookupLink" title="{5}" tabindex="{7}">&nbsp;</a></td></tr></table><input type="hidden" id="{0}_Item{1}" value="{4}"/><input type="hidden" id="{0}_Text{1}" value="{6}"/>',
                this.get_id(), field.Index, this.htmlEncode(field, s), String.format(Web.DataViewResources.Lookup.SelectToolTip, field.Label), this.get_lookupValue(), String.format(Web.DataViewResources.Lookup.ClearToolTip, field.Label), Web.DataView.htmlAttributeEncode(s), $nextTabIndex(), this.get_enabled() ? '' : ' disabled="true" class="Disabled"');
            this._renderCreateNewEnd(sb, field);
            this.get_element().appendChild(this._container);
            this._container.innerHTML = sb.toString();
            if (this.get_lookupValue() == '' || !this.get_enabled()) $get(this.get_id() + '_Item0_ClearLookupLink').style.display = 'none';
        }
        else {
            sb.appendFormat('<table class="DataView {1}_{2}{3}{4}" cellpadding="0" cellspacing="0"{0}>', this.get_isModal() ? String.format(' style="width:{0}px"', this._container.offsetWidth - 20) : '', this.get_controller(), this.get_viewId(), this._numberOfColumns > 0 ? ' MultiColumn' : '', this._tabs.length > 0 ? ' Tabbed' : '');
            if (this.get_view().Type == 'Form')
                this._renderFormView(sb);
            else
                this._renderGridView(sb);
            sb.append('</table>');
            if (this._mergedRow) {
                for (var i = 0; i < this._allFields.length; i++) {
                    var f = this._allFields[i];
                    if (f.Hidden && !f.IsPrimaryKey) {
                        v = this._mergedRow[i];
                        sb.appendFormat('<input id="{0}_Item{1}" type="hidden" value="{2}"/>', this.get_id(), i, Web.DataView.htmlAttributeEncode(v != null ? v : ''));
                    }
                }
            }
            this._container.innerHTML = sb.toString();
            if (this._multipleSelection != null && this._multipleSelection == true)
                $get(this.get_id() + '_ToggleButton').checked = true;
            this._attachBehaviors();
            this._updateVisibility();
            if (this.get_isEditing()) this._focus();
        }
        sb.clear();
        this._removeRowUpdates();
    },
    _renderFormView: function(sb) {
        this._renderViewDescription(sb);
        if (Web.DataViewResources.Form.ShowActionBar) this._renderActionBar(sb);
        var row = this.get_currentRow();
        this._mergeRowUpdates(row);
        if (this.get_isInserting() && this._expressions) {
            for (i = 0; i < this._expressions.length; i++) {
                var exp = this._expressions[i];
                if (exp.Scope == Web.DynamicExpressionScope.DefaultValues && exp.Type == Web.DynamicExpressionType.ClientScript) {
                    f = this.findField(exp.Target);
                    if (f) {
                        if (String.isNullOrEmpty(exp.Test))
                            row[f.Index] = exp.Result;
                        else {
                            var r = eval(exp.Test);
                            if (r)
                                row[f.Index] = String.isNullOrEmpty(exp.Result) ? r : exp.Result;
                        }
                    }
                }
            }
        }
        var fieldCount = 0;
        for (i = 0; i < this._allFields.length; i++)
            if (!this._allFields[i].Hidden) fieldCount++;
        var hasButtonsOnTop = fieldCount > Web.DataViewResources.Form.SingleButtonRowFieldLimit && row;
        if (hasButtonsOnTop) this._renderActionButtons(sb, 'Top', 'Form');
        var selectedTab = this._tabs.length > 0 ? this._tabs[this.get_categoryTabIndex()] : null;
        if (this._tabs.length > 0) {
            sb.appendFormat('<tr class="TabsRow"><td colspan="{0}" class="TabsBar{1}">', this._get_colSpan(), !hasButtonsOnTop ? ' WithMargin' : '');
            sb.append('<table cellpadding="0" cellspacing="0" class="Tabs"><tr>');
            for (i = 0; i < this._tabs.length; i++)
                sb.appendFormat('<td id="{2}_Tab{3}" class="Tab{1}" onmouseover="$hoverTab(this,true)" onmouseout="$hoverTab(this,false)"><span class="Outer"><span class="Inner"><span class="Tab"><a href="javascript:" onclick="$find(&quot;{2}&quot;).set_categoryTabIndex({3})" onfocus="$hoverTab(this,true)" onblur="$hoverTab(this,false)" tabindex="{4}">{0}</a></span></span></span></td>', Web.DataView.htmlEncode(this._tabs[i]), i == this.get_categoryTabIndex() ? ' Selected' : '', this.get_id(), i, $nextTabIndex());
            sb.append('</tr></table></td></tr>');
        }
        if (!row) this._renderNoRecordsWhenNeeded(sb);
        else {
            var t = this._get_template();
            if (t) {
                sb.appendFormat('<tr class="CategoryRow"><td valign="top" class="Fields" colspan="{0}">', this._get_colSpan());
                this._renderTemplate(t, sb, row, true, false);
                sb.append('</td></tr>');
            }
            else {
                var categories = this.get_categories();
                var numCols = this._numberOfColumns;
                if (numCols > 0) {
                    sb.appendFormat('<tr class="Categories"><td class="Categories" colspan="{0}"><table class="Categories"><tr class="CategoryRow">', this._get_colSpan());
                    for (var k = 0; k < numCols; k++) {
                        if (k > 0)
                            sb.append('<td class="CategorySeparator">&nbsp;</td>');
                        sb.appendFormat('<td class="CategoryColumn" valign="top" style="width:{0}%">', 100 / numCols);
                        for (i = 0; i < categories.length; i++) {
                            var c = categories[i];
                            if (c.ColumnIndex == k || this._ignoreColumnIndex) {
                                sb.appendFormat('<div id="{0}_Category{1}" class="Category" style="display:{2}">', this.get_id(), i, !selectedTab || selectedTab == c.Tab ? 'block' : 'none');
                                sb.appendFormat('<table class="Category" cellpadding="0" cellspacing="0"><tr><td class="HeaderText">{0}</td></tr><tr><td class="Description">{1}</td></tr></table>', c.HeaderText, this._formatViewText(Web.DataViewResources.Views.DefaultCategoryDescriptions[c.Description], true, c.Description));
                                sb.append('<table class="Fields"><tr class="FieldsRow"><td class="Fields" valign="top" width="100%">');

                                if (!String.isNullOrEmpty(c.Template))
                                    this._renderTemplate(c.Template, sb, row, true, false);
                                else {
                                    for (j = 0; j < this.get_fields().length; j++) {
                                        var field = this.get_fields()[j];
                                        if (!field.Hidden && field.CategoryIndex == c.Index) {
                                            sb.appendFormat('<table cellpadding="0" cellspacing="0" class="FieldWrapper"><tr class="FieldWrapper"><td class="Header" valign="top">');
                                            this._renderItem(sb, field, row, true, false, false, true);
                                            sb.appendFormat('</td><td class="Item{0}" valign="top">', this.get_isEditing() && !field.ReadOnly ? '' : ' ReadOnly');
                                            this._renderItem(sb, field, row, true);
                                            sb.append('</td></tr></table>');
                                        }
                                    }
                                }
                                sb.append('</td></tr></table>');
                                sb.append('</div>');
                            }
                        }
                        sb.append('</td>');
                    }
                    sb.append('</tr></table></td></tr>');
                }
                else {
                    for (i = 0; i < categories.length; i++) {
                        c = categories[i];
                        sb.appendFormat('<tr class="CategoryRow" id="{2}_Category{3}" style="display:{4}"><td valign="top" class="Category"><table class="Category" cellpadding="0" cellspacing="0"><tr><td class="HeaderText">{0}</td></tr><tr><td class="Description">{1}</td></tr></table></td><td valign="top" class="Fields">', c.HeaderText, this._formatViewText(Web.DataViewResources.Views.DefaultCategoryDescriptions[c.Description], true, c.Description), this.get_id(), i, !selectedTab || selectedTab == c.Tab ? 'block' : 'none');
                        if (!String.isNullOrEmpty(c.Template))
                            this._renderTemplate(c.Template, sb, row, true, false);
                        else {
                            for (j = 0; j < this.get_fields().length; j++) {
                                field = this.get_fields()[j];
                                if (!field.Hidden && field.CategoryIndex == c.Index)
                                    this._renderItem(sb, field, row, true);
                            }
                        }
                        sb.append('</td></tr>');
                    }
                }
            }
        }
        if (row) this._renderActionButtons(sb, 'Bottom', 'Form');
    },
    _updateTabbedCategoryVisibility: function() {
        if (this._tabs && this._tabs.length > 0) {
            var tab = this._tabs[this.get_categoryTabIndex()];
            for (var i = 0; i < this._tabs.length; i++) {
                var elem = $get(String.format('{0}_Tab{1}', this.get_id(), i));
                if (elem) {
                    if (i == this.get_categoryTabIndex())
                        Sys.UI.DomElement.addCssClass(elem, 'Selected');
                    else
                        Sys.UI.DomElement.removeCssClass(elem, 'Selected');
                }
            }
            for (i = 0; i < this._categories.length; i++) {
                var c = this._categories[i];
                elem = $get(String.format('{0}_Category{1}', this.get_id(), i));
                if (elem) Sys.UI.DomElement.setVisible(elem, c.Tab == tab);
            }
        }
    },
    _renderItem: function(sb, field, row, isSelected, isInlineForm, isFirstRow, headerOnly) {
        var isForm = this.get_view().Type == 'Form' || isInlineForm;
        var v = row[field.Index];
        if (v != null) v = v.toString();
        var checkBox = null;
        var isEditing = this.get_isEditing();
        if (isEditing && field.ItemsStyle == 'CheckBox' && field.Items.length == 2) {
            var fv = field.Items[0][0];
            var tv = field.Items[1][0];
            if (fv == 'true') {
                fv = 'false';
                tv = 'true';
            }
            if (v == null) v = field.Items[0][0];
            checkBox = String.format('<input type="checkbox" id="{0}_Item{1}"{2} tabindex="{3}" value="{4}" onclick="this.value=this.checked?\'{6}\':\'{5}\';$find(&quot;{0}&quot;)._valueChanged({1});" onfocus="$find(&quot;{0}&quot;)._valueFocused({1});"/>', this.get_id(), field.Index, tv && (v == 'true' || v == tv) ? ' checked' : '', $nextTabIndex(), v, fv, tv);
        }
        if (isForm) {
            var errorHtml = String.format('<div class="Error" id="{0}_Item{1}_Error" style="display:none"></div>', this.get_id(), field.Index);
            if (!headerOnly) sb.appendFormat('<div class="Item {2}" id="{0}_ItemContainer{1}">', this.get_id(), field.Index, field.Name);
            if (this._numberOfColumns == 0) sb.append(errorHtml);
            var headerText = this._allFields[field.AliasIndex].HeaderText;
            if (headerText.length > 0)
                sb.appendFormat('<div class="Header">{3}<label for="{0}_Item{1}">{2}{4}</label>{5}</div>', this.get_id(), field.Index, headerText, checkBox, isEditing && !field.AllowNulls && !checkBox && !field.ReadOnly && Web.DataViewResources.Form.RequiredFieldMarker ? Web.DataViewResources.Form.RequiredFieldMarker : '');
            if (headerOnly) return;
            if (checkBox == null)
                sb.append('<div class="Value">');
        }
        var needObjectRef = !isEditing && field.ItemsDataController && field.ItemsDataController.length > 0 && field.ItemsStyle != 'CheckBoxList' && !isFirstRow && v;
        if (needObjectRef && !isForm) sb.append('<table width="100%" cellpadding="0" cellspacing="0" class="ObjectRef"><tr><td>');
        if (isEditing && isSelected && !field.ReadOnly) {
            if (field._LEV != null)
                sb.append('<table cellpadding="0" cellspacing="0"><tr><td>');
            if (!isForm && checkBox) sb.append(checkBox);
            var lov = field.DynamicItems ? field.DynamicItems : field.Items;
            if (!String.isNullOrEmpty(field.ItemsStyle) && field.ItemsStyle != 'Lookup' && lov.length == 0 && !String.isNullOrEmpty(field.ContextFields)) {
                lov = [];
                if (field.AllowNulls)
                    Array.add(lov, ['', Web.DataViewResources.Data.NullValueInForms]);
                if (v != null)
                    Array.add(lov, [v, row[field.AliasIndex]]);
            }
            else if (field.DynamicItems) {
                var hasValue = false;
                for (var i = 0; i < lov.length; i++) {
                    if (lov[i][0] == v) {
                        hasValue = true;
                        break;
                    }
                }
                //if (!hasValue && !String.isNullOrEmpty(v)) Array.insert(lov, 0, [v, row[field.AliasIndex]]);
                if (!hasValue) v = null;
                if ((field.AllowNulls || !hasValue) && !String.isNullOrEmpty(lov[0][0]))
                    Array.insert(lov, 0, ['', Web.DataViewResources.Data.NullValueInForms]);
            }
            if (checkBox == null)
                if (lov.length > 0) {
                if (field.ItemsStyle == 'RadioButtonList') {
                    sb.append('<table cellpadding="0" cellspacing="0" class="RadioButtonList">');
                    var columns = field.Columns == 0 ? 1 : field.Columns;
                    var rows = Math.floor(lov.length / columns) + (lov.length % columns > 0 ? 1 : 0);
                    for (var r = 0; r < rows; r++) {
                        sb.append('<tr>');
                        for (var c = 0; c < columns; c++) {
                            var itemIndex = c * rows + r; //r * columns + c;
                            if (itemIndex < lov.length) {
                                var item = lov[itemIndex];
                                var itemValue = item[0] == null ? '' : item[0].toString();
                                if (v == null) v = '';
                                sb.appendFormat(
                                        '<td class="Button"><input type="radio" id="{0}_Item{1}_{2}" name="{0}_Item{1}" value="{3}"{4} tabindex="{6}" onclick="$find(&quot;{0}&quot;)._valueChanged({1})"  onfocus="$find(&quot;{0}&quot;)._valueFocused({1});"/></td><td class="Option"><label for="{0}_Item{1}_{2}">{5}<label></td>',
                                        this.get_id(), field.Index, itemIndex, itemValue, itemValue == v ? " checked" : "", this.htmlEncode(field, item[1]), $nextTabIndex());
                            }
                            else
                                sb.append('<td class="Button">&nbsp;</td><td class="Option"></td>');
                        }
                        sb.append('</tr>');
                    }
                    sb.append('</table>');
                }
                else if (field.ItemsStyle == 'CheckBoxList') {
                    var lov2 = v ? v.split(',') : [];
                    sb.appendFormat('<input type="hidden" id="{0}_Item{1}" name="{0}_Item{1}" value=""/>', this.get_id(), field.Index);
                    sb.append('<table cellpadding="0" cellspacing="0" class="RadioButtonList">');
                    columns = field.Columns == 0 ? 1 : field.Columns;
                    rows = Math.floor(lov.length / columns) + (lov.length % columns > 0 ? 1 : 0);
                    for (r = 0; r < rows; r++) {
                        sb.append('<tr>');
                        for (c = 0; c < columns; c++) {
                            itemIndex = c * rows + r; //r * columns + c;
                            if (itemIndex < lov.length) {
                                item = lov[itemIndex];
                                itemValue = item[0] == null ? '' : item[0].toString();
                                if (v == null) v = '';
                                sb.appendFormat(
                                        '<td class="Button"><input type="checkbox" id="{0}_Item{1}_{2}" name="{0}_Item{1}" value="{3}"{4} tabindex="{6}"  onclick="$find(&quot;{0}&quot;)._valueChanged({1})" onfocus="$find(&quot;{0}&quot;)._valueFocused({1});"/></td><td class="Option"><label for="{0}_Item{1}_{2}">{5}<label></td>',
                                        this.get_id(), field.Index, itemIndex, itemValue, Array.indexOf(lov2, itemValue) != -1 ? " checked" : "", this.htmlEncode(field, item[1]), $nextTabIndex());
                            }
                            else
                                sb.append('<td class="Button">&nbsp;</td><td class="Option"></td>');
                        }
                        sb.append('</tr>');
                    }
                    sb.append('</table>');
                }
                else {
                    sb.appendFormat('<select id="{0}_Item{1}" size="{2}" tabindex="{3}" onchange="$find(&quot;{0}&quot;)._valueChanged({1});" onfocus="$find(&quot;{0}&quot;)._valueFocused({1});">', this.get_id(), field.Index, field.ItemsStyle == 'ListBox' ? (field.Rows == 0 ? 5 : field.Rows) : '1', $nextTabIndex());
                    if (v == null) v = '';
                    v = v.toString();
                    for (i = 0; i < lov.length; i++) {
                        item = lov[i];
                        itemValue = item[0];
                        if (itemValue == null) itemValue = '';
                        itemValue = itemValue.toString();
                        sb.appendFormat('<option value="{0}"{1}>{2}</option>', itemValue, itemValue == v ? ' selected' : '', this.htmlEncode(field, item[1]));
                    }
                    sb.append('</select>');
                }
            }
            else if (field.ItemsDataController && field.ItemsDataController.length > 0) {
                v = row[field.AliasIndex];
                if (v == null) v = Web.DataViewResources.Lookup.SelectLink;
                var s = field.format(v);
                this._renderCreateNewBegin(sb, field);
                sb.appendFormat('<table cellpadding="0" cellspacing="0" class="Lookup"><tr><td><a href="#" onclick="$find(\'{0}\').showLookup({1});return false" id="{0}_Item{1}_ShowLookupLink" title="{3}" tabindex="{5}">{2}</a><a href="#" class="Clear" onclick="$find(\'{0}\').clearLookupValue({1});return false" id="{0}_Item{1}_ClearLookupLink" title="{7}" tabindex="{6}" style="display:{8}">&nbsp;</a></td></tr></table><input type="hidden" id="{0}_Item{1}" value="{4}"/><input type="hidden" id="{0}_Item{9}" value="{2}"/>',
                    this.get_id(), field.Index, this.htmlEncode(field, s), String.format(Web.DataViewResources.Lookup.SelectToolTip, field.Label), row[field.Index], $nextTabIndex(), $nextTabIndex(), String.format(Web.DataViewResources.Lookup.ClearToolTip, field.Label), row[field.Index] != null ? 'display' : 'none', field.AliasIndex);
                this._renderCreateNewEnd(sb, field);
            }
            else if (field.OnDemand) this._renderOnDemandItem(sb, field, row, isSelected, isForm);
            else if (field.Rows > 1) {
                sb.appendFormat('<textarea id="{0}_Item{1}" tabindex="{2}"  onchange="$find(&quot;{0}&quot;)._valueChanged({1})"  onfocus="$find(&quot;{0}&quot;)._valueFocused({1});" style="', this.get_id(), field.Index, $nextTabIndex());
                if (field.TextMode == 3 && !String.isNullOrEmpty(v))
                    sb.append('display:none;');
                if (!isForm)
                    sb.append('display:block;width:90%"');
                else
                    sb.appendFormat('" cols="{0}"', field.Columns > 0 ? field.Columns : 50);
                sb.appendFormat(' rows="{0}"', field.Rows);
                sb.append('>');
                sb.append(field.HtmlEncode ? this.htmlEncode(field, v) : v);
                sb.append('</textarea>');
                if (field.TextMode == 3 && !String.isNullOrEmpty(v))
                    sb.appendFormat('<div>{2}<div><a href="javascript:" onclick="var o=$get(\'{0}_Item{1}\');o.style.display=\'block\';o.focus();this.parentNode.parentNode.style.display=\'none\';return false;">{3}</a> | <a href="javascript:" onclick="if(!confirm(\'{5}\'))return;$get(\'{0}_Item{1}\').value=\'\';this.parentNode.parentNode.parentNode.parentNode.style.display=\'none\';return false;">{4}</a></div></div>', this.get_id(), field.Index, this.htmlEncode(field, v).replace(/(\r\n*)/g, '<br/>'), Web.DataViewResources.Data.NoteEditLabel, Web.DataViewResources.Data.NoteDeleteLabel, Web.DataViewResources.Data.NoteDeleteConfirm);
            }
            else {
                sb.appendFormat('<input type="{3}" id="{0}_Item{1}" tabindex="{2}" onchange="$find(&quot;{0}&quot;)._valueChanged({1})"  onfocus="$find(&quot;{0}&quot;)._valueFocused({1});"', this.get_id(), field.Index, $nextTabIndex(), field.TextMode != 1 ? 'text' : 'password');
                if (!isForm)
                    sb.append(' style="display:block;width:90%;"');
                else
                    sb.appendFormat(' size="{0}"', field.Columns > 0 ? field.Columns : 50);
                v = row[field.Index];
                if (v == null)
                    s = '';
                else if (field.AliasIndex != field.Index)
                    s = v.toString();
                else
                    s = field.format(v);
                sb.appendFormat(' value="{0}" {1}', Web.DataView.htmlAttributeEncode(s), isForm ? '' : String.format('class="{0}Type"', field.Type));
                sb.append('/>');
                if (field.Type.startsWith('DateTime') && isForm && Web.DataViewResources.Form.ShowCalendarButton)
                    sb.appendFormat('<a id="{0}_Item{1}_Button" href="#" onclick="return false" class="Calendar" tabindex="{2}">&nbsp;</a>', this.get_id(), field.Index, $nextTabIndex());
            }
            if (field._LEV != null)
                sb.appendFormat('</td><td class="UseLEV"><a href="javascript:" onclick="$find(\'{0}\')._applyLEV({1});return false" tabindex="{2}" title="{3}">&nbsp;</a></td></tr></table>', this.get_id(), field.Index, $nextTabIndex(), Web.DataView.htmlAttributeEncode(String.format(Web.DataViewResources.Data.UseLEV, field.format(this._allFields[field.AliasIndex]._LEV))));
            if (this._lastCommandName == 'BatchEdit')
                sb.appendFormat('<div class="BatchSelect"><table cellpadding="0" cellspacing="0"><tr><td><input type="checkbox" id="{0}$BatchSelect{1}" onclick="Web.DataView._updateBatchSelectStatus(this,{3})"/></td><td><label for="{0}$BatchSelect{1}">{2}</a></td></tr></table></div>', this.get_id(), field.Index, Web.DataViewResources.Data.BatchUpdate, isForm == true);
        }
        else {
            if (field.OnDemand) this._renderOnDemandItem(sb, field, row, isSelected, isForm);
            else {
                v = this.htmlEncode(field, row[field.AliasIndex]);
                if (field.Items.length == 0) {
                    if (field.Type == 'String' && v != null && v.length > Web.DataViewResources.Data.MaxReadOnlyStringLen && field.TextMode != 3)
                        v = v.substring(0, Web.DataViewResources.Data.MaxReadOnlyStringLen) + '...';
                    if (v && field.TextMode == 3)
                        v = v.replace(/(\r\n*)/g, '<br/>');
                    s = String.isBlank(v) ? (isForm ? Web.DataViewResources.Data.NullValueInForms : Web.DataViewResources.Data.NullValue) : (field.format(v));
                }
                else if (field.ItemsStyle == 'CheckBoxList') {
                    lov = v ? v.split(',') : [];
                    var fi = true;
                    for (i = 0; i < field.Items.length; i++) {
                        item = field.Items[i];
                        itemValue = item[0] == null ? '' : item[0].toString();
                        if (Array.contains(lov, itemValue)) {
                            if (fi) fi = false; else sb.append(', ');
                            sb.append(Web.DataView.htmlEncode(item[1]));
                        }
                    }
                    s = lov.length == 0 ? Web.DataViewResources.Data.NullValueInForms : '';
                }
                else {
                    item = this._findItemByValue(field, v);
                    s = item[1];
                    if (!isForm && s == Web.DataViewResources.Data.NullValueInForms)
                        s = Web.DataViewResources.Data.NullValue;
                }
                if (!String.isNullOrEmpty(field.HyperlinkFormatString)) {
                    var location = this._parseLocation(field.HyperlinkFormatString, row);
                    var m = location.match(Web.DataView.LocationRegex);
                    s = m ? String.format('<a href="javascript:" onclick="Web.DataView._navigated=true;window.open(\'{0}\', \'{1}\');return false;">{2}</a>', m[2].replace(/\'/g, '\\\'').replace(/"/g, '&quot;'), m[1], s) : String.format('<a href="{0}" onclick="Web.DataView._navigated=true;">{1}</a>', location, s);
                }
                sb.append(s);
            }
        }
        if (needObjectRef) {
            if (!isForm) sb.append('</td><td align="right">');
            sb.appendFormat('<span class="ObjectRef" title="{0}" onclick="$find(&quot;{1}&quot;).executeCommand({{commandName: &quot;_ViewDetails&quot;, commandArgument: &quot;{2}&quot;}})">&nbsp;</span>',
                String.format(Web.DataViewResources.Lookup.DetailsToolTip, Web.DataView.htmlAttributeEncode(this._allFields[field.AliasIndex].HeaderText)), this.get_id(), field.Name);
            if (!isForm) sb.append('</td></tr></table>');
        }
        if (isForm) {
            if (checkBox == null)
                sb.append('</div>');
            if (this._numberOfColumns > 0) sb.append(errorHtml);
            if (field.FooterText && field.FooterText.length > 0)
                sb.appendFormat('<div class="Footer">{0}</div>', field.FooterText);
            sb.append('</div>');
        }
    },
    _renderOnDemandItem: function(sb, field, row, isSelected, isForm) {
        var v = row[field.Index];
        var m = v ? v.match(/^null\|(.+)$/) : null;
        var isNull = m != null || v == null;
        if (m) v = m[1];
        if (isNull && !isSelected && field.OnDemandStyle == 1)
            sb.append(isForm ? Web.DataViewResources.Data.NullValueInForms : Web.DataViewResources.Data.NullValue);
        else {
            var blobHref = this.resolveClientUrl(Web.DataViewResources.Data.BlobHandler);
            if (isSelected && !isNull) sb.appendFormat('<a href="{0}?{1}=o|{2}" target="_blank" title="{3}">', blobHref, field.OnDemandHandler, v, Web.DataViewResources.Data.BlobDownloadHint);
            if (field.OnDemandStyle == 1) {
                if (isNull)
                    sb.append(isForm ? Web.DataViewResources.Data.NullValueInForms : Web.DataViewResources.Data.NullValue);
                else
                    sb.append(isSelected ? Web.DataViewResources.Data.BlobDownloadLink : Web.DataViewResources.Data.BlobDownloadLinkReadOnly);
            }
            else {
                if (!isNull)
                    sb.appendFormat('<img src="{0}?{1}=t|{2}" class="Thumbnail"/>', blobHref, field.OnDemandHandler, v);
                else
                    sb.append(isForm ? Web.DataViewResources.Data.NullValueInForms : Web.DataViewResources.Data.NullValue);
            }
            if (isSelected && !isNull) sb.append('</a>');
            if (!field.ReadOnly && (this.get_isEditing() && isSelected))
                sb.appendFormat('<iframe src="{0}?{1}=u|{2}&owner={3}&index={4}" frameborder="0" scrolling="no" id="{3}_Frame{4}"></iframe><div style="display:none" id="{3}_ProgressBar{4}" class="UploadProgressBar">{5}</div>', blobHref, field.OnDemandHandler, v, this.get_id(), field.Index, 'Uploading...');
        }
    },
    _showUploadProgress: function(index) {
        var f = $get(String.format('{0}_Frame{1}', this.get_id(), index));
        var p = $get(String.format('{0}_ProgressBar{1}', this.get_id(), index));
        if (f != null && p != null) {
            Sys.UI.DomElement.setVisible(p, true);
            var padding = $common.getPaddingBox(p);
            var border = $common.getBorderBox(p);
            p.style.width = (f.offsetWidth - padding.horizontal - border.horizontal) + 'px';
            p.style.height = (f.offsetHeight - padding.vertical - border.vertical) + 'px';
            Sys.UI.DomElement.setVisible(f, false);
        }
    },
    _renderActionButtons: function(sb, location, scope) {
        var actions = this.get_actions(scope);
        if (actions.length == 0) return;
        if (scope == 'Row') {
            var show = false;
            for (var i = 0; i < actions.length; i++) {
                if (this._isActionAvailable(actions[i])) {
                    show = true;
                    break;
                }
            }
            if (!show) return;
        }
        sb.appendFormat('<tr class="ActionButtonsRow {0}ButtonsRow"><td colspan="{1}" class="ActionButtons {2}ActionButtons">', location, this._get_colSpan(), scope);
        sb.append('<table style="width:100%" cellpadding="0" cellspacing="0" class="ActionButtons"><tr>');
        if (scope == 'Form')
            sb.appendFormat('<td id="{0}_Wait" align="left">{1}&nbsp;</td><td align="right">&nbsp;', location == 'Bottom' ? this.get_id() : '', this.get_isEditing() && Web.DataViewResources.Form.RequiredFieldMarker ? Web.DataViewResources.Form.RequiredFiledMarkerFootnote : '');
        else
            sb.append('<td>');
        for (i = 0; i < actions.length; i++) {
            var action = actions[i];
            if (this._isActionAvailable(action)) {
                var className = action.CssClass && action.CssClass.length > 0 ? action.CssClass : '';
                if (action.HeaderText && action.HeaderText.length > 10) {
                    if (className.length > 0) className += ' ';
                    className += 'AutoWidth';
                }
                sb.appendFormat('<button onclick="$find(\'{0}\').executeAction(\'{5}\', {1},-1);return false;" tabindex="{3}"{4}>{2}</button>', this.get_id(), i, action.HeaderText, $nextTabIndex(), className.length > 0 ? String.format(' class="{0}"', className) : '', scope);
            }
        }
        sb.append('</td></tr></table></td></tr>');
    },
    _isActionMatched: function(action) {
        var result =
            (action.WhenViewRegex == null || (action.WhenViewRegex.exec(this.get_viewId()) != null) == action.WhenViewRegexResult) &&
            (action.WhenTagRegex == null || (action.WhenTagRegex.exec(this.get_tag()) != null) == action.WhenTagRegexResult) &&
            (action.WhenHRefRegex == null || (action.WhenHRefRegex.exec(location.pathname) != null) == action.WhenHRefRegexResult) &&
            (String.isNullOrEmpty(action.WhenClientScript) || eval(action.WhenClientScript) == true);
        return result;
    },
    _isActionAvailable: function(action, rowIndex) {
        var lastCommand = action.WhenLastCommandName ? action.WhenLastCommandName : '';
        var lastArgument = action.WhenLastCommandArgument ? action.WhenLastCommandArgument : '';
        var available = lastCommand.length == 0 || (lastCommand == this.get_lastCommandName() && (lastArgument.length == 0 || lastArgument == this.get_lastCommandArgument()));
        if (available && this.get_isEditing()) {
            var isSelected = this._rowIsSelected(rowIndex == null ? this._selectedRowIndex : rowIndex);
            if (isSelected)
                return (lastCommand == 'New' || lastCommand == 'Edit' || lastCommand == 'BatchEdit' || lastCommand == 'Duplicate') && this._isActionMatched(action);
            else if (!isSelected && rowIndex == null && (lastCommand == 'New' || lastCommand == 'Duplicate'))
                return this._isActionMatched(action);
            else
                return lastCommand.length == 0 && rowIndex != null && this._isActionMatched(action);
        }
        return available && (!action.WhenKeySelected || action.WhenKeySelected && this._selectedKey && this._selectedKey.length > 0) && this._isActionMatched(action) && (action.CommandName != 'New' || this._hasKey());
    },
    _hasKey: function() { return this._keyFields && this._keyFields.length > 0; },
    _rowIsSelected: function(rowIndex) {
        if (!this._hasKey()) return false;
        var row = this._rows[rowIndex];
        if (row && this._keyFields.length == this._selectedKey.length) {
            if (row == this._mergedRow) return true;
            for (var j = 0; j < this._keyFields.length; j++) {
                var field = this._keyFields[j];
                var v1 = this._selectedKey[j];
                var v2 = row[field.Index];
                if (field.Type.startsWith('DateTime')) {
                    if (!(v1 || v2)) return false;
                    v1 = v1.toString();
                    v2 = v2.toString();
                }
                if (v1 != v2) return false;
            }
            return true;
        }
        else
            return false;
    },
    _get_template: function(type) {
        return $get(this.get_controller() + '_' + this.get_viewId() + (type ? '_' + type : ''));
    },
    _renderTemplate: function(template, sb, row, isSelected, isInlineForm) {
        var s = String.isInstanceOfType(template) ? template : template.innerHTML;
        var iterator = /([\s\S]*?)\{([\w\d]+)(\:([\S\s]+?)){0,1}\}/g;
        var lastIndex = 0;
        var match = iterator.exec(s);
        while (match) {
            lastIndex = match.index + match[0].length;
            sb.append(match[1]);
            var field = this.findField(match[2]);
            if (field)
                if (match[4] && match[4].length > 0)
                sb.appendFormat('{0:' + match[4] + '}', row[field.Index]);
            else
                this._renderItem(sb, field, row, isSelected, isInlineForm);
            match = iterator.exec(s);
        }
        if (lastIndex < s.length) sb.append(s.substring(lastIndex));
    },
    _renderNewRow: function(sb) {
        if (this.get_isInserting()) {
            var t = this._get_template('new');
            sb.appendFormat('<tr class="Row Selected{0}">', t ? ' InlineFormRow' : '');
            if (this._selectionMode == Web.DataViewSelectionMode.Multiple) sb.append('<td class="Cell">&nbsp;</td>');
            if (this.get_showIcons()) sb.append('<td class="Cell Icons {0}">&nbsp;</td>');
            var row = this._newRow ? this._newRow : [];
            this._mergeRowUpdates(row);
            if (t) {
                sb.appendFormat('<td class="Cell" colspan="{0}">', this.get_fields().length);
                this._renderTemplate(t, sb, row, true, true);
                sb.append('</td>');
            }
            else {
                for (var j = 0; j < this.get_fields().length; j++) {
                    var field = this._fields[j];
                    var af = this._allFields[field.AliasIndex];
                    sb.appendFormat('<td class="Cell {0} {1}Type">', af.Name, af.Type)
                    if (!field.ReadOnly) sb.appendFormat('<div class="Error" id="{0}_Item{1}_Error" style="display:none"></div>', this.get_id(), field.Index);
                    this._renderItem(sb, field, row, !field.OnDemand, null);
                    sb.append('</td>');
                }
            }
            sb.append('</tr>');
            this._renderActionButtons(sb, 'Bottom', 'Row')
        }
    },
    _renderGridView: function(sb) {
        this._renderViewDescription(sb);
        this._renderActionBar(sb);
        this._renderInfoBar(sb);
        var isInLookupMode = this.get_lookupField() != null;
        var expressions = this._enumerateExpressions(Web.DynamicExpressionType.Any, Web.DynamicExpressionScope.ViewRowStyle, this.get_viewId());
        sb.append('<tr class="HeaderRow">');
        var hasKey = this._hasKey();
        var multipleSelection = this._selectionMode == Web.DataViewSelectionMode.Multiple && hasKey;
        var showIcons = this.get_showIcons();
        if (multipleSelection) {
            sb.appendFormat('<th class="Toggle"><input type="checkbox" onclick="$find(&quot;{0}&quot;).toggleSelectedRow()" id="{0}_ToggleButton"/></th>', this.get_id());
            this._multipleSelection = false;
        }
        if (this.get_showIcons()) sb.append('<th class="Icons">&nbsp;&nbsp;</th>');
        for (var i = 0; i < this.get_fields().length; i++) {
            var field = this.get_fields()[i];
            field = this._allFields[field.AliasIndex];
            sb.appendFormat('<th class="FieldHeaderSelector {0} {1}Type"', field.Name, field.Type);
            if (field.AllowSorting || field.AllowQBE)
                sb.appendFormat(' onmouseover="$showHover(this,\'{0}$FieldHeaderSelector${1}\',\'FieldHeaderSelector\')" onmouseout="$hideHover(this)" onclick="$toggleHover()"', this.get_id(), i);
            sb.append('>');
            if (field.AllowSorting) {
                sb.appendFormat('<a href="#" onclick="$find(\'{0}\').sort(\'{1}\');$preventToggleHover();return false" title="{3}" onfocus="$showHover(this,\'{0}$FieldHeaderSelector${4}\',\'FieldHeaderSelector\',1)" onblur="$hideHover(this)" tabindex="{5}">{2}</a>',
                    this.get_id(), field.Name, field.HeaderText, String.format(Web.DataViewResources.HeaderFilter.SortBy, field.HeaderText), i, $nextTabIndex());
                if (this.get_sortExpression() != null && this.get_sortExpression().startsWith(field.Name + " "))
                    sb.append(this.get_sortExpression().endsWith(' asc') ? '<span class="SortUp">&nbsp;</span>' : '<span class="SortDown">&nbsp;</span>');
                if (this.filterOf(field) != null)
                    sb.append('<span class="Filter">&nbsp;</span>');
            }
            else
                sb.append(field.HeaderText);
            sb.append('</th>');
        }
        sb.append('</tr>');
        var isEditing = this.get_isEditing();
        var isInserting = this.get_isInserting();
        var newRowIndex = this._lastSelectedRowIndex;
        var t = isEditing ? this._get_template() : null;
        var ts = this._get_template('selected');
        this._registerRowSelectorItems();
        for (i = 0; i < this.get_rows().length; i++) {
            var row = this.get_rows()[i];
            var customCssClasses = ' ' + this._evaluateJavaScriptExpressions(expressions, row, true);
            var isSelectedRow = this._rowIsSelected(i);
            if (isSelectedRow) this._selectedRowIndex = i;
            var checkBoxCell = null;
            var multiSelectedRowClass = '';
            if (multipleSelection) {
                var selected = Array.indexOf(this._selectedKeyList, this._createRowKey(i)) != -1;
                if (selected) this._multipleSelection = true;
                checkBoxCell = String.format('<td class="Cell Toggle"><input type="checkbox" id="{0}_CheckBox{1}" onclick="$find(&quot;{0}&quot;).toggleSelectedRow({1})"{2}" class="MultiSelect{3}"/></td>', this.get_id(), i, selected ? ' checked="checked"' : null, selected ? ' Selected' : '');
                if (selected) multiSelectedRowClass = ' MultiSelectedRow';
            }
            var iconCell = showIcons ? String.format('<td class="Cell Icons {0}">&nbsp;</td>', this._icons[i]) : '';
            if (isSelectedRow && (isEditing && t || ts)) {
                sb.appendFormat('<tr id="{0}_Row{1}" class="{2}Row{3} Selected">{5}{6}<td class="Cell" colspan="{4}">', this.get_id(), i, i % 2 == 0 ? '' : 'Alternating', ' InlineFormRow', this.get_fields().length, checkBoxCell, iconCell);
                this._renderTemplate(isEditing && t ? t : ts, sb, row, true, true);
                sb.append('</td>');
            }
            else {
                sb.appendFormat('<tr id="{0}_Row{1}" class="{2}Row{3}{4}{5}" onmouseover="Sys.UI.DomElement.addCssClass(this,\'Highlight\');" onmouseout="Sys.UI.DomElement.removeCssClass(this,\'Highlight\')">', this.get_id(), i, i % 2 == 0 ? '' : 'Alternating', isSelectedRow ? ' Selected' + customCssClasses : customCssClasses, multiSelectedRowClass, hasKey ? '' : ' ReadOnlyRow');
                if (checkBoxCell) sb.append(checkBoxCell);
                if (showIcons) sb.append(iconCell);
                if (isEditing && isSelectedRow)
                    this._mergeRowUpdates(row);
                for (j = 0; j < this.get_fields().length; j++) {
                    field = this._fields[j];
                    var af = this._allFields[field.AliasIndex];
                    if (j == 0 && hasKey && !isInLookupMode || isSelectedRow && isEditing || field.OnDemand && isSelectedRow) sb.appendFormat('<td class="Cell {0} {1}Type">', field.Name, af.Type); else sb.appendFormat('<td class="Cell {2} {3}Type" style="cursor:default;" onclick="$find(&quot;{0}&quot;).executeRowCommand({1},&quot;Select&quot;)">', this.get_id(), i, field.Name, af.Type == 'Byte[]' ? 'Binary' : af.Type);
                    if (isSelectedRow && isEditing && !field.ReadOnly) sb.appendFormat('<div class="Error" id="{0}_Item{1}_Error" style="display:none"></div>', this.get_id(), field.Index);
                    if (j == 0 && hasKey) {
                        var family = Web.HoverMonitor.Families[String.format('{0}$RowSelector${1}', this.get_id(), i)];
                        if (!isInLookupMode && family && family.items.length > 1)
                            sb.appendFormat('<div id="{0}_RowSelector{1}" class="RowSelector" onmouseover="$showHover(this, \'{0}$RowSelector${1}\', \'RowSelector\')" onmouseout="$hideHover(this)" onclick="$toggleHover()">', this.get_id(), i);
                        if (!(isSelectedRow && isEditing)) {
                            var focusEvents = isInLookupMode || !family || family.items.length == 1 ? '' : String.format(' onfocus="$showHover(this, \'{0}$RowSelector${1}\', \'RowSelector\', 1)" onblur="$hideHover(this)" ', this.get_id(), i);
                            if (!isInLookupMode) sb.appendFormat('<a href="#" onclick="$hoverOver(this, 2);$find(\'{0}\').executeAction(\'Grid\',-1,{1});return false" tabindex="{2}"{3}>', this.get_id(), i, $nextTabIndex(), focusEvents); else sb.appendFormat('<a href="javascript:" onclick="return false" tabindex="{0}">', $nextTabIndex());
                        }
                    }
                    this._renderItem(sb, field, row, isSelectedRow, null, j == 0 && hasKey);
                    if (j == 0 && hasKey && !isEditing) {
                        if (!(isSelectedRow && isEditing)) sb.append('</a>');
                        if (!isInLookupMode && family && family.items.length > 1) sb.append('</div>');
                    }
                    sb.append('</td>');
                }
            }
            sb.append('</tr>');
            if (isSelectedRow) this._renderActionButtons(sb, 'Bottom', 'Row');
            if (isInserting && newRowIndex == i) {
                newRowIndex = -2;
                this._renderNewRow(sb);
            }
        }
        if (isInserting && newRowIndex != -2) this._renderNewRow(sb);
        this._renderAggregates(sb, multipleSelection);
        this._renderNoRecordsWhenNeeded(sb);
        this._renderViewPager(sb);
    },
    _renderAggregates: function(sb, multipleSelection) {
        if (this._totalRowCount == 0 || this.get_aggregates() == null) return;
        sb.append('<tr class="AggregateRow">');
        if (multipleSelection) sb.append('<td class="Aggregate">&nbsp;</td>');
        if (this.get_showIcons()) sb.append('<td class="Aggregate">&nbsp;</td>');
        for (var i = 0; i < this.get_fields().length; i++) {
            var field = this.get_fields()[i];
            if (field.Aggregate == 0) sb.append('<td class="None">&nbsp;</td>');
            else {
                var v = this.get_aggregates()[field.Index];
                if (v == null) v = Web.DataViewResources.Data.NullValue;
                else v = field.format(v);
                var f = this._allFields[field.AliasIndex];
                var a = Web.DataViewResources.Grid.Aggregates[Web.DataViewAggregates[field.Aggregate]];
                sb.appendFormat('<td class="Aggregate {0} {1}Type" title="', f.Name, f.Type);
                sb.appendFormat(a.ToolTip, f.HeaderText);
                sb.append('">');
                sb.appendFormat(a.FmtStr, v);
                sb.append('</td>');
            }
        }
        sb.append('</tr>');
    },
    _renderNoRecordsWhenNeeded: function(sb) {
        if (this._totalRowCount == 0)
            sb.appendFormat('<tr class="Row NoRecords"><td colspan="{0}" class="Cell">{1}</td></tr>', this._get_colSpan(), Web.DataViewResources.Data.NoRecords);
    },
    _attachBehaviors: function() {
        this._detachBehaviors();
        this._attachFieldBehaviors();
        var e = this.get_quickFindElement();
        if (e) $addHandlers(e, this._quickFindHandlers, this);
    },
    _attachFieldBehaviors: function() {
        if (this.get_isEditing()) {
            for (var i = 0; i < this.get_fields().length; i++) {
                var field = this.get_fields()[i];
                var element = $get(this.get_id() + '_Item' + field.Index);
                if (element) {
                    if (field.Mask && field.Mask.length > 0) {
                        var cc = Sys.CultureInfo.CurrentCulture;
                        var sdp = cc.dateTimeFormat.ShortDatePattern.toUpperCase().split(cc.dateTimeFormat.DateSeparator);
                        var m = $create(AjaxControlToolkit.MaskedEditBehavior, {
                            'CultureAMPMPlaceholder': cc.dateTimeFormat.AMDesignator + ';' + cc.dateTimeFormat.PMDesignator,
                            'CultureCurrencySymbolPlaceholder': cc.numberFormat.CurrencySymbol,
                            'CultureDateFormat': sdp[0].substring(0, 1) + sdp[1].substring(0, 1) + sdp[2].substring(0, 1),
                            'CultureDatePlaceholder': cc.dateTimeFormat.DateSeparator,
                            'CultureDecimalPlaceholder': cc.numberFormat.NumberDecimalSeparator,
                            'CultureName': cc.name,
                            'CultureThousandsPlaceholder': cc.numberFormat.NumberGroupSeparator,
                            'CultureTimePlaceholder': cc.dateTimeFormat.TimeSeparator,
                            'DisplayMoney': field.DataFormatString == '{0:c}',
                            'Mask': field.Mask,
                            'MaskType': field.MaskType,
                            'ClearMaskOnLostFocus': field.MaskType > 0,
                            'id': this.get_id() + '_MaskedEdit' + field.Index
                        },
                            null, null, element);
                        if (field.MaskType == 2) m.set_InputDirection(1);
                        Array.add(field.Behaviors, m);

                    }
                    if (field.Type.startsWith('DateTime')) {
                        var c = $create(AjaxControlToolkit.CalendarBehavior, { id: this.get_id() + '_Calendar' + field.Index }, null, null, element);
                        c.set_format(field.DataFormatString.match(/\{0:([\s\S]*?)\}/)[1]);
                        var button = $get(element.id + '_Button');
                        if (button) c.set_button(button);
                        Array.add(field.Behaviors, c);
                    }
                    else if (element.type == 'text' && field.AutoCompletePrefixLength > 0) {
                        c = $create(AjaxControlToolkit.AutoCompleteBehavior, {
                            'completionInterval': 500,
                            'contextKey': String.format('{0},{1},{2}', this.get_controller(), this.get_viewId(), field.Name),
                            'delimiterCharacters': ',;',
                            'id': this.get_id() + '_AutoComplete' + field.Index,
                            'minimumPrefixLength': field.AutoCompletePrefixLength,
                            'serviceMethod': 'GetCompletionList',
                            'servicePath': this.get_servicePath(),
                            'useContextKey': true
                        },
                            null, null, element);
                        Array.add(field.Behaviors, c);
                    }
                }
            }
        }
    },
    _detachBehaviors: function() {
        // detach quick find
        var e = this.get_quickFindElement();
        if (e) $clearHandlers(e);
        // detach row header drop downs and field behaviors
        if (this.get_fields() != null) {
            for (i = 0; i < this.get_fields().length; i++) {
                var field = this.get_fields()[i];
                if (field._lookupModalBehavior != null) {
                    field._lookupModalBehavior.dispose();
                    field._lookupModalPanel.parentNode.removeChild(field._lookupModalPanel);
                    delete field._lookupModalPanel;
                    field._lookupModalBehavior = null;
                }
                if (field._lookupDataControllerBehavior != null) {
                    field._lookupDataControllerBehavior.dispose();
                    field._lookupDataControllerBehavior = null;
                }
                for (var j = 0; j < field.Behaviors.length; j++)
                    field.Behaviors[j].dispose();
                Array.clear(field.Behaviors);
            }
        }
    },
    _registerFieldHeaderItems: function(fieldIndex) {
        var startIndex = fieldIndex == null ? 0 : fieldIndex;
        var endIndex = fieldIndex == null ? this.get_fields().length - 1 : fieldIndex;
        var sort = this.get_sortExpression();
        if (sort) sort = sort.match(/^(\w+)\s+(asc|desc)/);
        for (var i = startIndex; i <= endIndex; i++) {
            var items = new Array();
            var family = String.format('{0}$FieldHeaderSelector${1}', this.get_id(), i);
            var originalField = this.get_fields()[i];
            var field = this._allFields[originalField.AliasIndex];
            if (field.AllowSorting || field.AllowQBE) {
                var ascending = Web.DataViewResources.HeaderFilter.GenericSortAscending;
                var descending = Web.DataViewResources.HeaderFilter.GenericSortDescending;
                switch (field.Type) {
                    case 'String':
                        ascending = Web.DataViewResources.HeaderFilter.StringSortAscending;
                        descending = Web.DataViewResources.HeaderFilter.StringSortDescending;
                        break;
                    case 'DateTime':
                    case 'DateTimeOffset':
                        ascending = Web.DataViewResources.HeaderFilter.DateSortAscending;
                        descending = Web.DataViewResources.HeaderFilter.DateSortDescending;
                        break;
                }
                if (field.AllowSorting) {
                    var sortedBy = sort && sort[1] == field.Name;
                    var item = new Web.Item(family, ascending);
                    if (sortedBy && sort[2] == 'asc')
                        item.set_selected(true);
                    else
                        item.set_cssClass('SortAscending');
                    item.set_script('$find("{0}").sort("{1} asc")', this.get_id(), field.Name);
                    Array.add(items, item);
                    item = new Web.Item(family, descending);
                    if (sortedBy && sort[2] == 'desc')
                        item.set_selected(true);
                    else
                        item.set_cssClass('SortDescending');
                    item.set_script('$find("{0}").sort("{1} desc")', this.get_id(), field.Name);
                    Array.add(items, item);
                }
                if (field.AllowQBE) {
                    var fieldFilter = this.filterOf(field);
                    fieldFilter = fieldFilter && fieldFilter.startsWith('=') ? fieldFilter.substr(1) : (fieldFilter ? '\0' : null);
                    if (field.AllowSorting) Array.add(items, new Web.Item())
                    item = new Web.Item(family, String.format(Web.DataViewResources.HeaderFilter.ClearFilter, field.HeaderText));
                    item.set_cssClass('FilterOff');
                    if (!fieldFilter) item.set_disabled(true);
                    item.set_script('$find("{0}").applyFilterByIndex({1},-1)', this.get_id(), originalField.AliasIndex);
                    Array.add(items, item);
                    item = new Web.Item(family, Web.DataViewResources.HeaderFilter.CustomFilterOption);
                    if (fieldFilter && fieldFilter.indexOf('\0') >= 0) item.set_selected(true);
                    else
                        item.set_cssClass('CustomFilter');
                    item.set_script('$find("{0}").showCustomFilter({1})', this.get_id(), originalField.AliasIndex);
                    Array.add(items, item);
                    if (originalField._listOfValues) {
                        for (var j = 0; j < originalField._listOfValues.length; j++) {
                            if (j == 0) Array.add(items, new Web.Item());
                            var v = originalField._listOfValues[j];
                            var isSelected = false;
                            var text = v;
                            if (v == null)
                                text = Web.DataViewResources.HeaderFilter.EmptyValue;
                            else if (field.Items.length > 0) {
                                item = this._findItemByValue(field, v);
                                text = item[1];
                            }
                            else {
                                if (field.Type == 'String' && v.length == 0)
                                    text = Web.DataViewResources.HeaderFilter.BlankValue;
                                else if (!String.isNullOrEmpty(field.DataFormatString))
                                    text = field.Type.startsWith('DateTime') ? text = String.localeFormat('{0:d}', v) : field.format(v);
                            }
                            v = v == null ? 'null' : this.convertFieldValueToString(field, v);
                            isSelected = v == fieldFilter;
                            if (text.length > Web.DataViewResources.HeaderFilter.MaxSampleTextLen) text = text.substring(0, Web.DataViewResources.HeaderFilter.MaxSampleTextLen) + '...';
                            if (typeof (text) != 'string') text = text.toString();
                            item = new Web.Item(family, text);
                            if (isSelected) item.set_selected(true);
                            item.set_script("$find(\'{0}\').applyFilterByIndex({1},{2});", this.get_id(), originalField.AliasIndex, j);
                            item.set_group(1);
                            Array.add(items, item);
                        }
                    }
                    else {
                        item = new Web.Item(family, Web.DataViewResources.HeaderFilter.Loading);
                        item.set_dynamic(true);
                        item.set_script('$find("{0}")._loadListOfValues("{1}","{2}","{3}")', this.get_id(), family, originalField.Name, field.Name);
                        Array.add(items, item);
                    }
                }
            }
            $registerItems(family, items, Web.HoverStyle.Click, Web.PopupPosition.Right, Web.ItemDescriptionStyle.ToolTip);
        }
    },
    _registerActionBarItems: function() {
        var groups = this.get_actionGroups('ActionBar');
        for (var i = 0; i < groups.length; i++) {
            var group = groups[i];
            var family = String.format('{0}${1}$ActionGroup${2}', this.get_id(), this.get_viewId(), i);
            if (!group.flat) {
                var items = new Array();
                for (var j = 0; j < group.Actions.length; j++) {
                    var a = group.Actions[j];
                    if (this._isActionAvailable(a)) {
                        var item = new Web.Item(family, a.HeaderText, a.Description);
                        item.set_cssClass(String.isNullOrEmpty(a.CssClass) ? a.CommandName + 'LargeIcon' : a.CssClass);
                        Array.add(items, item);
                        item.set_script(String.format('$find(\'{0}\').executeAction(\'ActionBar\',{1},null,{2})', this.get_id(), j, i));
                    }
                }
                $registerItems(family, items, Web.HoverStyle.ClickAndStay, Web.PopupPosition.Left, Web.ItemDescriptionStyle.Inline);
            }
            else
                $unregisterItems(family);
        }
        Array.clear(groups);
    },
    _registerViewSelectorItems: function() {
        var items = new Array();
        var family = this.get_id() + '$ViewSelector';
        for (var i = 0; i < this.get_views().length; i++) {
            var view = this.get_views()[i];
            if (view.Type != 'Form' || view.Id == this.get_viewId()) {
                var item = new Web.Item(family, view.Label);
                if (view.Id == this.get_viewId())
                    item.set_selected(true);
                item.set_script('$find("{0}").executeCommand({{commandName:"Select",commandArgument:"{1}"}})', this.get_id(), view.Id);
                Array.add(items, item);
            }
        }
        $registerItems(family, items, Web.HoverStyle.ClickAndStay, Web.PopupPosition.Right, Web.ItemDescriptionStyle.None);
    },
    _registerRowSelectorItems: function() {
        var actions = this.get_actions('Grid');
        if (actions && actions.length > 0) {
            for (var i = 0; i < this._rows.length; i++) {
                var items = new Array();
                var family = String.format('{0}$RowSelector${1}', this.get_id(), i);
                for (var j = 0; j < actions.length; j++) {
                    var a = actions[j];
                    if (this._isActionAvailable(a, i)) {
                        var item = new Web.Item(family, a.HeaderText, a.Description);
                        item.set_cssClass(String.isNullOrEmpty(a.CssClass) ? a.CommandName + 'Icon' : a.CssClass);
                        Array.add(items, item);
                        item.set_script(String.format('$find("{0}").executeAction("Grid", {1},{2})', this.get_id(), j, i));
                    }
                }
                $registerItems(family, items, Web.HoverStyle.Click, Web.PopupPosition.Right, Web.ItemDescriptionStyle.ToolTip);
            }
        }
    },
    _renderActionBar: function(sb) {
        if (!this.get_showActionBar()) return;
        sb.appendFormat('<tr class="ActionRow"><td colspan="{0}"  class="ActionBar">', this._get_colSpan());
        sb.append('<table style="width:100%" cellpadding="0" cellspacing="0"><tr><td style="width:100%">');
        var groups = this.get_actionGroups('ActionBar');

        sb.append('<table cellpadding="0" cellspacing="0" class="Groups"><tr>');
        if (this.get_view().Type == 'Grid' && this.get_showQuickFind()) {
            var s = this.get_quickFindText();
            sb.appendFormat('<td class="QuickFind" title="{2}"><div class="QuickFind"><table cellpadding="0" cellspacing="0"><tr><td><input type="text" id="{0}_QuickFind" value="{1}" class="{3}" tabindex="{4}"/></td><td><span class="Button" onclick="$find(\'{0}\').quickFind()">&nbsp;</span></td></tr></table></div></td>', this.get_id(), Web.DataView.htmlAttributeEncode(s), Web.DataViewResources.Grid.QuickFindToolTip, s == Web.DataViewResources.Grid.QuickFindText ? 'Empty' : 'NonEmpty', $nextTabIndex());
            sb.append('<td class="Divider"><div></div></td>');
            if (this.get_lookupField() && !String.isNullOrEmpty(this.get_lookupField().ItemsNewDataView)) {
                sb.appendFormat('<td class="QuickCreateNew"><a href="#" onclick="$find(&quot;{0}&quot;).closeLookupAndCreateNew()" class="CreateNew" title="{1}" tabindex="{2}><span class="Placeholder"></span></a></td>', this.get_id(), Web.DataViewResources.Lookup.GenericNewToolTip, $nextTabIndex());
            }
        }
        else {
            if (groups.length == 0 || this.get_lookupField()) sb.append('<td class="Divider"><div style="visibility:hidden"></div></td>');
        }
        this._registerViewSelectorItems();
        if (!this.get_lookupField()) {
            // create action bar items
            this._registerActionBarItems();
            // render action group
            for (var i = 0; i < groups.length; i++) {
                if (i > 0)
                    sb.append('<td class="Divider"><div></div></td>');
                var group = groups[i];
                if (group.Flat) {
                    for (var j = 0; j < group.Actions.length; j++) {
                        var a = group.Actions[j];
                        if (this._isActionAvailable(a) && !String.isNullOrEmpty(a.HeaderText))
                            sb.appendFormat('<td class="Group FlatGroup{6}" onmouseover="$showHover(this,&quot;{0}${2}$ActionGroup${1}&quot;,&quot;ActionGroup&quot;)" onmouseout="$hideHover(this)" onclick="if(this._skip)this._skip=null;else $find(\'{0}\').executeAction(\'ActionBar\',{1},null,{2})"><span class="Outer">{5}<a href="javascript:" tabindex="{4}" onclick="this.parentNode.parentNode._skip=true;$find(\'{0}\').executeAction(\'ActionBar\',{1},null,{2});return false;" onfocus="$showHover(this,&quot;{0}${2}$ActionGroup${1}&quot;,&quot;ActionGroup&quot;,2)" onblur="$hideHover(this)" >{3}</a></span></td>',
                                this.get_id(), j, i, Web.DataView.htmlEncode(a.HeaderText), $nextTabIndex(), !String.isNullOrEmpty(a.CssClass) ? String.format('<span class="FlatGroupIcon {0}">&nbsp;</span>', a.CssClass) : '', !String.isNullOrEmpty(a.CssClass) ? ' FlatGroupWithIcon' : '');
                    }
                }
                else
                    sb.appendFormat('<td class="Group" onmouseover="$showHover(this,&quot;{0}${1}$ActionGroup${2}&quot;,&quot;ActionGroup&quot;)" onmouseout="$hideHover(this)" onclick="$toggleHover()"><span class="Outer"><a href="javascript:" onfocus="$showHover(this,&quot;{0}${1}$ActionGroup${2}&quot;,&quot;ActionGroup&quot;,2)" onblur="$hideHover(this)" tabindex="{4}" onclick="$hoverOver(this, 2);return false;">{3}</a></span></td>',
                        this.get_id(), this.get_viewId(), i, group.HeaderText, $nextTabIndex());
            }
        }
        sb.append('</tr></table>');
        sb.append('</td><td class="ViewSelectorControl">');
        if (this.get_showViewSelector()) {
            sb.appendFormat('<table cellpadding="0" cellspacing="0"><tr><td class="ViewSelectorLabel">{0}:</td><td>', Web.DataViewResources.ActionBar.View);
            sb.appendFormat('<span class="ViewSelector" onmouseover="$showHover(this,&quot;{0}$ViewSelector&quot;,&quot;ViewSelector&quot;)" onmouseout="$hideHover(this)" onclick="$toggleHover()"><span class="Outer"><span class="Inner"><a href="javascript:" tabindex="{2}" onfocus="$showHover(this,&quot;{0}$ViewSelector&quot;,&quot;ViewSelector&quot;,3)" onblur="$hideHover(this)">{1}</a></span></span></span>',
            this.get_id(), Web.DataView.htmlEncode(this.get_view().Label), $nextTabIndex());
            sb.append('</td></tr></table>');
        }
        sb.append('</td></tr></table>');
        sb.append('</td></tr>');
    },
    _renderViewDescription: function(sb) {
        if (!this.get_showDescription()) return;
        var t = this.get_view().HeaderText;
        if (!String.isNullOrEmpty(t) || this.get_lookupField()) {
            sb.appendFormat('<tr class="HeaderTextRow"><td colspan="{0}" class="HeaderText">', this._get_colSpan());
            if (this.get_lookupField() != null)
                sb.append('<table style="width:100%" cellpadding="0" cellspacing="0"><tr><td style="padding:0px">');
            sb.append(this._formatViewText(Web.DataViewResources.Views.DefaultDescriptions[t], true, t));
            if (this.get_lookupField() != null)
                sb.appendFormat('</td><td align="right" style="padding:0px"><a href="#" class="Close" onclick="$find(\'{0}\').hideLookup();return false" tabindex="{2}" title="{1}">&nbsp;</a></td></tr></table>', this.get_id(), Web.DataViewResources.ModalPopup.Close, $nextTabIndex());
            sb.append('</td></tr>');
        }
    },
    _renderInfoBar: function(sb) {
        var filter = this.get_filter();
        if (filter.length > 0 && !this.filterIsExternal()) {
            sb.appendFormat('<tr class="InfoRow"><td colspan="{0}">', this._get_colSpan());
            if (this.get_view().Type != "Form") this._renderFilterDetails(sb, filter);
            sb.append('</td></tr>');
        }
    },
    _renderFilterDetails: function(sb, currentFilter) {
        sb.appendFormat('<a href="javascript:" onclick="$find(\'{0}\').clearFilter();return false" class="Close" tabindex="{3}" title="{2}">&nbsp;</a><span class="Details"><span class="Information">&nbsp;</span>{1}', this.get_id(), Web.DataViewResources.InfoBar.FilterApplied, Web.DataViewResources.ModalPopup.Close, $nextTabIndex());
        for (var i = 0; i < currentFilter.length; i++) {
            var filter = currentFilter[i].match(/(\w+):([\s\S]*)/);
            var field = this.findField(filter[1]);
            if (!field || this._fieldIsInExternalFilter(field)) continue;
            var aliasField = this._allFields[field.AliasIndex];
            var re = /(\*|~|\>={0,1}|\<={0,1}|=)([\s\S]*?)(\0|$)/g;
            if (filter[2].startsWith('~')) sb.append(Web.DataViewResources.InfoBar.QuickFind);
            else sb.appendFormat(Web.DataViewResources.InfoBar.ValueIs, aliasField.HeaderText);
            var first = true;
            var fieldOperator = filter[2].match(">|<") ? Web.DataViewResources.InfoBar.And : Web.DataViewResources.InfoBar.Or;
            while ((info = re.exec(filter[2])) != null) {
                if (first)
                    first = false;
                else
                    sb.append(fieldOperator);
                switch (info[1]) {
                    case '=':
                        sb.append(info[2] == 'null' ? Web.DataViewResources.InfoBar.Empty : Web.DataViewResources.InfoBar.EqualTo);
                        break;
                    case '<':
                        sb.append(Web.DataViewResources.InfoBar.LessThan);
                        break;
                    case '<=':
                        sb.append(Web.DataViewResources.InfoBar.LessThanOrEqualTo);
                        break;
                    case '>':
                        sb.append(Web.DataViewResources.InfoBar.GreaterThan);
                        break;
                    case '>=':
                        sb.append(Web.DataViewResources.InfoBar.GreaterThanOrEqual);
                        break;
                    case '*':
                        sb.append(info[2].startsWith('%') ? Web.DataViewResources.InfoBar.Like : Web.DataViewResources.InfoBar.StartsWith);
                        break;
                }
                var item = this._findItemByValue(field, info[2]);
                var v = item == null ? info[2] : item[1];
                if (info[2] != 'null') sb.appendFormat('<b>{0}</b>', Web.DataView.htmlEncode(v));
            }
            sb.append('.</span>');
        }
    },
    _findItemByValue: function(field, value) {
        if (field.Items.length == 0) return null;
        value = value == null ? '' : value.toString();
        for (var i = 0; i < field.Items.length; i++) {
            var item = field.Items[i];
            var itemValue = item[0] == null ? "" : item[0].toString();
            if (itemValue == value)
                return item;
        }
        return [value, value];
    },
    _renderViewPager: function(sb) {
        sb.appendFormat('<tr class="FooterRow"><td colspan="{0}" class="Footer"><table cellpadding="0" cellspacing="0" style="width:100%"><tr><td align="left" class="Pager">', this._get_colSpan());
        if (this.get_pageCount() > 1) {
            var buttonIndex = this._firstPageButtonIndex;
            var buttonCount = Web.DataViewResources.Pager.PageButtonCount;
            if (this.get_pageIndex() > 0)
                sb.appendFormat('<a href="#" onclick="$find(\'{1}\').goToPage({0});return false" class="PaddedLink" tabindex="{3}">{2}</a>', this.get_pageIndex() - 1, this.get_id(), Web.DataViewResources.Pager.Previous, $nextTabIndex());
            else
                sb.appendFormat('<span class="Disabled">{0}</span>', Web.DataViewResources.Pager.Previous);
            sb.appendFormat(' | {0}: ', Web.DataViewResources.Pager.Page);
            if (buttonIndex > 0)
                sb.appendFormat('<a href="#" onclick="$find(\'{1}\').goToPage({0});return false" class="PaddedLink" tabindex="{2}">...</a>', buttonIndex - 1, this.get_id(), $nextTabIndex());
            while (buttonCount > 0 && buttonIndex < this.get_pageCount()) {
                if (buttonIndex == this.get_pageIndex())
                    sb.appendFormat('<span class="Selected">{0}</span>', buttonIndex + 1);
                else
                    sb.appendFormat('<a href="#" onclick="$find(\'{1}\').goToPage({0});return false" class="PaddedLink" tabindex="{3}">{2}</a>', buttonIndex, this.get_id(), buttonIndex + 1, $nextTabIndex());
                buttonIndex++;
                buttonCount--;
            }
            if (buttonIndex <= this.get_pageCount() - 1)
                sb.appendFormat('<a href="#" onclick="$find(\'{1}\').goToPage({0});return false" class="PaddedLink" tabindex="{2}">...</a>', this._firstPageButtonIndex + Web.DataViewResources.Pager.PageButtonCount, this.get_id(), $nextTabIndex());
            sb.append(' | ');
            if (this.get_pageIndex() < this.get_pageCount() - 1)
                sb.appendFormat('<a href="#" onclick="$find(\'{1}\').goToPage({0});return false" class="PaddedLink" tabindex="{3}">{2}</a>', this.get_pageIndex() + 1, this.get_id(), Web.DataViewResources.Pager.Next, $nextTabIndex());
            else
                sb.appendFormat('<span class="Disabled">{0}</span>', Web.DataViewResources.Pager.Next);
        }
        sb.append('</td><td align="right" class="Pager">&nbsp;');
        var pageSizes = this._pageSizes;
        if (this._totalRowCount > this.get_pageSize()) {
            sb.append(Web.DataViewResources.Pager.ItemsPerPage);
            for (i = 0; i < pageSizes.length; i++) {
                if (i > 0) sb.append(', ');
                if (this.get_pageSize() == pageSizes[i])
                    sb.appendFormat('<b>{0}</b>', this.get_pageSize());
                else
                    sb.appendFormat('<a href="#" onclick="$find(\'{0}\').set_pageSize({1});return false" tabindex="{2}">{1}</a>', this.get_id(), pageSizes[i], $nextTabIndex());
            }
            sb.append(' | ');
        }
        if (this._totalRowCount > 0) {
            var lastVisibleItemIndex = (this.get_pageIndex() + 1) * this.get_pageSize();
            if (lastVisibleItemIndex > this._totalRowCount) lastVisibleItemIndex = this._totalRowCount;
            sb.appendFormat(Web.DataViewResources.Pager.ShowingItems, this.get_pageIndex() * this.get_pageSize() + 1, lastVisibleItemIndex, this._totalRowCount);
            if (this._selectionMode == Web.DataViewSelectionMode.Multiple) {
                sb.appendFormat('<span id="{0}$SelectionInfo">', this.get_id());
                if (this._selectedKeyList.length > 0) sb.appendFormat(Web.DataViewResources.Pager.SelectionInfo, this._selectedKeyList.length);
                sb.append('</span>');
            }
            sb.append(' | ');
        }
        sb.appendFormat('</td><td align="center" class="Pager" id="{0}_Wait" style="width:45px">', this.get_id());
        sb.appendFormat('<a href="#" onclick="$find(\'{0}\').goToPage(-1);return false" class="PaddedLink" tabindex="{2}">{1}</a>', this.get_id(), Web.DataViewResources.Pager.Refresh, $nextTabIndex());
        sb.append('</td></tr></table>');
        sb.append('</td></tr>');
    },
    refresh: function(noFetch, newValues) {
        if (this.get_isEditing()) {
            var values = this._collectFieldValues(true);
            var ditto = [];
            for (var i = 0; i < values.length; i++) {
                var v = values[i];
                Array.add(ditto, { 'name': v.Name, 'value': v.Modified ? v.NewValue : v.OldValue });
            }
            if (newValues) {
                for (i = 0; i < newValues.length; i++) {
                    v = newValues[i];
                    for (var j = 0; j < ditto.length; j++) {
                        if (ditto[j].name == v.name) {
                            Array.removeAt(ditto, j);
                            break;
                        }
                    }
                    Array.add(ditto, v);
                }
            }
            this._ditto = ditto;
        }
        this._lastSelectedCategoryTabIndex = this.get_categoryTabIndex();
        if (noFetch)
            this._render();
        else
            this.goToView(this.get_viewId());
    },
    _createParams: function() {
        var lc = this.get_lookupContext();
        return { PageIndex: this.get_pageIndex(), PageSize: this.get_pageSize(), SortExpression: this.get_sortExpression(), Filter: this.get_filter(), ContextKey: this.get_id(), Cookie: this.get_cookie(), FilterIsExternal: this._externalFilter.length > 0, LookupContextFieldName: lc ? lc.FieldName : null, LookupContextController: lc ? lc.Controller : null, LookupContextView: lc ? lc.View : null, LookupContext: lc, Inserting: this.get_isInserting(), LastCommandName: this.get_lastCommandName(), LastCommandArgument: this.get_lastCommandArgument(), /*SelectedValues: this.get_selectedValues(),*/ExternalFilter: this.get_externalFilter() };
    },
    _loadPage: function() {
        this._delayedLoading = false;
        if (this._source) return;
        if (this.get_mode() != Web.DataViewMode.View) {
            this._allFields = [{ Index: 0, Label: '', DataFormatString: '', AliasIndex: 0, ItemsDataController: this.get_controller(), ItemsNewDataView: this.get_newViewId(), _dataView: this, Behaviors: [], format: _field_format}];
            this._fields = this._allFields;
            this._render();
        }
        else {
            this._busy(true);
            this._detachBehaviors();
            this._showWait();
            this._invoke('GetPage', { controller: this.get_controller(), view: this.get_viewId(), request: this._createParams() }, Function.createDelegate(this, this._onGetPageComplete));
        }
    },
    _invoke: function(methodName, params, onSuccess, userContext) {
        if (this.get_servicePath().startsWith('http')) {
            var m = this.get_servicePath().match(/(.+?)\w+\/\w+\.\w+(\?|$)/)
            var scriptParamId = String.format('__{0}_{1}_ScriptParam', this.get_id(), methodName);
            var scriptParam = $get(scriptParamId);
            var p = scriptParam ? scriptParam.value : Sys.Serialization.JavaScriptSerializer.serialize(params);
            if (scriptParam) scriptParam.value = '';
            var paramLength = p.length;
            while (p) {
                var src = String.format('{0}ScriptHost.ashx?sender={1}&method={2}&ctx={3}&args={4}&cookie={5}', m && m[1].startsWith('http') ? m[1] : '', this.get_id(), methodName, userContext, encodeURI(p), this.get_cookie());
                if (src.length <= 2048) {
                    if (scriptParam && scriptParam.value.length == 0)
                        scriptParam.parentElement.removeChild(scriptParam);
                    break;
                };
                if (!scriptParam) {
                    scriptParam = document.createElement('input');
                    scriptParam.setAttribute('type', 'hidden');
                    scriptParam.setAttribute('id', scriptParamId);
                    document.body.appendChild(scriptParam);
                }
                paramLength = Math.round(paramLength / 3 * 2);
                p = p + scriptParam.value; // reconstruct the serialized parameter
                scriptParam.value = p.substr(paramLength, p.length - paramLength);
                p = p.substr(0, paramLength)
            }
            var head = document.getElementsByTagName('head')[0];
            var script = document.createElement('script');
            script.setAttribute('id', String.format('__{0}_{1}_ScriptCallBack', this.get_id(), methodName));
            script.setAttribute('type', 'text/javascript');
            script.setAttribute('language', 'javascript');
            script.setAttribute('src', src + (scriptParam ? '&c=1' : ''));
            head.appendChild(script);
        }
        else
            Sys.Net.WebServiceProxy.invoke(this.get_servicePath(), methodName, false, params, onSuccess, Function.createDelegate(this, this._onMethodFailed), userContext);
    },
    _disposeFields: function() {
        if (this._allFields) {
            for (var i = 0; i < this._allFields.length; i++) {
                var f = this._allFields[i];
                f._dataView = null;
                if (f._listOfValues) Array.clear(f._listOfValues);
            }
        }
    },
    _formatViewText: function(text, lowerCase, altText) {
        var vl = this._views.length > 0 ? this._views[0].Label : '';
        return !String.isNullOrEmpty(text) ? String.format(text, lowerCase == true ? vl.toLowerCase() : vl) : altText;
    },
    _onGetPageComplete: function(result, context) {
        this._busy(false);
        if (Sys.Services && Sys.Services.AuthenticationService && Sys.Services.AuthenticationService.get_isLoggedIn && Sys.Services.AuthenticationService.get_isLoggedIn() && !result.IsAuthenticated) {
            window.location.reload();
            return;
        }
        if (this._pageIndex < 0) {
            if (this._pageIndex == -1) {
                this._disposeFields();
                this._expressions = result.Expressions;
                this._detachBehaviors();
                this._allFields = result.Fields;
                this._fields = [];
                var selectedKeyMap = [];
                if (this._keyFields && this._selectedKey.length > 0) {
                    for (var i = 0; i < this._keyFields.length; i++)
                        selectedKeyMap[i] = { 'name': this._keyFields[i].Name, 'value': this._selectedKey[i] };
                    this._selectedKey = [];
                }
                this._keyFields = [];
                for (i = 0; i < result.Fields.length; i++) result.Fields[i].Index = i;
                for (i = 0; i < result.Fields.length; i++) {
                    var field = result.Fields[i];
                    field.AliasIndex = field.AliasName && field.AliasName.length > 0 ? this.findField(field.AliasName).Index : i;
                    if (this._fieldIsInExternalFilter(field) && this.get_hideExternalFilterFields())
                        field.Hidden = true;
                    field.Behaviors = [];
                }
                this._hasDynamicLookups = false;
                this._requiresConfiguration = false;
                for (i = 0; i < result.Fields.length; i++) {
                    field = result.Fields[i];
                    field._dataView = this;
                    if (!field.Hidden) Array.add(this._fields, field);
                    if (field.IsPrimaryKey) {
                        Array.add(this._keyFields, field);
                        for (var j = 0; j < selectedKeyMap.length; j++) {
                            if (selectedKeyMap[j].name == field.Name) {
                                Array.add(this._selectedKey, selectedKeyMap[j].value);
                                break;
                            }
                        }
                    }
                    if (String.isNullOrEmpty(field.HeaderText)) field.HeaderText = field.Label;
                    if (String.isNullOrEmpty(field.HeaderText)) field.HeaderText = field.Name;
                    field.format = _field_format;
                    if (field.DataFormatString && field.DataFormatString.indexOf('{') == -1) field.DataFormatString = '{0:' + field.DataFormatString + '}';
                    if (field.DataFormatString) field.DataFormatString = this.resolveClientUrl(field.DataFormatString);
                    if (field.Type.startsWith('DateTime')) {
                        if (!field.DataFormatString) field.DataFormatString = '{0:d}';
                        else {
                            var fmt = Web.DataView.dateFormatStrings[field.DataFormatString];
                            if (fmt) field.DataFormatString = '{0:' + fmt + '}';
                        }
                    }
                    if (field.Type == 'Boolean' && field.Items.length == 0) {
                        field.Items = field.AllowNulls ? Web.DataViewResources.Data.BooleanOptionalDefaultItems : Web.DataViewResources.Data.BooleanDefaultItems;
                        if (!field.ItemsStyle) field.ItemsStyle = Web.DataViewResources.Data.BooleanDefaultStyle;
                    }
                    if (field.Items && field.Items.length > 0 && (field.AllowNulls || field.ItemsStyle == 'DropDownList') && !String.isNullOrEmpty(field.Items[0][0]) && field.ItemsStyle != 'CheckBoxList')
                        Array.insert(field.Items, 0, [null, Web.DataViewResources.Data.NullValueInForms]);
                    if (!String.isNullOrEmpty(field.ItemsStyle) && !String.isNullOrEmpty(field.ContextFields) && field.ItemsStyle != 'Lookup' && !String.isNullOrEmpty(field.ItemsDataController)) {
                        this._hasDynamicLookups = true;
                        field.ItemsAreDynamic = true;
                    }
                    if (!String.isNullOrEmpty(field.Configuration))
                        this._requiresConfiguration = true;
                    if (field.AllowLEV) this._allowLEVs = true;
                }
                this._views = result.Views;
                this._actionGroups = result.ActionGroups ? result.ActionGroups : [];
                var whenTest = /^(true|false)\:(.+)$/;
                for (i = 0; i < this._actionGroups.length; i++) {
                    var ag = this._actionGroups[i];
                    var agt = Web.DataViewResources.Actions.Scopes[ag.Scope];
                    if (agt._Self) {
                        var ast = agt._Self[ag.HeaderText];
                        if (ast) ag.HeaderText = ast.HeaderText;
                    }
                    for (j = 0; j < ag.Actions.length; j++) {
                        var action = ag.Actions[j];
                        if (String.isNullOrEmpty(action.HeaderText)) {
                            var at = agt[action.CommandName];
                            if (at) {
                                if (at.CommandArgument) {
                                    var at2 = at.CommandArgument[action.CommandArgument];
                                    if (at2) at = at2;
                                }
                                if (at.WhenLastCommandName) {
                                    at2 = at.WhenLastCommandName[action.WhenLastCommandName];
                                    if (at2) at = at2;
                                }
                                action.HeaderText = at.HeaderText;
                                if (!String.isNullOrEmpty(at.HeaderText) && at.HeaderText.indexOf('{') >= 0)
                                    action.HeaderText = at.VarMaxLen != null && result.Views[0].Label.length > at.VarMaxLen ? at.HeaderText2 : this._formatViewText(at.HeaderText);
                                action.Description = this._formatViewText(at.Description);
                                action.Confirmation = at.Confirmation;
                            }
                            else
                                action.HeaderText = action.CommandName;
                        }
                        if (String.isNullOrEmpty(action.WhenView))
                            action.WhenViewRegex = null;
                        else {
                            var m = whenTest.exec(action.WhenView);
                            action.WhenViewRegex = new RegExp(m ? m[2] : action.WhenView);
                            action.WhenViewRegexResult = m ? m[1] != 'false' : true;
                        }
                        if (String.isNullOrEmpty(action.WhenTag))
                            action.WhenTagRegex = null;
                        else {
                            m = whenTest.exec(action.WhenTag);
                            action.WhenTagRegex = new RegExp(m ? m[2] : action.WhenTag);
                            action.WhenTagRegexResult = m ? m[1] != 'false' : true;
                        }
                        if (String.isNullOrEmpty(action.WhenHRef))
                            action.WhenHRefRegex = null;
                        else {
                            m = whenTest.exec(action.WhenHRef);
                            action.WhenHRefRegex = new RegExp(m ? m[2] : action.WhenHRef);
                            action.WhenHRefRegexResult = m ? m[1] != 'false' : true;
                        }
                    }
                }
                var numberOfColumns = 1;
                var hasColumns = false;
                this._categories = result.Categories;
                this._tabs = [];
                for (i = 0; i < this._categories.length; i++) {
                    var c = this._categories[i];
                    c.Index = i;
                    if (!String.isNullOrEmpty(c.Tab) && !Array.contains(this._tabs, c.Tab))
                        Array.add(this._tabs, c.Tab);
                    if (c.NewColumn) {
                        if (i > 0) numberOfColumns++;
                        hasColumns = true;
                    }
                    c.ColumnIndex = numberOfColumns - 1;
                    if (c.Floating && String.isNullOrEmpty(c.Template)) {
                        var sb = new Sys.StringBuilder();
                        for (j = 0; j < this._allFields.length; j++) {
                            var f = this._allFields[j];
                            if (!f.Hidden && i == f.CategoryIndex)
                                sb.appendFormat('<div class="FieldPlaceholder">{{{0}}}</div>', f.Name);
                        }
                        c.Template = sb.toString();
                    }
                }
                if (this._tabs.length > 0) {
                    for (i = 0; i < this._categories.length; i++) {
                        c = this._categories[i];
                        if (String.isNullOrEmpty(c.Tab)) {
                            c.Tab = Web.DataViewResources.Form.GeneralTabText;
                            if (this._tabs[0] != Web.DataViewResources.Form.GeneralTabText) Array.insert(this._tabs, 0, Web.DataViewResources.Form.GeneralTabText);
                        }
                        c.ColumnIndex = 0;
                    }
                    if (this._lastSelectedCategoryTabIndex != null) {
                        this.set_categoryTabIndex(!(this._lastSelectedCategoryTabIndex >= 0) ? this._tabs.length - 1 : 0);
                        delete this._lastSelectedCategoryTabIndex;
                    }
                    else
                        this.set_categoryTabIndex(0);
                    numberOfColumns = 1;
                }
                else
                    this.set_categoryTabIndex(-1);
                this._numberOfColumns = hasColumns ? numberOfColumns : 0;
            }
            this._totalRowCount = result.TotalRowCount;
            this._filter = result.Filter;
            this._sortExpression = result.SortExpression;
            this._pageIndex = result.PageIndex;
            this._firstPageButtonIndex = Math.floor(result.PageIndex / Web.DataViewResources.Pager.PageButtonCount) * Web.DataViewResources.Pager.PageButtonCount; //result.PageIndex;
            this._pageSize = result.PageSize;
            this._pageCount = Math.floor(result.TotalRowCount / result.PageSize);
            if (result.TotalRowCount % result.PageSize != 0)
                this._pageCount++;
        }
        this._icons = result.Icons;
        if (!this.get_isInserting()) {
            if (this._rows) {
                for (i = 0; i < this._rows.length; i++)
                    Array.clear(this._rows[i]);
                Array.clear(this._rows);
            }
            this._rows = result.Rows;
        }
        this._newRow = result.NewRow;
        if (result.Aggregates) this._aggregates = result.Aggregates;
        if (this.get_view().Type == 'Form' && this._selectedRowIndex == null && this._totalRowCount > 0) {
            this._selectedRowIndex = 0;
            this._selectKeyByRowIndex(0);
        }
        if (this.get_startCommandName()) {
            var command = this.get_startCommandName();
            var argument = this.get_startCommandArgument();
            this.set_startCommandName(null);
            this.set_startCommandArgument(null);
            this.executeCommand({ commandName: command, commandArgument: argument ? argument : '' });
            if (this._skipRender) {
                this._skiprender = false;
                return;
            }
        }
        this._render();
        if (this.get_modalAnchor())
            this._adjustModalPopupSize();
        else
            this._adjustLookupSize();
        if (this._isInInstantDetailsMode()) {
            var size = $common.getClientBounds();
            var contentSize = $common.getContentSize(document.body);
            window.resizeBy(0, contentSize.height - size.height);
        }
        if (this._pendingSelectedEvent) {
            this._pendingSelectedEvent = false;
            this.updateSummary();
        }

        this._registerFieldHeaderItems();
        _body_performResize();
    },
    _initializeModalPopup: function() {
        Sys.UI.DomElement.addCssClass(this.get_element(), 'ModalPlaceholder');
        var cb = $common.getClientBounds();
        var width = cb.width / 5 * 4;
        if (width > Web.DataViewResources.ModalPopup.MaxWidth) width = Web.DataViewResources.ModalPopup.MaxWidth;
        var height = cb.height / 5 * 4;
        if (this._container.style.overflowX != null) {
            this._container.style.overflowY = 'auto';
            this._container.style.overflowX = 'hidden';
        }
        else
            this._container.style.overflow = 'auto';
        this._container.style.height = height + 'px';
        this._container.style.width = width + 'px';
        this._saveTabIndexes();
        this._modalPopup = $create(AjaxControlToolkit.ModalPopupBehavior, { id: this.get_id() + 'ModalPopup' + Sys.Application.getComponents().length, PopupControlID: this.get_element().id, DropShadow: true, BackgroundCssClass: 'ModalBackground' }, null, null, this.get_modalAnchor());
        this._modalPopup.show();
    },
    _adjustModalPopupSize: function() {
        var sb = new Sys.StringBuilder();
        var rowsToDelete = [];
        var tables = this._container.getElementsByTagName('table');
        for (var i = tables.length - 1; i >= 0; i--) {
            var t = tables[i];
            if (t.className == 'ActionButtons') {
                if (sb.isEmpty()) {
                    sb.append('<table class="DataView" cellSpacing=0 cellPadding=0><tr class="ActionButtonsRow BottomButtonsRow">')
                    sb.append(t.parentNode.parentNode.innerHTML);
                    sb.append('</tr></table>');
                }
                Array.add(rowsToDelete, t.parentNode.parentNode);
            }
        }
        while (rowsToDelete.length > 0) {
            rowsToDelete[0].parentNode.removeChild(rowsToDelete[0]);
            delete rowsToDelete[0];
            Array.removeAt(rowsToDelete, 0);
        }
        if (!this._buttons) {
            var b = $common.getContentSize(this._container.childNodes[0]);
            this._buttons = document.createElement('div');
            this.get_element().appendChild(this._buttons);
            this._buttons.style.width = b.width + 'px';
            Sys.UI.DomElement.addCssClass(this._buttons, 'FixedButtons');
            this._title = document.createElement('div');
            this._title.innerHTML = Web.DataView.htmlEncode(this.get_view().Label);
            Sys.UI.DomElement.addCssClass(this._title, 'FixedTitle');
            this.get_element().insertBefore(this._title, this._container);
        }
        this._buttons.innerHTML = sb.toString();
        sb.clear();
        var containerBounds = $common.getBounds(this._container);
        var contentSize = $common.getBounds(this._container.childNodes[0]);
        var clientBounds = $common.getClientBounds();
        var maxHeight = Math.ceil(clientBounds.height / 5 * 4);
        if (containerBounds.height > maxHeight)
            this._container.style.height = maxHeight + 'px';
        if (containerBounds.height > contentSize.height) {
            $common.setContentSize(this._container, contentSize);
        }
        this._container.childNodes[0].style.width = this._title.offsetWidth + 'px';
        this._buttons.style.width = this._title.offsetWidth + 'px';
        Sys.UI.DomElement.setVisible(this.get_element(), true);
        if (this._modalPopup) {
            if (Sys.Browser.agent === Sys.Browser.InternetExplorer) this._modalPopup.hide();
            this._modalPopup.show();
        }
        if (Sys.Browser.agent === Sys.Browser.InternetExplorer && this.get_isEditing()) this._focus();
    },
    endModalState: function(commandName) {
        if (this.get_isModal()) {
            var exitCommands = this.get_exitModalStateCommands();
            if (exitCommands) {
                for (var i = 0; i < exitCommands.length; i++) {
                    if (commandName == exitCommands[i]) {
                        this._modalPopup.dispose();
                        this._restoreTabIndexes();
                        var elem = this.get_element();
                        this.dispose();
                        elem.parentNode.removeChild(elem);
                        return true;
                    }
                }
            }
        }
        return false;
    },
    _adjustLookupSize: function() {
        if (this.get_lookupField() && Web.DataView.isIE6) this.get_lookupField()._lookupModalBehavior._layout();
        if (this.get_lookupField() && this.get_pageSize() > 3) {
            var scrolling = $common.getScrolling();
            var clientBounds = $common.getClientBounds()
            var b = $common.getBounds(this.get_element());
            if (b.height + b.y > clientBounds.height + scrolling.y) this.set_pageSize(Math.ceil(this.get_pageSize() * 0.66));
        }
    },
    _onMethodFailed: function(err, response, context) {
        if (Web.DataView._navigated) return;
        this._busy(false);
        alert(String.format('Timed out: {0}\r\nException: {1}\r\nMessage: {2}\r\nStack:\r\n{3}', err.get_timedOut(), err.get_exceptionType(), err.get_message(), err.get_stackTrace()));
    },
    _loadListOfValues: function(family, fieldName, distinctFieldName) {
        this._busy(true);
        var lc = this.get_lookupContext();
        var filter = this.get_filter();
        this._invoke('GetListOfValues', { controller: this.get_controller(), view: this.get_viewId(), request: { FieldName: distinctFieldName, Filter: filter.length == 1 && filter[0].match(/(\w+):/)[1] == distinctFieldName ? null : filter, LookupContextFieldName: lc ? lc.FieldName : null, LookupContextController: lc ? lc.Controller : null, LookupContextView: lc ? lc.View : null} },
            Function.createDelegate(this, this._onGetListOfValuesComplete), { 'family': family, 'fieldName': fieldName });
    },
    _onGetListOfValuesComplete: function(result, context) {
        this._busy(false);
        var field = this.findField(context.fieldName);
        field._listOfValues = result;
        if (result[result.length - 1] == null) {
            Array.insert(result, 0, result[result.length - 1]);
            Array.removeAt(result, result.length - 1);
        }
        this._registerFieldHeaderItems(Array.indexOf(this.get_fields(), field));
        $refreshHoverMenu(context.family);
        Web.DataView._resized = true;
    },
    get_selectedValues: function() {
        var selection = this.get_selectedValue();
        return selection.length == 0 ? [] : (this.get_selectionMode() == Web.DataViewSelectionMode.Single ? [selection] : selection.split(';'));
    },
    _execute: function(args) {
        this._busy(true);
        this._showWait();
        this._lastArgs = args;
        args.Filter = this.get_filter();
        args.SortExpression = this.get_sortExpression();
        args.SelectedValues = this.get_selectedValues();
        args.ExternalFilter = this.get_externalFilter();
        this._invoke('Execute', { controller: args.Controller, view: args.View, args: args }, Function.createDelegate(this, this._onExecuteComplete));
    },
    _populateDynamicLookups: function(result) {
        for (var i = 0; i < result.Values.length; i++) {
            var v = result.Values[i];
            var f = this.findField(v.Name);
            if (f) f.DynamicItems = v.NewValue;
        }
        this._skipPopulateDynamicLookups = true;
        this.refresh(true);
        this._focus();
    },
    _updateCalculatedFields: function(result) {
        var values = []
        for (var i = 0; i < result.Values.length; i++) {
            var v = result.Values[i];
            Array.add(values, { 'name': v.Name, 'value': v.NewValue });
        }
        this.refresh(true, values);
        this._focus();
    },
    _get_LEVs: function() {
        for (var i = 0; i < Web.DataView.LEVs.length; i++) {
            var lev = Web.DataView.LEVs[i];
            if (lev.controller == this.get_controller())
                return lev.records;
        }
        lev = { 'controller': this.get_controller(), 'records': [] };
        Array.add(Web.DataView.LEVs, lev);
        return lev.records;
    },
    _recordLEVs: function() {
        if (!this._allowLEVs || !this._lastArgs || !this._lastArgs.CommandName.match(/Insert|Update/)) return;
        var levs = this._get_LEVs();
        var skip = true;
        for (var i = 0; i < this._lastArgs.Values.length; i++) {
            if (this._lastArgs.Values[i].Modified) {
                skip = false;
                break;
            }
        }
        if (skip) return;
        if (levs.length > 0)
            Array.removeAt(levs, levs.length - 1);
        Array.insert(levs, 0, this._lastArgs.Values)
    },
    _applyLEV: function(fieldIndex) {
        var f = this._allFields[fieldIndex];
        var f2 = this._allFields[f.AliasIndex];
        var values = [];
        var r = this._get_LEVs()[0];
        for (var i = 0; i < r.length; i++) {
            var v = r[i];
            if (v.Name == f.Name || v.Name == f2.Name)
                Array.add(values, { 'name': v.Name, 'value': v.NewValue });
        }
        this.refresh(true, values);
    },
    _onExecuteComplete: function(result, context) {
        this._busy(false);
        this._hideWait();
        if (this._lastArgs.CommandName == 'PopulateDynamicLookups') {
            this._populateDynamicLookups(result);
            return;
        }
        else if (this._lastArgs.CommandName == 'Calculate') {
            this._updateCalculatedFields(result);
            return;
        }
        var ev = { 'result': result, 'context': context, 'handled': false }
        this.raiseExecuted(ev);
        if (ev.handled) return;
        var existingRow = !this._lastArgs.CommandName.match(/Insert/i);
        if (this._lastArgs.CommandName.match(/Delete/i) && result.RowsAffected > 0) {
            this._selectedKey = [];
            this._selectedKeyFilter = [];
            this.raiseSelected();
        }
        else if (existingRow) {
            for (var i = 0; i < result.Values.length; i++) {
                var v = result.Values[i];
                var field = this.findFind(v.Name);
                if (field) this.get_selectedRow()[field.Index] = v.NewValue;
            }
        }
        else {
            this._selectedKey = [];
            this._selectedKeyFilter = [];
            if (result.Values.length == 0) result.Values = this._lastArgs.Values;
            for (i = 0; i < this._keyFields.length; i++) {
                field = this._keyFields[i];
                v = null;
                for (var j = 0; j < result.Values.length; j++)
                    if (result.Values[j].Name == field.Name) {
                    v = result.Values[j];
                    break;
                }
                Array.add(this._selectedKey, v ? v.NewValue : null);
                Array.add(this._selectedKeyFilter, field.Name + ':=' + this.convertFieldValueToString(field, v ? v.NewValue : null));
            }
            this.raiseSelected();
        }
        if (result.Errors.length == 0) {
            this._selectedKeyList = [];
            this._recordLEVs();
            this.updateSummary();
            if (result.ClientScript) {
                result.ClientScript = this.resolveClientUrl(result.ClientScript);
                eval(result.ClientScript);
                if (this._exportRedirect && this.get_servicePath().startsWith('http'))
                    this._exportRedirect = this.resolveClientUrl(this._exportRedirect.replace(/(.*?)(\/\w+\.\w+.*)$/, '~$2'));
            }
            else if (result.NavigateUrl) {
                result.NavigateUrl = this.resolveClientUrl(result.NavigateUrl);
                this.navigate(result.NavigateUrl, existingRow ? this._lastArgs.Values : result.Values);
            }
            else if (this._closeInstantDetails()) { }
            else if (this.endModalState(this._lastArgs.CommandName)) { }
            else if (this.get_backOnCancel()) history.go(-1)
            else {
                var actions = this.get_actions(this.get_view().Type);
                var lastCommand = this._lastArgs.CommandName;
                var lastArgument = this._lastArgs.CommandArgument;
                for (i = 0; i < actions.length; i++) {
                    var a = actions[i];
                    if (a.WhenLastCommandName == lastCommand && (a.WhenLastCommandArgument == '' || a.WhenLastCommandArgument == lastArgument) && this._isActionMatched(a)) {
                        this.executeCommand({ commandName: a.CommandName, commandArgument: a.CommandArgument, causesValidation: a.CausesValidation });
                        return;
                    }
                }
                this.set_lastCommandName(null);
                this.goToView(this._lastViewId);
            }
        }
        else {
            if (result.ClientScript) {
                result.ClientScript = this.resolveClientUrl(result.ClientScript);
                eval(result.ClientScript);
            }
            var sb = new Sys.StringBuilder();
            for (i = 0; i < result.Errors.length; i++)
                sb.append(Web.DataView.formatMessage('Attention', result.Errors[i]));
            Web.DataView.showMessage(sb.toString());
            sb.clear();
        }
    },
    _busy: function(isBusy) {
        this._isBusy = isBusy;
        this._enableButtons(!isBusy);
    },
    _enableButtons: function(enable) {
        var buttons = this._element.getElementsByTagName('button');
        for (var i = 0; i < buttons.length; i++) {
            var b = buttons[i];
            if (b)
                if (!enable) {
                b.WasDisabled = true;
                b.disabled = true;
            }
            else if (b.WasDisabled) {
                b.WasDisabled = false;
                b.disabled = false;
            }
        }
        buttons = null;
    },
    _bodyKeydown: function(e) {
        if (this._customFilterField) {
            if (e.keyCode == Sys.UI.Key.enter) {
                e.preventDefault();
                e.stopPropagation();
                this.applyCustomFilter();
            }
            else if (e.keyCode == Sys.UI.Key.esc) this.closeCustomFilter();
        }
        else if (this.get_lookupField())
            if (e.keyCode == Sys.UI.Key.esc) this.hideLookup();
    },
    _filterSourceSelected: function(sender, args, keepContext) {
        var oldValues = [];
        for (var i = 0; i < this._externalFilter.length; i++) {
            Array.add(oldValues, this._externalFilter[i].Value);
            this._externalFilter[i].Value = null;
        }
        if (Web.DataView.isInstanceOfType(sender))
            this._populateExternalViewFilter(sender);
        else if (this._externalFilter.length > 0)
            this._externalFilter[0].Value = sender.target.value;
        this.applyExternalFilter();
        var emptySourceFilter = true;
        var sourceFilterChanged = false;
        for (i = 0; i < this._externalFilter.length; i++) {
            var v = this._externalFilter[i].Value;
            if (v != null) emptySourceFilter = false;
            if (v != oldValues[i]) sourceFilterChanged = true;
        }
        if (this.get_autoHide() != Web.AutoHideMode.Nothing) this._updateLayoutContainerVisibility(!emptySourceFilter);
        if (sourceFilterChanged) {
            if (!keepContext) this.set_pageIndex(-1);
            this.loadPage();
            if (!keepContext) Array.clear(this._selectedKey);
        }
        this.raiseSelected();
        this.updateSummary();
    },
    _createExternalFilter: function() {
        this._externalFilter = [];
        var iterator = /(\w+)(,|$)/g;
        if (this.get_filterFields()) {
            var s = match = iterator.exec(this.get_filterFields());
            var match = iterator.exec(s);
            while (match) {
                Array.add(this._externalFilter, { Name: match[1], Value: null });
                match = iterator.exec(s);
            }
        }
    },
    _populateExternalViewFilter: function(view) {
        if (!(view._selectedKey && view._keyFields && view._selectedKey.length == view._keyFields.length)) return;
        for (var i = 0; i < this._externalFilter.length; i++) {
            var filterItem = this._externalFilter[i];
            var found = false;
            for (var j = 0; j < view._keyFields.length; j++) {
                var field = view._keyFields[j];
                if (filterItem.Name == field.Name) {
                    filterItem.Value = view.convertFieldValueToString(field, view._selectedKey[j]);
                    found = true;
                    break;
                }
            }
            if (!found && this.get_controller() != view.get_controller())
                for (j = 0; j < view._allFields.length; j++) {
                field = view._allFields[j];
                if (filterItem.Name == field.Name) {
                    filterItem.Value = view.convertFieldValueToString(field, view.get_selectedRow()[view._allFields[j].Index]);
                    found = true;
                    break;
                }
            }
            if (!found && view._selectedKey.length >= i)
                filterItem.Value = view._selectedKey[i];
        }
    },
    _cloneChangedRow: function() {
        if (this.get_isEditing()) {
            var values = this._collectFieldValues();
            var selectedRow = this.get_currentRow();
            var row = selectedRow ? Array.clone(selectedRow) : null;
            for (var i = 0; i < values.length; i++) {
                var v = values[i];
                var f = this.findField(v.Name);
                if (f && !f.ReadOnly)
                    row[f.Index] = v.NewValue;
            }
            return row;
        }
        else
            return this.get_selectedRow();
    },
    _updateVisibility: function() {
        if (!this._expressions) return;
        var expressions = [];
        var row = this._cloneChangedRow();
        if (!row) return;
        var changed = false;
        for (var i = 0; i < this._expressions.length; i++) {
            var exp = expressions[0] = this._expressions[i];
            if (exp.Scope == Web.DynamicExpressionScope.DataFieldVisibility && exp.Type == Web.DynamicExpressionType.ClientScript) {
                var f = this.findField(exp.Target);
                if (f) {
                    var elem = $get(String.format('{0}_ItemContainer{1}', this.get_id(), f.Index));
                    if (elem) {
                        var result = this._evaluateJavaScriptExpressions(expressions, row, false);
                        var isVisible = Sys.UI.DomElement.getVisible(elem);
                        if (Sys.UI.DomElement.containsCssClass(elem.parentNode, 'FieldPlaceholder')) elem = elem.parentNode;
                        else if (Sys.UI.DomElement.containsCssClass(elem.parentNode.parentNode, 'FieldWrapper')) elem = elem.parentNode.parentNode.parentNode.parentNode;
                        Sys.UI.DomElement.setVisible(elem, result == true);
                        if (isVisible != result) changed = true;
                    }
                }
            }
            else if (exp.Scope == Web.DynamicExpressionScope.CategoryVisibility && exp.Type == Web.DynamicExpressionType.ClientScript) {
                var c = this.findCategory(exp.Target);
                if (c) {
                    elem = $get(String.format('{0}_Category{1}', this.get_id(), c.Index));
                    if (elem) {
                        result = this._evaluateJavaScriptExpressions(expressions, row, false);
                        isVisible = Sys.UI.DomElement.getVisible(elem);
                        Sys.UI.DomElement.setVisible(elem, result == true);
                        if (isVisible != result) changed = true;
                    }
                }
            }
        }
        if (changed) _body_performResize();
    },
    _updateDynamicValues: function(field) {
        for (var i = 0; i < this._allFields.length; i++) {
            var f = this._allFields[i];
            var hasContextFields = !String.isNullOrEmpty(f.ContextFields);
            if (hasContextFields) {
                var m = f.ContextFields.match(/\w+=(\w+)/);
                if (f.ItemsAreDynamic && (field == null || m && m[1] == field.Name)) {
                    this._raisePopulateDynamicLookups();
                    break;
                }
                else if (f.Calculated && f.ContextFields.match(field.Name))
                    this._raiseCalculate(f);
            }
        }
    },
    _valueFocused: function(index) {
        var field = this._allFields[index];
        this._focusedFieldName = field.Name;
        Web.DataView._focusedDataViewId = this._id;
        Web.DataView._focusedItemIndex = index;
    },
    _copyStaticLookupValues: function(field) {
        if (!String.isNullOrEmpty(field.Copy) && (field.ItemsStyle == 'RadioButtonList' || field.ItemsStyle == 'ListBox' || field.ItemsStyle == 'DropDownList')) {
            var currentRow = this._collectFieldValues();
            var selectedValue = currentRow[field.Index].NewValue;
            if (selectedValue != null && typeof (selectedValue) != 'string')
                selectedValue = selectedValue.toString();
            var selectedItem = null;
            for (var i = 0; i < field.Items.length; i++) {
                var item = field.Items[i];
                var itemValue = item[0];
                if (itemValue != null && typeof (itemValue) != 'string') itemValue = itemValue.toString();
                if (itemValue == selectedValue) {
                    selectedItem = item;
                    break;
                }
            }
            if (selectedItem) {
                var values = [];
                var iterator = /(\w+)=(\w+)/g;
                var index = 2;
                var m = iterator.exec(field.Copy);
                while (m) {
                    Array.add(values, { 'name': m[1], 'value': selectedItem[index++] });
                    m = iterator.exec(field.Copy);
                }
                this.refresh(true, values);
            }
        }
    },
    _valueChanged: function(index) {
        var field = this._allFields[index];
        this._copyStaticLookupValues(field);
        this._updateVisibility();
        this._updateDynamicValues(field);
    },
    _quickFind_focus: function(e) {
        var qf = this.get_quickFindElement();
        if (qf.value == Web.DataViewResources.Grid.QuickFindText)
            qf.value = '';
        Sys.UI.DomElement.removeCssClass(qf, 'Empty');
        Sys.UI.DomElement.removeCssClass(qf, 'NonEmpty');
        qf.select();
    },
    _quickFind_blur: function(e) {
        var qf = this.get_quickFindElement();
        if (String.isBlank(qf.value)) {
            qf.value = Web.DataViewResources.Grid.QuickFindText;
            Sys.UI.DomElement.addCssClass(qf, 'Empty');
        }
        else
            Sys.UI.DomElement.addCssClass(qf, 'NonEmpty');
    },
    quickFind: function(sample) {
        var q = (String.isNullOrEmpty(sample) ? this.get_quickFindElement().value : sample).match(/^\s*(.*?)\s*$/);
        var qry = q[1] == Web.DataViewResources.Grid.QuickFindText ? '' : q[1];
        this.set_quickFindText(qry);
        for (var i = 0; i < this._allFields.length; i++)
            this._allFields[i]._listOfValues = null;
        for (i = 0; i < this._allFields.length; i++) {
            var f = this._allFields[i];
            if (!f.Hidden) {
                f = this._allFields[f.AliasIndex];
                if (String.isNullOrEmpty(qry)) {
                    this.removeFromFilter(f);
                    this.set_pageIndex(-2);
                    this._loadPage();
                }
                else
                    this.applyFilter(f, '~', qry);
                break;
            }
        }
    },
    _quickFind_keydown: function(e) {
        if (e.keyCode == Sys.UI.Key.enter) {
            e.preventDefault();
            e.stopPropagation();
            this.quickFind();
        }
    }
}

Web.DataView.registerClass('Web.DataView', Sys.UI.Behavior);

Web.DataView.hideMessage = function() { Web.DataView.showMessage() }

Web.DataView.formatMessage = function(type, message) { return String.format('<table cellpadding="0" cellspacing="0" style="width:100%"><tr><td class="{0}" valign="top">&nbsp;</td><td class="Message">{1}</td></tr></table>', type, message) }

Web.DataView.showMessage = function(message) {
    if (String.isBlank(message)) message = null;
    var bodyTag = document.getElementsByTagName('body')[0];
    var messageIsVisible = false;
    if (!Web.DataView.MessageBar) {
        var panel = document.createElement('div');
        panel.id = 'DataView_MessageBar';
        bodyTag.appendChild(panel);
        Sys.UI.DomElement.setVisible(panel, false);
        Sys.UI.DomElement.addCssClass(panel, 'MessageBar');
        Web.DataView.MessageBar = $create(AjaxControlToolkit.AlwaysVisibleControlBehavior, { VerticalOffset: AjaxControlToolkit.VerticalSide.Top }, null, null, panel);
        var b = Sys.UI.DomElement.getBounds(bodyTag);
        if (b.y < 0) b.y = 0;
        Web.DataView.OriginalBodyTopOffset = b.y;
    }
    panel = $get('DataView_MessageBar');
    var visible = Sys.UI.DomElement.getVisible(panel);
    panel.innerHTML = message ? message : '';
    Sys.UI.DomElement.setVisible(panel, message != null);
    var bounds = Sys.UI.DomElement.getBounds(panel);
    var bodyTop = message ? Web.DataView.OriginalBodyTopOffset + bounds.height : Web.DataView.OriginalBodyTopOffset;
    bodyTag.style.paddingTop = bodyTop + 'px';
    var loginDialog = $get("Membership_Login");
    if (loginDialog) loginDialog.style.marginTop = (bodyTop) + 'px';
    if (Sys.UI.DomElement.getVisible(panel) != visible) _body_performResize();
}

Web.DataView._tagsWithIndexes = new Array('A', 'AREA', 'BUTTON', 'INPUT', 'OBJECT', 'SELECT', 'TEXTAREA', 'IFRAME');
Web.DataView._delayedLoadingViews = [];

Web.DataView._performDelayedLoading = function() {
    var i = 0;
    while (i < Web.DataView._delayedLoadingViews.length) {
        var v = Web.DataView._delayedLoadingViews[i];
        if (v.get_isDisplayed()) {
            Array.remove(Web.DataView._delayedLoadingViews, v);
            v._loadPage();
        }
        else i++;
    }
}

Web.DataView.find = function(id) {
    var cid = '_' + id;
    var list = Sys.Application.getComponents();
    for (var i = 0; i < list.length; i++) {
        var c = list[i];
        if (Web.DataView.isInstanceOfType(c) && c.get_id().endsWith(cid) || c.get_id() == id) return c;
    }
    return null;
}

Web.DataView.showModal = function(anchor, controller, view, startCommandName, startCommandArgument, baseUrl, servicePath, filter) {
    if (!anchor) {
        var links = document.getElementsByTagName('a', 'input', 'button');
        for (var i = links.length - 1; i >= 0; i--) {
            if (links[i].tabIndex >= 0) {
                anchor = links[i];
                break;
            }
        }
    }
    if (anchor == null) {
        alert('Cannot find an anchor for a modal popup.');
        return;
    }
    if (!baseUrl) baseUrl = Web.DataView._baseUrl;
    if (!servicePath) servicePath = Web.DataView._servicePath;
    var placeholder = this._placeholder = document.createElement('div');
    placeholder.id = String.format('{0}_{1}_Placeholder{2}', controller, view, Sys.Application.getComponents().length);
    document.body.appendChild(placeholder);
    placeholder.className = 'ModalPlaceholder FixedDialog';
    Sys.UI.DomElement.setVisibilityMode(placeholder, Sys.UI.VisibilityMode.hide);
    Sys.UI.DomElement.setVisible(placeholder, false);
    return $create(Web.DataView, { id: controller + '_ModalDataView' + Sys.Application.getComponents().length, baseUrl: baseUrl, servicePath: servicePath,
        controller: controller, viewId: view, showActionBar: false, modalAnchor: anchor, startCommandName: startCommandName, startCommandArgument: startCommandArgument, exitModalStateCommands: ['Cancel'], externalFilter: filter
    }, null, null, placeholder);
}

Web.DataView._resizeInterval = null;
Web.DataView._resizing = false;
Web.DataView._resized = false;

function _body_hideLayoutContainers() {
    if (!Web.DataView._layoutContainers) return;
    for (var i = 0; i < Web.DataView._layoutContainers.length; i++) {
        var lc = Web.DataView._layoutContainers[i];
        if (lc.width != '100%')
            Sys.UI.DomElement.setVisible($get(lc.id), false);
    }
}

function _body_resizeLayoutContainers() {
    var layoutContainers = Web.DataView._layoutContainers;
    if (!layoutContainers || layoutContainers.length == 0) return;
    var pc = $get('PageContent');
    var bounds = $common.getBounds(pc);
    var padding = $common.getPaddingBox(pc);
    var border = $common.getBorderBox(pc);
    var margin = $common.getMarginBox(pc);
    bounds.width -= padding.horizontal + border.horizontal + margin.horizontal;
    var rowIndex = layoutContainers[layoutContainers.length - 1].rowIndex;
    while (rowIndex >= layoutContainers[0].rowIndex) {
        var usedSpace = 0;
        for (var i = 0; i < layoutContainers.length; i++) {
            var lc = layoutContainers[i];
            if (lc.rowIndex == rowIndex && !String.isNullOrEmpty(lc.width)) {
                var div = $get(lc.id);
                var divPadding = $common.getPaddingBox(div);
                var divBorder = $common.getBorderBox(div);
                var divMargin = $common.getMarginBox(div);
                var m = lc.width.match(/(\d+)(%|px|)/);
                var divWidth = m[2] != '%' ? parseFloat(m[1]) : Math.floor(bounds.width * parseFloat(m[1]) / 100);
                usedSpace += divWidth;
                divWidth -= divPadding.horizontal + divBorder.horizontal + divMargin.horizontal
                if (lc.width != '100%') {
                    div.style.width = divWidth + 'px';
                    Sys.UI.DomElement.setVisible(div, true);
                }
                else
                    Sys.UI.DomElement.removeCssClass(div, 'LayoutContainer');
            }
        }
        if (usedSpace < bounds.width) {
            for (i = 0; i < layoutContainers.length; i++) {
                lc = layoutContainers[i];
                if (lc.rowIndex == rowIndex && String.isNullOrEmpty(lc.width)) {
                    div = $get(lc.id);
                    divPadding = $common.getPaddingBox(div);
                    divBorder = $common.getBorderBox(div);
                    divMargin = $common.getMarginBox(div);
                    divWidth = Math.floor((bounds.width - usedSpace) / lc.peersWithoutWidth);
                    divWidth -= divPadding.horizontal + divBorder.horizontal + divMargin.horizontal
                    if (divWidth < 1) divWidth = 1;
                    div.style.width = divWidth + 'px';
                    Sys.UI.DomElement.setVisible(div, true);
                }
            }
        }
        rowIndex--;
    }
}

function _body_keydown(e) {
    if (e.keyCode == Sys.UI.Key.enter && Web.DataView._focusedItemIndex != null) {
        var elem = $get(Web.DataView._focusedDataViewId + '_Item' + Web.DataView._focusedItemIndex);
        if (elem && elem.tagName == 'INPUT' && elem.type == 'text' && elem == e.target) {
            e.preventDefault();
            e.stopPropagation();
            var dv = $find(Web.DataView._focusedDataViewId);
            if (dv) dv._valueChanged(Web.DataView._focusedItemIndex);
        }
    }
}

function _body_resize() {
    if (Web.DataView._resizeInterval)
        window.clearInterval(Web.DataView._resizeInterval);
    if (!Web.DataView._resizing && !Web.DataView._resized) {
        Web.DataView._resizeInterval = window.setInterval('_body_performResize()', 10);
        _body_hideLayoutContainers();
        _body_resizeLayoutContainers();
    }
    else
        $closeHovers();
    Web.DataView._resized = false;
}

function _body_scroll() {
    var sideBar = $getSideBar();
    var scrolling = $common.getScrolling();
    var clientBounds = $common.getClientBounds();
    var bounds = $common.getBounds(sideBar);
    if (sideBar._originalTop == null)
        sideBar._originalTop = bounds.y;
    if (scrolling.y > sideBar._originalTop && bounds.height + 4 <= clientBounds.height) {
        sideBar.style.width = bounds.width + 'px';
        if (Sys.Browser.agent == Sys.Browser.InternetExplorer && Sys.Browser.version <= 6) {
            sideBar.style.top = (4 + scrolling.y) + 'px';
            sideBar.style.position = 'absolute';
        }
        else {
            sideBar.style.top = 4 + 'px';
            sideBar.style.position = 'fixed';
        }
    }
    else {
        sideBar.style.top = '';
        sideBar.style.width = '';
        sideBar.style.position = '';
    }
}

function _body_createPageContext(persist) {
    var pc = $get('PageContent');
    if (!pc) return;
    var b = $common.getBounds(pc);
    var pb = $common.getPaddingBox(pc);
    var bb = $common.getBorderBox(pc);
    var ctx = { 'height': b.height - pb.vertical - bb.vertical, 'scrolling': $common.getScrolling() };
    if (persist != false) Web.DataView._pageContext = ctx;
    return ctx;
}

function _body_performResize() {
    if (Web.DataView._resizeInterval) window.clearInterval(Web.DataView._resizeInterval);
    Web.DataView._resizeInterval = null;
    $closeHovers();
    var pc = $get('PageContent');
    if (!pc) return;
    Web.DataView._resizing = true;
    var pageContext = Web.DataView._pageContext;
    if (pageContext == null)
        pageContext = _body_createPageContext(false);
    else
        Web.DataView._pageContext = null;
    var scrolling = $common.getScrolling();
    if (scrolling.y == 0) pc.style.height = '10px';
    _body_resizeLayoutContainers();
    //_body_scroll();
    var bounds = $common.getBounds(pc);
    var padding = $common.getPaddingBox(pc);
    var border = $common.getBorderBox(pc);
    var pfc = $get('PageFooterContent');
    var pfb = $get('PageFooterBar');
    var cb = $common.getClientBounds();
    var newHeight = scrolling.y + cb.height - bounds.y - pfb.offsetHeight - pfc.offsetHeight - border.vertical - padding.vertical;
    if (bounds.height < newHeight) {
        if (Sys.Browser.agent == Sys.Browser.Firefox || Sys.Browser.agent == Sys.Browser.Opera)
            newHeight += border.vertical + padding.vertical;
    }
    if (pageContext.scrolling.y == 0 || Web.DataView._numberOfContainers < 2)
        pc.style.height = document.body.offsetHeight > cb.height ? '' : newHeight + 'px';
    else {
        pc.style.height = pageContext.height + 'px';
        window.scrollTo(0, pageContext.scrolling.y);
    }
    _body_scroll();
    Web.DataView._resizing = false;
    Web.DataView._resized = true;
}

Web.DataView._activate = function(source, elementId, type) {
    var activatorRegex = new RegExp('^\\s*' + type + '\\s*\\|');
    var elem = $get(elementId);
    if (type == 'SideBarTask') {
        var lc = elem;
        while (lc && String.isNullOrEmpty(lc.getAttribute('factory:flow')))
            lc = lc.parentNode;
        var peers = lc.getElementsByTagName('div');
        for (var i = 0; i < peers.length; i++) {
            var activator = peers[i].getAttribute('factory:activator');
            if (!String.isNullOrEmpty(activator) && activatorRegex.exec(activator))
                Sys.UI.DomElement.setVisible(peers[i], false);
        }
    }
    Sys.UI.DomElement.setVisible(elem, type == 'SideBarTask' ? true : !Sys.UI.DomElement.getVisible(elem));
    elem._activated = true;
    if (type == 'SiteAction' && elem.childNodes[0].className != 'CloseSiteAction') {
        var closeLink = document.createElement('div');
        closeLink.className = 'CloseSiteAction';
        closeLink.innerHTML = String.format('<a href="javascript:" onclick="Web.DataView._activate(null,\'{0}\',\'SiteAction\')">{1}</a>', elementId, Web.DataViewResources.ModalPopup.Close);
        elem.insertBefore(closeLink, elem.childNodes[0]);
    }
    if (Sys.UI.DomElement.getVisible(elem)) {
        var bounds = $common.getBounds(elem);
        var clientBounds = $common.getClientBounds();
        var scrolling = $common.getScrolling();
        if (bounds.y < scrolling.y || bounds.y > scrolling.y + clientBounds.height)
            elem.scrollIntoView(false);
    }
    if (source) {
        while (source && !Sys.UI.DomElement.containsCssClass(source, 'Task'))
            source = source.parentNode;
        for (i = 0; i < source.parentNode.childNodes.length; i++) {
            var peer = source.parentNode.childNodes[i];
            if (peer.className)
                Sys.UI.DomElement.removeCssClass(peer, 'Selected');
        }
        Sys.UI.DomElement.addCssClass(source, 'Selected');
    }
    _body_performResize();
}

Web.DataView._partialUpdateBeginRequest = function(sender, args) {
    var r = args.get_request();
    var components = Sys.Application.getComponents();
    var controllers = [];
    for (var i = 0; i < components.length; i++) {
        var c = components[i];
        if (c._controller && c._viewId && Web.DataView.isInstanceOfType(c)) {
            var tag = c.get_tag();
            if (!String.isNullOrEmpty(tag)) {
                Array.add(controllers, tag);
                Array.add(controllers, c.get_selectedKey());
            }
        }
    }
    var s = Sys.Serialization.JavaScriptSerializer.serialize(controllers);
    r.set_body(r.get_body() + '&' + encodeURIComponent('__WEB_DATAVIEWSTATE') + '=' + encodeURIComponent(s));
}

Web.DataView._load = function() {
    if (Web.DataView._loaded) return;
    if (Sys.WebForms && Sys.WebForms.PageRequestManager._instance) Sys.WebForms.PageRequestManager._instance.add_beginRequest(Web.DataView._partialUpdateBeginRequest);
    Web.DataView._loaded = true;
    updateACT();
    var pc = $get('PageContent');
    if (pc) {
        var divs = document.body.getElementsByTagName('div');
        for (var i = 0; i < divs.length; i++) {
            if (divs[i].className.match(/Loading/)) {
                Sys.UI.DomElement.removeCssClass(divs[i], 'Loading');
                break;
            }
        }
        divs = pc.getElementsByTagName('div');
        var layoutContainers = [];
        var rowIndex = 0;
        var hasActivators = false;
        var hasSideBarActivators = false;
        var sb = null;
        var siteActions = [];
        Web.DataView._numberOfContainers = 0;
        for (i = 0; i < divs.length; i++) {
            var div = divs[i];
            var width = div.getAttribute('factory:width');
            var flow = div.getAttribute('factory:flow');
            if (!String.isNullOrEmpty(width) || !String.isNullOrEmpty(flow)) {
                if (flow != 'NewColumn') {
                    div.style.clear = 'left';
                    rowIndex++;
                }
                if (String.isNullOrEmpty(div.id))
                    div.id = "_lc" + layoutContainers.length;
                Sys.UI.DomElement.addCssClass(div, 'LayoutContainer');
                Web.DataView._numberOfContainers++;
                if (width != '100%')
                    div.style.overflow = 'hidden';
                Array.add(layoutContainers, { 'id': div.id, 'width': width, 'rowIndex': rowIndex, 'peersWithoutWidth': 0 });
                var childDivs = div.getElementsByTagName('div');
                var divsWithActivator = [];
                for (var j = 0; j < childDivs.length; j++) {
                    var childDiv = childDivs[j];
                    var activator = childDiv.getAttribute('factory:activator');
                    if (!String.isNullOrEmpty(activator)) {
                        childDiv.id = String.isNullOrEmpty(childDiv.id) ? div.id + '$a' + j : childDiv.id
                        var da = { 'elem': childDiv, 'activator': activator.split('|'), 'id': childDiv.id };
                        da.activator[0] = da.activator[0].trim();
                        if (da.activator.length == 1) da.activator[1] = j.toString();
                        Array.add(divsWithActivator, da);
                    }
                }
                j = 1;
                while (j < divsWithActivator.length) {
                    da = divsWithActivator[j];
                    for (var k = 0; k < j; k++) {
                        var da2 = divsWithActivator[k];
                        if (da2.activator[0] == da.activator[0] & da2.activator[1] == da.activator[1]) {
                            while (da.elem.childNodes.length > 0)
                                da2.elem.appendChild(da.elem.childNodes[0]);
                            delete da.elem;
                            Array.removeAt(divsWithActivator, j);
                            da = null;
                            break;
                        }
                    }
                    if (da) j++;
                }
                if (divsWithActivator.length > 0) {
                    hasActivators = true;
                    var nodes = [];
                    var firstSideBarActivator = true;
                    for (j = 0; j < divsWithActivator.length; j++) {
                        da = divsWithActivator[j];
                        if (da.activator[0] == 'Tab') {
                            if (nodes.length == 0) {
                                var menuBar = document.createElement('div');
                                menuBar.className = 'TabBar';
                                div.insertBefore(menuBar, da.elem);
                            }
                            var n = { 'title': da.activator[1], 'elementId': da.id, 'selected': nodes.length == 0, 'description': da.elem.getAttribute('factory:description'), 'hidden': da.elem.getAttribute('factory:hidden') };
                            Array.add(nodes, n);
                            Sys.UI.DomElement.setVisible(da.elem, n.selected);
                            da.elem._activated = true;
                            Sys.UI.DomElement.addCssClass(da.elem, 'TabBody TabContainer');
                        }
                        else if (da.activator[0] == 'SideBarTask') {
                            if (!sb) {
                                sb = new Sys.StringBuilder();
                                sb.appendFormat('<div class="TaskBox"><div class="Inner"><div class="Header">{0}</div>', Web.DataViewResources.Menu.Tasks);
                            }
                            da.elem._activated = firstSideBarActivator;
                            if (firstSideBarActivator) firstSideBarActivator = false;
                            Sys.UI.DomElement.setVisible(da.elem, da.elem._activated);
                            sb.appendFormat('<div class="Task{1}"{4}><a href="javascript:" onclick="Web.DataView._activate(this,\'{2}\',\'SideBarTask\');return false;" title=\'{3}\'>{0}</a></div>', da.activator[1], !hasSideBarActivators ? ' Selected' : '', da.id, Web.DataView.htmlAttributeEncode(da.elem.getAttribute('factory:description')), da.elem.getAttribute('factory:hidden') == 'true' ? ' style="display:none"' : '');
                            hasSideBarActivators = true;
                        }
                        else if (da.activator[0] == 'SiteAction' && Web.Menu.get_siteActionsFamily()) {
                            var item = new Web.Item(Web.Menu.get_siteActionsFamily(), da.activator[1], da.elem.getAttribute('factory:description'));
                            item.set_cssClass(da.elem.getAttribute('factory:cssClass'));
                            item.set_script('Web.DataView._activate(null,"{0}","SiteAction")', da.id);
                            Array.add(siteActions, item);
                            Sys.UI.DomElement.setVisible(da.elem, false);
                        }
                        else {
                            Sys.UI.DomElement.setVisible(da.elem, false);
                        }
                        da.elem = null;
                    }
                    if (nodes.length > 0) {
                        $create(Web.Menu, { 'id': div.id + '$ActivatorMenu', 'nodes': nodes }, null, null, menuBar);
                        if (nodes.length < 2 && i == 0)
                            Sys.UI.DomElement.addCssClass(menuBar, 'EmptyTabBar');
                    }
                }
            }
        }
        if (hasActivators)
            Web.DataView._performDelayedLoading();
        if (hasSideBarActivators && sb) {
            var sideBar = $getSideBar();
            if (sideBar) {
                sb.append('</div></div>');
                var tasksBox = document.createElement('div');
                tasksBox.innerHTML = sb.toString();
                sb.clear();
                sideBar.insertBefore(tasksBox, sideBar.childNodes[0]);
                sideBar._hasActivators = true;
            }
        }
        if (siteActions.length > 0)
            Web.Menu.set_siteActions(siteActions);
        var ri = rowIndex;
        while (layoutContainers.length > 0 && ri >= layoutContainers[0].rowIndex) {
            var containersWithoutWidth = 0;
            var peerCount = 0;
            for (i = 0; i < layoutContainers.length; i++) {
                lc = layoutContainers[i];
                if (lc.rowIndex == ri) {
                    peerCount++;
                    if (String.isNullOrEmpty(lc.width))
                        containersWithoutWidth++;
                }
            }
            for (i = 0; i < layoutContainers.length; i++) {
                lc = layoutContainers[i];
                if (lc.rowIndex == ri) {
                    lc.peersWithoutWidth = containersWithoutWidth;
                    if (peerCount == 1 && String.isNullOrEmpty(lc.width))
                        lc.width = '100%';
                }
            }
            ri--;
        }
        Web.DataView._layoutContainers = layoutContainers;
        _body_performResize();
        $addHandler(window, 'resize', _body_resize);
        $addHandler(window, 'scroll', _body_scroll);
    }
    Web.DataView._startDelayedLoading();
    $addHandler(document.body, 'keydown', _body_keydown);
}

Web.DataView._unload = function() {
    if (Web.DataView._delayedLoadingTimer)
        window.clearInterval(Web.DataView._delayedLoadingTimer);
    if ($get('PageContent')) {
        $removeHandler(window, 'resize', _body_resize);
        $removeHandler(window, 'scroll', _body_scroll);
    }
    $removeHandler(document.body, 'keydown', _body_keydown);
}

Web.DataView._startDelayedLoading = function() {
    if (Web.DataView._delayedLoadingViews.length > 0 && !Web.DataView._delayedLoadingTimer)
        Web.DataView._delayedLoadingTimer = window.setInterval('Web.DataView._performDelayedLoading()', 1000);
}

Web.DataView._updateBatchSelectStatus = function(cb, isForm) {
    var targetClass = isForm ? 'Item' : 'Cell';
    var elem = cb.parentNode;
    while (elem != null && !Sys.UI.DomElement.containsCssClass(elem, targetClass)) elem = elem.parentNode;
    if (elem) {
        if (cb.checked)
            Sys.UI.DomElement.addCssClass(elem, 'BatchEditFrame');
        else
            Sys.UI.DomElement.removeCssClass(elem, 'BatchEditFrame');
    }
}

Web.DataView.DetailsHandler = 'Details.aspx';
Web.DataView.LocationRegex = /^(_.+?):(.+)$/;
Web.DataView.LEVs = [];

Sys.Application.add_load(Web.DataView._load);
Sys.Application.add_unload(Web.DataView._unload);

function $createDataView(placeholderId, controller, args) {
    var params = { 'id': controller + 'Extender', 'controller': controller, 'baseUrl': './', 'servicePath': 'Services/DataControllerService.asmx' }
    if (args) {
        for (var i = 0; i < args.length; i++)
            params[args[i].name] = args[i].value;
    }
    $create(Web.DataView, params, null, null, $get(placeholderId));
}

function updateACT() {
    if (Sys.Extended && typeof (AjaxControlToolkit) == "undefined") AjaxControlToolkit = Sys.Extended.UI;
    var c = Sys.CultureInfo.CurrentCulture.dateTimeFormat;
    Web.DataView.dateFormatStrings = {
        '{0:g}': c.ShortDatePattern + ' ' + c.ShortTimePattern,
        '{0:G}': c.ShortDatePattern + ' ' + c.LongTimePattern,
        '{0:f}': c.LongDatePattern + ' ' + c.ShortTimePattern,
        '{0:u}': c.SortableDateTimePattern,
        '{0:U}': c.UniversalSortableDateTimePattern
    }
    Sys.Browser.WebKit = {};
    if (navigator.userAgent.indexOf('WebKit/') > -1) {
        Sys.Browser.agent = Sys.Browser.WebKit;
        Sys.Browser.version = parseFloat(navigator.userAgent.match(/WebKit\/(\d+(\.\d+)?)/)[1]);
        Sys.Browser.name = 'WebKit';
    }
    $common.getScrolling = function() {
        var x = 0;
        var y = 0;
        if (window.pageYOffset) {
            y = window.pageYOffset;
            x = window.pageXOffset;
        } else if (document.body && (document.body.scrollLeft || document.body.scrollTop)) {
            y = document.body.scrollTop;
            x = document.body.scrollLeft;
        } else if (document.documentElement && (document.documentElement.scrollLeft || document.documentElement.scrollTop)) {
            y = document.documentElement.scrollTop;
            x = document.documentElement.scrollLeft;
        }
        return new Sys.UI.Point(x, y);
    }
    if (!(Sys.Browser.agent == Sys.Browser.InternetExplorer || Sys.Browser.agent == Sys.Browser.Firefox || Sys.Browser.agent == Sys.Browser.Opera)) {
        $common.old_getBounds = $common.getBounds;
        $common.getBounds = function(element) {
            var bounds = $common.old_getBounds(element);
            var scrolling = $common.getScrolling();
            if (scrolling.y || scrolling.x) {
                bounds.x += scrolling.x;
                bounds.y += scrolling.y;
            }
            return bounds;
        }
    }
    if (AjaxControlToolkit.CalendarBehavior && !AjaxControlToolkit.CalendarBehavior.prototype.old_show) {
        AjaxControlToolkit.CalendarBehavior.prototype.old_show = AjaxControlToolkit.CalendarBehavior.prototype.show;
        AjaxControlToolkit.CalendarBehavior.prototype.show = function() {
            this.old_show();
            this._container.style.zIndex = 100100;
            var clientBounds = $common.getClientBounds();
            var scrolling = $common.getScrolling();
            var popupDivBounds = $common.getBounds(this._popupDiv);
            if (scrolling.y + clientBounds.height < popupDivBounds.y + popupDivBounds.height) {
                var bounds = $common.getBounds(this._element);
                this._popupDiv.style.top = bounds.y - popupDivBounds.height + 3;
            }
            if (scrolling.x + clientBounds.width < popupDivBounds.x + popupDivBounds.width) {
                bounds = $common.getBounds(this._element);
                this._popupDiv.style.left = scrolling.x + clientBounds.width - popupDivBounds.width;
            }
        }
    }
    if (AjaxControlToolkit.TabContainer && !AjaxControlToolkit.TabContainer.prototype.old_set_activeTabIndex) {
        AjaxControlToolkit.TabContainer.prototype.old_set_activeTabIndex = AjaxControlToolkit.TabContainer.prototype.set_activeTabIndex;
        AjaxControlToolkit.TabContainer.prototype.set_activeTabIndex = function(value) {
            var oldActiveTabIndex = this.get_activeTabIndex();
            this.old_set_activeTabIndex(value);
            if (value != oldActiveTabIndex)
                _body_performResize();
        }
    }
    if (AjaxControlToolkit.AutoCompleteBehavior && !AjaxControlToolkit.AutoCompleteBehavior.prototype.old_dispose) {
        AjaxControlToolkit.AutoCompleteBehavior.prototype.old_dispose = AjaxControlToolkit.AutoCompleteBehavior.prototype.dispose;
        AjaxControlToolkit.AutoCompleteBehavior.prototype.dispose = function() {
            this.old_dispose();
            if (this._completionListElement) {
                this._completionListElement.parentNode.removeChild(this._completionListElement);
                delete this._completionListElement;
            }
        }
        AjaxControlToolkit.AutoCompleteBehavior.prototype.old__handleFlyoutFocus = AjaxControlToolkit.AutoCompleteBehavior.prototype._handleFlyoutFocus;
        AjaxControlToolkit.AutoCompleteBehavior.prototype._handleFlyoutFocus = function() {
            if (!this._completionListElement) return;
            this.old__handleFlyoutFocus();
        }
        AjaxControlToolkit.AutoCompleteBehavior.prototype.old_showPopup = AjaxControlToolkit.AutoCompleteBehavior.prototype.showPopup;
        AjaxControlToolkit.AutoCompleteBehavior.prototype.showPopup = function() {
            this.old_showPopup();
            if (Sys.UI.DomElement.getVisible(this._completionListElement)) {
                Sys.UI.DomElement.addCssClass(this._completionListElement, 'CompletionList');
                this._completionListElement.style.width = '';
                var cb = $common.getClientBounds();
                var bounds = $common.getBounds(this._completionListElement);
                if (bounds.width > cb.width / 3) bounds.width = cb.width / 3;
                var elemBounds = $common.getBounds(this._element);
                var borderBox = $common.getBorderBox(this._completionListElement);
                var paddingBox = $common.getPaddingBox(this._completionListElement);
                this._completionListElement.style.width = ((bounds.width > elemBounds.width ? bounds.width : elemBounds.width) - borderBox.horizontal - paddingBox.horizontal) + 'px';
                bounds = $common.getBounds(this._completionListElement);
                if (bounds.x + bounds.width > cb.width)
                    this._completionListElement.style.left = (cb.width - bounds.width) + 'px';
                if (bounds.y < elemBounds.y)
                    this._completionListElement.style.top = (elemBounds.y - bounds.height) + 'px';
            }
        }
    }
}

function $hoverTab(elem, active) {
    while (elem && elem.tagName != 'TD')
        elem = elem.parentNode;
    if (elem) {
        if (active) {
            Sys.UI.DomElement.addCssClass(elem, 'Active');
            elem.focus();
        }
        else
            Sys.UI.DomElement.removeCssClass(elem, 'Active');
    }
}

function $getSideBar() {
    var sideBar = $get('PageContentSideBar');
    if (!sideBar) return null;
    for (var i = 0; i < sideBar.childNodes.length; i++) {
        var n = sideBar.childNodes[i];
        if (n.className == 'SideBarBody') return n;
    }
    return null;
}

function _field_format(v) {
    try { return this.FormatOnClient && this.DataFormatString != null && this.DataFormatString.length > 0 ? String.localeFormat(this.DataFormatString, v) : v.toString(); }
    catch (e) { throw new Error(String.format('\nField: {0}\nData Format String: {1}\n{2}', this.Name, this.DataFormatString, e.message)) }
}

Web.DataView._blankRegex = /^\s*$/;

if (typeof (Sys) !== 'undefined') Sys.Application.notifyScriptLoaded();
