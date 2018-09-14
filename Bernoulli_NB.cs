using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NLP_Task
{
    public class Bernoulli_NB
    {
        double[] poriorProb;
        int[,] bagWordsCount;
        string[] class1;
        string[] class2;
        string[] bagOfWords;
        int[] numWordsInClass;
        Dictionary<string, double[]> probKeeper;
        public Bernoulli_NB()
        {
            numWordsInClass = new int[2];
            poriorProb = new double[2];
            probKeeper = new Dictionary<string, double[]>();
            //change to class1 dir
            string path = Directory.GetCurrentDirectory();
            class1 = Directory.GetFiles(path + "\\sport",
            "*.txt", SearchOption.AllDirectories);
            //change to class2 dir
            class2 = Directory.GetFiles(path + "\\tech",
            "*.txt", SearchOption.AllDirectories);
            //change to bag of words
            bagOfWords = new string[] { "Chelsea", "St-Germain", "season", "Manchester", "Dynamo", "Kiev", "Roma", "Real", "Juventus", "Bayern", "matches", "winners", "Arsenal", "Barcelona", "final", "champions", "goals", "champion", "boxer", "boxing", "Liverpool", "Jurgen", "Klopp", "Augsburg", "Tottenham", "Fiorentina", "Valencia", "clubs", "points", "Atletico", "Villarreal", "scored", "Modric", "Soldado", "Cristiano", "Ronaldo", "United", "Nicola", "Adams", "chance", "Championship", "FIFA", "Cup", "Premier", "Tennis", "Serena", "Williams", "Grand", "Slam", "winner", "player", "players", "WTA", "stadium", "Basketball", "NBA", "Athletics", "FIBA ", "MVP", "team's", "FIVB ", "Volleyball", "sport", "Olympic", "yards", "UEFA", "Benfica", "UFC Featherweight", "UFC", "Messi", "assists", "leagues", "Facebook", "Snapchat", "computer", "virus", "mobile", "internet", "robots", "technologies", "Twitter", "hacking", "hackers", "email", "websites", "smartphone", "Samsung", "iPhone", "software", "programs", "Wi-Fi", "phones", "Artificial", "intelligence", "Yahoo", "games", "screen", "TV", "Nasa", "Nasa's", "Google", "BlackBerry", "Android", "HTC", "Camera", "Microsoft", "Sony", "PlayStation", "Xbox", "iOS", "Apple", "app", "Cloud", "Cisco", "IBM" };
            for (int i = 0; i < bagOfWords.Length; i++)
            {
                bagOfWords[i] = bagOfWords[i].ToLower();
            }
        }


        public void computePorior()
        {
            int numOfDocuments = class1.Length + class2.Length;
            poriorProb[0] = class1.Length / (double)(numOfDocuments);
            poriorProb[1] = class2.Length / (double)(numOfDocuments);

        }

        public void allWordsInClass()
        {
            bagWordsCount = new int[2, bagOfWords.Length];
            //all files in class 2
            foreach (string fileName in class1)
            {
                string allTextInFile = System.IO.File.ReadAllText(@fileName).ToLower();
                //find each word if word file of class 1
                for (int i = 0; i < bagOfWords.Length; i++)
                {
                    bagWordsCount[0, i] += Regex.IsMatch(allTextInFile, bagOfWords[i]) ? 1 : 0;
                }

            }
            //total number of words in class1
            for (int i = 0; i < bagOfWords.Length; i++)
            {
                numWordsInClass[0] += bagWordsCount[0, i];
            }

            //all files in class 2
            foreach (string fileName in class2)
            {
                string allTextInFile = System.IO.File.ReadAllText(@fileName).ToLower();
                //find each word if word file of class 1
                for (int i = 0; i < bagOfWords.Length; i++)
                {
                    bagWordsCount[1, i] += Regex.IsMatch(allTextInFile, bagOfWords[i]) ? 1 : 0;
                }
                //for (int i = 0; i < bagOfWords.Length; i++)
                //{
                //    Console.WriteLine(bagOfWords[i] + "\t" + bagWordsCount[0, i]);
                //}

            }

            //total number of words in class1
            for (int i = 0; i < bagOfWords.Length; i++)
            {
                numWordsInClass[1] += bagWordsCount[1, i];
            }
            // Console.WriteLine(numWordsInClass[1]);
        }

        string equation = "";
        public void calculateProb()
        {
            for (int i = 0; i < bagOfWords.Length; i++)
            {
                equation += "(" + bagWordsCount[0, i] + "+" + 1 + ")/(" + class1.Length + "+" + 2 + ")\n";
                equation += "(" + bagWordsCount[1, i] + "+" + 1 + ")/(" + class2.Length + "+" + 2 + ")\n";
                double probForClass1 = (bagWordsCount[0, i] + 1) / (double)(class1.Length + 2);
                double probForClass2 = (bagWordsCount[1, i] + 1) / (double)(class2.Length + 2);
                //Console.WriteLine(probForClass1 + "\t" + probForClass2);
                probKeeper[bagOfWords[i]] = new double[2] { probForClass1, probForClass2 };

            }
            Console.WriteLine(equation);
        }


        public double getProbOfWordClass1(string word)
        {
            return (bagOfWords.Contains(word) ? probKeeper[word][0] : 1);

        }
        public double getProbOfWordClass2(string word)
        {
            return (bagOfWords.Contains(word) ? probKeeper[word][1] : 1);

        }
        public double getClass1Proir()
        {
            return poriorProb[0];
        }
        public double getClass2Proir()
        {
            return poriorProb[1];
        }
        public string class1Equation(string word)
        {
            bagOfWords.ToList();

            //System.Windows.Forms.MessageBox.Show(bagOfWords.ToList().IndexOf(word)+"");
            // System.Windows.Forms.MessageBox.Show(word);
            return "(" + word + "|Sport) (" + bagWordsCount[0, bagOfWords.ToList().IndexOf(word)] + "+" + 1 + ")/(" + class1.Length + "+" + 2 + ")";

        }

        public string class2Equation(string word)
        {


            return "(" + word + "|Tech) (" + bagWordsCount[1, bagOfWords.ToList().IndexOf(word)] + "+" + 1 + ")/(" + class2.Length + "+" + 2 + ")";

        }
    }
}
