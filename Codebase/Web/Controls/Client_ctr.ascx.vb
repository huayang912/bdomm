Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports BUDI2_NS.Data
Imports BUDI2_NS.Web

Partial Public Class Controls_Client
    Inherits System.Web.UI.UserControl

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load


        Dim chars As New List(Of App.CustomModels.StartsWith)
        chars = App.CustomModels.StartsWith.GetStartsWith()
        rptStartsWith.DataSource = chars
        rptStartsWith.DataBind()


        'End If
    End Sub



    Protected Sub SearchClientContact(ByVal startsWith As String)
        Dim filter As List(Of FieldFilter) = New List(Of FieldFilter)


        'The Like Filter for name added on 8th January 2010
        If String.IsNullOrEmpty(startsWith) Then
            filter.Add(New FieldFilter("Name", RowFilterOperation.None))
        Else
            filter.Add(New FieldFilter("Name", RowFilterOperation.Like, String.Format("{0}%", startsWith)))
        End If

        ClientListExtender.AssignFilter(filter)
    End Sub


    Protected Sub rptStartsWith_ItemDataBound(ByVal sender As Object, ByVal e As RepeaterItemEventArgs)
        Dim start As App.CustomModels.StartsWith
        start = CType(e.Item.DataItem, App.CustomModels.StartsWith)

        'Dim ltrStartsWith As Literal
        'ltrStartsWith = CType(e.Item.FindControl("ltrStartsWith"), Literal)
        'ltrStartsWith.Text = "Another Day In Paradise" 'GetHtml(start.Start);
        Dim lkbCommand As LinkButton
        lkbCommand = CType(e.Item.FindControl("lkbCommand"), LinkButton)
        lkbCommand.CommandArgument = start.Start
        lkbCommand.Text = start.Start
    End Sub
    Protected Sub rptStartsWith_Command(ByVal sender As Object, ByVal e As RepeaterCommandEventArgs)
        SearchClientContact(e.CommandArgument)
    End Sub
End Class