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
<Appearance("Appearance Default Disabled for SalesInvoiceDetail", enabled:=False, AppearanceItemType:="ViewItem", targetitems:="ReturnedQuantity, PaidQuantity, Total, Discount, GrandTotal")>
<RuleCombinationOfPropertiesIsUnique("Rule Combination Unique for SalesInvoiceDetail", DefaultContexts.Save, "SalesInvoice, Item")>
<RuleCriteria("Rule Criteria for SalesInvoiceDetail.Total > 0", DefaultContexts.Save, "Total > 0", "Total must be greater than zero")>
<DeferredDeletion(False)>
<DefaultClassOptions()> _
Public Class SalesInvoiceDetail
    Inherits AppBase
    Public Sub New(ByVal session As Session)
        MyBase.New(session)

    End Sub

    Public Overrides Sub AfterConstruction()
        MyBase.AfterConstruction()

    End Sub

    Private _sequence As Integer
    Private _salesInvoice As SalesInvoice
    Private _item As Item
    Private _quantity As Integer
    Private _paidQuantity As Integer
    Private _returnedQuantity As Integer
    Private _unitPrice As Double
    Private _total As Double
    Private _discountType As DiscountType
    Private _discountValue As Double
    Private _discount As Double
    Private _grandTotal As Double
    Private _inventoryItemDeductTransaction As InventoryItemDeductTransaction
    Public Property Sequence As Integer
        Get
            Return _sequence
        End Get
        Set(value As Integer)
            SetPropertyValue("Sequence", _sequence, value)
        End Set
    End Property
    <Association("SalesInvoice-SalesInvoiceDetail")>
    <RuleRequiredField("Rule Required for SalesInvoiceDetail.SalesInvoice", DefaultContexts.Save)>
    Public Property SalesInvoice As SalesInvoice
        Get
            Return _salesInvoice
        End Get
        Set(ByVal value As SalesInvoice)
            Dim oldValue = SalesInvoice
            SetPropertyValue("SalesInvoice", _salesInvoice, value)
            If Not IsLoading Then
                If SalesInvoice IsNot Nothing Then
                    SalesInvoice.Total += GrandTotal
                    If SalesInvoice.Details.Count = 0 Then
                        Sequence = 0
                    Else
                        SalesInvoice.Details.Sorting = New SortingCollection(New SortProperty("Sequence", DB.SortingDirection.Ascending))
                        Sequence = SalesInvoice.Details(SalesInvoice.Details.Count - 1).Sequence + 1
                    End If
                End If
                If oldValue IsNot Nothing Then oldValue.Total -= GrandTotal
            End If
        End Set
    End Property
    <RuleRequiredField("Rule Required for SalesInvoiceDetail.Item", DefaultContexts.Save)>
    Public Property Item As Item
        Get
            Return _item
        End Get
        Set(ByVal value As Item)
            SetPropertyValue("Item", _item, value)
            If Not IsLoading Then
                UnitPrice = Item.DefaultPrice
            End If
        End Set
    End Property
    <ImmediatePostData(True)>
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
                SalesInvoice.UpdatePaymentOutstandingStatus()
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
                SalesInvoice.UpdateReturnOutstandingStatus()
            End If
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
    <ImmediatePostData(True)>
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
            SetPropertyValue("Total", _total, value)
            If Not IsLoading Then
                CalculateDiscount()
            End If
        End Set
    End Property
    <ImmediatePostData(True)>
    Public Property DiscountType As DiscountType
        Get
            Return _discountType
        End Get
        Set(ByVal value As DiscountType)
            SetPropertyValue("DiscountType", _discountType, value)
            If Not IsLoading Then
                CalculateDiscount()
            End If
        End Set
    End Property
    <ImmediatePostData(True)>
    <RuleRange(0, 100, targetcriteria:="DiscountType = 'ByPercentage'")>
    Public Property DiscountValue As Double
        Get
            Return _discountValue
        End Get
        Set(ByVal value As Double)
            SetPropertyValue("DiscountValue", _discountValue, value)
            If Not IsLoading Then
                CalculateDiscount()
            End If
        End Set
    End Property
    <ImmediatePostData(True)>
    Public Property Discount As Double
        Get
            Return _discount
        End Get
        Set(ByVal value As Double)
            SetPropertyValue("Discount", _discount, value)
            If Not IsLoading Then
                CalculateGrandTotal()
            End If
        End Set
    End Property
    Public Property GrandTotal As Double
        Get
            Return _grandTotal
        End Get
        Set(ByVal value As Double)
            Dim oldValue = GrandTotal
            SetPropertyValue("GrandTotal", _grandTotal, value)
            If Not IsLoading Then
                If SalesInvoice IsNot Nothing Then
                    SalesInvoice.Total -= oldValue
                    SalesInvoice.Total += GrandTotal
                End If
            End If
        End Set
    End Property

    <VisibleInDetailView(False), VisibleInListView(False), Browsable(False)>
    Public Property InventoryItemDeductTransaction As InventoryItemDeductTransaction
        Get
            Return _inventoryItemDeductTransaction
        End Get
        Set(value As InventoryItemDeductTransaction)
            SetPropertyValue("InventoryItemDeductTransaction", _inventoryItemDeductTransaction, value)
        End Set
    End Property
    Private Sub CalculateTotal()
        Total = UnitPrice * Quantity
    End Sub
    Private Sub CalculateDiscount()
        Select Case DiscountType
            Case [Module].DiscountType.ByAmount
                Discount = DiscountValue
            Case [Module].DiscountType.ByPercentage
                Discount = Total * DiscountValue / 100
        End Select
    End Sub
    Private Sub CalculateGrandTotal()
        GrandTotal = Total - Discount
    End Sub
    Public Overrides ReadOnly Property DefaultDisplay As String
        Get
            If SalesInvoice Is Nothing OrElse Item Is Nothing Then Return Nothing
            Return SalesInvoice.DefaultDisplay & " ~ " & Item.DefaultDisplay
        End Get
    End Property
End Class
Public Enum DiscountType
    ByAmount
    ByPercentage
End Enum