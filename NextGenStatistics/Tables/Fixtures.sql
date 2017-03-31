CREATE TABLE [dbo].[Fixtures]
(
	[FixtureId] INT NOT NULL PRIMARY KEY NONCLUSTERED
	, FixtureName VARCHAR(MAX) NOT NULL
	, GeneralUpdates INT NOT NULL
	, GeneralAvg INT NOT NULL
	, GeneralMin INT NOT NULL
	, GeneralLQ INT NOT NULL
	, GeneralMed INT NOT NULL
	, GeneralUQ INT NOT NULL
	, GeneralMax INT NOT NULL
	, SnapshotUpdates INT NOT NULL
	, SnapshotAvg INT NOT NULL
	, SnapshotMin INT NOT NULL
	, SnapshotLQ INT NOT NULL
	, SnapshotMed INT NOT NULL
	, SnapshotUQ INT NOT NULL
	, SnapshotMax INT NOT NULL
)
