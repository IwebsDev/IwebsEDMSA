using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Galatee.Structure;

namespace WcfService
{
    public class Tree
    {
        private List<Tree> enfants1;

        public List<Tree> Enfants
        {
            get { return enfants1; }
            set { enfants1 = value; }
        }
        private string IDnode1;

        public string IDnode
        {
            get { return IDnode1; }
            set { IDnode1 = value; }
        }
        private string LibelleNode1;

        public string LibelleNode
        {
            get { return LibelleNode1; }
            set { LibelleNode1 = value; }
        }
        private CSMenuGalatee Tag1;
        private int ImageIndex1;

        public int ImageIndex
        {
            get { return ImageIndex1; }
            set { ImageIndex1 = value; }
        }


        public CSMenuGalatee Tag
        {
            get { return Tag1; }
            set { Tag1 = value; }
        }

        public Tree(string libelleNode)
        {
            LibelleNode = libelleNode;
        }

        public void Add(Tree children)
        {
            Enfants.Add(children);
        }
    }
}