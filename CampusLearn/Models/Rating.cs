using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;

namespace CampusLearn.Models
{
    public class Rating
    {
        public long RatingID { get; set; }
        public short RatingValue { get; set; }

        public ICollection<SessionRating> SessionRatings { get; set; } = new List<SessionRating>();
    }
}
