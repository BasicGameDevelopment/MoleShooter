#define Debug
namespace MoleShooter
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;
    using Models;

    public partial class MoleShooter : Form
    {
#if Debug
        private int currX = 0;
        private int currY = 0;
#endif
        private Mole mole;
        public MoleShooter()
        {
            InitializeComponent();
            this.mole = new Mole(10,200);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void timeGameLoop_Tick(object sender, EventArgs e)
        {

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
            dc.DrawRectangle(new Pen(Color.Black, 3), mole.moleHotSpot.X, this.mole.moleHotSpot.Y, this.mole.moleHotSpot.Width, this.mole.moleHotSpot.Height);

#endif
            this.mole.DrawImage(dc);
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
    }
}