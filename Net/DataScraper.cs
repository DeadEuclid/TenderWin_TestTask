using AngleSharp.Dom;
using AngleSharp.Html.Parser;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Net
{
    public class DataScraper
    {
        public DataScraper(int tenderId)
        {
            this.tenderId = tenderId;
        }

        public DataScraper()
        {
        }

        public int tenderId { get; set; }




        public PageOfDeliveryJsonModel GetBaseFields(int tenderId)

        {
            //базовый запрос
            var client = new RestClient(" https://api.market.mosreg.ru");
            var requestPageDelivery = new RestRequest("api/Trade/GetTradesForParticipantOrAnonymous", Method.POST);
            requestPageDelivery.AddJsonBody(new requestPageDeliveryParams(tenderId));
            var respounse = client.Execute<PageOfDeliveryJsonModel>(requestPageDelivery);
            if (respounse.Data.totalrecords==0)
            {
                throw new ArgumentException("Тендера с данным номером не существует");
            }

            return respounse.Data;


        }
        public AdditionData GetAdditionData(int tenderId)
        {
            var client = new RestClient("https://market.mosreg.ru");
            var request = new RestRequest("/Trade/ViewTrade/"+tenderId);
            var respounse = client.Execute(request);

            if (respounse.ResponseUri.ToString() == "https://market.mosreg.ru/Trade")
                throw new ArgumentException("Тендера с данным номером не существует");

            var pageNotification = new HtmlParser().ParseDocument(respounse.Content);
            //получение места доставки
            var plaseOfDelivery = pageNotification.QuerySelectorAll(".informationAboutCustomer__informationPurchase-infoBlock").ToList().First(elem => elem.FirstElementChild.InnerHtml == "Место поставки:").QuerySelector("p").TextContent;
            //получение данных о товарах
            var ElementsLots = pageNotification.QuerySelectorAll(".outputResults__oneResult");
            var Lots = new List<Lot>();
            foreach (var lot in ElementsLots)
            {
                var listFilds = lot.QuerySelectorAll("div [class$=parag]").ToList();

                var name = getValueField("Наименование", listFilds);
                var unit = getValueField("Единицы измерения", listFilds);
                var count = getValueField("Количество", listFilds);
                var price = getValueField("Стоимость единицы", listFilds);

                Lots.Add(new Lot(name, unit, count, price));
            }
            return new AdditionData(plaseOfDelivery, Lots);
        }
        private string getValueField(string elementAtribute, List<IElement> elements) =>
            elements.First(elem => elem.FirstElementChild.TextContent.Contains(elementAtribute)).TextContent.Split(':')[1].Trim();
        public List<DocumentJsonModel> GetDocuments(int tenderId)
        {
            var client = new RestClient("https://api.market.mosreg.ru/");
            var request = new RestRequest($"/api/Trade/{tenderId}/GetTradeDocuments");
            var respounse=client.Execute<List<DocumentJsonModel>>(request);
            if (respounse.Content.Contains("не найден")&&respounse.StatusCode==System.Net.HttpStatusCode.BadRequest)
            {
                throw new ArgumentException("Тендера с данным номером не существует");
            }

            return respounse.Data;
        }
    }

    public class AdditionData
    {
        public AdditionData(string plaseOfDelivery, List<Lot> lots)
        {
            PlaseOfDelivery = plaseOfDelivery;
            Lots = lots;
        }

        public string PlaseOfDelivery { get; set; }
        public List<Lot> Lots { get; set; }
    }
    public class Lot
    {
        public Lot(string name, string unit, string count, string price)
        {
            Name = name;
            Unit = unit;
            Count = count;
            Price = price;
        }

        public string Name { get; set; }
        public string Unit { get; set; }
        public string Count { get; set; }
        public string Price { get; set; }
    }

    public class requestPageDeliveryParams
    {
        public requestPageDeliveryParams(int tenderId)
        {
            page = 1;
            itemsPerPage = 10;
            this.id = tenderId;
        }

        public int page { get; set; }
        public int itemsPerPage { get; set; }
        public int id { get; set; }
    }
}
