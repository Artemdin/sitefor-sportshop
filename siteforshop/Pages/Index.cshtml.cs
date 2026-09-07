using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using siteforshop.Models;
using System.Net.NetworkInformation;

namespace siteforshop.Pages
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public string Status { get; set; } = "В наявності";
        public bool IsAdmin { get; set; } = true;

        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public static List<Product> Products { get; set; } = new(){
    new FoodProduct { Id = 1, Title = "Протеїн Whey Gold", Price = 1250, Status = "В наявності", WeightGrams = 1000, Flavor = "Шоколад" },
    new ClothingProduct { Id = 2, Title = "Спортивний костюм Nike", Price = 2400, Status = "Очікується", Size = "M", Color = "Чорний" }
};

        [BindProperty(SupportsGet = true)]
        public string SearchQuery { get; set; }

        [BindProperty(SupportsGet = true)]
        public List<string> Category { get; set; } = new();

        [BindProperty] public string ProductType { get; set; } = "Food";
        [BindProperty] public string Title { get; set; } = string.Empty;
        [BindProperty] public decimal Price { get; set; }
        [BindProperty] public int WeightGrams { get; set; }
        [BindProperty] public string Flavor { get; set; } = string.Empty;
        [BindProperty] public string Size { get; set; } = string.Empty;
        [BindProperty] public string Color { get; set; } = string.Empty;

        // Властивості для швидкого редагування товару
        [BindProperty] public int EditId { get; set; }
        [BindProperty] public string EditTitle { get; set; } = string.Empty;
        [BindProperty] public decimal EditPrice { get; set; }
        [BindProperty] public string EditStatus { get; set; } = "В наявності";
        public void OnGet() { }

        // Додавання товару з поліморфною валідацією
        public IActionResult OnPostAdd()
        {
            Product newProduct = ProductType switch
            {
                "Food" => new FoodProduct
                {
                    Title = Title,
                    Price = Price,
                    Status = Status, 
                    WeightGrams = WeightGrams,
                    Flavor = Flavor
                },
                "Clothing" => new ClothingProduct
                {
                    Title = Title,
                    Price = Price,
                    Status = Status,
                    Size = Size,
                    Color = Color
                },
                _ => new Product
                {
                    Title = Title,
                    Price = Price,

                    Status = Status
                }
            
            };

            // Викликаємо поліморфний метод IsValid() залежно від типу товару
            if (!newProduct.IsValid())
            {
                return RedirectToPage();
            }

            newProduct.Id = Products.Any() ? Products.Max(p => p.Id) + 1 : 1;
            Products.Add(newProduct);

            return RedirectToPage();
        }

        // Видалення товару
        public IActionResult OnPostDelete(int id)
        {
            var item = Products.FirstOrDefault(p => p.Id == id);
            if (item != null)
            {
                Products.Remove(item);
            }
            return RedirectToPage();
        }

        // Сумісність зі старішою назвою методу
        public IActionResult OnPostClear(int id) => OnPostDelete(id);


        // Редагування товару (Edit)
        public IActionResult OnPostEdit()
        {
            if (!Product.IsValidBase(EditTitle, EditPrice))
            {
                return RedirectToPage();
            }

            var item = Products.FirstOrDefault(p => p.Id == EditId);
            if (item != null)
            {
                item.Title = EditTitle.Trim();
                item.Price = EditPrice;
                item.Status = EditStatus;
            }
            return RedirectToPage();
        }
    
    }
}

