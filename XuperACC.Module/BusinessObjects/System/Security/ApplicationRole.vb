Imports System
Imports System.ComponentModel

Imports DevExpress.Xpo
Imports DevExpress.Data.Filtering

Imports DevExpress.ExpressApp
Imports DevExpress.Persistent.Base
Imports DevExpress.Persistent.BaseImpl
Imports DevExpress.Persistent.Validation
Imports DevExpress.Persistent.Base.Security

<DefaultProperty("RoleName")>
<DeferredDeletion(False)>
<DefaultClassOptions()> _
Public Class ApplicationRole
    Inherits RoleBase
    Implements DevExpress.Persistent.Base.Security.IRole

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

    <Association("ApplicationUser-ApplicationRole", GetType(ApplicationUser))>
    Public ReadOnly Property Users As XPCollection(Of ApplicationUser)
        Get
            Return GetCollection(Of ApplicationUser)("Users")
        End Get
    End Property

    <VisibleInDetailView(False), VisibleInListView(False), Browsable(False)>
    Public Property RoleName As String Implements DevExpress.Persistent.Base.Security.IRole.Name
        Get
            Return Name
        End Get
        Set(ByVal value As String)
            SetPropertyValue("RoleName", Name, value)
        End Set
    End Property

    <VisibleInDetailView(False), VisibleInListView(False), Browsable(False)>
    Public ReadOnly Property IPermissions As System.Collections.ObjectModel.ReadOnlyCollection(Of System.Security.IPermission) Implements DevExpress.Persistent.Base.Security.IRole.Permissions
        Get
            Return Permissions
        End Get
    End Property

    <VisibleInDetailView(False), VisibleInListView(False), Browsable(False)>
    Public ReadOnly Property IUsers As System.Collections.Generic.IList(Of DevExpress.Persistent.Base.Security.IUser) Implements DevExpress.Persistent.Base.Security.IRole.Users
        Get
            Return New ListConverter(Of IUser, ApplicationUser)(Users)
        End Get
    End Property
End Class
