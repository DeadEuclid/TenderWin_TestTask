using Net;
using System;
using System.Linq;

namespace UI
{
    class Program
    {
        static void Main()
        {

            new App().Run();
        }

    }
    class App
    {
        public App()
        {
            formater = new FormaterDataOfTenders();
            scraper = new DataScraper();
        }
        private readonly FormaterDataOfTenders formater;
        private readonly DataScraper scraper;
        public void Run()
        {
            while (true)
            {

                try
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
                        Console.WriteLine(formater.FormatAllResults(baseFieldsResult, additionDataResult, documentsResult));
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("Неизвестная ошибка");
                }

            }
        }
    }

}

