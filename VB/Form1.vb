Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Linq
Imports System.Text
Imports System.Windows.Forms
Imports DevExpress.XtraGrid.Views.Grid.ViewInfo
Imports DevExpress.XtraGrid.Columns

Namespace WindowsFormsApplication1
	Partial Public Class Form1
		Inherits Form
		Private mergedRows As New Dictionary(Of Integer, Integer)()
		Public Sub New()
			InitializeComponent()
		End Sub

		Private Sub Form1_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
			Dim someDt As New DataTable()
			someDt.Columns.Add("ID", GetType(Integer))
			someDt.Columns.Add("Name", GetType(String))
			someDt.Columns.Add("Value", GetType(String))

			For i As Integer = 1 To 100
				If i = 3 OrElse i = 8 OrElse i = 9 OrElse i = 12 OrElse i = 16 OrElse i = 17 OrElse i = 15 OrElse i = 32 OrElse i = 44 OrElse i = 60 OrElse i = 15 OrElse i = 93 Then
					If i = 9 OrElse i = 16 OrElse i = 17 Then
						If i = 17 Then
							someDt.Rows.Add(New Object() { i - 3, "Name" & (i - 3).ToString(), "Value" & (i - 3).ToString() })
						Else
							someDt.Rows.Add(New Object() { i - 2, "Name" & (i - 2).ToString(), "Value" & (i - 2).ToString() })
						End If
					Else
						someDt.Rows.Add(New Object() { i - 1, "Name" & (i - 1).ToString(), "Value" & (i - 1).ToString() })
					End If
				Else
					someDt.Rows.Add(New Object() { i, "Name" & i.ToString(), "Value" & i.ToString() })
				End If
			Next i

			gridControl1.DataSource = someDt
			gridView1.OptionsView.AllowCellMerge = True
			gridControl1.ForceInitialize()
			PrepareListOfMergedRows()
		End Sub

		Private Sub PrepareListOfMergedRows()
			Dim iMergeCellCount As Integer = 0
			Dim vInfo As GridViewInfo = TryCast(gridView1.GetViewInfo(), GridViewInfo)

			Dim UnCalculatedHeightScale As Integer = vInfo.RowsLoadInfo.VisibleRowCount / vInfo.RowsInfo.Count
			Dim allBounds As New Rectangle(vInfo.Bounds.X, vInfo.Bounds.Y, vInfo.Bounds.Width, vInfo.Bounds.Height * UnCalculatedHeightScale)
			Dim initialBounds As Rectangle = vInfo.Bounds

			vInfo.Calc(gridControl1.CreateGraphics(), allBounds)
			For i As Integer = 0 To vInfo.RowsInfo.Count - 1
				Dim rowInfo As GridDataRowInfo = TryCast(vInfo.RowsInfo(i), GridDataRowInfo)
				If rowInfo IsNot Nothing Then
					Dim cInfo As GridCellInfo = rowInfo.Cells(gridView1.Columns("ID"))
					If (Not cInfo.IsMerged) OrElse cInfo.MergedCell.FirstCell Is cInfo Then
						mergedRows.Add(i, iMergeCellCount - 1)
					Else
						mergedRows.Add(i, iMergeCellCount)
						iMergeCellCount += 1
					End If
					mergedRows(i) += 1
				End If
			Next i
			vInfo.Calc(gridControl1.CreateGraphics(), initialBounds)
		End Sub

		Private Sub gridView1_RowCellStyle(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs) Handles gridView1.RowCellStyle
			Dim mergedCellCountForCurrenRH As Integer = 0
			If mergedRows.Count > 0 Then
				mergedCellCountForCurrenRH = mergedRows(e.RowHandle)
			End If
			If (e.RowHandle + mergedCellCountForCurrenRH) Mod 2 = 0 Then
				e.Appearance.BackColor = Color.AliceBlue

			End If
		End Sub
	End Class
End Namespace
