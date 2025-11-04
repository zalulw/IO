namespace konyvek;

public class Book
{
    public string FirstName { get; set; }

    public string LastName { get; set; }

    public DateTime BirthDate { get; set; }

    public string Title { get; set; }

    public string ISBN { get; set; }

    public string Publisher { get; set; }

    public int ReleaseYear { get; set; }

    public int Price { get; set; }

    public string Theme { get; set; }

    public int NumberOfPages { get; set; }

    public int Honorary { get; set; }

    public string ToFullyString()
    {
        return $"{FirstName}\t{LastName}\t{BirthDate:yyyy-MM-dd}\t{Title}\t{ISBN}\t{Publisher}\t{ReleaseYear}\t{Price}\t{Theme}\t{NumberOfPages}\t{Honorary}";
    }

    public override string ToString()
    {
        return $"{FirstName} {LastName}({BirthDate:yyyy-MM-dd}), - {Title}({ReleaseYear})";
    }
}
