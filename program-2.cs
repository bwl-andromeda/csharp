using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

namespace растровая
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new SeedFillForm());
        }
    }

    public class SeedFillForm : Form
    {
        private PictureBox pictureBox;
        private Button drawShapeButton;
        private Bitmap bitmap;

        public SeedFillForm()
        {
            Text = "Seed Fill - Flood Fill Algorithm";
            Width = 800;
            Height = 600;

            pictureBox = new PictureBox
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White
            };
            pictureBox.MouseClick += PictureBox_MouseClick;
            Controls.Add(pictureBox);

            drawShapeButton = new Button
            {
                Text = "Draw Shape",
                Dock = DockStyle.Top
            };
            drawShapeButton.Click += (s, e) => DrawShape();
            Controls.Add(drawShapeButton);

            bitmap = new Bitmap(pictureBox.Width, pictureBox.Height);
            pictureBox.Image = bitmap;
        }

        private void PictureBox_MouseClick(object sender, MouseEventArgs e)
        {
            Color targetColor = bitmap.GetPixel(e.X, e.Y);
            Color replacementColor = Color.Red;
            if (targetColor.ToArgb() != replacementColor.ToArgb())
            {
                SeedFill(e.X, e.Y, targetColor, replacementColor);
                pictureBox.Refresh();
            }
        }

        private void DrawShape()
        {
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.Clear(Color.White);
                Pen pen = new Pen(Color.Black, 2);
                g.DrawEllipse(pen, 100, 100, 200, 150);
                g.DrawRectangle(pen, 150, 200, 100, 100);
                g.DrawLine(pen, 200, 200, 300, 300);
            }
            pictureBox.Refresh();
        }

        private void SeedFill(int x, int y, Color targetColor, Color replacementColor)
        {
            Stack<Point> pixels = new Stack<Point>();
            pixels.Push(new Point(x, y));

            while (pixels.Count > 0)
            {
                Point p = pixels.Pop();
                if (p.X < 0 || p.X >= bitmap.Width || p.Y < 0 || p.Y >= bitmap.Height) continue;
                if (bitmap.GetPixel(p.X, p.Y) != targetColor) continue;

                bitmap.SetPixel(p.X, p.Y, replacementColor);

                pixels.Push(new Point(p.X + 1, p.Y));
                pixels.Push(new Point(p.X - 1, p.Y));
                pixels.Push(new Point(p.X, p.Y + 1));
                pixels.Push(new Point(p.X, p.Y - 1));
            }
        }
    }
}
