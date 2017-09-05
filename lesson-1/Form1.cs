using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lesson_1
{
	public partial class Form1 : Form
	{
		private static int STEPS = 100;

		private Graphics g;

        private float leftLimit = -1;
        private float rightLimit = 1;
        private Func<float, float> func;

        private static Dictionary<String, Func<float, float>> options = new Dictionary<string, Func<float, float>>();

        static Form1()
        {
            options["sin(x)"] = x => (float)Math.Sin(x);
            options["x^2"] = x => x * x;
            options["abs(x)"] = x => Math.Abs(x);
            options["sin(x) * cos(2/x)"] = x => (float)(Math.Sin(x) * Math.Cos(2 / x));
            options["tg(x)"] = x => (float)(Math.Tan(x));
            options["tgh(x)"] = x => (float)(Math.Tanh(x));
        }

        public Form1()
		{
			InitializeComponent();

            leftTextBox.Text = leftLimit.ToString();
            rightTextBox.Text = rightLimit.ToString();
            functionComboBox.Items.AddRange(options.Keys.ToArray());
			graphPictureBox.Image = new Bitmap(graphPictureBox.Width, graphPictureBox.Height);
			g = Graphics.FromImage(graphPictureBox.Image);
		}

        private void RedrawGraph()
        {
            PointF[] points = new PointF[STEPS + 1];
            float min = float.MaxValue, max = float.MinValue;
            for (int i = 0; i <= STEPS; ++i)
            {
                float x = leftLimit + i * (rightLimit - leftLimit) / STEPS;
                float y = func(x);
                points[i].X = x;
                points[i].Y = y;
                if (y < min) min = y;
                if (max < y) max = y;
            }
            if (min == max) max = min + 1;
            for (int i = 0; i <= STEPS; ++i)
            {
                points[i].X = (points[i].X - leftLimit) / (rightLimit - leftLimit) * graphPictureBox.Width;
                points[i].Y = (max - points[i].Y) / (max - min) * graphPictureBox.Height;
            }
            g.Clear(Color.White);
            g.DrawLines(Pens.Black, points);
            graphPictureBox.Invalidate();
        }

		private void functionComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var comboBox = (ComboBox)sender;
            func = options[(string)comboBox.SelectedItem];
            RedrawGraph();
        }

        private void viewButton_Click(object sender, EventArgs e)
        {
            float new_left;
            float new_right;
            try
            {
                new_left = float.Parse(leftTextBox.Text);
                new_right = float.Parse(rightTextBox.Text);
                if (new_right <= new_left)
                {
                    MessageBox.Show("\"От\" должно быть меньше чем \"До\".");
                    return;
                }
            }
            catch
            {
                MessageBox.Show("В поле ввода должно быть число.");
                return;
            }
            leftLimit = new_left;
            rightLimit = new_right;
            RedrawGraph();
        }
    }
}
