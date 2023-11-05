using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sort2Collocium
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void Close_Button_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form1 form1 = new Form1();
            form1.Show();


        }

        private void More1_Click(object sender, EventArgs e)
        {
            this.Hide();
            CoctailShacker coctailShacker = new CoctailShacker();
            coctailShacker.Show();

        }

        private void more2_Click(object sender, EventArgs e)
        {
            this.Hide();
            BitonicMerge bitonicMerge = new BitonicMerge();
            bitonicMerge.Show();
            


        }

        private void more3_Click(object sender, EventArgs e)
        {
            this.Hide();
            OddEven oddEven = new OddEven();
            oddEven.Show();

        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Hide();
            MainProgramm mainProgramm = new MainProgramm();
            mainProgramm.Show();


        }
    }
}
