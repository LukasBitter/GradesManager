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
    public partial class NewModule : Window
    {
        string moduleName;
        
        public string ModuleName
        {
            get
            {
                return moduleName;
            }

            set
            {
                moduleName = value;
            }
        }

        public NewModule()
        {
            InitializeComponent();
        }

        private void btn_ok_Click(object sender, RoutedEventArgs e)
        {
            if(txt_moduleName.Text != "")
            {
                moduleName = txt_moduleName.Text;
                //Console.WriteLine("input: " + Name + " / " + Weight); 
                DialogResult = true;
                Close();
            }
            else
            {
                MessageBox.Show("Please provide a name for the new module");
            }

        }

        private void btn_cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
