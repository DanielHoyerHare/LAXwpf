using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Configuration;

namespace LAX_program
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public int ID { get; set; }
        private DataRowView data { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            FillDataGrid("SELECT FilmID, Titel, Årstal, Længde, Rating FROM Films ORDER BY FilmID");
        }
        public void FillDataGrid(string CmdString)
        {
            try
            {
                string ConString = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
                using (SqlConnection con = new SqlConnection(ConString))
                {
                    SqlCommand cmd = new SqlCommand(CmdString, con);
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable("Film");
                    sda.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        Tabel.ItemsSource = dt.DefaultView;
                        Tabel.ColumnWidth = (Tabel.Width - 10) / 5;
                        NoData.Visibility = Visibility.Hidden;
                    }
                    else
                    {
                        Tabel.ItemsSource = null;
                        NoData.Visibility = Visibility.Visible;
                    }
                }
            }
            catch
            {
                MessageBox.Show("Forbindelse til database fejlede.", "Fejl", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void add_Click(object sender, RoutedEventArgs e)
        {
            Add p = new Add();
            p.Show();
        }
        private void edit_Click(object sender, RoutedEventArgs e)
        {
            if (data != null)
            {
                Edit p = new Edit();
                p.Show();
            }
            else
            {
                MessageBox.Show("Du skal vælge et element i tabellen", "Fejl", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void details_Click(object sender, RoutedEventArgs e)
        {
            if (data != null)
            {
                Details p = new Details();
                p.Show();
            }
            else
            {
                MessageBox.Show("Du skal vælge et element i tabellen", "Fejl", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void remove_Click(object sender, RoutedEventArgs e)
        {
            if (data != null)
            {
                delete();
            }
            else
            {
                MessageBox.Show("Du skal vælge et element i tabellen", "Fejl", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void reloadData(object sender, RoutedEventArgs e)
        {
            FillDataGrid("SELECT FilmID, Titel, Årstal, Længde, Rating FROM Films ORDER BY FilmID");
        }
        public void delete()
        {
            MessageBoxResult result = MessageBox.Show("Er du sikker på at du slette filmen " + data.Row[1].ToString() + "", "Slet film", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                Sql sql = new Sql();
                sql.SqlCmd("DELETE FROM Films WHERE FilmID = '" + ID + "'");
                FillDataGrid("SELECT FilmID, Titel, Årstal, Længde, Rating FROM Films ORDER BY FilmID");
            }
        }
        private void KeyHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                string searchkey = searchbar.Text;
                searchbar.Text = null;
                FillDataGrid("SELECT FilmID, Titel, Årstal, Længde, Rating FROM Films WHERE FilmID LIKE '%" + searchkey + "%' OR Titel LIKE '%" + searchkey + "%' OR Årstal LIKE '%" + searchkey + "%' OR Længde LIKE '%" + searchkey + "%' OR Rating LIKE '%" + searchkey + "%' ORDER BY FilmID");
            }
        }

        private void Tabel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            data = (DataRowView)Tabel.SelectedItem;
            if (data != null)
            {
                ID = Convert.ToInt32(data.Row[0].ToString());
            }
        }
    }
}
