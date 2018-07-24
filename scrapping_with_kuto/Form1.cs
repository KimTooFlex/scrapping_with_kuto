using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace scrapping_with_kuto
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //LETS LOAD THE TEAMS TO COMOBO BOX
            this.Cursor = Cursors.WaitCursor;
            List<Dictionary<string,string>> teams = NBA_scrapper.getTeamList();

            cmbTeam.Tag = teams;

            foreach (Dictionary<string,string> team in teams)
            {
                cmbTeam.Items.Add(team["name"]);
            }
            this.Cursor = Cursors.Default;
           // cmbTeam.SelectedIndex = 0;
        }

        private void cmbTeam_SelectedIndexChanged(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();

            List<Dictionary<string, string>> teams = (List<Dictionary<string, string>>)cmbTeam.Tag;
            Dictionary<string, string> team = teams[cmbTeam.SelectedIndex];


            this.Cursor = Cursors.WaitCursor;
            //load team players
            List<Dictionary<string, string>> TeamPlayers = NBA_scrapper.GetPlayers(team["url"]);
 
            //loop players and add to datagrid
            foreach (Dictionary<string,string> playerInfo in TeamPlayers)
            {
                dataGridView1.Rows.Add(
                    new object[]
                    {
                        playerInfo["no"],
                        playerInfo["name"],
                        playerInfo["pos"],
                        playerInfo["age"],
                        playerInfo["ht"],
                        playerInfo["wt"],
                        playerInfo["college"],
                        playerInfo["salary"]

                    }

                    );
            }


            this.Cursor = Cursors.Default;
          
        }
    }
}
