using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace LIFEPLUS
{
    class Lattice : Terrain
    {
        Terrain terrain;

        public Lattice(Terrain terrain)
        {
            this.terrain = terrain;
        }

        public override Terrain Ter()
        {
            return terrain;
        }

        public override void Draw(Image img)
        {
            int size = img.Height;
            int del = size / n;

            terrain.Draw(img);
            
            Pen pen = new Pen(Color.DarkGray);

            Graphics flagGraphics = Graphics.FromImage(img);

            for (int i = 0; i <= n; i++)
            {
                flagGraphics.DrawLine(pen, new Point(0, del * i), new Point(size, del * i));
                flagGraphics.DrawLine(pen, new Point(del * i, 0), new Point(del * i, size));
            }
        }

        public override void MakeTurn()
        {
            terrain.MakeTurn();
        }

        public override double Delta()
        {
            return terrain.Delta();
        }

        public override int Stars()
        {
            return terrain.Stars();
        }

        public override double Time()
        {
            return terrain.Time();
        }

        public override double TimeScan()
        {
            return terrain.TimeScan();
        }
    }
}
