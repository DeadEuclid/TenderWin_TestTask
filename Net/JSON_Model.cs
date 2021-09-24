using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Net
{
    public class Invdata
    {


        public int TradeState { get; set; }
        public string TradeName { get; set; }
        public int Id { get; set; }
        public string CustomerFullName { get; set; }
        public bool IsInitialPriceDefined { get; set; }
        public double InitialPrice { get; set; }
        public DateTime FillingApplicationEndDate { get; set; }
        public DateTime FillingApplicationEndDateUTC7 { get=>FillingApplicationEndDate.AddHours(7);  }
        public DateTime PublicationDate { get; set; }
        public DateTime PublicationDateUTC7 { get => PublicationDate.AddHours(7); }

    }

    public class PageOfDeliveryJsonModel
    {
        public Invdata PageOfDelivery => invdata.First();
        public int totalrecords { get; set; }
        public List<Invdata> invdata { get; set; }
    }
    public class DocumentJsonModel
    {
        public string FileName { get; set; }
        public string Url { get; set; }
    }


}
