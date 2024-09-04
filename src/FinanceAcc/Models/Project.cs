using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace FinanceAcc.Models
{
    public class Project
    {
        public int Id { get; set; }

        public string Name { get; set; }


        public Project(string name)
        {
            Name = name;
        }

        public Project(int id, string name)
        {
            Id = id;
            Name = name;
        }


        public ProjectDetailed ConvertToProjectDetailed(MemberStatus userStatus)
        {
            return new ProjectDetailed(Id, Name, userStatus);
        }
    }
}


namespace FinanceAcc.Models
{
    public class ProjectDetailed
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public MemberStatus UserStatus { get; set; }


        public ProjectDetailed(int id, string name, MemberStatus userStatus)
        {
            Id = id;
            Name = name;
            UserStatus = userStatus;
        }


        public Project ConvertToProject()
        {
            return new Project(Id, Name);
        }
    }
}

