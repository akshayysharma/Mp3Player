using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Mp3_Player
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnLoadFile_Click(object sender, EventArgs e)
        {
            Stream loadfile = null;
            OpenFileDialog open = new OpenFileDialog();
            
            //set up the properties like initial directories,types of files etc.
            open.InitialDirectory = "c:\\";
            
            //type of files which we want
            open.Filter = "MP3 Audio file (*.mp3)|*.mp3|Windows Media Audio File (*.wma)|*.wma|WAV Audio File (*.wav)|*.wav|All Files (*.*)|*.*";
            
            //filter index=filter which we want to display by default
            //ex in my code mp3 is 1 in our filter so set it to 1 
            //setting it 2 it would display wma files first
            open.FilterIndex = 1;
            
            //Restore directory=it will restore the default directory every time we brouse the new files
            open.RestoreDirectory = false;

            //multiselect=allow us to select multiple files
            open.Multiselect = true;

            if (open.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if ((loadfile = open.OpenFile()) != null)
                    {
                        using (loadfile)
                        {
                            //setting up the strings
                            //setting up the absolute file path and the safe file path
                            string[] fileNameAndPath = open.FileNames;
                            string[] filename = open.SafeFileNames;

                            for (int i = 0; i < open.SafeFileName.Count(); i++)
                            {
                                //adding each file to the list view
                                //str[0]=main file or the safe file name
                                //str[1]=secontd main file or the absolute path used by media player
                                string[] str = new string[2];
                                str[0] = filename[i];
                                str[1] = fileNameAndPath[i];

                                ListViewItem lvi = new ListViewItem(str);
                                listView1.Items.Add(lvi);
                            }
                        }
                    }
                }
                    //if file not found
                catch (Exception ex)
                {
                    MessageBox.Show("Error:Could not read the file from the disk");
                }
            }
        }

        private void btnPlayAll_Click(object sender, EventArgs e)
        {
            WMPLib.IWMPPlaylist play = axWindowsMediaPlayer1.playlistCollection.newPlaylist("myplaylist");
            WMPLib.IWMPMedia media;

            for (int i = 0; i < listView1.Items.Count; i++)
            {
                int j = 1;
                
                //add subitems to the playlist
                //playlist=basic text file contain absolute path to the rext files
                media = axWindowsMediaPlayer1.newMedia(listView1.Items[i].SubItems[j].Text);
                play.appendItem(media);
                j++;

                //set the media player current playlist to the one which we have created
                axWindowsMediaPlayer1.currentPlaylist = play;
                
                //play the files
                axWindowsMediaPlayer1.Ctlcontrols.play();

            }
            
        }

        //getting the single selected files
        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            //play the selected files
            string select = listView1.FocusedItem.SubItems[1].Text;
            axWindowsMediaPlayer1.URL = @select;
        }

      

       
    }
}
