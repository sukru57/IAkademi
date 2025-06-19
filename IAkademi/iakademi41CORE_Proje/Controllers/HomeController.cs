using Microsoft.AspNetCore.Mvc;
using iakademi41CORE_Proje.Models.MVVM;
using iakademi41CORE_Proje.Models;
using XAct;
using X.PagedList;

namespace iakademi41CORE_Proje.Controllers
{
    public class HomeController : Controller
    {
        //Hocan Proje istenmeyecek.Herkes kendi yazacak.
        // Sebep = öğrenmenin en iyi yolu

        //Tools - Nuget - Package
        //PM> Add-Migration "Createİakademi41"

        /*
            Product product = new Product();
            product.Kdv = -18; //set 

           int kdv =  product.Kdv; //get
            */

        //header
        //dinamik kısmı  body
        //footer

        MainPageModel mpm = new MainPageModel(); //modele birden fazla sorgu göndermek icin
        Cls_Product cls_Product = new Cls_Product();
        Cls_User cls_User = new Cls_User();
        Cls_Category cls_Category = new Cls_Category();
        Cls_Supplier cls_Supplier = new Cls_Supplier();
        Cls_Order cls_Order = new Cls_Order();
        iakademi41Context context = new iakademi41Context();

        //constructor = ctor + tab + tab
        public HomeController()
        {
            cls_Product.mainpageCount = context.Settings.FirstOrDefault(s => s.SettingID == 1).MainpageCount;
            cls_Product.subpageCount = context.Settings.FirstOrDefault(s => s.SettingID == 1).SubpageCount;
        }

        public async Task<IActionResult> Index()
        {
            mpm.SliderProducts = cls_Product.ProductSelect("Slider", -1);
            mpm.NewProducts = cls_Product.ProductSelect("New", -1);
            mpm.SpecialProducts = cls_Product.ProductSelect("Special", -1);
            mpm.DiscountedProducts = cls_Product.ProductSelect("Discounted", -1);
            mpm.HighLightedProducts = cls_Product.ProductSelect("HighLighted", -1);
            mpm.TopSellerProducts = cls_Product.ProductSelect("TopSeller", -1);
            mpm.StarProducts = cls_Product.ProductSelect("Star", -1);
            mpm.OpportunityProducts = cls_Product.ProductSelect("Opportunity", -1);
            mpm.NotableProducts = cls_Product.ProductSelect("Notable", -1);
            mpm.Productofday = await cls_Product.ProductDetails(0);
            return View(mpm);
        }

        //alt sayfa ilk defa tıklanınca
        public IActionResult NewProducts()
        {
            mpm.NewProducts = cls_Product.ProductSelect("New", 0); //yeni
            return View(mpm);
        }

        //alt sayfa AJAX yaparken yeni ürünler
        public PartialViewResult _PartialNewProducts(string pageno)
        {
            int pagenumber = Convert.ToInt32(pageno);
            mpm.NewProducts = cls_Product.ProductSelect("New", pagenumber); //yeni
            return PartialView(mpm);
        }


        //alt sayfa ilk defa tıklanınca
        public IActionResult SpecialProducts()
        {
            mpm.SpecialProducts = cls_Product.ProductSelect("Special", 0); //yeni
            return View(mpm);
        }

        //alt sayfa AJAX yaparken
        public PartialViewResult _PartialSpecialProducts(string pageno)
        {
            int pagenumber = Convert.ToInt32(pageno);
            mpm.SpecialProducts = cls_Product.ProductSelect("Special", pagenumber); //yeni
            return PartialView(mpm);
        }

        public IActionResult DiscountedProducts()
        {
            mpm.DiscountedProducts = cls_Product.ProductSelect("Discounted", 0); //indirimli
            return View(mpm);
        }

        //alt sayfa AJAX yaparken
        public PartialViewResult _PartialDiscountedProducts(string pageno)
        {
            int pagenumber = Convert.ToInt32(pageno);
            mpm.DiscountedProducts = cls_Product.ProductSelect("Discounted", pagenumber); //indirimli
            return PartialView(mpm);
        }

        public IActionResult HighlightedProducts()
        {
            mpm.HighLightedProducts = cls_Product.ProductSelect("HighLighted", 0); //öne cıkanlar
            return View(mpm);
        }

        //alt sayfa AJAX yaparken
        public PartialViewResult _PartialHighlightedProducts(string pageno)
        {
            int pagenumber = Convert.ToInt32(pageno);
            mpm.HighLightedProducts = cls_Product.ProductSelect("HighLighted", pagenumber); //öne cıkanlar
            return PartialView(mpm);
        }


       
        public IActionResult TopsellerProducts(int page = 1)
        {
             //manage nuget pacgages install
            //  X.pagedlist (10.0.3)
            // X.Web.PagedList (10.1.2)
            //  İPTAL -> X.pagedlist.Mvc.Core (8.4.7) 

            // eger page = null ise , int page = 1 ile , page e 1 değeri verdik
            
            cls_Product.page = page; //property ye değer atadık
            //using X.PagedList; (PagedList ctrl .)
            PagedList<Product> model = cls_Product.TopsellerProductsList();
            return View("TopsellerProducts",model);
        }


        //menüden kategori tıklanınca,o kategoriye ait ürünler getiren sayfa
        public async Task<IActionResult> CategoryPage(int id)
        {
            List<Product> products = cls_Product.ProductSelect(id,"Category");

            Category category = await cls_Category.CategoryDetails(id);
            ViewBag.Header = category.CategoryName;

            return View(products);
        }

        //footer kısmından marka tıklanınca,o markaya ait ürünler getiren sayfa
        public async Task<IActionResult> SupplierPage(int id)
        {
            List<Product> products = cls_Product.ProductSelect(id, "Supplier");

            Supplier supplier = await cls_Supplier.SupplierDetails(id);
            ViewBag.Header = supplier.BrandName;

            return View(products);
        }

        //projenin herhangi bir sayfasında sepete ekle butonu tıklanınca buraya gelecek
        public IActionResult CartProcess(int id)
        {
            //çerez(cookie)
            //ProductID=Adet  &  ProductID=Adet
            //10=1 & 20=1 & 30=1

            //hangi sayfadan sepete ekle yapıldıysa,ürün sepete eklendikten sonra yine aynı sayfada kalacak
            string refererUrl = Request.Headers["Referer"].ToString(); //url yolunu bulur
            string url = "";

            if (id > 0)
            {
                Cls_Product.Highlighted_Increase(id); //HighLighted kolonunun değerini arttırdım(öne cıkanlar)

                cls_Order.ProductID = id;
                cls_Order.Quantity = 1;

                var cookieOptions = new CookieOptions();
                var cookie = Request.Cookies["sepetim"];//tarayıcıda sepetim isminde,daha önceden yaratılmış sepet varmı

                if (cookie == null)
                {
                    //sepetim diye birşey yok,kullanıcı sepetine henüz hiçbirsey eklememiş,dolayısıyla sepetim diye birsey olusturmamısım
                    cookieOptions.Expires = DateTime.Now.AddDays(1);//1 günlük çerez
                    cookieOptions.Path = "/";
                    cls_Order.MyCart = "";
                    //sepete ekle metodunu cagıracagım
                    cls_Order.AddToMyCart(id.ToString());
                    //property deki sepet bilgisini , tarayıcıya gönder
                    Response.Cookies.Append("sepetim", cls_Order.MyCart, cookieOptions);
                    TempData["Message"] = "Ürün sepetinize eklendi";
                }
                else
                {
                    //daha önceden sepete eklenen ürünler var
                    //10=1&20=1
                    cls_Order.MyCart = cookie; // tarayıcıdaki sepetim içerisindeki daha önceki ürünleri property'e gönderdim.
                    if (cls_Order.AddToMyCart(id.ToString()) == false)
                    {
                        //aynı ürün daha önce sepete eklenmediyse, burada ekleyecegim
                        HttpContext.Response.Cookies.Append("sepetim", cls_Order.MyCart, cookieOptions);
                        TempData["Message"] = "Ürün sepetinize eklendi";
                    }
                    else
                    {
                        TempData["Message"] = "Bu ürün zeten sepetinizde var";
                    }
                }

                Uri refererUri = new Uri(refererUrl, UriKind.Absolute);
                url = refererUri.AbsolutePath; // Get the path part of the URL

                // Check the path for specific criteria and redirect accordingly
                if (url.Contains("DpProducts") || refererUrl.Contains("http://localhost:7263"))
                {
                    return RedirectToAction("Index");
                }
                return Redirect(url);

            }
            else
            {
                // Handle cases where id is not greater than 0
                Uri refererUri = new Uri(refererUrl, UriKind.Absolute);
                url = refererUri.AbsolutePath; // Get the path part of the URL

                if (url.Contains("DpProducts"))
                {
                    return RedirectToAction("Index");
                }
                return Redirect(url);
            }
        }

        public IActionResult Cart() 
        {
            if (HttpContext.Request.Query["ProductID"] == "")
            {
                //sag üst köseden sepet tıklanınca
                var cookie = Request.Cookies["sepetim"]; //tarayıcıdan(browser) sepet bilgilerini al gel
                if (cookie == null)
                {
                    //sag üst köseden sepet tıklanınca sepet boş
                    cls_Order.MyCart = "";
                    ViewBag.Sepetim = cls_Order.SelectMyCart(); //icinde kayıt yok, count=0
                }
                else
                {
                    //sag üst köseden sepet tıklanınca , sepette en az 1 ürün varsa
                    cls_Order.MyCart = Request.Cookies["sepetim"];
                    ViewBag.Sepetim = cls_Order.SelectMyCart();  // sepeti listele
                }
            }
            else
            {
                string? ProductID = HttpContext.Request.Query["ProductID"]; //cshtml sayfasından parametre (metod parametresi olarak yakalanmayacaksa , bu sekilde yakalarız)
                cls_Order.MyCart = Request.Cookies["sepetim"];  //çerezi alıp property ye gönderiyorum
                cls_Order.DeleteFromMyCart(ProductID); //sil metodunu tetikliyorum
                var cookieOptions = new CookieOptions();
                Response.Cookies.Append("sepetim", cls_Order.MyCart, cookieOptions);
                cookieOptions.Expires = DateTime.Now.AddDays(1);
                TempData["Message"] = "Ürün Sepetten Silindi";
                ViewBag.Sepetim = cls_Order.SelectMyCart();
            }
            return View();
        }


        public IActionResult Order()
        {
            if (HttpContext.Session.GetString("Email") != null)
            {
                //kullanıcı Login.cshtml den giriş yapıp , Session alıp gelmiştir,Modelle kullanıcının bilgilerini gösterecegim
                User? usr = Cls_User.SelectMemberInfo(HttpContext.Session.GetString("Email"));
                 return View(usr);
            }
            else
            {
                //kullanıcı Login.cshtml ye gitmemiş , Session alıp gelmemiş
                return RedirectToAction("Login");
            }
        }



        [HttpPost]
        public IActionResult Order(IFormCollection frm)
        {
            // string kredikartno = Request.Form["kredikartno"]; //1. yol
            // string kredikartay = frm["kredikartno"]; // 2. yol

            string txt_individual = Request.Form["txt_individual"]; //bireysel
            string txt_corporate = Request.Form["txt_corporate"]; //kurumsal

            if (txt_individual != null)
            {
                //bireysel fatura
                //digital planet
              //  WebServiceController.tckimlik_vergi_no = txt_individual;
                Cls_Order.tckimlik_vergi_no = txt_individual;
                cls_Order.EfaturaCreate();
            }
            else
            {
                //kurumsal fatura
                //WebServiceController.tckimlik_vergi_no = txt_corporate;
                Cls_Order.tckimlik_vergi_no = txt_corporate;
                cls_Order.EfaturaCreate();
            }

            string kredikartno = Request.Form["kredikartno"];
            string kredikartay = frm["kredikartay"];
            string kredikartyil = frm["kredikartyil"];
            string kredikartcvs = frm["kredikartcvs"];

            return RedirectToAction("backref");

            //buradan sonraki kodlar , payu , iyzico

            //payu dan gelen örnek kodlar

            /* AŞAGIDAKİ KODLAR GERÇEK HAYATTA AÇILALAK 
             
            NameValueCollection data = new NameValueCollection();
            string url = "https://www.sedattefci.com/backref";

            data.Add("BACK_REF", url);
            data.Add("CC_CVV", kredikartcvs);
            data.Add("CC_NUMBER", kredikartno);
            data.Add("EXP_MONTH", kredikartay);
            data.Add("EXP_YEAR", "20" + kredikartyil);

            var deger = "";

            foreach (var item in data)
            {
                var value = item as string;
                var byteCount = Encoding.UTF8.GetByteCount(data.Get(value));
                deger += byteCount + data.Get(value);
            }

            var signatureKey = "size verilen SECRET_KEY buraya yazılacak";

            var hash = HashWithSignature(deger, signatureKey);

            data.Add("ORDER_HASH", hash);

            var x = POSTFormPAYU("https://secure.payu.com.tr/order/....", data);

            //sanal kart
            if (x.Contains("<STATUS>SUCCESS</STATUS>") && x.Contains("<RETURN_CODE>3DS_ENROLLED</RETURN_CODE>"))
            {
                //sanal kart (debit kart) ile alış veriş yaptı , bankadan onay aldı
            }
            else
            {
                //gerçek kart ile alış veriş yaptı , bankadan onay aldı
            }
            */
        }


        public static string OrderGroupGUID = "";
        public IActionResult backref()
        {
            //sipariş tablosuna kaydet
            //sepetim cookie sinden sepeti temizleyecegiz
            //e-fatura olustur metodunu cagır
            var cookieOptions = new CookieOptions();
            var cookie = Request.Cookies["sepetim"];
            if (cookie != null)
            {
                cls_Order.MyCart = cookie;
                OrderGroupGUID = cls_Order.OrderCreate(HttpContext.Session.GetString("Email").ToString());

                cookieOptions.Expires = DateTime.Now.AddDays(1);
                Response.Cookies.Delete("sepetim"); //tarayıcıdan sepeti sil
                                                    //    cls_User.Send_Sms(OrderGroupGUID);
                                                    //   cls_User.Send_Email(OrderGroupGUID);
            }
            return RedirectToAction("ConfirmPage");
        }

        public IActionResult ConfirmPage()
        {
            ViewBag.OrderGroupGUID = OrderGroupGUID;
            return View();
        }

        //detaylı arama
        public IActionResult DetailedSearch()
        {
            ViewBag.Categories = context.Categories.ToList();
            ViewBag.Suppliers = context.Suppliers.ToList();
            return View();
        }

        //detaylı arama ara butonu
        public IActionResult DProducts(int CategoryID, string[] SupplierID, string price, string IsInStock)
        {
            price = price.Replace(" ", "").Replace("TL", "");
            string[] PriceArray = price.Split('-');
            string startprice = PriceArray[0];
            string endprice = PriceArray[1];

            string sign = ">";
            if (IsInStock == "0")
            {
                sign = ">=";
            }

            string suppliervalue = ""; //1,2,4
            for (int i = 0; i < SupplierID.Length; i++)
            {
                if (i == 0)
                {
                    //ilk marka
                    suppliervalue = "SupplierID =" + SupplierID[i];
                }
                else
                {
                    //ikinci ve sonrası markalar
                    suppliervalue += " or SupplierID =" + SupplierID[i];
                }
            }

            string query = "select * from Products where CategoryID=" + CategoryID + " and (" + suppliervalue + ") and (UnitPrice >= " + startprice + " and UnitPrice <= " + endprice + ") and Stock " + sign + " 0 order by UnitPrice";

            ViewBag.Products = cls_Product.SelectProductsByDetails(query);

            return View();
        }

        //siparislerim
        //arama
        //faydalı bilgiler
        //ürün detayı
        //hakkımızda - iletişim 
        //footer 

        public IActionResult Details(int id)
        {
            Cls_Product.Highlighted_Increase(id); //HighLighted kolonunun değerini arttırdım(öne cıkanlar)
            return View();
        }


        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(User user)
        {
            if (ModelState.IsValid)
            {
                if (Cls_User.loginEmailControl(user) == false)
                {
                    //email kayıtlı değil, yeni kayıt yapılacak
                    bool answer = Cls_User.AddUser(user);

                    if (answer)
                    {
                        TempData["Message"] = "Kaydedildi.";
                        return RedirectToAction("Login");
                    }
                    TempData["Message"] = "Hata.Tekrar deneyiniz.";
                }
                else
                {
                    TempData["Message"] = "Bu Email Zaten mevcut.Başka Deneyiniz.";
                }
            }
            return View();
        }


        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(User user)
        {
            //Session icin program.cs icinde 2 satır ekleme yaptık
            string answer = Cls_User.MemberControl(user);

            if (answer == "error")
            {
                TempData["Message"] = "Email/Şifre yanlış girildi";
                return View();
            }
            else if (answer == "admin")
            {
                HttpContext.Session.SetString("Email", answer);
                HttpContext.Session.SetString("Admin", answer);
                return RedirectToAction("Index", "Admin");
            }
            else
            {
                HttpContext.Session.SetString("Email", answer);
                return RedirectToAction("Index");
            }
        }


        public IActionResult Logout()
        {
            HttpContext.Session.Remove("Email");
            HttpContext.Session.Remove("Admin");
            return RedirectToAction("Index");
        }



        //menüden siparislerimtıklanınca
        public IActionResult MyOrders()
        {
            //daha önceden giriş yapmış
            if (HttpContext.Session.GetString("Email") != null)
            {
                List<Vw_MyOrder> orders = cls_Order.SelectMyOrders(HttpContext.Session.GetString("Email").ToString());
                return View(orders);
            }
            else
            { //yapmamış
                return RedirectToAction("Login");
            }
        }


        public IActionResult AboutUs()
        {
            return View();
        }


        public IActionResult ContactUs()
        {
            return View();
        }

        public IActionResult Message(Message message)
        {
            return View();
        }

    }
}
