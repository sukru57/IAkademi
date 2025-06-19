using iakademi41CORE_Proje.Models.MVVM;

namespace iakademi41CORE_Proje.Models
{
    public class Cls_Sorgular
    {
        iakademi41Context context = new iakademi41Context();

        public void Sorgular()
        {
            //bir kaydın bütün kolonlarının bilgisi
            //tek bir ÜRÜN BÜTÜN KOLONLARI
            //ado.net = select * from Products where ProductID = 10
            //entityframeworkcore 
            Product? product = context.Products.FirstOrDefault(p => p.ProductID == 10);

            //ürünLER
            //ado.net = select * from Products
            List<Product> products = context.Products.ToList();
            //ado.net = select* from Products where UnitPrice > 1000 and UnitPrice< 20000
            List<Product> products2 = context.Products.Where(p => p.UnitPrice > 1000 && p.UnitPrice < 20000).ToList();

            //tek bir ÜRÜN SADECE BİR  KOLONU
            //ado.net = select CategoryName from Categories where CategoryID = 5
            //entityframeworkcore 
            string categotyname = context.Categories.FirstOrDefault(c => c.CategoryID == 5).CategoryName;

            decimal fiyat = context.Products.FirstOrDefault(p => p.ProductID == 10).UnitPrice;
        }

    }
}
