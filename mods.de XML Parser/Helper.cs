using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
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
                xmlDoc.Load(_url);
            }
            catch (XmlException)
            {
                WebClient client = new WebClient();
                Stream stream = client.OpenRead(_url);
                StreamReader reader = new StreamReader(stream);
                String content = reader.ReadToEnd();

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
    }
}
