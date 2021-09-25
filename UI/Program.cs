using Net;
using System;
using System.Linq;

namespace UI
{
    class Program
    {
        static void Main()
        {


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
        void Run()
        {
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
                    Console.WriteLine(formater.FormatAllResults(baseFieldsResult, additionDataResult, documentsResult));
                }
            }
        }
    }

}

