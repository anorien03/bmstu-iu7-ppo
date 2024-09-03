using System;
using System.ComponentModel.DataAnnotations;

namespace FinanceAcc.Models
{
	public class Category
	{
		public int? Id { get; set; }

		public int ProjectId { get; set; }

        public string Name { get; set; }


        public Category(int id, int projectId, string name)
		{
			Id = id;
            ProjectId = projectId;
			Name = name;
		}


        public Category(int projectId, string name)
        {
            ProjectId = projectId;
            Name = name;
        }
    }
}

