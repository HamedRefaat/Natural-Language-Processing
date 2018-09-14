using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NLP_Task
{
  public  class charngram
    {
      string rawTraining = "i will go to prepare my papers to be prepared. This is what i do for living.";
           string  rawTest = "warning preprocessing will kill you";
           int v;
           string test;
           string traning;

      public double charNgarm(int n, out List<string> eq, out List<string> numeq)
      {
          traning = rawTraining.Replace(".", " ");
          traning = rawTraining.Replace("\\s+", " ");
          //test preprocessing
          test = rawTest.Replace(".", " ");
          test = rawTest.Replace("\\s+", " ");
          v = traning.Distinct().Count();

          eq = new List<string>();
          numeq = new List<string>();
          double prob = 1;
          int upCount, downCount;
          for (int i = 0; i < test.Length - n; i++)
          {

              StringBuilder b = new StringBuilder();
              b.Append(test.Substring(i, n).Reverse().ToArray());


              eq.Add("P(" + test.Substring(i, n - 1)[0] + "|" + b.ToString().Substring(0,b.Length-1) + ")");
              upCount = Regex.Matches(traning, test.Substring(i, n)).Count;
              downCount = Regex.Matches(traning, test.Substring(i, n - 1)).Count;

              numeq.Add("(" + (upCount + 1) + "/" + (downCount + v) + ")");
              prob *= (double)(upCount + 1) / (downCount + v);
          }

          return (prob);

      }
    }
}
