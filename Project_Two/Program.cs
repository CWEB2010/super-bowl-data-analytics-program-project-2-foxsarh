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
            FileStream outFile;
            StreamReader read;
            StreamWriter writer;
            string primer;
            string[] superBowlData;

            //*** add two more modules inside of main method ***/// 
            try
            {
                infile = new FileStream(PATH, FileMode.Open, FileAccess.Read);
                read = new StreamReader(infile);
                primer = read.ReadLine(); // primer 
                List<SuperBowl>superbowlList = new List<SuperBowl>();


                //Looping structure to read in all columns from CSV
                while (!read.EndOfStream)
                {
                    superBowlData = read.ReadLine().Split(',');
                    superbowlList.Add(new SuperBowl(superBowlData[0], superBowlData[1], Convert.ToInt32(superBowlData[2]), superBowlData[3],
                        superBowlData[4], superBowlData[5], Convert.ToInt32(superBowlData[6]), superBowlData[7], superBowlData[8], superBowlData[9],
                        Convert.ToInt32(superBowlData[10]), superBowlData[11], superBowlData[12], superBowlData[13], superBowlData[14]));
 
                }// end of while loop

                read.Dispose();
                infile.Dispose();

                //Introduction
                Console.WriteLine("-------------------------------------------");
                Console.WriteLine("    Welcome to The Super Bowl Database!    ");
                Console.WriteLine("-------------------------------------------");
                Console.WriteLine("You may choose the name of the file you would like your database to be saved in.");
                Console.WriteLine("It will be saved as a text file on your desktop\n");

                //Writing to a file --> don't need a using block since I will be writing to this file multiple times throughout the program and not just once
                string userName = Environment.UserName; //Grabs the username on the person's computer
                Console.Write("Please Enter the name of the text file: ");
                string userInput = Console.ReadLine();
                string filePath = $@"C:\Users\{userName}\Desktop\{userInput}.txt";
                outFile = new FileStream(filePath, FileMode.Create, FileAccess.Write);
                writer = new StreamWriter(outFile);

                //Conclusion
                Console.WriteLine("\nThank you for using the Super Bowl Database!");
                Console.WriteLine("----------------------------------------------");
                Console.WriteLine("Your file is stored at: " + filePath);

                //Print winners to file
                formatting("Winners", writer);
                writeDataToFile(superbowlList, "allWinners", writer);

                //Generate a list of top 5 most attendance superbowls
                var attendanceQuery = (from superbowl in superbowlList
                                       orderby superbowl.Attendance descending
                                       select superbowl).ToList<SuperBowl>().Take(5);

                //Print Top Five to file
                formatting("Top Five", writer);
                writeDataToFile(attendanceQuery.ToList(), "attendance", writer);
                //attendanceQuery.ToList<SuperBowl>().ForEach(x => Console.WriteLine($"1. Date = {x.Date}\n2. Winning Team = {x.Winner}\n3. Losing Team = {x.Loser}" +
                //$"\n4. City = {x.City}\n5. State = {x.State}\n6. Stadium = {x.Stadium}\n\n"));

                //Generate a list of the state that hosted the most super bowls
                int mostStates = superbowlList.GroupBy(x => x.State).OrderByDescending(x => x.Count()).First().Count();
                var stateQuery = (from SB in superbowlList
                                  group SB by SB.State into stateGroup
                                  where stateGroup.Count() == mostStates
                                  orderby stateGroup.Key descending
                                  select stateGroup).Take(1);

                formatting("State", writer);
                foreach (var x in stateQuery)
                {
                    foreach (var superbowl in x)
                    {
                        writer.WriteLine($"1. The city = {superbowl.City}\n2. The State = {superbowl.State}\n3. The Stadium = {superbowl.Stadium}\n\n");
                    }
                }

                /** Adapted from code originally by Leann Simonson **/
                //Generate a list of players who won MVP more than once
                var MVPCount = from x in superbowlList 
                               group x by x.MVP into MVPGroup //create a group of the MVP data so it can be counted/quantified
                               where MVPGroup.Count() > 1
                               orderby MVPGroup.Key // Order the list according to # of times a player was MVP
                               select MVPGroup; // Return the ordered list of MVPs in List format  
                /** end of citation **/

                formatting("MVP", writer);
                foreach (var x in MVPCount)
                {
                    writer.WriteLine($"\nPlayer {x.Key} won MVP {x.Count()} times");
                    writer.WriteLine("--------------------------------");

                    foreach (var superbowl in x)
                    {
                        writer.WriteLine($"Winning Team = {superbowl.Winner}\nLosing Team = {superbowl.Loser}\n");
                    }
                    
                }
                writer.WriteLine(""); //formatting

                writer.WriteLine("----------------");
                writer.WriteLine(" Misc Fun Facts ");
                writer.WriteLine("----------------");
                
                //Determine which coach lost the most superbowls
                int mostCoachLosses = superbowlList.GroupBy(x => x.CoachLoser).OrderByDescending(x => x.Count()).First().Count();
                var losingCoachQuery = (from SB in superbowlList
                                        group SB by SB.CoachLoser into losingCoachGroup
                                        where losingCoachGroup.Count() == mostCoachLosses
                                        orderby losingCoachGroup.Key descending
                                        select losingCoachGroup).Take(1);

                
                foreach (var x in losingCoachQuery)
                {
                    writer.WriteLine($"The coach who lost the most Super Bowls = {x.Key}");
                }

                //Determine which coach won the most superbowls
                int mostCoachWins = superbowlList.GroupBy(x => x.CoachWinner).OrderByDescending(x => x.Count()).First().Count();
                var winningCoachQuery = (from SB in superbowlList
                                         group SB by SB.CoachWinner into winningCoachGroup
                                         where winningCoachGroup.Count() == mostCoachWins
                                         orderby winningCoachGroup.Key descending
                                         select winningCoachGroup).Take(1);

                foreach (var x in winningCoachQuery)
                {
                    writer.WriteLine($"The coach who won the most super bowls = {x.Key}");
                }

                //Determine which team(s) won the most superbowls
                int mostWins = superbowlList.GroupBy(x => x.Winner).OrderByDescending(x => x.Count()).First().Count();
                var winningTeam = (from SB in superbowlList
                                   group SB by SB.Winner into winnerGroup
                                   where winnerGroup.Count() == mostWins
                                   //orderby winnerGroup.Key descending
                                   select winnerGroup.Key).ToArray();

                writer.WriteLine("\nThe Teams Who Won the Most Super Bowls:");
                for (var x=0; x < winningTeam.Length; x++)
                {
                    writer.WriteLine($"{winningTeam[x]}");
                }

                //Determine which team(s) lost the most superbowls
                int mostLosses = superbowlList.GroupBy(x => x.Loser).OrderByDescending(x => x.Count()).First().Count();
                var losingTeam = (from SB in superbowlList
                                  group SB by SB.Loser into loserGroup
                                  where loserGroup.Count() == mostLosses
                                  //orderby loserGroup.Key descending
                                  select loserGroup).Take(2);

                //Ouput which team lost the most supeprbowls 
                writer.WriteLine("\nThe Teams Who Lost the Most Super Bowls:");
                foreach (var x in losingTeam)
                {
                    writer.WriteLine($"{x.Key}");
                }

                // DEBUGGING ----->> superbowlList.ForEach(x => Console.WriteLine(x.pointDifference()));

                //Determine which Superbowl had the greatest point difference
                var greatestPoint = (from SB in superbowlList
                                     group SB by SB.pointDifference() into pointsGroup
                                     orderby pointsGroup.Key descending
                                     select pointsGroup).Take(1);

                writer.WriteLine(""); //formatting 
                foreach (var x in greatestPoint)
                {
                    foreach (var sb in x)
                    {
                        writer.WriteLine($"The Super Bowl with the greatest point difference:\n{sb.SuperBowlNumber} by {sb.pointDifference()} points");
                    }
                }

                writer.Close();
                outFile.Close();
            }// end of try

            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }// end of catch

            //ask user for file name and put it on their desktop, then i can concatenate the filename to the path that i know would work

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


        //method that outputs data
        public static void outputData(List<SuperBowl> data, string determinant)
        {
            if (determinant == "allWinners")
            {
                foreach (SuperBowl x in data)
                {
                    Console.WriteLine(x.outputWinner());
                }
            }
            else if (determinant == "attendance")
            {
                data.ToList<SuperBowl>().ForEach(x => Console.WriteLine($"1. Date = {x.Date}\n2. Winning Team = {x.Winner}\n3. Losing Team = {x.Loser}" +
                $"\n4. City = {x.City}\n5. State = {x.State}\n6. Stadium = {x.Stadium}\n\n"));
            }
            else
            {
                Console.WriteLine("Something went wrong");
            }
        }

        public static void writeDataToFile(List<SuperBowl> data, string determinant, StreamWriter outFile)
        {
            if (determinant == "allWinners")
            {
                foreach (SuperBowl x in data)
                {
                    outFile.WriteLine(x.outputWinner());
                }
            } else if (determinant == "attendance")
            {
                foreach (SuperBowl x in data)
                {
                    outFile.WriteLine(x.outputTopFive());
                }
            }
            else
            {
                Console.WriteLine("Oops! Something went wrong");
            }
        }
        public static void writeGroupDataToFile(Dictionary<String, SuperBowl> data, string determinant, StreamWriter outFile)
        {
            if (determinant == "state")
            {
                foreach (KeyValuePair<String, SuperBowl> x in data)
                {
                    Console.WriteLine($"1. The city = { x.Value.City}\n2. The State = {x.Value.State}\n3. The Stadium = {x.Value.Stadium}\n\n");
                }
            }
        }

        public static void formatting(string header, StreamWriter outfile)
        {
            outfile.WriteLine("------------------------------------------------------");
            if (header == "Winners")
            {
                outfile.WriteLine("Below is a List of All Super Bowl Winners");
            } 
            
            else if (header == "Top Five")
            {
                outfile.WriteLine("Below is a List of The Top 5 Most Attended Superbowls");
            }

            else if (header == "State")
            {
                outfile.WriteLine("Below is the State that Hosted the Most Superbowls");
            }

            else if (header == "MVP")
            {
                outfile.WriteLine("Below are the Players Who Have Won MVP More Than Once");
            }

            outfile.WriteLine("------------------------------------------------------\n");
        }
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

        public string outputState()
        {
            return String.Format($"1. The city = {City}\n2. The State = {State}\n3. The Stadium = {Stadium}\n\n");
        }

        public int pointDifference()
        {
            return (WinningPoints - LosingPoints);
        }

    }



}
