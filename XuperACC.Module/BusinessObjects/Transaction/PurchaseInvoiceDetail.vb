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
<CreatableItem(False)>
<Appearance("Appearance Default Disabled for PurchaseInvoiceDetail", enabled:=False, AppearanceItemType:="ViewItem", targetitems:="ReturnedQuantity, PaidQuantity, Total")>
<RuleCombinationOfPropertiesIsUnique("Rule Combination Unique for PurchaseInvoiceDetail", DefaultContexts.Save, "PurchaseInvoice, Item")>
<RuleCriteria("Rule Criteria for PurchaseInvoiceDetail.Total > 0", DefaultContexts.Save, "Total > 0", "Total must be greater than zero")>
<DeferredDeletion(False)>
<DefaultClassOptions()> _
Public Class PurchaseInvoiceDetail
    Inherits AppBase
    Public Sub New(ByVal session As Session)
        MyBase.New(session)

    End Sub

    Public Overrides Sub AfterConstruction()
        MyBase.AfterConstruction()

    End Sub

    Private _sequence As Integer
    Private _purchaseInvoice As PurchaseInvoice
    Private _item As Item
    Private _quantity As Integer
    Private _paidQuantity As Integer
    Private _returnedQuantity As Integer
    Private _unitPrice As Double
    Private _total As Double
    Private _inventoryItem As InventoryItem
    Public Property Sequence As Integer
        Get
            Return _sequence
        End Get
        Set(value As Integer)
            SetPropertyValue("Sequence", _sequence, value)
        End Set
    End Property
    <Association("PurchaseInvoice-PurchaseInvoiceDetail")>
    <RuleRequiredField("Rule Required for PurchaseInvoiceDetail.PurchaseInvoice", DefaultContexts.Save)>
    Public Property PurchaseInvoice As PurchaseInvoice
        Get
            Return _purchaseInvoice
        End Get
        Set(ByVal value As PurchaseInvoice)
            Dim oldValue = PurchaseInvoice
            SetPropertyValue("PurchaseInvoice", _purchaseInvoice, value)
            If Not IsLoading Then
                If PurchaseInvoice IsNot Nothing Then
                    PurchaseInvoice.Total += Total
                    If PurchaseInvoice.Details.Count = 0 Then
                        Sequence = 0
                    Else
                        PurchaseInvoice.Details.Sorting = New SortingCollection(New SortProperty("Sequence", DB.SortingDirection.Ascending))
                        Sequence = PurchaseInvoice.Details(PurchaseInvoice.Details.Count - 1).Sequence + 1
                    End If
                End If
                If oldValue IsNot Nothing Then oldValue.Total -= Total
            End If
        End Set
    End Property
    <RuleRequiredField("Rule Required for PurchaseInvoiceDetail.Item", DefaultContexts.Save)>
    Public Property Item As Item
        Get
            Return _item
        End Get
        Set(ByVal value As Item)
            SetPropertyValue("Item", _item, value)
        End Set
    End Property
    Public Property Quantity As Integer
        Get
            Return _quantity
        End Get
        Set(ByVal value As Integer)
            SetPropertyValue("Quantity", _quantity, value)
            If Not IsLoading Then
                CalculateTotal()
            End If
        End Set
    End Property
    Public Property PaidQuantity As Integer
        Get
            Return _paidQuantity
        End Get
        Set(value As Integer)
            SetPropertyValue("PaidQuantity", _paidQuantity, value)
            If Not IsLoading Then
                PurchaseInvoice.UpdatePaymentOutstandingStatus()
            End If
        End Set
    End Property
    Public Property ReturnedQuantity As Integer
        Get
            Return _returnedQuantity
        End Get
        Set(value As Integer)
            SetPropertyValue("ReturnedQuantity", _returnedQuantity, value)
            If Not IsLoading Then
                PurchaseInvoice.UpdateReturnOutstandingStatus()
            End If
        End Set
    End Property
    <VisibleInDetailView(False), VisibleInListView(False), Browsable(False)>
    Public Property InventoryItem As InventoryItem
        Get
            Return _inventoryItem
        End Get
        Set(value As InventoryItem)
            SetPropertyValue("InventoryItem", _inventoryItem, value)
        End Set
    End Property
    <PersistentAlias("Quantity - PaidQuantity")>
    Public ReadOnly Property PaymentOutstandingQuantity As Integer
        Get
            Return EvaluateAlias("PaymentOutstandingQuantity")
        End Get
    End Property
    <VisibleInDetailView(False), VisibleInListView(False), Browsable(False)>
    <PersistentAlias("Quantity - ReturnedQuantity")>
    Public ReadOnly Property ReturnOutstandingQuantity As Integer
        Get
            Return EvaluateAlias("ReturnOutstandingQuantity")
        End Get
    End Property
    Public Property UnitPrice As Double
        Get
            Return _unitPrice
        End Get
        Set(ByVal value As Double)
            SetPropertyValue("UnitPrice", _unitPrice, value)
            If Not IsLoading Then
                CalculateTotal()
            End If
        End Set
    End Property
    Public Property Total As Double
        Get
            Return _total
        End Get
        Set(ByVal value As Double)
            Dim oldValue = Total
            SetPropertyValue("Total", _total, value)
            If Not IsLoading Then
                If PurchaseInvoice IsNot Nothing Then
                    PurchaseInvoice.Total -= oldValue
                    PurchaseInvoice.Total += Total
                End If
            End If
        End Set
    End Property
    Private Sub CalculateTotal()
        Total = UnitPrice * Quantity
    End Sub
    Public Overrides ReadOnly Property DefaultDisplay As String
        Get
            If PurchaseInvoice Is Nothing OrElse Item Is Nothing Then Return Nothing
            Return PurchaseInvoice.DefaultDisplay & " ~ " & Item.DefaultDisplay
        End Get
    End Property
End Class
