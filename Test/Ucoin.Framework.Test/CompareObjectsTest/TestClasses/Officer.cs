namespace Ucoin.Framework.Test
{
    public class Officer
    {
        public string Name { get; set; }
        public Deck Type { get; set; }
        public Deck? Type2 { get; set; }
    }

    public enum Deck
    {
        Engineering,
        SickBay,
        AstroPhysics
    }
}
