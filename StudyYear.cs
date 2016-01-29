using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace gradesManager
{
    [JsonObject(MemberSerialization.OptIn)]
    internal class StudyYear : ObservableCollection<StudyYear>, ICalculable
    {
        [JsonProperty]
        private int yearBegin;
        [JsonProperty]
        private List<Module> modules;

        public StudyYear(int yearBegin)
        {
            this.yearBegin = yearBegin;
            modules = new List<Module>();
        }

        public List<Module> Modules
        {
            get { return modules; }
            set { modules = value; }
        }

        public int YearBegin 
        {
            get { return yearBegin; }
            set { this.yearBegin = value; }
        }

        public double getAverage()
        {
            double average = 0;
            foreach (Module module in modules)
            {
                average += module.getAverage();
            }

            if (average != 0)
                return average / modules.Count;
            else
                return 0;
        }

        public double getValue()
        {
            return yearBegin;
        }

        public double getWeigth()
        {
            throw new NotImplementedException();
        }

        internal void addModule(Module newModule)
        {
            this.modules.Add(newModule);
        }
    }
}