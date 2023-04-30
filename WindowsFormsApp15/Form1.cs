using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace WindowsFormsApp15
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private async void searchButton_Click(object sender, EventArgs e)
        {
            string searchWord = searchTextBox.Text;
            string directoryPath = directoryTextBox.Text;

            if (string.IsNullOrEmpty(searchWord) || string.IsNullOrEmpty(directoryPath))
            {
                MessageBox.Show("Please enter a search word and directory path.");
                return;
            }

            if (!Directory.Exists(directoryPath))
            {
                MessageBox.Show("Directory does not exist.");
                return;
            }

            resultTextBox.Clear();

            await SearchFilesAsync(directoryPath, searchWord);
        }

        private async Task SearchFilesAsync(string directoryPath, string searchWord)
        {
            await Task.Run(() =>
            {
                string[] files = Directory.GetFiles(directoryPath, "*", SearchOption.AllDirectories);

                foreach (string file in files)
                {
                    int count = CountWordOccurrences(file, searchWord);

                    if (count > 0)
                    {
                        resultTextBox.Invoke(new Action(() =>
                        {
                            resultTextBox.AppendText($"File name: {Path.GetFileName(file)}" + Environment.NewLine);
                            resultTextBox.AppendText($"Path: {file}" + Environment.NewLine);
                            resultTextBox.AppendText($"Occurrences: {count}" + Environment.NewLine + Environment.NewLine);
                        }));
                    }
                }
            });
        }

        private int CountWordOccurrences(string filePath, string searchWord)
        {
            int count = 0;

            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;

                while ((line = reader.ReadLine()) != null)
                {
                    count += (line.ToLower().Split(new char[] { ' ', '.', ',' }, StringSplitOptions.RemoveEmptyEntries)).Count(word => word == searchWord.ToLower());
                }
            }

            return count;
        }
    }
}
