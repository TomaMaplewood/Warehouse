using System;
using System.Reflection.PortableExecutable;

namespace Warehouse.Model
{
    public class Box : WarehouseObject
    {
        private const int EXPIRY_DAYS = 100;
        private DateOnly maxValue = new DateOnly(2100, 12, 12);

        public DateOnly ProductionDate { get; set; }
        public DateOnly ExpiryDate { get; set; }

        public Box(int width, int height, int depth, int weight, 
            DateOnly date, bool check) : base(width,height,depth)
        {
            if (((width | height) | (depth | weight)) <=0)
            {
                throw new ArgumentException("Неверные данные.");
            }

            Id = Guid.NewGuid();
            Weight = weight;
           
            if (check)
            {
                ExpiryDate = date.AddDays(EXPIRY_DAYS);
                ProductionDate = date;
            }
            else
            {
                ProductionDate = date.AddDays(-EXPIRY_DAYS);
                ExpiryDate = date;
            }

            if (date > DateOnly.FromDateTime(DateTime.Today.AddDays(1)))
            {
                throw new ArgumentException("Неверные данные.");
            }
        }

        public override string ToString()
        {
            return $"\n\tBOX\t" +
                $"Номер коробки: {Id} " + base.ToString() + $"\t| Срок годности: {ExpiryDate}, Дата производства:{ProductionDate}";
        }
    }
}