Imports Microsoft.VisualBasic
Imports System
Imports System.Linq
Imports DevExpress.Xpo
Imports DevExpress.ExpressApp
Imports DevExpress.Data.Filtering
Imports DevExpress.ExpressApp.Xpo
Imports DevExpress.Persistent.Base
Imports DevExpress.ExpressApp.Updating
Imports DevExpress.Persistent.BaseImpl
Imports DevExpress.ExpressApp.Security
'Imports DevExpress.ExpressApp.Reports
'Imports DevExpress.ExpressApp.PivotChart
'Imports DevExpress.ExpressApp.Security.Strategy
'Imports XuperACC.Module.BusinessObjects

' Allows you to handle a database update when the application version changes (http://documentation.devexpress.com/#Xaf/clsDevExpressExpressAppUpdatingModuleUpdatertopic help article for more details).
Public Class Updater
    Inherits ModuleUpdater
    Public Sub New(ByVal objectSpace As IObjectSpace, ByVal currentDBVersion As Version)
        MyBase.New(objectSpace, currentDBVersion)
    End Sub

    ' Override to specify the database update code which should be performed after the database schema is updated (http://documentation.devexpress.com/#Xaf/DevExpressExpressAppUpdatingModuleUpdater_UpdateDatabaseAfterUpdateSchematopic).
    Public Overrides Sub UpdateDatabaseAfterUpdateSchema()
        MyBase.UpdateDatabaseAfterUpdateSchema()
        Dim DefaultRoleName As String = "System Administrator"
        Dim DefaultAdministratorName As String = "Andi"
        Dim objSpace = ObjectSpace
        Dim defaultrole As ApplicationRole = objSpace.FindObject(Of ApplicationRole)(New BinaryOperator("RoleName", DefaultRoleName))
        If defaultrole Is Nothing Then
            defaultrole = objSpace.CreateObject(Of ApplicationRole)()
            defaultrole.RoleName = DefaultRoleName
            Do While defaultrole.PersistentPermissions.Count > 0
                ObjectSpace.Delete(defaultrole.PersistentPermissions(0))
            Loop
            defaultrole.AddPermission(New ObjectAccessPermission(GetType(Object), ObjectAccess.AllAccess))
            defaultrole.AddPermission(New EditModelPermission(ModelAccessModifier.Allow))
        End If
        Dim defaultuser As ApplicationUser = objSpace.FindObject(Of ApplicationUser)(New BinaryOperator("UserName", DefaultAdministratorName))
        If defaultuser Is Nothing Then
            defaultuser = objSpace.CreateObject(Of ApplicationUser)()
            defaultuser.ChangePasswordAfterLogon = False
            defaultuser.UserName = "Andi"
            defaultuser.SetPassword("123456")
            For i = 0 To defaultuser.Roles.Count - 1
                defaultuser.Roles.Remove(defaultuser.Roles(0))
            Next
            defaultuser.Roles.Add(defaultrole)
        End If
        Dim Session As Session = CType(objSpace, XPObjectSpace).Session
        Report.Module.ReportConfiguration.GetInstance(Session)
        CompanyInformation.GetInstance(Session)
        SystemConfig.GetInstance(Session)
        Dim objDebitNoteAutoNo As AutoNo = objSpace.FindObject(Of AutoNo)(New BinaryOperator("TargetType", "XuperACC.Module.DebitNote"))
        If objDebitNoteAutoNo Is Nothing Then
            objDebitNoteAutoNo = objSpace.CreateObject(Of AutoNo)()
            objDebitNoteAutoNo.TargetType = "XuperACC.Module.DebitNote"
            objDebitNoteAutoNo.TargetProperty = "No"
            objDebitNoteAutoNo.TotalDigit = "4"
            objDebitNoteAutoNo.Increment = 1
            objDebitNoteAutoNo.StartNumber = 1
            objDebitNoteAutoNo.Description = "Auto no for Debit Note"
            objDebitNoteAutoNo.IsActive = True
            objDebitNoteAutoNo.LookUpCriteria = "Return prm1.TransDate.ToString(""yyyy/MM"")"
            objDebitNoteAutoNo.Suffix = "Return ""DN/""  prm1.TransDate.ToString(""yyyy/MM/"")"
        End If
        objSpace.CommitChanges()
        Dim objCreditNoteAutoNo As AutoNo = objSpace.FindObject(Of AutoNo)(New BinaryOperator("TargetType", "XuperACC.Module.CreditNote"))
        If objCreditNoteAutoNo Is Nothing Then
            objCreditNoteAutoNo = objSpace.CreateObject(Of AutoNo)()
            objCreditNoteAutoNo.TargetType = "XuperACC.Module.CreditNote"
            objCreditNoteAutoNo.TargetProperty = "No"
            objCreditNoteAutoNo.TotalDigit = "4"
            objCreditNoteAutoNo.Increment = 1
            objCreditNoteAutoNo.StartNumber = 1
            objCreditNoteAutoNo.Description = "Auto no for Credit Note"
            objCreditNoteAutoNo.IsActive = True
            objCreditNoteAutoNo.LookUpCriteria = "Return prm1.TransDate.ToString(""yyyy/MM"")"
            objCreditNoteAutoNo.Suffix = "Return ""CN/""  prm1.TransDate.ToString(""yyyy/MM/"")"
        End If
        objSpace.CommitChanges()
    End Sub
    ' Override to perform the required changes with the database structure before the database schema is updated (http://documentation.devexpress.com/#Xaf/DevExpressExpressAppUpdatingModuleUpdater_UpdateDatabaseBeforeUpdateSchematopic).
    Public Overrides Sub UpdateDatabaseBeforeUpdateSchema()
        MyBase.UpdateDatabaseBeforeUpdateSchema()
        ' Example:
        'If (CurrentDBVersion < New Version("1.1.0.0") AndAlso CurrentDBVersion > New Version("0.0.0.0")) Then
        '    RenameColumn("DomainObject1Table", "OldColumnName", "NewColumnName")
        'End If
    End Sub
End Class
