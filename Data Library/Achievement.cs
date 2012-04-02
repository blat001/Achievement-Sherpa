using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data_Library
{
    public partial class Achievement
    {

        public bool IsPartOfSeries
        {
            get
            {
                return Series != null;
            }
        }


        public override bool Equals(object obj)
        {
            if (obj is Achievement)
            {
                return ((Achievement)obj).AchievementID == AchievementID;
            }
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return AchievementID.GetHashCode();
        }
    }
}
