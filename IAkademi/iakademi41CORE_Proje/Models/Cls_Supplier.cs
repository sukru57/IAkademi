using iakademi41CORE_Proje.Models.MVVM;
using Microsoft.EntityFrameworkCore;
using XAct;

namespace iakademi41CORE_Proje.Models
{
    public class Cls_Supplier
    {

        iakademi41Context context = new iakademi41Context();

        public List<Supplier> SupplierSelect()
        {
            List<Supplier> suppliers = context.Suppliers.ToList();
            return suppliers;
        }

        //  bool answer = Cls_Supplier.SupplierInsert(supplier);
        public static bool SupplierInsert(Supplier supplier)
        {
            try
            {
                //metod static oldugu icin , context burada tanımlamak zorundayım
                using (iakademi41Context context = new iakademi41Context())
                {
                    context.Add(supplier);
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<Supplier> SupplierDetails(int? id)
        {
            // Supplier? supplier = await context.Suppliers.FirstOrDefaultAsync(p => p.SupplierID == id);
            Supplier? supplier = await context.Suppliers.FindAsync(id);
            return supplier;
        }

        public static bool SupplierUpdate(Supplier supplier)
        {
            try
            {
                //metod static oldugu icin , context burada tanımlamak zorundayım
                using (iakademi41Context context = new iakademi41Context())
                {
                    context.Update(supplier);
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool SupplierDelete(int id)
        {
            try
            {
                //gelen id ile database den eski kaydını alıyorum,Active kolonuna false basıyorum
                Supplier supplier = context.Suppliers.FirstOrDefault(s => s.SupplierID == id);
                supplier.Active = false;
                context.SaveChanges();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


    }
}
