/// <reference name="MicrosoftAjax.debug.js" />

Type.registerNamespace("Web");

if (!Web) Web = {}
Web.MembershipResources = {}
Web.MembershipResources.Bar = {
    LoginLink: 'Login',
    LoginText: ' to this website',
    HelpLink: 'Help',
    UserName: 'User Name:',
    Password: 'Password:',
    RememberMe: 'Remember me next time',
    ForgotPassword: 'Forgot your password?',
    SignUp: 'Sign up now',
    LoginButton: 'Login',
    MyAccount: 'My Account',
    LogoutLink: 'Logout',
    HelpCloseButton: 'Close',
    HelpFullScreenButton: 'Full Screen',
    UserIdle: 'Are you still there? Please login again.',
    History: 'History',
    Permalink: 'Permalink',
    AddToFavorites: 'Add to Favorites',
    RotateHistory: 'Rotate',
    Welcome: 'Welcome <b>{0}</b>, Today is {1:D}',
    ChangeLanguageToolTip: 'Change your language',
    PermalinkToolTip: 'Create a permanent link for selected record',
    HistoryToolTip: 'View history of previously selected records',
    AutoDetectLanguageOption: 'Auto Detect'
}

Web.MembershipResources.Messages = {
    InvalidUserNameAndPassword: 'Your user name and password are not valid.',
    BlankUserName: 'User name cannot be blank.',
    BlankPassword: 'Password cannot be blank.',
    PermalinkUnavailable: 'Permalink is not available. Please select a record.',
    HistoryUnavailable: 'History is not available.'
}

Web.MembershipResources.Manager = {
    UsersTab: 'Users',
    RolesTab: 'Roles'
}

if (typeof (Sys) !== 'undefined') Sys.Application.notifyScriptLoaded();
