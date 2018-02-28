using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIFEPLUS
{
    class BlackColony
    {
        public List<Cell> colony;
        public int direction;
        private Random rnd = new Random();

        public BlackColony(List<Cell> list)
        {
            this.colony = list;
            this.direction = rnd.Next(1, 5);
        }

        public void Turn(List<BlackColony> list)
        {
            if (direction == 0)
            {
                direction = rnd.Next(1, 5);
            }

            List<Cell> newColony = new List<Cell>();
            foreach (Cell c in colony)
                newColony.Add(c);

            for (int j = 0; j < colony.Count; j += 2)
            {
                for (int i = 0; i < 8; i++)
                {
                    if (colony[j].neighbors[i].level == 2 && !colony.Contains(colony[j].neighbors[i]))
                    {
                        foreach (BlackColony bc in list)
                        {
                            if (bc.colony.Contains(colony[j].neighbors[i]))
                            {
                                foreach (Cell c in bc.colony)
                                    newColony.Add(c);
                                bc.colony.Clear();
                            }
                        }
                    }
                    if (colony[j].neighbors[i].level == 1 && colony[j].neighbors[i].white + 1 < colony[j].neighbors[i].black)
                    {
                        if (!colony.Contains(colony[j].neighbors[i]))
                            newColony.Add(colony[j].neighbors[i]);
                    }
                    else if (colony[j].neighbors[i].level == -1 &&
                        ((i == 1 && direction == 1) || (i == 4 && direction == 2) ||
                         (i == 6 && direction == 3) || (i == 3 && direction == 4)))
                    {
                        direction = 0;
                    }
                }
                if (colony[j].white > colony[j].black + 1)
                {
                    colony[j].level = 1;
                    newColony.Remove(colony[j]);
                }
            }

            colony.Clear();
            foreach (Cell c in newColony)
                colony.Add(c);

            List<Cell> moveColony = new List<Cell>();
            List<Cell> review = new List<Cell>();
            foreach (Cell cell in colony)
                moveColony.Add(cell);
            foreach (Cell cell in moveColony)
                if (!review.Contains(cell))
                    Move(cell, review);
        }

        private void Move(Cell cell, List<Cell> review)
        {
            if (direction == 0)
                return;

            colony.Remove(cell);
            cell.level = 0;
            review.Add(cell);

            Cell help = new Cell();
            switch (direction)
            {
                case 1:
                    {
                        help = cell.neighbors[1];
                        while (help.level == 2)
                        {
                            review.Add(help);
                            help = help.neighbors[1];
                        }
                        break;
                    }
                case 2:
                    {
                        help = cell.neighbors[4];
                        while (help.level == 2)
                        {
                            review.Add(help);
                            help = help.neighbors[4];
                        }
                        break;
                    }
                case 3:
                    {
                        help = cell.neighbors[6];
                        while (help.level == 2)
                        {
                            review.Add(help);
                            help = help.neighbors[6];
                        }
                        break;
                    }
                case 4:
                    {
                        help = cell.neighbors[3];
                        while (help.level == 2)
                        {
                            review.Add(help);
                            help = help.neighbors[3];
                        }
                        break;
                    }
                default:
                    break;
            }

            if (help.level != -1)
            {
                review.Add(help);
                colony.Add(help);
                help.level = 2;
            }
        }

        public int Count
        {
            get
            {
                return colony.Count();
            }
        }
    }
}
