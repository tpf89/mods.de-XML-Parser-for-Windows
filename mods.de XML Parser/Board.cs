using System;
using System.Collections.Generic;
using System.Xml;

namespace mods.de_XML_Parser
{
    /// <summary>
    /// Abstraction of xml/board.php
    /// </summary>
    public class Board
    {
        #region private fields
        private readonly int id;
        private readonly Dictionary<string, int> threadsWith;
        private readonly string name;
        private readonly string description;
        private readonly int numberOfThreads;
        private readonly int numberOfReplies;
        private readonly int categoryId;
        #endregion

        #region properties
        public int Id
        {
            get
            {
                return id;
            }
        }

        public Dictionary<string, int> ThreadsWith
        {
            get
            {
                return threadsWith;
            }
        }

        public string Name
        {
            get
            {
                return name;
            }
        }

        public string Description
        {
            get
            {
                return description;
            }
        }

        public int NumberOfThreads
        {
            get
            {
                return numberOfThreads;
            }
        }

        public int NumberOfReplies
        {
            get
            {
                return numberOfReplies;
            }
        }

        public int CategoryId
        {
            get
            {
                return categoryId;
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a board object
        /// </summary>
        /// <param name="_boardId">Board id</param>
        public Board(int _boardId)
        {
            int pageId = 1;

            id = _boardId;
            threadsWith = new Dictionary<string, int>(5);

            string xmlFile = $"http://forum.mods.de/bb/xml/board.php?BID={id}&page={pageId}";
            var xmlDoc = Helper.LoadXml(xmlFile);

            if (xmlDoc != null)
            {
                name = xmlDoc.DocumentElement.ChildNodes[0].InnerText;
                description = xmlDoc.DocumentElement.ChildNodes[1].InnerText;
                numberOfThreads = Convert.ToInt32(xmlDoc.DocumentElement.ChildNodes[2].Attributes["value"].Value);
                numberOfReplies = Convert.ToInt32(xmlDoc.DocumentElement.ChildNodes[3].Attributes["value"].Value);
                categoryId = Convert.ToInt32(xmlDoc.DocumentElement.ChildNodes[4].Attributes["id"].Value);

                foreach (XmlAttribute item in xmlDoc.DocumentElement.ChildNodes[5].Attributes)
                {
                    threadsWith.Add(item.Name, Convert.ToInt32(item.Value));
                }
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Gets all threads of a page
        /// </summary>
        /// <param name="_page">Page number</param>
        /// <returns>A list of all threads on a page</returns>
        public List<Thread> GetThreadsOfPage(int _page)
        {
            var xmlFile = $"http://forum.mods.de/bb/xml/board.php?BID={this.Id}&page={_page}";
            var xmlDoc = Helper.LoadXml(xmlFile);

            if (xmlDoc != null)
            {
                int threadsOnPageCount = Convert.ToInt32(xmlDoc.DocumentElement.ChildNodes[5].Attributes["count"].Value);
                List<Thread> threads = new List<Thread>(threadsOnPageCount);

                foreach (XmlNode thread in xmlDoc.DocumentElement.ChildNodes[5])
                {
                    int threadId = Convert.ToInt32(thread.Attributes["id"].Value);
                    if (Helper.CheckIfParsable($"http://forum.mods.de/bb/xml/thread.php?TID={threadId}"))
                    {
                        threads.Add(new Thread(threadId));
                    }
                    else
                    {
                        Console.WriteLine($"http://forum.mods.de/bb/xml/thread.php?TID={threadId} wasn't parsable.");
                    }
                }

                return threads;
            }
            return null;
        }
        #endregion
    }
}
