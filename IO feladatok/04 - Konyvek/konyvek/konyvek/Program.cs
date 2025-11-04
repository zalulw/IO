using konyvek;
using System.Text;

var fileData = await File.ReadAllLinesAsync("adatok.txt", Encoding.UTF7);

var books = new List<Book>();

foreach (var line in fileData)
{
    var data = line.Split('\t');
    books.Add(new Book
    {
        FirstName = data[0],
        LastName = data[1],
        BirthDate = DateTime.Parse(data[2]),
        Title = data[3],
        ISBN = data[4],
        Publisher = data[5],
        ReleaseYear = int.Parse(data[6]),
        Price = int.Parse(data[7]),
        Theme = data[8],
        NumberOfPages = int.Parse(data[9]),
        Honorary = int.Parse(data[10])
    });
}

//Írjuk ki a képernyőre az össz adatot
Console.WriteLine($"1. feladat (ossz adat): \n");
foreach (var book in books) {
    Console.WriteLine(book);
}

//Keressük ki az informatika témajú könyveket és mentsük el őket az informatika.txt állömányba
var it = fileData.Where(x => x.Contains("informatika"));

await File.WriteAllLinesAsync("informatika.txt", it, encoding: Encoding.UTF8);


//Az 1900.txt állományba mentsük el azokat a könyveket amelyek az 1900-as években íródtak
var booksInThe1900s = books.Where(x => x.ReleaseYear >= 1900)
                           .Where(x => x.ReleaseYear < 2000).Select(x => x.ToFullyString());

await File.WriteAllLinesAsync("1900.txt", booksInThe1900s, encoding: Encoding.UTF8);

//Rendezzük az adatokat a könyvek oldalainak száma szerint csökkenő sorrendbe és a sorbarakott.txt állományba mentsük el.
var sortedBypages = books.OrderByDescending(x => x.NumberOfPages)
                         .Select(x => x.ToFullyString());

await File.WriteAllLinesAsync("sorbarakott.txt", sortedBypages, encoding: Encoding.UTF8);

//„kategoriak.txt” állományba mentse el a könyveket téma szerint.
var groupedByTheme = books
    .GroupBy(b => b.Theme)
    .OrderBy(g => g.Key);

using (var writer = new StreamWriter("kategoriak.txt", false, Encoding.UTF8))
{
    foreach (var group in groupedByTheme)
    {
        await writer.WriteLineAsync($"{group.Key}:");
        foreach (var book in group)
        {
            await writer.WriteLineAsync($"\t - {book.Title}");
        }
        await writer.WriteLineAsync();
    }
}


Console.ReadKey();