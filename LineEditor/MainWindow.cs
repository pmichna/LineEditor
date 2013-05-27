using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LineEditor
{
    public partial class MainWindow : Form
    {
        private Bitmap bmp;
        private bool lineStarted = false;
        private bool inserting = false;
        private bool editing = false;
        private bool deleting = false;
        private bool isSelected = false;
        private Point editedPoint;
        private Line editedLine;
        private Point startPoint = new Point();
        private List<Line> lines = new List<Line>();
        private Color color = Color.Black;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            bmp = new Bitmap(pictureBox.Width, pictureBox.Height);
            setWhite(bmp);
            pictureBox.Image = bmp;
        }

        private void setWhite(Bitmap bmp)
        {
            for (int x = 0; x < bmp.Width; x++)
                for (int y = 0; y < bmp.Height; y++)
                    bmp.SetPixel(x, y, Color.White);
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            inserting = true;
            editing = false;
            deleting = false;
        }

        private void pictureBox_MouseClick(object sender, MouseEventArgs e)
        {
            if (inserting)
            {
                if (!lineStarted)
                {
                    startPoint.X = e.X;
                    startPoint.Y = e.Y;
                    lineStarted = true;
                }
                else
                {
                    Line line = new Line(startPoint, new Point(e.X, e.Y), bmp.Height, (float)Decimal.ToDouble(numericUpDown.Value), color);
                    drawLine(line);
                    lines.Add(line);
                    lineStarted = false;
                }
            }
            else if (deleting)
            {
                foreach (Line l in lines)
                {
                    if (l.isPointInLine(new Point(e.X, e.Y)))
                    {
                        deleteLine(l);
                        break;
                    }
                }

            }
            else if (editing)
            {
                if (!isSelected)
                {
                    foreach (Line l in lines)
                    {
                        Point p = new Point(e.X, e.Y);
                        if (l.isPointInStartArea(p))
                        {
                            isSelected = true;
                            editedLine = l;
                            editedPoint = l.StartPoint; // surowy
                            break;
                        }
                        else if (l.isPointInEndArea(p))
                        {
                            isSelected = true;
                            editedLine = l;
                            editedPoint = l.EndPoint; //surowy
                            break;
                        }
                    }
                }
                else
                {
                    if (editedPoint.Equals(editedLine.StartPoint)) //surowy
                    {
                        Line newLine = new Line(new Point(e.X, e.Y), new Point(editedLine.EndPoint.X, editedLine.EndPoint.Y), bmp.Height, (float)Decimal.ToDouble(numericUpDown.Value), editedLine.Color);
                        deleteLine(editedLine);
                        lines.Add(newLine);
                        drawLine(newLine);
                        isSelected = false;
                    }
                    else if (editedPoint.Equals(editedLine.EndPoint))
                    {
                        Line newLine = new Line(new Point(editedLine.StartPoint.X, editedLine.StartPoint.Y), new Point(e.X, e.Y), bmp.Height, (float)Decimal.ToDouble(numericUpDown.Value), editedLine.Color);
                        deleteLine(editedLine);
                        lines.Add(newLine);
                        drawLine(newLine);
                        isSelected = false;
                    }
                }
            }
        }

        private void deleteLine(Line l)
        {
            foreach (Point p in l.Points)
                bmp.SetPixel(p.X, p.Y, Color.White);
            lines.Remove(l);
            foreach (Line ln in lines)
            {
                foreach (Point p in ln.Points)
                    bmp.SetPixel(p.X, p.Y, ln.Color);
            }
            pictureBox.Refresh();

        }

        private void drawLine(Line line)
        {
            if (line.StartPoint.Equals(line.EndPoint))
                return;
            foreach (Point p in line.Points)
                bmp.SetPixel(p.X, p.Y, line.Color);
            pictureBox.Refresh();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            editing = true;
            inserting = false;
            deleting = false;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            deleting = true;
            inserting = false;
            editing = false;
        }

        private void btnColor_Click(object sender, EventArgs e)
        {
            if (colorDialog.ShowDialog() == DialogResult.OK)
                color = colorDialog.Color;
        }
    }
}
