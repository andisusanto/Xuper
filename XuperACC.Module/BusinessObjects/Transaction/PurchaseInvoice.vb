Imports Microsoft.VisualBasic
Imports System
Imports System.Linq
Imports System.Text
Imports DevExpress.Xpo
Imports DevExpress.ExpressApp
Imports System.ComponentModel
Imports DevExpress.ExpressApp.DC
Imports DevExpress.Data.Filtering
Imports DevExpress.Persistent.Base
Imports System.Collections.Generic
Imports DevExpress.ExpressApp.Model
Imports DevExpress.Persistent.BaseImpl
Imports DevExpress.Persistent.Validation
Imports DevExpress.ExpressApp.ConditionalAppearance

<RuleCriteria("Rule Criteria for Cancel PurchaseInvoice.PaymentOutstandingStatus", "Cancel", "PaymentOutstandingStatus = 'Full'")>
<RuleCriteria("Rule Criteria for Cancel PurchaseInvoice.ReturnOutstandingStatus", "Cancel", "ReturnOutstandingStatus = 'Full'")>
<RuleCriteria("Rule Criteria for PurchaseInvoice.Total > 0", DefaultContexts.Save, "Total > 0")>
<Appearance("Appearance Default Disabled for PurchasePayment", enabled:=False, AppearanceItemType:="ViewItem", targetitems:="Total, PaymentOutstandingStatus")>
<DeferredDeletion(False)>
<DefaultClassOptions()> _
Public Class PurchaseInvoice
    Inherits TransactionBase
    Public Sub New(ByVal session As Session)
        MyBase.New(session)

    End Sub

    Public Overrides Sub AfterConstruction()
        MyBase.AfterConstruction()
        TransDate = Now
    End Sub
    Private _no As String
    Private _referenceNo As String
    Private _transDate As Date
    Private _supplier As Supplier
    Private _inventory As Inventory
    Private _total As Double
    Private _paymentOutstandingStatus As OutstandingStatus
    Private _returnOutstandingStatus As OutstandingStatus
    <RuleUniqueValue("Rule Unique for PurchaseInvoice.No", DefaultContexts.Save)>
    <RuleRequiredField("Rule Required for PurchaseInvoice.No", DefaultContexts.Save)>
    Public Property No As String
        Get
            Return _no
        End Get
        Set(value As String)
            SetPropertyValue("No", _no, value)
        End Set
    End Property
    <RuleRequiredField("Rule Required for PurchaseInvoice.ReferenceNo", DefaultContexts.Save)>
    Public Property ReferenceNo As String
        Get
            Return _referenceNo
        End Get
        Set(value As String)
            SetPropertyValue("ReferenceNo", _referenceNo, value)
        End Set
    End Property
    <RuleRequiredField("Rule Required for PurchaseInvoice.TransDate", DefaultContexts.Save)>
    Public Property TransDate As Date
        Get
            Return _transDate
        End Get
        Set(value As Date)
            SetPropertyValue("TransDate", _transDate, value)
        End Set
    End Property
    <RuleRequiredField("Rule Required for PurchaseInvoice.Supplier", DefaultContexts.Save)>
    Public Property Supplier As Supplier
        Get
            Return _supplier
        End Get
        Set(value As Supplier)
            SetPropertyValue("Supplier", _supplier, value)
        End Set
    End Property
    <RuleRequiredField("Rule Required for PurchaseInvoice.Inventory", DefaultContexts.Save)>
    Public Property Inventory As Inventory
        Get
            Return _inventory
        End Get
        Set(value As Inventory)
            SetPropertyValue("Inventory", _inventory, value)
        End Set
    End Property
    Public Property Total As Double
        Get
            Return _total
        End Get
        Set(ByVal value As Double)
            SetPropertyValue("Total", _total, value)
        End Set
    End Property
    Public Property PaymentOutstandingStatus As OutstandingStatus
        Get
            Return _paymentOutstandingStatus
        End Get
        Set(value As OutstandingStatus)
            SetPropertyValue("PaymentOutstandingStatus", _paymentOutstandingStatus, value)
        End Set
    End Property
    <VisibleInDetailView(False), VisibleInListView(False), Browsable(False)>
    Public Property ReturnOutstandingStatus As OutstandingStatus
        Get
            Return _returnOutstandingStatus
        End Get
        Set(value As OutstandingStatus)
            SetPropertyValue("ReturnOutstandingStatus", _returnOutstandingStatus, value)
        End Set
    End Property
    <Association("PurchaseInvoice-PurchaseInvoiceDetail"), DevExpress.Xpo.Aggregated()>
    Public ReadOnly Property Details As XPCollection(Of PurchaseInvoiceDetail)
        Get
            Return GetCollection(Of PurchaseInvoiceDetail)("Details")
        End Get
    End Property
    Public Overrides ReadOnly Property DefaultDisplay As String
        Get
            Return No
        End Get
    End Property
    <Action(autoCommit:=False, Caption:="Recalculate Outstanding Status", _
   confirmationMessage:="Are you really want to recalculate these transactions' PaymentOutstandingStatus?", _
   selectiondependencytype:=MethodActionSelectionDependencyType.RequireMultipleObjects, _
    targetobjectscriteria:="PaymentOutstandingStatus = 'Cleared'", _
   ImageName:="Recalculate")>
    Public Sub UpdatePaymentOutstandingStatus()
        Dim totalQuantity As Double = 0
        Dim totalOutstandingQuantity As Double = 0
        For Each objDetail In Details
            totalQuantity += objDetail.Quantity
            totalOutstandingQuantity += objDetail.PaymentOutstandingQuantity
        Next
        If totalQuantity <> totalOutstandingQuantity Then
            If totalOutstandingQuantity = 0 Then
                PaymentOutstandingStatus = OutstandingStatus.Cleared
            Else
                PaymentOutstandingStatus = OutstandingStatus.PartiallyPaid
            End If
        Else
            PaymentOutstandingStatus = OutstandingStatus.Full
        End If
    End Sub
    <Action(autoCommit:=False, Caption:="Set as clear", _
     confirmationMessage:="Are you sure want to set these transactions' PaymentOutstandingStatus as cleared?", _
     selectiondependencytype:=MethodActionSelectionDependencyType.RequireMultipleObjects, _
     targetobjectscriteria:="PaymentOutstandingStatus <> 'Cleared'", _
     imageName:="SetAsClear")>
    Public Sub SetAsClear()
        PaymentOutstandingStatus = OutstandingStatus.Cleared
    End Sub
    Public Sub UpdateReturnOutstandingStatus()
        Dim totalQuantity As Double = 0
        Dim totalOutstandingQuantity As Double = 0
        For Each objDetail In Details
            totalQuantity += objDetail.Quantity
            totalOutstandingQuantity += objDetail.ReturnOutstandingQuantity
        Next
        If totalQuantity <> totalOutstandingQuantity Then
            If totalOutstandingQuantity = 0 Then
                ReturnOutstandingStatus = OutstandingStatus.Cleared
            Else
                ReturnOutstandingStatus = OutstandingStatus.PartiallyPaid
            End If
        Else
            ReturnOutstandingStatus = OutstandingStatus.Full
        End If
    End Sub
    Protected Overrides Sub OnSubmitted()
        MyBase.OnSubmitted()
        For Each objDetail In Details
            objDetail.InventoryItem = InventoryServices.CreateInventoryItem(Inventory, objDetail.Item, TransDate, objDetail.Quantity, objDetail.UnitPrice)
        Next
    End Sub
    Protected Overrides Sub OnCanceled()
        MyBase.OnCanceled()
        For Each objDetail In Details
            Dim tmp = objDetail.InventoryItem
            objDetail.InventoryItem = Nothing
            InventoryServices.DeleteInventoryItem(tmp)
        Next
    End Sub
End Class
Public Enum OutstandingStatus
    Full
    [PartiallyPaid]
    Cleared
End Enum