Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports BUDI2_NS.Data
Imports BUDI2_NS.Web

Partial Public Class Controls_ClientContactSrch
    Inherits System.Web.UI.UserControl

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            ' EnquiryStatusText.Text = Session("EnquiryStatusText")
            'SupplierCompanyNameText.Text = Session("SupplierCompanyName")
            'CategoryCategoryNameText.Text = Session("CategoryCategoryName")
        End If
    End Sub

    Protected Sub ShowAllButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ShowAllButton.Click
        Dim filter As List(Of FieldFilter) = New List(Of FieldFilter)

        filter.Add(New FieldFilter("NL_All", RowFilterOperation.None))
        filter.Add(New FieldFilter("NL_Procurement", RowFilterOperation.None))
        filter.Add(New FieldFilter("NL_Personnel", RowFilterOperation.None))
        filter.Add(New FieldFilter("NL_O_M", RowFilterOperation.None))
        filter.Add(New FieldFilter("NL_Project", RowFilterOperation.None))

        CRM_Type.SelectedIndex = -1
    


        ClientContactListExtender.AssignFilter(filter)
    End Sub


    Protected Sub SearchButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SearchButton.Click
        Dim filter As List(Of FieldFilter) = New List(Of FieldFilter)
        '   If String.IsNullOrEmpty(EnquiryStatusText.Text) Then
        '       filter.Add(New FieldFilter("StatusName", RowFilterOperation.None))
        '   Else
        '       filter.Add(New FieldFilter("StatusName", RowFilterOperation.Like, EnquiryStatusText.Text))
        '   End If

    

        If CRM_Type.Text = "Show All" Then
            filter.Add(New FieldFilter("NL_All", RowFilterOperation.None))
            filter.Add(New FieldFilter("NL_Procurement", RowFilterOperation.None))
            filter.Add(New FieldFilter("NL_Personnel", RowFilterOperation.None))
            filter.Add(New FieldFilter("NL_O_M", RowFilterOperation.None))
            filter.Add(New FieldFilter("NL_Project", RowFilterOperation.None))

        ElseIf CRM_Type.Text = "All" Then
            filter.Add(New FieldFilter("NL_All", RowFilterOperation.Equal, True))

            filter.Add(New FieldFilter("NL_Procurement", RowFilterOperation.None))
            filter.Add(New FieldFilter("NL_Personnel", RowFilterOperation.None))
            filter.Add(New FieldFilter("NL_O_M", RowFilterOperation.None))
            filter.Add(New FieldFilter("NL_Project", RowFilterOperation.None))

        ElseIf CRM_Type.Text = "Procurement" Then
            filter.Add(New FieldFilter("NL_Procurement", RowFilterOperation.Equal, True))

            filter.Add(New FieldFilter("NL_All", RowFilterOperation.None))
            filter.Add(New FieldFilter("NL_Personnel", RowFilterOperation.None))
            filter.Add(New FieldFilter("NL_O_M", RowFilterOperation.None))
            filter.Add(New FieldFilter("NL_Project", RowFilterOperation.None))

        ElseIf CRM_Type.Text = "OM" Then
            filter.Add(New FieldFilter("NL_O_M", RowFilterOperation.Equal, True))

            filter.Add(New FieldFilter("NL_All", RowFilterOperation.None))
            filter.Add(New FieldFilter("NL_Procurement", RowFilterOperation.None))
            filter.Add(New FieldFilter("NL_Personnel", RowFilterOperation.None))
            filter.Add(New FieldFilter("NL_Project", RowFilterOperation.None))


        ElseIf CRM_Type.Text = "Personnel" Then
            filter.Add(New FieldFilter("NL_Personnel", RowFilterOperation.Equal, True))

            filter.Add(New FieldFilter("NL_All", RowFilterOperation.None))
            filter.Add(New FieldFilter("NL_Procurement", RowFilterOperation.None))
            filter.Add(New FieldFilter("NL_O_M", RowFilterOperation.None))
            filter.Add(New FieldFilter("NL_Project", RowFilterOperation.None))



        ElseIf CRM_Type.Text = "Project" Then
            filter.Add(New FieldFilter("NL_Project", RowFilterOperation.Equal, True))

            filter.Add(New FieldFilter("NL_All", RowFilterOperation.None))
            filter.Add(New FieldFilter("NL_Procurement", RowFilterOperation.None))
            filter.Add(New FieldFilter("NL_Personnel", RowFilterOperation.None))
            filter.Add(New FieldFilter("NL_O_M", RowFilterOperation.None))


        End If

  
        ClientContactListExtender.AssignFilter(filter)
       End Sub


    
End Class