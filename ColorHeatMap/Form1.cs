using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ColorHeatMap
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();

            userControl11.Min = 0;
            userControl11.Max = 1;
            userControl11.CellsWide = 12;
            userControl11.CellsTall = 8;
            userControl11.Values = new float[96];

            var rnd = new Random();

            for (int i = 0; i < 96; i++)
            {
                userControl11.Values[i] = (float) (rnd.NextDouble() * 1.2 - 0.1);
            }

            comboBox1.SelectedIndex = 0;
        }

        private Color[] CreateGradientPalette()
        {
            Bitmap b = new Bitmap(100, 1);
            var g = Graphics.FromImage(b);

            Point[] points = { new Point(0, 0), new Point(50, 0), new Point(100, 0), new Point(100, 1) };
            Color[] colors = { Color.Red, Color.Yellow, Color.Green };
            //GraphicsPath path = new GraphicsPath();
            //path.AddLines(points);

            // Use the path to construct a path gradient brush.
            PathGradientBrush br = new PathGradientBrush(points);
            br.SurroundColors = colors;

            // var br = new LinearGradientBrush(new Point(0, 0), new Point(100, 0), Color.Red, Color.Green);
            g.FillRectangle(br, 0, 0, 100, 1);

            Color[] palette = new Color[b.Size.Width];
            for (int i = 0; i < palette.Length; i++)
            {
                palette[i] = b.GetPixel(i, 0);
            }

            b.Dispose();

            return palette;
        }

        private Color[] CreateSolidPalette()
        {
            var palette = new Color[3];
            palette[0] = Color.Red;
            palette[1] = Color.Yellow;
            palette[2] = Color.Green;

            return palette;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Color[] palette;

            if (comboBox1.SelectedItem.ToString() == "Gradient")
                palette = CreateGradientPalette();
            else
                palette = CreateSolidPalette();

            userControl11.Palette = palette;
        }
    }
}
