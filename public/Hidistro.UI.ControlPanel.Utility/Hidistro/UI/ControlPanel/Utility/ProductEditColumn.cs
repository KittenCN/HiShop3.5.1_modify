namespace Hidistro.UI.ControlPanel.Utility
{
    using Hidistro.Core;
    using System;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class ProductEditColumn : DataControlField
    {
        private void cell_DataBinding(object sender, EventArgs e)
        {
            TableCell cell = (TableCell) sender;
            GridViewRow namingContainer = (GridViewRow) cell.NamingContainer;
            try
            {
                int num = Convert.ToInt32(DataBinder.Eval(namingContainer.DataItem, "ProductId"));
                string adminAbsolutePath = Globals.GetAdminAbsolutePath(string.Format("/product/EditProduct.aspx?productId={0}", num));
                cell.Text = string.Format("<a href=\"{0}\"><img border=\"0\" src=\"{1}/admin/images/inout.gif\" alt=\"{2}\" /></a>", adminAbsolutePath, Globals.ApplicationPath, "编辑");
            }
            catch
            {
                throw new Exception("Specified DataField was not found.");
            }
        }

        protected override DataControlField CreateField()
        {
            return new ProductEditColumn();
        }

        public override void InitializeCell(DataControlFieldCell cell, DataControlCellType cellType, DataControlRowState rowState, int rowIndex)
        {
            base.InitializeCell(cell, cellType, rowState, rowIndex);
            if (cell == null)
            {
                throw new ArgumentNullException("cell");
            }
            if (cellType == DataControlCellType.DataCell)
            {
                cell.DataBinding += new EventHandler(this.cell_DataBinding);
            }
        }
    }
}

