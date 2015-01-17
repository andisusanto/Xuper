Partial Class PrintInvoiceController

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
        Me.PrintInvoice = New DevExpress.ExpressApp.Actions.SimpleAction(Me.components)
        '
        'PrintInvoice
        '
        Me.PrintInvoice.Caption = "Print Invoice"
        Me.PrintInvoice.Category = "View"
        Me.PrintInvoice.ConfirmationMessage = Nothing
        Me.PrintInvoice.Id = "PrintInvoice"
        Me.PrintInvoice.ImageName = "Action_Printing_Print"
        Me.PrintInvoice.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.CaptionAndImage
        Me.PrintInvoice.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject
        Me.PrintInvoice.Shortcut = "Ctrl+p"
        Me.PrintInvoice.Tag = Nothing
        Me.PrintInvoice.TargetObjectsCriteria = Nothing
        Me.PrintInvoice.TargetObjectType = GetType(XuperACC.[Module].SalesInvoice)
        Me.PrintInvoice.TargetViewId = Nothing
        Me.PrintInvoice.TargetViewNesting = DevExpress.ExpressApp.Nesting.Root
        Me.PrintInvoice.ToolTip = Nothing
        Me.PrintInvoice.TypeOfView = Nothing

    End Sub
    Friend WithEvents PrintInvoice As DevExpress.ExpressApp.Actions.SimpleAction

End Class
