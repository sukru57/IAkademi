using iakademi41CORE_Proje.Models;

namespace iakademi41CORE_Proje.ViewComponents
{
    public class Address
    {

        iakademi41Context context = new iakademi41Context();
        public string Invoke()
        {
            string address = context.Settings.FirstOrDefault(s => s.SettingID == 1).Address;
            return $"{address}";
        }


    }
}
