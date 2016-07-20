#define Debug
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
            if (this.moleCounter >= 10)
            {
                this.UpdateMole();
                this.moleCounter = 0;
            }

            if (this.isDead)
            {
                if (this.splashCounter >= 10)
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
            TextFormatFlags textFormatFlags = TextFormatFlags.Left | TextFormatFlags.EndEllipsis;
            Font font = new Font("Stencil", 12, FontStyle.Regular);
            TextRenderer.DrawText(
                dc,
                $"X = {this.currX} : Y = {this.currY}",
                font,
                new Rectangle(0, 0, 150, 20),
                SystemColors.ControlText,
                textFormatFlags);
           // dc.DrawRectangle(new Pen(Color.Black, 3), mole.moleHotSpot.X, this.mole.moleHotSpot.Y, this.mole.moleHotSpot.Width, this.mole.moleHotSpot.Height);

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
            }
            else if (e.X >= 412 && e.X <= 570 && e.Y >= 194 && e.Y <= 230)
            {
                Application.Exit();
            }
            else
            {
                if (this.mole.Hit(e.X, e.Y))
                {
                    this.isDead = true;
                    this.bloodSplash.Left = this.mole.Left;
                    this.bloodSplash.Top = this.mole.Top;
                }

                this.ProduceGunshot();
            }

        }

        private void ProduceGunshot()
        {
            SoundPlayer gunSound = new SoundPlayer(Resources.GunShot);
            gunSound.Play();
        }
    }
}