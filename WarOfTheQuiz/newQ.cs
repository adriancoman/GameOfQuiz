using System.Runtime.Serialization;

namespace WarOfTheQuiz
{
    [DataContract]
    class newQ
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }

        [DataMember(Name = "playerName")]
        public string PlayerName { get; set; }

        [DataMember(Name = "question")]
        public string Questin { get; set; }

        [DataMember(Name = "corect")]
        public string Corect { get; set; }

        [DataMember(Name = "var1")]
        public string Var1 { get; set; }

        [DataMember(Name = "var2")]
        public string Var2 { get; set; }

        [DataMember(Name = "var3")]
        public string Var3 { get; set; }

        [DataMember(Name = "categorie")]
        public string Categorie { get; set; }
    }
}
