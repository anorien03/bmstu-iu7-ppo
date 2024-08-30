using System;
namespace FinanceAcc.Models.UserLevelLimits
{
	public class UserLevelLimit
	{
		private readonly Dictionary<UserLevel, Dictionary<MemberStatus, int>> limits = new Dictionary<UserLevel, Dictionary<MemberStatus, int>>
		{
			{UserLevel.Free, new Dictionary<MemberStatus, int> {{MemberStatus.Admin, 1},
																{MemberStatus.Member, 3},
                                                                {MemberStatus.Invited, -1} }},

			{UserLevel.Silver, new Dictionary<MemberStatus, int> {{MemberStatus.Admin, 3},
																{MemberStatus.Member, 6},
																{MemberStatus.Invited, -1} }},

			{UserLevel.Gold, new Dictionary<MemberStatus, int> {{MemberStatus.Admin, -1},
																{MemberStatus.Member, -1},
																{MemberStatus.Invited, -1}}}
		};


		public int getLimit(UserLevel userLevel, MemberStatus memberStatus)
		{
			return limits[userLevel][memberStatus];
		}


	}
}

