using GetCards_CP;
using HtmlAgilityPack;
using System.Text;
using System.Web;

var cards = new List<Card>();
var url = new HtmlWeb().LoadFromWebAsync("https://scryfall.com/sets/blb").Result;
var linksList = url.DocumentNode.SelectNodes("//*[@id=\"main\"]/div[2]/div/div/a");
if (linksList != null)
{
    Parallel.ForEach(linksList, link =>
    {
        var cardLink = link.GetAttributeValue("href", null);
        var cardHtml = new HtmlWeb().LoadFromWebAsync(cardLink).Result;
        var title = cardHtml.DocumentNode.SelectSingleNode("//*[@id=\"main\"]/div[1]/div/div[3]/h1/span[1]").InnerHtml.Trim();

        if (title != "Page Not Found")
        {
            var description = cardHtml.DocumentNode.SelectNodes("//*[@id=\"main\"]/div[1]/div/div[3]/div/div/p");
            var cardDescription = "";
            foreach (var p in description)
            {
                cardDescription += p.InnerHtml + " ";
            }

            cards.Add(new Card(
                title,
                cardDescription
            ));

        }

    });

    StringBuilder file = new StringBuilder();
    cards.ForEach(card => file.AppendLine($"Card: {card.name}\n\n{card.description}\n\n"));
    File.WriteAllText("cards.txt", file.ToString());
    Console.WriteLine(".txt file generated successfully!");
}
