using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace WarOfTheQuiz
{
    [DataContract]
    public class Leaderboard
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }

        [DataMember(Name = "position")]
        public int Position { get; set; }

        [DataMember(Name = "playerName")]
        public string PlayerName { get; set; }

        [DataMember(Name = "score")]
        public int Score { get; set; }
    }
}
