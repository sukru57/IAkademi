using iakademi41CORE_Proje.Models.MVVM;
using iakademi41CORE_Proje.Models;
using Microsoft.AspNetCore.Mvc;

namespace iakademi41CORE_Proje.ViewComponents
{
    public class Contact : ViewComponent
    {

        iakademi41Context context = new iakademi41Context();

        public IViewComponentResult Invoke()
        {
            Setting setting = context.Settings.FirstOrDefault(s => s.SettingID == 1);
            return View(setting);
        }

    }
}
