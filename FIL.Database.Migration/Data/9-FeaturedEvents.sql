IF NOT EXISTS(SELECT * FROM FeaturedEvents WHERE SiteId = 1)
BEGIN
INSERT INTO FeaturedEvents (EventId,SortOrder,SiteId,IsAllowedInFooter,IsEnabled,CreatedUtc,CreatedBy)
SELECT TOP 12 Id, ROW_NUMBER() OVER(ORDER BY Id ASC), 1, 0, 1, GETUTCDATE(), 'C043DDEE-D0B1-48D8-9C3F-309A77F44781' FROM Events ORDER BY NEWID()
END

IF NOT EXISTS(SELECT * FROM FeaturedEvents WHERE SiteId = 2)
BEGIN
INSERT INTO FeaturedEvents (EventId,SortOrder,SiteId,IsAllowedInFooter,IsEnabled,CreatedUtc,CreatedBy)
SELECT TOP 12 Id, ROW_NUMBER() OVER(ORDER BY Id ASC), 2, 0, 1, GETUTCDATE(), 'C043DDEE-D0B1-48D8-9C3F-309A77F44781' FROM Events ORDER BY NEWID()
END

IF NOT EXISTS(SELECT * FROM FeaturedEvents WHERE SiteId = 3)
BEGIN
INSERT INTO FeaturedEvents (EventId,SortOrder,SiteId,IsAllowedInFooter,IsEnabled,CreatedUtc,CreatedBy)
SELECT TOP 12 Id, ROW_NUMBER() OVER(ORDER BY Id ASC), 3, 0, 1, GETUTCDATE(), 'C043DDEE-D0B1-48D8-9C3F-309A77F44781' FROM Events ORDER BY NEWID()
END

IF NOT EXISTS(SELECT * FROM FeaturedEvents WHERE SiteId = 4)
BEGIN
INSERT INTO FeaturedEvents (EventId,SortOrder,SiteId,IsAllowedInFooter,IsEnabled,CreatedUtc,CreatedBy)
SELECT TOP 12 Id, ROW_NUMBER() OVER(ORDER BY Id ASC), 4, 0, 1, GETUTCDATE(), 'C043DDEE-D0B1-48D8-9C3F-309A77F44781' FROM Events ORDER BY NEWID()
END

IF NOT EXISTS(SELECT * FROM FeaturedEvents WHERE SiteId = 5)
BEGIN
INSERT INTO FeaturedEvents (EventId,SortOrder,SiteId,IsAllowedInFooter,IsEnabled,CreatedUtc,CreatedBy)
SELECT TOP 12 Id, ROW_NUMBER() OVER(ORDER BY Id ASC), 5, 0, 1, GETUTCDATE(), 'C043DDEE-D0B1-48D8-9C3F-309A77F44781' FROM Events ORDER BY NEWID()
END

IF NOT EXISTS(SELECT * FROM FeaturedEvents WHERE SiteId = 6)
BEGIN
INSERT INTO FeaturedEvents (EventId,SortOrder,SiteId,IsAllowedInFooter,IsEnabled,CreatedUtc,CreatedBy)
SELECT TOP 12 Id, ROW_NUMBER() OVER(ORDER BY Id ASC), 6, 0, 1, GETUTCDATE(), 'C043DDEE-D0B1-48D8-9C3F-309A77F44781' FROM Events ORDER BY NEWID()
END
