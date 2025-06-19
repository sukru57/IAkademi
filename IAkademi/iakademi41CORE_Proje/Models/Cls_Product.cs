using iakademi41CORE_Proje.Models.MVVM;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using X.PagedList;
using XAct;

namespace iakademi41CORE_Proje.Models
{
    public class Cls_Product
    {

        public int page { get; set; }
        public int mainpageCount { get; set; }
        public int subpageCount { get; set; }
        public int ProductID { get; set; }
        public string? ProductName { get; set; }
        public decimal UnitPrice { get; set; }
        public string? PhotoPath { get; set; }
        public string? Notes { get; set; }


        iakademi41Context context = new iakademi41Context();

        //metod overload (aynı isimli metodu,farklı parametre sırası ile yazmak)
        //Home/Index
        public List<Product> ProductSelect(string mainPageName, int pagenumber)
        {
            //pagenumber = -1   Home/Index
            //pagenumber = 0    Alt sayfa ilk tıklanınca
            //pagenumber > 0   Ajax sayfalama
            List<Product> products;
            if (mainPageName == "Slider")
            {
                //select * from Products where StatusID = 1   (ado.net)
                 products = context.Products.Where(p => p.StatusID == 1 && p.Active == true).OrderBy(p => p.ProductName).Take(mainpageCount).ToList(); //EFCore
            }
            else if (mainPageName == "New")
            {
                if (pagenumber == -1)
                {
                    //Home/Index
                    products = context.Products.Where(p => p.Active == true).OrderByDescending(p => p.AddDate).Take(mainpageCount).ToList();
                }
                else if (pagenumber == 0)
                {
                    //alt sayfa ilk tıklama
                    products = context.Products.Where(p => p.Active == true).OrderByDescending(p => p.AddDate).Take(subpageCount).ToList();
                }
                else
                {
                    //ajax sayfalama
                    products = context.Products.Where(p => p.Active == true).OrderByDescending(p => p.AddDate).Skip(pagenumber * subpageCount).Take(subpageCount).ToList();
                }
            }
            else if (mainPageName == "Special")
            {
                if (pagenumber == -1)
                {
                    //Home/Index
                    products = context.Products.Where(p => p.StatusID == 2 && p.Active == true).Take(mainpageCount).ToList(); //EFCore
                }
                else if (pagenumber == 0)
                {
                    //menuden özel urun sayfası butonu tıklanınca ilk calısacak
                    products = context.Products.Where(p => p.StatusID == 2 && p.Active == true).Take(subpageCount).ToList(); //EFCore
                }
                else
                {
                    //ajax
                    products = context.Products.Where(p => p.StatusID == 2 && p.Active == true).Skip(pagenumber * subpageCount).Take(subpageCount).ToList();
                }
            }
            else if (mainPageName == "Discounted")
            {
                if (pagenumber == -1)
                {
                    //Home/Index
                    products = context.Products.Where(p => p.Active == true).OrderByDescending(p => p.Discount).Take(mainpageCount).ToList();
                }
                else if (pagenumber == 0)
                {
                    //menuden indirimli urun sayfası butonu tıklanınca ilk calısacak
                    products = context.Products.Where(p => p.Active == true).OrderByDescending(p => p.Discount).Take(subpageCount).ToList();
                }
                else
                {
                    //ajax
                    products = context.Products.Where(p => p.Active == true).OrderByDescending(p => p.Discount).Skip(pagenumber * subpageCount).Take(subpageCount).ToList();
                }
            }
            else if (mainPageName == "HighLighted")
            {
                if (pagenumber == -1)
                {
                    products = context.Products.Where(p => p.Active == true).OrderByDescending(p => p.HighLighted).Take(mainpageCount).ToList();
                }
                else if (pagenumber == 0)
                {
                    products = context.Products.Where(p => p.Active == true).OrderByDescending(p => p.HighLighted).Take(subpageCount).ToList();
                }
                else
                {
                    //ajax
                    products = context.Products.Where(p => p.Active == true).OrderByDescending(p => p.HighLighted).Skip(pagenumber * subpageCount).Take(subpageCount).ToList();
                }
            }
            else if (mainPageName == "TopSeller")
            {
                products = context.Products.Where(p => p.Active == true).OrderByDescending(p => p.TopSeller).Take(mainpageCount).ToList();
            }
            else if (mainPageName == "Star")
            {
                products = context.Products.Where(p => p.StatusID == 3 && p.Active == true).Take(mainpageCount).ToList(); 
            }
            else if (mainPageName == "Opportunity")
            {
                products = context.Products.Where(p => p.StatusID == 4 && p.Active == true).Take(mainpageCount).ToList();
            }
            else if (mainPageName == "Notable")
            {
                products = context.Products.Where(p => p.StatusID == 5 && p.Active == true).Take(mainpageCount).ToList();
            }
            else
            {
                products = context.Products.Where(p => p.Active == true).Take(mainpageCount).ToList(); //EFCore
            }
            return products;
        }

        //metod overload (ProductSelect daha önce farklı parametre sırası ile yazıldı)
        public List<Product> ProductSelect(int id,string TableName)
        {
            List<Product> products;
            if (TableName == "Category")
            {
                products = context.Products.Where(p => p.CategoryID == id).OrderBy(p => p.ProductName).ToList();
            }
            else if (TableName == "Supplier")
            {
                products = context.Products.Where(p => p.SupplierID == id).ToList();
            }
            else
            {
                products = context.Products.ToList();
            }
            return products;
        }

        //AdminController
        public async Task<List<Product>> ProductSelect()
        {
            List<Product> products = await context.Products.OrderBy(p => p.StatusID).ToListAsync();
            //List<Product> products = await context.Products.ToListAsync();
            // Product tekurun = await context.Products.FirstOrDefaultAsync();
            return products;
        }





        public static int NewProductCount()
        {
            using (iakademi41Context context = new iakademi41Context())
            {
                int count = context.Products.Count(p => p.AddDate == DateTime.Now);
                return count;
            }
        }

        //context.Products.Where(p => p.SupplierID == id).Count();
        public static int ProductCount(int? id)
        {
            using iakademi41Context context = new iakademi41Context();
            int count =   context.Products.Where(p => p.SupplierID == id).Count();
            return count;
        }


      

        public static bool ProductInsert(Product product)
        {
            try
            {
                using (iakademi41Context context = new iakademi41Context())
                {
                    product.AddDate = DateTime.Now;
                    context.Add(product);
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<Product> ProductDetails(int? id)
        {
            Product? product;
            if (id == 0)  //Home/Index = günün ürünü icin id = 0 parametresi gönderdim
            {
                product = await context.Products.Where(p => p.StatusID == 6).FirstOrDefaultAsync();
            }
            else
            {
                //Admin tarafta güncelle,silme,detay
                //Home ürünün detay bilgilerini getir
                product = await context.Products.FindAsync(id); 
            }
            return product;
        }

        public static bool ProductUpdate(Product product)
        {
            try
            {
                using (iakademi41Context context = new iakademi41Context())
                {
                    context.Update(product);
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }


        public static bool ProductDelete(int id)
        {
            try
            {
                using (iakademi41Context context = new iakademi41Context())
                {
                    Product? product = context.Products.FirstOrDefault(c => c.ProductID == id);
                    product.Active = false;
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        //alt sayfa cok satan ürünler . arabam.com sayfalama icin
        public PagedList<Product> TopsellerProductsList()
        {
            PagedList<Product> model = new PagedList<Product>(context.Products.OrderByDescending(p => p.TopSeller), page, subpageCount);

            return model;
        }



        //ürünün detayına veya sepete ekle butonuna tıklanınca, Highlighted(öne cıkanlar) kolon değerini bir arttırıyoruz.. like lamak
        public static void Highlighted_Increase(int id)
        {
            using (iakademi41Context context = new iakademi41Context())
            {
                //eski kayıtlarını buluyorum
                Product? product = context.Products.FirstOrDefault(p => p.ProductID == id);

                //eski kaydın HighLighted kolon değerini 1 arttırıyorum
                product.HighLighted += 1;
                //veritabanına tekrar geri yazıyorum,sadece HighLighted kolon değeri 1 arttı
                context.Update(product);
                context.SaveChanges();
            }
        }


        //detaylı arama
        public List<Cls_Product> SelectProductsByDetails(string query)
        {
            List<Cls_Product> products = new List<Cls_Product>();
            SqlConnection sqlConnection = Connection.ServerConnect;
            SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
            sqlConnection.Open();
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

            while (sqlDataReader.Read())
            {
                Cls_Product product = new Cls_Product();
                product.ProductID = Convert.ToInt32(sqlDataReader["ProductID"]);
                product.ProductName = sqlDataReader["ProductName"].ToString();
                product.UnitPrice = Convert.ToDecimal(sqlDataReader["UnitPrice"]);
                product.PhotoPath = sqlDataReader["PhotoPath"].ToString();
                product.Notes = sqlDataReader["Notes"].ToString();
                products.Add(product);
            }
            return products;
        }


    }
}
