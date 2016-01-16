namespace mods.de_XML_Parser
{
    public struct User
    {
        #region private fields
        private int id;
        private string name;
        private int groupId;
        private Avatar avatar;
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

        public int GorupId
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
        public User(int _userId, string _userName, int _groupId, Avatar _avatar)
        {
            id = _userId;
            name = _userName;
            avatar = _avatar;
            groupId = _groupId;
        }
        #endregion
    }
}
