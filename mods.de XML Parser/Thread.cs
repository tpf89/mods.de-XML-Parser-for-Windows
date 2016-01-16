using System;
using System.Collections.Generic;
using System.Xml;

namespace mods.de_XML_Parser
{
    /// <summary>
    /// All information of a Thread are stored in objects of this class
    /// </summary>
    public class Thread
    {
        #region private fields
        private readonly int id;
        private readonly int currentUserId;
        private readonly int numberOfReplies;
        private readonly int numberOfHits;
        private readonly int numberOfPages;
        private readonly int inThreadId;
        private readonly int inBoardId;
        private readonly User author;
        private readonly string title;
        private readonly string subtitle;
        private readonly Dictionary<string, bool> flags;
        private readonly DateTime date;
        #endregion

        #region Properties
        public int Id
        {
            get
            {
                return id;
            }
        }

        public int CurrentUserId
        {
            get
            {
                return currentUserId;
            }
        }

        public int NumberOfReplies
        {
            get
            {
                return numberOfReplies;
            }
        }

        public int NumberOfHits
        {
            get
            {
                return numberOfHits;
            }
        }

        public int NumberOfPages
        {
            get
            {
                return numberOfPages;
            }
        }

        public int InThreadId
        {
            get
            {
                return inThreadId;
            }
        }

        public int InBoardId
        {
            get
            {
                return inBoardId;
            }
        }

        public User Author
        {
            get
            {
                return author;
            }
        }

        public string Title
        {
            get
            {
                return title;
            }
        }

        public Dictionary<string, bool> Flags
        {
            get
            {
                return flags;
            }
        }

        public DateTime Date
        {
            get
            {
                return date;
            }
        }

        public string Subtitle
        {
            get
            {
                return subtitle;
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a thread object by a thread id
        /// </summary>
        /// <param name="_threadId">Thread id</param>
        public Thread(int _threadId)
        {
            flags = new Dictionary<string, bool>(5);

            string xmlFile = $"http://forum.mods.de/bb/xml/thread.php?TID={_threadId}";
            var xmlDoc = Helper.LoadXml(xmlFile);

            currentUserId = Convert.ToInt32(xmlDoc.DocumentElement.Attributes["current-user-id"].Value);
            id = _threadId;
            title = xmlDoc.DocumentElement.ChildNodes[0].InnerText;
            subtitle = xmlDoc.DocumentElement.ChildNodes[1].InnerText;
            numberOfReplies = Convert.ToInt32(xmlDoc.DocumentElement.ChildNodes[2].Attributes["value"].Value);
            numberOfHits = Convert.ToInt32(xmlDoc.DocumentElement.ChildNodes[3].Attributes["value"].Value);
            numberOfPages = Convert.ToInt32(xmlDoc.DocumentElement.ChildNodes[4].Attributes["value"].Value);

            foreach (XmlNode item in xmlDoc.DocumentElement.ChildNodes[5].ChildNodes)
            {
                int state = Convert.ToInt32(item.Attributes["value"].Value);
                Flags.Add(item.Name, Convert.ToBoolean(state));
            }

            inBoardId = Convert.ToInt32(xmlDoc.DocumentElement.ChildNodes[6].Attributes["id"].Value);

            XmlNode firstPostNode = xmlDoc.DocumentElement.ChildNodes[7].ChildNodes[0];

            int threadStarterId = Convert.ToInt32(firstPostNode.ChildNodes[0].Attributes["id"].Value);
            string threadStarterName = firstPostNode.ChildNodes[0].InnerText;
            author = new User(threadStarterId, threadStarterName);

            double unixTimeStamp = Convert.ToInt64(firstPostNode.ChildNodes[1].Attributes["timestamp"].Value);
            date = Helper.UnixTimeStampToDateTime(unixTimeStamp);

            inThreadId = Convert.ToInt32(firstPostNode.ChildNodes[2].Attributes["id"].Value);
            inBoardId = Convert.ToInt32(firstPostNode.ChildNodes[3].Attributes["id"].Value);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Gets all posts of a Thread
        /// </summary>
        /// <returns>A list with all posts which have been posted in this thread</returns>
        public List<Post> GetPostsOfThread()
        {
            List<Post> posts = new List<Post>(this.NumberOfReplies);

            for (int i = 0; i < this.NumberOfPages; i++)
            {
                posts.AddRange(Post.GetPostsOfPage(this.Id, i + 1));
            }

            return posts;
        }
        #endregion
    }
}
