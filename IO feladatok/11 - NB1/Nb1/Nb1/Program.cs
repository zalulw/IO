using Nb1;
using System.Reflection.Metadata.Ecma335;
using System.Text;

var fileData = await File.ReadAllLinesAsync("adatok.txt", Encoding.UTF8);

var players = new List<Player>();

foreach (var line in fileData)
{
    var data = line.Split('\t').ToList();

    if(data.Count == 8)
    {
        data.Insert(3, string.Empty);
    }

    players.Add(new Player
    {
        ClubName = data[0],
        Number = int.Parse(data[1]),
        LastName = data[2],
        FamilyName = data[3],
        BirthDate = DateTime.Parse(data[4]),
        IsHungarian = data[5],
        IsForeign = data[6],
        Pay = int.Parse(data[7]),
        Position = data[8]
    });
}

var oldestFieldPlayer = players.Where(p => p.Position != "kapus").OrderBy(p => p.BirthDate).First();

Console.WriteLine($"1. feladat: A mezőnyjátékosok közül a legidősebb játékos: {oldestFieldPlayer.LastName} {oldestFieldPlayer.FamilyName}, születési dátum: {oldestFieldPlayer.BirthDate:yyyy.MM.dd}");

var hungarianCount = players.Count(p => p.IsHungarian == "-1");
var foreignCount = players.Count(p => p.IsForeign == "-1");
var dualCitizenCount = players.Count(p => p.IsHungarian == "-1" && p.IsForeign == "-1");
Console.WriteLine($"2. feladat: Magyar játékosok száma: {hungarianCount} fő, Külföldi játékosok száma: {foreignCount} fő, Kettős állampolgárságú játékosok száma: {dualCitizenCount} fő");


var totalValueByClub = players.GroupBy(p => p.ClubName).Select(g => new
{
    ClubName = g.Key,
    TotalValue = g.Sum(p => p.Pay)
});

Console.WriteLine("3. feladat: Csapatok összértéke:");
foreach (var club in totalValueByClub)
{
    Console.WriteLine($"\t{club.ClubName}: {club.TotalValue} Ft");
}

var uniquePositions = players.GroupBy(p => new {p.ClubName, p.Position})
                             .Select(g => new { g.Key.ClubName, g.Key.Position })
                             .GroupBy(x => x.ClubName)
                             .Select(g => new { ClubName = g.Key, PositionCount = g.Count() });

foreach (var pos in uniquePositions)
{
    Console.WriteLine($"4. feladat: {pos.ClubName} - {pos.PositionCount} különböző poszton játszanak a játékosok");
    break;
}


var averageValue = players.Average(p => p.Pay);
var belowAveragePlayers = players.Where(p => p.Pay < averageValue);
foreach (var player in belowAveragePlayers)
{
    Console.WriteLine($"5. feladat: {player.LastName} {player.FamilyName}, {player.ClubName}, {player.Pay} Ft");

    break;
}

var youngHungarianPlayers = players.Where(p => p.IsHungarian == "-1" && p.BirthDate.Year >= 2000);

if(!youngHungarianPlayers.Any())
{
    Console.WriteLine("6. feladat: Nincs 2000 után született magyar játékos");
}
else
{
    foreach (var player in youngHungarianPlayers)
    {
        Console.WriteLine($"6. feladat: {player.LastName} {player.FamilyName}, születési dátum: {player.BirthDate:yyyy.MM.dd}");
        break;
    }
}

var hungarianPlayers = players.Where(p => p.IsHungarian == "-1").GroupBy(p => p.ClubName);
var foreignPlayers = players.Where(p => p.IsForeign == "-1").GroupBy(p => p.ClubName);

await File.WriteAllLinesAsync("hazai.txt", hungarianPlayers.SelectMany(g => new[] { g.Key }.Concat(g.Select(p => $"{p.LastName} {p.FamilyName} {p.Position} {p.Pay}K EUR"))));
await File.WriteAllLinesAsync("legios.txt", foreignPlayers.SelectMany(g => new[] { g.Key }.Concat(g.Select(p => $"{p.LastName} {p.FamilyName} {p.Position} {p.Pay}K EUR"))));

Console.ReadKey();