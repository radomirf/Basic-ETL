using System;
using System.IO;

namespace Basic_ETL
{

    internal class Program
    {

      
        struct MovieNActors
        {
            public int Id;
            public string MovieName;
            public string[] Actors;
        }
        static void Main(string[] args)
        {


            string fileName = @"C:\Users\Acer\Desktop\C#\Basic ETL\Basic ETL\text.csv";
            string[] csvLines = Extract(fileName);


            MovieNActors[] movieNActors = Transform(csvLines);

            LoadMovieNActors(movieNActors);
        }

        private static MovieNActors[] Transform(string[] csvLines)
        {

            MovieNActors[] movieNActors = new MovieNActors[csvLines.Length];

            for (int i = 0; i < csvLines.Length; i++)
            {
                if (i > 0)
                {
                    string line = csvLines[i];
                    movieNActors[i] = MapCsvLines(line);
                }
            }
            return movieNActors;
        }

        private static MovieNActors MapCsvLines(string line)
        {
            MovieNActors movieNActors = new MovieNActors();
            string[] columns = line.Split(",\"");
            string[] columns2 = columns[0].Split(",");
            if (columns.Length.Equals(3))
            {
                columns2 = columns[2].Split(",");

                movieNActors.Id = int.Parse(columns[0]);
                movieNActors.MovieName = columns2[1];
                movieNActors.Actors = ExtractMovieActors(columns[2]);
            }
            else
            {
                columns2 = columns[0].Split(",");

                movieNActors.Id = int.Parse(columns2[0]);
                movieNActors.MovieName = columns2[1];
                movieNActors.Actors = ExtractMovieActors(columns[1]);
            }

            return movieNActors;
        }

        private static string[] ExtractMovieActors(string actorsColumn)
        {
            string[] jsonObject = actorsColumn.Split("},");
            string[] actors = new string[jsonObject.Length];
            for (int i = 0; i < jsonObject.Length; i++)
            {
                string actor = jsonObject[i];
                actors[i] = ExctractActor(actor);
            }
            return actors;
        }
  
        private static string ExctractActor(string actor)
        {
            int beginingOfName = actor.IndexOf("'name': '");
            string namePart = actor.Substring(beginingOfName + ("'name': '".Length));
            int endOfName = namePart.IndexOf("',");
            if (beginingOfName > 0)
            {
                string actorName = namePart.Substring(0, endOfName);
                return actorName;
            }
            return null;
        }


       
        static string[] Extract(string fileName)
        {
            string[] lines = File.ReadAllLines(fileName);
            return lines;
        }

    
        private static void LoadMovieNActors(MovieNActors[] movieNActors)
        {

            for (int index = 0; index < movieNActors.Length; index++)
            {

                File.AppendAllText(@"C:\test\Movies.csv", string.Concat(movieNActors[index].Id, ",", movieNActors[index].MovieName, Environment.NewLine));


                if (movieNActors[index].Actors != null)
                {
                    for (int jIndex = 0; jIndex < movieNActors[index].Actors.Length; jIndex++)
                    {
                        string actor = movieNActors[index].Actors[jIndex];

                        File.AppendAllText(@"C:\test\Actors.csv", string.Concat(movieNActors[index].Id, ",", actor, Environment.NewLine));
                    
                    }
                }
            }
        }



    }
}
