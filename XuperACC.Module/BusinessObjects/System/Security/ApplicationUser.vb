Imports System
Imports System.ComponentModel

Imports DevExpress.Xpo
Imports DevExpress.Data.Filtering

Imports DevExpress.ExpressApp
Imports DevExpress.Persistent.Base
Imports DevExpress.Persistent.BaseImpl
Imports DevExpress.Persistent.Validation
Imports DevExpress.Persistent.Base.Security
Imports System.Security
<DeferredDeletion(False)>
<DefaultClassOptions()> _
Public Class ApplicationUser
    Inherits AppBase
    Implements DevExpress.Persistent.Base.Security.IUserWithRoles, DevExpress.Persistent.Base.Security.IAuthenticationStandardUser

    Public Sub New(ByVal session As Session)
        MyBase.New(session)
        ' This constructor is used when an object is loaded from a persistent storage.
        ' Do not place any code here or place it only when the IsLoading property is false:
        ' if (!IsLoading){
        '   It is now OK to place your initialization code here.
        ' }
        ' or as an alternative, move your initialization code into the AfterConstruction method.
        If user Is Nothing Then user = New UserImpl(Me)
    End Sub
    Public Overrides Sub AfterConstruction()
        MyBase.AfterConstruction()
        user = New UserImpl(Me)
        ChangePasswordAfterLogon = True
        IsActive = True
        ' Place here your initialization code.
    End Sub
    Private user As UserImpl
    <NonCloneable()>
    <RuleRequiredField("Rule Required for ApplicationUser.UserName", DefaultContexts.Save)>
     <RuleUniqueValue("Rule Unique for ApplicationUser.UserName", DefaultContexts.Save)>
     <Size(80)>
    Public Property UserName As String
        Get
            Return user.UserName
        End Get
        Set(ByVal value As String)
            SetPropertyValue("UserName", user.UserName, value)
        End Set
    End Property
    <NonCloneable()>
    <VisibleInDetailView(False), VisibleInListView(False), Browsable(False)>
    Public Property StoredPassword As String
        Get
            Return user.StoredPassword
        End Get
        Set(ByVal value As String)
            SetPropertyValue("StoredPassword", user.StoredPassword, value)
        End Set
    End Property
    <NonCloneable()>
    Public Property ChangePasswordAfterLogon As Boolean Implements DevExpress.Persistent.Base.Security.IAuthenticationStandardUser.ChangePasswordOnFirstLogon
        Get
            Return user.ChangePasswordAfterLogon
        End Get
        Set(ByVal value As Boolean)
            SetPropertyValue("ChangePasswordOnAfterLogon", user.ChangePasswordAfterLogon, value)
        End Set
    End Property

    <Association("ApplicationUser-ApplicationRole", GetType(ApplicationRole))>
    Public ReadOnly Property Roles As XPCollection(Of ApplicationRole)
        Get
            Return GetCollection(Of ApplicationRole)("Roles")
        End Get
    End Property

    Public Function ComparePassword(ByVal password As String) As Boolean Implements DevExpress.Persistent.Base.Security.IAuthenticationStandardUser.ComparePassword
        Return user.ComparePassword(password)
    End Function

    Public Sub SetPassword(ByVal password As String) Implements DevExpress.Persistent.Base.Security.IAuthenticationStandardUser.SetPassword
        user.SetPassword(password)
    End Sub
    <VisibleInDetailView(False), VisibleInListView(False), Browsable(False)>
    Public ReadOnly Property IUserName As String Implements DevExpress.Persistent.Base.Security.IAuthenticationStandardUser.UserName, DevExpress.Persistent.Base.Security.IUser.UserName
        Get
            Return UserName
        End Get
    End Property

    Public Property IsActive As Boolean Implements DevExpress.Persistent.Base.Security.IUser.IsActive
        Get
            Return user.IsActive
        End Get
        Set(ByVal value As Boolean)
            SetPropertyValue("IsActive", user.IsActive, value)
        End Set
    End Property

    Public ReadOnly Property Permissions As System.Collections.Generic.IList(Of System.Security.IPermission) Implements DevExpress.Persistent.Base.Security.IUser.Permissions
        Get
            Dim permission As New List(Of IPermission)()
            For Each appRole In Roles
                permission.AddRange(appRole.Permissions)
            Next
            Return permission.AsReadOnly()
        End Get
    End Property

    Public Sub ReloadPermissions() Implements DevExpress.Persistent.Base.Security.IUser.ReloadPermissions
        For Each appRole In Roles
            appRole.PersistentPermissions.Reload()
        Next
    End Sub

    <VisibleInDetailView(False), VisibleInListView(False), Browsable(False)>
    Public ReadOnly Property IRoles As System.Collections.Generic.IList(Of DevExpress.Persistent.Base.Security.IRole) Implements DevExpress.Persistent.Base.Security.IUserWithRoles.Roles
        Get
            Return New ListConverter(Of IRole, ApplicationRole)(Roles)
        End Get
    End Property
    Public Overrides ReadOnly Property DefaultDisplay As String
        Get
            Return UserName
        End Get
    End Property
End Class
