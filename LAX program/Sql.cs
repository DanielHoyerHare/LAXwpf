using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LAX_program
{
    class Sql
    {
        public void SqlCmd(string CmdString)
        {
            string ConString = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(ConString))
            {
                using (SqlCommand com = con.CreateCommand())
                {
                    com.CommandText = CmdString;
                    con.Open();
                    try { com.ExecuteNonQuery(); }
                    catch { MessageBox.Show("Noget uforventet gik galt, prøv igen", "Fejl", MessageBoxButton.OK, MessageBoxImage.Error); }
                    con.Close();
                }
            }
        }
        public bool? tjekgenre(string genre)
        {
            string CmdString = "SELECT * FROM Genre WHERE Genre = '" + genre + "'";
            string ConString = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(ConString))
            {
                SqlCommand cmd = new SqlCommand(CmdString, con);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable("Genre");
                sda.Fill(dt);
                try
                {
                    var value = dt.Rows[0].ItemArray[0];
                    return null;
                }
                catch
                {
                    MessageBoxResult result = MessageBox.Show("Genren du har valgt eksisterer ikke, ønsker du gemme den som en ny genre?", "Genre eksisterer ikke", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes) { return true; }
                    else { return false; }
                }
            }
        }
    }
}
