using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PubSub;

namespace CmsPubSub
{


    public class GameStartEvent
    {
        public string Lag1 { get; set; }
        public string Lag2 { get; set; }
    }


    public class GameEndEvent
    {
        public string Lag1 { get; set; }
        public string Lag2 { get; set; }
        public int Goals1 { get; set; }
        public int Goals2 { get; set; }
    }



    public class PeriodStartEvent
    {
        public string Lag1 { get; set; }
        public string Lag2 { get; set; }
        public int Period { get; set; }
    }


    public class PeriodEndEvent
    {
        public string Lag1 { get; set; }
        public string Lag2 { get; set; }
        public int Period { get; set; }
    }

    public class GoalEvent
    {
        public string ScoringTeam { get; set; }

        public string Lag1 { get; set; }
        public string Lag2 { get; set; }
        public int Period { get; set; }
    }



    class Game2
    {
        private int goalsTeam1 = 0;
        private string team1;
        private int goalsTeam2 = 0;
        private string team2;

        public Game2(string lag1, string lag2)
        {
            team1 = lag1;
            team2 = lag2;
        }

        public void Run()
        {
            Hub.Default.Publish(new GameStartEvent { Lag1 = team1, Lag2 = team2 });

            for (int i = 1; i <= 3; i++)
                RunPeriod(i);

            Hub.Default.Publish(new GameEndEvent { Lag1 = team1, Lag2 = team2, Goals1 = goalsTeam1, Goals2 = goalsTeam2 });
        }

        private void RunPeriod(int periodNo)
        {
            Hub.Default.Publish(new PeriodStartEvent { Period = periodNo, Lag1 = team1, Lag2 = team2 });
            //Perioden simuleras...
            while (true)
            {
                Console.WriteLine("1. Mål Lag1");
                Console.WriteLine("2. Mål Lag2");
                Console.WriteLine("3. Perioden slut");
                var a = Console.ReadLine();
                if (a == "1")
                {
                    goalsTeam1++;
                    Hub.Default.Publish(new GoalEvent() { ScoringTeam = team1, Period = periodNo, Lag1 = team1, Lag2 = team2 });
                }
                if (a == "2")
                {
                    goalsTeam2++;
                    Hub.Default.Publish(new GoalEvent() { ScoringTeam = team2, Period = periodNo, Lag1 = team1, Lag2 = team2 });
                }

                if (a == "3")
                    break;
            }
            Hub.Default.Publish(new PeriodEndEvent { Period = periodNo, Lag1 = team1, Lag2 = team2 });

        }
    }

    class KorvGubbe
    {
        public KorvGubbe()
        {
            Hub.Default.Subscribe<PeriodStartEvent>(this, ev =>
            {
                if (ev.Period == 2)
                {
                    //burk
                }
            });                        
        }
    }


    class Lottery
    {
        public Lottery()
        {
            Hub.Default.Subscribe<PeriodStartEvent>(this, ev =>
            {
                if (ev.Period == 2)
                {
                    //dra lott
                }
            });
        }
    }


    class Jumbotron
    {
        public Jumbotron()
        {
            Hub.Default.Subscribe<GameStartEvent>(this, ev =>
            {
                Console.WriteLine($"Matchen mellan {ev.Lag1} och {ev.Lag2} börjar");
            });
            Hub.Default.Subscribe<GoalEvent>(this, ev =>
            {
                Console.WriteLine($"{ev.Lag1} - {ev.Lag2}  nu gjorde {ev.ScoringTeam} mål");
            });

            
        }
    }




    class Program
    {
        static void Main(string[] args)
        {
            var korvGubbe = new KorvGubbe();
            var sittBiljett = new Lottery();
            var jumboTron = new Jumbotron();

            Game2 g = new Game2("Tre kronor", "Team Canada");
            g.Run();




            //EVENTSTYRT see?

            // När period 2 startar: Skicka ett mail -> korvgubben att nu ska han hämta en ny korvburk
            // När period 2 startar: Dra en sittbiljettslottnummer

            // När hemmalaget tar ledningen . Spela upp ljud i högtalarna

            // När matchen början skriv ut lagnamn och ställning på  Jumbotronen

            // När mål skriv ut lagnamn och ställning på  Jumbotronen
        }
    }
}
