using System;


namespace FinanceAcc.Models
{
	public class Record
	{
        public int Id { get; set; }

        public int UserId { get; set; }

        public int ProjectId { get; set; }

        public int CategoryId { get; set; }

        public int Sum { get; set; }

        public string? Note { get; set; }

        public DateTime Date { get; set; }


        public Record(int id, int userId, int projectId, int categoryId, int sum, string? note, DateTime date)
        {
            Id = id;
            UserId = userId;
            ProjectId = projectId;
            CategoryId = categoryId;
            Sum = sum;
            Note = note;
            Date = date;
        }


        public Record(int userId, int projectId, int categoryId, int sum, string? note, DateTime date)
        {
            UserId = userId;
            ProjectId = projectId;
            CategoryId = categoryId;
            Sum = sum;
            Note = note;
            Date = date;
        }
    }
}
