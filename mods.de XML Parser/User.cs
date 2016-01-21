namespace mods.de_XML_Parser
{
    /// <summary>
    /// All user information are stored in this struct
    /// </summary>
    public struct User
    {
        #region private fields
        private readonly int id;
        private readonly string name;
        private readonly int groupId;
        private readonly Avatar avatar;
        private readonly bool banned;
        #endregion

        #region Properties
        public int Id
        {
            get
            {
                return id;
            }
        }

        public string Name
        {
            get
            {
                return name;
            }

        }

        public int GroupId
        {
            get
            {
                return groupId;
            }
        }

        public Avatar Avatar
        {
            get
            {
                return avatar;
            }
        }

        public bool Banned
        {
            get
            {
                return banned;
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a User struct
        /// This constructor is needed when getting the THREAD CREATOR out of /xml/Thread.php
        /// </summary>
        /// <param name="_userId">User id</param>
        /// <param name="_userName">User nam</param>
        public User(int _userId, string _userName)
        {
            id = _userId;
            name = _userName;
            banned = false;
            groupId = -1;
            avatar = new Avatar(-1, null);
        }

        /// <summary>
        /// Creates a User struct
        /// This constructor is needed when getting the POST AUTHOR out of /xml/Thread.php
        /// </summary>
        /// <param name="_userId"></param>
        /// <param name="_userName"></param>
        /// <param name="_groupId"></param>
        /// <param name="_avatar"></param>
        public User(int _userId, string _userName, int _groupId, Avatar _avatar, bool _banned)
        {
            id = _userId;
            name = _userName;
            avatar = _avatar;
            banned = _banned;
            groupId = _groupId;
            banned = _banned;
        }
        #endregion
    }
}
