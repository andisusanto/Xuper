Imports Report.Module
Public Class SystemReportParameterControl
    Implements IReportParameterControl

    Public Property ControlName As String
    Public Property CriteriaString As String()
    Public Property Values() As String()
    Public Property IsActive As Boolean Implements IReportParameterControl.IsActive

    Public ReadOnly Property IReportParameterControl_ControlName As String Implements IReportParameterControl.ControlName
        Get
            Return ControlName
        End Get
    End Property

    Public ReadOnly Property IReportParameterControl_CriteriaStrings As String() Implements IReportParameterControl.CriteriaStrings
        Get
            Return CriteriaString
        End Get
    End Property

    Public ReadOnly Property IReportParameterControl_Values As String() Implements IReportParameterControl.Values
        Get
            Return Values
        End Get
    End Property
End Class