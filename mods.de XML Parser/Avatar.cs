namespace mods.de_XML_Parser
{
    public struct Avatar
    {
        #region private fields
        int id;
        string url;
        #endregion

        #region Properties
        public int Id
        {
            get
            {
                return id;
            }
        }

        public string Url
        {
            get
            {
                return url;
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a avatar struct
        /// </summary>
        /// <param name="_id">Avatar id</param>
        /// <param name="_url">Avatar url</param>
        public Avatar(int _id, string _url)
        {
            id = _id;
            url = _url;
        }
        #endregion
    }
}
