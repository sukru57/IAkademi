using iakademi41CORE_Proje.Models.MVVM;
using Microsoft.EntityFrameworkCore;
using System.Text;
using XSystem.Security.Cryptography;

namespace iakademi41CORE_Proje.Models
{
    public class Cls_User
    {
        //Metod yazma formülü
        //1 - erişim belirleyicisi (public,private,protected)
        //2 - static yazılır yada yazılmaz , metodu new diye cagırırsam static yok
        //3 - geriye dönüş tipi , metod geriye birsey döndürmüyorsa void yazılır
        //4 - metod ismi
        //5 - parantez içinde varsa parametre
        //6 - eger 3 numaradaki geri dönüş tipi void değilse,
        //yani metod geriye birşey döndürüyorsa,
        //metod icinde ***return ZORUNLU*** ile geriye değer döndürülür

        //  User? usr =  cls_User.loginControl(user);   bu sekilde cagıracagız

        iakademi41Context context = new iakademi41Context();

        public async Task<User?> loginControl(User user)
        {
            string md5Sifre = MD5Sifrele(user.Password); //d375af34cc08aba9a1cc9b6596a70c36

            User? usr = await context.Users.FirstOrDefaultAsync(u => u.Email == user.Email && u.Password == md5Sifre);

            return usr;
        }

        //Xact.core.pcl (nuget)

        public static string MD5Sifrele(string value)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] btr = Encoding.UTF8.GetBytes(value);
            btr = md5.ComputeHash(btr);

            StringBuilder sb = new StringBuilder();
            foreach (byte item in btr)
            {
                sb.Append(item.ToString("x2").ToLower());
            }
            return sb.ToString();
        }

        public static bool loginEmailControl(User user)
        {
            using (iakademi41Context context = new iakademi41Context())
            {
                User? usr = context.Users.FirstOrDefault(u => u.Email == user.Email);

                if (usr == null)
                {
                    return false;
                }
                return true;
            }
        }

        public static bool AddUser(User user)
        {
            using (iakademi41Context context = new iakademi41Context())
            {
                try
                {
                    user.Active = true;
                    user.IsAdmin = false;
                    user.Password = MD5Sifrele(user.Password);
                    context.Users.Add(user);
                    context.SaveChanges();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }


        public static string MemberControl(User user)
        {
            using (iakademi41Context context = new iakademi41Context())
            {
                string answer = "";

                try
                {
                    string md5Sifre = MD5Sifrele(user.Password);
                    User? usr = context.Users.FirstOrDefault(u => u.Email == user.Email && u.Password == md5Sifre);

                    if (usr == null)
                    {
                        //kullanıcı yanlıs sifre veya emal girdi
                        answer = "error";
                    }
                    else
                    {
                        //kullanıcı veritabanında kayıtlı.
                        if (usr.IsAdmin == true)
                        {
                            //admin yetkisi olan personel giriş yapıyor
                            answer = "admin";
                        }
                        else  //siteden alısveriş yapacak olan normal kullanıcı
                        {
                            answer = usr.Email;
                        }
                    }
                }
                catch (Exception)
                {
                    return "HATA";
                }
                return answer;
            }
        }


        public static User? SelectMemberInfo(string Email)
        {
            using (iakademi41Context context = new iakademi41Context())
            {
                return context.Users.FirstOrDefault(u => u.Email == Email);
            }
        }


    }
}
