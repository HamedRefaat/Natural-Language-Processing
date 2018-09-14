using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NLP_Task
{

    public class Stemmer
    {
        string Word{get; set;}
        StreamReader reader;
        List<string> prefix;
        List<string> suffix;
        public Stemmer()
        {
            fill();
        }
        private void fill()
        {
            reader = new StreamReader(Directory.GetCurrentDirectory().Replace("bin\\Debug", "prefix-suffix\\Prefix.txt"));

           
            prefix = new List<string>();
            suffix = new List<string>();
            string line;
            while ((line=reader.ReadLine()) != null)
            {
                if(line!=string.Empty)
                prefix.Add(line);
            }
            reader.Close();
            line = string.Empty;
            reader = new StreamReader(Directory.GetCurrentDirectory().Replace("bin\\Debug", "prefix-suffix\\suffix.txt"));
            while ((line = reader.ReadLine()) != null)
            {
                if (line != string.Empty)
                    suffix.Add(line);
            }
            reader.Close();
        }

        string removprefix(string word)
        {
           // MessageBox.Show(word.Length.ToString());
            foreach (string pre in prefix)
            {
                if (word.StartsWith(pre)&&word.Length-pre.Length>2)
                {
                    word = word.Remove(0, pre.Length);
                    break;
                }
            }
            return word;
        }
        string removesuffix(string word)
        {
            foreach (string  suf in suffix)
            {
                if (word.EndsWith(suf)&&word.Length-suf.Length>2)
                {

                    word = word.Remove(word.Length - suf.Length);
                    break;
                }
                
            }
            return word;
        }
        public string Stem(string word)
        {
            word = removprefix(word);
            return removesuffix(word);
        
        }
      
        
    }
}
