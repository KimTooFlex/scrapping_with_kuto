using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace scrapping_with_kuto
{
    /// <summary>
    /// http://www.espn.com/nba/teams
    /// </summary>
    class NBA_scrapper
    {

        /// <summary>
        /// Gets the team list.
        /// </summary>
        /// <returns></returns>
        public static List<Dictionary<string,string>> getTeamList()
        {
            // lets rewrte the getTeams list

            // http://www.espn.com/nba/team/roster/_/name/atl


            string HTML = httpGet(" http://www.espn.com/nba/team/roster/_/name/atl");

            //import kuto
            ktf.Kuto scrapper = new ktf.Kuto(HTML);

            //snip the data limints

            scrapper = scrapper.Extract("<select", "</select>");
            scrapper = scrapper.Extract("</option>", ""); //remove the rist item
 
            List<Dictionary<string, string>> teams = new List<Dictionary<string, string>>();

            while (scrapper.Contains("<option"))
            {

                //scrap the info
                Dictionary<string, string> teamInfo = new Dictionary<string, string>();

                string url = "http:"+scrapper.Extract("value=\"", "\">").ToString().Trim();
                string name = scrapper.Extract("\">", "</").ToString().Trim();
               
                teamInfo.Add("name",name);
                teamInfo.Add("url", url);

             
                teams.Add(teamInfo);

                scrapper = scrapper.Extract("</option>", ""); //removes the first item
                
            }


            return teams;
        }

        /// <summary>
        /// Gets the players.
        /// </summary>
        /// <param name="team">The team.</param>
        /// <returns></returns>
        public static List<Dictionary<string, string>> GetPlayers(string url)
        {

         
            string HTML = httpGet(url);


            //import kuto
            ktf.Kuto scrapper = new ktf.Kuto(HTML.Replace("&nbsp;",""));

            scrapper = scrapper.Extract("<table", "</table>"); //extract the first table
            scrapper = scrapper.Extract("</tr>", ""); //skip the first row
            scrapper = scrapper.Extract("</tr>", ""); //skip the 2nd row


            List<Dictionary<string, string>> teamPlayers = new List<Dictionary<string, string>>();
                //loop and get player info

            while (scrapper.Contains("<tr"))
            {
                ktf.Kuto PlayerScrapper = new ktf.Kuto(scrapper.Extract("<tr","</tr>").ToString().Replace("<td >", "<td>")); //get player row
                Dictionary<string, string> playerInfo = new Dictionary<string, string>();

                //extract the player info
                playerInfo.Add("no",PlayerScrapper.Extract("<td>","</td>").ToString());


              
                PlayerScrapper = PlayerScrapper.Extract("</td>","");
                playerInfo.Add("name", PlayerScrapper.Extract("<td", "</td>").Extract("\">","</a>").StripTags().ToString());
 
                PlayerScrapper = PlayerScrapper.Extract("</td>", "");
                playerInfo.Add("pos", PlayerScrapper.Extract("<td>", "</td>").ToString());

                PlayerScrapper = PlayerScrapper.Extract("</td>", "");
                playerInfo.Add("age", PlayerScrapper.Extract("<td>", "</td>").ToString());

                PlayerScrapper = PlayerScrapper.Extract("</td>", "");
                playerInfo.Add("ht", PlayerScrapper.Extract("<td>", "</td>").ToString());

                PlayerScrapper = PlayerScrapper.Extract("</td>", "");
                playerInfo.Add("wt", PlayerScrapper.Extract("<td>", "</td>").ToString());

                PlayerScrapper = PlayerScrapper.Extract("</td>", "");
                playerInfo.Add("college", PlayerScrapper.Extract("<td>", "</td>").ToString());

                PlayerScrapper = PlayerScrapper.Extract("</td>", "");
                playerInfo.Add("salary", PlayerScrapper.Extract("<td>", "</td>").ToString());

                teamPlayers.Add(playerInfo);
                scrapper = scrapper.Extract("</tr>", ""); //skip the first row
                 
            }


            return teamPlayers;
        }

       
        /// <summary>
        /// lOADS WEBSITE html
        /// </summary>
        /// <param name="URL">The URL.</param>
        /// <returns></returns>
        static string httpGet(string URL)
        {
            String HTML = "";
            WebClient web = new WebClient();
            System.IO.Stream stream = web.OpenRead(URL);
            using (System.IO.StreamReader reader = new System.IO.StreamReader(stream))
            {
                HTML = reader.ReadToEnd();
            }

            return HTML;
        }
    }
}
