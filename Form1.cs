using LemmaSharp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NLP_Task
{
    public partial class Form1 : Form
    {
        Dictionary<string, int> Wordes;
        double phasepro = 1;
        float count = 0;
        string[] Token_wordes;
        string[] bagofwords = new string[] { "Chelsea", "St-Germain", "season", "Manchester", "Dynamo", "Kiev", "Roma", "Real", "Juventus", "Bayern", "matches", "winners", "Arsenal", "Barcelona", "final", "champions", "goals", "champion", "boxer", "boxing", "Liverpool", "Jurgen", "Klopp", "Augsburg", "Tottenham", "Fiorentina", "Valencia", "clubs", "points", "Atletico", "Villarreal", "scored", "Modric", "Soldado", "Cristiano", "Ronaldo", "United", "Nicola", "Adams", "chance", "Championship", "FIFA", "Cup", "Premier", "Tennis", "Serena", "Williams", "Grand", "Slam", "winner", "player", "players", "WTA", "stadium", "Football","Basketball", "NBA", "Athletics", "FIBA ", "MVP", "team's", "FIVB ", "Volleyball", "sport", "Olympic", "yards", "UEFA", "Benfica", "UFC Featherweight", "UFC", "Messi", "assists", "leagues", "Facebook", "Snapchat", "computer", "virus", "mobile", "internet", "robots", "technologies", "Twitter", "hacking", "hackers", "email", "websites", "smartphone", "Samsung", "iPhone", "software", "programs", "Wi-Fi", "phones", "Artificial", "intelligence", "Yahoo", "games", "screen", "TV", "Nasa", "Nasa's", "Google", "BlackBerry", "Android", "HTC", "Camera", "Microsoft", "Sony", "PlayStation", "Xbox", "iOS", "Apple", "app", "Cloud", "Cisco", "IBM" };

        public Form1()
        {
            InitializeComponent();
        }

        private void countWordsInFile(string file,string pattren, Dictionary<string, int> words)
        {
            var content = "";
         
                content = file.ToLower();

            string pattern = pattren;
           
            var wordPattern = new Regex(pattern);


            int i = 0;
            count = (float)wordPattern.Matches(content).Count;
            Token_wordes = new string[(int)count];
            //   Token_wordes[0] = "sss";
            foreach (Match match in wordPattern.Matches(content))
            {


                Token_wordes[i++] = match.Value;

                int currentCount = 0;

                words.TryGetValue(match.Value, out currentCount);

                currentCount++;
                words[match.Value] = currentCount;


            }
            //       Wordes["sss"] = 1;
            //       Wordes["eee"] = 1;

            //      Token_wordes[i] = "eee";

        }


        private void countWordsInFile(string file, Dictionary<string, int> words)
        {
            var content = "";
            if (chkcasesensitive.Checked)
                content = file;
            else
                content = file.ToLower();

            string pattern = "";
            if (checkBox2.Checked)
                pattern = @"\w+";
            else
                pattern = @"[a-zA-Z'.]+";
            var wordPattern= new Regex(pattern);


            int i = 0;
            count = (float)wordPattern.Matches(content).Count;
            Token_wordes = new string[(int)count];
         //   Token_wordes[0] = "sss";
            foreach (Match match in wordPattern.Matches(content))
            {


                Token_wordes[i++] = match.Value;

                int currentCount = 0;

                words.TryGetValue(match.Value, out currentCount);

                currentCount++;
                words[match.Value] = currentCount;


            }
     //       Wordes["sss"] = 1;
     //       Wordes["eee"] = 1;

      //      Token_wordes[i] = "eee";

        }
        Thread upgui;
    
        private void button1_Click(object sender, EventArgs e)
        {
            int i = 0;

            dataGridView1.Invoke(new Action(delegate { dataGridView1.Rows.Clear(); }));
            Wordes = new Dictionary<string, int>();
            Invoke(new Action(delegate { countWordsInFile(rtxtfile.Text, Wordes); }));

            foreach (KeyValuePair<string, int> item in Wordes)
            {
                upgui = new Thread(new ThreadStart(new Action(delegate
                {

                    dataGridView1.Invoke(new Action(delegate
                    {
                        dataGridView1.FirstDisplayedScrollingRowIndex = dataGridView1.RowCount - 1;
                    }));
                }))); //upgui.Start();
                Invoke(new Action(delegate
                {
                    dataGridView1.Rows.Add(new object[] { ++i, item.Key, item.Value, (float)(item.Value / count) });


                }));


            }
            btnngram.Invoke(new Action(delegate { btnngram.Visible = true; }));
            txtngram.Invoke(new Action(delegate { txtngram.Visible = true; }));
            //   MessageBox.Show("Done.!!!");
            MessageBox.Show(Token_wordes.Length.ToString());
            th1.Abort();
           
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
           // button2_Click(sender, e);
        }

        private void button1_Click_2(object sender, EventArgs e)
        {
            
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            tabControl1.ItemSize = new Size((this.Width - 20) / tabControl1.TabCount, 80);
        }

        void p2(string[] tokens,bool full)
        {
            StringBuilder searchword = new StringBuilder();
            StringBuilder searchwor2 = new StringBuilder();
            for (int i = tokens.Length-1; i >= 0; i--)
            {
                searchword.Append(tokens[i]);
                if(i!=0)
                searchwor2.Append(tokens[i]);
                searchword.Append(" ");
            }
            int up = countword(searchword.ToString());
            int dw = 0;
            try
            {
                dw = Wordes[tokens[0]];
            }
            catch { dw = countword(searchwor2.ToString()); }

            double pro = (double)(up + 1) / (double)(dw + dataGridView1.Rows.Count);
            if (full)
            {

                int len = tokens.Length-1;
                StringBuilder contex=new StringBuilder();
                StringBuilder appear = new StringBuilder();
                appear.Append("P( ");
                appear.Append(tokens[0]);
                appear.Append(" | ");
                contex.Append(tokens[len]);
                for (int i = 1; i < tokens.Length-1; i++)
                {

                    appear.Append(tokens[i]+" , ");
                    contex.Append(" ");
                    if (tokens[len - i]==null)
                        break;
                    contex.Append(tokens[len-i]);


                }
                appear.Append(tokens[len]);
                contex.Append(" "+tokens[0]);
                appear.Append(" )");
                dataGridView2.Rows.Add(new object[] { ++ind, appear.ToString(), contex.ToString(), pro });
                //   dataGridView2.FirstDisplayedScrollingRowIndex = dataGridView2.RowCount-1;
            }
            else
                phasepro *= pro;
            
        }
        void P(string wi, string wi_1,bool full)
        {
            int up = countword(wi_1 + " " + wi);
        //    MessageBox.Show(wi_1 + " " + wi+"  ==> "+up);
            int dw = 0;
            try
            {
                dw = Wordes[wi_1];
            }
            catch { dw = 0; }
            
            double pro = (double)(up+1) / (double)(dw+dataGridView1.Rows.Count);
            if (full)
            {
                dataGridView2.Rows.Add(new object[] { ++ind, "P(" + wi + " | " + wi_1 + " )", wi_1 + " " + wi, pro });
             //   dataGridView2.FirstDisplayedScrollingRowIndex = dataGridView2.RowCount-1;
            }
            else
                phasepro *= pro;

        }

        int countword(string wordes)
        {
            var wordPattern = new Regex(@wordes);

            return wordPattern.Matches(rtxtfile.Text).Count;
        }
        int countword(string wordes, string file)
        {
            var wordPattern = new Regex(@wordes);

            return wordPattern.Matches(file).Count;
        
        }
         string[] Gneratwordgivenbynwordes(int currentindexofword,int N)
        {
            string[] result = new string[N];
            result[0] = Token_wordes[currentindexofword];
            if (N <= (currentindexofword))
            {
                for (int i = 1; i < N; i++)
                {
                    result[i] = Token_wordes[currentindexofword - i];
                }
            }
            else
            {
                result = null;
            
            }
             


            return result;
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            int n=int.Parse(txtngram.Text);
            dataGridView2.Invoke(new Action(delegate { dataGridView2.Rows.Clear(); }));
            for (int i = 1; i < Token_wordes.Length; i++)
            {
                string[] tokens=Gneratwordgivenbynwordes(i,n);
                if(tokens!=null)
                Invoke(new Action(delegate { p2(tokens,true); }));

            }
                        th2.Abort();
        }

        private void button1_Click_3(object sender, EventArgs e)
        {
            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            phasepro = 1;
            string input = "sss "+txtsearch.Text+" eee";
            string[] tokens = input.Split(' ');
            
            for (int i = 1; i < tokens.Length; i++)
            {
                P(tokens[i], tokens[i - 1], false);
            }
            lablresult.Text = phasepro.ToString();
        }
        Thread th1;
        private void button1_Click_4(object sender, EventArgs e)
        {
            
           
        }
        int ind;
        Thread th2;
        private void button3_Click_2(object sender, EventArgs e)
        {
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            btnngram.Visible = false;
            txtngram.Visible = false;
            int wed=(int)((this.Width-20) / tabControl1.TabCount);
            tabControl1.ItemSize = new Size(wed, 80);
            
        }

        private void Form1_DragDrop(object sender, DragEventArgs e)
        {
            if ((e.X >= rtxtfile.Location.X && e.X <= rtxtfile.Width) && (e.Y <= rtxtfile.Location.Y && e.Y <= rtxtfile.Height))
            { 
          
            
            }
        }

        private void btnbrowes_Click(object sender, EventArgs e)
        {
            openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "text files(.txt)|*.txt";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                rtxtfile.Clear();
                string line;
                txtpath.Text = openFileDialog1.FileName;
                StreamReader file = new System.IO.StreamReader(openFileDialog1.FileName);
                while ((line = file.ReadLine()) != null)
                {
                    line = line.Trim();
                    if (line != "")
                        rtxtfile.AppendText("sss " + line.ToLower() + " eee\n");

                }

                file.Close();



            }
        }

        private void btnunigram_Click(object sender, EventArgs e)
        {
            th1 = new Thread(new ThreadStart(new Action(delegate { button1_Click(sender, e); })));
            th1.Start();
            
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void btnngram_Click(object sender, EventArgs e)
        {
            groupBox4.Hide();
            ind = 0;
            th2 = new Thread(new ThreadStart(new Action(delegate { button3_Click_1(sender, e); })));

            th2.Start();
        }

        private void groupBox8_Enter(object sender, EventArgs e)
        {
           
        }

        public int Compute(string s, string t, out int[,] result,out string[]w1,out string[]w2,char type='c')
        {
             int n ,m;
             string[] stokens=null, ttokens=null;
             if (type == 'c')
             {
                 n = s.Length;
                 m = t.Length;
                 w1 = new string[s.Length];

                 w2 = new string[t.Length];
                 for (int i = 0; i < ((s.Length>=t.Length)?s.Length:t.Length); i++)
                 {
                     if (i < w1.Length)
                         w1[i] = s[i].ToString();
                     if (i < w2.Length)
                         w2[i] = t[i].ToString();
                 }
           
             }
             else
             { 
              stokens=s.Split(' ');
             ttokens = t.Split(' ');
             n = stokens.Length;
             m = ttokens.Length;
             w1 = stokens;
             w2 = ttokens;
             
             }
             result = new int[n + 1, m + 1];
             
            // Step 1
            if (n == 0)
            {
                return m;
            }

            if (m == 0)
            {
                return n;
            }

            // Step 2
            for (int i = 0; i <= n; result[i, 0] = i++)
            {

            }

            for (int j = 0; j <= m; result[0, j] = j++)
            {
            }

            // Step 3
            for (int i = 1; i <= n; i++)
            {
                //Step 4

                for (int j = 1; j <= m; j++)
                {
                    // Step 5
                    int cost;
                    if(type=='c')
                        cost = (t[j - 1] == s[i - 1]) ? 0 : 2;
                    else
                        cost = (ttokens[j - 1] == stokens[i - 1]) ? 0 : 2;

                    // Step 6
                    result[i, j] = Math.Min(
                        Math.Min(result[i - 1, j] + 1, result[i, j - 1] + 1),
                        result[i - 1, j - 1] + cost);
                    //         Console.Write(result[i,j]+"  ");
                }
                //   Console.WriteLine("\n");
            }
            // Step 7
            //   Console.WriteLine("\n\n\n");
            return result[n, m];
        }

        string[] w1, w2;
        private void button1_Click_5(object sender, EventArgs e)
        {

            dataGridView3.Rows.Clear();
            string st1 = textBox1.Text.ToUpper();
            string st2 = textBox2.Text.ToUpper();
            int[,] res;
           
            if(checkBox1.Checked)
                Compute(st1, st2, out res,out w1,out w2,'s');
            else
                Compute(st1, st2, out res,out w1,out w2);
            dataGridView3.ColumnCount = res.GetLength(1)+1;
            //dataGridView3.ColumnHeadersHeight = 40;
          
            
            for (int i = 0; i < res.GetLength(0); i++)
            {
                DataGridViewRow r = new DataGridViewRow();
                if (checkBox1.Checked)
                    r.Height = 60;
                else
                    r.Height = 40;
                DataGridViewTextBoxCell cc = new DataGridViewTextBoxCell();
                if (i < w1.Length)
                {
                    cc.Value = w1[w1.Length - 1 - i];
                    cc.Style.BackColor = Color.Yellow;
                }
                else
                {
                    cc.Value = "#";
                    cc.Style.BackColor = Color.Gray;
                }
                if(checkBox1.Checked)
                cc.Style.Font = new Font("Times New Roman", 12,FontStyle.Bold);
                else
                    cc.Style.Font = new Font("Times New Roman", 25, FontStyle.Bold);
                
                
                r.Cells.Add(cc);
                for (int j = 0; j < res.GetLength(1); j++)
                {
                    if (checkBox1.Checked)
                    {
                        dataGridView3.Columns[j].Width = 100;
                        dataGridView3.Columns[res.GetLength(1)].Width = 100;

                        
                    }
                    else
                    {

                        
                        dataGridView3.Columns[j].Width = 60;
                        dataGridView3.Columns[res.GetLength(1)].Width = 60;
                   
                    }
            
                    DataGridViewTextBoxCell c = new DataGridViewTextBoxCell();
                    
                    c.Value = res[res.GetLength(0)-1-i, j].ToString();
                   
                    r.Cells.Add(c);

                }
                dataGridView3.Rows.Add(r);
            }
            DataGridViewRow r2 = new DataGridViewRow();
            r2.Height = 50;
            DataGridViewTextBoxCell c1 = new DataGridViewTextBoxCell();
            c1.Value = "";
            c1.Style.BackColor = dataGridView3.BackgroundColor;
            r2.Cells.Add(c1);
            DataGridViewTextBoxCell c2 = new DataGridViewTextBoxCell();
            c2.Value = "#";
            c2.Style.BackColor = Color.Gray;
            if(checkBox1.Checked)
            c2.Style.Font = new System.Drawing.Font("Times New Roman", 12, FontStyle.Bold);
            else
                c2.Style.Font = new System.Drawing.Font("Times New Roman", 25, FontStyle.Bold);
            r2.Cells.Add(c2);
            for (int i = 0; i < dataGridView3.ColumnCount-2; i++)
            {
                 DataGridViewTextBoxCell c = new DataGridViewTextBoxCell();
                 if (i < w2.Length)
                     c.Value = w2[i];
                if(checkBox1.Checked)
                 c.Style.Font = new System.Drawing.Font("Times New Roman", 12, FontStyle.Bold);
                else
                    c.Style.Font = new System.Drawing.Font("Times New Roman", 25, FontStyle.Bold);
                 c.Style.BackColor = Color.Yellow;
                r2.Cells.Add(c);
                         
            
                
            }
            dataGridView3.Rows.Add(r2);
            dataGridView3.Rows[0].Cells[dataGridView3.ColumnCount - 1].Style.BackColor = Color.Green;
            dataGridView3[0, 0].Selected = false;
            dataGridView3.FirstDisplayedScrollingRowIndex = 0;
            dataGridView3.FirstDisplayedScrollingColumnIndex = dataGridView3.ColumnCount-1;
        }

       

       

        private void button2_Click_2(object sender, EventArgs e)
        {
            openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "text files(.txt)|*.txt";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                richTextBox1.Clear();
             //   string line;
            //    txtpath.Text = openFileDialog1.FileName;
                StreamReader file = new System.IO.StreamReader(openFileDialog1.FileName);
                //while ((line = file.ReadLine()) != null)
                //{
                //    line = line.Trim();
                //    if (line != "")
                //        richTextBox1.AppendText("sss " + line.ToLower() + " eee\n");

                //}
                richTextBox1.Text = file.ReadToEnd().TrimStart();

                file.Close();



            }
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            
        }

        private void btnstem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                txtstem.Clear();
                StreamReader reader = new StreamReader(openFileDialog1.FileName);
             //   string line;
                //while ((line = reader.ReadLine()) != null)
                //{
                //    txtstem.AppendText(line);
                
                //}
                txtstem.Text = reader.ReadToEnd().TrimStart();
            
            }
          
        }

        
        private void btnstem_Click_1(object sender, EventArgs e)
        {
            dataGridView4.Rows.Clear();
            
            Stemmer stem = new Stemmer();
            var _wordes = new Regex("[a-zA-Z-]+");
            int i = 0;
            Dictionary<string, int> worddict = new Dictionary<string, int>();
            foreach (Match item in _wordes.Matches(txtstem.Text))
            {


                if (worddict.ContainsKey(item.ToString().ToLower()))
                    continue;
                worddict.Add(item.ToString().ToLower(), 1);
                string stemresult = stem.Stem(item.ToString().ToLower());
          //      if (stemresult == "" || stemresult.Length <= 2||stemresult==item.ToString().ToLower())
            //        continue;
                dataGridView4.Rows.Add(new object[] { ++i, item, stemresult });
                    dataGridView4.Rows[i - 1].Height = 40;
                
            }
           
        }

        private void tabPage1_Enter(object sender, EventArgs e)
        {
            this.Text = (sender as TabPage).Text;
        }

        private void btnbacktrack_Click(object sender, EventArgs e)
        {
            
            int r = 0;
            int c = dataGridView3.ColumnCount - 1;
            var orignalcell = dataGridView3.Rows[r].Cells[c];
            var leftcell = dataGridView3.Rows[r].Cells[c - 1];
            var dowencell = dataGridView3.Rows[r + 1].Cells[c];
            var diagonalcell = dataGridView3.Rows[r + 1].Cells[c - 1];
         
            for (int i = 0; i < dataGridView3.RowCount-1; i++)
            {
               
                if (r < dataGridView3.RowCount - 2||c>1)
                {


                    if (r < dataGridView3.RowCount - 2 && c > 1 && int.Parse(orignalcell.Value.ToString()) == int.Parse(diagonalcell.Value.ToString()) && dataGridView3.Rows[orignalcell.RowIndex].Cells[0].Value.ToString() == dataGridView3.Rows[dataGridView3.RowCount - 1].Cells[orignalcell.ColumnIndex].Value.ToString() && orignalcell.Style.BackColor == Color.Green)
                    {
                      //  MessageBox.Show("Test");
                        diagonalcell.Style.BackColor = Color.Green;
                        orignalcell = diagonalcell;
                        leftcell = dataGridView3.Rows[orignalcell.RowIndex].Cells[orignalcell.ColumnIndex - 1];
                        dowencell = dataGridView3.Rows[orignalcell.RowIndex + 1].Cells[orignalcell.ColumnIndex];
                        diagonalcell = dataGridView3.Rows[orignalcell.RowIndex + 1].Cells[orignalcell.ColumnIndex - 1];
                        dataGridView3.FirstDisplayedScrollingRowIndex = r;
                        dataGridView3.FirstDisplayedScrollingColumnIndex = c;
                    }
                    else if (r < dataGridView3.RowCount - 2 && c > 1 && int.Parse(orignalcell.Value.ToString()) - int.Parse(diagonalcell.Value.ToString()) == 2 && dataGridView3.Rows[orignalcell.RowIndex].Cells[0].Value.ToString() != dataGridView3.Rows[dataGridView3.RowCount - 1].Cells[orignalcell.ColumnIndex].Value.ToString() && orignalcell.Style.BackColor == Color.Green)
                    {
                        diagonalcell.Style.BackColor = Color.Green;
                        orignalcell = diagonalcell;
                        leftcell = dataGridView3.Rows[orignalcell.RowIndex].Cells[orignalcell.ColumnIndex - 1];
                        dowencell = dataGridView3.Rows[orignalcell.RowIndex + 1].Cells[orignalcell.ColumnIndex];
                        diagonalcell = dataGridView3.Rows[orignalcell.RowIndex + 1].Cells[orignalcell.ColumnIndex - 1];
                        dataGridView3.FirstDisplayedScrollingRowIndex = r;
                        dataGridView3.FirstDisplayedScrollingColumnIndex = c;
                    }
                    else
                    {
                        if (c > 1 && r < dataGridView3.RowCount - 2)
                        {
                            orignalcell = (int.Parse(leftcell.Value.ToString()) <= int.Parse(dowencell.Value.ToString())) ? leftcell : dowencell;
                            dataGridView3.FirstDisplayedScrollingRowIndex = r;
                            dataGridView3.FirstDisplayedScrollingColumnIndex = c;
                        }
                        else if (c > 1)
                        {
                            orignalcell = leftcell;
                           
                            //dataGridView3.FirstDisplayedScrollingColumnIndex = c;
                        }
                        else
                            orignalcell = dowencell;
                        if (orignalcell == leftcell)
                        {
                            r--;
                        }
                        else
                            c++;
                        orignalcell.Style.BackColor = Color.Green;
                        leftcell = dataGridView3.Rows[orignalcell.RowIndex].Cells[orignalcell.ColumnIndex - 1];
                        dowencell = dataGridView3.Rows[orignalcell.RowIndex + 1].Cells[orignalcell.ColumnIndex];
                        diagonalcell = dataGridView3.Rows[orignalcell.RowIndex + 1].Cells[orignalcell.ColumnIndex - 1];
                         i--; 
                    }

                  
                }
                r++; c--;
                
            }
            dataGridView3.Rows[dataGridView3.RowCount - 2].Cells[1].Style.BackColor = Color.Green;
            dataGridView3.FirstDisplayedScrollingRowIndex = dataGridView3.RowCount - 1;
            dataGridView3.FirstDisplayedScrollingColumnIndex = 0;
        }

        private void btnbacktrack_MouseHover(object sender, EventArgs e)
        {
            (sender as Button).BackColor = Color.Yellow;
;
        }

        private void btnbacktrack_MouseLeave(object sender, EventArgs e)
        {
            (sender as Button).BackColor = Color.Silver;
        }
        public void ZoomGrid(float f)
        {
            

            dataGridView3.Font = new System.Drawing.Font("Arial",10*f);
            dataGridView3.Scale(new SizeF(f, f));
            //dataGridView3.Font = new Font(dataGridView3.Font.FontFamily,
              //                            dataGridView3.Font.Size * f, dataGridView3.Font.Style);
            dataGridView3.RowTemplate.Height = (int)(dataGridView3.RowTemplate.Height * f);
           
            foreach (DataGridViewColumn col in dataGridView3.Columns)
            {
                
                col.Width = (int)(col.Width * f);
            }
        }
        private void button5_Click(object sender, EventArgs e)
        {
            ZoomGrid(.98f);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGridView3.RowCount; i++)
            {
                dataGridView3.Rows[i].Height = 13;
                for (int j = 0; j < dataGridView3.ColumnCount; j++)
                {
                    dataGridView3.Columns[j].Width = 22;
                    try
                    {
                        
                        //dataGridView3.DefaultCellStyle.Font = new Font("Times New Roman", 10);
                        dataGridView3.Rows[i].Cells[j].Style.Font = new Font("Times New Roman", 8);
                    }
                    catch { continue; }
                    }
            }

            ZoomGrid(1.02f);
        }

        void searchregx(string pattren)
        {
            listBox1.Items.Clear();
            var matchs = new Regex(pattren);
            foreach (Match item in matchs.Matches(richTextBox1.Text))
            {
                listBox1.Items.Add(item);
            }
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

            /*
              Word
              E-mail
              Money
              N-ID
              Number
              All Started with Cabital
              word.word
             */
            string pattren = "";
            switch (comboBox1.SelectedIndex)
            {

                case 0: textBox3.Visible = true; textBox3.Text = (textBox3.Text == "") ? "Word" : textBox3.Text; char ch = textBox3.Text[0];  pattren = "[" + ch.ToString().ToLower()+ch.ToString().ToUpper()+"]"+textBox3.Text.Remove(0, 1); break;
                case 1: pattren = @"[a-zA-Z0-9._]+@[a-zA-Z]{3,}\.[a-zA-Z]{2,}"; break;
                case 2: pattren = @"\d+\.?\d+\$"; break;
                case 3: pattren = @"\b[0-9]{14}\b"; break;
                case 4: pattren = @"\d+\.?\d+"; break;
                case 5: pattren = @"[A-Z][a-z]+"; break;
                case 6: pattren = @"[a-zA-Z]+\.[a-zA-Z]+"; break;
                   

                default:
                    break;
            }
            searchregx(pattren);
        }

        private void dataGridView3_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
           /* 
            {
                Rectangle newRect = new Rectangle(e.CellBounds.X + 1,
                    e.CellBounds.Y + 1, e.CellBounds.Width - 4,
                    e.CellBounds.Height - 4);

                using (
                    Brush gridBrush = new SolidBrush(this.dataGridView1.GridColor),
                    backColorBrush = new SolidBrush(e.CellStyle.BackColor))
                {
                    using (Pen gridLinePen = new Pen(gridBrush))
                    {
                        // Erase the cell.
                        e.Graphics.FillRectangle(backColorBrush, e.CellBounds);

                        // Draw the grid lines (only the right and bottom lines;
                        // DataGridView takes care of the others).
                        e.Graphics.DrawLine(gridLinePen, e.CellBounds.Left,
                            e.CellBounds.Bottom - 1, e.CellBounds.Right - 1,
                            e.CellBounds.Bottom - 1);
                        e.Graphics.DrawLine(gridLinePen, e.CellBounds.Right - 1,
                            e.CellBounds.Top, e.CellBounds.Right - 1,
                            e.CellBounds.Bottom);

                        // Draw the inset highlight box.
                        e.Graphics.DrawRectangle(Pens.Blue, newRect);

                        // Draw the text content of the cell, ignoring alignment.
                        if (e.Value != null)
                        {
                            e.Graphics.DrawString((String)e.Value, e.CellStyle.Font,
                                Brushes.Crimson, e.CellBounds.X + 2,
                                e.CellBounds.Y + 2, StringFormat.GenericDefault);
                        }
                        e.Handled = true;
                    }
                }
            }*/

        }

        private void button3_Click_3(object sender, EventArgs e)
        {
            dataGridView4.Rows.Clear();
            int i = 0;
            var _wordes = new Regex("[a-zA-Z-]+");
            PorterStemmer proter = new PorterStemmer();
            Dictionary<string, int> worddict = new Dictionary<string, int>();
            foreach (Match item in _wordes.Matches(txtstem.Text))
            {


                if (worddict.ContainsKey(item.ToString().ToLower()))
                    continue;
                worddict.Add(item.ToString().ToLower(), 1);
                proter.add(item.Value.ToLower().ToCharArray(), item.Value.Length);
                proter.stem();

                dataGridView4.Rows.Add(new object[] { ++i, item,proter.ToString()  });
                dataGridView4.Rows[i - 1].Height = 40;

            }


        }

        private void btnpro_Click(object sender, EventArgs e)
        {
            button4_Click(sender, e);
        }

        private void button4_Click_2(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string line;
                richTextBox2.Clear();
                StreamReader reader = new StreamReader(openFileDialog1.FileName);
                while ((line=reader.ReadLine())!=null)
                {
                    if(line!=" ")
                    richTextBox2.AppendText(line);
                }

            }
 
        }


        Thread t2;
        private void button8_Click(object sender, EventArgs e)
        {
            richTextBox3.Clear();
            string[] lines = richTextBox2.Text.Split(new string[] {" . " },StringSplitOptions.None);
            t2 = new Thread(new ThreadStart(new Action(delegate {
                string normalline1 = "";
                string normalline = "";
                foreach (string line in lines)
                {

                 normalline1=line.Replace("<h>", "");
                 normalline = normalline1.Replace("<p>","");
                 richTextBox3.Invoke(new Action(delegate { richTextBox3.AppendText(normalline); richTextBox3.AppendText("\n"); }));
                }
            })));
            t2.Start();
        }
        Thread t1;
        private void button7_Click(object sender, EventArgs e)
        {
            richTextBox3.Clear();
            string[] lines = richTextBox2.Text.Split(new string[] { "<h>" }, StringSplitOptions.None);
             t1 = new Thread(new ThreadStart(new Action(delegate
            {
                string normalline1 = "";
                string normalline = "";
                foreach (string line in lines)
                {

                    normalline1 = line.Replace("<h>", "");
                    normalline = normalline1.Replace("<p>", "");
                    richTextBox3.Invoke(new Action(delegate { richTextBox3.AppendText(normalline); richTextBox3.AppendText("\n\n\n   "); }));
                }
            })));
            t1.Start();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (th1!=null&&th1.IsAlive)
                th1.Abort();
            if (th2 != null && th2.IsAlive)
                th2.Abort();
            if (t1 != null && t1.IsAlive)
                t1.Abort();
            if (t2 != null && t2.IsAlive)
                t2.Abort();
           
        }

        private void button9_Click(object sender, EventArgs e)
        {
           
            if (richTextBox4.Text != string.Empty)
            {
                string[] candidates = null;
                dataGridView5.Rows.Clear();
                int i=0;
                string[] Words = richTextBox4.Text.Split(' ');
                foreach (string word in Words)
                {
                    candidates = spellingcorrection.getallcandidates(word);
                    string corectedword = spellingcorrection.Correct(word);
                    dataGridView5.Rows.Add(new object[] { ++i, word, null,corectedword });
                    DataGridViewComboBoxCell owner = (DataGridViewComboBoxCell)dataGridView5.Rows[i-1].Cells[2];
                    foreach (string can in candidates)
                        owner.Items.Add(can);
                    
                    owner.Selected = true;
                   
                }
            }
        }
        SpellingCorrection spellingcorrection;
        private void tabPage6_Enter(object sender, EventArgs e)
        {
            spellingcorrection = new SpellingCorrection();
        }

        ILemmatizer lmtz = new LemmatizerPrebuiltFull(LemmaSharp.LanguagePrebuilt.English);
            
        private  static string LemmatizeOne(LemmaSharp.ILemmatizer lmtz, string word)
        {

            string wordLower = word.ToLower();
            string lemma = lmtz.Lemmatize(wordLower);

            //our output remove this but keep wordLower and lemma to be presented

            if (word.ToLower() != lemma.ToLower())
                return lemma;
            return string.Empty;
           
        }
        private void button11_Click(object sender, EventArgs e)
        {
            int ind = 0;
            string lemword = "";
            string[] tokens = richTextBox5.Text.Split(new char[] { ' ', ',', '.', ')', '(', '_', '-' },StringSplitOptions.RemoveEmptyEntries);
            foreach (string word in tokens)
            {
                lemword = LemmatizeOne(lmtz, word);
                if (lemword != string.Empty)
                {

                    dataGridView6.Rows.Add(new object[]{++ind,word,lemword});
                }
            }
        
        }

        private void button12_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                txtstem.Clear();
                StreamReader reader = new StreamReader(openFileDialog1.FileName);
                //   string line;
                //while ((line = reader.ReadLine()) != null)
                //{
                //    txtstem.AppendText(line);

                //}
                richTextBox5.Text = reader.ReadToEnd().TrimStart();

            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            dataGridView7.Rows.Clear();
            double p;
            charngram ch = new charngram();
            List<string> chars, pros;
            int n = int.Parse(textBox4.Text);
           p= ch.charNgarm(n, out chars, out pros);
            for (int i = 0; i < chars.Count; i++)
            {
                dataGridView7.Rows.Add(new object[]{i,chars[i],pros[i]});
            }
            MessageBox.Show(p.ToString());
        }

        private void button14_Click(object sender, EventArgs e)
        {
            dataGridView8.Rows.Clear();
            string test = txttst.Text.ToLower(); ;
            MultinomialNB train = new MultinomialNB();
            string[] tokens = test.Split(' ');
            List<string> acutaqltoken = new List<string>();
            foreach (string w in tokens)
            {
                if (bagofwords.Contains(w))
                    acutaqltoken.Add(w);
            }

            train.computePorior();
            train.allWordsInClass();
            train.calculateProb();
            double pro1 = train.getClass1Proir();
            int i = 0;
            foreach (string word in acutaqltoken)
            {


                string res = (train.class1Equation(word));

                pro1 *= train.getProbOfWordClass1(word);
                string eq = res.Split(' ')[1];
                dataGridView8.Rows.Add(new object[] { ++i, res.Split(' ')[0], eq });

            }
            double pro2 = train.getClass2Proir();
            foreach (string word in acutaqltoken)
            {


                string res = (train.class2Equation(word));

                pro2 *= train.getProbOfWordClass2(word);
                dataGridView8.Rows.Add(new object[] { ++i, res.Split(' ')[0], res.Split(' ')[1] });

            }

            lblpc1.Text = pro1 + "";
            lblpc2.Text = pro2 + "";
            lblclas.Text = (pro1 >= pro2) ? "Sport" : "Tech";
        }

        private void button15_Click(object sender, EventArgs e)
        {
            dataGridView8.Rows.Clear();
            string test = txttst.Text;
            test = RemoveDuplicateWords(test.ToLower());
            // MessageBox.Show(test);
            Binarized_Multinomial_NB train = new Binarized_Multinomial_NB();
            string[] tokens = test.Split(' ');
            List<string> acutaqltoken = new List<string>();

            foreach (string w in tokens)
            {
                if (bagofwords.Contains(w) && !acutaqltoken.Contains(w))
                    acutaqltoken.Add(w);
            }

            train.computePorior();
            train.allWordsInClass();
            train.calculateProb();
            double pro1 = train.getClass1Proir();
            //MessageBox.Show(pro1+"");
            int i = 0;
            foreach (string word in acutaqltoken)
            {


                string res = (train.class1Equation(word));

                pro1 *= train.getProbOfWordClass1(word);
                //  MessageBox.Show(train.getProbOfWordClass1(word)+"");
                dataGridView8.Rows.Add(new object[] { ++i, res.Split(' ')[0], res.Split(' ')[1] });

            }
            double pro2 = train.getClass2Proir();
            foreach (string word in acutaqltoken)
            {


                string res = (train.class2Equation(word));

                pro2 *= train.getProbOfWordClass2(word);
                dataGridView8.Rows.Add(new object[] { ++i, res.Split(' ')[0], res.Split(' ')[1] });

            }

            lblpc1.Text = pro1 + "";
            lblpc2.Text = pro2 + "";
            lblclas.Text = (pro1 >= pro2) ? "Sport" : "Tech";

        }

        private void button13_Click(object sender, EventArgs e)
        {
            dataGridView8.Rows.Clear();
            string test = txttst.Text;
            test = RemoveDuplicateWords(test.ToLower());
            // MessageBox.Show(test);
            Bernoulli_NB train = new Bernoulli_NB();
            string[] tokens = test.Split(' ');
            List<string> acutaqltoken = new List<string>();

            foreach (string w in tokens)
            {
                if (bagofwords.Contains(w) && !acutaqltoken.Contains(w))
                    acutaqltoken.Add(w);
            }

            train.computePorior();
            train.allWordsInClass();
            train.calculateProb();
            double pro1 = train.getClass1Proir();
            //MessageBox.Show(pro1+"");
            int i = 0;
            foreach (string word in acutaqltoken)
            {


                string res = (train.class1Equation(word));

                pro1 *= train.getProbOfWordClass1(word);
                //  MessageBox.Show(train.getProbOfWordClass1(word)+"");
                dataGridView8.Rows.Add(new object[] { ++i, res.Split(' ')[0], res.Split(' ')[1] });

            }
            List<string> notintest = new List<string>();

            foreach (var item in bagofwords)
            {
                if (!tokens.Contains(item))
                {
                    // Console.WriteLine(item);
                    notintest.Add(item);
                }
            }
            foreach (string word in notintest)
            {
                string res = (train.class1Equation(word));
                dataGridView8.Rows.Add(new object[] { ++i, res.Split(' ')[0], "1 -" + res.Split(' ')[1] });
                //Console.WriteLine(1 - train.getProbOfWordClass1(word));
                pro1 *= (1 - train.getProbOfWordClass1(word));
            }


            double pro2 = train.getClass2Proir();
            foreach (string word in acutaqltoken)
            {


                string res = (train.class2Equation(word));

                pro2 *= train.getProbOfWordClass2(word);
                dataGridView8.Rows.Add(new object[] { ++i, res.Split(' ')[0], res.Split(' ')[1] });

            }
            foreach (string word in notintest)
            {
                string res = (train.class2Equation(word));
                dataGridView8.Rows.Add(new object[] { ++i, res.Split(' ')[0], "1 -" + res.Split(' ')[1] });
                //Console.WriteLine(1 - train.getProbOfWordClass2(word));
                pro2 *= (1 - train.getProbOfWordClass2(word));
            }
            lblpc1.Text = pro1 + "";
            lblpc2.Text = pro2 + "";
            lblclas.Text = (pro1 >= pro2) ? "Sport" : "Tech";
        }
        public string RemoveDuplicateWords(string v)
        {
            // 1
            // Keep track of words found in this Dictionary.
            var d = new Dictionary<string, bool>();

            // 2
            // Build up string into this StringBuilder.
            StringBuilder b = new StringBuilder();

            // 3
            // Split the input and handle spaces and punctuation.
            string[] a = v.Split(new char[] { ' ', ',', ';', '.' },
                StringSplitOptions.RemoveEmptyEntries);

            // 4
            // Loop over each word
            foreach (string current in a)
            {
                // 5
                // Lowercase each word
                string lower = current.ToLower();

                // 6
                // If we haven't already encountered the word,
                // append it to the result.
                if (!d.ContainsKey(lower))
                {
                    b.Append(current).Append(' ');
                    d.Add(lower, true);
                }
            }
            // 7
            // Return the duplicate words removed
            return b.ToString().Trim();
        }
        private void tabPage9_Enter(object sender, EventArgs e)
        {
            for (int i = 0; i < bagofwords.Length; i++)
            {
                bagofwords[i] = bagofwords[i].ToLower();
            }
        }

    }
}
