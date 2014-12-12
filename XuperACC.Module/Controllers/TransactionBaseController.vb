Imports System
Imports System.ComponentModel
Imports System.Collections.Generic
Imports System.Diagnostics
Imports System.Text
Imports System.Linq

Imports DevExpress.ExpressApp
Imports DevExpress.ExpressApp.Actions
Imports DevExpress.Persistent.Base
Imports DevExpress.ExpressApp.Editors
Imports DevExpress.Data.Filtering

Public Class TransactionBaseController
    Inherits DevExpress.ExpressApp.ViewController

    Public Sub New()
        MyBase.New()

        'This call is required by the Component Designer.
        InitializeComponent()
        RegisterActions(components)

    End Sub
    Public ReadOnly Property MySubmitAction As SimpleAction
        Get
            Return SubmitAction
        End Get
    End Property
    Public ReadOnly Property MyCancelAction As SimpleAction
        Get
            Return CancelSubmitAction
        End Get
    End Property
    Protected Overrides Sub OnActivated()
        MyBase.OnActivated()
        If IsDescendant(Of TransactionBase)(View.ObjectTypeInfo.Type) Then
            AddHandler View.CurrentObjectChanged, AddressOf View_CurrentObjectChanged
            CheckSubmitAction()
            If TypeOf View Is ListView Then
                SetTransDateFilterAction()
                SetStatusFilterAction()
                TransactionTransDateFilter.SelectedIndex = 0
                TransactionStatusFilter.SelectedIndex = 0
                TransactionTransDateFilter.Caption = "Date:"
                TransactionStatusFilter.Caption = "Status:"
                ExecuteTransDateFilter()
                ExecuteStatusFilter()
            End If
        Else
            SubmitAction.Active("SubmitAction") = False
            CancelSubmitAction.Active("CancelSubmitAction") = False
            TransactionTransDateFilter.Active("TransactiomBaseFilter") = False
            TransactionStatusFilter.Active("TransactiomBaseFilter") = False
        End If

    End Sub
#Region "Submit"

    Private Sub CheckSubmitAction()
        If View.CurrentObject Is Nothing OrElse CType(View.CurrentObject, TransactionBase).Status <> TransactionStatus.Submitted Then
            SubmitAction.Active("SubmitAction") = True
            CancelSubmitAction.Active("CancelSubmitAction") = False
        Else
            SubmitAction.Active("SubmitAction") = False
            CancelSubmitAction.Active("CancelSubmitAction") = True
        End If
    End Sub
    Private Sub View_CurrentObjectChanged(sender As Object, e As System.EventArgs)
        CheckSubmitAction()
    End Sub

    Private Sub SubmitAction_Execute(sender As Object, e As SimpleActionExecuteEventArgs) Handles SubmitAction.Execute
        Try
            View.ObjectSpace.CommitChanges()
            CType(View.CurrentObject, TransactionBase).Submit()
            View.ObjectSpace.CommitChanges()
        Finally
            View.ObjectSpace.Refresh()
        End Try
    End Sub

    Private Sub CancelSubmitAction_Execute(sender As Object, e As SimpleActionExecuteEventArgs) Handles CancelSubmitAction.Execute
        Try
            View.ObjectSpace.CommitChanges()
            CType(View.CurrentObject, TransactionBase).Cancel()
            View.ObjectSpace.CommitChanges()
        Finally
            View.ObjectSpace.Refresh()
        End Try
    End Sub
#End Region

#Region "Filtering"
    Private Sub SetTransDateFilterAction()
        With TransactionTransDateFilter.Items
            .Add(New ChoiceActionItem("Today", 0))
            .Add(New ChoiceActionItem("Last One Week", 1))
            .Add(New ChoiceActionItem("Last One Month", 2))
            .Add(New ChoiceActionItem("All", 3))
        End With
    End Sub

    Private Sub SetStatusFilterAction()
        With TransactionStatusFilter.Items
            .Add(New ChoiceActionItem("Submitted", 0))
            .Add(New ChoiceActionItem("Entry", 1))
            .Add(New ChoiceActionItem("All", 2))
        End With
    End Sub
    Private Sub TransactionBaseFilter_Execute(sender As Object, e As SingleChoiceActionExecuteEventArgs) Handles TransactionTransDateFilter.Execute
        ExecuteTransDateFilter()
    End Sub
    Private Sub ExecuteTransDateFilter()
        Select Case TransactionTransDateFilter.SelectedIndex
            Case 0
                CType(View, ListView).CollectionSource.Criteria("TransDateFilter") = New BinaryOperator("TransDate", Today.Date)
            Case 1
                CType(View, ListView).CollectionSource.Criteria("TransDateFilter") = New BetweenOperator("TransDate", DateAdd(DateInterval.Weekday, -1, Today.Date), Today.Date)
            Case 2
                CType(View, ListView).CollectionSource.Criteria("TransDateFilter") = New BetweenOperator("TransDate", DateAdd(DateInterval.Month, -1, Today.Date), Today.Date)
            Case 3
                CType(View, ListView).CollectionSource.Criteria("TransDateFilter") = Nothing
        End Select
    End Sub

    Private Sub TransactionStatusFilter_Execute(sender As Object, e As SingleChoiceActionExecuteEventArgs) Handles TransactionStatusFilter.Execute
        ExecuteStatusFilter()
    End Sub
    Private Sub ExecuteStatusFilter()
        Select Case TransactionStatusFilter.SelectedIndex
            Case 0
                CType(View, ListView).CollectionSource.Criteria("StatusFilter") = New BinaryOperator("Status", TransactionStatus.Submitted)
            Case 1
                CType(View, ListView).CollectionSource.Criteria("StatusFilter") = New BinaryOperator("Status", TransactionStatus.Entered)
            Case 2
                CType(View, ListView).CollectionSource.Criteria("StatusFilter") = Nothing
        End Select
    End Sub
#End Region
End Class
