using Roplabda;
using System.Text;
using static System.Reflection.Metadata.BlobBuilder;

var fileData = await File.ReadAllLinesAsync("adatok.txt", Encoding.UTF8);

var players = new List<Player>();

foreach (var line in fileData)
{
    var data = line.Split('\t');
    players.Add(new Player
    {
        Name = data[0],
        Height = int.Parse(data[1]),
        Post = data[2],
        Nationality = data[3],
        Team = data[4],
        Country = data[5],
    });
}
//Írjuk ki a képernyőre az össz adatot
Console.WriteLine("Összes adat:");
foreach (var player in players) {
    Console.WriteLine(player);
}

//Keressük ki az ütő játékosokat az utok.txt állömányba
var hitters = players.Where(x => x.Post == "ütõ").Select(x => x.ToStringPost());

await File.WriteAllLinesAsync("utok.txt", hitters, encoding: Encoding.UTF8);

//A csapattagok.txt állományba mentsük a csapatokat és a hozzájuk tartozó játékosokat
var groupedByTeam = players
    .GroupBy(b => b.Team)
    .OrderBy(g => g.Key);

using (var writer = new StreamWriter("csapattagok.txt", false, Encoding.UTF8))
{
    foreach (var group in groupedByTeam)
    {
        await writer.WriteAsync($"{group.Key}: ");
        foreach (var player in group)
        {
            await writer.WriteAsync($"{player.Name}, ");
        }
        await writer.WriteLineAsync();
    }
}

//Rendezzük a játékosokat magasság szerint növekvő sorrendbe és a magaslatok.txt állományba mentsük el.
var highs = players.OrderBy(x => x.Height).Select(x => x.ToString());

await File.WriteAllLinesAsync("magaslatok.txt", highs, encoding: Encoding.UTF8);

// Mutassuk be a nemzetisegek.txt állományba, hogy mely nemzetiségek képviseltetik magukat a röplabdavilágban mint játékosok és milyen számban.
var groupedByNationality = players
    .GroupBy(b => b.Nationality)
    .OrderBy(g => g.Key);

using (var writer = new StreamWriter("nemzetisegek.txt", false, Encoding.UTF8))
{
    foreach (var group in groupedByNationality)
    {
        await writer.WriteLineAsync($"{group.Key}: ");
        foreach (var player in group)
        {
            await writer.WriteLineAsync($"\t - {player.Name} -- {player.Post}");
        }
        await writer.WriteLineAsync();
    }
}

//atlagnalmagasabbak.txt állományba keressük azon játékosok nevét és magasságát akik magasabbak mint az „adatbázisban” szereplő játékosok átlagos magasságánál.
var sumOfHeights = 0;
var numberOfPlayers = 0;

foreach (var player in players) 
{ 
    sumOfHeights += player.Height;
    numberOfPlayers++;
}

var averageHeight = sumOfHeights / numberOfPlayers;

var playersHigherThanAverage = players.Where(x => x.Height >= averageHeight).Select(x => x.ToString());

await File.WriteAllLinesAsync("atlagnalmagasabbak.txt", playersHigherThanAverage, encoding: Encoding.UTF8);

//Állítsa növekvő sorrendbe a posztok szerint a játékosok ösz magasságát
var groupByPost = players.GroupBy(x => x.Post)
                         .OrderBy(g => g.Key);

foreach (var group in groupByPost) {
    var groupOrder = group.OrderBy(x => x.Height);
    Console.WriteLine($"{group.Key}: ");
    foreach (var player in groupOrder) {
        Console.WriteLine($"\t - {player.Name}, {player.Height}");    
    }
}

//Egy szöveges állományba, „alacsonyak.txt” keresse ki a játékosok átlagmagasságától alacsonyabb játékosokat. Az állomány tartalmazza a játékosok nevét,  magasságát és hogy mennyivel alacsonyabbak az átlagnál, 2 tizedes pontossággal.
var playersShorterThanAverage = players.Where(x => x.Height < averageHeight).Select(x => x.ToStringDwarves());

await File.WriteAllLinesAsync("alacsonyak.txt", playersShorterThanAverage, encoding: Encoding.UTF8);