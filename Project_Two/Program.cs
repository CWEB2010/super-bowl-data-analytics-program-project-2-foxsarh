using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace Project_Two
{
    class Program
    {
        static void Main(string[] args)
        {
            /**Your application should allow the end user to pass end a file path for output 
            * or guide them through generating the file.
            **/

            //Declarations//
            const string PATH = @"C: \Users\foxsarh\OneDrive - Dunwoody College of Technology\Freshman Yr\Advanced_Programming\Project 2\Super_Bowl_Project.csv";

            FileStream input;
            StreamReader read;
            string primer;
            string[] superBowlData;
            List<SuperBowl> superbowlList = null; //Need an empty list to start so I can fill it with the data from the CSV file

            try
            {
                input = new FileStream(PATH, FileMode.Open, FileAccess.Read);
                read = new StreamReader(input);
                //primer = read.ReadLine(); <-- is this redundant? 
                superbowlList = new List<SuperBowl>();


                //Looping structure to read in all columns from CSV
                while (!read.EndOfStream)
                {
                    superBowlData = read.ReadLine().Split(',');
                    superbowlList.Add(new SuperBowl(superBowlData[0], superBowlData[1], Convert.ToInt32(superBowlData[2]), superBowlData[3],
                        superBowlData[4], superBowlData[5], Convert.ToInt32(superBowlData[6]), superBowlData[7], superBowlData[8], superBowlData[9],
                        Convert.ToInt32(superBowlData[10]), superBowlData[11], superBowlData[12], superBowlData[13], superBowlData[14]));

                    primer = read.ReadLine(); //primer
                }

            }// end of try


        }
    }//End of Program Class

    class SuperBowl
    {
        string Date;
        string SuperBowlNumber;
        int Attendance;
        string QBWinner;
        string CoachWinner;
        string Winner;
        int WinningPoints;
        string QBLoser;
        string CoachLoser;
        string Loser;
        int LosingPoints;
        string MVP;
        string Stadium;
        string City;
        string State;

        public SuperBowl (string Date, string SuperBowlNumber, int Attendance, string QBWinner, string CoachWinner, string Winner,
            int WinningPts, string QBLoser, string CoachLoser, string Loser, int LosingPts, string MVP, string Stadium, string City, string State)
        {
            this.Date = Date;
            this.SuperBowlNumber = SuperBowlNumber;
            this.Attendance = Attendance;
            this.QBWinner = QBWinner;
            this.CoachWinner = CoachWinner;
            this.Winner = Winner;
            WinningPoints = WinningPts;
            this.QBLoser = QBLoser;
            this.CoachLoser = CoachLoser;
            this.Loser = Loser;
            LosingPoints = LosingPts;
            this.MVP = MVP;
            this.Stadium = Stadium;
            this.City = City;
            this.State = State;          
        }
    }


}
