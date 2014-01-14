using WarOfTheQuiz.Common;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Popups;
using NotificationsExtensions.TileContent;
using Windows.UI.Notifications;
using System.Collections.Generic;


namespace WarOfTheQuiz
{

    public sealed partial class NewGame : Page
    {

        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        DispatcherTimer qTimer;
        DispatcherTimer textTimer;
        DispatcherTimer tTimer;



        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        Random qRand = new Random();
        Random vRand = new Random();
        public NewGame()
        {
            this.InitializeComponent();

            qTimer = new DispatcherTimer();
            qTimer.Interval = TimeSpan.FromSeconds(1);
            qTimer.Tick += qTimer_Tick;
            qTimer.Start();

            textTimer = new DispatcherTimer();
            textTimer.Interval = TimeSpan.FromMilliseconds(100);
            textTimer.Tick += textTimer_Tick;
            textTimer.Start();

            tTimer = new DispatcherTimer();
            tTimer.Interval = TimeSpan.FromSeconds(1);
            tTimer.Tick += tTimer_Tick;
            tTimer.Start();

            ShuffleMe();

            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
            this.navigationHelper.SaveState += navigationHelper_SaveState;
        }
        int[] shuffleArray = new int[40];
        int z;
        private void ShuffleMe()
        {
            z = 0;
            List<int> choices = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29,30,31,32,33,34,35,36,37,38,39};
            while (choices.Count > 0)
            {
                int index = rand.Next() % choices.Count;
                int choice = choices[index];
                shuffleArray[z] = choice;
                z++;
                choices.RemoveAt(index);
            }
        }

        int textTime = 0;
        private void textTimer_Tick(object sender, object e)
        {
            textTime++;
            floatText.Margin = new Thickness(0, -150 - 10 * textTime, 0, 0);
            floatText.Opacity -= 0.1;
           // if (floatText.Opacity == 0) textTimer.Stop();
        }

        private void tTimer_Tick(object sender, object e)
        {
            tT--;
            if (tT>0)
            {
                timeTxt.Text = tT.ToString();
            }
            else
            {
                qTimer.Stop();
                tTimer.Stop();
                registerScore();
            }
        }

        private async void registerScore()
        {
            MakeTile();
            var resultsEntity = new Result { PlayerName = App.username, Scor = scor };
            await App.mobileService.GetTable<Result>().InsertAsync(resultsEntity);
            this.Frame.Navigate(typeof(topscore),resultsEntity.Id);
        }

        int qT = 11;
        int tT = 61;
        private void qTimer_Tick(object sender, object e)
        {
            qT--;
            if (qT > 0)
            {
                timpTxt.Text = qT.ToString();

            }
            else
            {
                Gresit(penalizare);
            }
        }
        private void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
        }

     
        private void navigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }
        int penalizare = 5;
        #region NavigationHelper registration

 
        string categorie;
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            categorie = e.Parameter as string;

            if (!string.IsNullOrWhiteSpace(categorie))
            {
                pageTitle.Text ="Întrebări din categoria "+ categorie;
                switch(categorie)
                {
                    case "istorie":
                    q = new question[40];               
                    GenerateIst();
                    QeneratePuzzle(0);
                    break;
                    case "geografie":
                    q = new question[40];  
                    GenerateGeo();
                    QeneratePuzzle(0);
                    break;
                    case "matematica":
                    q = new question[40];  
                    GenerateMate();
                    QeneratePuzzle(0);
                    break;
                    case "it":
                    q = new question[40];  
                    GenerateIt();
                    QeneratePuzzle(0);
                    break;
                    case "sport":
                    q = new question[40];  
                    GenerateSp();
                    QeneratePuzzle(0);
                    break;
                    case "tot":
                    q = new question[240];  
                    GenerateAll();
                    QeneratePuzzle(1);
                    break;
                }
            }
            else
            {
                pageTitle.Text = "Houston, we have a problem.";
            }
        }

        Random rand = new Random();
        int vCorect;
        int i=0;
        private void QeneratePuzzle(int all)
        {
            
            if (all == 0)
            {
                i++;
                if (i == 39)
                {
                    ShuffleMe();
                    i = 0;
                }
            }
            else
            {
                i = rand.Next(0, 240);
            }
            vCorect = rand.Next(0, 3);
            try
            {
                switch (vCorect)
                {
                    case 0:
                        intrebare.Text = q[shuffleArray[i]].quest;
                        var1.Content = q[shuffleArray[i]].v1;
                        var2.Content = q[shuffleArray[i]].v2;
                        var3.Content = q[shuffleArray[i]].v3;
                        var4.Content = q[shuffleArray[i]].v4;
                        break;
                    case 1:
                        intrebare.Text = q[shuffleArray[i]].quest;
                        var1.Content = q[shuffleArray[i]].v3;
                        var2.Content = q[shuffleArray[i]].v1;
                        var3.Content = q[shuffleArray[i]].v2;
                        var4.Content = q[shuffleArray[i]].v4;
                        break;
                    case 2:
                        intrebare.Text = q[shuffleArray[i]].quest;
                        var1.Content = q[shuffleArray[i]].v4;
                        var2.Content = q[shuffleArray[i]].v3;
                        var3.Content = q[shuffleArray[i]].v1;
                        var4.Content = q[shuffleArray[i]].v2;
                        break;
                    case 3:
                        intrebare.Text = q[shuffleArray[i]].quest;
                        var1.Content = q[shuffleArray[i]].v2;
                        var2.Content = q[shuffleArray[i]].v4;
                        var3.Content = q[shuffleArray[i]].v3;
                        var4.Content = q[shuffleArray[i]].v1;
                        break;

                }
            }
            catch (Exception ex)
            {
                //pageTitle.Text = i.ToString();
            }
        }

        question[] q;
        #region questions
        private void GenerateAll()
        {
            q[0] = new question(
                @"Care este cel mai medaliat sportiv din istoria jocurilor olimpice?",
                @"Michael Phelps",
                @"Larisa Latanina",
                @"Usain Bolt",
                @"Yohan Blake");

            q[1] = new question(
            @"Cine este singurul boxer care a castigat titlul mondial si a ramas neinvins?",
            @"Rocky Marciano",
            @"Muhammad Ali",
            @"Joe Frazier",
            @"Mike Tyson");

            q[2] = new question(
            @"Cel mai mare salariu din sport ii apartine lui:",
            @"Tiger Woods",
            @"LeBron James",
            @"Drew Brees",
            @"Cristiano Ronaldo");

            q[3] = new question(
            @"Echipa nationala cu cele mai multe cupe mondiale(fotbal) castigate este:",
            @"Brazilia",
            @"Italia",
            @"Germania",
            @"Uruguay");

            q[4] = new question(
            @"Jucatorul cu cele mai multe selectii in echipa nationala de fotbal a Romaniei este:",
            @"Dorinel Munteanu",
            @"Gheorghe Hagi",
            @"Adrian Mutu",
            @"Dan Petrescu");

            q[5] = new question(
            @"Cine este tenismenul care a castigat cele mai multe turnee de Grand Slam",
            @"Roger Federer",
            @"Pete Sampras",
            @"Rafael Nadal",
            @"Andre Agassi");

            q[6] = new question(
            @"Cine este jucatorul de baschet cu cele mai multe inele NBA din istorie?",
            @"Bill Russell",
            @"Michael Jordan",
            @"Scottie Pippen",
            @"Kobe Bryant");

            q[7] = new question(
            @"Echipa de fotbal cu cele mai multe cupe ale campionilor castigate:",
            @"Real Madrid",
            @"A.C. Milan",
            @"Bayern Munich",
            @"Barcelona");

            q[8] = new question(
            @"Cine este jucatorul cu cele mai multe baloane de aur castigate?",
            @"Lionel Messi",
            @"Cristiano Ronaldo",
            @"Michel Platini",
            @"Franz Beckenbauer");

            q[9] = new question(
            @"In ce tara se vor desfasura Jocurile Olimpice din 2016?",
            @"Brazilia",
            @"Italia",
            @"Rusia",
            @"Germania");

            q[10] = new question(
            @"Cate titluri de campioni NBA au castigat Chicago Bulls cu Michael Jordan in echipa?",
            @"6",
            @"2",
            @"5",
            @"9");

            q[11] = new question(
            @"Echipa de hochei cu cele mai multe Cupe Stanley in palmares este:",
            @"Montreal Canadiens",
            @"Toronto maple Leafs",
            @"Detroit Red Wings",
            @"Boston Bruins");

            q[12] = new question(
            @"Din cati jucatori e formata o echipa in baschet(doar cei de pe teren)?",
            @"5",
            @"6",
            @"7",
            @"4");

            q[13] = new question(
            @"Ce atlet detine recordul mondial pentru maratonul terminat in cel mai scurt timp?",
            @"Wilson Kipsang",
            @"Gabriela Szabo",
            @"Fauja Singh",
            @"James Kwambai");

            q[14] = new question(
            @"Singurul sportiv din lume care a participat la Super Bowl si World Series(baseball) este:",
            @"Deion Sanders",
            @"Payton Manning",
            @"Reggie Bush",
            @"Babe Ruth");

            q[15] = new question(
            @"Care este cea mai vizionata competitie sportiva din lume?",
            @"Cupa Mondiala",
            @"Super Bowl",
            @"Monaco Grand Prix",
            @"Cupa Mondiala la Cricket");

            q[16] = new question(
            @"Cat de lung este turul Frantei?",
            @"3600 km",
            @"1100 km",
            @"4200 km",
            @"3200");

            q[17] = new question(
            @"Cunoscut ca si Akebono,Chad Rowan din Honolulu a devenit primul campion mondial american la ce sport?",
            @"Sumo",
            @"Volei",
            @"Box",
            @"Patinaj Artistic");

            q[18] = new question(
            @"Pe 2 martie 1962 recordul pentru cele mai multe punce marcate intr-un meci de baschet(NBA) a fost realizat de:",
            @"Wilt Chamberlain",
            @"Michael Jordan",
            @"Carl Malone",
            @"Carmelo Anthony");

            q[19] = new question(
            @"Care este sportul care se desfasoara pe cea mai scurta perioada de timp de la olimpiada?",
            @"Polo",
            @"Tenis de masa",
            @"Lupte libere",
            @"Curling");
            q[20] = new question(
            @"Java a fost inventatat de către:",
            @"Sun",
            @"Oracle",
            @"Novell",
            @"Microsoft");
            q[21] = new question(
            @"Ce este limbajul C?",
            @"Limbaj de generația a treia, de nivel înalt",
            @"Limbaj de asamblare",
            @"Limbaj cod-mașină",
            @"Toate cele de mai sus");
            q[22] = new question(
            @"Primul sistem de operare UNIX, în proces de dezvoltare, a fost scris în:",
            @"Limbajul B",
            @"Limbajul C",
            @"Limbaj de asamblare",
            @"Nici unul dintre cele de mai sus");
            q[23] = new question(
            @"ASCII este acronimul de la:",
            @" American Standard Code for Information Interchange",
            @"Alliance Standard Code Interchange Integration",
            @"American Standard Code for Information Integration",
            @"American Standard Code for Implementing Information");
            q[24] = new question(
            @"Cine este considerat părintele Inteligenței Artificiale?",
            @"John McCarthy",
            @"George Boole",
            @"Alan Turing",
            @"Allen Newell");
            q[25] = new question(
            @"IP este acronimul de la:",
            @"Internet Protocol",
            @"Internet Program",
            @"Interface program",
            @"Interface protocol");
            q[26] = new question(
            @"Care dintre următoarele a fost primul procesor Intel introdus pe piață?",
            @"4004",
            @"8080",
            @"8086",
            @"3080");
            q[27] = new question(
            @"Care este dispozitivul ce convertește semnale digitale în analogice?",
            @"Modem",
            @"Network Packet",
            @"Data Packet",
            @"Niciunul dintre cele de sus");
            q[28] = new question(
            @"Care dintre urmatoarele este un exemplu de memorie nevolatilă?",
            @"ROM",
            @"VLSI",
            @"RAM",
            @"LSI");
            q[29] = new question(
            @"Ce limbaj are nevoie de un compilator pentru a compila codul sursă?",
            @"C/C++",
            @"Basic",
            @"Oracle",
            @"Python");
            q[30] = new question(
            @"Ce este MMU (Memory Manager Unit)?",
            @"Un dispozitiv hardware",
            @"O unitate de stocare",
            @"Un fișier sistem",
            @"Un director");
            q[31] = new question(
            @"Care dintre următoarele nu este un tip de hard disk?",
            @"FDD",
            @"EIDE",
            @"IDE",
            @"SCSI");
            q[32] = new question(
            @"BIOS este un acronim pentru:",
            @" basic input output system ",
            @"basic input output startup",
            @"boot initial operating startup",
            @"bootstrap initial operating system");
            q[33] = new question(
            @"Primul computer electronic digital dezvoltat de Mauchly și Eckert în jurul anului 1946 ,este?",
            @"ENIAC",
            @"EDVAC",
            @"Apple",
            @"IBM PC");
            q[34] = new question(
            @"Care dintre următoarele este cel mai puternic tip de calculator?",
            @"Super Computer",
            @"Super Micro",
            @"Super Conductor",
            @"Megaframe");
            q[35] = new question(
            @"Cine este considerat primul programator?",
            @"Ada Lovelace",
            @"Bill Gates",
            @"Alan Turing",
            @"Tim Berners-Lee");
            q[36] = new question(
            @"Când a fost trimis primul e-mail?",
            @"1971",
            @"1982",
            @"1990",
            @"1996");
            q[37] = new question(
            @"HTTP,FTP și TCP/IP sunt tipuri diferite de:",
            @"Protocoale",
            @"Website-uri",
            @"Programe software",
            @"Fișiere de sistem");
            q[38] = new question(
            @"Un cal troian poate fi clasificat ca ce tip de software?",
            @"Malware",
            @"Shareware",
            @"Scareware",
            @"Freeware");
            q[39] = new question(
            @"Ce feature a fost exclus din Windows 8?",
            @"Start Menu",
            @"Desktop",
            @"Task Bar",
            @"Notepad");
q[40]= new question(
@"Care este numele actorului principal din filmul Ghost Rider ",
@"Nicolas Cage",
@"Angelina Jolie",
@"McKenzy Frenzy",
@"Jean de la Craiova");

q[41]= new question(
@"În sistemul international puterea se masoarã în",
@"Watt",
@"Joule",
@"Metri cubi",
@"Bari");

q[42]= new question(
@"Care este numele presedintelui Kenyei(2013)",
@"Mwai Kibaki",
@"Nelson Mandela",
@"Robert O'Sullivan",
@"Joseph Kabila Kabange");

q[43]= new question(
@" Cine este Mick Jagger (n.1943)?",
@"Toate raspunsurile sunt corecte",
@"vocalistul trupei The Rolling Stones",
@"chitaristul trupei The Rolling Stones",
@"compozitor ºi actor britanic)");

q[44]= new question(
@"Cum se numeºte eroina din jocul pe computer Tomb Raider?",
@" Lara Croft",
@"Noah Taylor",
@"Angelina Jolie",
@"Iain Glen");

q[45]= new question(
@"Ce a inventat farmacistul american John S. Pemberton în anul 1886?",
@"Reþeta pentru Coca-Cola",
@"Avionul",
@"Telefonia multipla",
@"Motorul Diesel");

q[46]= new question(
@"În ce sport frebuie sã se adapteze stângacii?",
@"La polo cãlare",
@"La ºah",
@"La hockey",
@"La volei");

q[47]= new question(
@" Care actor a jucat rolul principal în Forrest Gump ?",
@"Tom Hanks",
@"Robin Williams",
@"Al Pacino",
@"Robert De Niro");

q[48]= new question(
@"Care este denumirea generalã pentru diverse snack-uri, ce sunt mâncate cu degetele dupã ce au fost trecute prin sosuri?",
@"Finger Food",
@"Pizza",
@"Fish head",
@"Puffed rice");

q[49]= new question(
@"Ce este carnea albã fiartã sau înãbuºitã în sos alb?",
@"Fricandou",
@"Fileu",
@"Chiulota",
@"Meats");

q[50]= new question(
@"Care actriþã este cunoscutã ca vânãtoarea de vampiri Buffy?",
@"Sarah Michelle Gellar",
@"Bernadette Peters",
@"Ana Fidelia Quirot",
@"Susan Lucci");

q[51]= new question(
@"In ce formaþie a cântat Sting înainte de a începe o carierã solo?",
@"The Police",
@"Scooter",
@"TNT",
@"Pet Shop Boys");

q[52]= new question(
@"Cine a jucat rolul principal în Wild Wild West?",
@"Will Smith",
@"Eddie Murphy",
@"Jon Lovitz",
@"Courteney Cox");

q[53]= new question(
@"Care modã a devenit popularã datoritã unui sport nautic?",
@"Moda Surf",
@"Moda Rafting",
@"Moda Hipism",
@"Moda Planorism");

q[54]= new question(
@"Care alergãtor de cursã lungã a obþinut nouã medalii olimpice?",
@"Finlandezul Paavo Nurmi",
@"Jean Bouin",
@"Otto Pelzer",
@"John Zander");

q[55]= new question(
@"Care englezoaicã excentricã a fãcut tot timpul valuri cu colecþiile ei de modã?",
@"Vivienne Westwood",
@"Luella Bartley",
@" Queen Elizabeth II",
@"Stella McCartney");

q[56]= new question(
@"Din câþi jucãtori este formatã o echipã de handbal?",
@"Din ºapte jucãtori",
@"Din ºase jucãtori",
@"Din cinci jucãtori",
@"Din opt jucãtori");

q[57]= new question(
@"Cine a construit în anul 1811 prima salã de gimnasticã pe terenul viran din Berlin?",
@"Friedrich Ludwig (pãrintele gimnasticii) Jahn",
@"Fabian Hambüchen",
@"Ulf Hoffmann",
@"Rudolf Körner");

q[58]= new question(
@"Care este comanda pentru poziþia de bazã în scrimã?",
@"En garde",
@"Floreta",
@"Duel",
@"Kendo");

q[59]= new question(
@"Cum se numeºte în tenis schimbul de mingi, care încheie jocul ºi decide asupra victoriei sau a înfrângerii?",
@"Minge de de meci",
@"Meci game",
@"Fin de meci",
@"Minge de oinã");
            
q[60]= new question(
@"Care este capitala Estoniei?",
@"Tallinn",
@"Vaduz",
@"Valletta",
@"Skopje");

q[61]= new question(
@"Capitala Liechtensteinului este ...?",
@"Vaduz",
@"Copenhaga",
@"Kiev",
@"Sarajevo");

q[62]= new question(
@"Care este simbolul internaþional pentru Emiratele Arabe Unite?",
@"AE",
@"EAU",
@"AE",
@"AU");

q[63]= new question(
@"Care este simbolul internaþional al Insulelor Cocos?",
@"CC",
@"CO",
@"CI",
@"IC");

q[64]= new question(
@"Care este simbolul internaþional pentru Angola?",
@"AO",
@"AN",
@"AG",
@"AL");

q[65]= new question(
@"Cel mai inalt varf al Romaniei este",
@"Moldoveanu",
@"Omu",
@"Negoiu",
@"Rodnei");

q[66]= new question(
@"Bauxitele, din care se extrage aluminiul, se gasesc in:",
@"Muntii Apuseni",
@"Muntii Banatului",
@"Muntii Fagaras",
@"Muntii Bucegi");

q[67]= new question(
@"Muntii Poiana Rusca apartin de:",
@"Carpatii Occidentali",
@"Carpatii Orientali",
@"Carpatii Meridionali",
@"Subcarpati");

q[68]= new question(
@"Climatul Romaniei este:",
@"temperat continental-moderat",
@"temperat umed",
@"temperat continental",
@"mediteranean");

q[69]= new question(
@"Cati kilometri parcurge Dunarea pe teritoriul tarii noastre?",
@"1075",
@"1050",
@"1320",
@"1015");

q[70]= new question(
@"Curentii care lipsesc Marii Negre sunt:",
@"Curentii verticali",
@"Curentii de compensatiet",
@"Curentii de suprafata",
@"Nu lipsesc");

q[71]= new question(
@"Ce animal este specific pentru padurile de foioase de la noi?",
@"lupul",
@"ursul",
@"vulpea",
@"cerbul");

q[72]= new question(
@"Care este cea mai joasa altitudine a Campiei Romane?",
@"10-20m",
@"5-10m",
@"40-50m",
@"70-80m");

q[73]= new question(
@"Raul Mures strabate podisul:",
@"Transilvaniei",
@"Mehedinti",
@"Getic",
@"Dobrogei");

q[74]= new question(
@"La recensamantul din anul 1948 populatia Romaniei era de:",
@"15,8 milioane locuitori",
@"15,3 milioane locuitori",
@"15,1 milioane locuitori",
@"14,8 milioane locuitori");

q[75]= new question(
@"Care este proportia de terenuri agricole din suprafata totala a Romaniei?",
@"62%",
@"55%",
@"38%",
@"74%");

q[76]= new question(
@"Sibiul are o veche traditie in ceea ce priveste:",
@"industria lânii",
@"industria lemnului",
@"industria hartiei ",
@"industria petrolului");

q[77]= new question(
@"Principalul bazin de extractie al huilei se afla in:",
@"Petrosani",
@"Petrila",
@"Lupeni",
@"Oltenitei");

q[78]= new question(
@"Hidrocentrala Portile de Fier II se afla la:",
@"Ostrovul Mare",
@"Anina",
@"Calafat",
@"Bazias");

q[79]= new question(
@"Rafinaria de la Brazi, in cadrul careia se fabrica cauciucul sintetic, se afla in judetul:",
@"Prahova",
@"Bacãu",
@"Valcea",
@"Arges");

            q[80] = new question(@"Cand s-a prabusit imperiul roman ?",@"476",@"452",@"560",@"281");
q[81] = new question(@"Cum a murit J.F Kennedy ?",@"asasinat",@"s-a inecat",@"cancer",@"altceva");
q[82] = new question(@"Intre ce ani a domnit Suleiman Magnificul ?",@"1520-1566",@"1501-1529",@"1555-1560",@"1581-1611");
q[83] = new question(@"Cand a cazut zidul Berlinului",@"9 noiembrie 1989",@"10 ianuarie 1988",@"22 martie 1990",@"1 aprilie 1989");
q[84] = new question(@"Cand sia declarat independenta SUA",@"1776",@"1778",@"1774",@"1770");
q[85] = new question(@"Cand a devenit crestinismul religie oficiala ?",@"394",@"320",@"416",@"432");
q[86] = new question(@"Cand a fost distrusa cetatea Troia ?",@"1180 I.H.",@"1180 D.H.",@"118 I.H.",@"1233 I.H.");
q[87] = new question(@"Numele conducator al hunilor, supranumit 'Biciul lui Dumnezeu' era:",@"Attila",@"Odoacru",@"Burebista",@"Alexandru cel Mare");
q[88] = new question(@"Cand s-a nascut Mahomed ?",@"570",@"625",@"425",@"325");
q[89] = new question(@"Capitala Imperiului Roman de Apus, a fost mutata la ?",@"Ravenna",@"Constantinopole",@"Milano",@"Venetia");
q[90] = new question(@"Unde a aparut tiparul cu presa din lemn?",@"China",@"Italia",@"Germania",@"India");
q[91] = new question(@"Unde a fost inventat praful de pusca?",@"China",@"Grecia",@"Egipt",@"Persia");
q[92] = new question(@"Chichen Itza, Tikal, Uxmal, apartineau unei mari civilizatii precolumbiene a Americii:",@"Maya",@"Azteca",@"Olmeca",@"Incasa");
q[93] = new question(@"Cand a fost descoperita America pentru prima data ?",@"Nu se stie sigur",@"Aproximativ anul 1000 D.H.",@"1492",@"1400");
q[94] = new question(@"Cuvantul 'viking' insemna in primele limbi scandinave:",@"pirat, navigator",@"fermier de coasta",@"luptator cu toporul",@"navigator al nordului");
q[95] = new question(@"Armata lui Attila a fost infranta in anul 451 de catre ?",@"armatele unite ale romanilor si vizigotilor",@"armata romana",@"armata vizigota",@"armata germana");
q[96] = new question(@"Limita maxima a expansiunii islamice in Europa Occidentala a fost:",@"Sudul Frantei",@"Nordul Spaniei",@"Sudul Germaniei",@"Sudul Germaniei");
q[97] = new question(@"De unde au preluat arabii sistemul numeric si pe cel zecimal?", @"din India", @"din China", @"din scrierile antice grecesti", @"din Persia");
q[98] = new question(@"Unde se afla Biblioteca din Alexandria?",@"Egipt",@"India",@"Macedonia",@"Grecia");
q[99] = new question(@"Când a început Revoluţia franceză?",@"1789",@"1799",@"1820",@"1756");
            q[100] = new question(
@"Ce inseamna HTTP?",
@"Hyper Text Transfer Protocol",
@"Hyper Transfer Text Protocol",
@"Hyper Terminal Tracing Program",
@"Head To This Page");

            q[101] = new question(
            @"Cand a fost lansat site-ul Wikipedia?",
            @"2001",
            @"2002",
            @"2000",
            @"2003");

            q[102] = new question(
            @"Care a fost primul slogan al companiei Apple?",
            @"Byte into an Apple",
            @"Simplicity is the Ultimate Sophistication",
            @"The Power to Be Your Best",
            @"The Computer for the rest of us");

            q[103] = new question(
            @"Sistemul de operare Android are la baza:",
            @"Linux",
            @"Windows",
            @"Symbian",
            @"Free DOS");

            q[104] = new question(
            @"Care este numele de cod al versiunii 3.0 a sistemului de operare Android?",
            @"Honeycomb",
            @"Froyo",
            @"Gingerbread",
            @"Ice Cream Sandwich");

            q[105] = new question(
            @"Primele procesoare Core i3 au fost lansate in:",
            @"Ianuarie 2010 ",
            @"Decembrie 2009",
            @"Iulie 2009",
            @"Martie 2010");

            q[106] = new question(
            @"Ce inseamna LCD?",
            @"Liquid Crystal Display",
            @"Low Cost Display",
            @"Low Class Display",
            @"Liquid Cathode Display");

            q[107] = new question(
            @"In ce an a aparut Bluetooth-ul ?",
            @"1994",
            @"1999",
            @"2002",
            @"1992");

            q[108] = new question(
            @"Cati biti are un byte?",
            @"8",
            @"10",
            @"16",
            @"32");

            q[109] = new question(
            @"In 2012, Google a achizitionat departamentul mobile al companiei:",
            @"Motorola",
            @"Nokia",
            @"BlackBerry",
            @"HTC");

            q[110] = new question(
            @"Lansat de Microsoft in 2009, Bing este:",
            @"Un motor de cautare",
            @"Un joc",
            @"Un sistem de operare",
            @"Un player audio");

            q[111] = new question(
            @"Care a fost materialul folosit pentru construirea primului mouse?",
            @"Lemnul",
            @"Fierul",
            @"Plasticul",
            @"Otelul");

            q[112] = new question(
            @"De cand produce Apple Inc. computere Mac cu procesoare Intel?",
            @"Ianuarie 2006",
            @"Aprilie 2006",
            @"Decembrie 2005",
            @"Ianuarie 2007");

            q[113] = new question(
            @"Ce culori folosesc pixelii unui display pentru a creea imagini?",
            @"Rosu, Verde si Albastru",
            @"Galben, Rosu si Albastru",
            @"Negru, Verde si Galben",
            @"Alb, Albastru si Rosu");

            q[114] = new question(
            @"Cand a fost lansata prima consola XBOX 360?",
            @"22 noiembrie 2005",
            @"15 decembrie 2005",
            @"5 ianuarie 2006",
            @"7 octombrie 2005");

            q[115] = new question(
            @"In ce an a fost inventat mouse-ul?",
            @"1963",
            @"1970",
            @"1975",
            @"1967");

            q[116] = new question(
            @"Care este capacitatea de stocare a fiecarui utilizator a serviciului Gmail?",
            @"10GB",
            @"1GB",
            @"1TB",
            @"500MB");

            q[117] = new question(
            @"Care este sistemul de numeratie folosit pentru stocarea informatiei?",
            @"Binar",
            @"Octal",
            @"Zecimal",
            @"Hexazecimal");

            q[118] = new question(
            @"Care a fost primul telefon cu Android?",
            @"HTC G1",
            @"Nexus One",
            @"Motorola Droid",
            @"Samsung Galaxy S1");

            q[119] = new question(
            @"Ce companie a construit primul procesor pentru Apple Macintosh?",
            @"Motorola",
            @"Apple",
            @"Intel",
            @"AMD");
            q[120] = new question(
@"Intr-un an, unele luni au 31 de zile, altele doar 30. Cate luni au 28 de zile?",
@"12 ",
@" 1 ",
@" 5 ",
@" 0 ");
            q[121] = new question(
            @"Ce numar urmeaza in sir? 17, 37, 47, 67, 97, ...",
            @"107",
            @"137",
            @"101",
            @"117");
            q[122] = new question(
            @"Daca din 3 chistoace faci o tigara, din 9 chistoace cate tigari faci?",
            @"4",
            @"3",
            @"1",
            @"6");
            q[123] = new question(
            @"Un tip bea ce bea pana se face ranga, iese din restaurant, coboara 3 trepte si moare. De ce?",
            @"A fost din cauza restaurantului.",
            @"A fost blestemat de o pisica neagra",
            @"Asa ceva este imposibil",
            @"Se stie ca poti muri daca bei prea mult");
            q[124] = new question(
            @"Alege numarul care completeaza sirul urmator: 169 144 121 100 81 ...",
            @"64",
            @"72",
            @"74",
            @"9");
            q[125] = new question(
            @"Andrei are 4 ani, iar sora lui are de 3 ori mai mult decat el. Ce varsta va avea sora cand Andrei va implini 12 ani?",
            @"20",
            @"18",
            @"28",
            @"32");
            q[126] = new question(
            @"Aneta este pentru Atena asa cum 39257 este pentru...",
            @"75293",
            @"75923",
            @"35297",
            @"32957");
            q[127] = new question(
            @"Dupa 15 minute de condus cu viteza constanta, un sofer observa ca a parcurs 15 km. Cu ce viteza a condus acesta?",
            @"60 Km/h",
            @"45 Km/h",
            @"30 Km/h",
            @"15 Km/h");
            q[128] = new question(
            @"Cate roti are o masina normala?",
            @"5",
            @"4",
            @"3",
            @"6");
            q[129] = new question(
            @"Care este cel de-al 10-lea termen al sirului 1,3,5,7,...?",
            @"19",
            @"9",
            @"11",
            @"21");
            q[130] = new question(
            @"Ce numar completeaza in mod logic seria: 2, 5, 12, 27, 58, ...",
            @"121",
            @"100",
            @"116",
            @"74");
            q[131] = new question(
            @"Ce numar completeaza in mod logic seria: 1, 8, 22, 43, 71, ...",
            @"106",
            @"152",
            @"89",
            @"80");
            q[132] = new question(
            @"Ce numar completeaza in mod logic seria: 0, 10, 10, 20, 30, 50, ...",
            @"80",
            @"90",
            @"300",
            @"120");
            q[133] = new question(
            @"Daca: 1 + 1 = 3 si 4 + 4 = 7 Cat face: 1 + 4?",
            @"5",
            @"7",
            @"8",
            @"3");
            q[134] = new question(
            @"Un gard drept, neinchis la capete, are 117 piloni verticali care il sustin. Cate spatii poti numara intre piloni?",
            @"116",
            @"0",
            @"119",
            @"117");
            q[135] = new question(
            @" Ce numar completeaza in mod logic seria: 3, 2, 9, 4, 81, ...",
            @"16",
            @"18",
            @"15",
            @"17");
            q[136] = new question(
            @"Ce numar completeaza in mod logic seria: 45, 15, 41, 19, 38, 22, ...",
            @"36",
            @"24",
            @"58",
            @"46");
            q[137] = new question(
            @"Ce numar completeaza in mod logic seria: 98, 88, 79, 71, ...",
            @"64",
            @"22",
            @"37",
            @"57");
            q[138] = new question(
            @"20 de pasari stau pe un copac. Toate in afara de 15 zboara. Cate pasari au ramas pe copac?",
            @"15",
            @"20",
            @"5",
            @"0");
            q[139] = new question(
            @"Esti intr-o cursa si tocmai l-ai ajuns pe cel aflat pe locul trei. Pe ce loc esti acum?",
            @"3",
            @"2",
            @"4",
            @"8");
            q[140] = new question ( 
@"Unde au avut loc jocurile olimpice din 2012?", 
@"Londra.",  
@"Atena.", 
@"Sydney.",
@"Barcelona.");
q[141] = new question (
@"Care este cel mai vizionat sport de pe lume?",
@"Fotbalul.",  
@"Tenisul.",
@"Voleyul.",
@"Hockeyul.");
q[142] = new question (
@"In perioada anilor 2000-2010 care a fost cel mai inalt jucator NBA?",
@"Yao Ming.",  
@"Shaquille O’neal",
@"James Lebron.",
@"Kobe Bryant.");
q[143] = new question (
@"Cine este castingatorul Campionatului European de Fotbal din 2008?",
@"Spania.",
@"Germania.",
@"Russia.", 
@"Olanda.");
q[144] = new question (
@"De cine au fost inventate Jocurile Olimpice? ",
@"Greci",
@"Romani.",
@" Daci.",
 @"Egipteni.");
q[145] = new question (
@"Care este cel mai scump fotbalist la ora actuala?",
@"Gareth Bale.",
@"Leo Messi.",
@"Cristiano Ronaldo.",
@"Andres Iniesta.");
q[146] = new question (
@"In ce tara a fost inventat Fotbalul?",
@"Anglia.",
@"Brazilia.",
@"Portugalia.",
@"Olanda");
q[147] = new question (
@"Care este singurul sport practicat pe Luna?",
@"Golf",
@"Fotbalul.",
@"Baseball",
@"Rugby.");
q[148] = new question (
@"Cate echipe sunt in total in NBA?",
@"30.",  
@"24.",
@"13.",
@"27");
q[149] = new question (
@"Din ce tara era prima femeie care a cucerit cel mai inalt virf al lumii?",
@"Japonia",   
@"Austria",
@"Germania.",
@"SUA.");
q[150] = new question (
@"In ce tara fiecare oras are cel putin cate un stadion de fotbal?",
@"Brazilia.",   
@"Anglia.",
@"Spania.",
@"Turcia.");
q[151] = new question (
@"In ce an sau facut primele campionate nationale de innot in SUA?",
@"1877",
@"1850.",
@"1800.",
@"1833"); 
q[152] = new question (
@"De cine a fost creat turul de ciclism din Franta?",
@"Henry Desgranges.",   
@"Maurice Garin.",
@"Bernard Hinault",
@"Fernando Tores.");
q[153] = new question (
@"In ce oras Mike Tyson a obtinut cea mai faimoasa victorie dupa un Knokout de 91 secunde in anul 1989?",
@"Atlantic City.",
@"New York.",
@"Chicago. ",
@"Las Vegas");
q[154] = new question (
@"Cine este considerat Cel mai bun sahist din istorie?",
@"Garry Kasparov.",  
@"IBM Deep Blue.",
@"Anatoly Karpov.",
@"Emanuel Lasker.");
q[155] = new question (
@"Unde a avut loc primul concurs de atletism din lume ?",
@"Bucuresti .",   
@"Atena.",
@"Barcelona.",
@"Moscova.");
q[156] = new question (
@"Cine detine cele mai multe medalii de aur de la Jocurile Olimpice ?",
@"Larissa Latynina.",    
@"Rafael Nadal.",
@"Michael Phelps",
@"Usain Bolt");
q[157] = new question (
@"Care este sportul cu cel mai mare numar de participant din lume?",
@"Pescuitul",   
@"Fotbalul.",
@"Tenisul de camp.",
@"Baseball");
q[158] = new question (
@"Ce fotbalist a dat gol cu mana la campionatul mondial de fotbal din Mexic(1986)?",
@"Diego Maradona.",  
@"Zinaedine Zidane." ,
@"Gary Lineker.",
@"David Beckham.");
q[159] = new question (
@"Cine e considerat cel mai bun fotbalist Roman?",
@"Gheorghe Hagi.",   
@"Chivu Cristian Eugen",
@"Stan Ilie",
@"Dinu Cornel.");


            q[160] = new question(@"Cine a dezvoltat limbajul de programare C ?",
                @"Dennis Ritchie",
                @"Steve Jobs",
                @"Bill gates",
                @"Armata SUA");
            q[161] = new question(@"Cum se numeste componenta hardware care se ocupa cu transmiterea imaginilor/lumiozitatii la display-ul laptop-ului?"
                , @"Invertor",
                  @"GPU",
                  @"Placa Video",
                  @"Procesorul");
            q[162] = new question(@"Ce reprezinta 'codul masina' al unui procesor ?",
                @"Modul binar de codificare a instrucțiunilor și datelor în memorie,",
                @"Modul binar de incarcare a windows-ului",
                @"Criptarea datelor",
                @"Toate variantele");
            q[163] = new question(@"Ce este The Common Language Runtime (CLR) ? ",
                @"Componenta a netframework-ului , masina virtuala",
                @"Manual de utilizare a windows-ului",
                @"Limbaj de programare secret",
                @"Componenta hardware");
            q[164] = new question(@"Cate blocuri de numere are o adresa IP ?",
                @"4",
                @"3",
                @"2",
                @"1");
            q[165] = new question(@"Care dintre urmatoarele nu este un Hard Disk ?",
                @"IDS",
                @"IDE",
                @"SATA",
                @"SCSI");
            q[166] = new question(@"Cine este considerat ca fiind primul programator ?",
                @"Ada Lovelace",
                @"Alan Turing",
                @"Bill gates",
                @"Tim Berners-Lee");
            q[167] = new question(@"JavaScript este...",
                @"Limbaj client side script. ",
                @"Limbaj server side script",
                @"un virus",
                @"un alt nume pentru Java");
            q[168] = new question(@"Ce este phishing?",
                @"O schema pentu furarea identitatii",
                @"Virus ",
                @"Site corupt",
                @"Spam");
            q[169] = new question(@"Unde este stocat BIOS-ul",
                @"Cip de memorie flash",
                @"Discheta",
                @"Hard Disk",
                @"Stick USB");
            q[170] = new question(@"Care este minimul necesar de memorie pentru a rula windows XP",
                @"64MB",
                @"32MB",
                @"128MB",
                @"256MB");
            q[171] = new question(@"Care era logo-ul original al Apple?",
                @"Isaac Newton sub un copac cu mere",
                @"Mar rosu",
                @"Marul muscat",
                @"Banana");
            q[172] = new question(@"Ce inseamna termenul de cookie?",
                @"Fisier cu informatii internet",
                @"Software cookie",
                @"WebSite",
                @"Fisier pentru hackeri");
            q[173] = new question(@"Un DNS traduce numele unui domeniu in?",
                @"IP",
                @"HEX",
                @"URL",
                @"Binar");
            q[174] = new question(@"In ce an a fost trimis primul email?",
                @"1971",
                @"1969",
                @"1974",
                @"1963");
            q[175] = new question(@"Care protocol este folosit pentru a trimite email?",
                @"SMTP",
                @"SSH",
                @"FTP",
                @"POP3");
            q[176] = new question(@"Care este rata de transfer pentru USB 2.0 ?",
                @"480Mbps",
                @"256Mbps",
                @"64Mbps",
                @"12Mbps");
            q[177] = new question(@"Ce este Apache?",
                @"WebServer",
                @"Sistem de operare",
                @"Browser",
                @"PC");
            q[178] = new question(@"Care este rata maxima de viteza pentru wieless 802.11a standard",
                @"54Mbps",
                @"24Mbps",
                @"10Mbps",
                @"100Mbps");
            q[179] = new question(@"In ce an a fost lansat Windows XP",
                @"2001",
                @"2002",
                @"2003",
                @"2004");



            q[180] = new question(
@"Cate procente de Raze Ultraviolete reflecta zapada?",
@"Peste 90%",
@"30%",
@"Mai putin de 50%",
@"Nu reflecta razele UV");

            q[181] = new question(
            @"Unde este construit anual cel mai mare palat de gheata din lume?",
            @"Finlanda",
            @"Rusia",
            @"Norvegia",
            @"Ungaria");

            q[182] = new question(
            @"În ce an a fost inregistrat cel mai puternic cutremur din Romania?",
            @"1940",
            @"1984",
            @"2000",
            @"2013");

            q[183] = new question(
            @"Ce magnitudine a avut cel mai puternic cutremur inregistrat in Romania?",
            @"7.3 grade",
            @"8.4 grade",
            @"4.0 grade",
            @"7.2 grade");

            q[184] = new question(
            @"Unde a avut loc cel mai puternic cutremur de pamant inregistrat vreodata?",
            @"Chile",
            @"Madagascar",
            @"Japonia",
            @"Romania");

            q[185] = new question(
            @"Care a foat magnitudinea celui mai puternic cutremur de pamant inregistrat vreodata?",
            @"9.5 grade",
            @"9.8 grade",
            @"10 grade",
            @"8.4 grade");

            q[186] = new question(
            @"Care este lungimea totala aproximativa a Dunarii?",
            @"2850 km.",
            @"3290 km.",
            @"1200 km.",
            @"150 m.");

            q[187] = new question(
            @"Care este cel mai lung lant muntos din Europa?",
            @"Muntii Carpati",
            @"Muntii Alpi",
            @"Muntii Ural",
            @"Muntii Fericirii");

            q[188] = new question(
            @"Care este cea mai mare structura acvatica anoxica din lume?",
            @"Marea Neagra",
            @"Marea Rosie",
            @"Lacul Baical",
            @"Raul Dambovita");

            q[189] = new question(
            @"Ce suprafata a planetei Terra o acopera apa?",
            @"71%",
            @"80%",
            @"30%",
            @"100%");

            q[190] = new question(
            @"Care este cel mai comun nume de pe glob?",
            @"Muhammad",
            @"Andrei",
            @"Ion",
            @"John");

            q[191] = new question(
            @"Care este tara cu cea mai mare populatie?",
            @"China",
            @"Vatican",
            @"Rusia",
            @"Canada");

            q[192] = new question(
            @"Care este cel mai mare continent?",
            @"Asia",
            @"America de Nord",
            @"America de Sud",
            @"Europa");

            q[193] = new question(
            @"Ce numar de locuitori are Antarctica?",
            @"Aproximativ 1000",
            @"Mai putin de 20 000",
            @"3000",
            @"14 000");

            q[194] = new question(
            @"Unde se pot gasi cele mai multe specii de reptile din lume?",
            @"Australia",
            @"Madagascar",
            @"Austria",
            @"Brazilia");

            q[195] = new question(
            @"Cate specii de reptile se gasesc in Australia?",
            @"755",
            @"320",
            @"400",
            @"Acolo nu exista reptile.");

            q[196] = new question(
            @"Care este cel mai mare parc national din lume?",
            @"Yellowstone",
            @"Gradina Botanica",
            @"NY Central Park",
            @"Grand Canyon");

            q[197] = new question(
            @"Cate perocente din populaţia lumii trăieşte în emisfera nordică?",
            @"90%",
            @"75%",
            @"30%",
            @"53%");

            q[198] = new question(
            @"Care ocean conţine peste jumătate din întreaga apă sărată din lume?",
            @"Oceanul Pacific",
            @"Oceanul Atlantic",
            @"Oceanul Indian",
            @"Oceanul Antarctic");

            q[199] = new question(
            @"Care tara gazduieste cea mai mare intindere desertica din lume?",
            @"Libia",
            @"Egipt",
            @"S.U.A.",
            @"Australia");

            q[200] = new question(
            @"Cate lacuri gazduieste,aproximativ, Canada ?",
            @"3 milioane",
            @"2 mii",
            @"30 mii",
            @"1 milion");

            q[201] = new question("Carui politician roman apartine fraza <<Veni, vidi, vici>>?",
@"Cezar",
@"Traian",
@"Balbinus",
@"Augustus");

q[202] = new question("Care a fost unltimul tar al Imperiului Rus?",
@"Nicolae II",
@"Boris Godunov",
@"Alexandru II",
@"Ivan IV");

q[203] = new question ("In ce an s-a terminat razboiul din Vietnam?",
@"1975",
@"1973",
@"1980",
@"1978");

q[204] = new question ("Cum se numeste curentul crestin creat de reformatorul Martin Luther in secolul XVI?",
@"Protestantism",
@"Evangelism",
@"Baptism",
@"Catolicism");

q[205] = new question ("In ce an a incetat sa existe Imperiul Roman de Vest?",
@"476",
@"378",
@"512",
@"788");

q[206] = new question("Care a fost ultima batalie din cariera lui Napoleon Bonaparte?",
@"Batalia de la Waterloo",
@"Batalia de la Austerlitz",
@"Batalia de la Jenna",
@"Batalia de la Borodino");

q[207] = new question ("Cine a fost succesorul presedintelui american J.F. Kennedy?",
@"Lyndon Johnson",
@"Andrew Jackson",
@"Jimmy Carter",
@"Dwight Eisenhower");

q[208] = new question ("Anul 1903 a fost marcat de aparitia unui aparat care a schimbat cursul istoriei. Cum se numeste aceasta inventie?",
@"Avion",
@"Camera de filmat",
@"Telefon",
@"iPad");

q[209] = new question ("Cui apartinea imperiul care se intindea pe teritoriul celor trei state actuale Germania, Franta, Italia in secolele VIII-XI?",
@"Carol cel Mare",
@"Papa Pius II",
@"William Cuceritorul",
@"Pepin cel Scurt");

q[210] = new question ("Ideologia pusa in practica de Marea Britanie, Franta, Olanda si Spania in secolele XVI-XIX ca urmare a descoperirilor geografice se numeste...?",
@"Colonialism",
@"Emigrare",
@"Embargo",
@"Autocratie");

q[211] = new question ("Intrebare Bonus: Care razboi inca nu s-a sfarsit?",
@"Al Doilea Razboi Mondial",
@"Razboiul din Vietnam",
@"Razboiul din Coreea",
@"Razboiul Rece");

q[212] = new question ("Care este cel mai cunoscut conducator si cuceritor din istoria Asiei?",
@"Ghenghis Khan",
@"Quin Zhiayu",
@"Kim Ir Sen",
@"Mahatma Gandhi");

q[213] = new question ("Care din liderii politici enumerati nu e considerat dictator?",
@"Nelson Mandela",
@"Pol Pot",
@"Iosif Stalin",
@"Adolf Hitler");

q[214] = new question("In ce an a ajuns primul om, Yuri Gagarin, in spatiu?",
@"1959",
@"1960",
@"1969",
@"1957");


q[215] =new question ("Cati ani a durat Razboiul de 100 de Ani?",
@"116",
@"100",
@"99",
@"108");

q[216] =new question ("Ce stat avea cea mai mare putere si influenta in Europa secolelor XII-XVII?",
@"Vaticanul",
@"Italia",
@"Franta",
@"Spania");

q[217] =new question ("Ce erau hughenotii?",
@"Calvinisti francezi",
@"Mercenari irlandezi",
@"Calugari nomazi",
@"Luterani austrieci");

q[218] =new question ("Cui apartin cuvintele <<E pur si muove>> (Si totusi se misca)?",
@"Galileo Galilei",
@"Albert Einstein",
@"Leonardo Da Vinci",
@"Isaac Newton");

q[219] = new question("In ce an a cazut Zidul Berlinului?",
@"1989",
@"1990",
@"1988",
@"1991");

            q[220]= new question(
@"Valorea numarului imaginar i la patrat este:",
@"-1",
@"1",
@"-i",
@"i");
q[221]= new question(
@"Inversa unei matrice se determina cu ajutorul urmatoarei formule:",
@"1/det(matrice)*matricea adjuncta",
@"matricea adjuncta*matricea unitate",
@"det(matrice)*matricea unitate",
@"det(matricea adjuncta)*matrice");
q[222]= new question(
@"Determinantul unei matrice se calculeza cand:",
@"Nr. de coloane = Nr. de linii",
@"Nr. de coloane > Nr. de linii",
@"Nr. de linii > Nr.de coloane",
@"Nu se calculeaza");
q[223]= new question(
@"Logaritmii in baza zece se numesc:",
@"Logaritmi zecimali",
@"Logaritmi uzuali",
@"Logaritmi naturali",
@"Logaritmi binari");
q[224]= new question(
@"Criteriul Raabe-Duhamel enunta ca o limita este convergenta cand:",
@"a>1",
@"a<1",
@"a=0",
@"a=1");
q[225]= new question(
@"Teorema lui Abel: Seria Suma din AnBn, cand n>=o este convergenta daca sirul Bn este:",
@"monoton si marginit",
@"monoton si nemarginit",
@"monoton",
@"nul");
q[226]= new question(
@"Functii: Limita din 1/infint este:",
@"0",
@"infinit",
@"caz de nedeterminare",
@"1");
q[227]= new question(
@"Functii, derivate: (arctg x)derivat are ca solutie:",
@"1/1+x2",
@"1/radical(1-x)",
@"sin x",
@"e");
q[228]= new question(
@"Functii, derivate: (sin x)derivat are ca solutie:",
@"cos x",
@"sin x",
@"-1",
@"1/cos x");
q[229]= new question(
@"Analiza: Asimptotele reprezinta dreapta ... fata de care graficul unei functii se apropie oricat de mult. :",
@"Toate variantele de raspuns",
@"Dreapta verticala ",
@"Dreapta orizontala",
@"Dreapta oblica");
q[230]= new question(
@"Algebra: Media aritmetica este reprezentata de formula:",
@"(A1+A2+...+An)/n",
@"(A1*A2*...*An)/n",
@"(A1+A2+...+An)/n*n",
@"(A1+A2+...+An)/2");
q[231]= new question(
@"Algebra: Media geometrica este reprezentata de formula:",
@"Radical din (a*b)",
@"Radical din (a+b)",
@"Radical din (a+b)(a-b)",
@"Radical din (a*b)/2");
q[232]= new question(
@"Determinantul unei matrice se poate calcula folosind:",
@"Regula triunghiului/lui Sarrus ",
@"Regula lui Cramer",
@"Teorema lui Rouche",
@"Transpusa matricei");
q[233]= new question(
@"Limite remarcabile: limita cand x>0 din sin x/x are ca rezultat:",
@"1",
@"-1",
@"infinit",
@"-infinit");
q[234]= new question(
@"Limite remarcabile: limita cand x>0 din ln(1+x)/x are ca rezultat:",
@"1",
@"2",
@"-1",
@"0");
q[235]= new question(
@"Limite remarcabile: limita cand x>0 din arcsin x/x are ca rezultat:",
@"1",
@"0",
@"-1",
@"r");
q[236]= new question(
@"Siruri: Orice sir crescator si nemarginit are limita:",
@"+infinit",
@"-infint",
@"zero",
@"finita");
q[237]= new question(
@"Integrala din 1/x dx are ca solutie:",
@"ln|x|",
@"x/ln|x|",
@"sin x",
@"-cos x");
q[238]= new question(
@"Integrala din sin x dx are ca solutie:",
@"-cos x",
@"cos x",
@"tg x",
@"-tg x");
q[239]= new question(
@"Integrala din tg x dx are ca solutie:",
@"- ln|cos x|",
@"- ln|sin x|",
@"-cos x",
@"ln|cos x|");

        }

        private void GenerateSp()
        {
            q[0]=new question(
@"Care este cel mai medaliat sportiv din istoria jocurilor olimpice?",
@"Michael Phelps",
@"Larisa Latanina",
@"Usain Bolt",
@"Yohan Blake");

q[1]=new question(
@"Cine este singurul boxer care a castigat titlul mondial si a ramas neinvins?",
@"Rocky Marciano",
@"Muhammad Ali",
@"Joe Frazier",
@"Mike Tyson");

q[2]=new question(
@"Cel mai mare salariu din sport ii apartine lui:",
@"Tiger Woods",
@"LeBron James",
@"Drew Brees",
@"Cristiano Ronaldo");

q[3]=new question(
@"Echipa nationala cu cele mai multe cupe mondiale(fotbal) castigate este:",
@"Brazilia",
@"Italia",
@"Germania",
@"Uruguay");

q[4]=new question(
@"Jucatorul cu cele mai multe selectii in echipa nationala de fotbal a Romaniei este:",
@"Dorinel Munteanu",
@"Gheorghe Hagi",
@"Adrian Mutu",
@"Dan Petrescu");

q[5]=new question(
@"Cine este tenismenul care a castigat cele mai multe turnee de Grand Slam",
@"Roger Federer",
@"Pete Sampras",
@"Rafael Nadal",
@"Andre Agassi");

q[6]=new question(
@"Cine este jucatorul de baschet cu cele mai multe inele NBA din istorie?",
@"Bill Russell",
@"Michael Jordan",
@"Scottie Pippen",
@"Kobe Bryant");

q[7]=new question(
@"Echipa de fotbal cu cele mai multe cupe ale campionilor castigate:",
@"Real Madrid",
@"A.C. Milan",
@"Bayern Munich",
@"Barcelona");

q[8]=new question(
@"Cine este jucatorul cu cele mai multe baloane de aur castigate?",
@"Lionel Messi",
@"Cristiano Ronaldo",
@"Michel Platini",
@"Franz Beckenbauer");

q[9]=new question(
@"In ce tara se vor desfasura Jocurile Olimpice din 2016?",
@"Brazilia",
@"Italia",
@"Rusia",
@"Germania");

q[10]=new question(
@"Cate titluri de campioni NBA au castigat Chicago Bulls cu Michael Jordan in echipa?",
@"6",
@"2",
@"5",
@"9");

q[11]=new question(
@"Echipa de hochei cu cele mai multe Cupe Stanley in palmares este:",
@"Montreal Canadiens",
@"Toronto maple Leafs",
@"Detroit Red Wings",
@"Boston Bruins");

q[12]=new question(
@"Din cati jucatori e formata o echipa in baschet(doar cei de pe teren)?",
@"5",
@"6",
@"7",
@"4");

q[13]=new question(
@"Ce atlet detine recordul mondial pentru maratonul terminat in cel mai scurt timp?",
@"Wilson Kipsang",
@"Gabriela Szabo",
@"Fauja Singh",
@"James Kwambai");

q[14]=new question(
@"Singurul sportiv din lume care a participat la Super Bowl si World Series(baseball) este:",
@"Deion Sanders",
@"Payton Manning",
@"Reggie Bush",
@"Babe Ruth");

q[15]=new question(
@"Care este cea mai vizionata competitie sportiva din lume?",
@"Cupa Mondiala",
@"Super Bowl",
@"Monaco Grand Prix",
@"Cupa Mondiala la Cricket");

q[16]=new question(
@"Cat de lung este turul Frantei?",
@"3600 km",
@"1100 km",
@"4200 km",
@"3200");

q[17]=new question(
@"Cunoscut ca si Akebono,Chad Rowan din Honolulu a devenit primul campion mondial american la ce sport?",
@"Sumo",
@"Volei",
@"Box",
@"Patinaj Artistic");

q[18]=new question(
@"Pe 2 martie 1962 recordul pentru cele mai multe punce marcate intr-un meci de baschet(NBA) a fost realizat de:",
@"Wilt Chamberlain",
@"Michael Jordan",
@"Carl Malone",
@"Carmelo Anthony");

q[19]=new question(
@"Care este sportul care se desfasoara pe cea mai scurta perioada de timp de la olimpiada?",
@"Polo",
@"Tenis de masa",
@"Lupte libere",
@"Curling");

q[21] = new question ( 
@"Unde au avut loc jocurile olimpice din 2012?", 
@"Londra.",  
@"Atena.", 
@"Sydney.",
@"Barcelona.");
q[22] = new question (
@"Care este cel mai vizionat sport de pe lume?",
@"Fotbalul.",  
@"Tenisul.",
@"Voleyul.",
@"Hockeyul.");
q[23] = new question (
@"In perioada anilor 2000-2010 care a fost cel mai inalt jucator NBA?",
@"Yao Ming.",  
@"Shaquille O’neal",
@"James Lebron.",
@"Kobe Bryant.");
q[24] = new question (
@"Cine este castingatorul Campionatului European de Fotbal din 2008?",
@"Spania.",
@"Germania.",
@"Russia.", 
@"Olanda.");
q[25] = new question (
@"De cine au fost inventate Jocurile Olimpice? ",
@"Greci",
@"Romani.",
@" Daci.",
 @"Egipteni.");
q[26] = new question (
@"Care este cel mai scump fotbalist la ora actuala?",
@"Gareth Bale.",
@"Leo Messi.",
@"Cristiano Ronaldo.",
@"Andres Iniesta.");
q[27] = new question (
@"In ce tara a fost inventat Fotbalul?",
@"Anglia.",
@"Brazilia.",
@"Portugalia.",
@"Olanda");
q[28] = new question (
@"Care este singurul sport practicat pe Luna?",
@"Golf",
@"Fotbalul.",
@"Baseball",
@"Rugby.");
q[29] = new question (
@"Cate echipe sunt in total in NBA?",
@"30.",  
@"24.",
@"13.",
@"27");
q[30] = new question (
@"Din ce tara era prima femeie care a cucerit cel mai inalt virf al lumii?",
@"Japonia",   
@"Austria",
@"Germania.",
@"SUA.");
q[30] = new question (
@"In ce tara fiecare oras are cel putin cate un stadion de fotbal?",
@"Brazilia.",   
@"Anglia.",
@"Spania.",
@"Turcia.");
q[31] = new question (
@"In ce an sau facut primele campionate nationale de innot in SUA?",
@"1877",
@"1850.",
@"1800.",
@"1833"); 
q[32] = new question (
@"De cine a fost creat turul de ciclism din Franta?",
@"Henry Desgranges.",   
@"Maurice Garin.",
@"Bernard Hinault",
@"Fernando Tores.");
q[33] = new question (
@"In ce oras Mike Tyson a obtinut cea mai faimoasa victorie dupa un Knokout de 91 secunde in anul 1989?",
@"Atlantic City.",  
@"New York.",
@"Chicago. ",
@"Las Vegas");
q[34] = new question (
@"Cine este considerat Cel mai bun sahist din istorie?",
@"Garry Kasparov.",  
@"IBM Deep Blue.",
@"Anatoly Karpov.",
@"Emanuel Lasker.");
q[35] = new question (
@"Unde a avut loc primul concurs de atletism din lume ?",
@"Bucuresti .",   
@"Atena.",
@"Barcelona.",
@"Moscova.");
q[36] = new question (
@"Cine detine cele mai multe medalii de aur de la Jocurile Olimpice ?",
@"Larissa Latynina.",    
@"Rafael Nadal.",
@"Michael Phelps",
@"Usain Bolt");
q[37] = new question (
@"Care este sportul cu cel mai mare numar de participant din lume?",
@"Pescuitul",   
@"Fotbalul.",
@"Tenisul de camp.",
@"Baseball");
q[38] = new question (
@"Ce fotbalist a dat gol cu mana la campionatul mondial de fotbal din Mexic(1986)?",
@"Diego Maradona.",  
@"Zinaedine Zidane." ,
@"Gary Lineker.",
@"David Beckham.");
q[39] = new question (
@"Cine e considerat cel mai bun fotbalist Roman?",
@"Gheorghe Hagi.",   
@"Chivu Cristian Eugen",
@"Stan Ilie",
@"Dinu Cornel.");

        }
        //done
        private void GenerateIt()
        {
            q[0]=new question(
            @"Ce inseamna HTTP?",
            @"Hyper Text Transfer Protocol",
            @"Hyper Transfer Text Protocol",
            @"Hyper Terminal Tracing Program",
            @"Head To This Page");

            q[1]=new question(
            @"Cand a fost lansat site-ul Wikipedia?",
            @"2001",
            @"2002",
            @"2000",
            @"2003");

            q[2]=new question(
            @"Care a fost primul slogan al companiei Apple?",
            @"Byte into an Apple",
            @"Simplicity is the Ultimate Sophistication",
            @"The Power to Be Your Best",
            @"The Computer for the rest of us");

            q[3]=new question(
            @"Sistemul de operare Android are la baza:",
            @"Linux",
            @"Windows",
            @"Symbian",
            @"Free DOS");

            q[4]=new question(
            @"Care este numele de cod al versiunii 3.0 a sistemului de operare Android?",
            @"Honeycomb",
            @"Froyo",
            @"Gingerbread",
            @"Ice Cream Sandwich");

            q[5]=new question(
            @"Primele procesoare Core i3 au fost lansate in:",
            @"Ianuarie 2010 ",
            @"Decembrie 2009",
            @"Iulie 2009",
            @"Martie 2010");

            q[6]=new question(
            @"Ce inseamna LCD?",
            @"Liquid Crystal Display",
            @"Low Cost Display",
            @"Low Class Display",
            @"Liquid Cathode Display");

            q[7]=new question(
            @"In ce an a aparut Bluetooth-ul ?",
            @"1994",
            @"1999",
            @"2002",
            @"1992");

            q[8]=new question(
            @"Cati biti are un byte?",
            @"8",
            @"10",
            @"16",
            @"32");

            q[9]=new question(
            @"In 2012, Google a achizitionat departamentul mobile al companiei:",
            @"Motorola",
            @"Nokia",
            @"BlackBerry",
            @"HTC");

            q[10]=new question(
            @"Lansat de Microsoft in 2009, Bing este:",
            @"Un motor de cautare",
            @"Un joc",
            @"Un sistem de operare",
            @"Un player audio");

            q[11]=new question(
            @"Care a fost materialul folosit pentru construirea primului mouse?",
            @"Lemnul",
            @"Fierul",
            @"Plasticul",
            @"Otelul");

            q[12]=new question(
            @"De cand produce Apple Inc. computere Mac cu procesoare Intel?",
            @"Ianuarie 2006",
            @"Aprilie 2006",
            @"Decembrie 2005",
            @"Ianuarie 2007");

            q[13]=new question(
            @"Ce culori folosesc pixelii unui display pentru a creea imagini?",
            @"Rosu, Verde si Albastru",
            @"Galben, Rosu si Albastru",
            @"Negru, Verde si Galben",
            @"Alb, Albastru si Rosu");

            q[14]=new question(
            @"Cand a fost lansata prima consola XBOX 360?",
            @"22 noiembrie 2005",
            @"15 decembrie 2005",
            @"5 ianuarie 2006",
            @"7 octombrie 2005");

            q[15]=new question(
            @"In ce an a fost inventat mouse-ul?",
            @"1963",
            @"1970",
            @"1975",
            @"1967");

            q[16]=new question(
            @"Care este capacitatea de stocare a fiecarui utilizator a serviciului Gmail?",
            @"10GB",
            @"1GB",
            @"1TB",
            @"500MB");

            q[17]=new question(
            @"Care este sistemul de numeratie folosit pentru stocarea informatiei?",
            @"Binar",
            @"Octal",
            @"Zecimal",
            @"Hexazecimal");

            q[18]=new question(
            @"Care a fost primul telefon cu Android?",
            @"HTC G1",
            @"Nexus One",
            @"Motorola Droid",
            @"Samsung Galaxy S1");

            q[19]=new question(
            @"Ce companie a construit primul procesor pentru Apple Macintosh?",
            @"Motorola",
            @"Apple",
            @"Intel",
            @"AMD");

                        q[20]= new question(
            @"Java a fost inventatat de către:",
            @"Sun",
            @"Oracle",
            @"Novell",
            @"Microsoft");
            q[21]= new question(
            @"Ce este limbajul C?",
            @"Limbaj de generația a treia, de nivel înalt",
            @"Limbaj de asamblare",
            @"Limbaj cod-mașină",
            @"Toate cele de mai sus");
            q[22]= new question(
            @"Primul sistem de operare UNIX, în proces de dezvoltare, a fost scris în:",
            @"Limbajul B",
            @"Limbajul C",
            @"Limbaj de asamblare",
            @"Nici unul dintre cele de mai sus");
            q[23]= new question(
            @"ASCII este acronimul de la:",
            @" American Standard Code for Information Interchange",
            @"Alliance Standard Code Interchange Integration",
            @"American Standard Code for Information Integration",
            @"American Standard Code for Implementing Information");
            q[24]= new question(
            @"Cine este considerat părintele Inteligenței Artificiale?",
            @"John McCarthy",
            @"George Boole",
            @"Alan Turing",
            @"Allen Newell");
            q[25]= new question(
            @"IP este acronimul de la:",
            @"Internet Protocol",
            @"Internet Program",
            @"Interface program",
            @"Interface protocol");
            q[26]= new question(
            @"Care dintre următoarele a fost primul procesor Intel introdus pe piață?",
            @"4004",
            @"8080",
            @"8086",
            @"3080");
            q[27]= new question(
            @"Care este dispozitivul ce convertește semnale digitale în analogice?",
            @"Modem",
            @"Network Packet",
            @"Data Packet",
            @"Niciunul dintre cele de sus");
            q[28]= new question(
            @"Care dintre urmatoarele este un exemplu de memorie nevolatilă?",
            @"ROM",
            @"VLSI",
            @"RAM",
            @"LSI");
            q[29]= new question(
            @"Ce limbaj are nevoie de un compilator pentru a compila codul sursă?",
            @"C/C++",
            @"Basic",
            @"Oracle",
            @"Python");
            q[30]= new question(
            @"Ce este MMU (Memory Manager Unit)?",
            @"Un dispozitiv hardware",
            @"O unitate de stocare",
            @"Un fișier sistem",
            @"Un director");
            q[31]= new question(
            @"Care dintre următoarele nu este un tip de hard disk?",
            @"FDD",
            @"EIDE",
            @"IDE",
            @"SCSI");
            q[32]= new question(
            @"BIOS este un acronim pentru:",
            @" basic input output system ",
            @"basic input output startup",
            @"boot initial operating startup",
            @"bootstrap initial operating system");
            q[33]= new question(
            @"Primul computer electronic digital dezvoltat de Mauchly și Eckert în jurul anului 1946 ,este?",
            @"ENIAC",
            @"EDVAC",
            @"Apple",
            @"IBM PC");
            q[34]= new question(
            @"Care dintre următoarele este cel mai puternic tip de calculator?",
            @"Super Computer",
            @"Super Micro",
            @"Super Conductor",
            @"Megaframe");
            q[35]= new question(
            @"Cine este considerat primul programator?",
            @"Ada Lovelace",
            @"Bill Gates",
            @"Alan Turing",
            @"Tim Berners-Lee");
            q[36]= new question(
            @"Când a fost trimis primul e-mail?",
            @"1971",
            @"1982",
            @"1990",
            @"1996");
            q[37]= new question(
            @"HTTP,FTP și TCP/IP sunt tipuri diferite de:",
            @"Protocoale",
            @"Website-uri",
            @"Programe software",
            @"Fișiere de sistem");
            q[38]= new question(
            @"Un cal troian poate fi clasificat ca ce tip de software?",
            @"Malware",
            @"Shareware",
            @"Scareware",
            @"Freeware");
            q[39]= new question(
            @"Ce feature a fost exclus din Windows 8?",
            @"Start Menu",
            @"Desktop",
            @"Task Bar",
            @"Notepad");

            // q[40] = new question(@"Cine a dezvoltat limbajul de programare C ?",
            //    @"Dennis Ritchie",
            //    @"Steve Jobs",
            //    @"Bill gates",
            //    @"Armata SUA");
            //q[41] = new question(@"Cum se numeste componenta hardware care se ocupa cu transmiterea imaginilor/lumiozitatii la display-ul laptop-ului?"
            //    , @"Invertor",
            //      @"GPU",
            //      @"Placa Video",
            //      @"Procesorul");
            //q[42] = new question(@"Ce reprezinta 'codul masina' al unui procesor ?",
            //    @"Modul binar de codificare a instrucțiunilor și datelor în memorie,",
            //    @"Modul binar de incarcare a windows-ului",
            //    @"Criptarea datelor",
            //    @"Toate variantele");
            //q[43] = new question(@"Ce este The Common Language Runtime (CLR) ? ",
            //    @"Componenta a netframework-ului , masina virtuala",
            //    @"Manual de utilizare a windows-ului",
            //    @"Limbaj de programare secret",
            //    @"Componenta hardware");
            //q[44] = new question(@"Cate blocuri de numere are o adresa IP ?",
            //    @"4",
            //    @"3",
            //    @"2",
            //    @"1");
            //q[45] = new question(@"Care dintre urmatoarele nu este un Hard Disk ?",
            //    @"IDS",
            //    @"IDE",
            //    @"SATA",
            //    @"SCSI");
            //q[46] = new question(@"Cine este considerat ca fiind primul programator ?",
            //    @"Ada Lovelace",
            //    @"Alan Turing",
            //    @"Bill gates",
            //    @"Tim Berners-Lee");
            //q[47] = new question(@"JavaScript este...",
            //    @"Limbaj client side script. ",
            //    @"Limbaj server side script",
            //    @"un virus",
            //    @"un alt nume pentru Java");
            //q[48] = new question(@"Ce este phishing?",
            //    @"O schema pentu furarea identitatii",
            //    @"Virus ",
            //    @"Site corupt",
            //    @"Spam");
            //q[49] = new question(@"Unde este stocat BIOS-ul",
            //    @"Cip de memorie flash",
            //    @"Discheta",
            //    @"Hard Disk",
            //    @"Stick USB");
            //q[50] = new question(@"Care este minimul necesar de memorie pentru a rula windows XP",
            //    @"64MB",
            //    @"32MB",
            //    @"128MB",
            //    @"256MB");
            //q[51] = new question(@"Care era logo-ul original al Apple?",
            //    @"Isaac Newton sub un copac cu mere",
            //    @"Mar rosu",
            //    @"Marul muscat",
            //    @"Banana");
            //q[52] = new question(@"Ce inseamna termenul de cookie?",
            //    @"Fisier cu informatii internet",
            //    @"Software cookie",
            //    @"WebSite",
            //    @"Fisier pentru hackeri");
            //q[53] = new question(@"Un DNS traduce numele unui domeniu in?",
            //    @"IP",
            //    @"HEX",
            //    @"URL",
            //    @"Binar");
            //q[54] = new question(@"In ce an a fost trimis primul email?",
            //    @"1971",
            //    @"1969",
            //    @"1974",
            //    @"1963");
            //q[55] = new question(@"Care protocol este folosit pentru a trimite email?",
            //    @"SMTP",
            //    @"SSH",
            //    @"FTP",
            //    @"POP3");
            //q[56] = new question(@"Care este rata de transfer pentru USB 2.0 ?",
            //    @"480Mbps",
            //    @"256Mbps",
            //    @"64Mbps",
            //    @"12Mbps");
            //q[57] = new question(@"Ce este Apache?",
            //    @"WebServer",
            //    @"Sistem de operare",
            //    @"Browser",
            //    @"PC");
            //q[58] = new question(@"Care este rata maxima de viteza pentru wieless 802.11a standard",
            //    @"54Mbps",
            //    @"24Mbps",
            //    @"10Mbps",
            //    @"100Mbps");
            //q[59] = new question(@"In ce an a fost lansat Windows XP",
            //    @"2001",
            //    @"2002",
            //    @"2003",
            //    @"2004");
        }
        //done

        private void GenerateMate()
        {q[0]= new question(
@"Intr-un an, unele luni au 31 de zile, altele doar 30. Cate luni au 28 de zile?",
@"12 ",
@" 1 ",
@" 5 ",
@" 0 ");
q[1] =new question(
@"Ce numar urmeaza in sir? 17, 37, 47, 67, 97, ...",
@"107",
@"137",
@"101",
@"117");
q[2] =new question(
@"Daca din 3 chistoace faci o tigara, din 9 chistoace cate tigari faci?",
@"4",
@"3",
@"1",
@"6");
q[3] =new question(
@"Un tip bea ce bea pana se face ranga, iese din restaurant, coboara 3 trepte si moare. De ce?",
@"A fost din cauza restaurantului.",
@"A fost blestemat de o pisica neagra",
@"Asa ceva este imposibil",
@"Se stie ca poti muri daca bei prea mult");
q[4] =new question(
@"Alege numarul care completeaza sirul urmator: 169 144 121 100 81 ...",
@"64",
@"72",
@"74",
@"9");
q[5] =new question(
@"Andrei are 4 ani, iar sora lui are de 3 ori mai mult decat el. Ce varsta va avea sora cand Andrei va implini 12 ani?",
@"20",
@"18",
@"28",
@"32");
q[6] =new question(
@"Aneta este pentru Atena asa cum 39257 este pentru...",
@"75293",
@"75923",
@"35297",
@"32957");
q[7] =new question(
@"Dupa 15 minute de condus cu viteza constanta, un sofer observa ca a parcurs 15 km. Cu ce viteza a condus acesta?",
@"60 Km/h",
@"45 Km/h",
@"30 Km/h",
@"15 Km/h");
q[8] =new question(
@"Cate roti are o masina normala?",
@"5",
@"4",
@"3",
@"6");
q[9] =new question(
@"Care este cel de-al 10-lea termen al sirului 1,3,5,7,...?",
@"19",
@"9",
@"11",
@"21");
q[10] =new question(
@"Ce numar completeaza in mod logic seria: 2, 5, 12, 27, 58, ...",
@"121",
@"100",
@"116",
@"74");
q[11] =new question(
@"Ce numar completeaza in mod logic seria: 1, 8, 22, 43, 71, ...",
@"106",
@"152",
@"89",
@"80");
q[12] =new question(
@"Ce numar completeaza in mod logic seria: 0, 10, 10, 20, 30, 50, ...",
@"80",
@"90",
@"300",
@"120");
q[13] =new question(
@"Daca: 1 + 1 = 3 si 4 + 4 = 7 Cat face: 1 + 4?",
@"5",
@"7",
@"8",
@"3");
q[14] =new question(
@"Un gard drept, neinchis la capete, are 117 piloni verticali care il sustin. Cate spatii poti numara intre piloni?",
@"116",
@"0",
@"119",
@"117");
q[15] =new question(
@" Ce numar completeaza in mod logic seria: 3, 2, 9, 4, 81, ...",
@"16",
@"18",
@"15",
@"17");
q[16] =new question(
@"Ce numar completeaza in mod logic seria: 45, 15, 41, 19, 38, 22, ...",
@"36",
@"24",
@"58",
@"46");
q[17] =new question(
@"Ce numar completeaza in mod logic seria: 98, 88, 79, 71, ...",
@"64",
@"22",
@"37",
@"57");
q[18] =new question(
@"20 de pasari stau pe un copac. Toate in afara de 15 zboara. Cate pasari au ramas pe copac?",
@"15",
@"20",
@"5",
@"0");
q[19] =new question(
@"Esti intr-o cursa si tocmai l-ai ajuns pe cel aflat pe locul trei. Pe ce loc esti acum?",
@"3",
@"2",
@"4",
@"8");
q[20]= new question(
@"Valorea numarului imaginar i la patrat este:",
@"-1",
@"1",
@"-i",
@"i");
q[21]= new question(
@"Inversa unei matrice se determina cu ajutorul urmatoarei formule:",
@"1/det(matrice)*matricea adjuncta",
@"matricea adjuncta*matricea unitate",
@"det(matrice)*matricea unitate",
@"det(matricea adjuncta)*matrice");
q[22]= new question(
@"Determinantul unei matrice se calculeza cand:",
@"Nr. de coloane = Nr. de linii",
@"Nr. de coloane > Nr. de linii",
@"Nr. de linii > Nr.de coloane",
@"Nu se calculeaza");
q[23]= new question(
@"Logaritmii in baza zece se numesc:",
@"Logaritmi zecimali",
@"Logaritmi uzuali",
@"Logaritmi naturali",
@"Logaritmi binari");
q[24]= new question(
@"Criteriul Raabe-Duhamel enunta ca o limita este convergenta cand:",
@"a>1",
@"a<1",
@"a=0",
@"a=1");
q[25]= new question(
@"Teorema lui Abel: Seria Suma din AnBn, cand n>=o este convergenta daca sirul Bn este:",
@"monoton si marginit",
@"monoton si nemarginit",
@"monoton",
@"nul");
q[26]= new question(
@"Functii: Limita din 1/infint este:",
@"0",
@"infinit",
@"caz de nedeterminare",
@"1");
q[27]= new question(
@"Functii, derivate: (arctg x)derivat are ca solutie:",
@"1/1+x2",
@"1/radical(1-x)",
@"sin x",
@"e");
q[28]= new question(
@"Functii, derivate: (sin x)derivat are ca solutie:",
@"cos x",
@"sin x",
@"-1",
@"1/cos x");
q[29]= new question(
@"Analiza: Asimptotele reprezinta dreapta ... fata de care graficul unei functii se apropie oricat de mult. :",
@"Toate variantele de raspuns",
@"Dreapta verticala ",
@"Dreapta orizontala",
@"Dreapta oblica");
q[30]= new question(
@"Algebra: Media aritmetica este reprezentata de formula:",
@"(A1+A2+...+An)/n",
@"(A1*A2*...*An)/n",
@"(A1+A2+...+An)/n*n",
@"(A1+A2+...+An)/2");
q[31]= new question(
@"Algebra: Media geometrica este reprezentata de formula:",
@"Radical din (a*b)",
@"Radical din (a+b)",
@"Radical din (a+b)(a-b)",
@"Radical din (a*b)/2");
q[32]= new question(
@"Determinantul unei matrice se poate calcula folosind:",
@"Regula triunghiului/lui Sarrus ",
@"Regula lui Cramer",
@"Teorema lui Rouche",
@"Transpusa matricei");
q[33]= new question(
@"Limite remarcabile: limita cand x>0 din sin x/x are ca rezultat:",
@"1",
@"-1",
@"infinit",
@"-infinit");
q[34]= new question(
@"Limite remarcabile: limita cand x>0 din ln(1+x)/x are ca rezultat:",
@"1",
@"2",
@"-1",
@"0");
q[35]= new question(
@"Limite remarcabile: limita cand x>0 din arcsin x/x are ca rezultat:",
@"1",
@"0",
@"-1",
@"r");
q[36]= new question(
@"Siruri: Orice sir crescator si nemarginit are limita:",
@"+infinit",
@"-infint",
@"zero",
@"finita");
q[37]= new question(
@"Integrala din 1/x dx are ca solutie:",
@"ln|x|",
@"x/ln|x|",
@"sin x",
@"-cos x");
q[38]= new question(
@"Integrala din sin x dx are ca solutie:",
@"-cos x",
@"cos x",
@"tg x",
@"-tg x");
q[39]= new question(
@"Integrala din tg x dx are ca solutie:",
@"- ln|cos x|",
@"- ln|sin x|",
@"-cos x",
@"ln|cos x|");
        }
        //done

        private void GenerateGeo()
        {

            q[0] = new question(
            @"Care este capitala Estoniei?",
            @"Tallinn",
            @"Vaduz",
            @"Valletta",
            @"Skopje");

            q[1] = new question(
            @"Capitala Liechtensteinului este ...?",
            @"Vaduz",
            @"Copenhaga",
            @"Kiev",
            @"Sarajevo");

            q[2] = new question(
            @"Care este simbolul internaþional pentru Emiratele Arabe Unite?",
            @"AE",
            @"EAU",
            @"AE",
            @"AU");

            q[3] = new question(
            @"Care este simbolul internaþional al Insulelor Cocos?",
            @"CC",
            @"CO",
            @"CI",
            @"IC");

            q[4] = new question(
            @"Care este simbolul internaþional pentru Angola?",
            @"AO",
            @"AN",
            @"AG",
            @"AL");

            q[5] = new question(
            @"Cel mai inalt varf al Romaniei este",
            @"Moldoveanu",
            @"Omu",
            @"Negoiu",
            @"Rodnei");

            q[6] = new question(
            @"Bauxitele, din care se extrage aluminiul, se gasesc in:",
            @"Muntii Apuseni",
            @"Muntii Banatului",
            @"Muntii Fagaras",
            @"Muntii Bucegi");

            q[7] = new question(
            @"Muntii Poiana Rusca apartin de:",
            @"Carpatii Occidentali",
            @"Carpatii Orientali",
            @"Carpatii Meridionali",
            @"Subcarpati");

            q[8] = new question(
            @"Climatul Romaniei este:",
            @"temperat continental-moderat",
            @"temperat umed",
            @"temperat continental",
            @"mediteranean");

            q[9] = new question(
            @"Cati kilometri parcurge Dunarea pe teritoriul tarii noastre?",
            @"1075",
            @"1050",
            @"1320",
            @"1015");

            q[10] = new question(
            @"Curentii care lipsesc Marii Negre sunt:",
            @"Curentii verticali",
            @"Curentii de compensatiet",
            @"Curentii de suprafata",
            @"Nu lipsesc");

            q[11] = new question(
            @"Ce animal este specific pentru padurile de foioase de la noi?",
            @"lupul",
            @"ursul",
            @"vulpea",
            @"cerbul");

            q[12] = new question(
            @"Care este cea mai joasa altitudine a Campiei Romane?",
            @"10-20m",
            @"5-10m",
            @"40-50m",
            @"70-80m");

            q[13] = new question(
            @"Raul Mures strabate podisul:",
            @"Transilvaniei",
            @"Mehedinti",
            @"Getic",
            @"Dobrogei");

            q[14] = new question(
            @"La recensamantul din anul 1948 populatia Romaniei era de:",
            @"15,8 milioane locuitori",
            @"15,3 milioane locuitori",
            @"15,1 milioane locuitori",
            @"14,8 milioane locuitori");

            q[15] = new question(
            @"Care este proportia de terenuri agricole din suprafata totala a Romaniei?",
            @"62%",
            @"55%",
            @"38%",
            @"74%");

            q[16] = new question(
            @"Sibiul are o veche traditie in ceea ce priveste:",
            @"industria lânii",
            @"industria lemnului",
            @"industria hartiei ",
            @"industria petrolului");

            q[17] = new question(
            @"Principalul bazin de extractie al huilei se afla in:",
            @"Petrosani",
            @"Petrila",
            @"Lupeni",
            @"Oltenitei");

            q[18] = new question(
            @"Hidrocentrala Portile de Fier II se afla la:",
            @"Ostrovul Mare",
            @"Anina",
            @"Calafat",
            @"Bazias");

            q[19] = new question(
            @"Rafinaria de la Brazi, in cadrul careia se fabrica cauciucul sintetic, se afla in judetul:",
            @"Prahova",
            @"Bacãu",
            @"Valcea",
            @"Arges");

            q[20] = new question(
            @"Cate procente de Raze Ultraviolete reflecta zapada?",
            @"Peste 90%",
            @"30%",
            @"Mai putin de 50%",
            @"Nu reflecta razele UV");

            q[21] = new question(
            @"Unde este construit anual cel mai mare palat de gheata din lume?",
            @"Finlanda",
            @"Rusia",
            @"Norvegia",
            @"Ungaria");

            q[22] = new question(
            @"În ce an a fost inregistrat cel mai puternic cutremur din Romania?",
            @"1940",
            @"1984",
            @"2000",
            @"2013");

            q[23] = new question(
            @"Ce magnitudine a avut cel mai puternic cutremur inregistrat in Romania?",
            @"7.3 grade",
            @"8.4 grade",
            @"4.0 grade",
            @"7.2 grade");

            q[24] = new question(
            @"Unde a avut loc cel mai puternic cutremur de pamant inregistrat vreodata?",
            @"Chile",
            @"Madagascar",
            @"Japonia",
            @"Romania");

            q[25] = new question(
            @"Care a foat magnitudinea celui mai puternic cutremur de pamant inregistrat vreodata?",
            @"9.5 grade",
            @"9.8 grade",
            @"10 grade",
            @"8.4 grade");

            q[26] = new question(
            @"Care este lungimea totala aproximativa a Dunarii?",
            @"2850 km.",
            @"3290 km.",
            @"1200 km.",
            @"150 m.");

            q[27] = new question(
            @"Care este cel mai lung lant muntos din Europa?",
            @"Muntii Carpati",
            @"Muntii Alpi",
            @"Muntii Ural",
            @"Muntii Fericirii");

            q[28] = new question(
            @"Care este cea mai mare structura acvatica anoxica din lume?",
            @"Marea Neagra",
            @"Marea Rosie",
            @"Lacul Baical",
            @"Raul Dambovita");

            q[29] = new question(
            @"Ce suprafata a planetei Terra o acopera apa?",
            @"71%",
            @"80%",
            @"30%",
            @"100%");

            q[30] = new question(
            @"Care este cel mai comun nume de pe glob?",
            @"Muhammad",
            @"Andrei",
            @"Ion",
            @"John");

            q[31] = new question(
            @"Care este tara cu cea mai mare populatie?",
            @"China",
            @"Vatican",
            @"Rusia",
            @"Canada");

            q[32] = new question(
            @"Care este cel mai mare continent?",
            @"Asia",
            @"America de Nord",
            @"America de Sud",
            @"Europa");

            q[33] = new question(
            @"Ce numar de locuitori are Antarctica?",
            @"Aproximativ 1000",
            @"Mai putin de 20 000",
            @"3000",
            @"14 000");

            q[34] = new question(
            @"Unde se pot gasi cele mai multe specii de reptile din lume?",
            @"Australia",
            @"Madagascar",
            @"Austria",
            @"Brazilia");

            q[35] = new question(
            @"Cate specii de reptile se gasesc in Australia?",
            @"755",
            @"320",
            @"400",
            @"Acolo nu exista reptile.");

            q[36] = new question(
            @"Care este cel mai mare parc national din lume?",
            @"Yellowstone",
            @"Gradina Botanica",
            @"NY Central Park",
            @"Grand Canyon");

            q[37] = new question(
            @"Cate perocente din populaţia lumii trăieşte în emisfera nordică?",
            @"90%",
            @"75%",
            @"30%",
            @"53%");

            q[38] = new question(
            @"Care ocean conţine peste jumătate din întreaga apă sărată din lume?",
            @"Oceanul Pacific",
            @"Oceanul Atlantic",
            @"Oceanul Indian",
            @"Oceanul Antarctic");

            q[39] = new question(
            @"Care tara gazduieste cea mai mare intindere desertica din lume?",
            @"Libia",
            @"Egipt",
            @"S.U.A.",
            @"Australia");

            //q[40] = new question(
            //@"Cate lacuri gazduieste,aproximativ, Canada ?",
            //@"3 milioane",
            //@"2 mii",
            //@"30 mii",
            //@"1 milion");



        }
        //done

        private void GenerateIst()
        {
            q[0] = new question(@"Cand s-a prabusit imperiul roman ?",@"476",@"452",@"560",@"281");
q[1] = new question(@"Cum a murit J.F Kennedy ?",@"asasinat",@"s-a inecat",@"cancer",@"altceva");
q[2] = new question(@"Intre ce ani a domnit Suleiman Magnificul ?",@"1520-1566",@"1501-1529",@"1555-1560",@"1581-1611");
q[3] = new question(@"Cand a cazut zidul Berlinului",@"9 noiembrie 1989",@"10 ianuarie 1988",@"22 martie 1990",@"1 aprilie 1989");
q[4] = new question(@"Cand sia declarat independenta SUA",@"1776",@"1778",@"1774",@"1770");
q[5] = new question(@"Cand a devenit crestinismul religie oficiala ?",@"394",@"320",@"416",@"432");
q[6] = new question(@"Cand a fost distrusa cetatea Troia ?",@"1180 I.H.",@"1180 D.H.",@"118 I.H.",@"1233 I.H.");
q[7] = new question(@"Numele conducator al hunilor, supranumit 'Biciul lui Dumnezeu' era:",@"Attila",@"Odoacru",@"Burebista",@"Alexandru cel Mare");
q[8] = new question(@"Cand s-a nascut Mahomed ?",@"570",@"625",@"425",@"325");
q[9] = new question(@"Capitala Imperiului Roman de Apus, a fost mutata la ?",@"Ravenna",@"Constantinopole",@"Milano",@"Venetia");
q[10] = new question(@"Unde a aparut tiparul cu presa din lemn?",@"China",@"Italia",@"Germania",@"India");
q[11] = new question(@"Unde a fost inventat praful de pusca?",@"China",@"Grecia",@"Egipt",@"Persia");
q[12] = new question(@"Chichen Itza, Tikal, Uxmal, apartineau unei mari civilizatii precolumbiene a Americii:",@"Maya",@"Azteca",@"Olmeca",@"Incasa");
q[13] = new question(@"Cand a fost descoperita America pentru prima data ?",@"Nu se stie sigur",@"Aproximativ anul 1000 D.H.",@"1492",@"1400");
q[14] = new question(@"Cuvantul 'viking' insemna in primele limbi scandinave:",@"pirat, navigator",@"fermier de coasta",@"luptator cu toporul",@"navigator al nordului");
q[15] = new question(@"Armata lui Attila a fost infranta in anul 451 de catre ?",@"armatele unite ale romanilor si vizigotilor",@"armata romana",@"armata vizigota",@"armata germana");
q[16] = new question(@"Limita maxima a expansiunii islamice in Europa Occidentala a fost:",@"Sudul Frantei",@"Nordul Spaniei",@"Sudul Germaniei",@"Sudul Germaniei");
q[17] = new question(@"De unde au preluat arabii sistemul numeric si pe cel zecimal?", @"din India", @"din China", @"din scrierile antice grecesti", @"din Persia");
q[18] = new question(@"Unde se afla Biblioteca din Alexandria?",@"Egipt",@"India",@"Macedonia",@"Grecia");
q[19] = new question(@"Când a început Revoluţia franceză?",@"1789",@"1799",@"1820",@"1756");
            q[20] = new question("Cine a facut parte din Antanta in Primul Razboi Mondial?",
 @"Rusia, Marea Britanie, Franta",
 @"Germania, Rusia, Franta",
 @"Italia, Japonia, Germania",
 @"Franta, Marea Britanie, Spania");

q[21] = new question("Carui politician roman apartine fraza >>Veni, vidi, vici?<<",
@"Cezar",
@"Traian",
@"Balbinus",
@"Augustus");

q[22] = new question("Care a fost unltimul tar al Imperiului Rus?",
@"Nicolae II",
@"Boris Godunov",
@"Alexandru II",
@"Ivan IV");

q[23] = new question ("In ce an s-a terminat razboiul din Vietnam?",
@"1975",
@"1973",
@"1980",
@"1978");

q[24] = new question ("Cum se numeste curentul crestin creat de reformatorul Martin Luther in secolul XVI?",
@"Protestantism",
@"Evangelism",
@"Baptism",
@"Catolicism");

q[25] = new question ("In ce an a incetat sa existe Imperiul Roman de Vest?",
@"476",
@"378",
@"512",
@"788");

q[26] = new question("Care a fost ultima batalie din cariera lui Napoleon Bonaparte?",
@"Batalia de la Waterloo",
@"Batalia de la Austerlitz",
@"Batalia de la Jenna",
@"Batalia de la Borodino");

q[27] = new question ("Cine a fost succesorul presedintelui american J.F. Kennedy?",
@"Lyndon Johnson",
@"Andrew Jackson",
@"Jimmy Carter",
@"Dwight Eisenhower");

q[28] = new question ("Anul 1903 a fost marcat de aparitia unui aparat care a schimbat cursul istoriei. Cum se numeste aceasta inventie?",
@"Avion",
@"Camera de filmat",
@"Telefon",
@"iPad");

q[29] = new question ("Cui apartinea imperiul care se intindea pe teritoriul celor trei state actuale Germania, Franta, Italia in secolele VIII-XI?",
@"Carol cel Mare",
@"Papa Pius II",
@"William Cuceritorul",
@"Pepin cel Scurt");

q[30] = new question ("Ideologia pusa in practica de Marea Britanie, Franta, Olanda si Spania in secolele XVI-XIX ca urmare a descoperirilor geografice se numeste...?",
@"Colonialism",
@"Emigrare",
@"Embargo",
@"Autocratie");

q[31] = new question ("Intrebare Bonus: Care razboi inca nu s-a sfarsit?",
@"Al Doilea Razboi Mondial",
@"Razboiul din Vietnam",
@"Razboiul din Coreea",
@"Razboiul Rece");

q[32] = new question ("Care este cel mai cunoscut conducator si cuceritor din istoria Asiei?",
@"Ghenghis Khan",
@"Quin Zhiayu",
@"Kim Ir Sen",
@"Mahatma Gandhi");

q[33] = new question ("Care din liderii politici enumerati nu e considerat dictator?",
@"Nelson Mandela",
@"Pol Pot",
@"Iosif Stalin",
@"Adolf Hitler");

q[34] = new question("In ce an a ajuns primul om, Yuri Gagarin, in spatiu?",
@"1959",
@"1960",
@"1969",
@"1957");


q[35] =new question ("Cati ani a durat Razboiul de 100 de Ani?",
@"116",
@"100",
@"99",
@"108");

q[36] =new question ("Ce stat avea cea mai mare putere si influenta in Europa secolelor XII-XVII?",
@"Vaticanul",
@"Italia",
@"Franta",
@"Spania");

q[37] =new question ("Ce erau hughenotii?",
@"Calvinisti francezi",
@"Mercenari irlandezi",
@"Calugari nomazi",
@"Luterani austrieci");

q[38] =new question ("Cui apartin cuvintele >>E pur si muove (Si totusi se misca)<<?",
@"Galileo Galilei",
@"Albert Einstein",
@"Leonardo Da Vinci",
@"Isaac Newton");


q[39] = new question("In ce an a cazut Zidul Berlinului?",
@"1989",
@"1990",
@"1988",
@"1991");
        }

        #endregion

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            qTimer.Stop();
            tTimer.Stop();
            navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        private void var_tap(object sender, TappedRoutedEventArgs e)
        {
            
            switch ((string)((Button)sender).Name)
            {
                case "var1":
                    if (vCorect == 0)
                    {
                        Corect();
                    }
                    else
                    {
                        Gresit(penalizare);
                    }
                    break;
                case "var2":
                    if (vCorect == 1)
                    {
                        Corect();
                    }
                    else
                    {
                        Gresit(penalizare);
                    }
                    break;
                case "var3":
                    if (vCorect == 2)
                    {
                        Corect();
                    }
                    else
                    {
                        Gresit(penalizare);
                    }
                    break;
                case "var4":
                    if (vCorect == 3)
                    {
                        Corect();
                    }
                    else
                    {
                        Gresit(penalizare);
                    }
                    break;

            }
        }

        int scor = 0;
        private void Gresit(int penalizare)
        {
            try
            {
                floatText.Margin = new Thickness(0, -150, 0, 0);
                floatText.Opacity = 1; textTime = 0;
                //MessageDialog msg = new MessageDialog("Ai răspuns gresit!");
                //var v = msg.ShowAsync();
                floatText.Text = "Ai răspuns greșit!"; 
                textTimer.Start();

            }
            catch (Exception)
            { }
            scor -= penalizare;
            UpdateScor();
        }
        private void Corect()
        {
            try
            {
                floatText.Margin = new Thickness(0, -150, 0, 0);
                floatText.Opacity = 1; textTime = 0;
                //MessageDialog msg = new MessageDialog("Ai răspuns corect!");
                //var v = msg.ShowAsync();
                floatText.Text = "Ai răspuns corect!";
                textTimer.Start();
            }
            catch(Exception)
            { }
            scor += 5;
            tT+=3;
            UpdateScor();
        }

        private void MakeTile()
        {
            ITileSquare310x310Text09 tileContent = TileContentFactory.CreateTileSquare310x310Text09();
            tileContent.TextHeadingWrap.Text = "Ultimul tău punctaj a fost " + scor;

            ITileWide310x150Text03 wide310x150Content = TileContentFactory.CreateTileWide310x150Text03();
            wide310x150Content.TextHeadingWrap.Text = "Ultimul tău punctaj a fost " + scor;

            ITileSquare150x150Text04 square150x150Content = TileContentFactory.CreateTileSquare150x150Text04();
            square150x150Content.TextBodyWrap.Text = "Ultimul tău punctaj a fost " + scor;

            wide310x150Content.Square150x150Content = square150x150Content;

            tileContent.Wide310x150Content = wide310x150Content;

            TileUpdateManager.CreateTileUpdaterForApplication().Update(tileContent.CreateNotification());
        }
        private void UpdateScor()
        {
            qT = 11;
            scorTxt.Text = scor.ToString();
            QeneratePuzzle(0);
        }

        private void nav(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(NewGame), ((Image)sender).Name);
            
        }
    }
}
