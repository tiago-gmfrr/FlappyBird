using System.IO;
using System.Web.Script.Serialization; // Add System.Web.Extensions

namespace FlappyBird
{
    static class Highscore
    {
        static int highscore;
        
        static string filename = "highscores.txt";

        public static void Initialize()
        {
            ReadHighscore();
        }

        static void ReadHighscore()
        {
            if (!File.Exists(filename))
                File.Create(filename);
            
            try
            {
                highscore = int.Parse(File.ReadAllText(filename));
            }
            catch
            {
                highscore = 0;
            }
        }

        static void WriteHighscore(int highscore)
        {
            try
            {
                File.WriteAllText(filename, highscore.ToString());
            }
            catch { }
        }
        
        public static int GetHighscore(int score)
        {
            if (highscore < score)
                highscore = score;

            WriteHighscore(highscore);

            return highscore;
        }
    }
}