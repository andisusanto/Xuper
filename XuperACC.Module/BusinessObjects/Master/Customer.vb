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
<DeferredDeletion(False)>
<DefaultClassOptions()> _
Public Class Customer
    Inherits AppBase
    Public Sub New(ByVal session As Session)
        MyBase.New(session)

    End Sub

    Public Overrides Sub AfterConstruction()
        MyBase.AfterConstruction()
        Active = True
    End Sub

    Private fCode As String
    Private fName As String
    Private fLocation As Location
    Private fActive As Boolean
    <RuleRequiredField("Rule Required for Customer.Code", DefaultContexts.Save)>
    <RuleUniqueValue("Rule Unique for Customer.Code", DefaultContexts.Save)>
    Public Property Code As String
        Get
            Return fCode
        End Get
        Set(ByVal value As String)
            SetPropertyValue("Code", fCode, value)
        End Set
    End Property
    <RuleRequiredField("Rule Required for Customer.Name", DefaultContexts.Save)>
    Public Property Name As String
        Get
            Return fName
        End Get
        Set(ByVal value As String)
            SetPropertyValue("Name", fName, value)
        End Set
    End Property
    <DataSourceCriteria("Active = TRUE")>
    Public Property Location As Location
        Get
            Return fLocation
        End Get
        Set(value As Location)
            SetPropertyValue("Location", fLocation, value)
        End Set
    End Property
    Public Property Active As Boolean
        Get
            Return fActive
        End Get
        Set(ByVal value As Boolean)
            SetPropertyValue("Active", fActive, value)
        End Set
    End Property
    <Association("Customer-CreditNote")>
    Public ReadOnly Property CreditNotes As XPCollection(Of CreditNote)
        Get
            Return GetCollection(Of CreditNote)("CreditNotes")
        End Get
    End Property
    Public Overrides ReadOnly Property DefaultDisplay As String
        Get
            Return Name
        End Get
    End Property
End Class
