<%@ Control Language="C#" AutoEventWireup="true" CodeFile="jQueryCalendar.ascx.cs" Inherits="UserControls_jQueryCalendar" EnableViewState="false" %>

<link href="../Scripts/jquery.ui.datepicker.css" type="text/css" rel="stylesheet" />
<script language="javascript" src="../Scripts/jquery.ui.core.min.js" type="text/javascript"></script>
<script language="javascript" src="../Scripts/jquery.ui.datepicker.min.js" type="text/javascript"></script>

<script language="javascript" type="text/javascript">
    
    DATE_FORMAT = '<%=ConfigReader.JSCalendarDateFormat %>';
    
    $(document).ready(function() {
        $('.CalendarTextBox').each(function() {
            $(this).datepicker({
                showOn: 'button',
                dateFormat: '<%=ConfigReader.JSCalendarDateFormat %>',
                changeMonth: true,
                changeYear: true,
                buttonImageOnly: true,
                buttonImage: '../Images/calenderIconBig.gif',
                buttonText: ''
            });
        });            
    });                
</script>