Imports System
Imports System.Collections.Generic
Imports DevExpress.Xpo
Imports DevExpress.ExpressApp
Imports DevExpress.Persistent.Base
Imports DevExpress.Persistent.BaseImpl
Imports DevExpress.Persistent.Validation
Imports DevExpress.ExpressApp.ConditionalAppearance
Imports DevExpress.ExpressApp.Model

<RuleCriteria("Rule Criteria for TransactionBase.Delete", DefaultContexts.Delete, "Status <> 'Submitted'", "Cannot delete submitted transaction")>
<Appearance("Appearance for TransactionBase.Default", AppearanceItemType:="ViewItem", enabled:=False, targetitems:="EntryDateTime, SubmitDateTime, Status")>
<Appearance("Appearance for TransactionBase.Status = Submitted", AppearanceItemType:="ViewItem", enabled:=False, targetitems:="*", criteria:="Status = 'Submitted'")> _
<NonPersistent()> _
<DeferredDeletion(False)> _
<DefaultClassOptions()> _
Public Class TransactionBase
    Inherits AppBase
    Public Sub New(ByVal session As Session)
        MyBase.New(session)
        ' This constructor is used when an object is loaded from a persistent storage.
        ' Do not place any code here or place it only when the IsLoading property is false:
        ' if (!IsLoading){
        '   It is now OK to place your initialization code here.
        ' }
        ' or as an alternative, move your initialization code into the AfterConstruction method.
    End Sub
    Public Overrides Sub AfterConstruction()
        MyBase.AfterConstruction()
        ' Place here your initialization code.
    End Sub

    Private _entryDateTime As DateTime
    Private _submitDateTime As DateTime
    Private _status As TransactionStatus

    <ModelDefault("DisplayFormat", "dd MMM yyyy HH:mm:ss")>
    <ModelDefault("EditMask", "dd MMM yyyy HH:mm:ss")>
    Public Property EntryDateTime() As DateTime
        Get
            Return _entryDateTime
        End Get
        Set(ByVal value As DateTime)
            SetPropertyValue("EntryDate", _entryDateTime, value)
        End Set
    End Property
    <ModelDefault("DisplayFormat", "dd MMM yyyy HH:mm:ss")>
    <ModelDefault("EditMask", "dd MMM yyyy HH:mm:ss")>
    Public Property SubmitDateTime() As DateTime
        Get
            Return _submitDateTime
        End Get
        Set(ByVal value As DateTime)
            SetPropertyValue("SubmitDateTime", _submitDateTime, value)
        End Set
    End Property
    Public Property Status() As TransactionStatus
        Get
            Return _status
        End Get
        Set(ByVal value As TransactionStatus)
            SetPropertyValue("Status", _status, value)
        End Set
    End Property
    Public Sub Submit()
        If Status <> TransactionStatus.Submitted Then
            OnSubmitting()
            Status = TransactionStatus.Submitted
            SubmitDateTime = GetServerNow(Session)
            OnSubmitted()
        End If
    End Sub
    Public Sub Cancel()
        If Status = TransactionStatus.Submitted Then
            OnCanceling()
            Status = TransactionStatus.Entered
            SubmitDateTime = New DateTime()
            OnCanceled()
        End If
    End Sub
    Protected Overridable Sub OnSubmitting()
    End Sub
    Protected Overridable Sub OnSubmitted()
    End Sub
    Protected Overridable Sub OnCanceling()
    End Sub
    Protected Overridable Sub OnCanceled()
    End Sub
    Protected Overrides Sub OnSaving()
        MyBase.OnSaving()
        If Not TypeOf Session Is NestedUnitOfWork AndAlso Session.IsNewObject(Me) Then
            EntryDateTime = GetServerNow(Session)
        End If
    End Sub
End Class
Public Enum TransactionStatus
    Entered
    Submitted
    Canceled
End Enum