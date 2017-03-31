CREATE VIEW [dbo].[SpinFixtureLog]
	AS SELECT
	MD.FixtureId AS FixtureId, 
	MD.FixtureName AS ForsetiFixtureName,
	ID.FixtureName AS AdapterFixtureName, 
	MD.TotalBooks AS TotalBooks,
	MD.TotalOffers AS TotalOffers, 
	MD.AvgOffersPerBook AS AvgOffersPerBook,
	MD.MaxOffersPerBook AS MaxOffersPerBook, 
	MD.BooksOver30Offers AS BooksOver30Offers,
	ID.GeneralUpdates AS GeneralUpdates, 
	ID.GeneralAvg AS GeneralAvg,
	ID.GeneralMin AS GeneralMin, 
	ID.GeneralLQ AS GeneralLQ,
	ID.GeneralMed AS GeneralMed, 
	ID.GeneralUQ AS GeneralUQ,
	ID.GeneralMax AS GeneralMax, 
	ID.SnapshotUpdates AS SnapshotUpdates,
	ID.SnapshotAvg AS SnapshotAvg, 
	ID.SnapshotMin AS SnapshotMin,
	ID.SnapshotLQ AS SnapshotLQ, 
	ID.SnapshotMed AS SnapshotMed,
	ID.SnapshotUQ AS SnapshotUQ, 
	ID.SnapshotMax AS SnapshotMax
	FROM
	MergeData AS MD
	LEFT JOIN   Fixtures AS ID ON MD.FixtureId = ID.FixtureId

