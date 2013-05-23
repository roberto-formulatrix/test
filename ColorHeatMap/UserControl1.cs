using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ColorHeatMap
{
// simple comment
    public partial class UserControl1 : UserControl
    {
        private Brush[] _paletteBrushes;
        private Color[] _palette;

        public UserControl1()
        {
            InitializeComponent();
            Graphics g = Graphics.FromImage(new Bitmap(100, 100));
            g.FillRectangle(Brushes.Red, new Rectangle(0, 0, 100, 2));
        }

        public int CellsWide { set; get; }
        public int CellsTall { set; get; }
        public float Min { set; get; }
        public float Max { set; get; }
        public float[] Values { set; get; }

        public Color[] Palette {
            get
            {
                return _palette;
            }
            set
            {
                if (value == null) return;
                DisposeBrushes();

                _palette = value;
                _paletteBrushes = new Brush[_palette.Length];
                for (int i = 0; i < _palette.Length; i++)
                {
                    _paletteBrushes[i] = new SolidBrush(_palette[i]);
                }

                Refresh();
            }
        }

        private void DisposeBrushes() 
        {
            //TODO:...
        }

        private void UserControl1_Paint(object sender, PaintEventArgs e)
        {
            if (DesignMode)
            {
                Rectangle r = ClientRectangle;
                r.Width--;
                r.Height--;
                e.Graphics.DrawRectangle(Pens.Black, r);
                return;
            }

            using (var f = new Font("Tahoma", 12))
                PaintCanvas(e, f);
        }

        private void PaintCanvas(PaintEventArgs e, System.Drawing.Font f)
        {
            int cellHeight = Height / CellsTall;
            int cellWidth = Width / CellsWide;

            var topLeft = new Point((int)(cellWidth * 0.1), (int)(cellHeight * 0.1));
            int padding = topLeft.X;
            var currentLocation = topLeft;

            var wellSize = new Size((int)(cellWidth * 0.8), (int)(cellHeight * 0.8));
            var r = new Rectangle(currentLocation, wellSize);
            
            for (int i = 0; i < Values.Length; i++)
            {
                r.Location = currentLocation;

                float v = Values[i];

                if (v >= Min && v <= Max)
                {
                    int idx = (int)((v - Min) / (Max*1.01 - Min) * _paletteBrushes.Length);
                    e.Graphics.FillRectangle(_paletteBrushes[idx], r);
                }
                else
                {
                    e.Graphics.DrawRectangle(Pens.Gray, r);
                }

                e.Graphics.DrawString(String.Format("{0:0.00}", Values[i]), f, Brushes.Black, r.X + 2, r.Y + 2);

                currentLocation.X += cellWidth;
                if (currentLocation.X + cellWidth - padding > Width)
                {
                    currentLocation.X = topLeft.X;
                    currentLocation.Y += cellHeight;
                }
            }
        }

        private void UserControl1_ClientSizeChanged(object sender, EventArgs e)
        {
            Refresh();
        }
    }
}
