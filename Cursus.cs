using System;
using System.Collections.Generic;

namespace gradesManager
{
    internal class Cursus : ICalculable
    {
        private int yearBegin;
        private string name;
        private List<StudyYear> studyYears;

        public Cursus(int yearBegin, string name, int nbYears)
        {
            this.yearBegin = yearBegin;
            this.name = name;
            StudyYears = new List<StudyYear>();

            for (int i = 0; i < nbYears; i++)
            {
                StudyYears.Add(new StudyYear(yearBegin+i));
            }
        }

        public Cursus()
        {
        }

        public int YearBegin
        {
            get { return yearBegin; }
            set { yearBegin = value; }
        }

        public List<StudyYear> StudyYears
        {
            get { return studyYears; }
            set { studyYears = value; }
        }

        public double getValue()
        {
            throw new NotImplementedException();
        }

        public double getWeigth()
        {
            throw new NotImplementedException();
        }

        public double getAverage()
        {
            double average = 0;
            foreach (StudyYear studyYear in StudyYears)
            {
                average += studyYear.getAverage();
            }

            if (average != 0)
                return average / StudyYears.Count;
            else
                return 0;
        }

        
    }
}