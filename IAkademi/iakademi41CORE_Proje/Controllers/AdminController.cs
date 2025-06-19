using iakademi41CORE_Proje.Models.MVVM;
using Microsoft.AspNetCore.Mvc;
using iakademi41CORE_Proje.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using XAct;

namespace iakademi41CORE_Proje.Controllers
{
    public class AdminController : Controller
    {
        Cls_User cls_User = new Cls_User(); //new ile nesne olusturduk , instance aldık
        Cls_Category cls_Category = new Cls_Category(); //new ile nesne olusturduk , instance aldık
        Cls_Supplier cls_Supplier = new Cls_Supplier();
        Cls_Status cls_Status = new Cls_Status();
        Cls_Product cls_Product = new Cls_Product();
        Cls_setting cls_setting = new Cls_setting();
        iakademi41Context context = new iakademi41Context();

        [HttpGet]
        public IActionResult Login()
        {
            //sayfanın tasarımını oluşturacak
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([Bind("Email,Password,NameSurname")] User user)
        {
            //sayfada formdan girilen veriler buraya gelecek

            //[Bind("Email,Password")] , .cshtml sayfasından sadece bu bilgiler kabul edilecek

            //ben bütün metodlarda zaman kaybetmemek icin
            //Bind,ModelState.IsValid
            //yazıp durmayacagım , ögrenciler bu kısımları tamamlayacak

            if (ModelState.IsValid)
            {
                //.cshtml sayfasındaki bütün zorunlu alanlar ok ise buraya girecek
                //User clasındaki  [Required] , EmailAddress kontrolü 
                User? usr =await  cls_User.loginControl(user); 
                if (usr != null)
                {
                    return RedirectToAction("Index"); //Admin/Index sayfasına gider
                }
                else
                {
                    ViewBag.error = "Login ve/veya şifre yanlış";
                }
            }
            return View(); //login sayfasında kalmaya devam edecek.login yada şifre hatalı,kullanıcı tekrar deneyecek
        }



        [HttpGet]
        public IActionResult Index()
        {
            //sayfanın tasarımını oluşturacak
            ViewBag.count = Cls_Product.NewProductCount();
            return View();
        }

        [HttpGet]
        public IActionResult CategoryIndex()
        {
            List<Category> categories = cls_Category.CategorySelect("all");
            return View(categories);
        }

        [HttpGet]
        public IActionResult CategoryCreate()
        {

            // bütün yazılım dillerinde ortak = Casting = tip dönüştürme = Convert.ToInt32

            // int yas = Convert.ToInt32(txt_yas);
            // int yas = (int)txt_yas;  = (IEnumerable<SelectListItem>)ViewData["categoryList"]
            // int yas = txt_yas as int; = ViewData["categoryList"] as (IEnumerable<SelectListItem>)

            CategoryFill("main");
            return View();
        }

        void CategoryFill(string main_or_all)
        {
            List<Category> categories = cls_Category.CategorySelect(main_or_all);
            ViewData["categoryList"] = categories.Select(c => new SelectListItem { Text = c.CategoryName, Value = c.CategoryID.ToString() });
        }


        [HttpPost]
        public IActionResult CategoryCreate(Category category)
        {
            if (ModelState.IsValid)
            {
                bool answer = Cls_Category.CategoryInsert(category);
                if (answer == true)
                {
                    TempData["Message"] = "Eklendi";
                }
                else
                {
                    TempData["Message"] = "HATA";
                }
            }
            //return RedirectToAction(nameof(CategoryCreate));
            return RedirectToAction("CategoryCreate"); // [HttpGet]
        }


       

        [HttpGet]
        public async Task<IActionResult> CategoryEdit(int? id)
        {
            CategoryFill("main");
            if (id == null || context.Categories == null)
            {
                return NotFound();
            }

            var category = await cls_Category.CategoryDetails(id);

            return View(category); //CategoryEdit.cshtml sayfasında modele gönderiyor
        }



        [HttpPost]
        public IActionResult CategoryEdit(Category category)
        {
            bool answer = Cls_Category.CategoryUpdate(category);
            if (answer == true)
            {
                //türkce karakter sorunu icin,Program.cs icinde,
                //builder.Services.AddWebEncoders();   eklendi
                TempData["Message"] = category.CategoryName + " Kategorisi Güncellendi";
                return RedirectToAction("CategoryIndex");
            }
            else
            {
                TempData["Message"] = "HATA";
                //[HttpPost] metodundan , CategoryEdit.cshtml sayfasına giderim
                //return View(); 

                //[HttpGet] metodundan , CategoryEdit.cshtml sayfasına giderim
                return RedirectToAction(nameof(CategoryEdit)); 
            }
        }


        [HttpGet]
        public async Task<IActionResult> CategoryDelete(int? id)
        {
            if (id == null || context.Categories == null)
            {
                return NotFound();
            }

            var category = await cls_Category.CategoryDetails(id);

            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost, ActionName("CategoryDelete")] //Routing
        public IActionResult CategoryDeleteConfirmed(int id)
        {
            bool answer = Cls_Category.CategoryDelete(id);
            if (answer == true)
            {
                TempData["Message"] = "Silindi";
                return RedirectToAction("CategoryIndex");
            }
            else
            {
                TempData["Message"] = "HATA";
                return RedirectToAction(nameof(CategoryDelete)); //[HttpGet]
            }
        }

        public async Task<IActionResult> CategoryDetails(int? id)
        {
            //using Microsoft.EntityFrameworkCore;     ctrl + .
            var category = await cls_Category.CategoryDetails(id);
            ViewBag.CategoryName = category?.CategoryName ; //menüde tekrar detay tıklanırken problem yaşanmaması için

            return View(category);
        }


        [HttpGet]
        public IActionResult SupplierIndex()
        {
            //jsp (jsf) = cshtml
            //tip = List<Supplier>
            //eclipse(netbeans)

            //degisken adı = suppliers
            //= sag tarafı , sorgu
            List<Supplier> suppliers = cls_Supplier.SupplierSelect();
            return View(suppliers);
        }


        [HttpGet]
        public IActionResult SupplierCreate()
        {
            return View();
        }


        [HttpPost]
        public IActionResult SupplierCreate(Supplier supplier)
        {
            if (ModelState.IsValid == true)
            {
                bool answer = Cls_Supplier.SupplierInsert(supplier);
                if (answer == true)
                {
                    TempData["Message"] = supplier.BrandName.ToUpper() + " Markası Eklendi";
                }
                else
                {
                    TempData["Message"] = "HATA";
                }
            }
            //return RedirectToAction(nameof(SupplierCreate));
            return RedirectToAction("SupplierCreate"); // [HttpGet]
        }



        [HttpGet]
        public async Task<IActionResult> SupplierEdit(int? id)
        {
            if (id == null || context.Suppliers == null)
            {
                return NotFound();
            }

            var supplier = await cls_Supplier.SupplierDetails(id);

            return View(supplier);
        }

        [HttpPost]
        public IActionResult SupplierEdit(Supplier supplier)
        {
            if (supplier.PhotoPath == null)
            {
                //eski resim kaydını muhafaza et
                supplier.PhotoPath = context.Suppliers.FirstOrDefault(s => s.SupplierID == supplier.SupplierID).PhotoPath.ToString();
            }

            bool answer = Cls_Supplier.SupplierUpdate(supplier);//static metod cagırıyoruz
            if (answer == true)
            {
                TempData["Message"] = "Güncellendi";
                return RedirectToAction(nameof(SupplierIndex));
            }
            else
            {
                TempData["Message"] = "HATA";
                return RedirectToAction(nameof(SupplierEdit)); //[HttpGet]
               // return View(); hangi metodta isek , o metodtan .cshtml sayfasına gideriz
               //suan  [HttpPost] ta oldugum icin asagıdaki 2 satır cshtml sayfası icin gerekli
               //onun icin RedirectToAction ile [HttpGet] gidiyorum,oradan 
               // var supplier = await cls_Supplier.SupplierDetails(id);
              //return View(supplier);
            }
        }



        public async Task<IActionResult> SupplierDetails(int? id)
        {
            var supplier = await cls_Supplier.SupplierDetails(id);
            ViewBag.ProductCount = Cls_Product.ProductCount(id);

            return View(supplier); //model e gönderiyorum
        }




        [HttpGet]
        public async Task<IActionResult> SupplierDelete(int? id)
        {
            if (id == null || context.Suppliers == null)
            {
                return NotFound();
            }

            var supplier = await cls_Supplier.SupplierDetails(id);

            if (supplier == null)
            {
                return NotFound();
            }
            return View(supplier); //cshtml sayfasında model e gönderdik
        }


        [HttpPost, ActionName("SupplierDelete")] //Routing
        public IActionResult SupplierDeleteConfirmed(int id)
        {
            /* sonsuz döngü
            for (int i = 0; i < 10; i++)
            {
                i = 0;
            }
            */

            bool answer = cls_Supplier.SupplierDelete(id);
            if (answer == true)
            {
                ViewBag.Status = true;
                TempData["Message"] = "Silindi";
                return RedirectToAction("SupplierIndex", new { status = "1" });
            }
            else
            {
                TempData["Message"] = "HATA";
                return RedirectToAction(nameof(SupplierDelete));
            }
        }

        public IActionResult StatusIndex()
        {
            List<Status> statuses = cls_Status.StatusSelect();
            return View(statuses); //.cshtml sayfasında model e verecek 
        }


        [HttpGet]
        public IActionResult StatusCreate()
        {
            return View();
        }


        [HttpPost]
        public IActionResult StatusCreate(Status status)
        {
            bool answer = Cls_Status.StatusInsert(status);
            if (answer == true)
            {
                TempData["Message"] = "Eklendi";
            }
            else
            {
                TempData["Message"] = "HATA";
            }
            return RedirectToAction(nameof(StatusCreate));
        }

        [HttpGet]
        public async Task<IActionResult> StatusEdit(int? id)
        {
            if (id == null || context.Statuses == null)
            {
                return NotFound();
            }
            var statuses = await cls_Status.StatusDetails(id);
            return View(statuses);
        }


        [HttpPost]
        public IActionResult StatusEdit(Status status)
        {
            bool answer = Cls_Status.StatusUpdate(status);
            if (answer == true)
            {
                TempData["Message"] = "Güncellendi";
                return RedirectToAction(nameof(StatusIndex));
            }
            else
            {
                TempData["Message"] = "HATA";
                return RedirectToAction(nameof(StatusEdit));
            }
        }



        public async Task<IActionResult> StatusDetails(int? id)
        {
            var status = await cls_Status.StatusDetails(id);
            ViewBag.statusname = status?.StatusName;

            return View(status);
        }


        [HttpGet]
        public async Task<IActionResult> StatusDelete(int id)
        {
            bool answer = Cls_Status.StatusDelete(id);
            if (answer == true)
            {
                TempData["Message"] = "Silindi";
                return RedirectToAction(nameof(StatusIndex));
            }
            else
            {
                TempData["Message"] = "HATA";
                return RedirectToAction(nameof(StatusIndex));
            }
        }


        //ctrl + M O = metodları kapatır
        //ctrl + M L = metodları acar

        public async Task<IActionResult> ProductIndex()
        {
            List<Product> products = await cls_Product.ProductSelect();
            return View(products);
        }


        [HttpGet]
        public async Task<IActionResult> ProductCreate()
        {
            CategoryFill("all");
            SupplierFill();
            StatusFill();
            //combobox = DropDownList
            return View();
        }

        void SupplierFill()
        {
            List<Supplier> suppliers = cls_Supplier.SupplierSelect();
            ViewData["supplierList"] = suppliers.Select(s => new SelectListItem { Text = s.BrandName, Value = s.SupplierID.ToString() });
        }

        void StatusFill()
        {
            List<Status> statuses = cls_Status.StatusSelect();
            ViewData["StatusList"] = statuses.Select(s => new SelectListItem { Text = s.StatusName, Value = s.StatusID.ToString() });
        }


        [HttpPost]
        public IActionResult ProductCreate(Product product)
        {
            if (ModelState.IsValid)
            {
                bool answer = Cls_Product.ProductInsert(product);
                if (answer)
                {
                    /*
                     resimler klasörünün dinamik yolunu bul..
                     sedat = E:\YEDEK\iakademi41_cumartesi_pazar\iakademi41CORE_Proje\iakademi41CORE_Proje\wwwroot
                    mert = C:\Projelerim\iakademi41CORE_Proje\iakademi41CORE_Proje\wwwroot
                    sukru = D:\İakademiProjelerim\iakademi41CORE_Proje\iakademi41CORE_Proje\wwwroot
                    string yol =  HttpContext.Server.MapPath("~/resimler");

                    Path.Combine(yol, product.Photopath);
                     */
                    /*
                 // Specify the directory where you want to retrieve the files
                   string uploadDir = HttpContext.Server.MapPath("~/resimler");
                   string fileName = $"{fileModel.Id}_{fileModel.Name}.png";
                   string filePath = Path.Combine(uploadDir, fileName);
                */
                    TempData["Message"] = "Eklendi";
                }
                else
                {
                    TempData["Message"] = "HATA";
                }
            }
            return RedirectToAction(nameof(ProductCreate)); // [HttpGet]
        }

        public async Task<IActionResult> ProductEdit(int? id)
        {
            CategoryFill("all");
            SupplierFill();
            StatusFill();

            if (id == null || context.Products == null)
            {
                return NotFound();
            }

            var product = await cls_Product.ProductDetails(id);

            return View(product);
        }



        [HttpPost]
        public IActionResult ProductEdit(Product product)
        {
            //veritabanından kaydını getirdim
            Product prd = context.Products.FirstOrDefault(s => s.ProductID == product.ProductID);
            //formdan gelmeyen , bazı kolonları null yerine , eski bilgilerini bastım
            product.AddDate = prd.AddDate;
            product.HighLighted = prd.HighLighted;
            product.TopSeller = prd.TopSeller;

            if (product.PhotoPath == null)
            {
                string? PhotoPath = context.Products.FirstOrDefault(s => s.ProductID == product.ProductID).PhotoPath;
                product.PhotoPath = PhotoPath;
            }

            bool answer = Cls_Product.ProductUpdate(product);
            if (answer == true)
            {
                TempData["Message"] = "Güncellendi";
                return RedirectToAction("ProductIndex");
            }
            else
            {
                TempData["Message"] = "HATA";
                return RedirectToAction(nameof(ProductEdit));
            }
        }


        public async Task<IActionResult> ProductDetails(int? id)
        {
            var product = await cls_Product.ProductDetails(id);

            return View(product);
        }

        [HttpGet]
        public async Task<IActionResult> ProductDelete(int? id)
        {
            if (id == null || context.Products == null)
            {
                return NotFound();
            }

            var product = await cls_Product.ProductDetails(id);

            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }


        [HttpPost, ActionName("ProductDelete")] //routing
        public async Task<IActionResult> ProductDeleteConfirmed(int id)
        {
            bool answer = Cls_Product.ProductDelete(id);
            if (answer == true)
            {
                TempData["Message"] = "Silindi";
                return RedirectToAction("ProductIndex");
            }
            else
            {
                TempData["Message"] = "HATA";
                return RedirectToAction(nameof(ProductDelete));
            }
        }

        [HttpGet]
        public async Task<IActionResult> SettingEdit()
        {
            var setting = await cls_setting.SettingDetails();

            return View(setting);
        }


        [HttpPost]
        public IActionResult SettingEdit(Setting setting)
        {

            bool answer = Cls_setting.SettingUpdate(setting);
            if (answer == true)
            {
                TempData["Message"] = "Güncellendi";
            }
            else
            {
                TempData["Message"] = "HATA";
            }
            return RedirectToAction(nameof(SettingEdit));
        }





    }
}
