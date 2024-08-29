using System;


namespace FinanceAcc.Models
{
	public class Record
	{
        public int? Id { get; set; }

        public int UserId { get; set; }

        public int CategoryId { get; set; }

        public double Sum { get; set; }

        public string? Note { get; set; }

        public DateTime Date { get; set; }


        public Record(int id, int userId, int categoryId, double sum, string note, DateTime date)
        {
            Id = id;
            UserId = userId;
            CategoryId = categoryId;
            Sum = sum;
            Note = note;
            Date = date;
        }
    }
}

