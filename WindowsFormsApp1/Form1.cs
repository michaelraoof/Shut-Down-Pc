using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Management;
using System.Globalization;
using System.Threading;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        static DateTime obj;

        public Form1()
        {
            InitializeComponent();
            //   timer1 = new Timer() { Enabled = false, Interval = 1000 };

        }

        static bool flag = false;
        Thread thread;
        private void button1_Click(object sender, EventArgs e)
        {
            if (flag)
            {
                thread.Abort();
            }
            flag = true;
            string temp = "";
            if (textBox1.Text == "")
            {
                temp = dateTimePicker1.Value.ToString();

            }
            else
            {
                double seconds = Convert.ToDouble(textBox1.Text) * 60 * 60;
                temp = DateTime.Now.AddSeconds(seconds).ToString();

            }
            CultureInfo culture = new CultureInfo("en-US");
            obj = Convert.ToDateTime(temp, culture);

             thread = new Thread(on_doing);
            thread.Start(); 
           

            label1.Text = "PC Will Restart " + obj.ToString();
        }
        void Shutdown()
        {
            ManagementBaseObject mboShutdown = null;
            ManagementClass mcWin32 = new ManagementClass("Win32_OperatingSystem");
            mcWin32.Get();

            // You can't shutdown without security privileges
            mcWin32.Scope.Options.EnablePrivileges = true;
            ManagementBaseObject mboShutdownParams =
                     mcWin32.GetMethodParameters("Win32Shutdown");

            // Flag 1 means we want to shut down the system. Use "2" to reboot.
            mboShutdownParams["Flags"] = "1";
            mboShutdownParams["Reserved"] = "0";
            foreach (ManagementObject manObj in mcWin32.GetInstances())
            {
                mboShutdown = manObj.InvokeMethod("Win32Shutdown",
                                               mboShutdownParams, null);
            }
        }
     
        private void button2_Click(object sender, EventArgs e)
        {
            thread.Abort();
            obj = new DateTime();
            label1.Text = "";
            label2.Text = "";
            flag = false;
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {

            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
                (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }

        }

        void on_doing()
        {
            while (true) {

                if (flag)
                {
                    label2.Text="you still have "+(Math.Floor((obj.Subtract(DateTime.Now).TotalMinutes))).ToString()+" Minutes. "+ (obj.Subtract(DateTime.Now).Seconds).ToString()+" Seconds.";
                    if (DateTime.Compare(DateTime.Now, obj) > 0)
                    {
                        Shutdown();
                         flag = false;
                        obj = new DateTime();



                    }
                }
               // Thread.Sleep(1000);

            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {if(thread !=null)
            thread.Abort();
        }
    }
}
