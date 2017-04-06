CREATE VIEW [dbo].[vw_MergeData] 
AS
	SELECT
		InnerSelect.Fixture AS Fixture, 
		MAX(InnerSelect.FixtureName) AS FixtureName,
		COUNT(DISTINCT InnerSelect.Book) AS TotalBooks,
		SUM(InnerSelect.Offers) AS TotalOffers,
		AVG(InnerSelect.Offers) AS AvgOffersPerBook,
		MAX(InnerSelect.Offers) AS MaxOffersPerBook,
		SUM(CASE WHEN InnerSelect.Offers >= 30 THEN 1 ELSE 0 END) AS BooksOver30Offers
		FROM
		(
	SELECT
		se.SubEventId AS Book, 
		MAX(se.MainEventId) AS Fixture,
		MAX(me.EventName) AS FixtureName, 
		COUNT(DISTINCT o.OfferId) AS Offers
		FROM
			[$(iBet)].dbo.SubEvent AS se
			LEFT JOIN   [$(iBet)].dbo.Offer AS o ON se.SubEventId = o.SubEventId
			LEFT JOIN   [$(iBet)].dbo.MainEvent AS me ON se.MainEventId = me.MainEventId
		WHERE
		se.MainEventId IN(SELECT fix.FixtureId FROM dbo.Fixtures AS fix)
		AND se.SubEventId IN(SELECT vse.SubEventId FROM dbo.ValidSubEvent AS vse)
		GROUP BY
		se.SubEventId
		) AS InnerSelect
	GROUP BY InnerSelect.Fixture
