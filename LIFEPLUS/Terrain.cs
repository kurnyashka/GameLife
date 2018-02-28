using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace LIFEPLUS
{
    class Terrain
    {
        public Cell[,] cells;
        public List<BlackColony> colonies;
        public int n = 50, size = 600;
        Random rnd = new Random();
        private int stars;
        private double delta;

        public Terrain()
        {
            stars = 250;

            cells = new Cell[n, n];
            colonies = new List<BlackColony>();
            int del = size / n;

            for (int k = 0; k < n; k++)
            {
                for (int j = 0; j < n; j++)
                {
                    cells[k, j] = new Cell(del * k, del * j);
                }
            }

            int x = rnd.Next(1, n), y = rnd.Next(1, n);
            int i = 0;

            while (i < n * n / 10)
            {
                if (cells[x, y].level == 0)
                {
                    cells[x, y].level = 1;
                    i++;
                }
                x = rnd.Next(1, n);
                y = rnd.Next(1, n);
            }

            for (int k = 0; k < n; k++)
            {
                for (int j = 0; j < n; j++)
                {
                    cells[k, j].FirstSetNeighbor(cells, k, j);
                }
            }
        }

        public virtual void Draw(Image img)
        {
            int del = img.Height / n;


            int stars2 = 0;
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++) {
                    if (this.cells[i, j].level > 0)
                        stars2++;
                }

            this.delta = Convert.ToDouble(stars2) / Convert.ToDouble(this.stars) * 100.00;
            this.delta = Math.Round(this.delta, 2);
            this.stars = stars2;

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (cells[i, j].level == 1)
                    {
                        DrawStar(cells[i, j].x, cells[i, j].y, del, img, Color.Pink);
                    }
                    else if (cells[i, j].level == 2)
                    {
                        DrawStar(cells[i, j].x, cells[i, j].y, del, img, Color.LightCoral);
                    }
                }
            }
        }

        private void DrawStar(int x, int y, int del, Image img, Color color)
        {
            Pen pen = new Pen(color, 2);

            Graphics flagGraphics = Graphics.FromImage(img);

            double r = del / 2 - 1, R = r / 2;
            double alpha = 0;
            double x0 = x + del / 2 - 1, y0 = y + del / 2 - 1;

            PointF[] points = new PointF[2 * 5 + 1];
            double a = alpha, da = Math.PI / 5, l;
            for (int k = 0; k < 2 * 5 + 1; k++)
            {
                l = k % 2 == 0 ? r : R;
                points[k] = new PointF((float)(x0 + l * Math.Cos(a)), (float)(y0 + l * Math.Sin(a)));
                a += da;
            }

            flagGraphics.DrawLines(pen, points);
        }

        public virtual void MakeTurn()
        {
            foreach (var colony in colonies)
            {
                if (colony.Count > 0)
                    colony.Turn(colonies);
            }

            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                {
                    cells[i, j].Turn();
                }

            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                {
                    cells[i, j].SetNeighbor();
                }
        }

        public virtual Terrain Ter()
        {
            return this;
        }

        public virtual double Time()
        { return 0; }

        public virtual double TimeScan()
        { return 0; }

        public virtual double Delta()
        { return delta; }

        public virtual int Stars()
        { return stars; }
    }
}
