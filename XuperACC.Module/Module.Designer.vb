Imports Microsoft.VisualBasic
Imports System

Partial Public Class XuperACCModule
    ''' <summary> 
    ''' Required designer variable.
    ''' </summary>
    Private components As System.ComponentModel.IContainer = Nothing

    ''' <summary> 
    ''' Clean up any resources being used.
    ''' </summary>
    ''' <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso (Not components Is Nothing) Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

#Region "Component Designer generated code"

    ''' <summary> 
    ''' Required method for Designer support - do not modify 
    ''' the contents of this method with the code editor.
    ''' </summary>
    Private Sub InitializeComponent()
        '
        'XuperACCModule
        '
        Me.AdditionalExportedTypes.Add(GetType(Report.[Module].Query))
        Me.AdditionalExportedTypes.Add(GetType(Report.[Module].QueryParameter))
        Me.AdditionalExportedTypes.Add(GetType(Report.[Module].Report))
        Me.AdditionalExportedTypes.Add(GetType(Report.[Module].ReportBaseObject))
        Me.AdditionalExportedTypes.Add(GetType(Report.[Module].ReportConfiguration))
        Me.AdditionalExportedTypes.Add(GetType(Report.[Module].ReportGeneralParameter))
        Me.AdditionalExportedTypes.Add(GetType(Report.[Module].ReportType))
        Me.RequiredModuleTypes.Add(GetType(DevExpress.ExpressApp.SystemModule.SystemModule))
        Me.RequiredModuleTypes.Add(GetType(DevExpress.ExpressApp.CloneObject.CloneObjectModule))
        Me.RequiredModuleTypes.Add(GetType(DevExpress.ExpressApp.ConditionalAppearance.ConditionalAppearanceModule))
        Me.RequiredModuleTypes.Add(GetType(DevExpress.ExpressApp.Validation.ValidationModule))
        Me.RequiredModuleTypes.Add(GetType(DevExpress.ExpressApp.Security.SecurityModule))

    End Sub

#End Region
End Class
