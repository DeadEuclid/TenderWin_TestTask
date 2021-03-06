using System;
using System.Collections.Generic;
using System.Linq;

namespace Net
{
    public class Invdata
    {


        public string TradeStateName { get; set; }
        public string TradeName { get; set; }
        public int Id { get; set; }
        public string CustomerFullName { get; set; }
        public bool IsInitialPriceDefined { get; set; }
        public double InitialPrice { get; set; }
        public DateTime FillingApplicationEndDate { get; set; }
        public DateTime FillingApplicationEndDateUTC7 => FillingApplicationEndDate.AddHours(7);
        public DateTime PublicationDate { get; set; }
        public DateTime PublicationDateUTC7 => PublicationDate.AddHours(7);

    }

    public class PageOfDeliveryJsonModel
    {
        public override string ToString()
        {
            var startMaxPrice = PageOfDelivery.IsInitialPriceDefined ? PageOfDelivery.InitialPrice.ToString() : "Начальная максимальная цена не назначена";
            return $"Номер тендера: {PageOfDelivery.Id}\n" +
                $"Наименование тендера: {PageOfDelivery.TradeName}\n" +
                $"Текущий статус: {PageOfDelivery.TradeStateName}\n" +
                $" Наименование заказчика: {PageOfDelivery.CustomerFullName}\n" +
                $" Начальная максимальная цена: {startMaxPrice}\n" +
                $" Дата публикации: {PageOfDelivery.PublicationDateUTC7}\n";

        }
        public Invdata PageOfDelivery => Invdata.First();
        public int Totalrecords { get; set; }
        public List<Invdata> Invdata { get; set; }
    }
    public class DocumentJsonModel
    {
        public override string ToString()
        {
            return $"Название: {FileName}\n" +
                    $"URL: {Url}";
        }
        public string FileName { get; set; }
        public string Url { get; set; }
    }


}
