using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Xml;

namespace mods.de_XML_Parser
{
    /// <summary>
    /// Just some Helper methods
    /// </summary>
    public static class Helper
    {
        /// <summary>
        /// the forum uses unix time stamps, this method is needed to convert it to a DateTime object
        /// </summary>
        /// <param name="unixTimeStamp">Unix timestamp which the forum uses</param>
        /// <returns>A DateTime object, calculated from a unix timestamp</returns>
        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

        /// <summary>
        /// Iterates through /xml/boards.php 
        /// </summary>
        /// <param name="_categories">A list of the categories you want the boards of</param>
        /// <returns>A list with all boards</returns>
        public static List<Board> GetBoards(List<Category> _categories)
        {
            List<Board> allBoards = new List<Board>(_categories.Count);

            foreach (Category category in _categories)
            {
                foreach (int boardId in category.BoardIds)
                {
                    allBoards.Add(new Board(boardId));
                }
            }

            return allBoards;
        }

        /// <summary>
        /// loads a xml from the web server
        /// </summary>
        /// <param name="_url">URL of the XML file</param>
        /// <returns>A XmlDocument object of the XML file</returns>
        public static XmlDocument LoadXml(string _url)
        {
            var xmlDoc = new XmlDocument();
            
            try
            {
                while (Helper.pingForum("forum.mods.de", 10000) == false)
                {
                    Console.WriteLine("Can't reach forum.mods.de right now, try again in 15 seconds...");
                    System.Threading.Thread.Sleep(15000);
                }

                xmlDoc.Load(_url);
            }
            catch (XmlException)
            {
                while (Helper.pingForum("forum.mods.de", 100000) == false)
                {
                    Console.WriteLine("Can't reach forum.mods.de right now, try again in 15 seconds...");
                    System.Threading.Thread.Sleep(15000);
                }

                WebClient client = new WebClient(); ;
                Stream stream = client.OpenRead(_url);
                StreamReader reader = new StreamReader(stream);
                string content = reader.ReadToEnd();

                content = RemoveTroublesomeCharacters(content);
                xmlDoc.LoadXml(content);
            }

            return xmlDoc;
        }

        /// <summary>
        /// Removes control characters and other non-UTF-8 characters
        /// </summary>
        /// <param name="_inString">The string to process</param>
        /// <returns>A string with no control characters or entities above 0x00FD</returns>
        public static string RemoveTroublesomeCharacters(string inString)
        {
            if (inString == null) return null;

            StringBuilder newString = new StringBuilder();
            char ch;

            for (int i = 0; i < inString.Length; i++)
            {

                ch = inString[i];
                // remove any characters outside the valid UTF-8 range as well as all control characters
                // except tabs and new lines
                //if ((ch < 0x00FD && ch > 0x001F) || ch == '\t' || ch == '\n' || ch == '\r')
                //if using .NET version prior to 4, use above logic
                if (XmlConvert.IsXmlChar(ch)) //this method is new in .NET 4
                {
                    newString.Append(ch);
                }
            }
            return newString.ToString();

        }

        private static bool pingForum(string _hostName, int _timeoutTime)
        {
            Ping ping = new Ping();
            PingReply pingReply = ping.Send(_hostName, _timeoutTime);

            if (pingReply.Status == IPStatus.Success)
            {
                return true;
            }

            return false;
        }

        public static bool CheckIfParsable(string _url)
        {
            var xmlDoc = new XmlDocument();

            try
            {
                while (Helper.pingForum("forum.mods.de", 10000) == false)
                {
                    Console.WriteLine("Can't reach forum.mods.de right now, try again in 15 seconds...");
                    System.Threading.Thread.Sleep(15000);
                }

                xmlDoc.Load(_url);
            }
            catch (XmlException)
            {
                while (Helper.pingForum("forum.mods.de", 100000) == false)
                {
                    Console.WriteLine("Can't reach forum.mods.de right now, try again in 15 seconds...");
                    System.Threading.Thread.Sleep(15000);
                }

                WebClient client = new WebClient(); ;
                Stream stream = client.OpenRead(_url);
                StreamReader reader = new StreamReader(stream);
                string content = reader.ReadToEnd();

                content = RemoveTroublesomeCharacters(content);
                xmlDoc.LoadXml(content);
            }
            catch (WebException)
            {
                return false;
            }
            catch (IOException)
            {
                return false;
            }

            return true;
        }
    }
}
