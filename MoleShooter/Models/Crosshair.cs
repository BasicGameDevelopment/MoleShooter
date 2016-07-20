namespace MoleShooter.Models
{
    using Properties;

    public class Crosshair : ImageBase
    {
        public Crosshair(int x, int y) 
            : base(Resources.Crosshair, x, y)
        {
        }
    }
}
