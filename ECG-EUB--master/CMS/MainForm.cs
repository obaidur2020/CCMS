using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO.Ports;
using System.Configuration;
using System.Data.SqlClient;
using CMS.All_Data;
using System.Drawing;
using System.IO;

namespace CMS
{
    public partial class MainForm : Form
    {
        

        static string constring = ConfigurationManager.ConnectionStrings["CMS.Properties.Settings.Setting"].ConnectionString;
        SqlConnection con = new SqlConnection(constring);

        Average average = new Average();

        int mov;
        int movX;
        int movY;
        DateTime startingTime;
        DateTime endingTime;


        List<int> rData = new List<int>();
        List<int> pData = new List<int>();
        List<int> qData = new List<int>();
        List<int> sData = new List<int>();
        List<int> tData = new List<int>();

        List<double> baseDouble = new List<double>();

        public MainForm()
        {
            InitializeComponent();
            timeLable.Text = DateTime.Now.ToLongTimeString();
            timer1.Start();
        }

        private SerialPort myport;
        private DateTime myDateTime;
        private string primaryDataString;
        private int primaryData;
        public List<int> ThresHoldData;
        double avLine;
        double thresholdR;
        double thresholdQS;
        double thresholdPT;

        private void startBtn_Click(object sender, EventArgs e)
        {
            myport = new SerialPort();
            myport.PortName = portComboBox.Text;
            myport.BaudRate = Convert.ToInt32(boudComboBox.Text);
            myport.Parity = Parity.None;
            myport.DataBits = 8;
            myport.StopBits = StopBits.One;
            myport.DataReceived += Myport_DataReceived;
            startingTime = DateTime.Now;
            try
            {
                if (myport.IsOpen)
                {
                    MessageBox.Show("Application is already running");
                }
                else
                {
                    
                    myport.Open();
                }
                
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            
        }

        private void Myport_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {  
            this.Invoke(new EventHandler(displaydata_event));

            primaryDataString = myport.ReadLine();
            primaryData = Convert.ToInt32(primaryDataString);            
            baseDouble.Add(primaryData);
            avLine = average.DataAverageDouble(baseDouble);
        }
        
        public void displaydata_event(object sender, EventArgs e)
        {

            
            myDateTime = DateTime.Now;
            string myTime = myDateTime.Hour + ":" + myDateTime.Minute + ":" + myDateTime.Second;
            dataShowTextBox.AppendText(myTime + "\t\t\t" + primaryDataString+"\n");
            //baseLineTextBox.Text = avLine.ToString();

            
            
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void closeBtn_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void minimizedBtn_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void exitToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void dragPanel_MouseDown(object sender, MouseEventArgs e)
        {
            mov = 1;
            movX = e.X;
            movY = e.Y;
        }

        private void dragPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (mov==1)
            {
                this.SetDesktopLocation(MousePosition.X - movX, MousePosition.Y - movY);
            }
        }

        private void dragPanel_MouseUp(object sender, MouseEventArgs e)
        {
            mov = 0;
        }
        private void saveBtn_Click(object sender, EventArgs e)
        {
            int pAverage = average.DataAverage(pData);
            int qAverage = average.DataAverage(qData);
            int rAverage = average.DataAverage(rData);
            int sAverage = average.DataAverage(sData);
            int tAverage = average.DataAverage(tData);

            int pCount = pData.Count();
            int qCount = qData.Count();
            int rCount = rData.Count();
            int sCount = sData.Count();
            int tCount = tData.Count();

            pCountMSG.Text = pCount > 1 ? "Total P found:" + pCount+". Average Value is: "+ pAverage : "P Value Missing";
            qCountMSG.Text = qCount > 1 ? "Total Q found:" + qCount + ". Average Value is: " + qAverage : "Q Value Missing";
            rCountMSG.Text = rCount > 1 ? "Total R found:" + rCount + ". Average Value is: " + rAverage : "R Value Missing";
            sCountMSG.Text = sCount > 1 ? "Total S found:" + sCount + ". Average Value is: " + sAverage : "S Value Missing";
            tCountMSG.Text = tCount > 1 ? "Total T found:" + tCount + ". Average Value is: " + tAverage : "T Value Missing";
        }
        private void stopBtn_Click(object sender, EventArgs e)
        {           
            if (myport.IsOpen)
            {
                myport.Close();
                endingTime = DateTime.Now;
                TimeSpan totTime = endingTime - startingTime;
                totalTimeTextBox.Text = totTime.ToString();
            }
            else
            {
                dataShowTextBox.Text = "Start First";
            }            
        }

        public void dataShowTextBox_TextChanged(object sender, EventArgs e)
        {
            
            double convertedDataDouble = Convert.ToDouble(primaryData);
            int convertedData = Convert.ToInt32(convertedDataDouble);

            thresholdR = avLine + (avLine * 0.3);
            thresholdQS = avLine - (avLine * 0.4);
            thresholdPT = avLine + (avLine * 0.15);
            

            if (convertedData>avLine)
            {
                if (convertedData>=thresholdR)
                {
                    rTextBox.Text = rData.Count().ToString();
                    rData.Add(convertedData);
                }

                else
                {
                    if (convertedData> thresholdPT)
                    {
                        tTextBox.Text = tData.Count().ToString();
                        tData.Add(convertedData);
                    }
                    else
                    {
                        pTextBox.Text = pData.Count().ToString();
                        pData.Add(convertedData);
                    }                    
                } 
            }

            else if(convertedData < avLine)
            {
                if (convertedData>thresholdQS)
                {
                    qTextBox.Text = qData.Count().ToString();
                    qData.Add(convertedData);
                }
                else
                {
                    sTextBox.Text = sData.Count().ToString();
                    sData.Add(Convert.ToInt32(convertedData));
                }               

            }

        }

        private void clearButton_Click(object sender, EventArgs e)
        {
            dataShowTextBox.Clear();            
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timeLable.Text = DateTime.Now.ToLongTimeString();
            timer1.Start();
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            try
            {
                string pathfile = @"C:\Users\GH Palash\Desktop\Data\";
                string filename = "GHPalash1.txt";
                File.WriteAllText(pathfile + filename, dataShowTextBox.Text);
                MessageBox.Show("Data has been Saved to " + pathfile, "Save file");

                dataGridView.Rows.Add(0, pTextBox.Text, qTextBox.Text, rTextBox.Text, sTextBox.Text);
            }

            catch (Exception exception3)
            {
                MessageBox.Show(exception3.Message, "Error");
            }
        }


    }
}
