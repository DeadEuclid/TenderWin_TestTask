using Net;
using System;
using System.Collections.Generic;
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
                    Console.WriteLine("Номер тендера введён не корректно повторите попытку");
                    continue;
                }

                var baseFields = scraper.GetBaseFields(tenderNumber);
                var additionData = scraper.GetAdditionData(tenderNumber);
                var documents = scraper.GetDocuments(tenderNumber);
                if (scraper.TenderExist)
                {
                    if (scraper.SelectorsOk)
                    {
                        if (scraper.NetIsWork)
                        {
                            Console.WriteLine(formater.FormatAllData(baseFields, additionData, documents));
                        }
                        else
                        {
                            Console.WriteLine("Возникли проблемы с сетью или сайтом.попробуйте позже");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Сайт изменился, попробуйте обратится к разработчику приложения");
                    }

                }
                else
                {
                    Console.WriteLine("Тендера с данным номером не существует, попробуйте ещё раз");
                }

            }

        }

    }
    class FormaterDataOfTenders
    {
        public string FormatAllData(PageOfDeliveryJsonModel pageOfDeliveryModel, AdditionData additionData, List<DocumentJsonModel> documents) =>
            FormatBaseData(pageOfDeliveryModel) + FormatAditionData(additionData) + FormatDocumentsData(documents);
        private string FormatBaseData(PageOfDeliveryJsonModel pageOfDeliveryModel)
        {

            var pageOfDelivery = pageOfDeliveryModel.PageOfDelivery;
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

