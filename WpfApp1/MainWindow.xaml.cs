using DevExpress.Xpf.Charts;
using Microsoft.Win32;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApp1
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();

            var task = Task.Run(() =>
            {
                double x = 0;
                int start = Environment.TickCount;
                while (true)
                {
                    int time = Math.Abs(Environment.TickCount - start);
                    if (time >= 1000)//1秒记录一个点
                    {
                        start = Environment.TickCount;
                        this.Dispatcher.Invoke(new Action(() =>
                        {
                            x = x + 0.1;
                            //series1.Points.Add(new SeriesPoint(DateTime.Now.ToString("hh:mm:ss fff"), Math.Sin(x) * 100 + 100));//X轴 时分秒毫秒
                            series1.Points.Add(new SeriesPoint(DateTime.Now.ToString("hh:mm:ss"), Math.Sin(x) * 100 + 100));//X轴 时分秒
                        }));
                    }
                    Thread.Sleep(1);
                }
            });
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Configure save file dialog
            var dlg = new SaveFileDialog
            {
                FileName = "",
                DefaultExt = ".csv",
                Filter = "CSV Files|*.csv|TSV Files|*.tsv|Text Files|*.txt|All Files|*.*"
            };
            // Default file name
            // Default file extension
            // Filter files by extension

            // Show save file dialog
            var result = dlg.ShowDialog();

            // Process save file dialog results
            if (result == true)
            {
                // Save document
                var filename = dlg.FileName;

                FileStream savefs = new FileStream(dlg.FileName, FileMode.Create);
                StreamWriter savesw = new StreamWriter(savefs);
                for (int i = 0; i < series1.Points.Count; i++)
                {
                    savesw.Write(series1.Points[i].Argument + ",");
                    savesw.Write(series1.Points[i].Value.ToString("0.00") + ",");
                    savesw.Write("\r\n");
                }

                savesw.Flush();
                savesw.Close();
                savefs.Close();
                Title = filename;//显示全路径
            }
        }
    }
}
