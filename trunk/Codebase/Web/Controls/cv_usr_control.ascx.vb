Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports BUDI2_NS.Data
Imports BUDI2_NS.Web

Partial Public Class Controls_cv_usr_control
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

  filter.Add(New FieldFilter("CV", RowFilterOperation.None))
  filter.Add(New FieldFilter("Notes", RowFilterOperation.None))


       CVTextBox1.text="" 
       NotesTextBox1.text="" 



      dv100Extender.AssignFilter(filter)
end sub


    Protected Sub SearchButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SearchButton.Click
        Dim filter As List(Of FieldFilter) = New List(Of FieldFilter)
   
    If CVTextBox1.text="" Then
            filter.Add(New FieldFilter("CV", RowFilterOperation.None))
        Else
            filter.Add(New FieldFilter("CV", RowFilterOperation.Like, CVTextBox1.text))
        End If

   If NotesTextBox1.text="" Then
            filter.Add(New FieldFilter("Notes", RowFilterOperation.None))
        Else
            filter.Add(New FieldFilter("Notes", RowFilterOperation.Like, NotesTextBox1.text))
        End If 

       dv100Extender.AssignFilter(filter)
     End Sub


End Class