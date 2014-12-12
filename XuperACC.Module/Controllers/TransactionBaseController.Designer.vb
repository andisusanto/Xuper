Partial Class TransactionBaseController

    <System.Diagnostics.DebuggerNonUserCode()> _
    Public Sub New(ByVal Container As System.ComponentModel.IContainer)
        MyClass.New()

        'Required for Windows.Forms Class Composition Designer support
        Container.Add(Me)

    End Sub

    'Component overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Component Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Component Designer
    'It can be modified using the Component Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.SubmitAction = New DevExpress.ExpressApp.Actions.SimpleAction(Me.components)
        Me.TransactionTransDateFilter = New DevExpress.ExpressApp.Actions.SingleChoiceAction(Me.components)
        Me.TransactionStatusFilter = New DevExpress.ExpressApp.Actions.SingleChoiceAction(Me.components)
        Me.CancelSubmitAction = New DevExpress.ExpressApp.Actions.SimpleAction(Me.components)
        '
        'SubmitAction
        '
        Me.SubmitAction.Caption = "Submit"
        Me.SubmitAction.Category = "RecordEdit"
        Me.SubmitAction.ConfirmationMessage = "Are you sure want to submit this transaction?"
        Me.SubmitAction.Id = "SubmitAction"
        Me.SubmitAction.ImageName = "Submit"
        Me.SubmitAction.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject
        Me.SubmitAction.Shortcut = Nothing
        Me.SubmitAction.Tag = Nothing
        Me.SubmitAction.TargetObjectsCriteria = Nothing
        Me.SubmitAction.TargetViewId = Nothing
        Me.SubmitAction.ToolTip = Nothing
        Me.SubmitAction.TypeOfView = Nothing
        '
        'TransactionTransDateFilter
        '
        Me.TransactionTransDateFilter.Caption = "Date"
        Me.TransactionTransDateFilter.Category = "Filters"
        Me.TransactionTransDateFilter.ConfirmationMessage = Nothing
        Me.TransactionTransDateFilter.Id = "TransactionBaseFilter"
        Me.TransactionTransDateFilter.ImageName = Nothing
        Me.TransactionTransDateFilter.Shortcut = Nothing
        Me.TransactionTransDateFilter.Tag = Nothing
        Me.TransactionTransDateFilter.TargetObjectsCriteria = Nothing
        Me.TransactionTransDateFilter.TargetViewId = Nothing
        Me.TransactionTransDateFilter.TargetViewNesting = DevExpress.ExpressApp.Nesting.Root
        Me.TransactionTransDateFilter.TargetViewType = DevExpress.ExpressApp.ViewType.ListView
        Me.TransactionTransDateFilter.ToolTip = Nothing
        Me.TransactionTransDateFilter.TypeOfView = GetType(DevExpress.ExpressApp.ListView)
        '
        'TransactionStatusFilter
        '
        Me.TransactionStatusFilter.Caption = "Status"
        Me.TransactionStatusFilter.Category = "Filters"
        Me.TransactionStatusFilter.ConfirmationMessage = Nothing
        Me.TransactionStatusFilter.Id = "TransactionStatusFilter"
        Me.TransactionStatusFilter.ImageName = Nothing
        Me.TransactionStatusFilter.Shortcut = Nothing
        Me.TransactionStatusFilter.Tag = Nothing
        Me.TransactionStatusFilter.TargetObjectsCriteria = Nothing
        Me.TransactionStatusFilter.TargetViewId = Nothing
        Me.TransactionStatusFilter.TargetViewNesting = DevExpress.ExpressApp.Nesting.Root
        Me.TransactionStatusFilter.TargetViewType = DevExpress.ExpressApp.ViewType.ListView
        Me.TransactionStatusFilter.ToolTip = Nothing
        Me.TransactionStatusFilter.TypeOfView = GetType(DevExpress.ExpressApp.ListView)
        '
        'CancelSubmitAction
        '
        Me.CancelSubmitAction.Caption = "Cancel Submit"
        Me.CancelSubmitAction.Category = "RecordEdit"
        Me.CancelSubmitAction.ConfirmationMessage = "Are you sure want to cancel this transaction?"
        Me.CancelSubmitAction.Id = "CancelSubmitAction"
        Me.CancelSubmitAction.ImageName = "Cancel"
        Me.CancelSubmitAction.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject
        Me.CancelSubmitAction.Shortcut = Nothing
        Me.CancelSubmitAction.Tag = Nothing
        Me.CancelSubmitAction.TargetObjectsCriteria = Nothing
        Me.CancelSubmitAction.TargetViewId = Nothing
        Me.CancelSubmitAction.ToolTip = Nothing
        Me.CancelSubmitAction.TypeOfView = Nothing
        '
        'TransactionBaseController
        '
        Me.TargetViewNesting = DevExpress.ExpressApp.Nesting.Root

    End Sub
    Friend WithEvents SubmitAction As DevExpress.ExpressApp.Actions.SimpleAction
    Friend WithEvents TransactionTransDateFilter As DevExpress.ExpressApp.Actions.SingleChoiceAction
    Friend WithEvents TransactionStatusFilter As DevExpress.ExpressApp.Actions.SingleChoiceAction
    Friend WithEvents CancelSubmitAction As DevExpress.ExpressApp.Actions.SimpleAction

End Class
