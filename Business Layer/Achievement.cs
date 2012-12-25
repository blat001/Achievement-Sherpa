using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AchievementSherpa.Business;


namespace AchievementSherpa.Business
{
    public enum AchievementCategories
    {
        General = 92,
        Quests = 96,
        Exploration = 97,
        PlayerVsPlayer = 95,
        DungeonsAndRaids = 168,
        Professions = 169,
        Scenarios = 15165,
        Reputation = 201,
        WorldEvents = 155,
        PetBattles = 15117

    }

    public class Achievement
    {
        public const int HordeOnly = 1;
        public const int AllianceOnly = 0;
        public const int BothSides = 2;
        public const string FeatsOfStrengthCategory = "81";
        public string _id
        {
            get;
            set;
        }

        public Achievement()
        {
            Chained = new List<Achievement>();
            Criteria = new List<int>();
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


        public IList<int> Criteria
        {
            get;
            set;
        }

        

        public int BlizzardID
        {
            get;
            set;
        }

        public string Category
        {
            get;
            set;
        }

        public int ParentCategoryID
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

        public int CategoryID { get; set; }
    }
}
