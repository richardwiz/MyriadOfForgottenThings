using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tatts.NextGen.StatsData
{
    public class MergeData
    {
        public virtual Int32 FixtureId { get; protected set; }
        public virtual String FixtureName { get; protected set; }
        public virtual Int32 TotalBooks { get; protected set; }
        public virtual Int32 TotalOffers { get; protected set; }
        public virtual Int32 AvgOffersPerBook { get; protected set; }
        public virtual Int32 MaxOffersPerBook { get; protected set; }
        public virtual Int32 BooksOver30Offers { get; protected set; }

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
