using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarOfTheQuiz
{
    public class LeaderboardModel
    {
        public LeaderboardModel()
        {
            this.Items = new List<leaderboardItemModel>();
        }

        public IList<leaderboardItemModel> Items { get; set; }
    }
}
