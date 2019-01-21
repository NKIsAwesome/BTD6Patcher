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
using System.Diagnostics;

namespace BTD6Pacther
{
    public partial class Form1 : Form
    {
        public FileStream fs;
        public String[] metadata;
        public static String[] keys = { "GetFreeTower(", "GetFreeUpgrade(", "HasCompletedMap(", "HasCompletedMode(", "HasUnlockedTower(", "HasUpgrade(s", "HasUnlockedHero(" };
        public String[] offsets = new String[keys.Length];
        public CheckBox[] boxes = new CheckBox[keys.Length];
        public Form1()
        {
            InitializeComponent();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            boxes[0] = checkBox1;
            boxes[1] = checkBox2;
            boxes[2] = checkBox3;
            boxes[3] = checkBox4;
            boxes[4] = checkBox5;
            boxes[5] = checkBox6;
            boxes[6] = checkBox7;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                metadata = File.ReadAllLines(openFileDialog1.FileName);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (openFileDialog2.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                fs = new FileStream(openFileDialog2.FileName, FileMode.Open);
                fs.Position = 0;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if(fs == null || metadata == null)
            {
                MessageBox.Show("Metadata and Binary must be loaded!");
                return;
            }
            readMetaData();
            writeHack();
            MessageBox.Show("Patching complete!");
        }
        private void readMetaData()
        {
            try
            {
                String[] option = { "Offset: " };
                for (var i = 0; i < metadata.Length; i++)
                {
                    for (var j = 0; j < keys.Length; j++)
                    {
                        if (metadata[i].Contains(keys[j]))
                        {
                            String[] split = metadata[i].Split(option, StringSplitOptions.None);
                            if (split.Length > 1)
                                offsets[j] = split[1];// String.Join(",",split);
                        }
                    }

                }
                Debug.Write(String.Join(",", offsets));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to read file.");
            }
        }

        private void writeHack()
        {
            byte[] bytes = { 0xB0,0x01,0xC3};
            try
            {
                for (var i = 0; i < offsets.Length; i++)
                {
                    if (!boxes[i].Checked) continue;
                    if (offsets[i].Length == 0) continue;
                    fs.Position = Convert.ToInt32(offsets[i], 16);
                    fs.WriteByte(bytes[0]);
                    fs.WriteByte(bytes[1]);
                    fs.WriteByte(bytes[2]);
                }
            } catch (Exception ex)
            {
                MessageBox.Show("Failed to write to binary.");
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                linkLabel1.LinkVisited = true; 
                System.Diagnostics.Process.Start("https://www.youtube.com/channel/UCPkouG116lenuZl3G1oYYrQ");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to open link that was clicked.");
            }
        }

        private void openFileDialog2_FileOk(object sender, CancelEventArgs e)
        {

        }
    }
}
