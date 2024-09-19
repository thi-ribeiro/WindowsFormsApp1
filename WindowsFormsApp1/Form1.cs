using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public string PathWow;
        //public string PathWtf;
        //public string PathUserRealm;
        //public string PathCharacter;
        public Boolean PathBool;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.ShowDialog();

            if (folderBrowserDialog1.SelectedPath != null)
            {
                pathtextbox.Text = folderBrowserDialog1.SelectedPath;
                //PathWow = folderBrowserDialog1.SelectedPath;
                // achaWow(folderBrowserDialog1.SelectedPath);
                VerificaPathWow();
            }

            Properties.Settings.Default.Pathconf = pathtextbox.Text;
            Properties.Settings.Default.Save();
        }

        public void VerificaPathWow()
        {
            string PathFromFolderDialog = folderBrowserDialog1.SelectedPath;
            string PathWowUser = PathFromFolderDialog.Length > 0 ? PathFromFolderDialog : Properties.Settings.Default.Pathconf;
            string PathWowExe = $@"{PathWowUser}\WoW.exe";

            PathBool = File.Exists(PathWowExe);
            PathWow = PathBool ? PathWowExe : toolStripStatusLabel1.Text = "Executável 'WoW.exe', não foi encontrado!";

            toolStripStatusLabel1.Text = PathBool ? "Wow encontrado!" : "Executável 'WoW.exe', não foi encontrado!";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            InicializaWow();
        }

        private void InicializaWow()
        {
            string dirWDB = $@"{PathWow}\WDB";
            string dirWoW = $@"{PathWow}\WoW.exe";
            string dirWtf = $@"{PathWow}\WTF";

            toolStripStatusLabel1.Text = null;

            ProcessStartInfo startInfo = new ProcessStartInfo(dirWoW);

            if (File.Exists(dirWoW))
            {
                if (checkBox2.Checked && Directory.Exists(dirWDB))
                {
                    Directory.Delete(dirWDB, true);
                    toolStripStatusLabel1.Text += "Diretório WDB foi removido!";
                }

                if (checkBox1.Checked && Directory.Exists(dirWtf))
                {
                    Directory.Delete(dirWtf, true);
                    toolStripStatusLabel1.Text += "Diretório WTF foi removido!";
                }

                startInfo.Arguments += openglcheckbox.Checked ? "-OpenGL" : null;
                startInfo.Arguments += consolecheckbox.Checked ? "-Console" : null;

                startInfo.Verb = "runas";
                Process.Start(startInfo);                             
                
                this.Hide();
            }
            else
            {
                toolStripStatusLabel1.Text = "Selecione corretamente a pasta do Wow.exe!";
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            checkBox2.Checked = Properties.Settings.Default.ChekboxWDB;
            pathtextbox.Text = Properties.Settings.Default.Pathconf;
            openglcheckbox.Checked = Properties.Settings.Default.OpenGL;

            VerificaPathWow();
        }

        private void checkBox2_CheckedChanged_1(object sender, EventArgs e)
        {
            Properties.Settings.Default.ChekboxWDB = checkBox2.Checked;
            Properties.Settings.Default.Save();
        }
        private void openglcheckbox_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.OpenGL = openglcheckbox.Checked;
            Properties.Settings.Default.Save();
        }
        private void dropdowncharlist_SelectedIndexChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.wtfusuariocharlist = this.Text;
            Properties.Settings.Default.Save();
        }
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            this.Show();
        }
        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            popupaddons formadds = new popupaddons(PathWow, PathBool);
            formadds.Show();
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.ClearWtf = checkBox1.Checked;
        }
    }

}
