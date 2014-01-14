
namespace WarOfTheQuiz
{
    class question
    {
        //intrebarea
        public string quest;

        //variantele de raspuns
        public string v1;
        public string v2;
        public string v3;
        public string v4;

        public question (string _quest, string _v1, string _v2, string _v3,string _v4 )
        {
            quest = _quest;
            v1 = _v1;
            v2 = _v2;
            v3 = _v3;
            v4 = _v4;
        }
        public question()
        { }
    }
}
