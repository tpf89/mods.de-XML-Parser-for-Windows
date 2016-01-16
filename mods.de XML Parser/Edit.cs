using System;

namespace mods.de_XML_Parser
{
    /// <summary>
    /// Contains all information of an edit (only the last of each post is available)
    /// xml/thread.php offers all the information we need for this
    /// </summary>
    public struct Edit
    {
        #region private field
        private User user;
        private DateTime date;
        #endregion

        #region Properties
        public User User
        {
            get
            {
                return user;
            }
        }

        public DateTime Date
        {
            get
            {
                return date;
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a edit object
        /// </summary>
        /// <param name="_user">User who wrote the edit</param>
        /// <param name="_unixTimeStamp">Unix timestamp of the forum</param>
        public Edit(User _user, double _unixTimeStamp)
        {
            user = _user;
            date = Helper.UnixTimeStampToDateTime(_unixTimeStamp);
        }
        #endregion
    }
}
