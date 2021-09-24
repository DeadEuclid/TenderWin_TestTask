using Net;
using System;
using System.Collections.Generic;
using System.Text;

namespace UI
{
    class Program
    {
        static void Main(string[] args)
        {
            var formater = new FormaterDataOfTenders();
            var scraper = new DataScraper();

            int tenderNumber;
            while (true)
            { 
                Console.WriteLine("Введите номер тендера:");
                if (!int.TryParse(Console.ReadLine(),out tenderNumber))
                {
                    Console.WriteLine("Номер тендера введён не корректно повторите попытку");
                    continue;
                } 
                try
                {
                    Console.WriteLine(formater.FormatAllData(
                        scraper.GetBaseFields(tenderNumber),
                        scraper.GetAdditionData(tenderNumber),
                        scraper.GetDocuments(tenderNumber)));
                }
                catch (ArgumentException notExistTender)
                {
                    Console.WriteLine(notExistTender.Message+"\n");
                }
            }
            Console.ReadLine();
        }
    }
    class FormaterDataOfTenders
    {
        public string FormatAllData(PageOfDeliveryJsonModel pageOfDeliveryModel, AdditionData additionData, List<DocumentJsonModel> documents) =>
            FormatBaseData(pageOfDeliveryModel) + FormatAditionData(additionData) + FormatDocumentsData(documents);
        public string FormatBaseData(PageOfDeliveryJsonModel pageOfDeliveryModel)
        {

            var pageOfDelivery = pageOfDeliveryModel.PageOfDelivery;
            string startMaxPrice = pageOfDelivery.IsInitialPriceDefined ? pageOfDelivery.InitialPrice.ToString() : "Начальная максимальная цена не назначена";
            return $"Номер тендера: {pageOfDelivery.Id}\n" +
                $"Наименование тендера: {pageOfDelivery.TradeName}\n" +
                $"Текущий статус: {pageOfDelivery.TradeState}\n" +
                $" Наименование заказчика: {pageOfDelivery.CustomerFullName}\n" +
                $" Начальная максимальная цена: {startMaxPrice}\n" +
                $" Дата публикации: {pageOfDelivery.PublicationDateUTC7}\n" +
                $" Дата ококнчания подачи заявок: {pageOfDelivery.FillingApplicationEndDateUTC7}\n";
        }
        public string FormatAditionData(AdditionData additionData)
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
        public string FormatDocumentsData(List<DocumentJsonModel> documents)
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

