CREATE TABLE [dbo].[MergeData]
(
	FixtureId BIGINT NOT NULL PRIMARY KEY NONCLUSTERED,
	FixtureName VARCHAR(MAX) NOT NULL, 
	TotalBooks INT NOT NULL,
	TotalOffers INT NOT NULL, 
	AvgOffersPerBook INT NOT NULL,
	MaxOffersPerBook INT NOT NULL, 
	BooksOver30Offers INT NOT NULL,
)
