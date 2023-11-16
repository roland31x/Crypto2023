using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace CryptographyApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SymmetricAlgorithm? selected;
        PaddingMode? paddingselected;
        CipherMode? cipherselected;
        bool fileok = false;
        string? filepath;
        string key = "0123456789123456";
        bool working = false;
        public MainWindow()
        {
            InitializeComponent();
            (AlgoBox.Items[0] as ComboBoxItem).Tag = Aes.Create();
            (AlgoBox.Items[1] as ComboBoxItem).Tag = DES.Create();
            (AlgoBox.Items[2] as ComboBoxItem).Tag = RC2.Create();
            (AlgoBox.Items[3] as ComboBoxItem).Tag = Rijndael.Create();
            (AlgoBox.Items[4] as ComboBoxItem).Tag = TripleDES.Create();

            foreach (var value in Enum.GetValues(typeof(PaddingMode)))
            {
                ComboBoxItem cb = new ComboBoxItem();
                cb.Tag = value;
                cb.Content = value.ToString();
                PaddingBox.Items.Add(cb);
            }
            PaddingBox.SelectedItem = PaddingBox.Items[0];
            foreach(var value in Enum.GetValues(typeof(CipherMode)))
            {
                ComboBoxItem cb = new ComboBoxItem();
                cb.Tag = value;
                cb.Content = value.ToString();
                ModeBox.Items.Add(cb);
            }
            ModeBox.SelectedItem = ModeBox.Items[0];
            KeyBox.Text = key;
        }

        private void FileSelect_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if(openFileDialog.ShowDialog() == true)
            {
                filepath = openFileDialog.FileName;
                fileok = true;
                FilePathLabel.Content = filepath;
            }
            else
            {
                fileok = false;
                FilePathLabel.Content = "";
            }                          
        }

        private async void Decrypt_Click(object sender, RoutedEventArgs e)
        {
            if (selected != null && cipherselected != null && paddingselected != null && filepath != null)
                await Work(false);
            else
                InfoLabel.Content = "Unset parameters!";
        }

        private async void Encrypt_Click(object sender, RoutedEventArgs e)
        {
            if (selected != null && cipherselected != null && paddingselected != null && filepath != null)
                await Work(true);
            else
                InfoLabel.Content = "Unset parameters!";
        }
        Task Work(bool encrypt)
        {
            if (working)
                return Task.CompletedTask;
            working = true;
            FileStream fin = new FileStream(filepath, FileMode.Open, FileAccess.Read);
            string output = "result.bin";
            int idx = 0;
            while(File.Exists(output))
            {
                output = $"result{idx}.bin";
                idx++;
            }
            FileStream fout = new FileStream(output, FileMode.OpenOrCreate, FileAccess.Write);
            fout.SetLength(0);

            //Create variables to help with read and write.
            byte[] bin = new byte[100]; //This is intermediate storage for the encryption.
            long rdlen = 0;              //This is the total number of bytes written.
            long totlen = fin.Length;    //This is the total length of the input file.
            int len;                     //This is the number of bytes to be written at a time.
            InfoLabel.Content = Encoding.UTF8.GetBytes(key).Length;

            CryptoStream encStream = new CryptoStream(fout, encrypt ? selected.CreateEncryptor(Encoding.UTF8.GetBytes(key), new byte[selected.BlockSize / 8]) : selected.CreateDecryptor(Encoding.UTF8.GetBytes(key), new byte[selected.BlockSize / 8]), CryptoStreamMode.Write);

            //Read from the input file, then encrypt and write to the output file.
            while (rdlen < totlen)
            {
                len = fin.Read(bin, 0, 100);
                encStream.Write(bin, 0, len);
                rdlen = rdlen + len;
                App.Current.Dispatcher.Invoke(() => InfoLabel.Content = $"{rdlen} bytes processed");
            }

            encStream.Close();
            fout.Close();
            fin.Close();

            App.Current.Dispatcher.Invoke(() => InfoLabel.Content = $"File saved to: " + Directory.GetCurrentDirectory() + @"\" + output);
            return Task.CompletedTask;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(AlgoBox.SelectedItem != null)
                selected = (SymmetricAlgorithm)(AlgoBox.SelectedItem as ComboBoxItem).Tag;
            if (ModeBox.SelectedItem != null)
                cipherselected = (CipherMode)(ModeBox.SelectedItem as ComboBoxItem).Tag;
            if (PaddingBox.SelectedItem != null)
                paddingselected = (PaddingMode)(PaddingBox.SelectedItem as ComboBoxItem).Tag;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string past = key;
            key = (sender as TextBox).Text;
            if (key.Length != 16)
            {
                InfoLabel.Content = "Key must be of length 16!";
                key = past;
            }
            else
            {
                InfoLabel.Content = "Key changed";
            }
                
        }
    }
}
