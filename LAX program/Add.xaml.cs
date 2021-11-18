using System;
using System.Collections.Generic;
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
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace LAX_program
{
    /// <summary>
    /// Interaction logic for Add.xaml
    /// </summary>
    public partial class Add : Window
    {
        public Add()
        {
            InitializeComponent();
            comboload();
        }
        public void comboload()
        {
            string CmdString = "SELECT * FROM Genre";
            string ConString = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(ConString))
            {
                SqlCommand cmd = new SqlCommand(CmdString, con);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable("Genre");
                sda.Fill(dt);
                Genre.ItemsSource = dt.DefaultView;
                Genre.DisplayMemberPath = dt.Columns["Genre"].ToString();
            }
        }

        private void Tilføj(object sender, RoutedEventArgs e)
        {
            if (Titel.Text == null || Instruktør.Text == null || Årstal.Text == null || Længde.Text == null || Rating.Text == null || Genre.Text == null)
            {
                MessageBox.Show("Du mangler at udfylde et eller flere fælter.", "Fejl", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                bool fejl = false;
                try { Convert.ToInt32(Årstal.Text); }
                catch 
                {
                    MessageBox.Show("Årstal skal være et tal.", "Fejl", MessageBoxButton.OK, MessageBoxImage.Error);
                    fejl = true;
                }
                try 
                {
                    int rating = Convert.ToInt32(Rating.Text);
                    if (rating < 0 || rating > 10)
                    {
                        MessageBox.Show("Rating skal være et tal mellem 0 og 10.", "Fejl", MessageBoxButton.OK, MessageBoxImage.Error);
                        fejl = true;
                    }
                }
                catch
                {
                    MessageBox.Show("Rating skal være et tal.", "Fejl", MessageBoxButton.OK, MessageBoxImage.Error);
                    fejl = true;
                }
                if (fejl == false)
                {
                    Sql Sql = new Sql();
                    bool? nygenre = Sql.tjekgenre(Genre.Text);
                    if (nygenre != false)
                    {
                        if (nygenre == true)
                        {
                            Sql.SqlCmd("INSERT INTO Genre VALUES ('" + Genre.Text + "')");
                        }
                        Sql.SqlCmd("INSERT INTO Films VALUES ('" + Titel.Text + "','" + Instruktør.Text + "','" + Årstal.Text + "','" + Længde.Text + "','" + Rating.Text + "','" + Genre.Text + "')");
                        this.Close();
                        (Application.Current.MainWindow as MainWindow).FillDataGrid("SELECT FilmID, Titel, Årstal, Længde, Rating FROM Films");
                    }
                }
            }
        }

        private void Annuller(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
