Imports System
Imports System.ComponentModel
Imports System.Collections.Generic
Imports System.Diagnostics
Imports System.Text

Imports DevExpress.ExpressApp
Imports DevExpress.ExpressApp.Actions
Imports DevExpress.Persistent.Base
Imports DevExpress.XtraReports.UI
Imports DevExpress.Data.Filtering
Imports DevExpress.ExpressApp.Xpo
Imports System.Data.SqlClient
Imports Report.Module

Public Class PrintInvoiceController
    Inherits DevExpress.ExpressApp.ViewController

    Public ReadOnly Property CurObject As SalesInvoice
        Get
            Return View.CurrentObject
        End Get
    End Property

    Public Sub New()
        MyBase.New()

        'This call is required by the Component Designer.
        InitializeComponent()
        RegisterActions(components)

    End Sub

    Private Sub PrintInvoice_Execute(sender As Object, e As SimpleActionExecuteEventArgs) Handles PrintInvoice.Execute
        View.ObjectSpace.CommitChanges()
        Dim tmpName As String = "Report Sales Invoice"
        Dim rpt As Report.Module.Report = View.ObjectSpace.FindObject(Of Report.Module.Report)(New BinaryOperator("Name", tmpName))
        Dim lst As New List(Of IReportParameterControl)
        lst.Add(New Report.Module.SystemReportParameterControl With {.ControlName = "SalesInvoice", .IsActive = True, .Values = {"('" & CurObject.Oid.ToString() & "')"}, .CriteriaString = {CurObject.No}})
        Dim Xrpt = rpt.GetXtraReport(lst)
        Xrpt.ShowRibbonPreviewDialog()
    End Sub
End Class
