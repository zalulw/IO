namespace Roplabda;

public class Player
{
    public string Name { get; set; }

    public int Height { get; set; }

    public string Post { get; set; }

    public string Nationality { get; set; }

    public string Team { get; set; }

    public string Country { get; set; }

    public double AverageHeight = 186;

    public string ToStringPost()
    {
        return $"{Name} - {Post}";
    }
    public string ToStringDwarves()
    {
        return $"{Name}, {Height}, {AverageHeight - Height}";
    }

    public override string ToString()
    {
        return $"{Name}\t{Height}\t{Post}\t{Nationality}\t{Team}\t{Country}";
    }
}
