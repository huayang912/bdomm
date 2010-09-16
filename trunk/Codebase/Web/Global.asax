<%@ Application Language="C#" %>

<script runat="server">
void Application_Start(object sender, EventArgs e)
{
    // Code that runs on application startup
    MembershipUser admin = Membership.GetUser("admin");
    if ((admin != null) && admin.IsLockedOut)
    	admin.UnlockUser();
    MembershipUser user = Membership.GetUser("user");
    if ((user != null) && user.IsLockedOut)
    	user.UnlockUser();
    if (Membership.GetUser("admin") == null)
    {
        // Automatic registration of 'admin' and 'user' may be removed from the final code.
        MembershipCreateStatus status;
        admin = Membership.CreateUser("admin", "admin123%", "admin@BUDI2_NS.com", "ASP.NET", "Code OnTime", true, out status);
        user = Membership.CreateUser("user", "user123%", "user@BUDI2_NS.com", "ASP.NET", "Code OnTime", true, out status);
        Roles.CreateRole("Administrators");
        Roles.CreateRole("Users");
        Roles.AddUserToRole(admin.UserName, "Users");
        Roles.AddUserToRole(user.UserName, "Users");
        Roles.AddUserToRole(admin.UserName, "Administrators");
    }
}

void Application_End(object sender, EventArgs e)
{
    //  Code that runs on application shutdown
}

void Application_Error(object sender, EventArgs e)
{
    // Code that runs when an unhandled error occurs
}

void Session_Start(object sender, EventArgs e)
{
    // Code that runs when a new session is started
}

void Session_End(object sender, EventArgs e)
{
    // Code that runs when a session ends.
    // Note: The Session_End event is raised only when the sessionstate mode
    // is set to InProc in the Web.config file. If session mode is set to StateServer
    // or SQLServer, the event is not raised.
}
</script>