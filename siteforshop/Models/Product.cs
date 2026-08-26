namespace siteforshop.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public bool InStock { get; set; }
        public virtual string Category => "Товар";
        public virtual string GetDetails() => "";

        public bool IsValidBase(string title, decimal price)
        {
            return !string.IsNullOrWhiteSpace(title) && price > 0;
        }

        public virtual bool IsValid()
        {
            return IsValidBase(Title, Price);
        }
    }

    public class FoodProduct : Product
    {
        public override string Category => "Спортивне харчування";
        public int WeightGrams { get; set; }
        public string Flavor { get; set; } = string.Empty;

        public override string GetDetails() => $"Смак: {Flavor}, Вага: {WeightGrams} г";

        public override bool IsValid()
        {
            return base.IsValid()
                && WeightGrams > 0
                && !string.IsNullOrWhiteSpace(Flavor);
        }
    }

    public class ClothingProduct : Product
    {
        public override string Category => "Одяг";
        public string Size { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;

        public override string GetDetails() => $"Розмір: {Size}, Колір: {Color}";

        private  readonly HashSet<string> AllowedSizes = new(StringComparer.OrdinalIgnoreCase)
        {
            "XXS", "XS", "S", "M", "L", "XL", "XXL", "3XL",
            "42", "44", "46", "48", "50", "52", "54", "56"
        };

        public override bool IsValid()
        {
            return base.IsValid()
                && !string.IsNullOrWhiteSpace(Color)
                && !string.IsNullOrWhiteSpace(Size)
                && AllowedSizes.Contains(Size.Trim());
        }

    }
}