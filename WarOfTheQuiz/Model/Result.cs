using System.Runtime.Serialization;

namespace WarOfTheQuiz
{
    [DataContract]
    public class Result
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }

        [DataMember(Name = "playerName")]
        public string PlayerName { get; set; }

        [DataMember(Name = "scor")]
        public int Scor { get; set; }

        [DataMember(Name = "leaderboardUpdated")]
        public bool LeaderboardUpdated { get; set; }
    }
}
