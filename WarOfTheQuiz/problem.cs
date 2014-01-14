using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace WarOfTheQuiz
{
    [DataContract]
    class problem
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }

        [DataMember(Name = "playerName")]
        public string PlayerName { get; set; }

        [DataMember(Name = "problema")]
        public int Problema { get; set; }

        [DataMember(Name = "detalii")]
        public bool Detalii { get; set; }
    }
}
