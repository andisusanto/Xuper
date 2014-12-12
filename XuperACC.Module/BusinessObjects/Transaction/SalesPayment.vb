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

<RuleCriteria("Rule Criteria for SalesPayment.Total > 0", DefaultContexts.Save, "Total > 0")>
<Appearance("Appearance Default Disabled for SalesPayment", enabled:=False, AppearanceItemType:="ViewItem", targetitems:="Total")>
<Appearance("Appearance for SalesPayment.EnableDetails = FALSE", AppearanceItemType:="ViewItem", criteria:="EnableDetails = FALSE", enabled:=False, targetitems:="Details")>
<Appearance("Appearance for SalesPayment.Details.Count > 0", AppearanceItemType:="ViewItem", criteria:="@Details.Count > 0", enabled:=False, targetitems:="Customer")>
<DeferredDeletion(False)>
<DefaultClassOptions()> _
Public Class SalesPayment
    Inherits TransactionBase
    Public Sub New(ByVal session As Session)
        MyBase.New(session)

    End Sub

    Public Overrides Sub AfterConstruction()
        MyBase.AfterConstruction()
        TransDate = Today
    End Sub
    Private _no As String
    Private _transDate As Date
    Private _Customer As Customer
    Private _total As Double
    Private _creditNoteAmount As Double
    Private _remainingAmount As Double

    <RuleUniqueValue("Rule Unique for SalesPayment.No", DefaultContexts.Save)>
    <RuleRequiredField("Rule Required for SalesPayment.No", DefaultContexts.Save)>
    Public Property No As String
        Get
            Return _no
        End Get
        Set(value As String)
            SetPropertyValue("No", _no, value)
        End Set
    End Property
    <RuleRequiredField("Rule Required for SalesPayment.TransDate", DefaultContexts.Save)>
    Public Property TransDate As Date
        Get
            Return _transDate
        End Get
        Set(value As Date)
            SetPropertyValue("TransDate", _transDate, value)
        End Set
    End Property
    <ImmediatePostData(True)>
    <RuleRequiredField("Rule Required for SalesPayment.Customer", DefaultContexts.Save)>
    Public Property Customer As Customer
        Get
            Return _Customer
        End Get
        Set(value As Customer)
            SetPropertyValue("Customer", _Customer, value)
        End Set
    End Property
    Public Property Total As Double
        Get
            Return _total
        End Get
        Set(value As Double)
            SetPropertyValue("Total", _total, value)
            If Not IsLoading Then
                CalculateRemainingAmount()
            End If
        End Set
    End Property
    Public Property CreditNoteAmount As Double
        Get
            Return _creditNoteAmount
        End Get
        Set(ByVal value As Double)
            SetPropertyValue("CreditNoteAmount", _creditNoteAmount, value)
            If Not IsLoading Then
                CalculateRemainingAmount()
            End If
        End Set
    End Property
    Public Property RemainingAmount As Double
        Get
            Return _remainingAmount
        End Get
        Set(ByVal value As Double)
            SetPropertyValue("RemainingAmount", _remainingAmount, value)
        End Set
    End Property
    Private Sub CalculateRemainingAmount()
        RemainingAmount = Total - CreditNoteAmount
    End Sub
    <Association("SalesPayment-SalesPaymentDetail"), DevExpress.Xpo.Aggregated()>
    Public ReadOnly Property Details As XPCollection(Of SalesPaymentDetail)
        Get
            Return GetCollection(Of SalesPaymentDetail)("Details")
        End Get
    End Property
    <Association("SalesPayment-SalesPaymentCreditNote"), DevExpress.Xpo.Aggregated()>
    Public ReadOnly Property CreditNotes As XPCollection(Of SalesPaymentCreditNote)
        Get
            Return GetCollection(Of SalesPaymentCreditNote)("CreditNotes")
        End Get
    End Property
    Public Overrides ReadOnly Property DefaultDisplay As String
        Get
            Return No
        End Get
    End Property
    <Browsable(False), VisibleInDetailView(False), VisibleInListView(False)>
    Public ReadOnly Property EnableDetails As Boolean
        Get
            Return Customer IsNot Nothing
        End Get
    End Property
    Protected Overrides Sub OnSubmitted()
        MyBase.OnSubmitted()
        For Each objDetail In Details
            If objDetail.SalesInvoiceDetail.SalesInvoice.Status <> TransactionStatus.Submitted Then Throw New Exception(String.Format("Sales Invoice with No {0} has not been submitted", objDetail.SalesInvoiceDetail.SalesInvoice.No))
            If objDetail.SalesInvoiceDetail.SalesInvoice.PaymentOutstandingStatus = OutstandingStatus.Cleared Then Throw New Exception(String.Format("Sales Invoice with No {0} already set as cleared", objDetail.SalesInvoiceDetail.SalesInvoice.No))
            If objDetail.SalesInvoiceDetail.PaymentOutstandingQuantity < objDetail.Quantity Then Throw New Exception(String.Format("Invalid Quantity for submitting Invoice. Invalid line : {0}", objDetail.ToString))
            objDetail.SalesInvoiceDetail.PaidQuantity += objDetail.Quantity
        Next
        For Each obj In CreditNotes
            If obj.CreditNote.RemainingAmount < obj.Amount Then Throw New Exception(String.Format("Credit note with no {0} has no enough balance", obj.CreditNote.No))
            obj.CreditNote.UsedAmount += obj.Amount
        Next
    End Sub
    Protected Overrides Sub OnCanceled()
        MyBase.OnCanceled()
        For Each objDetail In Details
            objDetail.SalesInvoiceDetail.PaidQuantity -= objDetail.Quantity
        Next
        For Each obj In CreditNotes
            obj.CreditNote.UsedAmount -= obj.Amount
        Next
    End Sub
End Class
