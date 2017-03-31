using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;

namespace Torqueo.Mappings
{
    public class SpinForsetiMappingMap : ClassMap<SpinForsetiMapping>
    {
        public SpinForsetiMappingMap()
        {
            Id(x => x.Id);
            Map(x => x.BetTypeId);
            Map(x => x.CloseOnPending);
            Map(x => x.DateFinalised);
            Map(x => x.DisconnectedFixture);
            Map(x => x.EventFinalised);
            Map(x => x.FixtureId);
            Map(x => x.ForsetiId);
            Map(x => x.ForsetiName);
            Map(x => x.ForsetiNameShort);
            Map(x => x.ImportSetupState);
            Map(x => x.IsLive);
            Map(x => x.LeagueId);
            Map(x => x.MainEventId);
            Map(x => x.MeetingId);
            Map(x => x.MeetingPrefix);
            Map(x => x.OfferExample);
            Map(x => x.OffsetInMinutes);
            Map(x => x.RequestSnapshot);
            Map(x => x.Result);
            Map(x => x.Sequence);
            Map(x => x.SPINId);
            Map(x => x.SPINName);
            Map(x => x.SPINUniqueIdentifierTag);
            Map(x => x.SportId);
            Map(x => x.StopResulting);
            Map(x => x.StopTransmission);
            Map(x => x.SubEventId);
            Map(x => x.Type);
            Table("SpinForsetiMapping");
        }
    }
}
