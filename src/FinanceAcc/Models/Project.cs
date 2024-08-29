using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace FinanceAcc.Models
{
    public class Project
    {
        public int? Id { get; set; }

        public string Name { get; set; }


        public Project(string name)
        {
            Id = null;
            Name = name;
        }

        public Project(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}

