using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AchievementSherpa.Business
{
    [Serializable]
    public class Guild
    {
        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        private string _server;

        public string Server
        {
            get { return _server; }
            set { _server = value; }
        }

        public Guild(string server, string name)
        {
            // TODO: Complete member initialization
            this._server = server;
            this._name = name;
        }
    }
}
