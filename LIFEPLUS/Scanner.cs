using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Drawing;

namespace LIFEPLUS
{
    class Scanner : Terrain
    {
        Terrain terrain;
        private double timeScan;

        public Scanner(Terrain terrain)
        {
            this.terrain = terrain;
        }

        public override void MakeTurn()
        {
            terrain.MakeTurn();
        }

        public override void Draw(Image img)
        {
            int size = img.Height;
            int del = size / n;

            Stopwatch time = new Stopwatch();
            time.Start();

            bool[,] pr = new bool[n, n];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (!pr[i, j])
                    {
                        IsPattern(i, j, pr);
                    }
                }
            }

            time.Stop();
            timeScan = time.ElapsedMilliseconds / 1000.0;

            terrain.Draw(img);
        }

        private bool IsPattern(int x, int y, bool[,] review)
        {
            review[x, y] = true;

            // Мигалка
            if (IsFlasher(x, y, review) || IsFlasher2(x, y, review))
                return true;

            //Блок
            if (IsBlock(x, y, review))
                return true;

            //Улей
            if (IsHive(x, y, review) || IsHive2(x, y, review))
                return true;

            //Каравай
            if (IsLoaf(x, y, review) || IsLoaf2(x, y, review) ||
                IsLoaf3(x, y, review) || IsLoaf4(x, y, review))
                return true;

            //Лодка
            if (IsBoat(x, y, review) || IsBoat2(x, y, review) ||
                IsBoat3(x, y, review) || IsBoat4(x, y, review))
                return true;

            //Пруд
            if (IsPond(x, y, review))
                return true;

            //Ящик
            if (IsBox(x, y, review))
                return true;

            //Змея
            if (IsSnake(x, y, review) || IsSnake2(x, y, review) ||
                IsSnake3(x, y, review) || IsSnake4(x, y, review))
                return true;

            //Баржа
            if (IsBarge(x, y, review) || IsBarge2(x, y, review))
                return true;

            //Корабль
            if (IsShip(x, y, review) || IsShip2(x, y, review))
                return true;

            //Длинный корабль
            if (IsLongShip(x, y, review) || IsLongShip2(x, y, review))
                return true;

            //Длинная лодка
            if (IsLongBoat(x, y, review) || IsLongBoat2(x, y, review) ||
                IsLongBoat3(x, y, review) || IsLongBoat4(x, y, review))
                return true;

            //Длинная баржа
            if (IsLongBarge(x, y, review) || IsLongBarge2(x, y, review))
                return true;

            return false;
        }

        private bool IsThat(int[,] that, int xt, int yt, int x, int y, bool[,] review)
        {
            if (x + xt - 1 > n || y + yt - 1 > n)
                return false;

            for (int i = x - 1; i < x + xt; i++)
                for (int j = y - 1; j < y + yt; j++)
                {
                    if (i >= 0 && j >= 0 && i < n && j < n)
                    {
                        if (that[i - x + 1, j - y + 1] != 2 && 
                            (terrain.cells[i, j].level == 2 || 
                            (terrain.cells[i, j].level == 1 && that[i - x + 1, j - y + 1] == 0) || 
                            (terrain.cells[i, j].level == 0 && that[i - x + 1, j - y + 1] == 1)))
                            return false;
                    }
                }

            List<Cell> newColony = new List<Cell>();
            
            for (int i = x - 1; i < x + xt; i++)
                for (int j = y - 1; j < y + yt; j++)
                {
                    if (i >= 0 && j >= 0 && i < n && j < n)
                    {
                        if (that[i - x + 1, j - y + 1] == 1)
                        {
                            terrain.cells[i, j].level = 2;
                            newColony.Add(terrain.cells[i, j]);
                        }
                        /*else if (that[i - x + 1, j - y + 1] == 0)
                        {
                            terrain.cells[i, j].level = 0;
                        }*/
                        review[i, j] = true;
                    }
                }
            terrain.colonies.Add(new BlackColony(newColony));
            return true;
        }

        private bool IsHive(int x, int y, bool[,] review)
        {
            int[,] hive = new int[5, 6];
            hive[2, 1] = 1;
            hive[1, 2] = 1;
            hive[1, 3] = 1;
            hive[2, 4] = 1;
            hive[3, 3] = 1;
            hive[3, 2] = 1;
            hive[0, 0] = 2;
            hive[4, 0] = 2;
            hive[0, 5] = 2;
            hive[4, 5] = 2;

            return IsThat(hive, 4, 5, x, y, review);
        }

        private bool IsHive2(int x, int y, bool[,] review)
        {
            int[,] hive = new int[6, 5];
            hive[1, 2] = 1;
            hive[2, 1] = 1;
            hive[3, 1] = 1;
            hive[4, 2] = 1;
            hive[3, 3] = 1;
            hive[2, 3] = 1;
            hive[0, 0] = 2;
            hive[0, 4] = 2;
            hive[5, 0] = 2;
            hive[5, 4] = 2;

            return IsThat(hive, 5, 4, x, y, review);
        }

        private bool IsBlock(int x, int y, bool[,] review)
        {
            int[,] block = new int[4, 4];

            for (int i = 1; i < 3; i++)
                for (int j = 1; j < 3; j++)
                {
                    block[i, j] = 1;
                }

            return IsThat(block, 3, 3, x, y, review);
        }

        private bool IsLoaf(int x, int y, bool[,] review)
        {
            int[,] loaf = new int[6, 6];

            loaf[0, 0] = 2;
            loaf[0, 1] = 2;
            loaf[1, 0] = 2;
            loaf[5, 0] = 2;
            loaf[0, 5] = 2;
            loaf[5, 5] = 2;

            loaf[3, 1] = 1;
            loaf[2, 2] = 1;
            loaf[4, 2] = 1;
            loaf[1, 3] = 1;
            loaf[4, 3] = 1;
            loaf[2, 4] = 1;
            loaf[3, 4] = 1;

            return IsThat(loaf, 5, 5, x, y, review);
        }

        private bool IsLoaf2(int x, int y, bool[,] review)
        {
            int[,] loaf = new int[6, 6];

            loaf[5, 0] = 2;
            loaf[5, 1] = 2;
            loaf[4, 0] = 2;
            loaf[0, 0] = 2;
            loaf[5, 5] = 2;
            loaf[0, 5] = 2;

            loaf[4, 3] = 1;
            loaf[3, 2] = 1;
            loaf[3, 4] = 1;
            loaf[2, 1] = 1;
            loaf[2, 4] = 1;
            loaf[1, 2] = 1;
            loaf[1, 3] = 1;

            return IsThat(loaf, 5, 5, x, y, review);
        }

        private bool IsLoaf3(int x, int y, bool[,] review)
        {
            int[,] loaf = new int[6, 6];

            loaf[0, 5] = 2;
            loaf[0, 4] = 2;
            loaf[1, 5] = 2;
            loaf[5, 5] = 2;
            loaf[0, 0] = 2;
            loaf[5, 0] = 2;

            loaf[3, 4] = 1;
            loaf[2, 3] = 1;
            loaf[4, 3] = 1;
            loaf[1, 2] = 1;
            loaf[4, 2] = 1;
            loaf[2, 1] = 1;
            loaf[3, 1] = 1;

            return IsThat(loaf, 5, 5, x, y, review);
        }

        private bool IsLoaf4(int x, int y, bool[,] review)
        {
            int[,] loaf = new int[6, 6];

            loaf[5, 5] = 2;
            loaf[5, 4] = 2;
            loaf[4, 5] = 2;
            loaf[0, 5] = 2;
            loaf[5, 0] = 2;
            loaf[0, 0] = 2;

            loaf[2, 4] = 1;
            loaf[3, 3] = 1;
            loaf[1, 3] = 1;
            loaf[4, 2] = 1;
            loaf[1, 2] = 1;
            loaf[3, 1] = 1;
            loaf[2, 1] = 1;

            return IsThat(loaf, 5, 5, x, y, review);
        }

        private bool IsFlasher(int x, int y, bool[,] review)
        {
            int[,] flasher = new int[3, 5];
            for (int i = 1; i < 4; i++)
                flasher[1, i] = 1;

            return IsThat(flasher, 2, 4, x, y, review);
        }

        private bool IsFlasher2(int x, int y, bool[,] review)
        {
            int[,] flasher = new int[5, 3];
            for (int i = 1; i < 4; i++)
                flasher[i, 1] = 1;

            return IsThat(flasher, 4, 2, x, y, review);
        }

        private bool IsBoat(int x, int y, bool[,] review)
        {
            int[,] boat = new int[5, 5];

            boat[4, 0] = 2;
            boat[0, 4] = 2;
            boat[4, 4] = 2;

            boat[1, 1] = 1;
            boat[1, 2] = 1;
            boat[2, 1] = 1;
            boat[2, 3] = 1;
            boat[3, 2] = 1;

            return IsThat(boat, 4, 4, x, y, review);
        }

        private bool IsBoat2(int x, int y, bool[,] review)
        {
            int[,] boat = new int[5, 5];

            boat[0, 0] = 2;
            boat[4, 4] = 2;
            boat[0, 4] = 2;

            boat[3, 1] = 1;
            boat[3, 2] = 1;
            boat[2, 1] = 1;
            boat[2, 3] = 1;
            boat[1, 2] = 1;

            return IsThat(boat, 4, 4, x, y, review);
        }

        private bool IsBoat3(int x, int y, bool[,] review)
        {
            int[,] boat = new int[5, 5];

            boat[4, 4] = 2;
            boat[0, 0] = 2;
            boat[4, 0] = 2;

            boat[1, 3] = 1;
            boat[1, 2] = 1;
            boat[2, 3] = 1;
            boat[2, 1] = 1;
            boat[3, 2] = 1;

            return IsThat(boat, 4, 4, x, y, review);
        }

        private bool IsBoat4(int x, int y, bool[,] review)
        {
            int[,] boat = new int[5, 5];

            boat[0, 4] = 2;
            boat[4, 0] = 2;
            boat[0, 0] = 2;

            boat[3, 3] = 1;
            boat[3, 2] = 1;
            boat[2, 3] = 1;
            boat[2, 1] = 1;
            boat[1, 2] = 1;

            return IsThat(boat, 4, 4, x, y, review);
        }

        private bool IsPond(int x, int y, bool[,] review)
        {
            int[,] pond = new int[6, 6];

            pond[0, 0] = 2;
            pond[5, 0] = 2;
            pond[0, 5] = 2;
            pond[5, 5] = 2;

            pond[2, 1] = 1;
            pond[3, 1] = 1;
            pond[1, 2] = 1;
            pond[1, 3] = 1;
            pond[2, 4] = 1;
            pond[3, 4] = 1;
            pond[4, 2] = 1;
            pond[4, 3] = 1;

            return IsThat(pond, 5, 5, x, y, review);
        }

        private bool IsBox(int x, int y, bool[,] review)
        {
            int[,] box = new int[5, 5];

            box[0, 0] = 2;
            box[4, 0] = 2;
            box[0, 4] = 2;
            box[4, 4] = 2;

            box[2, 1] = 1;
            box[1, 2] = 1;
            box[3, 2] = 1;
            box[2, 3] = 1;

            return IsThat(box, 4, 4, x, y, review);
        }

        private bool IsSnake(int x, int y, bool[,] review)
        {
            int[,] snake = new int[6, 4];

            snake[1, 1] = 1;
            snake[1, 2] = 1;
            snake[2, 2] = 1;
            snake[3, 1] = 1;
            snake[4, 1] = 1;
            snake[4, 2] = 1;

            return IsThat(snake, 5, 3, x, y, review);
        }

        private bool IsSnake2(int x, int y, bool[,] review)
        {
            int[,] snake = new int[6, 4];

            snake[1, 1] = 1;
            snake[1, 2] = 1;
            snake[2, 1] = 1;
            snake[3, 2] = 1;
            snake[4, 1] = 1;
            snake[4, 2] = 1;

            return IsThat(snake, 5, 3, x, y, review);
        }

        private bool IsSnake3(int x, int y, bool[,] review)
        {
            int[,] snake = new int[4, 6];

            snake[1, 1] = 1;
            snake[1, 2] = 1;
            snake[2, 1] = 1;
            snake[2, 3] = 1;
            snake[1, 4] = 1;
            snake[2, 4] = 1;

            return IsThat(snake, 3, 5, x, y, review);
        }

        private bool IsSnake4(int x, int y, bool[,] review)
        {
            int[,] snake = new int[4, 6];

            snake[1, 1] = 1;
            snake[2, 1] = 1;
            snake[2, 2] = 1;
            snake[1, 3] = 1;
            snake[1, 4] = 1;
            snake[2, 4] = 1;

            return IsThat(snake, 3, 5, x, y, review);
        }

        private bool IsBarge(int x, int y, bool[,] review)
        {
            int[,] barge = new int[6, 6];

            barge[0, 0] = 2;
            barge[4, 0] = 2;
            barge[5, 0] = 2;
            barge[5, 1] = 2;
            barge[0, 4] = 2;
            barge[0, 5] = 2;
            barge[1, 5] = 2;
            barge[5, 5] = 2;

            barge[2, 1] = 1;
            barge[1, 2] = 1;
            barge[3, 1] = 1;
            barge[2, 3] = 1;
            barge[4, 3] = 1;
            barge[3, 4] = 1;

            return IsThat(barge, 5, 5, x, y, review);
        }

        private bool IsBarge2(int x, int y, bool[,] review)
        {
            int[,] barge = new int[6, 6];

            barge[5, 0] = 2;
            barge[1, 0] = 2;
            barge[0, 0] = 2;
            barge[0, 1] = 2;
            barge[5, 4] = 2;
            barge[5, 5] = 2;
            barge[4, 5] = 2;
            barge[0, 5] = 2;

            barge[3, 1] = 1;
            barge[4, 2] = 1;
            barge[2, 1] = 1;
            barge[3, 3] = 1;
            barge[1, 3] = 1;
            barge[2, 4] = 1;

            return IsThat(barge, 5, 5, x, y, review);
        }

        private bool IsShip(int x, int y, bool[,] review)
        {
            int[,] ship = new int[5, 5];

            ship[4, 0] = 2;
            ship[0, 4] = 2;

            ship[1, 1] = 1;
            ship[1, 2] = 1;
            ship[2, 1] = 1;
            ship[3, 3] = 1;
            ship[2, 3] = 1;
            ship[3, 2] = 1;

            return IsThat(ship, 4, 4, x, y, review);
        }

        private bool IsShip2(int x, int y, bool[,] review)
        {
            int[,] ship = new int[5, 5];

            ship[0, 0] = 2;
            ship[4, 4] = 2;

            ship[3, 1] = 1;
            ship[3, 2] = 1;
            ship[2, 1] = 1;
            ship[1, 3] = 1;
            ship[2, 3] = 1;
            ship[1, 2] = 1;

            return IsThat(ship, 4, 4, x, y, review);
        }

        private bool IsLongBarge(int x, int y, bool[,] review)
        {
            int[,] barge = new int[7, 7];

            barge[0, 0] = 2;
            barge[4, 0] = 2;
            barge[5, 0] = 2;
            barge[5, 1] = 2;
            barge[0, 4] = 2;
            barge[0, 5] = 2;
            barge[1, 5] = 2;
            barge[5, 5] = 2;
            barge[6, 0] = 2;
            barge[6, 1] = 2;
            barge[6, 2] = 2;
            barge[0, 6] = 2;
            barge[1, 6] = 2;
            barge[2, 6] = 2;

            barge[2, 1] = 1;
            barge[1, 2] = 1;
            barge[3, 1] = 1;
            barge[2, 3] = 1;
            barge[4, 3] = 1;
            barge[3, 4] = 1;
            barge[5, 4] = 1;
            barge[4, 5] = 1;

            return IsThat(barge, 6, 6, x, y, review);
        }

        private bool IsLongBarge2(int x, int y, bool[,] review)
        {
            int[,] barge = new int[7, 7];

            barge[6, 0] = 2;
            barge[2, 0] = 2;
            barge[1, 0] = 2;
            barge[1, 1] = 2;
            barge[6, 4] = 2;
            barge[6, 5] = 2;
            barge[5, 5] = 2;
            barge[1, 5] = 2;
            barge[0, 0] = 2;
            barge[0, 1] = 2;
            barge[0, 2] = 2;
            barge[6, 6] = 2;
            barge[5, 6] = 2;
            barge[4, 6] = 2;

            barge[4, 1] = 1;
            barge[5, 2] = 1;
            barge[3, 1] = 1;
            barge[4, 3] = 1;
            barge[2, 3] = 1;
            barge[3, 4] = 1;
            barge[1, 4] = 1;
            barge[2, 5] = 1;

            return IsThat(barge, 6, 6, x, y, review);
        }

        private bool IsLongBoat(int x, int y, bool[,] review)
        {
            int[,] boat = new int[6, 6];

            boat[4, 0] = 2;
            boat[0, 4] = 2;
            boat[0, 5] = 2;
            boat[1, 5] = 2;
            boat[5, 0] = 2;
            boat[5, 1] = 2;
            boat[5, 5] = 2;

            boat[1, 1] = 1;
            boat[1, 2] = 1;
            boat[2, 1] = 1;
            boat[2, 3] = 1;
            boat[3, 2] = 1;
            boat[4, 3] = 1;
            boat[3, 4] = 1;

            return IsThat(boat, 5, 5, x, y, review);
        }

        private bool IsLongBoat2(int x, int y, bool[,] review)
        {
            int[,] boat = new int[6, 6];

            boat[1, 0] = 2;
            boat[5, 4] = 2;
            boat[5, 5] = 2;
            boat[4, 5] = 2;
            boat[0, 0] = 2;
            boat[0, 1] = 2;
            boat[0, 5] = 2;

            boat[4, 1] = 1;
            boat[4, 2] = 1;
            boat[3, 1] = 1;
            boat[3, 3] = 1;
            boat[2, 2] = 1;
            boat[1, 3] = 1;
            boat[2, 4] = 1;

            return IsThat(boat, 5, 5, x, y, review);
        }

        private bool IsLongBoat3(int x, int y, bool[,] review)
        {
            int[,] boat = new int[6, 6];

            boat[4, 5] = 2;
            boat[0, 1] = 2;
            boat[0, 0] = 2;
            boat[1, 0] = 2;
            boat[5, 5] = 2;
            boat[5, 4] = 2;
            boat[5, 0] = 2;

            boat[1, 4] = 1;
            boat[1, 3] = 1;
            boat[2, 4] = 1;
            boat[2, 2] = 1;
            boat[3, 3] = 1;
            boat[4, 2] = 1;
            boat[3, 1] = 1;

            return IsThat(boat, 5, 5, x, y, review);
        }

        private bool IsLongBoat4(int x, int y, bool[,] review)
        {
            int[,] boat = new int[6, 6];

            boat[1, 5] = 2;
            boat[5, 1] = 2;
            boat[5, 0] = 2;
            boat[4, 0] = 2;
            boat[0, 5] = 2;
            boat[0, 4] = 2;
            boat[0, 0] = 2;

            boat[4, 4] = 1;
            boat[4, 3] = 1;
            boat[3, 4] = 1;
            boat[3, 2] = 1;
            boat[2, 3] = 1;
            boat[1, 2] = 1;
            boat[2, 1] = 1;

            return IsThat(boat, 5, 5, x, y, review);
        }

        private bool IsLongShip(int x, int y, bool[,] review)
        {
            int[,] ship = new int[6, 6];

            ship[4, 0] = 2;
            ship[0, 4] = 2;
            ship[0, 5] = 2;
            ship[1, 5] = 2;
            ship[5, 0] = 2;
            ship[5, 1] = 2;

            ship[1, 1] = 1;
            ship[1, 2] = 1;
            ship[2, 1] = 1;
            ship[4, 4] = 1;
            ship[2, 3] = 1;
            ship[3, 2] = 1;
            ship[3, 4] = 1;
            ship[4, 3] = 1;

            return IsThat(ship, 5, 5, x, y, review);
        }

        private bool IsLongShip2(int x, int y, bool[,] review)
        {
            int[,] ship = new int[6, 6];

            ship[1, 0] = 2;
            ship[5, 4] = 2;
            ship[5, 5] = 2;
            ship[4, 5] = 2;
            ship[0, 0] = 2;
            ship[0, 1] = 2;

            ship[4, 1] = 1;
            ship[4, 2] = 1;
            ship[3, 1] = 1;
            ship[1, 4] = 1;
            ship[3, 3] = 1;
            ship[2, 2] = 1;
            ship[2, 4] = 1;
            ship[1, 3] = 1;

            return IsThat(ship, 5, 5, x, y, review);
        }

        public override Terrain Ter()
        {
            return terrain;
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
            return timeScan;
        }
    }
}
