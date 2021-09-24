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

        public DataScraper()
        {
        }




        public bool TenderExist { get; private set; }
        public bool NetIsWork { get; private set; }
        public bool SelectorsOk = true;

        private void AllSuccess()
        {
            TenderExist = true;
            NetIsWork = true;
        }
        public PageOfDeliveryJsonModel GetBaseFields(int tenderId)

        {
            //базовый запрос
            var client = new RestClient(" https://api.market.mosreg.ru");
            var requestPageDelivery = new RestRequest("api/Trade/GetTradesForParticipantOrAnonymous", Method.POST);
            requestPageDelivery.AddJsonBody(new RequestPageDeliveryParams(tenderId));
            var respounse = client.Execute<PageOfDeliveryJsonModel>(requestPageDelivery);
            if (respounse.Data.Totalrecords == 0)
            {
                TenderExist = false;
                return new PageOfDeliveryJsonModel();
            }
            if (respounse.StatusCode != System.Net.HttpStatusCode.OK)
            {
                NetIsWork = false;
                return new PageOfDeliveryJsonModel();
            }
            AllSuccess();
            return respounse.Data;


        }
        public AdditionData GetAdditionData(int tenderId)
        {
            SelectorsOk = true;
            var client = new RestClient("https://market.mosreg.ru");
            var request = new RestRequest("/Trade/ViewTrade/" + tenderId);
            var respounse = client.Execute(request);
            string plaseOfDelivery;
            var Lots = new List<Lot>();
            if (respounse.ResponseUri.ToString() == "https://market.mosreg.ru/Trade")
            {
                TenderExist = false;
                return new AdditionData();
            }
            if (respounse.StatusCode != System.Net.HttpStatusCode.OK)
            {
                NetIsWork = false;
                return new AdditionData();
            }
            var pageNotification = new HtmlParser().ParseDocument(respounse.Content);
            //получение места доставки
            try
            {
                 plaseOfDelivery = pageNotification.QuerySelectorAll(".informationAboutCustomer__informationPurchase-infoBlock").ToList().First(elem => elem.FirstElementChild.InnerHtml == "Место поставки:").QuerySelector("p").TextContent;
                //получение данных о товарах
                var ElementsLots = pageNotification.QuerySelectorAll(".outputResults__oneResult");
;
                foreach (var lot in ElementsLots)
                {
                    var listFilds = lot.QuerySelectorAll("div [class$=parag]").ToList();

                    var name = GetValueField("Наименование", listFilds);
                    var unit = GetValueField("Единицы измерения", listFilds);
                    var count = GetValueField("Количество", listFilds);
                    var price = GetValueField("Стоимость единицы", listFilds);

                    Lots.Add(new Lot(name, unit, count, price));
                }
            }
            catch (Exception)
            {
                SelectorsOk = false;
                return new AdditionData();
            }

            AllSuccess();
            return new AdditionData(plaseOfDelivery, Lots);
        }
        private string GetValueField(string elementAtribute, List<IElement> elements) =>
            elements.First(elem => elem.FirstElementChild.TextContent.Contains(elementAtribute)).TextContent.Split(':')[1].Trim();
        public List<DocumentJsonModel> GetDocuments(int tenderId)
        {
            var client = new RestClient("https://api.market.mosreg.ru/");
            var request = new RestRequest($"/api/Trade/{tenderId}/GetTradeDocuments");
            var respounse = client.Execute<List<DocumentJsonModel>>(request);

            if (respounse.Content.Contains("не найден") && respounse.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                TenderExist = false;
                return new List<DocumentJsonModel>();
            }
            if (respounse.StatusCode != System.Net.HttpStatusCode.OK)
            {
                NetIsWork = false;
                return new List<DocumentJsonModel>();
            }
            AllSuccess();
            return respounse.Data;
        }
    }

    public class AdditionData
    {
        public AdditionData()
        {
        }

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

    public class RequestPageDeliveryParams
    {
        public RequestPageDeliveryParams(int tenderId)
        {
            Page = 1;
            ItemsPerPage = 10;
            this.Id = tenderId;
        }

        public int Page { get; set; }
        public int ItemsPerPage { get; set; }
        public int Id { get; set; }
    }
}
