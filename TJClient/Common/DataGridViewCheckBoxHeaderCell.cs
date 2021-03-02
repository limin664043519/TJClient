using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace FBYClient
{
    public delegate void DataGridViewCheckBoxHeaderEventHander(object sender, datagridviewCheckboxHeaderEventArgs e);
    public delegate void DataGridViewButtonEventHander(object sender, EventArgs e);




    public class datagridviewCheckboxHeaderEventArgs : EventArgs
    {
        private bool checkedState = false;

        public bool CheckedState
        {
            get { return checkedState; }
            set { checkedState = value; }
        }
    }

    public class DataGridViewCheckBoxHeaderCell : DataGridViewColumnHeaderCell
    {
        Point checkBoxLocation;
        Size checkBoxSize;
        bool _checked = false;
        Point _cellLocation = new Point();
        System.Windows.Forms.VisualStyles.CheckBoxState _cbState =
            System.Windows.Forms.VisualStyles.CheckBoxState.UncheckedNormal;
        public event DataGridViewCheckBoxHeaderEventHander OnCheckBoxClicked;

        protected override void Paint(
            Graphics graphics,
            Rectangle clipBounds,
            Rectangle cellBounds,
            int rowIndex,
            DataGridViewElementStates dataGridViewElementState,
            object value,
            object formattedValue,
            string errorText,
            DataGridViewCellStyle cellStyle,
            DataGridViewAdvancedBorderStyle advancedBorderStyle,
            DataGridViewPaintParts paintParts)
        {
            base.Paint(graphics, clipBounds, cellBounds, rowIndex,
                dataGridViewElementState, value,
                formattedValue, errorText, cellStyle,
                advancedBorderStyle, paintParts);

            Point p = new Point();
            Size s = CheckBoxRenderer.GetGlyphSize(graphics, CheckBoxState.UncheckedNormal);

            p.X = cellBounds.Location.X +
                (cellBounds.Width / 2) - (s.Width / 2) - 1;
            p.Y = cellBounds.Location.Y +
                (cellBounds.Height / 2) - (s.Height / 2);

            _cellLocation = cellBounds.Location;
            checkBoxLocation = p;
            checkBoxSize = s;
            if (_checked)
                _cbState = System.Windows.Forms.VisualStyles.
                    CheckBoxState.CheckedNormal;
            else
                _cbState = System.Windows.Forms.VisualStyles.
                    CheckBoxState.UncheckedNormal;

            CheckBoxRenderer.DrawCheckBox
            (graphics, checkBoxLocation, _cbState);
        }

        protected override void OnMouseClick(DataGridViewCellMouseEventArgs e)
        {
            Point p = new Point(e.X + _cellLocation.X, e.Y + _cellLocation.Y);
            if (p.X >= checkBoxLocation.X && p.X <=
                checkBoxLocation.X + checkBoxSize.Width
            && p.Y >= checkBoxLocation.Y && p.Y <=
                checkBoxLocation.Y + checkBoxSize.Height)
            {
                _checked = !_checked;

                datagridviewCheckboxHeaderEventArgs ex = new datagridviewCheckboxHeaderEventArgs();
                ex.CheckedState = _checked;

                object sender = new object();
                //sender=
                if (OnCheckBoxClicked != null)
                {
                    OnCheckBoxClicked(sender, ex);
                    this.DataGridView.InvalidateCell(this);
                }
            }
            base.OnMouseClick(e);
        }
    }


    //删除
    public class DataGridViewButtonColumnDel : DataGridViewColumn
    {
        public DataGridViewButtonColumnDel()
        {
            this.CellTemplate = new DataGridViewButtonCellDel(); this.HeaderText = "button";
        }
    }

    public class DataGridViewButtonCellDel : DataGridViewButtonCell
    {
        protected override void Paint(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex,DataGridViewElementStates cellState, object value,object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
        {
            base.Paint(graphics, clipBounds, cellBounds, rowIndex, cellState, value, formattedValue, errorText, cellStyle, advancedBorderStyle, paintParts);
            Image _img = global::TJClient.Properties.Resources.login_btn2_2;
            graphics.DrawImage(_img, cellBounds.Location.X + 5, cellBounds.Location.Y + 3, _img.Width, _img.Height);

        }
    }

    //public class DataGridViewButtonCellEdi : DataGridViewButtonCell
    //{
    //    protected override void Paint(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex, DataGridViewElementStates cellState, object value, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
    //    {
    //        base.Paint(graphics, clipBounds, cellBounds, rowIndex, cellState, value, formattedValue, errorText, cellStyle, advancedBorderStyle, paintParts);
    //        Image _img = Properties.Resources.btn_save;
    //        graphics.DrawImage(_img, cellBounds.Location.X + 5, cellBounds.Location.Y + 3, _img.Width, _img.Height);
    //    }
    //}


    //public class DataGridViewButtonCellEdi : DataGridViewButtonCell
    //{
    //    Point buttonLocation;
    //    Size buttonSize;
    //    bool _checked = false;
    //    Point _cellLocation = new Point();
    //    //System.Windows.Forms.VisualStyles.CheckBoxState _cbState =
    //    //    System.Windows.Forms.VisualStyles.CheckBoxState.UncheckedNormal;
    //    public event DataGridViewButtonEventHander OnButtonClicked;
    //    protected override void Paint(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex, DataGridViewElementStates cellState, object value, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
    //    {
    //        //base.Paint(graphics, clipBounds, cellBounds, rowIndex, cellState, value, formattedValue, errorText, cellStyle, advancedBorderStyle, paintParts);
    //        //Image _img = Properties.Resources.btn_save;
    //        //graphics.DrawImage(_img, cellBounds.Location.X + 5, cellBounds.Location.Y + 3, _img.Width, _img.Height);

    //    //     Graphics graphics,
    //    //    Rectangle clipBounds,
    //    //    Rectangle cellBounds,
    //    //    int rowIndex,
    //    //    DataGridViewElementStates dataGridViewElementState,
    //    //    object value,
    //    //    object formattedValue,
    //    //    string errorText,
    //    //    DataGridViewCellStyle cellStyle,
    //    //    DataGridViewAdvancedBorderStyle advancedBorderStyle,
    //    //    DataGridViewPaintParts paintParts)
    //    //{
    //        //base.Paint(graphics, clipBounds, cellBounds, rowIndex,
    //        //    dataGridViewElementState, value,
    //        //    formattedValue, errorText, cellStyle,
    //        //    advancedBorderStyle, paintParts);

    //        base.Paint(graphics, clipBounds, cellBounds, rowIndex, cellState, value, formattedValue, errorText, cellStyle, advancedBorderStyle, paintParts);

    //        Image _img = Properties.Resources.btn_save;
    //        graphics.DrawImage(_img, cellBounds.Location.X + 5, cellBounds.Location.Y + 3, _img.Width, _img.Height);
           
    //    }
       
    //}








    //编辑
    public class DataGridViewButtonColumnEdi : DataGridViewColumn
    {
        public DataGridViewButtonColumnEdi()
        {
            this.CellTemplate = new DataGridViewButtonCellEdi();
            this.HeaderText = "button";

        }
    }

    public class DataGridViewButtonCellEdi : DataGridViewButtonCell
    {
        protected override void Paint(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex, DataGridViewElementStates cellState, object value, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
        {
            base.Paint(graphics, clipBounds, cellBounds, rowIndex, cellState, value, formattedValue, errorText, cellStyle, advancedBorderStyle, paintParts);
            Image _img = global ::TJClient.Properties.Resources.btn_save;
            graphics.DrawImage(_img, cellBounds.Location.X + 5, cellBounds.Location.Y + 3, _img.Width, _img.Height);
        }
    }

    //添加
    public class DataGridViewButtonColumnAdd : DataGridViewColumn 
    { 
        public DataGridViewButtonColumnAdd() 
        { 
            this.CellTemplate = new DataGridViewButtonCellAdd(); this.HeaderText = "button";
        } 
    }
    public class DataGridViewButtonCellAdd : DataGridViewButtonCell
    {
        protected override void Paint(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex, DataGridViewElementStates cellState, object value, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
        {
            base.Paint(graphics, clipBounds, cellBounds, rowIndex, cellState, value, formattedValue, errorText, cellStyle, advancedBorderStyle, paintParts);
            Image _img = global ::TJClient .Properties.Resources.btn_view;
            graphics.DrawImage(_img, cellBounds.Location.X + 5, cellBounds.Location.Y + 3, _img.Width, _img.Height);
        }
    }
}
