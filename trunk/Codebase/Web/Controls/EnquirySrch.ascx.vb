Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports BUDI2_NS.Data
Imports BUDI2_NS.Web

Partial Public Class Controls_EnquirySrch
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

        filter.Add(New FieldFilter("StatusName", RowFilterOperation.None))
        filter.Add(New FieldFilter("CreatedOn", RowFilterOperation.None))

        StatusList.SelectedIndex = -1
        YearList.SelectedIndex = -1
        MonthList.SelectedIndex = -1


        EnquiryListExtender.AssignFilter(filter)
    End Sub


    Protected Sub SearchButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SearchButton.Click
        Dim filter As List(Of FieldFilter) = New List(Of FieldFilter)
        '   If String.IsNullOrEmpty(EnquiryStatusText.Text) Then
        '       filter.Add(New FieldFilter("StatusName", RowFilterOperation.None))
        '   Else
        '       filter.Add(New FieldFilter("StatusName", RowFilterOperation.Like, EnquiryStatusText.Text))
        '   End If

        If StatusList.Text = "All" Then
            filter.Add(New FieldFilter("StatusName", RowFilterOperation.None))
        Else
            filter.Add(New FieldFilter("StatusName", RowFilterOperation.Like, StatusList.Text))
        End If
        If YearList.Text = "All" Then
            filter.Add(New FieldFilter("CreatedOn", RowFilterOperation.None))
        Else

            Dim Dt As DateTime = CDate(YearList.SelectedItem.Value)

            Dt = Dt.AddYears(1)  ' add year
            'Dt=Dt.AddDays(-1)  ' add year
            '  Dt=Dt.AddHours(23)  ' add year
            '  Dt=Dt.addMinutes(59)  ' take out 1 day so its
            '  Dt=Dt.addSeconds(59) 

            filter.Add(New FieldFilter("CreatedOn", RowFilterOperation.GreaterThanOrEqual, CDate(YearList.SelectedItem.Value)))
            filter.Add(New FieldFilter("CreatedOn", RowFilterOperation.LessThan, CDate(Dt)))
        End If

        If MonthList.Text = "All" Then
            filter.Add(New FieldFilter("CreatedOn", RowFilterOperation.None))
        Else
            filter.Add(New FieldFilter("CreatedOn", RowFilterOperation.None)) ' clear year filter
            Dim dYear As DateTime

            If YearList.Text = "All" Then
                dYear = CDate("1/2010")
            Else 'year elected
                dYear = CDate(YearList.SelectedItem.Value)
            End If

            Dim Dtm_from As DateTime = dYear
            Dim Dtm_to As DateTime = dYear

            Dtm_from = Dtm_from.AddMonths(MonthList.SelectedItem.Value - 1)
            Dtm_to = Dtm_to.AddMonths(MonthList.SelectedItem.Value)


            '  Dt=Dt.addSeconds(59) 

            filter.Add(New FieldFilter("CreatedOn", RowFilterOperation.GreaterThanOrEqual, CDate(Dtm_from)))
            filter.Add(New FieldFilter("CreatedOn", RowFilterOperation.LessThan, CDate(Dtm_to)))
        End If



        '   If String.IsNullOrEmpty(SupplierCompanyNameText.Text) Then
        '        filter.Add(New FieldFilter("SupplierCompanyName", RowFilterOperation.None))
        '   Else
        '       filter.Add(New FieldFilter("SupplierCompanyName", RowFilterOperation.Like, SupplierCompanyNameText.Text))
        '   End If
        '   If String.IsNullOrEmpty(CategoryCategoryNameText.Text) Then
        '      filter.Add(New FieldFilter("CategoryCategoryName", RowFilterOperation.None))
        '   Else
        '       filter.Add(New FieldFilter("CategoryCategoryName", RowFilterOperation.Like, CategoryCategoryNameText.Text))
        '   End If
        EnquiryListExtender.AssignFilter(filter)
        '     EnquiryStatusText.Focus()
        '    Session("EnquiryStatusText") = EnquiryStatusText.Text
        '   Session("SupplierCompanyName") = SupplierCompanyNameText.Text
        '   Session("CategoryCategoryName") = CategoryCategoryNameText.Text
    End Sub


End Class