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
    /// Interaction logic for Edit.xaml
    /// </summary>
    public partial class Edit : Window
    {
        private string ConString = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
        private int ID { get; set; }
        public Edit()
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
            comboload();
        }
        public void comboload()
        {
            string CmdString = "SELECT * FROM Genre";
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
        private void Gem(object sender, RoutedEventArgs e)
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
                        Sql.SqlCmd("UPDATE Films SET Titel = '" + Titel.Text + "', Instruktør = '" + Instruktør.Text + "', Årstal = '" + Årstal.Text + "', Længde = '" + Længde.Text + "', Rating = '" + Rating.Text + "', Genre = '" + Genre.Text + "' WHERE FilmID = '" + ID + "'");
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
