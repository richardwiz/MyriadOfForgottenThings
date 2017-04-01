using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text.RegularExpressions;
using Tatts.NextGen.SpinStats.Enums;
using log4net;

namespace Tatts.NextGen.SpinStats
{

    public partial class SpinStatsService : ServiceBase
    {
        static List<TimeStampedLine> lines = new List<TimeStampedLine>();
        static List<Update> updates = new List<Update>();
        static Dictionary<int, Update> pendingUpdates = new Dictionary<int, Update>();
        static List<Fixture> fixtures = new List<Fixture>();
        public ILog Logger { get; set; }

        public SpinStatsService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            String logDirectory = ConfigurationManager.AppSettings["LogLocation"].ToString();
            LoadData(logDirectory);
        }

        protected override void OnStop()
        {
        }

        static void LoadData(string directoryPath)
        {
            Console.WriteLine("");
            Console.WriteLine("Merging & sorting data...");

            string[] paths = Directory.GetFiles(directoryPath);

            string basicPattern = @"([0-9]+-[0-9]+-[0-9]+ [0-9]+:[0-9]+:[0-9]+,[0-9]+).+";
            Regex basixRegex = new Regex(basicPattern, RegexOptions.None);

            int fileCount = 0;
            int lineCount = 0;

            foreach (string path in paths)
            {
                if (path.Contains("TattsPlugin"))
                {
                    string line;

                    System.IO.StreamReader file = new System.IO.StreamReader(path);
                    while ((line = file.ReadLine()) != null)
                    {
                        Match basicMatch = basixRegex.Match(line);
                        if (basicMatch.Success)
                        {
                            DateTime timestamp = DateTime.Parse(basicMatch.Groups[1].Value.Replace(',', '.'));

                            lines.Add(new TimeStampedLine(timestamp, line));
                        }

                        lineCount++;
                    }

                    file.Close();

                    fileCount++;
                }
            }

            Console.WriteLine(string.Format("Merged & sorted {0:n0} lines from {1:n0} log files.", lineCount, fileCount));
        }

        static void ParseData()
        {
            Console.WriteLine("");
            Console.WriteLine("Parsing data...");

            foreach (TimeStampedLine timeStampedLine in lines.OrderBy(o => o.TimeStamp))
            {
                string line = timeStampedLine.Value;

                Match match = null;
                LineType type = LineParser.ParseLine(line, out match);

                switch (type)
                {
                    case LineType.UpdateStart:
                        ProcessUpdateStart(match, UpdateType.General);
                        break;

                    case LineType.SnapshotStart:
                        ProcessUpdateStart(match, UpdateType.Snapshot);
                        break;

                    case LineType.UpdateComplete:
                    case LineType.SnapshotComplete:
                        ProcessUpdateComplete(match);
                        break;

                    case LineType.MarketThread:
                        ProcessMarketThread(match);
                        break;

                    case LineType.MarketSummary:
                        ProcessMarketSummary(match);
                        break;

                    case LineType.ResultMessage:
                        ProcessResultMessage(match);
                        break;

                    default:
                        break;
                }
            }

            while (pendingUpdates.Count > 0)
            {
                KeyValuePair<int, Update> record = pendingUpdates.First();

                Update update = record.Value;

                update.State = UpdateState.Misaligned;
                update.Finalise(DateTime.MaxValue);

                updates.Add(update);

                pendingUpdates.Remove(record.Key);
            }

            foreach (long fixtureId in updates.Where(o => o.State == UpdateState.OK && o.FixtureId != long.MinValue).Select(o => o.FixtureId).Distinct())
            {
                Fixture fixture = new Fixture(fixtureId);

                fixture.Updates.AddRange(updates.Where(o => o.FixtureId == fixtureId && o.State == UpdateState.OK).OrderBy(o => o.StartTime));
                fixture.FixtureName = fixture.Updates.Last().FixtureName;

                List<Update> general = fixture.Updates.Where(o => o.Type == UpdateType.General).ToList();
                List<Update> generalZMU = general.Where(o => o.ObservedMarketUpdates == 0).ToList();

                fixture.GeneralUpdates = general.Count;
                fixture.GeneralUpdatesZMU = generalZMU.Count;
                fixture.GeneralUpdateTimeAvg = (general.Count > 0) ? Math.Round(general.Select(o => o.Duration).Average()) : 0;
                fixture.GeneralUpdateTimeMin = (general.Count > 0) ? Math.Round(general.Select(o => o.Duration).Min()) : 0;
                fixture.GeneralUpdateTimeMax = (general.Count > 0) ? Math.Round(general.Select(o => o.Duration).Max()) : 0;
                fixture.GeneralUpdateTimeMed = (general.Count > 0) ? Math.Round(general.Select(o => o.Duration).Median()) : 0;

                List<double> generalLQS = general.Where(o => o.Duration < fixture.GeneralUpdateTimeMed).Select(o => o.Duration).ToList();
                List<double> generalUQS = general.Where(o => o.Duration > fixture.GeneralUpdateTimeMed).Select(o => o.Duration).ToList();

                fixture.GeneralUpdateTimeLQ = (generalLQS.Count > 0) ? Math.Round(generalLQS.Median()) : fixture.GeneralUpdateTimeMed;
                fixture.GeneralUpdateTimeUQ = (generalUQS.Count > 0) ? Math.Round(generalUQS.Median()) : fixture.GeneralUpdateTimeMed;

                List<Update> snapshot = fixture.Updates.Where(o => o.Type == UpdateType.Snapshot).ToList();
                List<Update> snapshotZMU = snapshot.Where(o => o.ObservedMarketUpdates == 0).ToList();

                fixture.SnapshotUpdates = snapshot.Count;
                fixture.SnapshotUpdatesZMU = snapshotZMU.Count;
                fixture.SnapshotUpdateTimeAvg = (snapshot.Count > 0) ? Math.Round(snapshot.Select(o => o.Duration).Average()) : 0;
                fixture.SnapshotUpdateTimeMin = (snapshot.Count > 0) ? Math.Round(snapshot.Select(o => o.Duration).Min()) : 0;
                fixture.SnapshotUpdateTimeMax = (snapshot.Count > 0) ? Math.Round(snapshot.Select(o => o.Duration).Max()) : 0;
                fixture.SnapshotUpdateTimeMed = (snapshot.Count > 0) ? Math.Round(snapshot.Select(o => o.Duration).Median()) : 0;

                List<double> snapshotLQS = snapshot.Where(o => o.Duration < fixture.SnapshotUpdateTimeMed).Select(o => o.Duration).ToList();
                List<double> snapshotUQS = snapshot.Where(o => o.Duration > fixture.SnapshotUpdateTimeMed).Select(o => o.Duration).ToList();

                fixture.SnapshotUpdateTimeLQ = (snapshotLQS.Count > 0) ? Math.Round(snapshotLQS.Median()) : fixture.SnapshotUpdateTimeMed;
                fixture.SnapshotUpdateTimeUQ = (snapshotUQS.Count > 0) ? Math.Round(snapshotUQS.Median()) : fixture.SnapshotUpdateTimeMed;

                fixtures.Add(fixture);
            }

            long marketUpdateCount = updates.Where(o => o.State == UpdateState.OK).Sum(o => o.ObservedMarketUpdates);

            Console.WriteLine(string.Format("Identified & parsed {0:n0} fixture updates containing {1:n0} market updates across {2:n0} fixtures:", updates.Count, marketUpdateCount, fixtures.Count));
            Console.WriteLine(string.Format("OK: {0:n0}, Misaligned: {1:n0}, Unmatched: {2:n0}.", updates.Where(o => o.State == UpdateState.OK).Count(), updates.Where(o => o.State != UpdateState.OK).Count(), updates.Where(o => o.FixtureId == long.MinValue).Count()));
        }

        static void ProcessUpdateStart(Match match, UpdateType type)
        {
            DateTime timeStamp = DateTime.Parse(match.Groups[1].Value.Replace(',', '.'));
            int threadId = int.Parse(match.Groups[2].Value);
            string fixtureName = match.Groups[3].Value;

            Update update = null;
            if (pendingUpdates.ContainsKey(threadId) && pendingUpdates.TryGetValue(threadId, out update))
            {
                update.State = UpdateState.Misaligned;
                update.Finalise(timeStamp);

                updates.Add(update);

                pendingUpdates.Remove(threadId);
                update = null;
            }

            update = new Update(timeStamp, threadId, type, fixtureName);
            pendingUpdates.Add(threadId, update);
        }

        static void ProcessUpdateComplete(Match match)
        {
            DateTime timeStamp = DateTime.Parse(match.Groups[1].Value.Replace(',', '.'));
            int threadId = int.Parse(match.Groups[2].Value);
            string fixtureName = match.Groups[3].Value;

            Update update = null;
            if (pendingUpdates.ContainsKey(threadId) && pendingUpdates.TryGetValue(threadId, out update))
            {
                update.State = UpdateState.OK;
                update.Finalise(timeStamp);

                updates.Add(update);

                pendingUpdates.Remove(threadId);
            }
        }

        static void ProcessMarketThread(Match match)
        {
            DateTime timeStamp = DateTime.Parse(match.Groups[1].Value.Replace(',', '.'));
            int threadId = int.Parse(match.Groups[2].Value);
            long fixtureId = long.Parse(match.Groups[3].Value);
            int marketCounter = int.Parse(match.Groups[4].Value);
            int workerId = int.Parse(match.Groups[5].Value);

            Update update = null;
            if (pendingUpdates.ContainsKey(threadId) && pendingUpdates.TryGetValue(threadId, out update))
            {
                if (update.FixtureId == long.MinValue)
                {
                    update.FixtureId = fixtureId;
                }
                else if (update.FixtureId != fixtureId)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("ERROR: Thread Alignment Error Detected!");
                    Console.ForegroundColor = ConsoleColor.White;

                    return;
                }

                update.ObservedMarketUpdates++;
            }
        }

        static void ProcessMarketSummary(Match match)
        {
            DateTime timeStamp = DateTime.Parse(match.Groups[1].Value.Replace(',', '.'));
            int threadId = int.Parse(match.Groups[2].Value);
            long fixtureId = long.Parse(match.Groups[3].Value);
            int marketCounter1 = int.Parse(match.Groups[4].Value);
            int marketCounter2 = int.Parse(match.Groups[5].Value);

            Update update = null;
            if (pendingUpdates.ContainsKey(threadId) && pendingUpdates.TryGetValue(threadId, out update))
            {
                if (update.FixtureId != fixtureId)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("ERROR: Thread alignment error detected!");
                    Console.ForegroundColor = ConsoleColor.White;

                    update.State = UpdateState.Misaligned;

                    return;
                }

                update.DeclaredMarketUpdates = marketCounter1;

                if (update.ObservedMarketUpdates != update.DeclaredMarketUpdates)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("WARN: Discrepency in observed versus declared market updates!");
                    Console.ForegroundColor = ConsoleColor.White;

                    update.State = UpdateState.Misaligned;
                }
            }
        }

        static void ProcessResultMessage(Match match)
        {
            DateTime timeStamp = DateTime.Parse(match.Groups[1].Value.Replace(',', '.'));
            int threadId = int.Parse(match.Groups[2].Value);
            long fixtureId = long.Parse(match.Groups[3].Value);

            Update update = null;
            if (pendingUpdates.ContainsKey(threadId) && pendingUpdates.TryGetValue(threadId, out update))
            {
                if (update.FixtureId == long.MinValue)
                {
                    update.FixtureId = fixtureId;
                }
                else if (update.FixtureId != fixtureId)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("ERROR: Thread Alignment Error Detected!");
                    Console.ForegroundColor = ConsoleColor.White;

                    return;
                }
            }
        }

    }
}
