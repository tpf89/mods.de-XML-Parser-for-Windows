using System;
using System.Collections.Generic;
using System.Xml;

namespace mods.de_XML_Parser
{
    /// <summary>
    /// Contains all information of a category
    /// xml/boards.php offers all information we need
    /// </summary>
    public struct Category
    {
        #region private fields
        int id;
        string name;
        private string description;
        private List<int> boardIds;
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

        public string Description
        {
            get
            {
                return description;
            }
        }

        public List<int> BoardIds
        {
            get
            {
                return boardIds;
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a category struct
        /// </summary>
        /// <param name="_id">Category id</param>
        /// <param name="_name">Category name</param>
        /// <param name="_description">Category description</param>
        /// <param name="_boardIds">List of all board ids in this category</param>
        public Category(int _id, string _name, string _description, List<int> _boardIds)
        {
            id = _id;
            name = _name;
            description = _description;
            boardIds = _boardIds;
        }
        #endregion

        #region Static methods
        /// <summary>
        /// iterates through /xml/boards.php and returns a List with all categories and boards in it
        /// </summary>
        /// <returns>A list of all categories</returns>
        public static List<Category> GetCategories()
        {
            var xmlFile = "http://forum.mods.de/bb/xml/boards.php";
            var xmlDoc = Helper.LoadXml(xmlFile);

            if (xmlDoc != null)
            {
                int categoryCount = xmlDoc.DocumentElement.ChildNodes.Count;
                List<Category> allCAtegories = new List<Category>(categoryCount);

                foreach (XmlNode category in xmlDoc.DocumentElement.ChildNodes)
                {
                    int id = Convert.ToInt32(category.Attributes[0].Value);
                    string name = category.ChildNodes[0].InnerText;
                    string description = category.ChildNodes[1].InnerText;
                    int boardCount = 0;

                    if (category.ChildNodes.Count > 2)
                    {
                        boardCount = category.ChildNodes[2].ChildNodes.Count;
                    }

                    List<int> boardIds = new List<int>(boardCount);

                    if (boardCount != 0)
                    {
                        foreach (XmlNode board in category.ChildNodes[2].ChildNodes)
                        {
                            int boardId = Convert.ToInt32(board.Attributes[0].Value);
                            boardIds.Add(boardId);
                        }
                    }

                    allCAtegories.Add(new Category(id, name, description, boardIds));
                }

                return allCAtegories;
            }

            return null;
        }
        #endregion
    }
}
