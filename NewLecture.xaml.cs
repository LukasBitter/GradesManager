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

namespace gradesManager
{
    /// <summary>
    /// Logique d'interaction pour Window1.xaml
    /// </summary>
    public partial class NewLecture : Window
    {
        string lectureName;
        double weight;

        public double Weight
        {
            get
            {
                return weight;
            }

            set
            {
                weight = value;
            }
        }

        public string LectureName
        {
            get
            {
                return lectureName;
            }

            set
            {
                lectureName = value;
            }
        }

        public NewLecture()
        {
            InitializeComponent();
            cbx_weight.Items.Add(0.25);
            cbx_weight.Items.Add(0.50);
            cbx_weight.Items.Add(0.75);
            cbx_weight.Items.Add(1.00);
            cbx_weight.Items.Add(1.25);
            cbx_weight.Items.Add(1.50);
            cbx_weight.Items.Add(1.75);
            cbx_weight.Items.Add(2.00);
            cbx_weight.Items.Add(2.25);
            cbx_weight.Items.Add(2.75);
            cbx_weight.Items.Add(3.00);
            cbx_weight.SelectedIndex = 3;
        }

        private void btn_ok_Click(object sender, RoutedEventArgs e)
        {
            if(txt_lectureName.Text != "")
            {
                lectureName = txt_lectureName.Text;
                Weight = Convert.ToDouble(cbx_weight.Text);
                //Console.WriteLine("input: " + lectureName + " / " + Weight);
                DialogResult = true;
                Close();
            }
            else
            {
                MessageBox.Show("Please provide a name for the new lecture");
            }

        }

        private void btn_cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
