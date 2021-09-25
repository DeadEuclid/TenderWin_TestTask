using System.Collections.Generic;
using System.Text;

namespace Net
{
    public class AdditionData
    {
        public AdditionData()
        {
        }
        public override string ToString()
        {
            StringBuilder result = new StringBuilder($"Место поставки: {PlaseOfDelivery}\n   Список лотов:\n");
            foreach (var lot in Lots)
            {

                result.AppendLine($"Наименование: {lot.Name}\n" +
                    $"Еденицы измерения: {lot.Unit}\n" +
                    $"Количество: {lot.Unit}\n" +
                    $"Стоймость еденицы: {lot.Price}\n");
            }
            return result.ToString();
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
