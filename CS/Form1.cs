using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraGrid.Columns;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        Dictionary<int, int> mergedRows = new Dictionary<int, int>();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            DataTable someDt = new DataTable();
            someDt.Columns.Add("ID", typeof(int));
            someDt.Columns.Add("Name", typeof(string));
            someDt.Columns.Add("Value", typeof(string));

            for (int i = 1; i <= 100; i++) {
                if (i == 3 || i == 8 || i == 9 || i == 12 || i == 16 || i == 17 || i == 15 || i == 32 || i == 44 || i == 60 || i == 15 || i == 93)
                    if (i == 9 || i == 16 || i == 17)
                        if (i == 17)
                            someDt.Rows.Add(new object[] { i - 3, "Name" + (i - 3).ToString(), "Value" + (i - 3).ToString() });
                        else
                            someDt.Rows.Add(new object[] { i - 2, "Name" + (i - 2).ToString(), "Value" + (i - 2).ToString() });
                    else
                        someDt.Rows.Add(new object[] { i - 1, "Name" + (i - 1).ToString(), "Value" + (i - 1).ToString() });
                else
                    someDt.Rows.Add(new object[] { i, "Name" + i.ToString(), "Value" + i.ToString() });
            }

            gridControl1.DataSource = someDt;
            gridView1.OptionsView.AllowCellMerge = true;
            gridControl1.ForceInitialize();
            PrepareListOfMergedRows();
        }

        void PrepareListOfMergedRows()
        {
            int iMergeCellCount = 0;
            GridViewInfo vInfo = gridView1.GetViewInfo() as GridViewInfo;

            int UnCalculatedHeightScale = vInfo.RowsLoadInfo.VisibleRowCount / vInfo.RowsInfo.Count;
            Rectangle allBounds = new Rectangle(vInfo.Bounds.X, vInfo.Bounds.Y, vInfo.Bounds.Width, vInfo.Bounds.Height * UnCalculatedHeightScale);
            Rectangle initialBounds = vInfo.Bounds;

            vInfo.Calc(gridControl1.CreateGraphics(), allBounds);
            for (int i = 0; i < vInfo.RowsInfo.Count; i++)
            {
                GridDataRowInfo rowInfo = vInfo.RowsInfo[i] as GridDataRowInfo;
                if (rowInfo != null)
                {                       
                    GridCellInfo cInfo = rowInfo.Cells[gridView1.Columns["ID"]];
                    if (!cInfo.IsMerged || cInfo.MergedCell.FirstCell == cInfo)
                    {
                        mergedRows.Add(i, iMergeCellCount - 1);
                    }
                    else
                    {
                        mergedRows.Add(i, iMergeCellCount);
                        iMergeCellCount++;                        
                    }
                    mergedRows[i]++;
                }
            }
            vInfo.Calc(gridControl1.CreateGraphics(), initialBounds);
        }

        private void gridView1_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            int mergedCellCountForCurrenRH = 0;
            if (mergedRows.Count > 0)
            {
                mergedCellCountForCurrenRH = mergedRows[e.RowHandle];
            }
            if ((e.RowHandle + mergedCellCountForCurrenRH) % 2 == 0)
            {
                e.Appearance.BackColor = Color.AliceBlue;
;
            }
        }
    }
}
