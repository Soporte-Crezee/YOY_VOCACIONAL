using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace POV.Web.PortalSocial.AppCode.Controls
{
    [Serializable()]
    public class MenuItem
    {
        private string item;

        private string icon;

        private string link;

        private long id;

        private List<MenuItem> children;

        //private List<Menu

        public MenuItem(string item, string image, string link)
        {

            Children = new List<MenuItem>();
            this.Item = item;
            this.Icon = image;
            this.Link = link;

        }

        public MenuItem(long id, string item, string image, string link)
        {

            Children = new List<MenuItem>();
            this.Item = item;
            this.Icon = image;
            this.Link = link;
            this.ID = id;

        }

        public string Item
        {
            get
            {
                return this.item;
            }
            private set 
            {
                this.item = value;
            }
        }

        public string Icon
        {
            get
            {
                return this.icon;
            }
            private set
            {
                this.icon = value;
            }
        }

        public string Link
        {
            get
            {
                return this.link;
            }
            private set
            {
                this.link = value;
            }
        }

        public long ID
        {
            get
            {
                return this.id;
            }
            private set
            {
                this.id = value;
            }
        }

        public List<MenuItem> Children 
        { 
            get {
                return this.children;
            }
            private set
            {
                this.children = value;
            }
        }
    }
}