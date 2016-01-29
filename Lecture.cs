using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace gradesManager
{
    internal class Lecture : ICalculable
    {
        private string name;
        private ObservableCollection<Grade> grades;
        private double weight;
        private Grade selectedItem;

        public Lecture(string name, double weight)
        {
            this.name = name;
            this.weight = weight;
            grades = new ObservableCollection<Grade>(); //new List<Grade>();
            grades.CollectionChanged += HandleChange;
        }
        
        private void HandleChange(object sender, NotifyCollectionChangedEventArgs e)
        {
            Console.WriteLine("Lecture.HandleChange");
            foreach (var x in e.NewItems)
            {
                Console.WriteLine("newitem: " +x);
                
            }

            foreach (var y in e.OldItems)
            {

                Console.WriteLine("oldItem: " +y);
            }
            if (e.Action == NotifyCollectionChangedAction.Move)
            {

                Console.WriteLine("move");
            }
        }
        public Grade SelectedItem
        {
            get { return selectedItem; }
            set { selectedItem = value; }
        }

        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
            }
        }

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

        public ObservableCollection<Grade> Grades
        {
            get { return grades; }
            set { grades = value; }
        }

        public double getValue()
        {
            return getAverage() * this.weight;
        }

        public double getWeigth()
        {
            return this.weight;
        }

        public void addGrade(Grade g)
        {
            this.grades.Add(g);
        }

        public double getAverage()
        {
            double average = 0;
            double weightTotal = 0;
            foreach (Grade grade in grades)
            {
                average += grade.getAverage() * grade.getWeigth();
                weightTotal += grade.getWeigth();
            }
            
            if (average != 0)
                return Math.Round(average / weightTotal, 2);
            else
                return 0;
        }
        
        internal void removeGrade(object selectedItem)
        {
            this.grades.Remove((Grade) selectedItem);
        }
    }
}