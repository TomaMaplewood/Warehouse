namespace Warehouse.Model
{
    public abstract class WarehouseObject
    {
        private int volume;
        private Guid id;
        private int width;
        private int height;
        private int depth;
        private int weight;

        public Guid Id  {  get => id;  set => id = value; }
        public int Width { get => width; set => width = value; }
        public int Height { get => height; set => height = value; }
        public int Depth { get => depth; set => depth = value; }
        public int Weight { get => weight; set => weight = value; }
        public virtual int Volume =>Width * Height * Depth; 

        public WarehouseObject(int width, int height, int depth)
        {
            Width = width; Height = height; Depth = depth;
        }

        public WarehouseObject() { }

        public override string ToString()
        {
            return $"Размеры: {Height} x {Width} x {Depth} \t|\tОбъем: {Volume}, Вес: {Weight}\n";
        }
    }
}
