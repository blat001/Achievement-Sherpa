using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_UI.Models
{
    public static class ClusterHelper
    {

        public static string GetClusterName(int clusterID)
        {
            switch (clusterID)
            {
                case 1:
                    return "Over Achiever";
                case 4:
                    return "Over Achiever";
                case 5:
                    return "Raider";
                default :
                    return "Unknown";

            }
        }
    }
}