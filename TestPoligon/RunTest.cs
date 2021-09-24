using RestSharp;
using AngleSharp;
using System;
using AngleSharp.Html.Parser;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace TestPoligon
{
    public class RunTest
    {
      static  public void Run(string tenderNum)
        {
            //базовый запрос
            var client = new RestClient(" https://api.market.mosreg.ru");
            var requestPageDelivery = new RestRequest("api/Trade/GetTradesForParticipantOrAnonymous", Method.POST);
            requestPageDelivery.AddJsonBody(new requestPageDeliveryParams(1763198));
            var requestDocuments = new RestRequest("https://api.market.mosreg.ru/api/Trade/1763198/GetTradeDocuments");
            var respBaseData = client.Execute<PageOfDeliveryJsonModel>(requestPageDelivery);//json ответ
            var documents = client.Execute<List<DocumentJsonModel>>(requestDocuments);//json ответ
            //получение страницы извещения
            var pageNotification = new RestClient("https://market.mosreg.ru/Trade/ViewTrade/1763198").Execute( new RestRequest("")
                ,Method.GET);

            var document = new HtmlParser().ParseDocument(pageNotification.Content);
            //получение места доставки
            var plaseOfDelivery =document.QuerySelectorAll(".informationAboutCustomer__informationPurchase-infoBlock").ToList().First(elem=>elem.FirstElementChild.TextContent== "Место поставки:").QuerySelector("p").TextContent;
            //получение данных о товарах
            var goods = document.QuerySelectorAll(".outputResults__oneResult");

            foreach (var good in goods)
            {
                var listFilds= good.QuerySelectorAll("div [class$=parag]").ToList();

                var name = listFilds.First(elem => elem.FirstElementChild.TextContent.Contains("Наименование")).TextContent.Split(':')[1].Trim();
                var unit = listFilds.First(elem => elem.FirstElementChild.InnerHtml.Contains("Единицы измерения")).TextContent.Split(':')[1].Trim();
                var count = listFilds.First(elem => elem.FirstElementChild.InnerHtml.Contains("Количество")).TextContent.Split(':')[1].Trim();
                var price = listFilds.First(elem => elem.FirstElementChild.InnerHtml.Contains("Стоимость единицы")).TextContent.Split(':')[1].Trim();
            }

        }
        public class requestPageDeliveryParams
        {
            public requestPageDeliveryParams(int id)
            {
                page = 1;
                itemsPerPage = 10;
                this.id = id;
            }

           public int page { get; set; }
           public int itemsPerPage { get; set; }
           public int id { get; set; }
        }
    }
}
