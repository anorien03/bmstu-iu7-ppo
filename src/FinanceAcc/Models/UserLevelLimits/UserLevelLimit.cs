using System;
namespace FinanceAcc.Models.UserLevelLimits
{
	public class UserLevelLimit
	{
		private static readonly Dictionary<UserLevel, Dictionary<MemberStatus, int?>> limits = new Dictionary<UserLevel, Dictionary<MemberStatus, int?>>
		{
			{UserLevel.Free, new Dictionary<MemberStatus, int?> {{MemberStatus.Admin, 1},
																{MemberStatus.Member, 3},
                                                                {MemberStatus.Invited, null} }},

			{UserLevel.Silver, new Dictionary<MemberStatus, int?> {{MemberStatus.Admin, 3},
																{MemberStatus.Member, 6},
																{MemberStatus.Invited, null} }},

			{UserLevel.Gold, new Dictionary<MemberStatus, int?> {{MemberStatus.Admin, null},
																{MemberStatus.Member, null},
																{MemberStatus.Invited, null}}}
		};


		public static int? getLimit(UserLevel userLevel, MemberStatus memberStatus)
		{
			return limits[userLevel][memberStatus];
		}


	}
}

