using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;

namespace Tatts.NextGen.StatsData
{
    public class MergeDataMap : ClassMap<MergeData>
    {
        public MergeDataMap()
        {
           Id( x => x.FixtureId);
           Map( x => x.FixtureName); 
           Map( x => x.TotalBooks);
           Map( x => x.TotalOffers);
           Map( x => x.AvgOffersPerBook); 
           Map( x => x.MaxOffersPerBook); 
           Map( x => x.BooksOver30Offers);
           Table("MergeData");
        }
    }
}
