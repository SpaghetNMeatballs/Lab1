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

/* 
              .======.
              | ШОБЫ |
              |  НЕ  |
              |КРАШИЛ|
     .========'      '========.
     |   _      xxxx      _   |
     |  /_;-.__ / _\  _.-;_\  |
     |     `-._`'`_/'`.-'     |
     '========.`\   /`========'
              | |  / |
              |/-.(  |
              |\_._\ |
              | \ \`;|
              |  > |/|
              | / // |
              | |//  |
              | \(\  |
              |  ``  |
              |      |
              |      |
              |      |
              |      |
  \\jgs _  _\\| \//  |//_   _ \// _
 ^ `^`^ ^`` `^ ^` ``^^`  `^^` `^ `^
 Шобы не ломалося
 */

namespace test1
{
    public partial class Form1 : Form
    {
        // Массивы лейблов
        private Label[] nameLabels = new Label[4];
        private Label[] driveNameLabels = new Label[26];
        private Label[] sizeLabels = new Label[26];
        private Label[] freeSizeLabels = new Label[26];
        private Label[] fileSystemLabels = new Label[26];

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Timer driveTimer = new Timer();
            driveTimer.Interval = 1000;
            driveTimer.Start();
            driveTimer.Tick += new EventHandler(driveTimer_Tick);
            int startY = 30;

            nameLabels[0] = new Label();
            nameLabels[0].Text = "Имя диска";
            this.nameLabels[0].Location = new System.Drawing.Point(0, 0);
            this.nameLabels[0].Size = new System.Drawing.Size(90, 30);
            nameLabels[0].BackColor = Color.Black;
            nameLabels[0].ForeColor = Color.White;
            this.Controls.Add(nameLabels[0]);

            nameLabels[1] = new Label();
            nameLabels[1].Text = "Объём диска";
            this.nameLabels[1].Location = new System.Drawing.Point(90, 0);
            this.nameLabels[1].Size = new System.Drawing.Size(130, 30);
            nameLabels[1].BackColor = Color.Black;
            nameLabels[1].ForeColor = Color.White;
            this.Controls.Add(nameLabels[1]);

            nameLabels[2] = new Label();
            nameLabels[2].Text = "Свободное место диска";
            this.nameLabels[2].Location = new System.Drawing.Point(220, 0);
            this.nameLabels[2].Size = new System.Drawing.Size(130, 30);
            nameLabels[2].BackColor = Color.Black;
            nameLabels[2].ForeColor = Color.White;
            this.Controls.Add(nameLabels[2]);

            nameLabels[3] = new Label();
            nameLabels[3].Text = "Тип диска";
            this.nameLabels[3].Location = new System.Drawing.Point(350, 0);
            this.nameLabels[3].Size = new System.Drawing.Size(50, 30);
            nameLabels[3].BackColor = Color.Black;
            nameLabels[3].ForeColor = Color.White;
            this.Controls.Add(nameLabels[3]);

            for (int i = 0; i < 26; i++)
            {
                // Настраиваем именные лейблы для данной строки
                driveNameLabels[i] = new Label();
                driveNameLabels[i].Text = ((char)(i+65)).ToString()+":\\";
                this.driveNameLabels[i].Location = new System.Drawing.Point(0, startY);
                this.driveNameLabels[i].Size = new System.Drawing.Size(90, 30);
                float currentSize = driveNameLabels[i].Font.Size;
                currentSize += 2.0F;
                driveNameLabels[i].Font = new Font(driveNameLabels[i].Font.Name, currentSize, driveNameLabels[i].Font.Style, driveNameLabels[i].Font.Unit);
                this.Controls.Add(driveNameLabels[i]);

                // Настраиваем лейблы с общим свободным объёмом памяти для данной строки
                sizeLabels[i] = new Label();
                sizeLabels[i].Text = "---";
                this.sizeLabels[i].Location = new System.Drawing.Point(90, startY);
                this.sizeLabels[i].Size = new System.Drawing.Size(130, 30);
                this.Controls.Add(sizeLabels[i]);
                currentSize = sizeLabels[i].Font.Size;
                currentSize += 1.0F;
                sizeLabels[i].Font = new Font(sizeLabels[i].Font.Name, currentSize, sizeLabels[i].Font.Style, sizeLabels[i].Font.Unit);
                this.Controls.Add(sizeLabels[i]);

                // Настраиваем лейблы с доступным общим объёмом памяти для данной строки
                freeSizeLabels[i] = new Label();
                freeSizeLabels[i].Text = "---";
                this.freeSizeLabels[i].Location = new System.Drawing.Point(220, startY);
                this.freeSizeLabels[i].Size = new System.Drawing.Size(130, 30);
                currentSize = freeSizeLabels[i].Font.Size;
                currentSize += 2.0F;
                freeSizeLabels[i].Font = new Font(freeSizeLabels[i].Font.Name, currentSize, freeSizeLabels[i].Font.Style, freeSizeLabels[i].Font.Unit);
                this.Controls.Add(freeSizeLabels[i]);

                fileSystemLabels[i] = new Label();
                fileSystemLabels[i].Text = "---";
                this.fileSystemLabels[i].Location = new System.Drawing.Point(350, startY);
                this.fileSystemLabels[i].Size = new System.Drawing.Size(50, 30);
                this.Controls.Add(fileSystemLabels[i]);
                currentSize = fileSystemLabels[i].Font.Size;
                currentSize += 1.0F;
                fileSystemLabels[i].Font = new Font(fileSystemLabels[i].Font.Name, currentSize, fileSystemLabels[i].Font.Style, fileSystemLabels[i].Font.Unit);
                this.Controls.Add(fileSystemLabels[i]);
                startY += 30;
            }
            this.Size = new Size(90+130+130+50+16, startY+39);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
        }

        private void driveTimer_Tick(object sender, EventArgs e)
        {
            DriveInfo[] allDrives = DriveInfo.GetDrives();
            Dictionary<string, DriveInfo> drivesByNames = new Dictionary<string, DriveInfo>();
            for (int i = 0; i < allDrives.Length; i++)
            {
                drivesByNames.Add(allDrives[i].Name, allDrives[i]);
            }
            for (int i = 0; i < sizeLabels.Length; i++)
            {
                if (drivesByNames.ContainsKey(driveNameLabels[i].Text)){
                    driveNameLabels[i].BackColor = Color.LightGreen;
                    sizeLabels[i].BackColor = Color.LightBlue;
                    freeSizeLabels[i].BackColor = Color.LightBlue;
                    fileSystemLabels[i].BackColor = Color.LightBlue;
                    sizeLabels[i].Text = (drivesByNames[driveNameLabels[i].Text].TotalSize/1024).ToString()+" MBs";
                    freeSizeLabels[i].Text = (drivesByNames[driveNameLabels[i].Text].TotalFreeSpace / 1024).ToString() + " MBs";
                    fileSystemLabels[i].Text = drivesByNames[driveNameLabels[i].Text].DriveFormat;
                }
                else
                {
                    driveNameLabels[i].BackColor = Color.LightGray;
                    sizeLabels[i].BackColor = Color.LightGray;
                    freeSizeLabels[i].BackColor = Color.LightGray;
                    fileSystemLabels[i].BackColor = Color.LightGray;
                    sizeLabels[i].Text = "---";
                    freeSizeLabels[i].Text = "---";
                    fileSystemLabels[i].Text = "---";
                    freeSizeLabels[i].Text = "---";

                }
            }
        }
    }
}
