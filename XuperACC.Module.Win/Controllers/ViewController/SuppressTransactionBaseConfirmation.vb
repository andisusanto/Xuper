Imports System
Imports System.ComponentModel
Imports System.Collections.Generic
Imports System.Diagnostics
Imports System.Text

Imports DevExpress.ExpressApp
Imports DevExpress.ExpressApp.Actions
Imports DevExpress.Persistent.Base

Public Class SuppressTransactionBaseConfirmation
    Inherits DevExpress.ExpressApp.Win.SystemModule.WinModificationsController

	Public Sub New()
		MyBase.New()

		'This call is required by the Component Designer.
		InitializeComponent()
		RegisterActions(components) 

    End Sub
    Private WithEvents SubmitAction As SimpleAction
    Private WithEvents CancelSubmitAction As SimpleAction
    Protected Overrides Sub OnActivated()
        MyBase.OnActivated()
        If IsDescendant(Of TransactionBase)(View.ObjectTypeInfo.Type) Then
            SubmitAction = Frame.GetController(Of XuperACC.Module.TransactionBaseController)().MySubmitAction
            CancelSubmitAction = Frame.GetController(Of XuperACC.Module.TransactionBaseController).MyCancelAction
        End If
    End Sub

    Private Sub TransactionConfirmationAction_Executing(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles CancelSubmitAction.Executing, SubmitAction.Executing
        ModificationsHandlingMode = DevExpress.ExpressApp.SystemModule.ModificationsHandlingMode.AutoRollback
    End Sub
End Class
