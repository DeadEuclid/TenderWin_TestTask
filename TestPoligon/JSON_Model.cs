using System;
using System.Collections.Generic;
using System.Text;

namespace TestPoligon
{
    public class Invdata
    {
        public int TradeState { get; set; }
        public string TradeName { get; set; }
        public int Id { get; set; }
        public double InitialPrice { get; set; }
        public DateTime FillingApplicationEndDate { get; set; }
        public DateTime PublicationDate { get; set; }

    }
    public class DocumentsJsonModel
    {
        public List<DocumentJsonModel> invdata { get; set; }
    }
    public class PageOfDeliveryJsonModel
    {
        public List<Invdata> invdata { get; set; }
    }
    public class DocumentJsonModel
    {
        public string FileName { get; set; }
        public string Url { get; set; }
    }


}
