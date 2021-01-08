using System;
using System.Collections.Generic;

namespace Galatee.Silverlight.Library.Models
{
    public abstract class Option
    {
        private string _name;
        private int? _id;
        private int? _pKId;
        private int? _IdGroupProgram;
        private int? _prgramID;
        private int? _MenuID;
        private int? _MainMenuID;

        public Option(string name, int? moduleId, int? pProgramId, int? MainMenuId, int? menuId, int? pkid, string pUSERCREATION, DateTime? pDATECREATION)
        {
            _name = name;
            IdGroupProgram = moduleId;
            PrgramID = pProgramId;
            MainMenuID = MainMenuId;
            MenuID = menuId;
            _pKId = pkid;
            USERCREATION = pUSERCREATION;
            DATECREATION = pDATECREATION;
        }

        public string USERCREATION { get; set; }

        public DateTime? DATECREATION { get; set; }

        public int? MenuID
        {
            get { return _MenuID; }
            set { _MenuID = value; }
        }

        public string Name
        {
		    get { return _name; }
        }

        public int? MainMenuID
        {
            get { return _MainMenuID; }
            set { _MainMenuID = value; }
        }

        public int? PrgramID
        {
            get { return _prgramID; }
            set { _prgramID = value; }
        }

        public int? IdGroupProgram
        {
            get { return _IdGroupProgram; }
            set { _IdGroupProgram = value; }
        }

        public int? Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public int? PkId
        {
            get { return _pKId; }
            set { _pKId = value; }
        }


        public abstract IEnumerable<Option> Children { get; }
        public abstract IEnumerable<OptionLeaf> Leaves { get; }
    }
}
