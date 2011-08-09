Type.registerNamespace("Web");Web.Item=function(c,d,b){var a=this;a._family=c;a._text=d;a._description=b;a._depth=0};Web.Item.prototype={get_family:function(){return this._family},get_text:function(){return this._text},set_text:function(a){this._text=a},get_description:function(){return this._description},set_description:function(a){this._description=a},get_url:function(){return this._url},set_url:function(a){this._url=a},get_script:function(){return this._script},set_script:function(a,b){this._script=b==null?a:String._toFormattedString(false,arguments)},get_cssClass:function(){return this._cssClass},set_cssClass:function(a){this._cssClass=a},get_disabled:function(){return this._disabled},set_disabled:function(a){this._disabled=a},get_group:function(){return this._group},set_group:function(a){this._group=a},get_selected:function(){return this._selected},set_selected:function(a){this._selected=a},get_children:function(){return this._children},get_confirmation:function(){return this._confirmation},set_confirmation:function(a){this._confirmation=a},get_dynamic:function(){return this._dynamic},set_dynamic:function(a){this._dynamic=a;this._cssClass=a?"Dynamic":"";this._disabled=a},get_depth:function(){return this._depth},get_parent:function(){return this._parent},get_path:function(){var a=this;if(!a._path){var b=a._parent?a._parent.get_path()+"/":"";b+=a._parent?Array.indexOf(a._parent.get_children(),a):Array.indexOf(Web.HoverMonitor.Families[a._family].items,a);a._path=b}return a._path},get_id:function(){var a=this;if(!a._id)a._id=String.format("HoverMonitor${0}$Item${1}",a._family,a.get_path().replace(/\//g,"_"));return a._id},get_children:function(){return this._children},addChild:function(b){var a=this;b._parent=a;if(!a._children){a._children=[];a._cssClass="Parent"}b._depth=a._depth+1;Array.add(a._children,b)},dispose:function(){var a=this;a._parent=null;if(a._children){for(var b=0;b<a._children;b++)a._children[b].dispose();Array.clear(a._children);a._children=null}},findChild:function(a){return Web.Item.find(this._family,a,this._children)}};Web.Item.find=function(h,g,a){var c=null;if(String.isNullOrEmpty(g))return c;if(!a){var d=Web.HoverMonitor.Families[h];if(!d)return c;a=d.items}var b=g.match(/^(\d+)(\/(.+?)$|$)/);if(!b)return c;var f=parseInt(b[1]);if(!a||f>=a.length)return c;var e=a[f];return b[3]&&b[3].length>0?e.findChild(b[3]):e};Web.Menu=function(b){var a=this;Web.Menu.initializeBase(a,[b]);a.set_orientation(Web.MenuOrientation.Horizontal);a.set_cssClass("Menu");a.set_hoverStyle(Web.HoverStyle.Auto);a.set_itemDescriptionStyle(Web.ItemDescriptionStyle.ToolTip);a.set_popupPosition(Web.PopupPosition.Left)};Web.Menu.prototype={initialize:function(){Web.Menu.callBaseMethod(this,"initialize")},dispose:function(){for(var b=this.get_nodes(),a=0;a<b.length;a++){var c=b[a];c.children&&$unregisterItems(this.get_id()+"_"+a)}Web.Menu.callBaseMethod(this,"dispose")},updated:function(){var b=this;Web.Menu.callBaseMethod(b,"updated");var f=b.get_nodes();if(!f)return;for(var i=null,a=0;a<f.length;a++){var c=f[a];b._currentNode=c;if(c.children){for(var h=b.get_id()+"_"+a,e=[],o=0;o<c.children.length;o++)Array.add(e,b._createItem(h,c.children[o]));b.set_items(e);$registerItems(h,e,b.get_hoverStyle(),b.get_popupPosition(),b.get_itemDescriptionStyle());if(c.selected)i=c}}b._render(f);if(b.get_showSiteActions()){h=b.get_id()+"_SiteActions";e=[];if(b._selectedItem){var p=b._selectedItem.get_parent()?b._selectedItem.get_parent().get_children():Web.HoverMonitor.Families[b._selectedItem.get_family()].items;for(a=0;a<p.length;a++){var j=p[a];if(!String.isNullOrEmpty(j.get_url())&&j!=b._selectedItem){var d=new Web.Item(h,j.get_text(),j.get_description());d.set_url(j.get_url());d.set_cssClass(j.get_cssClass());Array.add(e,d)}}}if(e.length==0){if(i&&i.children&&!(i.children.length==1&&i.children[0].selected))f=i.children;for(a=0;a<f.length;a++){c=f[a];if(!String.isNullOrEmpty(c.url)&&(!c.selected||f.length==1)){d=new Web.Item(h,c.title,c.description);d.set_url(c.url);d.set_cssClass(c.cssClass);Array.add(e,d)}}}$registerItems(h,e,Web.HoverStyle.ClickAndStay,Web.PopupPosition.Right,Web.ItemDescriptionStyle.Inline);Web.Menu._siteActionsFamily=h;var k=$getSideBar();if(k&&e.length>0){for(a=0;a<k.childNodes.length;a++){var n=k.childNodes[a];if(n.className&&Sys.UI.DomElement.containsCssClass(n,"About")){n.getElementsByTagName("div")[1].innerHTML=Web.DataViewResources.Menu.About;break}}var g=new Sys.StringBuilder;g.append('<div class="Inner">');g.appendFormat('<div class="Header">{0}</div>',Web.DataViewResources.Menu.SeeAlso);for(a=0;a<e.length;a++){d=e[a];!String.isNullOrEmpty(d.get_url())&&g.appendFormat('<div class="Item"><a href="{0}" title="{2}">{1}</a></div>',d.get_url(),String.trimLongWords(d.get_text()),String.trimLongWords(d.get_description()));if(a>5)break}g.append("</div>");var m=document.createElement("div");m.className="TaskBox SeeAlso";m.innerHTML=g.toString();g.clear();k.insertBefore(m,k.childNodes[k.childNodes.length-1])}var q=$get("TableOfContents");if(q){g.append('<div class="TableOfContents">');for(a=0;a<f.length;a++){c=f[a];g.appendFormat('<div class="Header">',c.get_text())}g.append("</div>")}}else{var l=Web.PageState.read(b.get_id());if(l!=null){var r=b._element.getElementsByTagName("td");b.select(l,r[l])}}},get_nodes:function(){var a=this;if(!a._nodes)a._nodes=Web.Menu.Nodes[a.get_id()];return a._nodes},set_nodes:function(a){this._nodes=a},get_orientation:function(){return this._orientation},set_orientation:function(a){this._orientation=a},get_cssClass:function(){return this._cssClass},set_cssClass:function(a){this._cssClass=a},get_hoverStyle:function(){return this._hoverStyle},set_hoverStyle:function(a){this._hoverStyle=a},get_itemDescriptionStyle:function(){return this._itemDescriptionStyle},set_itemDescriptionStyle:function(a){this._itemDescriptionStyle=a},get_popupPosition:function(){return this._popupPosition},set_popupPosition:function(a){this._popupPosition=a},get_showSiteActions:function(){return this._showSiteActions},set_showSiteActions:function(a){this._showSiteActions=a},get_items:function(){return this._items},set_items:function(a){this._items=a},_createItem:function(e,a,d){var b=new Web.Item(e,a.title,a.description);b.set_url(a.url);if(a.selected){b.set_selected(a.selected);this._currentNode.selected=true;this._selectedItem=b}d&&d.addChild(b);if(a.children)for(var c=0;c<a.children.length;c++)this._createItem(e,a.children[c],b);return b},_render:function(e){var a=this,b=new Sys.StringBuilder;b.appendFormat('<table class="{0}" cellpadding="0" cellspacing="0" style="float:left">',a.get_cssClass());a.get_orientation()==Web.MenuOrientation.Horizontal&&b.append("<tr>");for(var d=0;d<e.length;d++){var c=e[d];a.get_orientation()==Web.MenuOrientation.Vertical&&b.append("<tr>");b.appendFormat('<td class="Item{3}" onmouseover="$showHover(this,&quot;{0}_{1}&quot;,&quot;{4}&quot;)" onmouseout="$hideHover(this)" onclick="if(this._skip)this._skip=false;else $find(&quot;{0}&quot).select({1},this);"{7}><span class="Outer"><span class="Inner"><span class="Link"><a href="javascript:" onclick="this.parentNode.parentNode.parentNode.parentNode._skip=true;$hoverOver(this, 4);$find(&quot;{0}&quot).select({1},this);return false;" onfocus="$showHover(this,&quot;{0}_{1}&quot;,&quot;{4}&quot;, 4)" onblur="$hideHover(this)" tabIndex="{6}" {5}>{2}</a></span></span></span></td>',a.get_id(),d,Web.DataView.htmlEncode(c.title),(c.children?" Parent":"")+(c.selected?" Selected":""),a.get_cssClass(),String.isNullOrEmpty(c.description)||a.get_itemDescriptionStyle()==Web.ItemDescriptionStyle.None?"":String.format(' title="{0}"',Web.DataView.htmlAttributeEncode(c.description)),$nextTabIndex(),c.hidden=="true"?' style="display:none"':"");a.get_orientation()==Web.MenuOrientation.Vertical&&b.append("</tr>")}a.get_orientation()==Web.MenuOrientation.Horizontal&&b.append("</tr>");b.append("</table>");if(a.get_showSiteActions()){b.appendFormat('<table class="Menu SiteActions" cellpadding="0" cellspacing="0" style="float:right"><tr><td class="Item Parent" onmouseover="$showHover(this,&quot;{0}_SiteActions&quot;,&quot;SiteActions&quot;)" onmouseout="$hideHover(this)" onclick="if(this._skip)this._skip=false;else $toggleHover()"><span class="Outer"><span class="Inner"><span class="Link"><a href="javascript:" onclick="this.parentNode.parentNode.parentNode.parentNode._skip=true;$toggleHover();$hoverOver(this, 4);return false" onfocus="$showHover(this,&quot;{0}_SiteActions&quot;,&quot;SiteActions&quot;, 4)" onblur="$hideHover(this)" tabIndex="{1}">{2}</a></span></span></span></td></tr></table>',a.get_id(),$nextTabIndex(),Web.DataViewResources.Menu.SiteActions);var f=document.createElement("div");a._element.parentNode.appendChild(f)}a._element.innerHTML=b.toString();b.clear()},select:function(f,a){var e="Selected",d=this;_body_createPageContext();var c=d.get_nodes()[f];if(!String.isNullOrEmpty(c.elementId)){Web.PageState.write(d.get_id(),f);for(var g=$get(c.elementId),b=0;b<d.get_nodes().length;b++){c=d.get_nodes()[b];!String.isNullOrEmpty(c.elementId)&&Sys.UI.DomElement.setVisible($get(c.elementId),false)}Sys.UI.DomElement.setVisible(g,true);while(!Sys.UI.DomElement.containsCssClass(a,"Item"))a=a.parentNode;for(b=0;b<a.parentNode.childNodes.length;b++)Sys.UI.DomElement.removeCssClass(a.parentNode.childNodes[b],e);Sys.UI.DomElement.addCssClass(a,e);_body_performResize();try{a.focus()}catch(h){}}else Web.Menu.navigate(c.url)}};Web.Menu.Nodes=[];Web.Menu.get_siteActionsFamily=function(){return Web.Menu._siteActionsFamily};Web.Menu.get_siteActions=function(){return Web.HoverMonitor.Families[this._siteActionsFamily].items};Web.Menu.set_siteActions=function(a){Web.HoverMonitor.Families[this._siteActionsFamily].items=a};Web.Menu.navigate=function(a){if(String.isNullOrEmpty(a))return;var b=a.match(/^(_.+?):(.+?)$/);if(b)window.open(b[2],b[1]);else{Web.DataView._navigated=true;location.href=a}};Web.Menu.registerClass("Web.Menu",Sys.UI.Behavior);Web.HoverStyle={None:0,Auto:1,Click:2,ClickAndStay:3};Web.PopupPosition={Left:0,Right:1};Web.MenuOrientation={Horizontal:0,Vertical:1};Web.ItemDescriptionStyle={None:0,Inline:1,ToolTip:2};Web.HoverMonitor=function(){Web.HoverMonitor.initializeBase(this);this._popups=[]};Web.HoverMonitor.prototype={initialize:function(){var a=this;Web.HoverMonitor.callBaseMethod(a,"initialize");a._hoverArrowHandlers={click:a._hoverArrow_click,mouseover:a._hoverArrow_mouseover,mouseout:a._hoverArrow_mouseout};a._hoverMenuHandlers={mouseover:a._hoverMenu_mouseover,mouseout:a._hoverMenu_mouseout};a._documentKeydownHandler=Function.createDelegate(a,a._document_keydown);a._documentClickHandler=Function.createDelegate(a,a._document_click);$addHandler(document,"keydown",a._documentKeydownHandler);$addHandler(document,"click",a._documentClickHandler);a._tabIndex=1},dispose:function(){var a=this;delete a._factory;for(var b=0;b<Web.HoverMonitor.Families.length;b++)$unregisterItems(Web.HoverMonitor.Families[b].family);$removeHandler(document,"click",a._documentClickHandler);$removeHandler(document,"keydown",a._documentKeydownHanlder);a.stopClose();a.stopCloseItem();a.stopOpen();for(b=0;b<a._popups.length;b++)a._destroyPopup(b);a._destroyFrame(a._hoverFrame);a._destroyFrame(a._openFrame);Web.HoverMonitor.callBaseMethod(a,"dispose")},nextTabIndex:function(){return this._tabIndex++},_createPopup:function(c){var b=this;if(!b._popups[c]){var a=document.createElement("div");document.body.appendChild(a);a.className="HoverMenu";Sys.UI.DomElement.setVisible(a,false);var d=$create(AjaxControlToolkit.PopupBehavior,{id:"HoverMonitorPopup"+c},null,null,a),e={behavior:d,container:a};b._popups[c]=e;$addHandlers(a,b._hoverMenuHandlers,b)}return b._popups[c]},_destroyPopup:function(b){var a=this._popups[b];if(!a)return;a.behavior.dispose();$clearHandlers(a.container);a.container.parentNode.removeChild(a.container);delete a.container;delete this._popups[b]},_renderPopup:function(c,h,a,q){var k="</div>",d=null,b=new Sys.StringBuilder;c.container.style.width="";c.container.style.height="";c.container.style.overflow="";c.container.className=String.format("HoverMenu {0}_HoverMenu",q);var m=false,j=this.get_itemDescriptionStyle(h),p=Web.HoverMonitor.Families[h];if(p){for(var n=a?a.get_children():p.items,f=d,i=0;i<n.length;i++){a=n[i];if(a.get_group()!=f){f!=d&&b.append(k);f=a.get_group();f!=d&&b.append('<div class="Group">')}if(a.get_text()){var l=a.get_selected();b.append('<a href="javascript:" class="Item');a.get_cssClass()&&b.append(" "+a.get_cssClass());l&&b.append(" Selected");a.get_disabled()&&b.appendFormat(" {0}Disabled Disabled",a.get_cssClass());b.append('"');if(a.get_disabled())b.append(' onclick="return false"');else{path=a.get_path();b.appendFormat(' onclick="$selectItem(&quot;{0}&quot;,&quot;{1}&quot);return false"',h,path);b.appendFormat(' onmouseover="$activateItem(&quot;{0}&quot;,&quot;{1}&quot;)" onmouseout="$deactivateItem(&quot;{0}&quot;,&quot;{1}&quot;)"',h,path)}var g=j==Web.ItemDescriptionStyle.None?d:a.get_description();!a.get_children()&&g!=d&&j==Web.ItemDescriptionStyle.ToolTip&&b.appendFormat(' title="{0}"',Web.DataView.htmlAttributeEncode(g));b.appendFormat(' id="{0}"',a.get_id());b.append(">");l&&a.get_children()&&b.append('<div class="Parent">');if(!String.isNullOrEmpty(g)&&j==Web.ItemDescriptionStyle.Inline){m=true;b.appendFormat('<span class="Text">{0}</span><span class="Description">{1}</span>',a.get_text(),g)}else b.append(Web.DataView.htmlEncode(a.get_text()));l&&a.get_children()&&b.append(k);b.append("</a>");a.get_dynamic()&&window.setTimeout(a.get_script(),50)}else b.append('<div class="Break"></div>')}f!=d&&b.append(k)}else return d;var e=this._factory;if(!e)e=this._factory=document.createElement("div");e.innerHTML=b.toString();c.container.innerHTML="";i=0;while(e.childNodes.length>0){var o=e.childNodes[0];e.removeChild(o);c.container.appendChild(o)}b.clear();if(m)c.container.className=c.container.className+" "+c.container.className.replace(/(\w+)/g,"$1Ex");return c},_createFrame:function(b){var e="none",d="absolute",c="div",a={top:document.createElement(c),right:document.createElement(c),bottom:document.createElement(c),left:document.createElement(c),arrow:document.createElement(c)};a.top.style.position=d;a.top.style.display=e;a.top.style.zIndex=b;a.top.className="HoverMonitor_TopLine";document.body.appendChild(a.top);a.right.style.position=d;a.right.style.display=e;a.right.style.zIndex=b;a.right.className="HoverMonitor_RightLine";document.body.appendChild(a.right);a.bottom.style.position=d;a.bottom.style.display=e;a.bottom.style.zIndex=b;a.bottom.className="HoverMonitor_BottomLine";document.body.appendChild(a.bottom);a.left.style.position=d;a.left.style.display=e;a.left.style.zIndex=b;a.left.className="HoverMonitor_LeftLine";document.body.appendChild(a.left);a.arrow.style.position=d;a.arrow.style.display=e;a.arrow.style.zIndex=b;a.arrow.className="HoverMonitor_Arrow";document.body.appendChild(a.arrow);a.arrow.innerHTML=String.format('<div style="z-index:{0}"></div>',b+1);return a},_destroyFrame:function(a){if(!a)return;document.body.removeChild(a.top);delete a.top;document.body.removeChild(a.right);delete a.right;document.body.removeChild(a.bottom);delete a.bottom;document.body.removeChild(a.left);delete a.left;$clearHandlers(a.arrow);document.body.removeChild(a.arrow);delete a.arrow;delete a.element},get_hoverFrame:function(){var a=this;if(!a._hoverFrame){a._hoverFrame=a._createFrame(Web.HoverMonitor.HoverFrameZIndex);$addHandlers(a._hoverFrame.arrow,a._hoverArrowHandlers,a)}return a._hoverFrame},get_openFrame:function(){var a=this;if(!a._openFrame)a._openFrame=a._createFrame(Web.HoverMonitor.OpenFrameZIndex);return a._openFrame},get_hoverItems:function(b){var a=Web.HoverMonitor.Families[b.family];return a?a.items:null},get_hoverStyle:function(b){var a=Web.HoverMonitor.Families[b.family];return a?a.style:Web.HoverStyle.None},get_PopupPosition:function(b){var a=Web.HoverMonitor.Families[b.family];return a?a.position:Web.HoverStyle.Left},get_itemDescriptionStyle:function(b){var a=Web.HoverMonitor.Families[b];return a?a.itemDescriptionStyle:Web.HoverStyle.Inline},get_hoverBounds:function(b){return $common.getBounds(b.element)},deactivate:function(b,d){var a=Web.Item.find(b,d);if(a){var c=$get(a.get_id());Sys.UI.DomElement.removeCssClass(c,"Active")}},get_activeFamily:function(){return this._activeFamily},get_activePath:function(){return this._activePath},get_activeItem:function(){return Web.Item.find(this.get_activeFamily(),this.get_activePath())},get_hoverFamily:function(){var a=this.get_hoverFrame().family;return Web.HoverMonitor.Families[a]},get_openFamily:function(){var a=this.get_openFrame().family;return Web.HoverMonitor.Families[a]},get_isVisible:function(){return this.get_hoverFrame().visible},get_isOpen:function(){return this.get_openFrame()._open},showFrame:function(h,j,e,a){var c="px",d=true;a.cssClass=e;a.family=j;a.element=h;Sys.UI.DomElement.addCssClass(h,a.cssClass+"_Hover");var b=this.get_hoverBounds(a);Sys.UI.DomElement.setVisible(a.top,d);a.top.style.width=b.width+c;a.top.style.left=b.x+c;a.top.style.top=b.y+c;a.top.className="HoverMonitor_TopLine "+e+"_TopLine";Sys.UI.DomElement.setVisible(a.right,d);a.right.style.left=b.x+b.width-1+c;a.right.style.top=b.y+c;a.right.style.height=b.height+c;a.right.className="HoverMonitor_RightLine "+e+"_RightLine";Sys.UI.DomElement.setVisible(a.bottom,d);a.bottom.style.left=b.x+c;a.bottom.style.top=b.y+b.height-1+c;a.bottom.style.width=b.width+c;a.bottom.className="HoverMonitor_BottomLine "+e+"_BottomLine";Sys.UI.DomElement.setVisible(a.left,d);a.left.style.left=b.x+c;a.left.style.top=b.y+c;a.left.style.height=b.height+c;a.left.className="HoverMonitor_LeftLine "+e+"_LeftLine";var k=Web.HoverMonitor.Families[a.family];if(k){Sys.UI.DomElement.setVisible(a.arrow,d);a.arrow.className="HoverMonitor_Arrow "+e+"_Arrow";var g=Sys.UI.DomElement.getVisible(a.arrow);!g&&Sys.UI.DomElement.setVisible(a.arrow,d);var f=$common.getBorderBox(a.arrow);b.width-=f.horizontal;b.height-=f.vertical;var i=a.arrow.offsetWidth-f.horizontal;a.arrow.style.top=b.y+1+c;a.arrow.style.left=b.x+b.width-1-i+c;a.arrow.style.height=b.height-2+c;!g&&Sys.UI.DomElement.setVisible(a.arrow,false)}a.visible=d},hideFrame:function(a){var b=false;if(!a.element)return;a.top.className="HoverMonitor_TopLine";Sys.UI.DomElement.setVisible(a.top,b);a.right.className="HoverMonitor_RightLine";Sys.UI.DomElement.setVisible(a.right,b);a.bottom.className="HoverMonitor_BottomLine";Sys.UI.DomElement.setVisible(a.bottom,b);a.left.className="HoverMonitor_LeftLine";Sys.UI.DomElement.setVisible(a.left,b);a.arrow.className="HoverMonitor_Arrow";a.arrow.style.width="";Sys.UI.DomElement.setVisible(a.arrow,b);Sys.UI.DomElement.removeCssClass(a.element,a.cssClass+"_Hover");a.visible=b},hidePopups:function(d){var a=null,b=this;if(d==a)d=0;for(var e=d;e<b._popups.length;e++){var f=b._popups[e];f&&f.behavior.hide()}if(d==0){var c=b.get_openFrame();c._open=false;c.family=a;c.cssClass=a;delete c.element;b._activeFamily=a;b._activePath=a}},showPopup:function(j,l){var e="px",n=true,i=this;i.stopOpen();var p=l?l.get_depth()+1:0;i.hidePopups(p);var f=$common.getClientBounds();if(!$common.getScrolling())return;var d=$common.getScrolling(),b=i._renderPopup(i._createPopup(p),j.family,l,j.cssClass);if(!b)return;var m=l?$get(l.get_id()):null,c=m?$common.getBounds(m):i.get_hoverBounds(j);if(l){c.x=c.x+c.width+2;c.y=c.y-(c.height-2)}b.behavior.set_x(c.x);b.behavior.set_y(c.y+c.height);b.behavior.show();if(m){var a=$common.getBounds(b.container),t=d.x+f.width<a.x+a.width;if(!t)for(var k=p-2;k>=0;k--){var s=$common.getBounds(i._popups[k].container),B=s.x<a.x&&s.x+s.width>a.x;if(B){t=n;break}}c=$common.getBounds(m);if(t){c.x=c.x-a.width;if(c.x>=0)b.container.style.left=c.x+e}}if(p==0&&Web.PopupPosition.Right==i.get_PopupPosition(j)){var C=$common.getSize(b.container);b.container.style.left=c.x+c.width-C.width+e}a=$common.getBounds(b.container);if(a.x+a.width>=d.x+f.width)b.container.style.left=d.x+f.width-a.width+e;else if(a.x<d.x)b.container.style.left=d.x+e;if(a.y+a.height>=d.y+f.height){var y=n,v=b.container.getElementsByTagName("div");for(k=0;k<v.length;k++){var h=v[k];if(h.className=="Group"){var A=Math.ceil((d.y+f.height-c.y)/5*4),g=$common.getBounds(h),x=g.height<100?g.height:100;g.height=A-(g.y-a.y);if(g.height<x)g.height=x;else y=false;if(h.offsetHeight>g.height){h._autoScrolling=n;h.style.height=g.height+e;h.style.overflow="auto";h.style.overflowX="hidden";if(h.offsetWidth<g.width&&window.external){var w=h.childNodes[h.childNodes.length-1],r=$common.getPaddingBox(w),u=$common.getBorderBox(w);w.style.width=g.width-r.horizontal-u.horizontal+e}}a=$common.getBounds(b.container);break}}v=null;if(y){var q=c.y+c.height-a.height;if(q<d.y)q=d.y;b.container.style.top=q+e;if(q+a.height>d.y+f.height){b.container.style.top=d.y+f.height-a.height-2+e;a=$common.getBounds(b.container);if(a.y<d.y){b.container.style.top=d.y+e;if(a.height>f.height){u=$common.getBorderBox(b.container);r=$common.getPaddingBox(b.container);b.container.style.height=f.height-u.vertical-r.vertical+e;b.container.style.overflow="auto";b.container.style.overflowX="hidden"}}}if(!m){a=$common.getBounds(b.container);a.y=c.y-a.height;if(a.y<d.y){var z=d.x+f.width-(c.x+c.width);if(z>=a.width)b.container.style.left=c.x+c.width+e;else{a.x=c.x-a.width;if(a.x<0)a.x=0;b.container.style.left=a.x+e}a.y=d.y}b.container.style.top=a.y+e}}}b.container.style.zIndex=Web.HoverMonitor.HoverMenuZIndex;var o=i.get_openFrame();o.family=j.family;o.cssClass=j.cssClass;o.element=j.element;o._open=n},hover:function(b,i,h,d){var a=this;if(d==null)d=0;a.stopOpen();a.stopClose();while(d-->0&&b)b=b.parentNode;var f=a.get_hoverFrame();f.arrow.className="HoverMonitor_Arrow";var e=a.get_hoverStyle(a.get_openFrame());a.hideFrame(a.get_openFrame());a.showFrame(b,i,h,f);var c=a.get_isOpen();if(c)(a.get_hoverFrame().element!=a.get_openFrame().element&&e==Web.HoverStyle.Auto||e==Web.HoverStyle.ClickAndStay)&&a.hidePopups();if(a._skipOpen){a._skipOpen=false;return}var g=a.get_hoverStyle(a.get_hoverFrame());if(g==Web.HoverStyle.Auto||c&&g==Web.HoverStyle.ClickAndStay)a.showPopup(a.get_hoverFrame());else c&&a._showOpenFrame()},_showOpenFrame:function(){var b=this,a=b.get_openFrame();if(b.get_isOpen()){if(!a.element)a=b.get_hoverFrame();b.showFrame(a.element,a.family,a.cssClass,a)}},unhover:function(){var a=this;if(a._skipUnhover)a._skipUnhover=false;else{a.hideFrame(a.get_hoverFrame());if(a.get_isOpen()){a.startClose();a.get_hoverStyle(a.get_openFrame())!=Web.HoverStyle.Click&&a._showOpenFrame()}}},select:function(c,d){try{var b=new Sys.UI.DomEvent(window.event);b.stopPropagation();b.preventDefault()}catch(e){}var a=Web.Item.find(c,d);if(a.get_confirmation()&&!confirm(a.get_confirmation()))return;if(!String.isNullOrEmpty(a.get_script())){eval(a.get_script());this.close()}else if(!String.isNullOrEmpty(a.get_url())){Web.Menu.navigate(a.get_url());this.close()}},stopClose:function(){this._closeInterval&&window.clearInterval(this._closeInterval);this._closeInterval=null},startClose:function(){this.stopClose();this._closeInterval=window.setInterval("var hm=Web.HoverMonitor._instance;hm.stopClose();hm.close()",1e3)},closeItem:function(c,e){var b=this;b.stopCloseItem();var d=c!=null&&e!=null?Web.Item.find(c,e):b.get_activeItem(),a=b.get_activeItem();d&&a&&(d!=a||!a.get_children())&&b.hidePopups(a.get_depth()+1)},stopCloseItem:function(){this._closeItemInterval&&window.clearInterval(this._closeItemInterval);this._closeItemInterval=null},startCloseItem:function(a,b){this.stopCloseItem();this._closeItemInterval=window.setInterval(String.format('Web.HoverMonitor._instance.closeItem("{0}","{1}")',a,b),1e3)},stopOpen:function(){this._openInterval&&window.clearInterval(this._openInterval);this._openInterval=null},startOpen:function(b,c){var a=this;a.stopOpen();a.stopCloseItem();a.stopClose();if(!a._skipStartOpen){a._openInterval=window.setInterval(String.format('Web.HoverMonitor._instance.open("{0}", "{1}")',b,c),Web.HoverMonitor._instance._tempOpenDelay!=null?Web.HoverMonitor._instance._tempOpenDelay:500);Web.HoverMonitor._instance._tempOpenDelay=null}a._skipStartOpen=false},open:function(c,d){var b=this;b.stopOpen();var a=Web.Item.find(c,d);if(a){b.hidePopups(a.get_depth()+1);if(a==null||!a.get_children())return;b.showPopup(b.get_openFrame(),a)}},toggle:function(){var a=this;if(Web.HoverMonitor._preventToggleHover){Web.HoverMonitor._preventToggleHover=false;return}if(a.get_isOpen()&&a.get_hoverFrame().family==a.get_openFrame().family){a.hideFrame(a.get_openFrame());a.hidePopups();var b=a.get_hoverFrame();a.showFrame(b.element,b.family,b.cssClass,b)}else{a.hideFrame(a.get_openFrame());a.showPopup(a.get_hoverFrame())}a.stopClose()},refresh:function(b){var a=this;if(a.get_isOpen()&&a.get_openFrame().family==b){a.toggle();a.toggle()}},_hoverArrow_click:function(a){a.preventDefault();a.stopPropagation();this.toggle()},_hoverArrow_mouseover:function(){var a=this.get_hoverFrame();this.showFrame(a.element,a.family,a.cssClass,a)},_hoverArrow_mouseout:function(){this.hideFrame(this.get_hoverFrame());this._showOpenFrame()},close:function(){var a=this;a.hideFrame(a.get_openFrame());a.hideFrame(a.get_hoverFrame());a.hidePopups()},activate:function(d,e,i){var c=this;c._activeFamily=d;c._activePath=e;var b=Web.Item.find(d,e);if(!b||b.get_children())c.startOpen(d,e);else if(b){c.stopOpen();c.startCloseItem(d,e)}var l=true;while(b){var a=$get(b.get_id());if(l){l=false;if(a){var f=a.parentNode;if(f.className=="Group")f=f.parentNode;for(var j=f.getElementsByTagName("a"),k=0;k<j.length;k++)Sys.UI.DomElement.removeCssClass(j[k],"Active");j=null}}a&&Sys.UI.DomElement.addCssClass(a,"Active");if(i&&a.parentNode._autoScrolling){var g=$common.getBounds(a.parentNode),h=$common.getBounds(a);(h.y<g.y||h.y+h.height>g.y+g.height-1)&&a.scrollIntoView(!(i=="down"||i=="end"));a.parentNode.scrollLeft=0}b=b.get_parent()}},_moveToItem:function(d,e){if(!d)return;var c=d.get_parent()?d.get_parent().get_children():Web.HoverMonitor.Families[d.get_family()].items,a=Array.indexOf(c,d),b=null;switch(e){case"down":a=a<c.length-1?a+1:0;break;case"up":a=a>0?a-1:c.length-1;break;case"home":a=0;break;case"end":a=c.length-1}b=c[a];if(b){var f=a;while(String.isNullOrEmpty(b.get_text())||b.get_disabled()){a+=e=="down"||e=="home"?1:-1;if(a<0)a=c.length;else if(a>c.length-1)a=0;if(a==f){b=null;break}else b=c[a]}}if(b){$activateItem(b.get_family(),b.get_path(),e);!b.get_children()&&this.startCloseItem(b.get_family(),b.get_path())}},_openItem:function(a){if(a&&a.get_children()){this.open(a.get_family(),a.get_path());a=a.get_children()[0];$activateItem(a.get_family(),a.get_path())}},_openFirstItem:function(){var a=this.get_openFamily();a&&this._moveToItem(a.items[0],"home")},_document_keydown:function(c){var a=this;if(a.get_isOpen()){a.stopOpen();Web.DataView._focusedItemIndex=null;var b=a.get_activeItem();if(!b&&(c.keyCode==Sys.UI.Key.down||c.keyCode==Sys.UI.Key.up)){c.preventDefault();a._openFirstItem()}else{if(c.keyCode==Sys.UI.Key.tab&&!b)if(a.get_hoverStyle(a.get_hoverFrame())==Web.HoverStyle.Auto)return;else{c.preventDefault();a._openFirstItem()}switch(c.keyCode){case Sys.UI.Key.enter:if(b)if(String.isNullOrEmpty(b.get_url())&&b.get_children())a._openItem(b);else a.select(b.get_family(),b.get_path());break;case Sys.UI.Key.esc:if(b&&b.get_parent()){a.hidePopups(b.get_depth());b=b.get_parent();$activateItem(b.get_family(),b.get_path());a.stopOpen()}else{var f=a.get_hoverFrame(),d=f.element;a.close();if(d){if(d.tagName.toLowerCase()!="a"){var e=d.getElementsByTagName("a");if(e&&e.length>0)d=e[0];delete e}try{d.focus()}catch(c){}}a._skipOpen=true;$showHover(f.element,f.family,f.cssClass)}break;case Sys.UI.Key.down:a._moveToItem(b,"down");break;case Sys.UI.Key.up:a._moveToItem(b,"up");break;case Sys.UI.Key.tab:a._moveToItem(b,c.shiftKey?"up":"down");break;case Sys.UI.Key.home:a._moveToItem(b,"home");break;case Sys.UI.Key.end:a._moveToItem(b,"end");break;case Sys.UI.Key.right:case Sys.UI.Key.left:a._openItem(b);break;default:return}c.preventDefault();c.stopPropagation()}}else if(a.get_isVisible())switch(c.keyCode){case Sys.UI.Key.down:case Sys.UI.Key.up:a.get_hoverFamily()&&a.showPopup(a.get_hoverFrame());c.preventDefault()}},_document_click:function(b){if(this._openFrame&&this._openFrame._open){var a=b.target;while(a){if(!String.isNullOrEmpty(a.className)&&a.className.match(/\s+\w+_Hover/))return;a=a.parentNode}this.close()}},_hoverMenu_mouseover:function(){var a=this;a.stopClose();a.get_isOpen()&&a.get_hoverStyle(a.get_openFrame())==Web.HoverStyle.Click&&a._showOpenFrame()},_hoverMenu_mouseout:function(){this.startClose()}};Web.HoverMonitor.Families=[];function $closeHovers(){Web.HoverMonitor._instance.close()}function $hoverOver(a,b){while(b-->0)a=a.parentNode;a.focus();$skipUnhover()}function $skipUnhover(){Web.HoverMonitor._instance._skipUnhover=true}function $nextTabIndex(){return Web.HoverMonitor._instance.nextTabIndex()}function $showHover(d,b,a,c){Web.HoverMonitor._instance.hover(d,b,a,c)}function $hideHover(){Web.HoverMonitor._instance.unhover()}function $selectItem(a,b){Web.HoverMonitor._instance.select(a,b)}function $toggleHover(){Web.HoverMonitor._instance.toggle()}function $preventToggleHover(){Web.HoverMonitor._preventToggleHover=true}function $refreshHoverMenu(a){Web.HoverMonitor._instance.refresh(a)}function $activateItem(b,c,a){Web.HoverMonitor._instance.activate(b,c,a)}function $deactivateItem(a,b){Web.HoverMonitor._instance.deactivate(a,b)}function $registerItems(d,e,c,b,a){if(c==null)c=Web.HoverStyle.Auto;if(b==null)b=Web.PopupPosition.Left;if(a==null)a=Web.ItemDescriptionStyle.Inline;$unregisterItems(d);Web.HoverMonitor.Families[d]={items:e,style:c,position:b,itemDescriptionStyle:a}}function $unregisterItems(c){var a=Web.HoverMonitor.Families[c];if(a&&a.items){for(var b=0;b<a.items.length;b++)a.items[b].dispose();Array.clear(a.items)}delete Web.HoverMonitor.Families[c]}Web.HoverMonitor.registerClass("Web.HoverMonitor",Sys.Component);Web.HoverMonitor._instance=$create(Web.HoverMonitor,{id:"GlobalHoverMonitor"});Web.HoverMonitor.OpenFrameZIndex=101001;Web.HoverMonitor.HoverFrameZIndex=101002;Web.HoverMonitor.HoverMenuZIndex=101003;typeof Sys!=="undefined"&&Sys.Application.notifyScriptLoaded()