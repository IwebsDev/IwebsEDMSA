using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Galatee.Structure;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
    [DataContract]
    public class TreeNodes
    {
        private List<TreeNodes> enfants1=new List<TreeNodes>();

        [DataMember]
        public List<TreeNodes> Enfants
        {
            get { return enfants1; }
            set { enfants1 = value; }
        }

        private string IDnode1;

        [DataMember]
        public string IDnode
        {
            get { return IDnode1; }
            set { IDnode1 = value; }
        }
        private string LibelleNode1;

        [DataMember]
        public string LibelleNode
        {
            get { return LibelleNode1; }
            set { LibelleNode1 = value; }
        }
        private CSMenuGalatee Tag1;

        private int ImageIndex1;


        [DataMember]
        public int ImageIndex
        {
            get { return ImageIndex1; }
            set { ImageIndex1 = value; }
        }

        [DataMember]
        public CSMenuGalatee Tag
        {
            get { return Tag1; }
            set { Tag1 = value; }
        }

        public TreeNodes(string libelleNode)
        {
            LibelleNode = libelleNode;
        }

        public void Add(TreeNodes children)
        {
            Enfants.Add(children);
        }
    }
}