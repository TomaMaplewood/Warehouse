using Org.BouncyCastle.Bcpg;
using Org.BouncyCastle.Utilities;
using System;

namespace Warehouse.Model
{
    public class Pallet : WarehouseObject
    {
        private const int PALLET_WEIGHT = 30;
        public List<Box> Boxes { get; set; }
        public DateOnly ExpirationDate { get; private set; }

        public Pallet(int width, int height, int depth) : base(width, height, depth)
        {
            Id = Guid.NewGuid();
            Width = width;
            Height = height;
            Depth = depth;
            Boxes = new List<Box>();
        }

        public Pallet()
        {
            Boxes = new List<Box>();
            ExpirationDate = DateOnly.FromDateTime(DateTime.Today);
            Weight = PALLET_WEIGHT;
        }
        public override int Volume
        {
            get
            {
                int volume = Width * Height * Depth;
                foreach (Box box in Boxes)
                {
                    volume += box.Volume;
                }
                return volume;
            }
        }
        
        public override string ToString()
        {
            return $"\nНомер паллета: {Id}\n" + base.ToString() + $"\tСрок годности паллета: {ExpirationDate} " + "\n";
        }

        public void AddBox(Box box)
        {
            if (box.Width > Width || box.Depth > Depth)
            {
                throw new Exception("\nРазмеры коробки больше размера паллета.\n");
            }

            Boxes.Add(box);

            Weight += box.Weight;

            if (box.ExpiryDate < ExpirationDate)
            {
                ExpirationDate = box.ExpiryDate;
            }
        }
    }
}
