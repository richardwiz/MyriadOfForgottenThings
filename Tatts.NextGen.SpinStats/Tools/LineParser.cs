using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Tatts.NextGen.SpinStats.Enums;

namespace Tatts.NextGen.SpinStats
{
    public class LineParser
    {
        protected static Regex UpdateStart      = new Regex(@"([0-9]+-[0-9]+-[0-9]+ [0-9]+:[0-9]+:[0-9]+,[0-9]+) \[([0-9]+)\].+Starting Plugin - PluginProcessStreamUpdate: (.+) \*.+", RegexOptions.Compiled);
        protected static Regex UpdateComplete   = new Regex(@"([0-9]+-[0-9]+-[0-9]+ [0-9]+:[0-9]+:[0-9]+,[0-9]+) \[([0-9]+)\].+Finished Plugin - PluginProcessStreamUpdate: (.+) \*.+", RegexOptions.Compiled);
        protected static Regex SnapshotStart    = new Regex(@"([0-9]+-[0-9]+-[0-9]+ [0-9]+:[0-9]+:[0-9]+,[0-9]+) \[([0-9]+)\].+Plugin Starting - PluginProcessSnapshot: (.+) \*.+", RegexOptions.Compiled);
        protected static Regex SnapshotComplete = new Regex(@"([0-9]+-[0-9]+-[0-9]+ [0-9]+:[0-9]+:[0-9]+,[0-9]+) \[([0-9]+)\].+Plugin Finished - PluginProcessSnapshot: (.+) \*.+", RegexOptions.Compiled);
        protected static Regex MarketThread     = new Regex(@"([0-9]+-[0-9]+-[0-9]+ [0-9]+:[0-9]+:[0-9]+,[0-9]+) \[([0-9]+)\].+>>>>>>>>>>>>> ([0-9]+) countMarket:([0-9]+) i:([0-9]+) Market.+", RegexOptions.Compiled);
        protected static Regex MarketSummary    = new Regex(@"([0-9]+-[0-9]+-[0-9]+ [0-9]+:[0-9]+:[0-9]+,[0-9]+) \[([0-9]+)\].+>>>>>>>>>>>>> FINISHED Tasks ([0-9]+) - #Mkts: ([0-9]+) countMarket: ([0-9]+).*", RegexOptions.Compiled);
        protected static Regex ResultsMessage = new Regex(@"([0-9]+-[0-9]+-[0-9]+ [0-9]+:[0-9]+:[0-9]+,[0-9]+) \[([0-9]+)\].+@@@@ Processing Results for Main Event ([0-9]+) @@@@.*", RegexOptions.Compiled);
        protected static Regex NoResultsIndicator = new Regex(@"([0-9]+-[0-9]+-[0-9]+ [0-9]+:[0-9]+:[0-9]+,[0-9]+) \[([0-9]+)\].+@@@@ No markets to result @@@@.*", RegexOptions.Compiled);
        protected static Regex OfferMapping = new Regex(@"([0-9]+-[0-9]+-[0-9]+ [0-9]+:[0-9]+:[0-9]+,[0-9]+) \[([0-9]+)\].+Updated SpinForsetiMapping FOfferSSelection.*", RegexOptions.Compiled);
        protected static Regex OfferSelectionChange = new Regex(@"([0-9]+-[0-9]+-[0-9]+ [0-9]+:[0-9]+:[0-9]+,[0-9]+) \[([0-9]+)\].+Updating offer SelectionId:.*", RegexOptions.Compiled);

        public static LineType ParseLine(string line, out Match match)
        {
            // Order of match execution was decided by likelihood of match.
            if(line.Contains("Updating offer SelectionId:"))
            {
                Match offerSelectionMatch = OfferSelectionChange.Match(line);
                if(offerSelectionMatch.Success)
                {
                    match = offerSelectionMatch;
                    return LineType.OfferSelectionChange;
                }
            }
            if(line.Contains("Updated SpinForsetiMapping FOfferSSelection"))
            {
                Match mappingUpdateMatch = OfferMapping.Match(line);
                if(mappingUpdateMatch.Success)
                {
                    match = mappingUpdateMatch;
                    return LineType.OfferMapping;
                }
            }
            if(line.Contains("@@@@ No markets to result @@@@"))
            {
                Match noResultsMatch = NoResultsIndicator.Match(line);
                if(noResultsMatch.Success)
                {
                    match = noResultsMatch;
                    return LineType.NoResultsIndicator;
                }
            }
            if(line.Contains("Processing Results for Main Event"))
            {
                Match matchResultsMessage = ResultsMessage.Match(line);
                if(matchResultsMessage.Success)
                {
                    match = matchResultsMessage;
                    return LineType.ResultMessage;
                }
            }

            if (line.Contains("countMarket:"))
            { 
                Match matchMarketThread = MarketThread.Match(line);
                if (matchMarketThread.Success)
                {
                    match = matchMarketThread;
                    return LineType.MarketThread;
                }
            }

            if (line.Contains("Starting Plugin - PluginProcessStreamUpdate:"))
            {
                Match matchUpdateStart = UpdateStart.Match(line);
                if (matchUpdateStart.Success)
                {
                    match = matchUpdateStart;
                    return LineType.UpdateStart;
                }
            }

            if (line.Contains("Finished Plugin - PluginProcessStreamUpdate:"))
            {
                Match matchUpdateComplete = UpdateComplete.Match(line);
                if (matchUpdateComplete.Success)
                {
                    match = matchUpdateComplete;
                    return LineType.UpdateComplete;
                }
            }

            if (line.Contains(">>>>>>>>>>>>> FINISHED Tasks"))
            {
                Match matchMarketSummary = MarketSummary.Match(line);
                if (matchMarketSummary.Success)
                {
                    match = matchMarketSummary;
                    return LineType.MarketSummary;
                }
            }

            if (line.Contains("Plugin Starting - PluginProcessSnapshot:"))
            {
                Match matchSnapshotStart = SnapshotStart.Match(line);
                if (matchSnapshotStart.Success)
                {
                    match = matchSnapshotStart;
                    return LineType.SnapshotStart;
                }
            }

            if (line.Contains("Plugin Finished - PluginProcessSnapshot:"))
            {
                Match matchSnapshotComplete = SnapshotComplete.Match(line);
                if (matchSnapshotComplete.Success)
                {
                    match = matchSnapshotComplete;
                    return LineType.SnapshotComplete;
                }
            }

            match = null;
            return LineType.None;
        }

    }
}
