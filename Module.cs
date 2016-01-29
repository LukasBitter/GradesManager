using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace gradesManager
{
    internal class Module : ICalculable
    {
        [JsonProperty]
        private string name;
        [JsonProperty]
        private List<Lecture> lectures;

        public Module(string name)
        {
            this.Name = name;
            lectures = new List<Lecture>();
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public List<Lecture> Lectures
        {
            get { return lectures; }
            set { lectures = value; }
        }

        public double getValue()
        {
            throw new NotImplementedException();
        }

        public double getWeigth()
        {
            throw new NotImplementedException();
        }

        public void addLecture(Lecture l)
        {
            this.lectures.Add(l);
        }

        public double getAverage()
        {
            double average = 0;
            double weightTotal = 0;
            foreach (Lecture lecture in lectures)
            {
                double av = lecture.getAverage();
                if (av >= 1)
                {
                    average += av * lecture.getWeigth();
                    weightTotal += lecture.getWeigth();
                }
            }

            if (average != 0)
                return Math.Round(average / weightTotal, 2);
            else
                return 0;
        }
    }
}