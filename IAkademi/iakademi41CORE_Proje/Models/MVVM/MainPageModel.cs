namespace iakademi41CORE_Proje.Models.MVVM
{
    public class MainPageModel
    {
        public List<Product>? SliderProducts { get; set; } //slider    prop + tab + tab
        public List<Product>? NewProducts { get; set; } //yeni
        public List<Product>? SpecialProducts { get; set; } //özel
        public List<Product>? DiscountedProducts { get; set; }//indirimli
        public List<Product>? HighLightedProducts { get; set; }//öne cıkanlar
        public List<Product>? TopSellerProducts { get; set; } //cok satanlar
        public List<Product>? StarProducts { get; set; } //yıldızlı ürünler
        public List<Product>? OpportunityProducts { get; set; } //fırsat ürünler
        public List<Product>? NotableProducts { get; set; } //dikkat ceken
        public Product? Productofday { get; set; } //günün ürünü

    }
}
