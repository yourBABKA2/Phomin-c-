using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Phomin_Lab_1
{
    public partial class Form1 : Form
    {
        const int padding = 16;

        int old_x, old_y;

        List<Figure> lst = new List<Figure>();

        Figure createFigure(string fig_type)
        {
            switch (fig_type)
            {
                case "Circle": return new Circle();
                case "Square": return new Square();
            }
            return null;
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void pictureBoxDraw_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.FillRectangle(Brushes.White, 0, 0, pictureBoxDraw.Width, pictureBoxDraw.Height);
            foreach (Figure fig in lst)
                fig.draw(e.Graphics);
        }

        private void pictureBoxDraw_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                int dx = e.X - old_x;
                int dy = e.Y - old_y;
                foreach (Figure fig in lst)
                {
                    if (fig.selected == false) continue;
                    fig.pos_x += dx;
                    fig.pos_y += dy;
                }
                pictureBoxDraw.Invalidate();
            }
            old_x = e.X;
            old_y = e.Y;
        }

        private void pictureBoxDraw_MouseDown(object sender, MouseEventArgs e)
        {
            foreach (Figure fig in lst)
                fig.selected = false;
            for (int i = lst.Count - 1; i >= 0; i--)
            {
                Figure fig = lst[i];
                fig.selected |= fig.test(e.X, e.Y);
                if (fig.selected == true) break;
            }
            pictureBoxDraw.Invalidate();
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            int i = 0;
            while (i < lst.Count)
            {
                if (lst[i].selected == true) lst.RemoveAt(i);
                i++;
            }
            pictureBoxDraw.Invalidate();
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            Figure fig = createFigure(comboBox_Figures.Text);
            if (fig == null) return;
            fig.pos_x = 100.0f;
            fig.pos_y = 100.0f;
            lst.Add(fig);
            pictureBoxDraw.Invalidate();
        }

    }

    public class Figure
    {
        public float pos_x, pos_y;
        public bool selected;

        virtual public bool test(float x, float y)
        {
            return false;
        }

        virtual public void draw(Graphics g)
        {

        }
    }

    public class Circle : Figure
    {
        public float radius;

        public Circle()
        {
            radius = 50.0f;
        }

        public override bool test(float x, float y)
        {
            float radius_square = radius * radius;
            float dx = x - pos_x;
            float dy = y - pos_y;
            if (dx * dx + dy * dy <= radius_square) return true;
            return false;
        }

        public override void draw(Graphics g)
        {
            float diameter = radius * 2.0f;
            float x0 = pos_x - radius;
            float y0 = pos_y - radius;
            Pen p = Pens.Black;
            if (selected == true) p = Pens.Red;
            g.DrawEllipse(p, x0, y0, diameter, diameter);
        }
    }

    public class Square : Figure
    {
        public float side;

        public Square()
        {
            side = 100;
        }

        public override bool test(float x, float y)
        {
            float half_side = side * 0.5f;
            float xmin = pos_x - half_side;
            float ymin = pos_y - half_side;
            float xmax = pos_x + half_side;
            float ymax = pos_y + half_side;
            if (x < xmin || y < ymin) return false;
            if (x > xmax || y > ymax) return false;
            return true;
        }

        public override void draw(Graphics g)
        {
            float half_side = side * 0.5f;
            float x0 = pos_x - half_side;
            float y0 = pos_y - half_side;
            Pen p = Pens.Black;
            if (selected == true) p = Pens.Red;
            g.DrawRectangle(p, x0, y0, side, side);
        }
    }
}
