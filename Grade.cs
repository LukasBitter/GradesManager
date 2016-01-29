

using System;

namespace gradesManager
{
    internal class Grade : ICalculable
    {
        private double value;
        private double weight;
        private string comment;
        private DateTime date;

        public Grade()
        {

        }

        public Grade(double value, double weight, string comment = "")
        {
            this.value = value;
            this.weight = weight;
            this.comment = comment;
        }

        public double Value
        {
            get
            {
                return value;
            }

            set
            {
                Console.WriteLine("grade.set");
                this.value = value;
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

        public string Comment
        {
            get  { return comment; }

            set { comment = value; }
        }
        

        public double getAverage()
        {
            return value;
        }

        public double getValue()
        {
            return value;
        }

        public double getWeigth()
        {
            return weight;
        }
        public override string ToString()
        {
            return "" + this.value;
        }
    }
}