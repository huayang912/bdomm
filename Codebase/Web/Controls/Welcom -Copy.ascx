<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Welcome.ascx.cs" Inherits="Controls_Welcome"  %>
								
<div style="padding-left:8px"><div class="ParaInfo">
        Sign in to access the protected site content.</div>
<div class="ParaHeader">
    Instructions
</div>
<div class="ParaText">
    Two standard user accounts are automatically created when application is initialized
    if membership option has been selected for this application.
</div>
<div class="ParaText">
    The administrative account <b>admin</b> is authorized to access all areas of the
    web site and membership manager. The standard <b>user</b> account is allowed to
    access all areas of the web site with the exception of membership manager.</div>
<div class="ParaText">
    Move the mouse pointer over the link <i>Login to this web site</i> on the right-hand side
    at the top of the page and sign in with one of the accounts listed below.</div>
<div class="ParaText">
    <div style="border: solid 1px black; background-color: InfoBackground; padding: 8px;
        float: left;">
        Administrative account:<br />
        <b title="User Name">admin</b> / <b title="Password">admin123%</b>
        <br />
        <br />
        Standard user account:<br />
        <b title="User Name">user</b> / <b title="Password">user123%</b>
    </div>
</div>

							