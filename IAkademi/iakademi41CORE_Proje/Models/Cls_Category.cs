using iakademi41CORE_Proje.Models.MVVM;
using Microsoft.EntityFrameworkCore;

namespace iakademi41CORE_Proje.Models
{
    public class Cls_Category
    {
        iakademi41Context context = new iakademi41Context(); //context i sadece static olmayanlar cagırabilir..static metodlar icinde using yaparak kullanacagız

        //asagıdaki sekilde Controller dan cagırıyoruz
        //List<Category> categories = cls_Category.CategorySelect("all");

        public List<Category> CategorySelect(string value)
        {
            List<Category> categories;
            if (value == "all")
            {
                //hepsi
                categories = context.Categories.ToList(); 
            }
            else
            {
                //ana kategoriler 
                 categories = context.Categories.Where(c => c.ParentID == 0).ToList();
            }
            
            return categories;
        }

        public static bool CategoryInsert(Category category)
        {
            try
            {
                //metod static oldugu icin , context burada tanımlamak zorundayım
                using (iakademi41Context context = new iakademi41Context())
                {
                    if (category.ParentID == null)
                    {
                        category.ParentID = 0;
                    }
                    context.Add(category);
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }


        // Category category = await cls_Category.CategoryDetails(id);
        public async Task<Category> CategoryDetails(int? id)
        {
            Category? category =await context.Categories.FirstOrDefaultAsync(c => c.CategoryID == id);
            return category;
        }




        public static bool CategoryUpdate(Category category)
        {
            try
            {
                //metod static oldugu icin , context burada tanımlamak zorundayım
                using (iakademi41Context context = new iakademi41Context())
                {
                    if (category.ParentID == null)
                    {
                        category.ParentID = 0;
                    }

                    //  category.ParentID = category.ParentID == null ? 0 : 1;  //turner if

                    context.Update(category);
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }


        public static bool CategoryDelete(int id)
        {
            try
            {
                using (iakademi41Context context = new iakademi41Context())
                {
                    //eski kaydını veritabanından getiriyorum
                    Category? category = context.Categories.FirstOrDefault(c => c.CategoryID == id);
                    category.Active = false;

                    //eger silinen ana kategori ise , alt kategori varsa bakıyorum ve siliyorum
                    List<Category> categoryList = context.Categories.Where(c => c.ParentID == id).ToList();
                    foreach (var item in categoryList)
                    {
                        //categoryList boş değilse foreach içine girer ,alt kategorileride siler
                        item.Active = false;
                    }

                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }



    }
}
