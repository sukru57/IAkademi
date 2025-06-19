using Microsoft.Data.SqlClient;

namespace iakademi41CORE_Proje.Models
{
    public class Connection
    {

        public static SqlConnection ServerConnect
        {
            //TrustServerCertificate=True;
            get
            {
                SqlConnection sqlcon = new SqlConnection("Server=localhost\\SQLEXPRESS;Trusted_Connection=True;Database=iakademi41Core_projeDB;TrustServerCertificate=True;");

                return sqlcon;
            }
        }

    }
}
