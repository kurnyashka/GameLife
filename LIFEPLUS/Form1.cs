using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LIFEPLUS
{
    public partial class Form1 : Form
    {
        Terrain terrain;
        bool isGrid;

        public Form1()
        {
            InitializeComponent();
            isGrid = false;
            terrain = new Terrain();
            terrain = new Scanner(terrain);
            terrain = new Statistics(terrain);
            timer1.Interval = 300;
            button_Pause.Text = "Пауза";
            timer1.Enabled = true;
        }

        private void pictureBox1_Click(object sender, EventArgs e){}

        private void button_Pause_Click(object sender, EventArgs e)
        {
            if (button_Pause.Text == "Пауза")
            {
                button_Pause.Text = "Далее";
                timer1.Enabled = false;
            }
            else
            {
                button_Pause.Text = "Пауза";
                timer1.Enabled = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            terrain = new Terrain();
            terrain = new Scanner(terrain);
            terrain = new Statistics(terrain);
        }

        private void grid_CheckedChanged(object sender, EventArgs e)
        {
            if (isGrid)
            {
                terrain = terrain.Ter();
                isGrid = false;
            }
            else
            {
                terrain = new Lattice(terrain);
                isGrid = true;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            terrain.Draw(pictureBox1.Image);
            terrain.MakeTurn();

            timeOfScan.Text = Convert.ToString(terrain.TimeScan() + " сек");
            number.Text = Convert.ToString(terrain.Stars());
            timeOfStep.Text = Convert.ToString(terrain.Time()) + " сек";
            percent.Text = Convert.ToString(terrain.Delta()) + "%";
        }
    }
}
