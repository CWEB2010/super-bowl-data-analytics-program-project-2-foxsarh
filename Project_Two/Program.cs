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
            const string PATH = @"C:\Users\foxsarh\Documents\Super_Bowl_Project.csv";

            FileStream infile;
            FileStream outfile;
            StreamReader read;
            StreamWriter write;
            string primer;
            string[] superBowlData;
            List<SuperBowl> superbowlList = null; //Need an empty list to start so I can fill it with the data from the CSV file
            //this list isn't filled with data unless all exceptions are handled 

            try
            {
                infile = new FileStream(PATH, FileMode.Open, FileAccess.Read);
                read = new StreamReader(infile);
                primer = read.ReadLine(); //<-- is this redundant? 
                superbowlList = new List<SuperBowl>();


                //Looping structure to read in all columns from CSV
                while (!read.EndOfStream)
                {
                    superBowlData = read.ReadLine().Split(',');
                    superbowlList.Add(new SuperBowl(superBowlData[0], superBowlData[1], Convert.ToInt32(superBowlData[2]), superBowlData[3],
                        superBowlData[4], superBowlData[5], Convert.ToInt32(superBowlData[6]), superBowlData[7], superBowlData[8], superBowlData[9],
                        Convert.ToInt32(superBowlData[10]), superBowlData[11], superBowlData[12], superBowlData[13], superBowlData[14]));

                    primer = read.ReadLine(); //primer
                }// end of while loop

                Console.WriteLine("Below is a List of All Super Bowl Winners");
                //superbowlList.ForEach(x => Console.WriteLine(x.Winner));
                foreach (SuperBowl x in superbowlList)
                {
                    Console.WriteLine(x.outputWinner());
                }

                //Generate a list of top 5 most attendance superbowls
                var attendanceQuery = from superbowl in superbowlList
                                      group superbowl by superbowl.Attendance into attendanceGroup
                                      select attendanceGroup;
                                        // HOW DO I DO THE MATH HERE FOR TOP 5...?


                //Generate a list of players who won MVP more than once
                var MVPCount = from x in superbowlList // <-- why does this only work with Var and not IEnumerable ??
                               group x by x.MVP into MVPGroup //create a group of the MVP data so it can be counted/quantified
                               where MVPGroup.Count() > 1
                               orderby MVPGroup.Key // Order the list according to # of times a player was MVP
                               select MVPGroup; // Return the ordered list of MVPs in List format  

                //Testing to see if above ^ code works
                //MVPCount.ToList<SuperBowl>().ForEach(x => Console.WriteLine(x.MVP));
                foreach (SuperBowl x in MVPCount)
                {
                    Console.WriteLine($"1. Name = {x.MVP}\n2. Winning Team = {x.Winner}\n3. Losing Team = {x.Loser}\n\n");
                }

                //var MVPCount = superbowlList.Where<SuperBowl>(x => x.MVP.Count() > 2).Select(x => x).ToList();
                //Console.WriteLine("Below are the Players Who Have Won MVP More Than Once");
                //MVPCount.ForEach(x => Console.WriteLine($"1. Name = {x.MVP}\n2. Winning Team = {x.Winner}\n3. Losing Team = {x.Loser}\n\n"));

                //write.Close();
                read.Close();
                infile.Close();

            }// end of try

            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }// end of catch


            ////write to a file
            //FileStream outFile = new
            //   FileStream("SomeText.txt", FileMode.Create,
            //   FileAccess.Write);
            //StreamWriter writer = new StreamWriter(outFile);
            //Console.Write("Enter some text >> ");
            //string text = Console.ReadLine();
            //writer.WriteLine(text);
            //// Error occurs if the next two statements are reversed
            //writer.Close();
            //outFile.Close();

        }// End of Main
    }//End of Program Class

    public class SuperBowl
    {
        public string Date;
        public string SuperBowlNumber;
        public int Attendance;
        public string QBWinner;
        public string CoachWinner;
        public string Winner;
        public int WinningPoints;
        public string QBLoser;
        public string CoachLoser;
        public string Loser;
        public int LosingPoints;
        public string MVP;
        public string Stadium;
        public string City;
        public string State;

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

        public string outputWinner()
        {
            return String.Format($"1. Team Name = {Winner}\n2. Date Won = {Date}\n3. Winning Quarterback = {QBWinner}" +
                $"\n4. Winning Coach = {CoachWinner}\n5. MVP = {MVP}\n6. Point Difference = {WinningPoints - LosingPoints}\n\n");
        } 

        public string outputTopFive()
        {
            return String.Format($"1. Date = {Date}\n2. Winning Team = {Winner}\n3. Losing Team = {Loser}" +
                $"\n4. City = {City}\n5. State = {State}\n6. Stadium = {Stadium}\n\n");
        }
    }



}
