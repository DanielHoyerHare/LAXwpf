using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data.SqlClient;
using System.Configuration;

namespace LAX_program
{
    /// <summary>
    /// Interaction logic for Details.xaml
    /// </summary>
    public partial class Details : Window
    {
        private string ConString = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
        private int ID { get; set; }
        public Details()
        {
            InitializeComponent();
            loaddata();
        }
        private void loaddata()
        {
            ID = (Application.Current.MainWindow as MainWindow).ID;
            string CmdString = "SELECT Titel, Instruktør, Årstal, Længde, Rating, Genre FROM Films WHERE FilmID = '" + ID + "'";
            using (SqlConnection con = new SqlConnection(ConString))
            {
                SqlCommand cmd = new SqlCommand(CmdString, con);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable("Films");
                sda.Fill(dt);
                Titel.Text = dt.Rows[0].ItemArray[0].ToString();
                Instruktør.Text = dt.Rows[0].ItemArray[1].ToString();
                Årstal.Text = dt.Rows[0].ItemArray[2].ToString();
                Længde.Text = dt.Rows[0].ItemArray[3].ToString();
                Rating.Text = dt.Rows[0].ItemArray[4].ToString();
                Genre.Text = dt.Rows[0].ItemArray[5].ToString();
            }
        }
        private void Slet(object sender, RoutedEventArgs e)
        {
            this.Close();
            (Application.Current.MainWindow as MainWindow).delete();
        }
        private void Rediger(object sender, RoutedEventArgs e)
        {
            this.Close();
            Edit p = new Edit();
            p.Show();
        }
    }
}
