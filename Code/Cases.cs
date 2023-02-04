using Bollekurs.Models;

namespace Bollekurs;

static class Cases
{
    public 
    static readonly Case[] Mat =
        {
            new Case("Ferdigmat", "Du skal argumentere for at det bare skal være ferdigmat.", new[] {"mat/ferdigmat.png"}),
            new Case("REAL Turmat", "Du skal argumentere for at det bare skal være REAL Turmat.", new[] {"mat/realturmat.png"}),
            new Case("Økologisk mat", "Du skal argumentere for at all maten skal være økologisk.", new[] {"mat/okologisk.png"}),
            new Case("Vegetarmat", "Du skal argumentere for at all maten skal være vegetarisk.", new[] {"mat/vegetar.png"}),
            new Case("Kortreis mat", "Du skal argumentere for at maten skal være mest mulig kortreist.", new[] {"mat/kortreist.png"}),
            new Case("Hjemmelaget", "Du skal argumentere for at all maten skal lages fra bunnen av.", new[] {"mat/hjemmelaget.png"}),
            new Case("Billig mat", "Du skal argumentere for at all maten skal være billigst mulig", new[] {"mat/billig.png"}),
            new Case("Bare brød", "NEI, NEI, NEI, gi meg brød med smør på! Og ikke noe mer!", new[] {"mat/brod.png"}),
            new Case("Restaurant", "Du skal argumentere for at vi skal spise på restaurant hver dag",new[] {"mat/restaurant.png"}),
        };

    public static readonly IDictionary<string, Case> Haiker = new Dictionary<string, Case>
        {
            //{"sykkel", new Case("Sykkel haik","Du skal argumentere for at haiken skal være med sykkel",new[]{"haik/sykkeltur.jpg"})},
            {"kano", new Case("Kano haik","Du skal argumentere for at haiken skal være med kano",new[]{"haik/kanotur.jpg","haik/kanotur2.jpg"})},
            {"ikke", new Case("Ikke haik","Du skal argumentere for å ikke ha haik",new[]{"haik/ikketur.jpg"})},
            {"by", new Case("By haik","Du skal argumentere for en by haik",new[]{"haik/byhaik.jpg"})},
            {"lang", new Case("Lang haik","Du skal argumentere for en lang haik",new []{"haik/Langtur haik.JPG"})},
            //{"kort", new Case("Kort haik","Du skal argumentere for en kort haik",new []{"haik/kort haik.JPG"})},
            //{"mat", new Case("Mat haik","Du skal argumentere for en mat-haik. \n En haik med god mat",new []{"haik/Mathaik.JPG"})},
            {"noysomhet", new Case("Nøysomhets haik","Du skal argumentere for en nøysomhets haik. \n Matbudsjett på 26kr per person.",new []{"haik/noysomhet.jpg"})},
            //{"jakke", new Case("Jakke haik","Du skal argumentere for en jakke haik. \n Du får kun ha med det du får plass til i ei jakke",new []{"haik/jakke.jpg"})},
        };
}
