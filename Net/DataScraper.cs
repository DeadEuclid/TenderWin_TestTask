using AngleSharp.Dom;
using AngleSharp.Html.Parser;
using FluentResults;
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
            MarketClient = new RestClient("https://market.mosreg.ru");
            ApiClient = new RestClient(" https://api.market.mosreg.ru");
        }

        private readonly RestClient MarketClient;
        private readonly RestClient ApiClient;
        private readonly Result NetFailedResult = Result.Fail("Ошибка подключения к сети или проблема на стороне сервера, повторите попытку позднее.");
        private readonly Result TenderNotExistResult = Result.Fail("Тендера с данным номером нет. Повторите попытку.");


        public Result<PageOfDeliveryJsonModel> GetBaseFields(int tenderId)

        {
            //базовый запрос

            var requestPageDelivery = new RestRequest("api/Trade/GetTradesForParticipantOrAnonymous", Method.POST);
            requestPageDelivery.AddJsonBody(new RequestPageDeliveryParams(tenderId));
            var respounse = ApiClient.Execute<PageOfDeliveryJsonModel>(requestPageDelivery);

            if (respounse.StatusCode != System.Net.HttpStatusCode.OK)
            {

                return NetFailedResult;
            }
            if (respounse.Data.Totalrecords == 0)
            {
                return TenderNotExistResult;

            }
            return Result.Ok(respounse.Data);


        }
        public Result<AdditionData> GetAdditionData(int tenderId)
        {
            var request = new RestRequest("/Trade/ViewTrade/" + tenderId);
            var respounse = MarketClient.Execute(request);
            string plaseOfDelivery;
            var Lots = new List<Lot>();
            if (respounse.StatusCode != System.Net.HttpStatusCode.OK)
            {

                return NetFailedResult;
            }
            if (respounse.ResponseUri.ToString().Equals("https://market.mosreg.ru/Trade"))
            {
                return TenderNotExistResult;
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

                return Result.Fail("Ошибка получения данных о месте доставки и лотах, обратитесь к разработчику приложения");
            }
            return Result.Ok(new AdditionData(plaseOfDelivery, Lots));
        }
        private string GetValueField(string elementAtribute, List<IElement> elements)
        {
            return elements.First(elem => elem.FirstElementChild.TextContent.Contains(elementAtribute)).TextContent.Split(':')[1].Trim();
        }

        public Result<List<DocumentJsonModel>> GetDocuments(int tenderId)
        {
            var request = new RestRequest($"/api/Trade/{tenderId}/GetTradeDocuments");
            var respounse = ApiClient.Execute<List<DocumentJsonModel>>(request);

            if (respounse.StatusCode != System.Net.HttpStatusCode.OK && respounse.StatusCode != System.Net.HttpStatusCode.BadRequest)
            {
                return NetFailedResult;
            }
            if (respounse.Content.Contains("не найден") && respounse.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                return TenderNotExistResult;
            }


            return Result.Ok(respounse.Data);
        }
    }
}
