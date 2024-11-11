using System;
using System.Drawing;
using System.Windows.Forms;

namespace растровая
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new RasterCurveForm());
        }
    }

    public class RasterCurveForm : Form
    {
        private PictureBox pictureBox;
        private Button bresenhamButton;
        private Button wuButton;

        public RasterCurveForm()
        {
            Text = "Raster Curve - Bresenham and Wu";
            Width = 800;
            Height = 600;

            pictureBox = new PictureBox
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White
            };
            Controls.Add(pictureBox);

            bresenhamButton = new Button
            {
                Text = "Bresenham",
                Dock = DockStyle.Top
            };
            bresenhamButton.Click += (s, e) => DrawBresenhamLine(50, 50, 400, 300);
            Controls.Add(bresenhamButton);

            wuButton = new Button
            {
                Text = "Wu",
                Dock = DockStyle.Top
            };
            wuButton.Click += (s, e) => DrawWuLine(50, 300, 400, 50);
            Controls.Add(wuButton);
        }

        private void DrawBresenhamLine(int x0, int y0, int x1, int y1)
        {
            using (Graphics g = pictureBox.CreateGraphics())
            {
                g.Clear(Color.White);
                int dx = Math.Abs(x1 - x0), dy = Math.Abs(y1 - y0);
                int sx = x0 < x1 ? 1 : -1;
                int sy = y0 < y1 ? 1 : -1;
                int err = dx - dy;

                while (true)
                {
                    g.FillRectangle(Brushes.Black, x0, y0, 1, 1);
                    if (x0 == x1 && y0 == y1) break;
                    int e2 = 2 * err;
                    if (e2 > -dy) { err -= dy; x0 += sx; }
                    if (e2 < dx) { err += dx; y0 += sy; }
                }
            }
        }

        private void DrawWuLine(int x0, int y0, int x1, int y1)
        {
            using (Graphics g = pictureBox.CreateGraphics())
            {
                g.Clear(Color.White);
                bool steep = Math.Abs(y1 - y0) > Math.Abs(x1 - x0);
                if (steep)
                {
                    (x0, y0) = (y0, x0);
                    (x1, y1) = (y1, x1);
                }
                if (x0 > x1)
                {
                    (x0, x1) = (x1, x0);
                    (y0, y1) = (y1, y0);
                }

                float dx = x1 - x0;
                float dy = y1 - y0;
                float gradient = dy / dx;

                float y = y0 + gradient;
                for (int x = x0 + 1; x < x1; x++)
                {
                    if (steep)
                    {
                        g.FillRectangle(new SolidBrush(Color.FromArgb((int)(255 * (1 - (y % 1))), Color.Black)), (int)y, x, 1, 1);
                        g.FillRectangle(new SolidBrush(Color.FromArgb((int)(255 * (y % 1)), Color.Black)), (int)y + 1, x, 1, 1);
                    }
                    else
                    {
                        g.FillRectangle(new SolidBrush(Color.FromArgb((int)(255 * (1 - (y % 1))), Color.Black)), x, (int)y, 1, 1);
                        g.FillRectangle(new SolidBrush(Color.FromArgb((int)(255 * (y % 1)), Color.Black)), x, (int)y + 1, 1, 1);
                    }
                    y += gradient;
                }
            }
        }
    }
}
