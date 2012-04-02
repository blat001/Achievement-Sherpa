using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AchievementSherpa.Business;


namespace AchievementSherpa.Business
{
    public class Achievement
    {
        public const int HordeOnly = 1;
        public const int AllianceOnly = 2;
        public const int BothSides = 3;
        public const string FeatsOfStrengthCategory = "81";
        public string _id
        {
            get;
            set;
        }

        public Achievement()
        {
            Chained = new List<Achievement>();
            Criteria = new List<string>();
        }
        public int Points
        {
            get;
            set;
        }

        public int Rank
        {
            get;
            set;
        }

        public int SeriesOrder
        {
            get;
            set;
        }

        public AchievementSeries Series
        {
            get;
            set;
        }

        public string Icon
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public bool HasBeenAchieved
        {
            get;
            set;
        }


        public IList<string> Criteria
        {
            get;
            set;
        }

        public string ID
        {
            get;
            set;
        }

        public string BlizzardID
        {
            get;
            set;
        }

        public string Category
        {
            get;
            set;
        }

        public string ParentCategory
        {
            get;
            set;
        }

        public bool IsPartOfSeries
        {
            get
            {
                return Series != null;
            }
        }

        public string Type
        {
            get;
            set;
        }

        public int Side
        {
            get;
            set;
        }




        public IList<Achievement> Chained
        {
            get;
            set;
        }

        public string Description { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is Achievement)
            {
                return _id == ((Achievement)obj)._id;
            }
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return _id.GetHashCode();
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
