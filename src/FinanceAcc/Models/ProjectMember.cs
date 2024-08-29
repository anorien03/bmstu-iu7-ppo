using System;


namespace FinanceAcc.Models
{
	public class ProjectMember
	{
        public int UserId { get; set; }

        public int ProjectId { get; set; }

        public MemberStatus Status{ get; set; }


		public ProjectMember(int userId, int projectId, MemberStatus status)
		{
			UserId = userId;
			ProjectId = projectId;
			Status = status;
		}
	}
}

