using Eu;
using System.Text;

var filedata = await File.ReadAllLinesAsync("EUcsatlakozas.txt", Encoding.UTF8);

var countries = new List<Country>();

foreach (var line in filedata)
{
    var data = line.Split(';');
    countries.Add(new Country
    {
        Name = data[0],
        JoinDate = DateTime.Parse(data[1])
    });
}

var sumOfCountriesIn2018 = countries.Where(c => c.JoinDate.Year <= 2018).Count();
Console.WriteLine($"3. feladat: EU tagállamainak száma: {sumOfCountriesIn2018}");

var sumOfJoinedIn2007 = countries.Where(c => c.JoinDate.Year == 2007).Count();
Console.WriteLine($"4. feladat: 2007-ben {sumOfJoinedIn2007} ország csatlakozott."); 

var dateOfHungaryJoin = countries.Where(c => c.Name.Contains("Magyar")).First().JoinDate;
Console.WriteLine($"5. feladat: Magyarország csatlakozásának időpontja: {dateOfHungaryJoin:yyyy.MM.dd}");

var whichMonthDidHungaryJoin = dateOfHungaryJoin.Month;
Console.WriteLine($"6. feladat: Magyarország a(z) {whichMonthDidHungaryJoin}. hónapban csatlakozott.");

var statistics = new Dictionary<int, int>();
foreach (var country in countries)
{
    var year = country.JoinDate.Year;
    if (statistics.ContainsKey(year))
    {
        statistics[year]++;
    }
    else
    {
        statistics[year] = 1;
    }
}
Console.WriteLine("7. feladat: Statisztika");
foreach (var stat in statistics)
{
    Console.WriteLine($"\t{stat.Key}: {stat.Value} ország");
}
