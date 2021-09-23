using RestSharp;
using AngleSharp;
using System;
using AngleSharp.Html.Parser;
using System.Linq;
using System.Collections;
using StringTools;

namespace TestPoligon
{
    public class RunTest
    {
      static  public void Run(string tenderNum)
        {
            //базовый запрос
            var client = new RestClient(" https://api.market.mosreg.ru");
            var request = new RestRequest("api/Trade/GetTradesForParticipantOrAnonymous", Method.POST);
            request.AddParameter("page",1);
            request.AddParameter("itemsPerPage", 10);
            request.AddParameter("id", 1763198);
            var respBaseData = client.Execute(request);//json ответ
            //получение страницы извещения
            var pageNotification = new RestClient("https://market.mosreg.ru/Trade/ViewTrade/1763198").Execute(
                new RestRequest("")
                ,Method.GET);
            //настройка AngleSharp
            var config = Configuration.Default;
            var context = BrowsingContext.New(config);
            var document = new HtmlParser().ParseDocument(pageNotification.Content);
            //получение места доставки
            var plaseOfDelivery =document.QuerySelectorAll(".informationAboutCustomer__informationPurchase-infoBlock").ToList().First(elem=>elem.FirstElementChild.InnerHtml== "Место поставки:").QuerySelector("p").InnerHtml;
            //получение данных о товарах
            var goods = document.QuerySelectorAll(".outputResults__oneResult");

            foreach (var good in goods)
            {
                var listFilds= good.QuerySelectorAll("div [class$=parag]").ToList();
                var name = listFilds.First(elem => elem.FirstElementChild.InnerHtml.Contains("Наименование")).InnerHtml.Substring("</span> ","\n");
                var unit = listFilds.First(elem => elem.FirstElementChild.InnerHtml.Contains("Единицы измерения")).InnerHtml.Substring("</span> ", "\n");
                var count = listFilds.First(elem => elem.FirstElementChild.InnerHtml.Contains("Количество")).InnerHtml.Substring("</span> ", "\n");
                var price = listFilds.First(elem => elem.FirstElementChild.InnerHtml.Contains("Стоимость единицы")).InnerHtml.Substring("</span> ", "\n");
            }

        }
    }
}
