using Solution;
using System.Text;
using System.Linq;
using System.Globalization;

var filedata = await File.ReadAllLinesAsync("vb2018.txt", Encoding.UTF8);

var stadiums = new List<Stadium>();

//filedata.Skip(1);
foreach (var line in filedata.Skip(1))
{
    var data = line.Split(';');
    if (data.Length < 4) continue; 

    if (!int.TryParse(data[3].Trim(), out var capacity))
        capacity = 0; 

    stadiums.Add(new Stadium
    {
        CityName = data[0].Trim(),
        StadiumName = data[1].Trim(),
        AltName = data[2].Trim(),
        Capacity = capacity
    });
}

//Console.WriteLine(filedata);

//3 jelenitse meg a kepernyon hany stadionban jatszottak
var uniqueStadiums = stadiums.Select(g => g.StadiumName).Distinct().Count();
Console.WriteLine($"3. feladat: {uniqueStadiums} stadionban játszottak.");

//4 legkevesebb ferohelyes adatai
if (stadiums.Count == 0)
{
    Console.WriteLine("4. feladat: Nincsenek stadion adatok.");
}
else
{
    var minCapacity = stadiums.Min(s => s.Capacity);
    var minStadiums = stadiums.Where(s => s.Capacity == minCapacity).ToList();

    Console.WriteLine("4. feladat: A legkevesebb férőhelyes stadion adatai:");
    foreach (var s in minStadiums)
    {
        Console.WriteLine($"Város: {s.CityName}\nStadion: {s.StadiumName}\nAlternatív név: {s.AltName}\nFérőhelyek: {s.Capacity}", Encoding.UTF8);
    }
}

//5 stadionok férőhelyének átlaga egy tizedesjegyre kerekítve
var validCapacities = stadiums
    .Select(s => s.Capacity)
    .Where(c => c > 0)
    .ToList();

if (validCapacities.Count == 0)
{
    Console.WriteLine("5. feladat: Nincsenek érvényes férőhely adatok az átlag számításához.");
}
else
{
    var average = validCapacities.Average();
    var formatted = average.ToString("F1", CultureInfo.InvariantCulture);
    Console.WriteLine($"5. feladat: A stadionok átlagos férőhelyszáma: {formatted}");
}

//6 számolja meg hány stadion rendelkezik névvel, kiiratni
var altNamedStadiums = stadiums.Where(s => s.AltName.Length > 4).Count();
Console.WriteLine($"6. feladat: {altNamedStadiums} db stadion rendelkezik alternatív névvel");

//7 
Console.WriteLine("Adja meg egy város nevét: ");

string? city = Console.ReadLine();
while (string.IsNullOrWhiteSpace(city) || city.Length < 3)
{
    Console.WriteLine("A város neve legalább 3 karakter hosszú legyen! Kérem, adja meg újra:");
    city = Console.ReadLine();
}

//8 
var searchCity = city!.Trim();
bool foundCity = false;

foreach (var s in stadiums)
{
    if (string.Equals(s.CityName, searchCity, StringComparison.OrdinalIgnoreCase))
    {
        Console.WriteLine($"7. feladat: A megadott város megtalálható: {s.CityName}\nStadion: {s.StadiumName}\nAlternatív név: {s.AltName}\n");
        foundCity = true;
        break; 
    }
}

if (!foundCity)
{
    Console.WriteLine("7. feladat: A megadott város nem található a stadionok között.");
}

