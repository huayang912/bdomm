/// <reference name="MicrosoftAjax.js"/>
/// <reference path="Web.DataViewResources.js"/>
/// <reference path="Web.DataView.js"/>

Type.registerNamespace("Web");

Web.Item = function(family, text, description) {
    this._family = family;
    this._text = text;
    this._description = description;
    this._depth = 0;
}

Web.Item.prototype = {
    get_family: function() {
        return this._family;
    },
    get_text: function() {
        return this._text;
    },
    set_text: function(value) {
        this._text = value;
    },
    get_description: function() {
        return this._description;
    },
    set_description: function(value) {
        this._description = value;
    },
    get_url: function() {
        return this._url;
    },
    set_url: function(value) {
        this._url = value;
    },
    get_script: function() {
        return this._script;
    },
    set_script: function(script, args) {
        this._script = args == null ? script : String._toFormattedString(false, arguments);
    },
    get_cssClass: function() {
        return this._cssClass;
    },
    set_cssClass: function(value) {
        this._cssClass = value;
    },
    get_disabled: function() {
        return this._disabled;
    },
    set_disabled: function(value) {
        this._disabled = value;
    },
    get_group: function() {
        return this._group;
    },
    set_group: function(value) {
        this._group = value;
    },
    get_selected: function() {
        return this._selected;
    },
    set_selected: function(value) {
        this._selected = value;
    },
    get_children: function() {
        return this._children;
    },
    get_confirmation: function() {
        return this._confirmation;
    },
    set_confirmation: function(value) {
        this._confirmation = value;
    },
    get_dynamic: function() {
        return this._dynamic;
    },
    set_dynamic: function(value) {
        this._dynamic = value;
        this._cssClass = value ? 'Dynamic' : '';
        this._disabled = value;
    },
    get_depth: function() {
        return this._depth;
    },
    get_parent: function() {
        return this._parent;
    },
    get_path: function() {
        if (!this._path) {
            var path = this._parent ? this._parent.get_path() + '/' : '';
            path += this._parent ? Array.indexOf(this._parent.get_children(), this) : Array.indexOf(Web.HoverMonitor.Families[this._family].items, this);
            this._path = path;
        }
        return this._path;
    },
    get_id: function() {
        if (!this._id)
            this._id = String.format('HoverMonitor${0}$Item${1}', this._family, this.get_path().replace(/\//g, '_'));
        return this._id;
    },
    addChild: function(item) {
        item._parent = this;
        if (!this._children) {
            this._children = [];
            this._cssClass = 'Parent';
        }
        item._depth = this._depth + 1;
        Array.add(this._children, item);

    },
    dispose: function() {
        this._parent = null;
        if (this._children) {
            for (var i = 0; i < this._children; i++)
                this._children[i].dispose();
            Array.clear(this._children);
            this._children = null;
        }
    },
    findChild: function(path) {
        return Web.Item.find(this._family, path, this._children);
    }
}

Web.Item.find = function(family, path, items) {
    if (String.isNullOrEmpty(path)) return null;
    if (!items) {
        var itemFamily = Web.HoverMonitor.Families[family];
        if (!itemFamily) return null;
        items = itemFamily.items;
    }
    var m = path.match(/^(\d+)(\/(.+?)$|$)/);
    if (!m) return null;
    var index = parseInt(m[1]);
    if (!items || index >= items.length) return null;
    var child = items[index];
    if (m[3] && m[3].length > 0)
        return child.findChild(m[3]);
    else
        return child;
}

Web.Menu = function(element) {
    Web.Menu.initializeBase(this, [element]);
    this.set_orientation(Web.MenuOrientation.Horizontal);
    this.set_cssClass('Menu');
    this.set_hoverStyle(Web.HoverStyle.Auto);
    this.set_itemDescriptionStyle(Web.ItemDescriptionStyle.ToolTip);
    this.set_popupPosition(Web.PopupPosition.Left);
}

Web.Menu.prototype = {
    initialize: function() {
        Web.Menu.callBaseMethod(this, 'initialize');
        // Add custom initialization here
    },
    dispose: function() {
        var nodes = this.get_nodes();
        for (var i = 0; i < nodes.length; i++) {
            var n = nodes[i];
            if (n.children)
                $unregisterItems(this.get_id() + '_' + i);
        }
        Web.Menu.callBaseMethod(this, 'dispose');
    },
    updated: function() {
        Web.Menu.callBaseMethod(this, 'updated');
        var nodes = this.get_nodes();
        if (nodes) {
            var selectedNode = null;
            for (var i = 0; i < nodes.length; i++) {
                var n = nodes[i];
                this._currentNode = n;
                if (n.children) {
                    var family = this.get_id() + '_' + i;
                    var items = [];
                    for (var j = 0; j < n.children.length; j++)
                        Array.add(items, this._createItem(family, n.children[j]));
                    this.set_items(items);
                    $registerItems(family, items, this.get_hoverStyle(), this.get_popupPosition(), this.get_itemDescriptionStyle());
                    if (n.selected) selectedNode = n;
                }
            }
            this._render(nodes);
            if (this.get_showSiteActions()) {
                family = this.get_id() + '_SiteActions';
                items = [];
                if (this._selectedItem) {
                    var peers = this._selectedItem.get_parent() ? this._selectedItem.get_parent().get_children() : Web.HoverMonitor.Families[this._selectedItem.get_family()].items;
                    for (i = 0; i < peers.length; i++) {
                        var peerItem = peers[i];
                        if (!String.isNullOrEmpty(peerItem.get_url()) && peerItem != this._selectedItem) {
                            var item = new Web.Item(family, peerItem.get_text(), peerItem.get_description());
                            item.set_url(peerItem.get_url());
                            item.set_cssClass(peerItem.get_cssClass());
                            Array.add(items, item);
                        }
                    }
                }
                if (items.length == 0) {
                    if (selectedNode && selectedNode.children && !(selectedNode.children.length == 1 && selectedNode.children[0].selected))
                        nodes = selectedNode.children;
                    for (i = 0; i < nodes.length; i++) {
                        n = nodes[i];
                        if (!String.isNullOrEmpty(n.url) && (!n.selected || nodes.length == 1)) {
                            item = new Web.Item(family, n.title, n.description);
                            item.set_url(n.url);
                            item.set_cssClass(n.cssClass);
                            Array.add(items, item);
                        }
                    }
                }
                $registerItems(family, items, Web.HoverStyle.ClickAndStay, Web.PopupPosition.Right, Web.ItemDescriptionStyle.Inline);
                Web.Menu._siteActionsFamily = family;
                var sideBar = $getSideBar();
                if (sideBar && items.length > 0) {
                    var sb = new Sys.StringBuilder();
                    sb.append('<div class="Inner">');
                    sb.appendFormat('<div class="Header">{0}</div>', Web.DataViewResources.Menu.SeeAlso);
                    for (i = 0; i < items.length; i++) {
                        item = items[i];
                        if (!String.isNullOrEmpty(item.get_url()))
                            sb.appendFormat('<div class="Item"><a href="{0}" title="{2}">{1}</a></div>', item.get_url(), item.get_text(), item.get_description());
                        if (i > 5) break;
                    }
                    sb.append('</div>');
                    var seeAlso = document.createElement('div');
                    seeAlso.className = 'TaskBox';
                    seeAlso.innerHTML = sb.toString();
                    sb.clear();
                    sideBar.insertBefore(seeAlso, sideBar.childNodes[sideBar.childNodes.length - 1]);
                }
                var tableOfContents = $get('TableOfContents');
                if (tableOfContents) {
                    sb.append('<div class="TableOfContents">');
                    for (i = 0; i < nodes.length; i++) {
                        n = nodes[i];
                        sb.appendFormat('<div class="Header">', n.get_text());

                    }
                    sb.append('</div>');
                }
            }
        }
    },
    get_nodes: function() {
        if (!this._nodes)
            this._nodes = Web.Menu.Nodes[this.get_id()];
        return this._nodes;
    },
    set_nodes: function(value) {
        this._nodes = value;
    },
    get_orientation: function() {
        return this._orientation;
    },
    set_orientation: function(value) {
        this._orientation = value;
    },
    get_cssClass: function() {
        return this._cssClass;
    },
    set_cssClass: function(value) {
        this._cssClass = value;
    },
    get_hoverStyle: function() {
        return this._hoverStyle;
    },
    set_hoverStyle: function(value) {
        this._hoverStyle = value;
    },
    get_itemDescriptionStyle: function() {
        return this._itemDescriptionStyle;
    },
    set_itemDescriptionStyle: function(value) {
        this._itemDescriptionStyle = value;
    },
    get_popupPosition: function() {
        return this._popupPosition;
    },
    set_popupPosition: function(value) {
        this._popupPosition = value;
    },
    get_showSiteActions: function() {
        return this._showSiteActions;
    },
    set_showSiteActions: function(value) {
        this._showSiteActions = value;
    },
    get_items: function() {
        return this._items;
    },
    set_items: function(value) {
        this._items = value;
    },
    _createItem: function(family, node, parentItem) {
        var item = new Web.Item(family, node.title, node.description);
        item.set_url(node.url);
        if (node.selected) {
            item.set_selected(node.selected);
            this._currentNode.selected = true;
            this._selectedItem = item;
        }
        if (parentItem)
            parentItem.addChild(item);
        if (node.children) {
            for (var i = 0; i < node.children.length; i++)
                this._createItem(family, node.children[i], item);
        }
        return item;
    },
    _render: function(nodes) {
        var sb = new Sys.StringBuilder();
        sb.appendFormat('<table class="{0}" cellpadding="0" cellspacing="0" style="float:left">', this.get_cssClass());
        if (this.get_orientation() == Web.MenuOrientation.Horizontal)
            sb.append('<tr>');
        for (var i = 0; i < nodes.length; i++) {
            var n = nodes[i];
            if (this.get_orientation() == Web.MenuOrientation.Vertical)
                sb.append('<tr>');
            sb.appendFormat('<td class="Item{3}" onmouseover="$showHover(this,&quot;{0}_{1}&quot;,&quot;{4}&quot;)" onmouseout="$hideHover(this)" onclick="if(this._skip)this._skip=false;else $find(&quot;{0}&quot).select({1},this);"{7}><span class="Outer"><span class="Inner"><span class="Link"><a href="javascript:" onclick="this.parentNode.parentNode.parentNode.parentNode._skip=true;$hoverOver(this, 4);$find(&quot;{0}&quot).select({1},this);return false;" onfocus="$showHover(this,&quot;{0}_{1}&quot;,&quot;{4}&quot;, 4)" onblur="$hideHover(this)" tabIndex="{6}" {5}>{2}</a></span></span></span></td>',
               this.get_id(), i, Web.DataView.htmlEncode(n.title), (n.children ? ' Parent' : '') + (n.selected ? ' Selected' : ''), this.get_cssClass(), String.isNullOrEmpty(n.description) || this.get_itemDescriptionStyle() == Web.ItemDescriptionStyle.None ? '' : String.format(' title="{0}"', Web.DataView.htmlAttributeEncode(n.description)), $nextTabIndex(), n.hidden=='true' ? ' style="display:none"' : '');
            if (this.get_orientation() == Web.MenuOrientation.Vertical)
                sb.append('</tr>');
        }
        if (this.get_orientation() == Web.MenuOrientation.Horizontal)
            sb.append('</tr>');
        sb.append('</table>');
        if (this.get_showSiteActions()) {
            sb.appendFormat('<table class="Menu SiteActions" cellpadding="0" cellspacing="0" style="float:right"><tr><td class="Item Parent" onmouseover="$showHover(this,&quot;{0}_SiteActions&quot;,&quot;SiteActions&quot;)" onmouseout="$hideHover(this)" onclick="if(this._skip)this._skip=false;else $toggleHover()"><span class="Outer"><span class="Inner"><span class="Link"><a href="javascript:" onclick="this.parentNode.parentNode.parentNode.parentNode._skip=true;$toggleHover();$hoverOver(this, 4);return false" onfocus="$showHover(this,&quot;{0}_SiteActions&quot;,&quot;SiteActions&quot;, 4)" onblur="$hideHover(this)" tabIndex="{1}">{2}</a></span></span></span></td></tr></table>',
                this.get_id(), $nextTabIndex(), Web.DataViewResources.Menu.SiteActions);
            var siteActions = document.createElement('div');
            this._element.parentNode.appendChild(siteActions);
        }
        this._element.innerHTML = sb.toString();
        sb.clear();
    },
    select: function(index, source) {
        _body_createPageContext();
        var n = this.get_nodes()[index];
        if (!String.isNullOrEmpty(n.elementId)) {
            var elem = $get(n.elementId);
            for (var i = 0; i < this.get_nodes().length; i++) {
                n = this.get_nodes()[i];
                if (!String.isNullOrEmpty(n.elementId))
                    Sys.UI.DomElement.setVisible($get(n.elementId), false);
            }
            Sys.UI.DomElement.setVisible(elem, true);
            while (!Sys.UI.DomElement.containsCssClass(source, 'Item'))
                source = source.parentNode;
            for (i = 0; i < source.parentNode.childNodes.length; i++)
                Sys.UI.DomElement.removeCssClass(source.parentNode.childNodes[i], 'Selected');
            Sys.UI.DomElement.addCssClass(source, 'Selected');
            _body_performResize();
            source.focus();
        }
        else
            Web.Menu.navigate(n.url);
    }
}

Web.Menu.Nodes = [];

Web.Menu.get_siteActionsFamily = function() {
    return Web.Menu._siteActionsFamily;
}

Web.Menu.get_siteActions = function() {
    return Web.HoverMonitor.Families[this._siteActionsFamily].items
}
Web.Menu.set_siteActions = function(items) {
    Web.HoverMonitor.Families[this._siteActionsFamily].items = items;
}
Web.Menu.navigate = function(url) {
    if (String.isNullOrEmpty(url)) return;
    var m = url.match(/^(_.+?):(.+?)$/);
    if (m)
        window.open(m[2], m[1]);
    else
        location.href = url;
}


Web.Menu.registerClass('Web.Menu', Sys.UI.Behavior);

Web.HoverStyle = {
    'None': 0,
    'Auto': 1,
    'Click': 2,
    'ClickAndStay': 3
}

Web.PopupPosition = {
    'Left': 0,
    'Right': 1
}

Web.MenuOrientation = {
    'Horizontal': 0,
    'Vertical': 1
}

Web.ItemDescriptionStyle = {
    'None': 0,
    'Inline': 1,
    'ToolTip': 2
}

Web.HoverMonitor = function() {
    Web.HoverMonitor.initializeBase(this);
    this._popups = [];
}

Web.HoverMonitor.prototype = {
    initialize: function() {
        Web.HoverMonitor.callBaseMethod(this, 'initialize');
        this._hoverArrowHandlers = {
            'click': this._hoverArrow_click,
            'mouseover': this._hoverArrow_mouseover,
            'mouseout': this._hoverArrow_mouseout
        }
        this._hoverMenuHandlers = {
            'mouseover': this._hoverMenu_mouseover,
            'mouseout': this._hoverMenu_mouseout
        }
        this._documentKeydownHandler = Function.createDelegate(this, this._document_keydown);
        this._documentClickHandler = Function.createDelegate(this, this._document_click);
        $addHandler(document, 'keydown', this._documentKeydownHandler);
        $addHandler(document, 'click', this._documentClickHandler);
        this._tabIndex = 1;
    },
    dispose: function() {
        delete this._factory;
        for (var i = 0; i < Web.HoverMonitor.Families.length; i++)
            $unregisterItems(Web.HoverMonitor.Families[i].family);
        $removeHandler(document, 'click', this._documentClickHandler);
        $removeHandler(document, 'keydown', this._documentKeydownHanlder);
        this.stopClose();
        this.stopOpen();
        for (i = 0; i < this._popups.length; i++)
            this._destroyPopup(i);
        this._destroyFrame(this._hoverFrame);
        this._destroyFrame(this._openFrame);
        Web.HoverMonitor.callBaseMethod(this, 'dispose');
    },
    nextTabIndex: function() {
        return this._tabIndex++;
    },
    _createPopup: function(level) {
        if (!this._popups[level]) {
            var container = document.createElement('div');
            document.body.appendChild(container);
            container.className = 'HoverMenu';
            Sys.UI.DomElement.setVisible(container, false);
            var behavior = $create(AjaxControlToolkit.PopupBehavior, { 'id': 'HoverMonitorPopup' + level }, null, null, container);
            var popup = { 'behavior': behavior, 'container': container };
            this._popups[level] = popup;
            $addHandlers(container, this._hoverMenuHandlers, this);
        }
        return this._popups[level];
    },
    _destroyPopup: function(level) {
        var popup = this._popups[level];
        if (!popup) return;
        popup.behavior.dispose();
        $clearHandlers(popup.container);
        popup.container.parentNode.removeChild(popup.container);
        //document.body.removeChild(popup.container);
        delete popup.container;
        delete this._popups[level]
    },
    _renderPopup: function(popup, family, item, cssClass) {
        var sb = new Sys.StringBuilder();
        popup.container.style.width = '';
        popup.container.style.height = '';
        popup.container.style.overflow = '';
        popup.container.className = String.format('HoverMenu {0}_HoverMenu', cssClass);
        // render items
        var hasDescriptions = false;
        var itemDescriptionStyle = this.get_itemDescriptionStyle(family);
        var f = Web.HoverMonitor.Families[family];
        if (f) {
            var items = item ? item.get_children() : f.items;
            var currentGroup = null;
            for (var i = 0; i < items.length; i++) {
                item = items[i];
                if (item.get_group() != currentGroup) {
                    if (currentGroup != null)
                        sb.append('</div>');
                    currentGroup = item.get_group();
                    if (currentGroup != null)
                        sb.append('<div class="Group">');
                }
                if (item.get_text()) {
                    sb.append('<a href="javascript:" class="Item');
                    if (item.get_cssClass())
                        sb.append(' ' + item.get_cssClass());
                    if (item.get_selected())
                        sb.append(' Selected');
                    if (item.get_disabled())
                        sb.appendFormat(' {0}Disabled Disabled', item.get_cssClass());
                    sb.append('"');
                    if (item.get_disabled())
                        sb.append(' onclick="return false"');
                    else {
                        path = item.get_path();
                        sb.appendFormat(' onclick="$selectItem(&quot;{0}&quot;,&quot;{1}&quot);return false"', family, path);
                        sb.appendFormat(' onmouseover="$activateItem(&quot;{0}&quot;,&quot;{1}&quot;)" onmouseout="$deactivateItem(&quot;{0}&quot;,&quot;{1}&quot;)"', family, path);
                    }
                    var description = itemDescriptionStyle == Web.ItemDescriptionStyle.None ? null : item.get_description();
                    if (!item.get_children() && description != null && itemDescriptionStyle == Web.ItemDescriptionStyle.ToolTip)
                        sb.appendFormat(' title="{0}"', Web.DataView.htmlAttributeEncode(description));
                    sb.appendFormat(' id="{0}"', item.get_id());
                    sb.append('>');
                    if (!String.isNullOrEmpty(description) && itemDescriptionStyle == Web.ItemDescriptionStyle.Inline) {
                        hasDescriptions = true;
                        sb.appendFormat('<span class="Text">{0}</span><span class="Description">{1}</span>', item.get_text(), description);
                    }
                    else
                        sb.append(Web.DataView.htmlEncode(item.get_text()));
                    sb.append('</a>');
                    if (item.get_dynamic())
                        window.setTimeout(item.get_script(), 50);

                }
                else
                    sb.append('<div class="Break"></div>');

            }
            if (currentGroup != null)
                sb.append('</div>');
        }
        else
            return null;
        // return popup to the caller
        var factory = this._factory;
        if (!factory)
            factory = this._factory = document.createElement('div');
        factory.innerHTML = sb.toString();
        popup.container.innerHTML = '';
        i = 0;
        while (factory.childNodes.length > 0) {
            var node = factory.childNodes[0];
            factory.removeChild(node);
            popup.container.appendChild(node);
        }
        sb.clear();
        if (hasDescriptions)
            popup.container.className = popup.container.className + ' ' + popup.container.className.replace(/(\w+)/g, '$1Ex');
        return popup;
    },
    _createFrame: function(zIndex) {
        var frame = { 'top': document.createElement('div'), 'right': document.createElement('div'), 'bottom': document.createElement('div'), 'left': document.createElement('div'), 'arrow': document.createElement('div') };
        frame.top.style.position = 'absolute';
        frame.top.style.display = 'none';
        frame.top.style.zIndex = zIndex;
        frame.top.className = 'HoverMonitor_TopLine';
        document.body.appendChild(frame.top);
        frame.right.style.position = 'absolute';
        frame.right.style.display = 'none';
        frame.right.style.zIndex = zIndex;
        frame.right.className = 'HoverMonitor_RightLine';
        document.body.appendChild(frame.right);
        frame.bottom.style.position = 'absolute';
        frame.bottom.style.display = 'none';
        frame.bottom.style.zIndex = zIndex;
        frame.bottom.className = 'HoverMonitor_BottomLine';
        document.body.appendChild(frame.bottom);
        frame.left.style.position = 'absolute';
        frame.left.style.display = 'none';
        frame.left.style.zIndex = zIndex;
        frame.left.className = 'HoverMonitor_LeftLine';
        document.body.appendChild(frame.left);
        frame.arrow.style.position = 'absolute';
        frame.arrow.style.display = 'none';
        frame.arrow.style.zIndex = zIndex;
        frame.arrow.className = 'HoverMonitor_Arrow';
        document.body.appendChild(frame.arrow);
        frame.arrow.innerHTML = String.format('<div style="z-index:{0}"></div>', zIndex + 1);
        return frame;
    },
    _destroyFrame: function(f) {
        if (!f) return;
        document.body.removeChild(f.top);
        delete f.top;
        document.body.removeChild(f.right);
        delete f.right;
        document.body.removeChild(f.bottom);
        delete f.bottom;
        document.body.removeChild(f.left);
        delete f.left;
        $clearHandlers(f.arrow);
        document.body.removeChild(f.arrow);
        delete f.arrow;
        delete f.element;
    },
    get_hoverFrame: function() {
        if (!this._hoverFrame) {
            this._hoverFrame = this._createFrame(Web.HoverMonitor.HoverFrameZIndex);
            $addHandlers(this._hoverFrame.arrow, this._hoverArrowHandlers, this);
        }
        return this._hoverFrame;
    },
    get_openFrame: function() {
        if (!this._openFrame)
            this._openFrame = this._createFrame(Web.HoverMonitor.OpenFrameZIndex);
        return this._openFrame;
    },
    get_hoverItems: function(frame) {
        var family = Web.HoverMonitor.Families[frame.family];
        return family ? family.items : null;

    },
    get_hoverStyle: function(frame) {
        var family = Web.HoverMonitor.Families[frame.family];
        return family ? family.style : Web.HoverStyle.None;
    },
    get_PopupPosition: function(frame) {
        var family = Web.HoverMonitor.Families[frame.family];
        return family ? family.position : Web.HoverStyle.Left;
    },
    get_itemDescriptionStyle: function(family) {
        var f = Web.HoverMonitor.Families[family];
        return f ? f.itemDescriptionStyle : Web.HoverStyle.Inline;
    },
    get_hoverBounds: function(frame) {
        var bounds = $common.getBounds(frame.element);
        //        if (Sys.Browser.agent == Sys.Browser.InternetExplorer && Sys.Browser.version > 7) {
        //            bounds.x += 2;
        //            bounds.y += 2;
        //        }
        //        else if (Sys.Browser.agent != Sys.Browser.Firefox) {
        //            bounds.width += 1;
        //        }
        return bounds;
    },
    deactivate: function(family, path) {
        var item = Web.Item.find(family, path);
        if (item) {
            var elem = $get(item.get_id());
            Sys.UI.DomElement.removeCssClass(elem, 'Active')
        }
    },
    get_activeFamily: function() {
        return this._activeFamily;
    },
    get_activePath: function() {
        return this._activePath;
    },
    get_activeItem: function() {
        return Web.Item.find(this.get_activeFamily(), this.get_activePath());
    },
    get_hoverFamily: function() {
        var family = this.get_hoverFrame().family;
        return Web.HoverMonitor.Families[family];
    },
    get_openFamily: function() {
        var family = this.get_openFrame().family;
        return Web.HoverMonitor.Families[family];
    },
    get_isVisible: function() {
        return this.get_hoverFrame().visible;
    },
    get_isOpen: function() {
        return this.get_openFrame()._open;
    },
    showFrame: function(elem, family, cssClass, frame) {
        //        if (frame.visible)
        //            this.hideFrame(frame);
        frame.cssClass = cssClass;
        frame.family = family;
        frame.element = elem;
        Sys.UI.DomElement.addCssClass(elem, frame.cssClass + '_Hover');
        var bounds = this.get_hoverBounds(frame);
        Sys.UI.DomElement.setVisible(frame.top, true);
        frame.top.style.width = bounds.width + 'px';
        frame.top.style.left = bounds.x + 'px';
        frame.top.style.top = bounds.y + 'px';
        Sys.UI.DomElement.addCssClass(frame.top, cssClass + '_TopLine');
        // show right line
        Sys.UI.DomElement.setVisible(frame.right, true);
        frame.right.style.left = (bounds.x + bounds.width - 1) + 'px';
        frame.right.style.top = bounds.y + 'px';
        frame.right.style.height = bounds.height + 'px';
        Sys.UI.DomElement.addCssClass(frame.right, cssClass + '_RightLine');
        // show bottom line
        Sys.UI.DomElement.setVisible(frame.bottom, true);
        frame.bottom.style.left = bounds.x + 'px';
        frame.bottom.style.top = (bounds.y + bounds.height - 1) + 'px';
        frame.bottom.style.width = bounds.width + 'px';
        Sys.UI.DomElement.addCssClass(frame.bottom, cssClass + '_BottomLine');
        // show left line
        Sys.UI.DomElement.setVisible(frame.left, true);
        frame.left.style.left = bounds.x + 'px';
        frame.left.style.top = bounds.y + 'px';
        frame.left.style.height = bounds.height + 'px';
        Sys.UI.DomElement.addCssClass(frame.left, cssClass + '_LeftLine');
        // show arrow
        var f = Web.HoverMonitor.Families[frame.family];
        if (f) {
            Sys.UI.DomElement.setVisible(frame.arrow, true);
            Sys.UI.DomElement.addCssClass(frame.arrow, cssClass + '_Arrow');
            var arrowVisible = Sys.UI.DomElement.getVisible(frame.arrow);
            if (!arrowVisible)
                Sys.UI.DomElement.setVisible(frame.arrow, true);
            var arrowBounds = $common.getBorderBox(frame.arrow);
            bounds.width -= arrowBounds.horizontal;
            bounds.height -= arrowBounds.vertical;
            var arrowWidth = frame.arrow.offsetWidth - arrowBounds.horizontal;
            frame.arrow.style.top = (bounds.y + 1) + 'px';
            frame.arrow.style.left = (bounds.x + bounds.width - 1 - arrowWidth) + 'px';
            frame.arrow.style.height = (bounds.height - 2) + 'px';
            if (!arrowVisible)
                Sys.UI.DomElement.setVisible(frame.arrow, false);
        }
        frame.visible = true;
    },
    hideFrame: function(frame) {
        if (!frame.element) return;
        Sys.UI.DomElement.removeCssClass(frame.top, frame.cssClass + '_TopLine');
        Sys.UI.DomElement.setVisible(frame.top, false);
        Sys.UI.DomElement.removeCssClass(frame.right, frame.cssClass + '_RightLine');
        Sys.UI.DomElement.setVisible(frame.right, false);
        Sys.UI.DomElement.removeCssClass(frame.bottom, frame.cssClass + '_BottomLine');
        Sys.UI.DomElement.setVisible(frame.bottom, false);
        Sys.UI.DomElement.removeCssClass(frame.left, frame.cssClass + '_LeftLine');
        Sys.UI.DomElement.setVisible(frame.left, false);
        Sys.UI.DomElement.removeCssClass(frame.arrow, frame.cssClass + '_Arrow');
        frame.arrow.style.width = '';
        Sys.UI.DomElement.setVisible(frame.arrow, false);
        // clean up
        Sys.UI.DomElement.removeCssClass(frame.element, frame.cssClass + '_Hover');
        frame.visible = false;
    },
    hidePopups: function(depth) {
        if (depth == null) depth = 0;
        for (var i = depth; i < this._popups.length; i++) {
            var p = this._popups[i];
            if (p) p.behavior.hide();
        }
        if (depth == 0) {
            var openFrame = this.get_openFrame();
            openFrame._open = false;
            openFrame.family = null;
            openFrame.cssClass = null;
            delete openFrame.element;
            this._activeFamily = null;
            this._activePath = null;
        }
    },
    showPopup: function(frame, item) {
        this.stopOpen();
        var depth = item ? item.get_depth() + 1 : 0;
        this.hidePopups(depth);
        var clientBounds = $common.getClientBounds();
        if (!$common.getScrolling()) return;
        var scrolling = $common.getScrolling();
        var popup = this._renderPopup(this._createPopup(depth), frame.family, item, frame.cssClass);
        if (!popup) return;
        var itemElement = item ? $get(item.get_id()) : null;
        var hoverBounds = itemElement ? $common.getBounds(itemElement) : this.get_hoverBounds(frame);
        if (item) {
            hoverBounds.x = hoverBounds.x + hoverBounds.width + 2;
            hoverBounds.y = hoverBounds.y - (hoverBounds.height - 2);
        }
        popup.behavior.set_x(hoverBounds.x);
        popup.behavior.set_y(hoverBounds.y + hoverBounds.height);
        popup.behavior.show();
        if (itemElement) {
            var bounds = $common.getBounds(popup.container);
            var showOnLeft = scrolling.x + clientBounds.width < bounds.x + bounds.width;
            if (!showOnLeft) {
                for (var i = depth - 2; i >= 0; i--) {
                    var peerBounds = $common.getBounds(this._popups[i].container);
                    if ($common.overlaps(bounds, peerBounds)) {
                        showOnLeft = true;
                        break;
                    }
                }
            }
            if (showOnLeft) {
                hoverBounds = $common.getBounds(itemElement);
                hoverBounds.x = hoverBounds.x - bounds.width;
                if (hoverBounds.x >= 0)
                    popup.container.style.left = hoverBounds.x + 'px';
            }
        }
        // adjust the menu position
        if (depth == 0 && Web.PopupPosition.Right == this.get_PopupPosition(frame)) {
            var size = $common.getSize(popup.container);
            popup.container.style.left = (hoverBounds.x + hoverBounds.width - size.width) + 'px';
        }
        // reposition the menu if out of the client bounds
        bounds = $common.getBounds(popup.container);
        if (bounds.x + bounds.width >= scrolling.x + clientBounds.width)
            popup.container.style.left = (scrolling.x + clientBounds.width - bounds.width) + 'px';
        else if (bounds.x < scrolling.x)
            popup.container.style.left = scrolling.x + 'px';
        if (bounds.y + bounds.height >= scrolling.y + clientBounds.height) {
            var needNewY = true;
            var groups = popup.container.getElementsByTagName('div');
            for (i = 0; i < groups.length; i++) {
                var g = groups[i];
                if (g.className == 'Group') {
                    var maxHeight = Math.ceil((scrolling.y + clientBounds.height - hoverBounds.y) / 5 * 4);
                    var gb = $common.getBounds(g);
                    var minHeight = gb.height < 100 ? gb.height : 100;
                    gb.height = maxHeight - (gb.y - bounds.y);
                    if (gb.height < minHeight)
                        gb.height = minHeight;
                    else
                        needNewY = false;
                    if (g.offsetHeight > gb.height) {
                        g._autoScrolling = true;
                        g.style.height = gb.height + 'px';
                        g.style.overflow = 'auto';
                        g.style.overflowX = 'hidden';
                        if (g.offsetWidth < gb.width && window.external) {
                            var n = g.childNodes[g.childNodes.length - 1];
                            var paddingBox = $common.getPaddingBox(n);
                            var borderBox = $common.getBorderBox(n);
                            n.style.width = (gb.width - paddingBox.horizontal - borderBox.horizontal) + 'px';
                        }
                    }
                    bounds = $common.getBounds(popup.container);
                    break;
                }
            }
            groups = null;
            if (needNewY) {
                var y = hoverBounds.y - bounds.height;
                if (y >= 0)
                    popup.container.style.top = y + 'px';
                else {
                    popup.container.style.top = (scrolling.y + clientBounds.height - bounds.height - 2) + 'px';
                    bounds = $common.getBounds(popup.container);
                    if (bounds.y < scrolling.y) {
                        popup.container.style.top = scrolling.y + 'px';
                        if (bounds.height > clientBounds.height) {
                            borderBox = $common.getBorderBox(popup.container);
                            paddingBox = $common.getPaddingBox(popup.container);
                            popup.container.style.height = (clientBounds.height - borderBox.vertical - paddingBox.vertical) + 'px';
                            popup.container.style.overflow = 'auto';
                            popup.container.style.overflowX = 'hidden';
                        }
                    }
                }
                if (!itemElement) {
                    bounds = $common.getBounds(popup.container);
                    if ($common.overlaps(bounds, hoverBounds)) {
                        var spaceOnRight = scrolling.x + clientBounds.width - (hoverBounds.x + hoverBounds.width);
                        if (spaceOnRight >= bounds.width)
                            popup.container.style.left = (hoverBounds.x + hoverBounds.width) + 'px';
                        else {
                            bounds.x = hoverBounds.x - bounds.width;
                            if (bounds.x < 0) bounds.x = 0;
                            popup.container.style.left = bounds.x + 'px';
                        }
                    }
                }
            }
        }
        popup.container.style.zIndex = Web.HoverMonitor.HoverMenuZIndex;
        var openFrame = this.get_openFrame();
        // remember the current hover properties in the open frame
        openFrame.family = frame.family;
        openFrame.cssClass = frame.cssClass;
        openFrame.element = frame.element;
        openFrame._open = true;
    },
    hover: function(elem, family, cssClass, depth) {
        if (depth == null) depth = 0;
        this.stopOpen();
        this.stopClose();
        while (depth-- > 0 && elem) elem = elem.parentNode;
        var frame = this.get_hoverFrame();
        var openStyle = this.get_hoverStyle(this.get_openFrame());
        this.hideFrame(this.get_openFrame());
        this.showFrame(elem, family, cssClass, frame);
        var isOpen = this.get_isOpen();
        if (isOpen) {
            if (this.get_hoverFrame().element != this.get_openFrame().element && openStyle == Web.HoverStyle.Auto || openStyle == Web.HoverStyle.ClickAndStay)
                this.hidePopups();
        }
        if (this._skipOpen) {
            this._skipOpen = false;
            return;
        }
        var style = this.get_hoverStyle(this.get_hoverFrame());
        if (style == Web.HoverStyle.Auto || isOpen && style == Web.HoverStyle.ClickAndStay) {
            this.showPopup(this.get_hoverFrame());
        }
        else {
            if (isOpen) this._showOpenFrame();
        }
    },
    _showOpenFrame: function() {
        var openFrame = this.get_openFrame();
        if (this.get_isOpen()) {
            if (!openFrame.element) openFrame = this.get_hoverFrame();
            this.showFrame(openFrame.element, openFrame.family, openFrame.cssClass, openFrame);
        }
    },
    unhover: function() {
        if (this._skipUnhover)
            this._skipUnhover = false;
        else {
            this.hideFrame(this.get_hoverFrame());
            if (this.get_isOpen()) {
                this.startClose();
                if (this.get_hoverStyle(this.get_openFrame()) != Web.HoverStyle.Click) {
                    this._showOpenFrame();
                }
            }
        }
    },
    select: function(family, path) {
        var item = Web.Item.find(family, path);
        if (item.get_confirmation() && !confirm(item.get_confirmation())) return;
        if (!String.isNullOrEmpty(item.get_script())) {
            eval(item.get_script());
            this.close();
        }
        else if (!String.isNullOrEmpty(item.get_url())) {
            Web.Menu.navigate(item.get_url());
            this.close();
        }
    },
    stopClose: function() {
        if (this._closeInterval)
            window.clearInterval(this._closeInterval);
        this._closeInterval = null;
    },
    startClose: function() {
        this.stopClose();
        this._closeInterval = window.setInterval('Web.HoverMonitor._instance.stopClose();Web.HoverMonitor._instance.close()', 1000);
    },
    stopOpen: function() {
        if (this._openInterval)
            window.clearInterval(this._openInterval);
        this._openInterval = null;
    },
    startOpen: function(family, path) {
        this.stopOpen();
        this.stopClose();
        if (!this._skipStartOpen)
            this._openInterval = window.setInterval(String.format('Web.HoverMonitor._instance.open("{0}", "{1}")', family, path), 500);
        this._skipStartOpen = false;
    },
    open: function(family, path) {
        this.stopOpen();
        var item = Web.Item.find(family, path);
        if (item) {
            this.hidePopups(item.get_depth() + 1);
            if (item == null || !item.get_children()) return;
            this.showPopup(this.get_openFrame(), item);
        }
    },
    toggle: function() {
        if (Web.HoverMonitor._preventToggleHover) {
            Web.HoverMonitor._preventToggleHover = false;
            return;
        }
        if (this.get_isOpen() && this.get_hoverFrame().family == this.get_openFrame().family) {
            this.hideFrame(this.get_openFrame());
            this.hidePopups();
            var frame = this.get_hoverFrame();
            this.showFrame(frame.element, frame.family, frame.cssClass, frame);
        }
        else {
            this.hideFrame(this.get_openFrame());
            this.showPopup(this.get_hoverFrame());
        }
        this.stopClose();
    },
    refresh: function(family) {
        if (this.get_isOpen() && this.get_openFrame().family == family) {
            this.toggle();
            this.toggle();
        }
    },
    _hoverArrow_click: function(e) {
        e.preventDefault();
        e.stopPropagation();
        this.toggle();
    },
    _hoverArrow_mouseover: function(e) {
        var f = this.get_hoverFrame();
        this.showFrame(f.element, f.family, f.cssClass, f);
    },
    _hoverArrow_mouseout: function(e) {
        this.hideFrame(this.get_hoverFrame());
        this._showOpenFrame();
    },
    close: function() {
        this.hideFrame(this.get_openFrame());
        this.hideFrame(this.get_hoverFrame());
        this.hidePopups();
    },
    activate: function(family, path, direction) {
        this.startOpen(family, path);
        this._activeFamily = family;
        this._activePath = path;
        var item = Web.Item.find(family, path);
        var first = true;
        while (item) {
            var elem = $get(item.get_id());
            if (first) {
                first = false;
                if (elem) {
                    var p = elem.parentNode;
                    if (p.className == 'Group') p = p.parentNode;
                    var peers = p.getElementsByTagName('a');
                    for (var i = 0; i < peers.length; i++)
                        Sys.UI.DomElement.removeCssClass(peers[i], 'Active');
                    peers = null;
                }
            }
            if (elem) Sys.UI.DomElement.addCssClass(elem, 'Active')
            if (direction && elem.parentNode._autoScrolling) {
                var parentBounds = $common.getBounds(elem.parentNode);
                var elemBounds = $common.getBounds(elem);
                if (elemBounds.y < parentBounds.y || elemBounds.y + elemBounds.height > parentBounds.y + parentBounds.height - 1)
                    elem.scrollIntoView(!(direction == 'down' || direction == 'end'));
                elem.parentNode.scrollLeft = 0;
            }
            item = item.get_parent();
        }
    },
    _moveToItem: function(item, direction) {
        if (!item) return;
        var peers = item.get_parent() ? item.get_parent().get_children() : Web.HoverMonitor.Families[item.get_family()].items;
        var index = Array.indexOf(peers, item);
        var activeItem = null;
        switch (direction) {
            case 'down':
                index = index < peers.length - 1 ? index + 1 : 0;
                break;
            case 'up':
                index = index > 0 ? index - 1 : peers.length - 1;
                break;
            case 'home':
                index = 0;
                break;
            case 'end':
                index = peers.length - 1;
                break;
        }
        activeItem = peers[index];
        if (activeItem) {
            var originalIndex = index;
            while (String.isNullOrEmpty(activeItem.get_text()) || activeItem.get_disabled()) {
                index += direction == 'down' || direction == 'home' ? 1 : -1
                if (index < 0) index = peers.length;
                else if (index > peers.length - 1) index = 0;
                if (index == originalIndex) {
                    activeItem = null;
                    break;
                }
                else
                    activeItem = peers[index];
            }
        }
        if (activeItem) {
            $activateItem(activeItem.get_family(), activeItem.get_path(), direction);
            this.hidePopups(activeItem.get_depth() + 1);
        }
    },
    _openItem: function(item) {
        if (item && item.get_children()) {
            this.open(item.get_family(), item.get_path());
            item = item.get_children()[0];
            $activateItem(item.get_family(), item.get_path());
        }
    },
    _openFirstItem: function() {
        var openFamily = this.get_openFamily();
        if (openFamily)
            this._moveToItem(openFamily.items[0], 'home');
    },
    _document_keydown: function(e) {
        if (this.get_isOpen()) {
            this.stopOpen();
            Web.DataView._focusedItemIndex = null;
            var activeItem = this.get_activeItem();
            if (!activeItem && (e.keyCode == Sys.UI.Key.down || e.keyCode == Sys.UI.Key.up)) {
                e.preventDefault();
                this._openFirstItem();
            }
            else {
                if (e.keyCode == Sys.UI.Key.tab && !activeItem) {
                    if (this.get_hoverStyle(this.get_hoverFrame()) == Web.HoverStyle.Auto)
                        return;
                    else {
                        e.preventDefault();
                        this._openFirstItem();
                    }
                }
                switch (e.keyCode) {
                    case Sys.UI.Key.enter:
                        if (activeItem) {
                            if (String.isNullOrEmpty(activeItem.get_url()) && activeItem.get_children())
                                this._openItem(activeItem);
                            else
                                this.select(activeItem.get_family(), activeItem.get_path());
                        }
                        break;
                    case Sys.UI.Key.esc:
                        if (activeItem && activeItem.get_parent()) {
                            this.hidePopups(activeItem.get_depth());
                            activeItem = activeItem.get_parent();
                            $activateItem(activeItem.get_family(), activeItem.get_path());
                            this.stopOpen();
                        }
                        else {
                            var hf = this.get_hoverFrame();
                            var element = hf.element;
                            this.close();
                            if (element) {
                                if (element.tagName.toLowerCase() != 'a') {
                                    var links = element.getElementsByTagName('a');
                                    if (links && links.length > 0)
                                        element = links[0];
                                    delete links;
                                }
                                try { element.focus(); } catch (e) { }
                            }
                            this._skipOpen = true;
                            $showHover(hf.element, hf.family, hf.cssClass);
                        }
                        break;
                    case Sys.UI.Key.down:
                        this._moveToItem(activeItem, 'down');
                        break;
                    case Sys.UI.Key.up:
                        this._moveToItem(activeItem, 'up');
                        break;
                    case Sys.UI.Key.tab:
                        this._moveToItem(activeItem, e.shiftKey ? 'up' : 'down');
                        break;
                    case Sys.UI.Key.home:
                        this._moveToItem(activeItem, 'home');
                        break;
                    case Sys.UI.Key.end:
                        this._moveToItem(activeItem, 'end');
                        break;
                    case Sys.UI.Key.right:
                    case Sys.UI.Key.left:
                        this._openItem(activeItem);
                        break;
                    default:
                        return;
                }
                e.preventDefault();
                e.stopPropagation();
            }
        }
        else if (this.get_isVisible()) {
            switch (e.keyCode) {
                case Sys.UI.Key.down:
                case Sys.UI.Key.up:
                    if (this.get_hoverFamily())
                        this.showPopup(this.get_hoverFrame());
                    e.preventDefault();
                    break;
            }
        }
    },
    _document_click: function(e) {
        if (this._openFrame && this._openFrame._open) {
            var elem = e.target;
            while (elem) {
                if (!String.isNullOrEmpty(elem.className) && elem.className.match(/\s+\w+_Hover/)) return;
                elem = elem.parentNode;
            }
            this.close();
        }
    },
    _hoverMenu_mouseover: function(e) {
        this.stopClose();
        if (this.get_isOpen() && this.get_hoverStyle(this.get_openFrame()) == Web.HoverStyle.Click)
            this._showOpenFrame();
    },
    _hoverMenu_mouseout: function(e) {
        this.startClose();
    }
}

Web.HoverMonitor.Families = [];

function $closeHovers() {
    Web.HoverMonitor._instance.close()
}

function $hoverOver(elem, depth) {
    while (depth-- > 0)
        elem = elem.parentNode;
    elem.focus();
    $skipUnhover();
}

function $skipUnhover() {
    Web.HoverMonitor._instance._skipUnhover = true;
}

function $nextTabIndex() {
    return Web.HoverMonitor._instance.nextTabIndex();
}

function $showHover(elem, family, cssClass, depth) {
    Web.HoverMonitor._instance.hover(elem, family, cssClass, depth);
}

function $hideHover(elem) {
    Web.HoverMonitor._instance.unhover();
}

function $selectItem(family, path) {
    Web.HoverMonitor._instance.select(family, path);
}

function $toggleHover() {
    Web.HoverMonitor._instance.toggle();
}

function $preventToggleHover() {
    Web.HoverMonitor._preventToggleHover = true;
}

function $refreshHoverMenu(family) {
    Web.HoverMonitor._instance.refresh(family);
}

function $activateItem(family, path, direction) {
    Web.HoverMonitor._instance.activate(family, path, direction);
}

function $deactivateItem(family, path) {
    Web.HoverMonitor._instance.deactivate(family, path);
}

function $registerItems(family, items, hoverStyle, popupPosition, itemDescriptionStyle) {
    if (hoverStyle == null) hoverStyle = Web.HoverStyle.Auto;
    if (popupPosition == null) popupPosition = Web.PopupPosition.Left;
    if (itemDescriptionStyle == null) itemDescriptionStyle = Web.ItemDescriptionStyle.Inline;
    $unregisterItems(family);
    Web.HoverMonitor.Families[family] = { 'items': items, 'style': hoverStyle, 'position': popupPosition, 'itemDescriptionStyle': itemDescriptionStyle }
}

function $unregisterItems(family) {
    var f = Web.HoverMonitor.Families[family];
    if (f && f.items) {
        for (var i = 0; i < f.items.length; i++)
            f.items[i].dispose();
        Array.clear(f.items);
    }
    delete Web.HoverMonitor.Families[family];
}

Web.HoverMonitor.registerClass('Web.HoverMonitor', Sys.Component);

Web.HoverMonitor._instance = $create(Web.HoverMonitor, { 'id': 'GlobalHoverMonitor' });

Web.HoverMonitor.OpenFrameZIndex = 101001;
Web.HoverMonitor.HoverFrameZIndex = 101002;
Web.HoverMonitor.HoverMenuZIndex = 101003;

if (typeof (Sys) !== 'undefined') Sys.Application.notifyScriptLoaded();
