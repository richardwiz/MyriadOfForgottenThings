using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tatts.NextGen.StatsData
{
    public class MergeData
    {
        public Int32 FixtureId { get; set; }
        public String FixtureName { get; set; }
        public Int32 TotalBooks { get; set; }
        public Int32 TotalOffers { get; set; }
        public Int32 AvgOffersPerBook { get; set; }
        public Int32 MaxOffersPerBook { get; set; }
        public Int32 BooksOver30Offers { get; set; }

        public MergeData()
        {
            this.FixtureId = 0;
            this.FixtureName = String.Empty;
            this.TotalBooks = 0;
            this.TotalOffers = 0;
            this.AvgOffersPerBook = 0;
            this.MaxOffersPerBook = 0;
            this.BooksOver30Offers = 0;

        }
    }
}
