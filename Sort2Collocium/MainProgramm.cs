using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using ProgressBar = System.Windows.Forms.ProgressBar;

namespace Sort2Collocium
{
    public partial class MainProgramm : Form
    {

        int[] array;
        int[] arrayCoc; 
        int[] arrayBit; 
        int[] arrayOddEven; 

        
        TimeSpan tsCocktailShaker; // 
        TimeSpan tsBitonicMerge;   // 
        TimeSpan tsOddEven;   // 

        bool fCancelCoc; 
        bool fCancelBit;
        bool fCancelOddEven;

        public MainProgramm()
        {
            InitializeComponent(); 

            
            textBox1.Text = "";

           
            listBox1.Items.Clear();
            listBox2.Items.Clear();
            listBox3.Items.Clear();

           
            progressBar1.Value = 0;
            progressBar2.Value = 0;
            progressBar3.Value = 0;

            
            Active(false);

            
            fCancelCoc = false;
            fCancelBit = false;
            fCancelOddEven = false;
        }


        private void Close_Button_Click(object sender, EventArgs e)
        {
            this.Hide();
            MainForm mainForm = new MainForm();
            mainForm.Show();
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        

        // Внутренний метод, который отображает массив в элементе управления типа ListBox
        private void DisplayArray(int[] A, ListBox LB)
        {
            LB.Items.Clear();
            for (int i = 0; i < A.Length; i++)
                LB.Items.Add(A[i]);
        }


        // Внутренний метод активации элементов управления
        private void Active(bool active)
        {
           
            label2.Enabled = active;
            label3.Enabled = active;
            label4.Enabled = active;
            label5.Enabled = active;
            label6.Enabled = active;
            label7.Enabled = active;
            listBox1.Enabled = active;
            listBox2.Enabled = active;
            listBox3.Enabled = active;
            progressBar1.Enabled = active;
            progressBar2.Enabled = active;
            progressBar3.Enabled = active;
            button2.Enabled = active;
            button3.Enabled = active;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            Active(false);

            
            label5.Text = "";
            label6.Text = "";
            label7.Text = "";

           
            if (!backgroundWorker1.IsBusy)
                backgroundWorker1.RunWorkerAsync();
        }

        // Кнопка "Sort" - запустить потоки на выполнение
        private void button2_Click(object sender, EventArgs e)
        {

            
            button1.Enabled = false;

          
            if (!backgroundWorker2.IsBusy)
                backgroundWorker2.RunWorkerAsync();

            if (!backgroundWorker3.IsBusy)
                backgroundWorker3.RunWorkerAsync();

            if (!backgroundWorker4.IsBusy)
                backgroundWorker4.RunWorkerAsync();

        }

        // Кнопка Stop - отменить выполнение всех потоков
        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                backgroundWorker2.CancelAsync(); 
                backgroundWorker3.CancelAsync(); 
                backgroundWorker4.CancelAsync(); 
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        // Выполнение потока, в котором генерируется массив чисел
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {

            
            Random rnd = new Random();

            
            int n = Convert.ToInt32(textBox1.Text);

            
            array = new int[n];
            arrayCoc = new int[n];
            arrayBit = new int[n];
            arrayOddEven = new int[n];

            for (int i = 0; i < n; i++)
            {
                Thread.Sleep(1);
                array[i] = rnd.Next(1, n + 1); 
                arrayCoc[i] = arrayBit[i] = arrayOddEven[i] = array[i]; 

               
                try
                {
                    backgroundWorker1.ReportProgress((i * 100) / n);
                }
                catch (InvalidOperationException ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }
            }

        }









        // Сортировка методом перемешивания (Cocktail Shaker Sort) - поток
        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            
            int left = 0;
            int right = arrayCoc.Length - 1;
            bool swapped = true;

            
            tsCocktailShaker = new TimeSpan(DateTime.Now.Ticks);

            while (left <= right && swapped)
            {
                Thread.Sleep(1); 
                swapped = false;

                for (int i = left; i < right; i++)
                {
                    if (arrayCoc[i] > arrayCoc[i + 1])
                    {
                      
                        int temp = arrayCoc[i];
                        arrayCoc[i] = arrayCoc[i + 1];
                        arrayCoc[i + 1] = temp;
                        swapped = true;
                    }
                }

                
                right--;

               
                for (int i = right; i > left; i--)
                {
                    if (arrayCoc[i - 1] > arrayCoc[i])
                    {
                        
                        int temp = arrayCoc[i];
                        arrayCoc[i] = arrayCoc[i - 1];
                        arrayCoc[i - 1] = temp;
                        swapped = true;
                    }
                }

                
                left++;

                
                try
                {
                    backgroundWorker2.ReportProgress((left * 100) / arrayCoc.Length);
                }
                catch (InvalidOperationException ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }

                if (backgroundWorker2.CancellationPending)
                {
                    fCancelCoc = true;
                    break;
                }
            }

        }







        // Сортировка слиянием (Merge Sort)
        private void backgroundWorker3_DoWork(object sender, DoWorkEventArgs e)
        {

            tsBitonicMerge = new TimeSpan(DateTime.Now.Ticks);

            MergeSort(arrayBit, 0, arrayBit.Length - 1);

            try
            {
                this.Invoke((MethodInvoker)delegate
                {
                    backgroundWorker3.ReportProgress(100);
                });
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        void Merge(int[] arr, int left, int mid, int right)
        {
            int n1 = mid - left + 1;
            int n2 = right - mid;

            int[] leftArr = new int[n1];
            int[] rightArr = new int[n2];

            Array.Copy(arr, left, leftArr, 0, n1);
            Array.Copy(arr, mid + 1, rightArr, 0, n2);

            int i = 0, j = 0, k = left;

            while (i < n1 && j < n2)
            {
                if (leftArr[i] <= rightArr[j])
                {
                    arr[k] = leftArr[i];
                    i++;
                }
                else
                {
                    arr[k] = rightArr[j];
                    j++;
                }
                k++;
            }

            while (i < n1)
            {
                arr[k] = leftArr[i];
                i++;
                k++;
            }

            while (j < n2)
            {
                arr[k] = rightArr[j];
                j++;
                k++;
            }
        }

        void MergeSort(int[] arr, int left, int right)
        {
            if (left < right)
            {
                int mid = (left + right) / 2;

                MergeSort(arr, left, mid);
                MergeSort(arr, mid + 1, right);

                Merge(arr, left, mid, right);
            }
        }









        private void backgroundWorker4_DoWork(object sender, DoWorkEventArgs e)
        {
            
            tsOddEven = new TimeSpan(DateTime.Now.Ticks);

            
            OddEvenSort(arrayOddEven);

           
            for (int i = 0; i < arrayOddEven.Length; i++)
            {
                
                try
                {
                    int progressPercentage = (i * 100) / arrayOddEven.Length;
                    backgroundWorker4.ReportProgress(progressPercentage);
                }
                catch (InvalidOperationException ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }

               
                if (backgroundWorker4.CancellationPending)
                {
                    break;
                }
            }

            try
            {
                backgroundWorker4.ReportProgress(100);
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void OddEvenSort(int[] arr)
        {
            bool sorted = false;
            while (!sorted)
            {
                sorted = true;

                
                for (int i = 0; i < arr.Length - 1; i += 2)
                {
                    if (arr[i] > arr[i + 1])
                    {
                       
                        int temp = arr[i];
                        arr[i] = arr[i + 1];
                        arr[i + 1] = temp;
                        sorted = false;
                    }
                }

                
                for (int i = 1; i < arr.Length - 1; i += 2)
                {
                    if (arr[i] > arr[i + 1])
                    {
                        
                        int temp = arr[i];
                        arr[i] = arr[i + 1];
                        arr[i + 1] = temp;
                        sorted = false;
                    }
                }
            }
        }











            private void progressBar1_Click(object sender, EventArgs e)
        {

        }





        
        private void backgroundWorker4_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            label7.Text = Convert.ToString(e.ProgressPercentage) + " %";
            progressBar3.Value = e.ProgressPercentage;
        }



        

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
           
            button1.Text = "Generate array " + e.ProgressPercentage.ToString() + "%";
        }


        private void backgroundWorker2_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            label5.Text = Convert.ToString(e.ProgressPercentage) + " %";

            progressBar1.Value = e.ProgressPercentage;
            

        }

       

        private void backgroundWorker3_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            label6.Text = Convert.ToString(e.ProgressPercentage) + " %";
            progressBar2.Value = e.ProgressPercentage;
        }





        private void backgroundWorker1_RunWorkerCompleted_1(object sender, RunWorkerCompletedEventArgs e)
        {
            
            button1.Text = "Generate array";

            Active(true);

            DisplayArray(array, listBox1);
            DisplayArray(array, listBox2);
            DisplayArray(array, listBox3);

        }

        private void backgroundWorker2_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            
            if (fCancelCoc)
            {
                
                label5.Text = "";

                
                DisplayArray(array, listBox1);

                fCancelCoc = false;
            }
            else
            {
               
                TimeSpan time = new TimeSpan(DateTime.Now.Ticks) - tsCocktailShaker;
                label5.Text = String.Format("Минуты: {1}, Секунды: {2}, Миллисекунды: {3}",
                time.Hours, time.Minutes, time.Seconds, time.Milliseconds);

                
                DisplayArray(arrayCoc, listBox1);
            }

          
            progressBar1.Value = 0;
            button1.Enabled = true;
        }

       
        private void backgroundWorker3_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (fCancelBit)
            {
               
                label6.Text = "";

                
                DisplayArray(array, listBox2);

                fCancelBit = false;
            }
            else
            {
              
                TimeSpan time = new TimeSpan(DateTime.Now.Ticks) - tsBitonicMerge;
                label6.Text = String.Format("Минуты: {1}, Секунды: {2}, Миллисекунды: {3}",
                time.Hours, time.Minutes, time.Seconds, time.Milliseconds);

            
                DisplayArray(arrayBit, listBox2);
            }

          
            progressBar2.Value = 0;
            button1.Enabled = true;
        }

       
        private void backgroundWorker4_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            
            if (fCancelOddEven)
            {
           
                label7.Text = "";

                
                DisplayArray(array, listBox3);

                fCancelOddEven = false;
            }
            else
            {
                
                TimeSpan time = new TimeSpan(DateTime.Now.Ticks) - tsOddEven;
                label7.Text = String.Format("Минуты: {1}, Секунды: {2}, Миллисекунды: {3}",
                time.Hours, time.Minutes, time.Seconds, time.Milliseconds);

                
                DisplayArray(arrayOddEven, listBox3);
            }

           
            progressBar3.Value = 0;
            button1.Enabled = true;
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void progressBar2_Click(object sender, EventArgs e)
        {

        }

        private void MainProgramm_Load(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }
    }
}
