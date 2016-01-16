using System;
using System.Collections.Generic;
using System.Xml;

namespace mods.de_XML_Parser
{
    /// <summary>
    /// All information of a post are stored in objects of this class
    /// </summary>
    public class Post
    {
        #region private fields
        private readonly int page;
        private readonly int id;
        private readonly int offset;
        private readonly int inThreadId;
        private readonly int inBoardId;
        private readonly User author;
        private readonly DateTime date;
        private readonly string message;
        private readonly int editedCount;
        private readonly Edit lastEdit;
        private readonly string title;
        #endregion

        #region Properties
        public int Page
        {
            get
            {
                return page;
            }
        }

        public int Id
        {
            get
            {
                return id;
            }
        }

        public int Offset
        {
            get
            {
                return offset;
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

        public DateTime Date
        {
            get
            {
                return date;
            }
        }

        public string Message
        {
            get
            {
                return message;
            }
        }

        public int EditedCount
        {
            get
            {
                return editedCount;
            }
        }

        public Edit LastEdit
        {
            get
            {
                return lastEdit;
            }

        }

        public string Title
        {
            get
            {
                return title;
            }
        }
        #endregion

        #region Constructors
        public Post(int _id, int _threadId)
        {
            id = _id;
            inThreadId = _threadId;

            string xmlFile = $"http://forum.mods.de/bb/xml/thread.php?TID={_threadId}&PID={_id}";
            var xmlDoc = Helper.LoadXml(xmlFile);

            // if logged in, it's ChildNodes[9], not ChildNodes[8]
            page = Convert.ToInt32(xmlDoc.DocumentElement.ChildNodes[8].Attributes["page"].Value);
            offset = Convert.ToInt32(xmlDoc.DocumentElement.ChildNodes[8].Attributes["offset"].Value);
            inBoardId = Convert.ToInt32(xmlDoc.DocumentElement.ChildNodes[6].Attributes["id"].Value);

            foreach (XmlNode node in xmlDoc.DocumentElement.ChildNodes[8])
            {
                if (_id == Convert.ToInt32(node.Attributes["id"].Value))
                {
                    int avatarId = Convert.ToInt32(node.ChildNodes[3].Attributes["id"].Value);
                    string avatarUrl = node.ChildNodes[3].InnerText;

                    int authorId = Convert.ToInt32(node.ChildNodes[0].Attributes["id"].Value);
                    int authorGroupId = Convert.ToInt32(node.ChildNodes[0].Attributes["group-id"].Value);
                    string authorName = node.ChildNodes[0].InnerText;
                    Avatar avatar = new Avatar(avatarId, avatarUrl);

                    author = new User(authorId, authorName, authorGroupId, avatar);

                    double unixTimeStamp = Convert.ToInt32(node.ChildNodes[1].Attributes["timestamp"].Value);
                    date = Helper.UnixTimeStampToDateTime(unixTimeStamp);

                    editedCount = Convert.ToInt32(node.ChildNodes[2].ChildNodes[0].Attributes["count"].Value);

                    if (EditedCount > 0)
                    {
                        string editUserName =
                            node.ChildNodes[2].ChildNodes[0].ChildNodes[0].ChildNodes[0].InnerText;
                        int editUserId =
                            Convert.ToInt32(node.ChildNodes[2].ChildNodes[0].ChildNodes[0].ChildNodes[0].Attributes["id"].Value);
                        double editTimeStamp =
                            Convert.ToInt32(node.ChildNodes[2].ChildNodes[0].ChildNodes[0].ChildNodes[1].Attributes["timestamp"].Value);

                        User editUser = new User(editUserId, editUserName);
                        lastEdit = new Edit(editUser, editTimeStamp);
                    }

                    message = node.ChildNodes[2].ChildNodes[1].InnerText;
                    title = node.ChildNodes[2].ChildNodes[2].InnerText;

                    break;
                }
            }
        }

        public Post(int _id, int _threadId, int _boardId, int _page, int _offset, User _author, DateTime _creationDate,
            string _message, string _title, int _editedCount)
        {
            id = _id;
            inThreadId = _threadId;
            inBoardId = _boardId;
            page = _page;
            offset = _offset;
            author = _author;
            date = _creationDate;
            message = _message;
            title = _title;
            editedCount = _editedCount;
        }

        public Post(int _id, int _threadId, int _boardId, int _page, int _offset, User _author, DateTime _creationDate,
            string _message, string _title, int _editedCount, Edit _lastEdit)
        {
            id = _id;
            inThreadId = _threadId;
            inBoardId = _boardId;
            page = _page;
            offset = _offset;
            author = _author;
            date = _creationDate;
            message = _message;
            title = _title;
            editedCount = _editedCount;
            lastEdit = _lastEdit;
        }
        #endregion

        #region static methods
        public static List<Post> GetPostsOfPage(int _threadId, int _pageId)
        {
            int inThreadId = _threadId;

            string xmlFile = $"http://forum.mods.de/bb/xml/thread.php?TID={_threadId}&page={_pageId}";
            var xmlDoc = Helper.LoadXml(xmlFile);

            // if logged in, it's ChildNodes[9], not ChildNodes[8]
            int page = _pageId;
            int offset = Convert.ToInt32(xmlDoc.DocumentElement.ChildNodes[8].Attributes["offset"].Value);
            int inBoardId = Convert.ToInt32(xmlDoc.DocumentElement.ChildNodes[6].Attributes["id"].Value);

            int postsOnPage = xmlDoc.DocumentElement.ChildNodes[8].ChildNodes.Count;
            List<Post> posts = new List<Post>(postsOnPage);

            foreach (XmlNode node in xmlDoc.DocumentElement.ChildNodes[8])
            {
                int id = Convert.ToInt32(node.Attributes["id"].Value);
                int avatarId;
                string avatarUrl;

                try
                {
                    avatarId = Convert.ToInt32(node.ChildNodes[3].Attributes["id"].Value);
                    avatarUrl = node.ChildNodes[3].InnerText;
                }
                catch (NullReferenceException)
                {
                    avatarId = -1;
                    avatarUrl = String.Empty;
                }

                int authorId = Convert.ToInt32(node.ChildNodes[0].Attributes["id"].Value);
                int authorGroupId;

                try
                {
                    authorGroupId = Convert.ToInt32(node.ChildNodes[0].Attributes["group-id"].Value);
                }
                catch (NullReferenceException)
                {
                    authorGroupId = -1;
                }

                string authorName = node.ChildNodes[0].InnerText;
                Avatar avatar = new Avatar(avatarId, avatarUrl);

                User author = new User(authorId, authorName, authorGroupId, avatar);

                double unixTimeStamp = Convert.ToInt32(node.ChildNodes[1].Attributes["timestamp"].Value);
                DateTime creationDate = Helper.UnixTimeStampToDateTime(unixTimeStamp);

                int editedCount = Convert.ToInt32(node.ChildNodes[2].ChildNodes[0].Attributes["count"].Value);

                string message = node.ChildNodes[2].ChildNodes[1].InnerText;
                string title = node.ChildNodes[2].ChildNodes[2].InnerText;

                if (editedCount > 0)
                {
                    string editUserName =
                        node.ChildNodes[2].ChildNodes[0].ChildNodes[0].ChildNodes[0].InnerText;
                    int editUserId =
                        Convert.ToInt32(
                            node.ChildNodes[2].ChildNodes[0].ChildNodes[0].ChildNodes[0].Attributes["id"].Value);
                    double editTimeStamp =
                        Convert.ToInt32(
                            node.ChildNodes[2].ChildNodes[0].ChildNodes[0].ChildNodes[1].Attributes["timestamp"].Value);

                    User editUser = new User(editUserId, editUserName);
                    Edit lastEdit = new Edit(editUser, editTimeStamp);

                    posts.Add(new Post(id, inThreadId, inBoardId, page, offset, author, creationDate, message, title,
                        editedCount, lastEdit));
                }
                else
                {
                    posts.Add(new Post(id, inThreadId, inBoardId, page, offset, author, creationDate, message, title,
                        editedCount));
                }
            }

            return posts;
        }
        #endregion
    }
}
