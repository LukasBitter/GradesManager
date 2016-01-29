using Microsoft.Win32;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace gradesManager
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Cursus cursus;
        StudyYear displayedStudyYear;

        public MainWindow()
        {
            InitializeComponent();
            
            initYears();

            welcome();
            //displayCursus();
        }

        private void welcome()
        {
            MessageBox.Show("Load a model from tools menu, or open your saved cursus from file menu");
        }

        public void initYears()
        {
            if (cursus != null && cursus.StudyYears != null)
            {
                foreach (StudyYear sy in cursus.StudyYears)
                {
                    Button b = new Button();
                    b.Width = 75;
                    b.Content = sy.YearBegin;
                    studyYears.Children.Add(b);
                }
            }
        }

        public void displayCursus()
        {
            //panel.Children.Clear();
            //studyYearGrid.ItemsSource = null;
            studyYears.Children.Clear();

            Console.WriteLine("displaycursus.clear");

            if (cursus != null && cursus.StudyYears != null) {


                Console.WriteLine("cursus not null");
                if(displayedStudyYear == null)
                {
                   displayedStudyYear = cursus.StudyYears.ElementAt(0);
                }

                foreach (Module m in displayedStudyYear.Modules)
                {
                    Border myBorder1 = new Border();
                    myBorder1.Background = Brushes.SkyBlue;
                    myBorder1.BorderBrush = Brushes.Black;
                    myBorder1.BorderThickness = new Thickness(1);

                    StackPanel m_panel = new StackPanel();
                    m_panel.Orientation = Orientation.Vertical;

                    TextBox tb_m = new TextBox();
                    tb_m.Text = m.Name + " : " + m.getAverage().ToString();
                    tb_m.Foreground = Brushes.Black;
                    tb_m.FontSize = 14;
                    tb_m.FontWeight = FontWeights.Bold;

                    Button btn_addLecture = new Button();
                    btn_addLecture.Width = 150;
                    btn_addLecture.Content = "Add Lecture";
                    btn_addLecture.Click += (sender, e) =>
                    {
                        var dialog = new NewLecture();
                        if(dialog.ShowDialog() == true)
                        {
                            Lecture newLecture = new Lecture(dialog.LectureName, dialog.Weight);
                            m.addLecture(newLecture);
                        }
                        displayCursus();
                    };

                    m_panel.Children.Add(tb_m);
                    m_panel.Children.Add(btn_addLecture);
                    myBorder1.Child = m_panel;
                    studyYears.Children.Add(myBorder1);

                    foreach (Lecture l in m.Lectures)
                    {
                        StackPanel l_panel = new StackPanel();
                        l_panel.Orientation = Orientation.Vertical;

                        TextBox tb_l = new TextBox();
                        setLectureAverage(l, tb_l);

                        l_panel.Children.Add(tb_l);
                        m_panel.Children.Add(l_panel);

                        DataGrid dg_l = new DataGrid();
                        dg_l.Name = "test";
                        dg_l.Width = 250;
                        dg_l.Height = 30 + (l.Grades.Count + 1) * 25;
                        //dg_l.MouseUp += Dg_l_MouseUp;
                        //dg_l.SelectionChanged += Dg_l_SelectionChanged;
                        try
                        {
                            dg_l.ItemsSource = l.Grades;
                        }
                        catch (System.InvalidOperationException)
                        {
                            MessageBox.Show("Please validate your entry or modification with 'enter' key");
                        }
                        dg_l.CanUserAddRows = true;
                        m_panel.Children.Add(dg_l);
                        Button b = new Button();
                        b.Width = 150;
                        b.Content = "Delete selected grade";
                        b.Click += (sender, e) =>
                        {
                            l.removeGrade(dg_l.SelectedItem);
                            displayCursus();
                        };
                        l_panel.Children.Add(b);
                    }
                }
            }
        }

        private static void setLectureAverage(Lecture l, TextBox tb_l)
        {
            tb_l.Text = l.Name + " (weight " + l.Weight + ") : " + l.getAverage().ToString();
        }

        private void Dg_l_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid d = (DataGrid)sender;
            Console.WriteLine(d.SelectedItem.GetType());
        }

        private void Dg_l_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Console.WriteLine("mouse up");
            displayCursus();
        }

        private void mnuNew_Click(object sender, RoutedEventArgs e)
        {
            cursus = new Cursus();
            displayCursus();
        }

        private void mnuOpen_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            openFileDialog.Filter = "Grade file (*.grad)|*.grad";
            if (openFileDialog.ShowDialog() == true)
            {
                string inputString;
                inputString = File.ReadAllText(openFileDialog.FileName);
                cursus = JsonConvert.DeserializeObject<Cursus>(inputString);
            }
            Console.WriteLine("open_click");
            displayCursus();
        }

        private void mnuSave_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            saveFileDialog.Filter = "Grade file (*.grad)|*.grad";
            if (saveFileDialog.ShowDialog() == true)
            {
                Console.WriteLine("mnu_Save_Click: saveFileDialog.ShowDialog() == true");
                string output = JsonConvert.SerializeObject(cursus, Formatting.Indented);
                JsonSerializer serializer = new JsonSerializer();
                serializer.Converters.Add(new JavaScriptDateTimeConverter());
                serializer.NullValueHandling = NullValueHandling.Ignore;

                using (StreamWriter sw = new StreamWriter(saveFileDialog.FileName))
                using (JsonWriter writer = new JsonTextWriter(sw))
                {
                    serializer.Serialize(writer, cursus);
                }
            }
        }

        private void mnuExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }


        private void mnuRefresh_Click(object sender, RoutedEventArgs e)
        {
            displayCursus();
        }

        private void mnuLoadModelDLM1_Click(object sender, RoutedEventArgs e)
        {
            initCursusINFDLM1();
            displayCursus();
        }

        private void mnuLoadModelDLM3_Click(object sender, RoutedEventArgs e)
        {
            initCursusINFDLM3();
            displayCursus();
        }

        private void mnuAddModule_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("add module click");
            var dialog = new NewModule();
            if (dialog.ShowDialog() == true)
            {
                Console.WriteLine("add module");
                Module newModule = new Module(dialog.ModuleName);
                displayedStudyYear.addModule(newModule);
                Console.WriteLine("module count: " + displayedStudyYear.Modules.Count);
            }
            else
            {
                Console.WriteLine("showDialog != true");
            }
            
            displayCursus();
        }
        

        private void mnuHelp_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Grades management - Lukas Bitter - He-Arc Ingéniérie DLM - 20.01.2016");
        }

        private void mnuReadme_Click(object sender, RoutedEventArgs e)
        {
            string file_name = "\\README.txt";
            file_name = Directory.GetCurrentDirectory() + file_name;
            if (System.IO.File.Exists(file_name) == true)
            {
                System.IO.StreamReader objReader;
                objReader = new System.IO.StreamReader(file_name);
                MessageBox.Show(objReader.ReadToEnd());
                objReader.Close();
            }
            else
            {
                MessageBox.Show("No such file: " + file_name);
            }
        }


        private void initCursusINFDLM1()
        {
            cursus = new Cursus(2016, "inf dlm", 3);

            List<StudyYear> studyYears = new List<StudyYear>();
            studyYears.Add(new StudyYear(2013));
            studyYears.Add(new StudyYear(2014));
            studyYears.Add(new StudyYear(2015));

            cursus.StudyYears = studyYears;

            Module m1 = new Module("Sciences IA");
            Module m2 = new Module("Sciences IB");
            Module m3 = new Module("Programmation I");

            studyYears.ElementAt(0).Modules.Add(m1);
            studyYears.ElementAt(0).Modules.Add(m2);
            studyYears.ElementAt(0).Modules.Add(m3);

            Lecture l1_1 = new Lecture("Math IA", 3);
            Lecture l1_2 = new Lecture("Physique IA", 2);
            m1.Lectures.Add(l1_1);
            m1.Lectures.Add(l1_2);

            Lecture l2_1 = new Lecture("Math IB", 5);
            Lecture l2_2 = new Lecture("Physique IB", 3);
            Lecture l2_3 = new Lecture("Labo Physique IB", 2);
            m2.Lectures.Add(l2_1);
            m2.Lectures.Add(l2_2);
            m2.Lectures.Add(l2_3);

            Lecture l3_1 = new Lecture("Langage C", 3);
            Lecture l3_2 = new Lecture("Langage C++", 3);
            Lecture l3_3 = new Lecture("Algo I", 4);
            Lecture l3_4 = new Lecture("Assembleur", 2);
            m3.Lectures.Add(l3_1);
            m3.Lectures.Add(l3_2);
            m3.Lectures.Add(l3_3);
            m3.Lectures.Add(l3_4);

            Grade g1_1 = new Grade(4, 1);
            Grade g1_2 = new Grade(5, 1);
            Grade g1_3 = new Grade(5, 1);
            l1_1.Grades.Add(g1_1);
            l1_1.Grades.Add(g1_2);
            l1_1.Grades.Add(g1_3);

            Grade g2_1 = new Grade(5, 1);
            Grade g2_2 = new Grade(5.5, 1);
            Grade g2_3 = new Grade(4, 1);
            l1_2.Grades.Add(g2_1);
            l1_2.Grades.Add(g2_2);
            l1_2.Grades.Add(g2_3);

            Grade g3_1 = new Grade(4, 1);
            Grade g3_2 = new Grade(4.2, 1);
            Grade g3_3 = new Grade(4.8, 0.5);
            Grade g3_4 = new Grade(4.6, 0.5);
            l2_1.Grades.Add(g3_1);
            l2_1.Grades.Add(g3_2);
            l2_1.Grades.Add(g3_3);
            l2_1.Grades.Add(g3_4);

            Grade g4_1 = new Grade(5, 1);
            Grade g4_2 = new Grade(5.6, 1);
            Grade g4_3 = new Grade(5.3, 1);
            l2_2.Grades.Add(g4_1);
            l2_2.Grades.Add(g4_2);
            l2_2.Grades.Add(g4_3);

            Grade g5_1 = new Grade(4, 1);
            Grade g5_2 = new Grade(3.5, 0.5);
            Grade g5_3 = new Grade(3.2, 2);
            l2_3.Grades.Add(g5_1);
            l2_3.Grades.Add(g5_2);
            l2_3.Grades.Add(g5_3);

            Console.WriteLine(g5_1.getValue());
            Console.WriteLine(g5_1.getAverage());
            Console.WriteLine(g5_2.getValue());
            Console.WriteLine(g5_2.getAverage());
            
            Console.WriteLine("grades count: " + l2_3.Grades.Count);
            Console.WriteLine("lecture average: " + l2_3.getAverage());

            Console.WriteLine("m1 average: " + m1.getAverage());
            Console.WriteLine("m2 average: " + m2.getAverage());
            Console.WriteLine("m3 average: " + m3.getAverage());            
        }

        private void initCursusINFDLM3()
        {
            cursus = new Cursus(2015, "inf dlm", 3);

            List<StudyYear> studyYears = new List<StudyYear>();
            studyYears.Add(new StudyYear(2015));

            cursus.StudyYears = studyYears;

            Module m1 = new Module("Gestion");
            Module m2 = new Module("Imagerie numérique");
            Module m3 = new Module("Développement système");
            Module m4 = new Module("Développement web et mobile");
            Module m5 = new Module("IA et frameworks");
            Module m6 = new Module("Projet P3");

            studyYears.ElementAt(0).Modules.Add(m1);
            studyYears.ElementAt(0).Modules.Add(m2);
            studyYears.ElementAt(0).Modules.Add(m3);
            studyYears.ElementAt(0).Modules.Add(m4);
            studyYears.ElementAt(0).Modules.Add(m5);
            studyYears.ElementAt(0).Modules.Add(m6);

            Lecture l1_1 = new Lecture("Gestion et économie d'entreprise", 1);
            Lecture l1_2 = new Lecture("Communication III", 1);
            Lecture l1_3 = new Lecture("Qualité du logiciel", 2);
            m1.Lectures.Add(l1_1);
            m1.Lectures.Add(l1_2);
            m1.Lectures.Add(l1_3);

            Lecture l2_1 = new Lecture("Infographie avec GLSL", 9);
            Lecture l2_2 = new Lecture("Traitement d'image", 9);
            Lecture l2_3 = new Lecture("GPGPU pour l'imagerie numérique", 6);
            m2.Lectures.Add(l2_1);
            m2.Lectures.Add(l2_2);
            m2.Lectures.Add(l2_3);

            Lecture l3_1 = new Lecture("Conception OS", 2);
            Lecture l3_2 = new Lecture("Compilateur", 2);
            Lecture l3_3 = new Lecture("Cryptographie", 1);
            Lecture l3_4 = new Lecture("Sécurité", 1);
            m3.Lectures.Add(l3_1);
            m3.Lectures.Add(l3_2);
            m3.Lectures.Add(l3_3);
            m3.Lectures.Add(l3_4);

            Lecture l4_1 = new Lecture("Développement mobile", 4);
            Lecture l4_2 = new Lecture("Développement Web", 5);
            m4.Lectures.Add(l4_1);
            m4.Lectures.Add(l4_2);

            Lecture l5_1 = new Lecture("Intelligence Artificielle", 1);
            Lecture l5_2 = new Lecture(".NET", 1);
            Lecture l5_3 = new Lecture("Java Entreprise Edition (JEE)", 1);
            m5.Lectures.Add(l5_1);
            m5.Lectures.Add(l5_2);
            m5.Lectures.Add(l5_3);

            Lecture l6_1 = new Lecture("Projet P3", 1);
            m6.Lectures.Add(l6_1);
        }
    }
}
