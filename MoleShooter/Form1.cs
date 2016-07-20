//#define Debug
namespace MoleShooter
{
    using System;
    using System.Drawing;
    using System.Media;
    using System.Windows.Forms;
    using Models;
    using Properties;

    public partial class MoleShooter : Form
    {
        private const int SplashNum = 3;

#if Debug
        private int currX = 0;
        private int currY = 0;
#endif
        private int moleCounter;
        private int splashCounter;
        private Mole mole;
        private MenuBoard menuBoard;
        private ScoreBoard scoreBoard;
        private BloodSplash bloodSplash;
        private Random random;
        private bool isDead = false;

        private int hits;
        private int misses;
        private int shotsFired;
        private double avgHits;
        private int frameNum = 8;
        private string level = "Noob";

        public MoleShooter()
        {
            InitializeComponent();

            Bitmap bmp = Resources.Crosshair;
            this.Cursor = CustomCursor.CreateCursor(bmp, bmp.Height / 2, bmp.Width / 2);

            this.mole = new Mole(10, 200);
            this.menuBoard = new MenuBoard(400, 60);
            this.scoreBoard = new ScoreBoard(10, -20);
            this.bloodSplash = new BloodSplash(0, 0);
            this.random = new Random();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void timeGameLoop_Tick(object sender, EventArgs e)
        {
            if (this.avgHits <= 30)
            {
                this.frameNum = 8;
                this.level = "Noob";
            }
            else if (this.avgHits <= 50)
            {
                this.frameNum = 6;
                this.level = "Medium";
            }
            else if (this.avgHits >= 75)
            {
                this.frameNum = 6;
                this.level = "Pro";
            }

            if (this.moleCounter >= this.frameNum)
            {
                this.UpdateMole();
                this.moleCounter = 0;
            }

            if (this.isDead)
            {
                if (this.splashCounter >= SplashNum)
                {
                    this.splashCounter = 0;
                    this.UpdateMole();
                    this.isDead = false;
                }

                this.splashCounter++;
            }

            this.moleCounter++;
            this.Refresh();
        }

        private void UpdateMole()
        {
            this.mole.Update(this.random.Next(Resources.Mole.Width, this.Width - Resources.Mole.Width),
                            this.random.Next(this.Height/2, this.Height - Resources.Mole.Height * 2));
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics dc = e.Graphics;

#if Debug
            TextRenderer.DrawText(
                dc,
                $"X = {this.currX} : Y = {this.currY}",
                font,
                new Rectangle(0, 0, 150, 20),
                SystemColors.ControlText,
                textFormatFlags);
           dc.DrawRectangle(new Pen(Color.Black, 3), mole.moleHotSpot.X, this.mole.moleHotSpot.Y, this.mole.moleHotSpot.Width, this.mole.moleHotSpot.Height);

#endif
            if (this.isDead)
            {
                this.bloodSplash.DrawImage(dc);
            }
            else
            {
                if (this.moleCounter >= 1)
                {
                    this.mole.DrawImage(dc);
                }
            }

            this.menuBoard.DrawImage(dc);
            this.scoreBoard.DrawImage(dc);

            TextFormatFlags textFormatFlags = TextFormatFlags.Left | TextFormatFlags.EndEllipsis;
            Font font = new Font("Stencil", 10, FontStyle.Bold);
            TextRenderer.DrawText(dc, $"SHOTS: {this.shotsFired}", font, new Rectangle(40, 30, 120, 20), Color.Black,textFormatFlags);
            TextRenderer.DrawText(dc, $"HITS: {this.hits}", font, new Rectangle(40, 47, 120, 20), Color.Black, textFormatFlags);
            TextRenderer.DrawText(dc, $"MISSES: {this.misses}", font, new Rectangle(40, 69, 120, 20), Color.Black, textFormatFlags);
            TextRenderer.DrawText(dc, $"AVG: {this.avgHits:F0} %", font, new Rectangle(40, 85, 120, 20), Color.Black, textFormatFlags);
            TextRenderer.DrawText(dc, $"Level: {this.level}", new Font("Times New Roman", 13, FontStyle.Bold), new Rectangle(40, 110, 200, 80), Color.Black, textFormatFlags);

            base.OnPaint(e);
        }

        private void MoleShooter_MouseMove(object sender, MouseEventArgs e)
        {
#if Debug
            this.currX = e.X;
            this.currY = e.Y;

#endif
            this.Refresh();
        }

        private void MoleShooter_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.X >= 412 && e.X <= 570 && e.Y >= 68 && e.Y <= 100)
            {
                this.timeGameLoop.Start();
            }

            else if (e.X >= 412 && e.X <= 570 && e.Y >= 125 && e.Y <= 167)
            {
                this.timeGameLoop.Stop();
                this.moleCounter = 0;
                this.splashCounter = 0;
                this.isDead = false;
                this.hits = 0;
                this.shotsFired = 0;
                this.avgHits = 0;
                this.misses = 0;
                this.level = "Noob";
            }
            else if (e.X >= 412 && e.X <= 570 && e.Y >= 194 && e.Y <= 230)
            {
                Application.Exit();
            }
            else if(this.moleCounter >= 1)
            {
                if (this.mole.Hit(e.X, e.Y))
                {
                    this.isDead = true;
                    this.bloodSplash.Left = this.mole.Left;
                    this.bloodSplash.Top = this.mole.Top;
                    this.hits++;
                }

                this.ProduceGunshot();
                this.shotsFired++;
                this.misses = this.shotsFired - this.hits;
                this.avgHits = (double) this.hits/this.shotsFired * 100;
            }

        }

        private void ProduceGunshot()
        {
            SoundPlayer gunSound = new SoundPlayer(Resources.GunShot);
            gunSound.Play();
        }
    }
}