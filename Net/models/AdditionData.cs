using System.Collections.Generic;

namespace Net
{
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
}
