using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AchievementSherpa.Business;

namespace Web_UI.Models
{
    public class CharacterList
    {
        public int TotalNumber
        {
            get;
            set;
        }

        public int CurrentPage
        {
            get;
            set;
        }

        public int PageSize
        {
            get;
            set;
        }

        public bool HasPrevious
        {
            get
            {
                if (CurrentPage > 0)
                {
                    return true;
                }

                return false;
            }
        }

        public bool HasNext
        {
            get
            {
                if ((CurrentPage + 1 * PageSize) >= TotalNumber)
                {
                    return false;
                }

                return true;
            }
        }

        public IList<Character> Characters
        {
            get;
            set;
        }
    }
}