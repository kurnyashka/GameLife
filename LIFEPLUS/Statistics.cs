using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace LIFEPLUS
{
    class Statistics : Terrain
    {
        Terrain terrain;

        private double time;
        private int stars;

        public Statistics(Terrain terrain)
        {
            this.terrain = terrain;
            stars = n * n / 10;
        }

        public override Terrain Ter()
        {
            return terrain;
        }

        public override void MakeTurn()
        {
            Stopwatch time = new Stopwatch();
            time.Start();
            this.terrain.MakeTurn();
            time.Stop();
            this.time = time.ElapsedMilliseconds / 1000.0;
        }

        public override void Draw(System.Drawing.Image img)
        {
            terrain.Draw(img);
        }

        public override double Time()
        {
            return this.time;
        }

        public override double Delta()
        {
            return terrain.Delta();
        }

        public override int Stars()
        {
            return terrain.Stars();
        }

        public override double TimeScan()
        {
            return terrain.TimeScan();
        }
    }
}
