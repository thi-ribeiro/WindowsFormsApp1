using System;
using System.IO;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class popupaddons : Form
    {
        Form1 formPrincipal = new Form1();
        public string pathw, PathWtf, PathUserRealm, PathCharacter;
        public bool pathok;
        public popupaddons(string PathWow, bool pathOk)
        {
            InitializeComponent();

            pathok = pathOk;
            pathw = PathWow;
            //pathwtf = PathWtf;
            //pathrealm = PathUserRealm;
            //pathcharacter = PathCharacter;
        }
        private void popupaddons_Load(object sender, EventArgs e)
        {
            if (pathok)
            {
                dropdownusuarios.Text = Properties.Settings.Default.wtfusuario;
                dropdownrealms.Text = Properties.Settings.Default.wtfrealms;
                dropdowncharlist.Text = Properties.Settings.Default.wtfusuariocharlist;

                carregaDiretorio(); //Carrega o path principal de addons do Wow
                CarregaUsuarios(pathw);

                //MessageBox.Show(PathWtf);
            }
        }
        public void carregaDiretorio()
        {
            string diretorio_addons = $@"{pathw}\Interface\AddOns\";
            if (Directory.Exists(diretorio_addons))
            {
                string[] addonsfolder = Directory.GetDirectories(diretorio_addons);

                checkedListBox1.Items.Clear();
                foreach (string dir in addonsfolder)
                {
                    int lastcharindex = dir.IndexOf(@"\AddOns\");

                    checkedListBox1.Items.Add(dir.Substring(lastcharindex + 8));
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            carregaDiretorio();
        }

        public void modificarDiretorio(string diretorio, string alteracao)
        {
            string diretorio_addons = $@"{pathw}\Interface\AddOns\";
            string[] addonsfolder = Directory.GetDirectories(diretorio_addons);

            foreach (string item in addonsfolder)
            {
                string stringOff = "[ Off ]";
                string diretorioNome = item.Substring(item.IndexOf(@"\AddOns\") + 8);
                string diretorioNomeOff = item.Substring(item.IndexOf(stringOff) + 7);

                if (diretorioNome == diretorio)
                {
                    if (alteracao == "desativado" && !item.Contains("[ Off ]"))
                    {
                        Directory.Move(item, $@"{diretorio_addons}\{stringOff + diretorioNome}");
                    }
                    else if (alteracao == "ativado" && item.Contains("[ Off ]"))
                    {
                        Directory.Move(item, $@"{diretorio_addons}\{diretorioNomeOff}");
                    }
                    else if (alteracao == "removido")
                    {
                        string diretorioNomeModifier = item.Contains("[ Off ]") ? diretorioNomeOff :
                        diretorioNome;

                        Directory.Delete($@"{diretorio_addons}\{diretorioNomeModifier}", true);
                    }
                    MessageBox.Show($"O addon {diretorioNome} foi {alteracao}!");
                }
            }
        }
        private void CarregaUsuarios(string PathWow)
        {
            dropdownusuarios.Items.Clear();

            PathWtf = $@"{pathw}\Wtf\Account";

            if (Directory.Exists(PathWtf))
            {
                string[] usuarios = Directory.GetDirectories(PathWtf);

                foreach (string diretorioUsuario in usuarios)
                {
                    string diretorioUsuarioClean = diretorioUsuario.Substring(diretorioUsuario.LastIndexOf(@"\") + 1);
                    dropdownusuarios.Items.Add(diretorioUsuarioClean);
                }

                dropdownusuarios.SelectedIndex = 0;
            }
            else
            {
                MessageBox.Show("Diretório Wtf do usuário não existe!");
            }
        }
        private void CarregaRealm(string PathWtf)
        {
            string wtfDir = $@"{PathWtf}\{dropdownusuarios.Text}\";
        
                string[] checkNothing = { "bindings-cache", "cache", "SavedVariables" };
                string[] dirUsuarioAddons = Directory.GetDirectories(wtfDir);

                dropdownrealms.Items.Clear();

                foreach (string realmNameDir in dirUsuarioAddons)
                {
                    if (Directory.Exists(realmNameDir))
                    {
                        string realmDirectoryCleaner = realmNameDir.Substring(realmNameDir.LastIndexOf(@"\") + 1);

                        if (!realmDirectoryCleaner.Contains(checkNothing[2]))
                        {
                            PathUserRealm = realmNameDir;
                            dropdownrealms.Items.Add(realmDirectoryCleaner);
                        }
                        dropdownrealms.SelectedIndex = 0;
                    }
                }
        }
        private void CarregaChars(string PathUserRealm)
        {
            dropdowncharlist.Items.Clear();

            string[] dirCharsList = Directory.GetDirectories(PathUserRealm);         

            foreach (string character in dirCharsList)
            {
                string characterDirectory = character.Substring(character.LastIndexOf(@"\") + 1);
                dropdowncharlist.Items.Add(characterDirectory);
            }
            dropdowncharlist.SelectedIndex = 0;

            CarregaCharsChecked();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            foreach (string item in checkedListBox1.CheckedItems)
            {
                if (radioativa.Checked)
                {
                    modificarDiretorio(item, "ativado");
                }
                else if (radiodesativa.Checked)
                {
                    modificarDiretorio(item, "desativado");
                }
                else if (radioremove.Checked)
                {
                    modificarDiretorio(item, "removido");
                }
            }
            carregaDiretorio();
        }
        private void dropdownrealms_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            CarregaChars(PathUserRealm);
            Properties.Settings.Default.wtfrealms = this.Text;
            Properties.Settings.Default.Save();
        }

        private void dropdownusuarios_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            CarregaRealm(PathWtf);
            Properties.Settings.Default.wtfusuario = this.Text;
            Properties.Settings.Default.Save();
        }

        public void CarregaCharsChecked()
        {
            string formarPathCharacter = $@"{PathUserRealm}{PathCharacter}\{dropdowncharlist.Text}";
            bool arquivo = File.Exists($@"{formarPathCharacter}\AddOns.txt");

            if (dropdowncharlist.Items.Count > 0 && arquivo)
            {
                checkedListBox2.Items.Clear();

                string[] readTxt = File.ReadAllLines($@"{formarPathCharacter}\AddOns.txt");
                string[] endis = { ": enabled", ": disabled" };

                int i = 0;

                foreach (string linha in readTxt)
                {
                    foreach (string formataText in endis)
                    {
                        if (linha.Contains(formataText))
                        {
                            checkedListBox2.Items.Add(linha.Replace(formataText, string.Empty));
                        }
                    }
                    if (linha.Contains("enabled"))
                    {
                        //MessageBox.Show($@"Enabled? {linha} index {i}");
                        checkedListBox2.SetItemCheckState(i, CheckState.Checked);
                    }
                    i++;
                }

            }
            //else
            //{
            //toolStripStatusLabel1.Text += " Nenhum arquivo de configuração encontrado!";
            //}
        }
    }
}
