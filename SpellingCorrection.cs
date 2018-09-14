using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NLP_Task
{
    public class SpellingCorrection
    {
        private static Dictionary<String, int> _dictionary = new Dictionary<String, int>();
        private static Regex _wordRegex = new Regex("[a-z'A-Z]+", RegexOptions.Compiled);
              static SpellingCorrection()
        {
            string directory = Directory.GetCurrentDirectory() + "\\bigtext.txt";
            string fileContent = File.ReadAllText(directory);
            
            foreach (Match word in _wordRegex.Matches(fileContent))
            {
                string trimmedWord = word.Value.Trim().ToLower();
                {
                    if (_dictionary.ContainsKey(trimmedWord))
                        _dictionary[trimmedWord]++;
                    else
                        _dictionary.Add(trimmedWord, 1);
                }
            }
        }
        Dictionary<string, int> candidates;
        public string[] getallcandidates(string word)
        {
            if (string.IsNullOrEmpty(word))
                return new string[]{word};

            word = word.ToLower();

            // known()
            if (_dictionary.ContainsKey(word))
            {
                candidates = new Dictionary<string, int>();
                candidates.Add(word, _dictionary[word]);
                return new string[] { word };
            }
            List<String> nearlist = Edits(word);
             candidates = new Dictionary<string, int>();

             foreach (string wordVariation in nearlist)
             {
                 if (_dictionary.ContainsKey(wordVariation) && !candidates.ContainsKey(wordVariation))
                     candidates.Add(wordVariation, _dictionary[wordVariation]);
             }

            if (candidates.Count > 0)
                return candidates.Keys.ToArray();
            
           return new string[] { word };
        
        }
        public string Correct(string word)
        {
            if (candidates != null && candidates.Count>0)
                return candidates.OrderByDescending(x => x.Value).First().Key;
            else
                return word;
        }

        private List<string> Edits(string word)
        {
            var splits = new List<Tuple<string, string>>();
            var transposes = new List<string>();
            var deletes = new List<string>();
            var replaces = new List<string>();
            var inserts = new List<string>();

            // Splits
           //Korrect
            for (int i = 0; i < word.Length; i++)
            {   //korrect 
                // k orrect
                // ko rrect
                // kor rect
                // korr ect
                // korre ct
                // korrec t
             
                 var tuple = new Tuple<string, string>(word.Substring(0, i), word.Substring(i));
                splits.Add(tuple);
            }

            // Deletes
            for (int i = 0; i < splits.Count; i++)
            {
                string a = splits[i].Item1;
                string b = splits[i].Item2;
                if (!string.IsNullOrEmpty(b))
                {
                    
                    deletes.Add(a + b.Substring(1));
                    //orrect
                    //krrect
                    //korect
                    //korect
                    //korret
                    //korrec
                    

                }
            }

            // Transposes
            for (int i = 0; i < splits.Count; i++)
            {
                string a = splits[i].Item1;
                string b = splits[i].Item2;
                if (b.Length > 1)
                {
                     transposes.Add(a + b[1] + b[0] + b.Substring(2));
                }
            }

            // Replaces
            for (int i = 0; i < splits.Count; i++)
            {
                string a = splits[i].Item1;
                string b = splits[i].Item2;
                if (!string.IsNullOrEmpty(b))
                {
                    for (char c = 'a'; c <= 'z'; c++)
                    {
                       replaces.Add(a + c + b.Substring(1));
                    }
                }
            }

            // Inserts
            for (int i = 0; i < splits.Count; i++)
            {
                string a = splits[i].Item1;
                string b = splits[i].Item2;
                for (char c = 'a'; c <= 'z'; c++)
                {
                    
                    inserts.Add(a + c + b);
                }
            }

            return deletes.Union(transposes).Union(replaces).Union(inserts).ToList();
            
        }
    }
}
