using FluentResults;
using Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UI
{
    class Program
    {
        static void Main()
        {
            var formater = new FormaterDataOfTenders();
            var scraper = new DataScraper();

            while (true)
            {
                Console.WriteLine("Введите номер тендера:");
                if (!int.TryParse(Console.ReadLine(), out int tenderNumber))
                {
                    Console.WriteLine("Номер тендера введён не корректно повторите попытку\n");
                    continue;
                }

                var baseFieldsResult = scraper.GetBaseFields(tenderNumber);
                if (baseFieldsResult.IsFailed)
                {
                    Console.WriteLine(baseFieldsResult.Errors.First().Message);
                    continue;
                }
                else
                {
                var additionDataResult = scraper.GetAdditionData(tenderNumber);
                var documentsResult = scraper.GetDocuments(tenderNumber);
                    Console.WriteLine(formater.FormatAllResults(baseFieldsResult,additionDataResult,documentsResult));
                }

             
            }

        }

    }
    class FormaterDataOfTenders
    {
        public string FormatAllResults(Result<PageOfDeliveryJsonModel> pageOfDeliveryModel, Result<AdditionData> additionData, Result<List<DocumentJsonModel>> documents)
        {
            return string.Concat(FormatBaseData(pageOfDeliveryModel),FormatAditionData(additionData),FormatDocumentsData(documents));
        }

        private string FormatBaseData(Result<PageOfDeliveryJsonModel> pageOfDelivery)
        {
            if (pageOfDelivery.IsFailed)
            {
                return pageOfDelivery.Errors.First().Message + "\n";
            }
            else return FormatBaseData(pageOfDelivery.Value);
        }
        private string FormatAditionData(Result<AdditionData> additionalData)
        {
            if (additionalData.IsFailed)
            {
                return additionalData.Errors.First().Message+"\n";
            }
            else return FormatAditionData(additionalData.Value);
        }
        private string FormatDocumentsData(Result<List<DocumentJsonModel>> documents)
        {
            if (documents.IsFailed)
            {
                return documents.Errors.First().Message + "\n";
            }
            else return FormatDocumentsData(documents.Value);
        }
        private string FormatBaseData(PageOfDeliveryJsonModel pageOfDeliveryJson)
        {

            var pageOfDelivery = pageOfDeliveryJson.PageOfDelivery;
            var startMaxPrice = pageOfDelivery.IsInitialPriceDefined ? pageOfDelivery.InitialPrice.ToString() : "Начальная максимальная цена не назначена";
            return $"Номер тендера: {pageOfDelivery.Id}\n" +
                $"Наименование тендера: {pageOfDelivery.TradeName}\n" +
                $"Текущий статус: {pageOfDelivery.TradeState}\n" +
                $" Наименование заказчика: {pageOfDelivery.CustomerFullName}\n" +
                $" Начальная максимальная цена: {startMaxPrice}\n" +
                $" Дата публикации: {pageOfDelivery.PublicationDateUTC7}\n" +
                $" Дата ококнчания подачи заявок: {pageOfDelivery.FillingApplicationEndDateUTC7}\n";
        }
        private string FormatAditionData(AdditionData additionData)
        {
            StringBuilder result = new StringBuilder($"Место поставки: {additionData.PlaseOfDelivery}\n   Список лотов:\n");
            foreach (var lot in additionData.Lots)
            {

                result.AppendLine($"Наименование: {lot.Name}\n" +
                    $"Еденицы измерения: {lot.Unit}\n" +
                    $"Количество: {lot.Unit}\n" +
                    $"Стоймость еденицы: {lot.Price}\n");
            }
            return result.ToString();
        }
        private string FormatDocumentsData(List<DocumentJsonModel> documents)
        {
            StringBuilder result = new StringBuilder("    Список документов:\n");
            foreach (var document in documents)
            {
                result.AppendLine($"Название: {document.FileName}\n" +
                    $"URL: {document.Url}");
            }
            return result.ToString();

        }

    }

}

