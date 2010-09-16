/// <reference name="MicrosoftAjax.debug.js" />
/// <reference path="Web.MembershipResources.js" />

Type.registerNamespace("Web");

Web.Membership = function() {
    Web.Membership.initializeBase(this);
    this._membershipBar = null;
    this._intervalId = null;
    Web.Membership._instance = this;
    this._displayLogin = true;
    Web.Membership._instance = this;
}

Web.Membership.prototype = {
    initialize: function() {
        Web.Membership.callBaseMethod(this, 'initialize');
        if (!Web.Membership._instance) Web.Membership._instance = this;
        this._loginButtonClickHandler = Function.createDelegate(this, this._loginButtonClick);
        this._loginDialogMouseOverHandler = Function.createDelegate(this, this._loginDialogMouseOver);
        this._loginDialogMouseOutHandler = Function.createDelegate(this, this._loginDialogMouseOut);
        this._login_CompletedHandler = Function.createDelegate(this, this._login_Completed);
        this._method_FailureHandler = Function.createDelegate(this, this._method_Failure);
        this._textBoxKeyPressHandler = Function.createDelegate(this, this._textBoxKeyPress);
        this._body_keydownDelegate = Function.createDelegate(this, this._body_keydown);
        this._body_mousemoveDelegate = Function.createDelegate(this, this._body_mousemove);
        $addHandler(document.body, 'keydown', this._body_keydownDelegate);
        $addHandler(document.body, 'mousemove', this._body_mousemoveDelegate);
        this._window_beforeUnloadDelegate = Function.createDelegate(this, this._window_beforeUnload);
        $addHandler(window, 'beforeunload', this._window_beforeUnloadDelegate);
    },
    dispose: function() {
        if (this._idleIntervalId) window.clearInterval(this._idleIntervalId);
        this._disposeIdentityResources();
        $removeHandler(window, 'beforeunload', this._window_beforeUnloadDelegate);
        $removeHandler(document.body, 'keydown', this._body_keydownDelegate);
        $removeHandler(document.body, 'mousemove', this._body_mousemoveDelegate);
        if (this._loginButton) $clearHandlers(this._loginButton);
        if (this._loginDialog) $clearHandlers(this._loginDialog);
        if (this._userName) $clearHandlers(this._userName);
        if (this._password) $clearHandlers(this._password);
        delete this._membershipBar;
        delete this._bookmarkBar;
        this._hideHistory();
        Web.Membership.callBaseMethod(this, 'dispose');
    },
    get_membershipBar: function() {
        return this._membershipBar;
    },
    get_isLoginDialogVisible: function() {
        return this._isLoginDialogVisible;
    },
    set_isLoginDialogVisible: function(value) {
        this._isLoginDialogVisible = value;
    },
    get_displayRememberMe: function() {
        return this._displayRememberMe != false;
    },
    set_displayRememberMe: function(value) {
        this._displayRememberMe = value;
    },
    get_rememberMeSet: function() {
        return this._rememberMeSet == true;
    },
    set_rememberMeSet: function(value) {
        this._rememberMeSet = value;
    },
    get_displayLogin: function() {
        return this._displayLogin;
    },
    set_displayLogin: function(value) {
        this._displayLogin = value;
    },
    get_displayPasswordRecovery: function() {
        return this._displayPasswordRecovery != false;
    },
    set_displayPasswordRecovery: function(value) {
        this._displayPasswordRecovery = value;
    },
    get_displaySignUp: function() {
        return this._displaySignUp != false;
    },
    set_displaySignUp: function(value) {
        this._displaySignUp = value;
    },
    get_displayMyAccount: function() {
        return this._displayMyAccount;
    },
    set_displayMyAccount: function(value) {
        this._displayMyAccount = value;
    },
    get_displayHelp: function() {
        return this._displayHelp;
    },
    set_displayHelp: function(value) {
        this._displayHelp = value;
    },
    get_baseUrl: function() {
        return this._baseUrl;
    },
    set_baseUrl: function(value) {
        this._baseUrl = value;
    },
    get_servicePath: function() {
        return this._servicePath;
    },
    set_servicePath: function(value) {
        this._servicePath = value;
    },
    get_welcome: function() {
        return this._welcome;
    },
    set_welcome: function(value) {
        this._welcome = value;
    },
    get_enablePermalinks: function() {
        return this._enablePermalinks == true;
    },
    set_enablePermalinks: function(value) {
        this._enablePermalinks = value;
    },
    get_enableHistory: function() {
        return this._enableHistory == true;
    },
    set_enableHistory: function(value) {
        this._enableHistory = value;
    },
    get_isAuthenticated: function() {
        return this._isAuthenticated;
    },
    set_isAuthenticated: function(value) {
        this._isAuthenticated = value;
    },
    get_commandLine: function() {
        return this._commandLine;
    },
    set_commandLine: function(value) {
        this._commandLine = value;
    },
    showLoginDialog: function() {
        this.hideLoginDialog();
    },
    hideLoginDialog: function() {
        if (this._intevalId) {
            window.clearInterval(this._intervalId);
            this._intervalId = null;
        }
    },
    addPermalink: function(arguments, html) {
        if (!(this.get_enablePermalinks() || this.get_enableHistory())) return;
        var link = location.pathname + '?' + arguments;
        this._savePermalinkParams = { 'link': link, 'html': html };
        window.setTimeout('Web.Membership._instance._savePermalink()', 500);
        if (!Sys.UI.DomElement.getVisible(this._membershipBar)) this._encodePermalink_Success('');
    },
    _savePermalink: function() {
        Sys.Net.WebServiceProxy.invoke(this.get_servicePath(), 'SavePermalink', false, this._savePermalinkParams);
    },
    showPermalink: function() {
        if (!this._savePermalinkParams || String.isNullOrEmpty(this._savePermalinkParams.html)) {
            alert(Web.MembershipResources.Messages.PermalinkUnavailable);
            return;
        }
        Sys.Net.WebServiceProxy.invoke(this.get_servicePath(), 'EncodePermalink', false, { 'link': this._savePermalinkParams.link }, Function.createDelegate(this, this._encodePermalink_Success), Function.createDelegate(this, this._method_Failure));
    },
    showHistory: function() {
        if (!this._historyDiv)
            Sys.Net.WebServiceProxy.invoke(this.get_servicePath(), 'ListAllPermalinks', false, null, Function.createDelegate(this, this._showHistory_Success), Function.createDelegate(this, this._method_Failure));
    },
    _focusHistory: function() {
        var rotateButton = $get(this.get_id() + '_RotateHistory');
        if (rotateButton) rotateButton.focus();
    },
    _renderHistory: function() {
        var cb = $common.getClientBounds();
        var scrolling = $common.getScrolling();
        var sb = new Sys.StringBuilder();
        sb.appendFormat('<div onclick="$find(\'{0}\')._hideHistory()" style="width:{1}px;height:{2}px;">', this.get_id(), cb.width, cb.height);
        var cardCount = this._historyData.length;
        if (cardCount > 10) cardCount = 10;
        var x = 0;
        var y = 0;
        var dx = 50;
        var dy = 30;
        if (cardCount <= 3) {
            dx = 65;
            dy = 30;
        }
        else if (cardCount <= 6) {
            dx = 65;
            dy = 30;
        }
        for (var i = cardCount - 1; i >= 0; i--) {
            var card = this._historyData[i][1];
            sb.appendFormat('<div style="position:absolute;background-color:#333333;"></div><div id="{0}_Card{1}" onclick="$find(\'{0}\')._selectHistory({1});return false;" style="position:absolute;left:{2}px;top:{3}px;" onmouseover="if(this._Activated!=99){{this.style.zIndex=600001;this._Activated=99;var s=this.previousSibling;s.style.zIndex=600000;s.style.backgroundColor=\'black\'}}" onmouseout="this.style.zIndex=\'\';this._Activated=0;var s=this.previousSibling;s.style.zIndex=\'\';s.style.backgroundColor=\'#333333\'">', this.get_id(), i, x, y);
            sb.append(card);
            sb.append('</div>');
            if (i == 0)
                sb.appendFormat('<a id="{0}_RotateHistory" href="javascript:" onclick="$find(\'{0}\')._rotateHistory();return false;"><div id="{0}_RotateButton" title="{3}" style="position:absolute;z-index:600002;left:{1}px;top:{2}px;" class="RotateHistory">&nbsp;</div></a>', this.get_id(), x + 110, y - 15, Web.MembershipResources.Bar.RotateHistory);
            x += dx;
            y += dy;
        }
        sb.append('</div>');
        this._historyDiv.innerHTML = sb.toString();
        var div = $get(this.get_id() + '_Card0');
        var b = $common.getBounds(div);
        dx = Math.floor((cb.width - (b.x + b.width)) / 2);
        dy = this._historyOffsetTop ? this._historyOffsetTop : Math.floor((cb.height - (b.y + b.height)) / 2);
        this._historyOffsetTop = dy;
        div = $get(this.get_id() + '_RotateButton');
        div.style.left = (div.offsetLeft + dx) + 'px';
        div.style.top = (div.offsetTop + dy) + 'px';
        if (cardCount == 1) {
            div.style.width = '1px';
            div.style.height = '1px';
        }
        for (i = 0; i < cardCount; i++) {
            div = $get(this.get_id() + '_Card' + i);
            div.style.left = (div.offsetLeft + dx) + 'px';
            div.style.top = (div.offsetTop + dy) + 'px';
            b = $common.getBounds(div);
            b.x += 10 - scrolling.x;
            b.y += 4 - scrolling.y;
            var shadow = div.previousSibling;
            shadow.style.left = b.x + 'px';
            shadow.style.top = b.y + 'px';
            shadow.style.width = (b.width - 13) + 'px';
            shadow.style.height = (b.height - 0) + 'px';
        }
    },
    _selectHistory: function(i) {
        location.href = this._historyData[i][0];
    },
    _hideHistory: function() {
        if (this._historyModalPopup) {
            this._historyModalPopup.hide();
            if (this._historyDiv) {
                this._historyDiv.parentNode.removeChild(this._historyDiv);
                delete this._historyDiv;
            }
            this._historyModalPopup.dispose();
            delete this._historyModalPopup;
        }
    },
    _rotateHistory: function() {
        var item = this._historyData[0];
        Array.removeAt(this._historyData, 0);
        Array.add(this._historyData, item);
        this._renderHistory();
        this._focusHistory();
    },
    _showHistory_Success: function(result) {
        if (result.length == 0) {
            alert(Web.MembershipResources.Messages.HistoryUnavailable)
            return;
        }
        this._historyData = result;
        var panel = this._historyDiv = document.createElement('div');
        document.body.appendChild(panel);
        panel.id = this.get_id() + '_HistoryPanel';
        panel.className = 'History';
        this._historyOffsetTop = null;
        this._renderHistory();
        this._historyModalPopup = $create(AjaxControlToolkit.ModalPopupBehavior, {
            PopupControlID: panel.id, DropShadow: false, BackgroundCssClass: 'ModalBackground'
        }, null, null, $get(this.get_id() + '_HistoryLink'));
        this._historyModalPopup.show();
        this._focusHistory();
    },
    _encodePermalink_Success: function(result) {
        var input = $get(this.get_id() + '_Permalink');
        var btn = $get(this.get_id() + '_AddToFavorites');
        var btn2 = $get(this.get_id() + '_CancelFavorites');
        var show = !Sys.UI.DomElement.getVisible(input.parentNode);
        Sys.UI.DomElement.setVisible(input.parentNode, show);
        Sys.UI.DomElement.setVisible(btn.parentNode, show);
        Sys.UI.DomElement.setVisible(btn2.parentNode, show);
        Sys.UI.DomElement.setVisible(this._membershipBar, !show);
        if (show) input.value = result;
        input.parentNode.title = this._savePermalinkParams.html.replace(/<div class="Value">/g, ' = ').replace(/(\s*<.+?>\s*)+/g, '\r\r').trim().replace(/&\w+;/g, '').replace(/(\r\r)+/g, '; ').replace(/;\s*=/g, ' = ').replace(/=\s*;/g, ' = ');
    },
    _addToFavorites: function() {
        this._encodePermalink_Success('');
        var permalink = $get(this.get_id() + '_Permalink');
        var title = permalink.parentNode.title;
        var url = permalink.value;
        if (window.sidebar) // firefox
            window.sidebar.addPanel(title, url, "");
        else if (window.opera && window.print) { // opera
            var elem = document.createElement('a');
            elem.setAttribute('href', url);
            elem.setAttribute('title', title);
            elem.setAttribute('rel', 'sidebar');
            elem.click();
        }
        else if (document.all)// ie
            window.external.AddFavorite(url, title);
    },
    changeLoginDialogVisibility: function(visible, delay) {
        if (this._intevalId) {
            window.clearInterval(this._intevalId);
            this._intevalId = null;
        }
        if (delay)
            this._intevalId = window.setInterval(String.format('$find("{0}").changeLoginDialogVisibility({1})', this.get_id(), visible), delay);
        else {
            if (visible != this.get_isLoginDialogVisible()) {
                var loginControls = $get('LoginControlsRow', this._membershipBar);
                if (loginControls) {
                    if (visible)
                        Sys.UI.DomElement.removeCssClass(this._loginDialog, 'LoginDialogCollapsed');
                    else
                        Sys.UI.DomElement.addCssClass(this._loginDialog, 'LoginDialogCollapsed');
                    Sys.UI.DomElement.setVisible(loginControls, visible);
                }
                if (visible && !(document.activeElement && document.activeElement.id == 'Password')) this._userName.focus();
                this.set_isLoginDialogVisible(visible);
            }
        }
    },
    get_authenticationEnabled: function() {
        return this._authenticationEnabled == null || this._authenticationEnabled == true;
    },
    set_authenticationEnabled: function(value) {
        this._authenticationEnabled = value;
    },
    get_idleTimeout: function() {
        return this._idleTimeout == null ? 0 : this._idleTimeout;
    },
    set_idleTimeout: function(value) {
        this._idleTimeout = value;
    },
    idle: function() {
        if (Sys.Services.AuthenticationService.get_isLoggedIn() && (new Date() - this._lastActivity > this.get_idleTimeout()) && !this._protecting) {
            Web.HoverMonitor._instance.close();
            var l = Sys.Application.getComponents();
            for (var i = l.length - 1; i >= 0; i--) {
                var c = l[i];
                if (Web.DataView.isInstanceOfType(c)) {
                    if (c.get_isModal())
                        c.endModalState('Cancel');
                    else if (c.get_lookupField())
                        c.hideLookup();
                }
            }
            this._protecting = true;
            var dialog = this._idleDialog = document.createElement('div');
            dialog.id = this.get_id() + '$IdentityValidation';
            document.body.appendChild(dialog);
            dialog.innerHTML = String.format('<div class="ModalPlaceholder FixedDialog UserIdle"><div class="Text">{1}</div><div class="Buttons"><button onclick="$find(\'{0}\').logout();return false;" >{2}</button></div></div>', this.get_id(), Web.MembershipResources.Bar.UserIdle, Web.MembershipResources.Bar.LoginLink);
            this._identityModalPopup = $create(AjaxControlToolkit.ModalPopupBehavior, { PopupControlID: dialog.id, DropShadow: true, BackgroundCssClass: 'ModalBackground' }, null, null, this._membershipBar.getElementsByTagName('a')[0]);
            this._identityModalPopup.show();
        }
    },
    _disposeIdentityResources: function() {
        if (this._identityModalPopup) {
            this._identityModalPopup.dispose();
            this._idleDialog.parentNode.removeChild(this._idleDialog);
            delete this._idleDialog;
            this._identityModalPopup = null;
        }
    },
    _updateLastActivity: function() {
        this._lastActivity = new Date();
    },
    _body_keydown: function(e) {
        this._updateLastActivity();
        if (this._historyDiv) {
            if (e.keyCode == Sys.UI.Key.esc) {
                e.preventDefault();
                this._hideHistory();
            }
            else if (e.keyCode == Sys.UI.Key.tab) {
                e.preventDefault();
                this._rotateHistory();
            }
            else if (e.keyCode == Sys.UI.Key.enter) {
                e.preventDefault();
                this._selectHistory(0);
            }
        }
    },
    _body_mousemove: function(e) {
        this._updateLastActivity();
    },
    _window_beforeUnload: function(e) {
        if (this._protecting) {
            this._protecting = false;
            this._disposeIdentityResources();
            this.logout();
        }
    },
    updated: function() {
        Web.Membership.callBaseMethod(this, 'updated');
        if (this.get_idleTimeout() > 0)
            this._idleIntervalId = window.setInterval(String.format('$find("{0}").idle();', this.get_id()), 180000);
        this._updateLastActivity();
        var bar = document.createElement('div');
        bar.className = 'MembershipBarPlaceholder';
        document.body.insertBefore(bar, document.body.childNodes[0]);
        this._membershipBar = $get('Membership_Login');
        var loggedIn = Sys.Services.AuthenticationService.get_isLoggedIn();
        if (!this._membershipBar) {
            this._membershipBar = document.createElement('div');
            if (document.body.childNodes.length > 0)
                document.body.insertBefore(this._membershipBar, document.body.childNodes[0]);
            else
                document.body.appendChild(this._membershipBar);
            var sb = new Sys.StringBuilder();
            this._membershipBar.id = 'Membership_Login';
            this._membershipBar.className = 'MembershipBar';
            if (this.get_displayHelp())
                sb.append(String.format('<div style="float:right;padding-top:5px;padding-right:4px;" class="HelpLink"> | <a href="javascript:" onclick="$find(&quot;{0}&quot;).help();return false;">{1}</a></div>', this.get_id(), Web.MembershipResources.Bar.HelpLink));
            if (!loggedIn && this.get_authenticationEnabled()) {
                if (this.get_displayLogin())
                    sb.append(String.format(
                        '<table id="LoginDialog" cellpadding="0" class="LoginDialog">' +
                        '<tr><td id="Anchor"><a href="javascript:" onfocus="$find(&quot;{0}&quot;).changeLoginDialogVisibility(true);return false">{1}</a>{2}</td></tr>' +
                        '<tr id="LoginControlsRow"><td id="LoginControls"><table>' +
                        '<tr><td align="right">{3}</td><td><input type="text" id="UserName" size="20" value="" /></td></tr>' +
                        '<tr><td align="right">{4}</td><td><input type="password" id="Password" size="20" /></td></tr>' +
                        (this.get_displayRememberMe() ? String.format('<tr><td align="right" colspan="2"><input type="checkbox" id="RememberMe"{0}/><label for="RememberMe">{1}</label></td></tr>', this.get_rememberMeSet() ? ' checked="checked"' : '', Web.MembershipResources.Bar.RememberMe) : '') +
                        (this.get_displayPasswordRecovery() ? '<tr><td>&nbsp;</td><td align="right"><a href="javascript:" onclick="$find(&quot;{0}&quot).passwordRecovery();return false;" id="PasswordRecovery">{6}</a></td></tr>' : '') +
                        (this.get_displaySignUp() ? '<tr><td>&nbsp;</td><td align="right"><a href="javascript:" onclick="$find(&quot;{0}&quot).signUp();return false;" id="SignUp">{7}</a></td></tr>' : '') +
                        '<tr><td>&nbsp;</td><td align="right"><button id="Login">{5}</button></td></tr>' +
                        '</table></td></tr></table>', this.get_id(), Web.MembershipResources.Bar.LoginLink, Web.MembershipResources.Bar.LoginText,
                        Web.MembershipResources.Bar.UserName, Web.MembershipResources.Bar.Password, Web.MembershipResources.Bar.LoginButton,
                        Web.MembershipResources.Bar.ForgotPassword, Web.MembershipResources.Bar.SignUp));
            }
            else
                sb.append(String.format(
                    '<table id="LoginDialog" cellpadding="0" class="LoginDialog LoginDialogCollapsed"><tr><td>' +
                    (this.get_welcome() ? this.get_welcome() + (this.get_authenticationEnabled() ? ' | ' : '') : '') +
                    (this.get_displayMyAccount() ? '<a id="MyAccount" href="javascript:" onclick="$find(&quot;{0}&quot;).myAccount();return false;">{1}</a> | ' : '') +
                    (this.get_authenticationEnabled() ? '<a href="javascript:" onclick="$find(&quot;{0}&quot;).logout();return false;">{2}</a>' : '') +
                    '</td></tr></table>', this.get_id(), Web.MembershipResources.Bar.MyAccount, Web.MembershipResources.Bar.LogoutLink));
            this._membershipBar.innerHTML = sb.toString();
            if (loggedIn && (this.get_enablePermalinks() || this.get_enableHistory())) {
                this._bookmarkBar = document.createElement('div');
                this._bookmarkBar.className = 'BookmarkBar';
                bar.appendChild(this._bookmarkBar);
                sb = new Sys.StringBuilder();
                sb.append('<table class="Frame"><tr>');
                if (this.get_enableHistory())
                    sb.appendFormat('<td><a id="{0}_HistoryLink" href="javascript:" onclick="$find(\'{0}\').showHistory();return false">{1}</a></td>', this.get_id(), Web.MembershipResources.Bar.History);
                if (this.get_enableHistory() && this.get_enablePermalinks())
                    sb.append('<td>|</td>');
                if (this.get_enablePermalinks())
                    sb.appendFormat('<td><a href="javascript:" onclick="$find(\'{0}\').showPermalink();return false">Permalink</a></td><td style="display:none"><input id="{0}_Permalink" type="text" onfocus="this.select();"/></td><td style="display:none"><a id="{0}_AddToFavorites" href="javascript:" onclick="$find(\'{0}\')._addToFavorites();return false" class="AddBookmark" title="{2}">&nbsp;</a></td><td style="display:none"><a id="{0}_CancelFavorites" href="javascript:" onclick="$find(\'{0}\').showPermalink();return false" class="CancelBookmark" title="{3}">&nbsp;</a></td>', this.get_id(), Web.MembershipResources.Bar.Permalink, Web.MembershipResources.Bar.AddToFavorites, Web.MembershipResources.Bar.HelpCloseButton);
                sb.append('</tr></table>');
                this._bookmarkBar.innerHTML = sb.toString();
            }
        }
        document.body.style.paddingTop = '0px';
        if (!loggedIn && this.get_displayLogin()) {
            this._loginDialog = $get('LoginDialog', this._membershipBar);
            this._loginDialog.style.width = this._loginDialog.offsetWidth + 'px';
            if ($get('LoginControlsRow', this._loginDialog))
                $addHandlers(this._loginDialog, { 'mouseover': this._loginDialogMouseOverHandler, 'mouseout': this._loginDialogMouseOutHandler }, this);
            this._loginButton = $get('Login', this._membershipBar);
            if (this._loginButton) $addHandler(this._loginButton, 'click', this._loginButtonClickHandler);
            this._userName = $get('UserName', this._membershipBar);
            if (this._userName) $addHandler(this._userName, 'keypress', this._textBoxKeyPressHandler);
            this._password = $get('Password', this._membershipBar);
            if (this._password) $addHandler(this._password, 'keypress', this._textBoxKeyPressHandler);
            this._rememberMe = $get('RememberMe', this._membershipBar);
            this.changeLoginDialogVisibility(window.location.href.match(/\?ReturnUrl=(.+)$/) != null);
        }
    },
    _login_Completed: function(validCredentials) {
        if (!validCredentials) {
            alert(Web.MembershipResources.Messages.InvalidUserNameAndPassword);
            this.changeLoginDialogVisibility(true);
            this._userName.focus();
        }
        else {
            var returnUrl = window.location.href.match(/\?ReturnUrl=(.+)$/);
            var anchoredHref = window.location.href.match(/^(.+?)#.*$/);
            window.location.href = returnUrl ? unescape(returnUrl[1]) : (anchoredHref ? anchoredHref[1] : window.location.href);
        }
    },
    _method_Failure: function(error, userContext, methodName) {
        alert(String.format('Method {0} has failed. {1}', methodName, error.get_message()));
    },
    _loginButtonClick: function(e) {
        this.login();
    },
    _loginDialogMouseOver: function(e) {
        this.changeLoginDialogVisibility(true, 50);
    },
    _loginDialogMouseOut: function(e) {
        this.changeLoginDialogVisibility(false, 500);
    },
    _textBoxKeyPress: function(e) {
        if (e.charCode == Sys.UI.Key.enter) {
            e.preventDefault();
            this.login();
        }
        if (e.charCode == Sys.UI.Key.esc) {
            e.preventDefault();
            this.set_isLoginDialogVisible(true);
            this.changeLoginDialogVisibility(false);
        }
    },
    login: function() {
        if (this._userName && this._password) {
            var isBlank = /^\s*$/;
            if (isBlank.exec(this._userName.value)) {
                alert(Web.MembershipResources.Messages.BlankUserName);
                this.set_isLoginDialogVisible(false);
                this._userName.focus();
                return;
            }
            if (isBlank.exec(this._password.value)) {
                this.set_isLoginDialogVisible(false);
                alert(Web.MembershipResources.Messages.BlankPassword);
                this.set_isLoginDialogVisible(false);
                this._password.focus();
                return;
            }
            Sys.Services.AuthenticationService.login(this._userName.value, this._password.value, this._rememberMe != null && this._rememberMe.checked, null, null, this._login_CompletedHandler, this._method_FailureHandler, null);
        }
        else
            alert('UserName and/or Password elements are not found in the Memership_Login');
    },
    logout: function() {
        this._protecting = false;
        Sys.Services.AuthenticationService.logout(null, null, null, null);
    },
    signUp: function() {
        this.changeLoginDialogVisibility(false);
        Web.DataView.showModal($get('SignUp', this.get_membershipBar()), 'aspnet_Membership', 'signUpForm', 'New', 'signUpForm', this.get_baseUrl(), this.get_servicePath());
    },
    passwordRecovery: function() {
        this.changeLoginDialogVisibility(false);
        Web.DataView.showModal($get('PasswordRecovery', this.get_membershipBar()), 'aspnet_Membership', 'passwordRequestForm', 'New', 'passwordRequestForm', this.get_baseUrl(), this.get_servicePath());
    },
    myAccount: function() {
        Web.DataView.showModal($get('MyAccount', this.get_membershipBar()), 'aspnet_Membership', 'myAccountForm', 'Edit', 'myAccountForm', this.get_baseUrl(), this.get_servicePath());
    },
    help: function(fullScreen) {
        var path = window.location.pathname;
        var helpPath = this.get_baseUrl() == './' ? path.substr(path.lastIndexOf('/'), 100) : '';
        if (helpPath.length == 0) {
            var baseSegments = this.get_baseUrl().split(/\//);
            var pathSegments = path.split(/\//);
            if (baseSegments[baseSegments.length - 1] == '')
                Array.removeAt(baseSegments, baseSegments.length - 1);
            if (pathSegments[pathSegments.length - 1] == '')
                Array.removeAt(pathSegments, pathSegments.length - 1);
            if (pathSegments[0] == '')
                Array.removeAt(pathSegments, 0);
            var levelsUp = baseSegments.length;
            if (pathSegments[pathSegments.length - 1].indexOf('.') == -1)
                levelsUp--;
            for (var i = pathSegments.length - levelsUp - 1; i < pathSegments.length; i++)
                helpPath += '/' + pathSegments[i];
        }
        var helpUrl = String.format('{0}help{1}', this.get_baseUrl(), helpPath);
        if (fullScreen) {
            window.open(helpUrl);
            this.help();
            return;
        }
        if (!this._helpBar) {
            this._helpDiv = document.createElement('div');
            document.body.appendChild(this._helpDiv);
            this._helpDiv.className = 'HelpBar';
            this._helpDiv.innerHTML = String.format('<div class="Title">{1}</div><iframe id="help" frameBorder="0"></iframe><div class="Buttons"><button onclick="$find(&quot;{0}&quot;).help()">{2}</button><button onclick="$find(&quot;{0}&quot;).help(true)">{3}</button></div>',
                this.get_id(), Web.MembershipResources.Bar.HelpLink, Web.MembershipResources.Bar.HelpCloseButton, Web.MembershipResources.Bar.HelpFullScreenButton);
            this._helpFrame = $get('help', this._helpDiv);
            this._helpBar = $create(AjaxControlToolkit.AlwaysVisibleControlBehavior, { HorizontalSide: AjaxControlToolkit.HorizontalSide.Right, VerticalSide: AjaxControlToolkit.VerticalSide.Top, HorizontalOffset: 15, VerticalOffset: 30 }, null, null, this._helpDiv);
            Sys.UI.DomElement.setVisible(this._helpDiv, false);
        }
        Sys.UI.DomElement.setVisible(this._helpDiv, !Sys.UI.DomElement.getVisible(this._helpDiv));
        if (Sys.UI.DomElement.getVisible(this._helpDiv)) {
            if (this._helpFrame.src == '')
                this._helpFrame.src = helpUrl;
            this._helpBar._reposition();
        }
    }
}
Web.Membership.registerClass('Web.Membership', Sys.Component);

if (typeof (Sys) !== 'undefined') Sys.Application.notifyScriptLoaded();
