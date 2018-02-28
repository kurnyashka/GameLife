using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIFEPLUS
{
    class Cell
    {
        public int level;
        public int black, white;
        public int x, y;
        public Cell[] neighbors;

        private Random rnd = new Random();

        public Cell()
        {
            level = -1;
            this.neighbors = new Cell[8];
        }

        public Cell(Cell c)
        {
            this.level = c.level;
            this.black = c.black;
            this.white = c.white;
            this.x = c.x;
            this.y = c.y;
            this.neighbors = c.neighbors;
        }

        public Cell(int x, int y)
        {
            this.x = x;
            this.y = y;
            this.level = 0;
            this.neighbors = new Cell[8];
        }

        public void FirstSetNeighbor(Cell[,] cells, int a, int b)
        {
            int n = (int)Math.Sqrt(cells.Length), k = 0;
            for (int i = -1; i < 2; i++)
                for (int j = -1; j < 2; j++)
                {
                    if (i != 0 || j != 0)
                    {
                        if (a + i >= 0 && a + i < n && b + j >= 0 && b + j < n)
                        {
                            neighbors[k++] = cells[a + i, b + j];
                        }
                        else
                        {
                            neighbors[k++] = new Cell();
                        }
                    }
                }
            SetNeighbor();
        }

        public void SetNeighbor()
        {
            this.white = 0;
            this.black = 0;
            for (int i = 0; i < 8; i++)
            {
                if (neighbors[i].level == 1)
                    this.white++;
                else if (neighbors[i].level == 2)
                    this.black++;
            }
        }

        public void Turn()
        {
            if (this.level == 1 && (this.black == this.white + 1 || this.black == 0) &&
                     (this.white + this.black < 2 || this.white + this.black > 3))
                this.level = 0;
            else if (this.level == 0 && this.white + this.black == 3)
                this.level = 1;
        }
    }
}
