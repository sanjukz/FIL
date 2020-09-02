ALTER TABLE CurrencyTypes
ALTER COLUMN Name NVARCHAR(50)
GO

IF NOT EXISTS (SELECT * FROM CurrencyTypes WHERE Code = 'AUD') 
BEGIN 
INSERT INTO CurrencyTypes (Code,Name,CountryId,ExchangeRate,IsEnabled,CreatedUtc,CreatedBy) VALUES ('AUD', 'Australian Dollar', 13,1.3, 1, GETUTCDATE(), 'C043DDEE-D0B1-48D8-9C3F-309A77F44781')
END
GO

IF NOT EXISTS (SELECT * FROM CurrencyTypes WHERE Code = 'BBD') 
BEGIN 
INSERT INTO CurrencyTypes (Code,Name,CountryId,ExchangeRate,IsEnabled,CreatedUtc,CreatedBy) VALUES ('BBD', 'Barbadian Dollar', 19,2.78, 1, GETUTCDATE(), 'C043DDEE-D0B1-48D8-9C3F-309A77F44781')
END
GO

IF NOT EXISTS (SELECT * FROM CurrencyTypes WHERE Code = 'CAD') 
BEGIN 
INSERT INTO CurrencyTypes (Code,Name,CountryId,ExchangeRate,IsEnabled,CreatedUtc,CreatedBy) VALUES ('CAD', 'Canadian Dollar', 38,1.31, 1, GETUTCDATE(), 'C043DDEE-D0B1-48D8-9C3F-309A77F44781')
END
GO

IF NOT EXISTS (SELECT * FROM CurrencyTypes WHERE Code = 'XCD') 
BEGIN 
INSERT INTO CurrencyTypes (Code,Name,CountryId,ExchangeRate,IsEnabled,CreatedUtc,CreatedBy) VALUES ('XCD', 'East Caribbean Dollar', 60,2.69, 1, GETUTCDATE(), 'C043DDEE-D0B1-48D8-9C3F-309A77F44781')
END
GO

IF NOT EXISTS (SELECT * FROM CurrencyTypes WHERE Code = 'GTQ') 
BEGIN 
INSERT INTO CurrencyTypes (Code,Name,CountryId,ExchangeRate,IsEnabled,CreatedUtc,CreatedBy) VALUES ('GTQ', 'Guatemalan Quetzal', 90,7.33, 1, GETUTCDATE(), 'C043DDEE-D0B1-48D8-9C3F-309A77F44781')
END
GO

IF NOT EXISTS (SELECT * FROM CurrencyTypes WHERE Code = 'GYD') 
BEGIN 
INSERT INTO CurrencyTypes (Code,Name,CountryId,ExchangeRate,IsEnabled,CreatedUtc,CreatedBy) VALUES ('GYD', 'Guyanese Dollar', 94,207.77, 1, GETUTCDATE(), 'C043DDEE-D0B1-48D8-9C3F-309A77F44781')
END
GO

IF NOT EXISTS (SELECT * FROM CurrencyTypes WHERE Code = 'INR') 
BEGIN 
INSERT INTO CurrencyTypes (Code,Name,CountryId,ExchangeRate,IsEnabled,CreatedUtc,CreatedBy) VALUES ('INR', 'Indian Rupee', 101,65.04, 1, GETUTCDATE(), 'C043DDEE-D0B1-48D8-9C3F-309A77F44781')
END
GO

IF NOT EXISTS (SELECT * FROM CurrencyTypes WHERE Code = 'JMD') 
BEGIN 
INSERT INTO CurrencyTypes (Code,Name,CountryId,ExchangeRate,IsEnabled,CreatedUtc,CreatedBy) VALUES ('JMD', 'Jamaican Dollar', 108,127.21, 1, GETUTCDATE(), 'C043DDEE-D0B1-48D8-9C3F-309A77F44781')
END
GO

IF NOT EXISTS (SELECT * FROM CurrencyTypes WHERE Code = 'PKR') 
BEGIN 
INSERT INTO CurrencyTypes (Code,Name,CountryId,ExchangeRate,IsEnabled,CreatedUtc,CreatedBy) VALUES ('PKR', 'Pakistani Rupee', 166,154.03, 1, GETUTCDATE(), 'C043DDEE-D0B1-48D8-9C3F-309A77F44781')
END
GO

IF NOT EXISTS (SELECT * FROM CurrencyTypes WHERE Code = 'XCD') 
BEGIN 
INSERT INTO CurrencyTypes (Code,Name,CountryId,ExchangeRate,IsEnabled,CreatedUtc,CreatedBy) VALUES ('XCD', 'East Caribbean Dollar', 184,2.69, 1, GETUTCDATE(), 'C043DDEE-D0B1-48D8-9C3F-309A77F44781')
END
GO

IF NOT EXISTS (SELECT * FROM CurrencyTypes WHERE Code = 'XCD') 
BEGIN 
INSERT INTO CurrencyTypes (Code,Name,CountryId,ExchangeRate,IsEnabled,CreatedUtc,CreatedBy) VALUES ('XCD', 'East Caribbean Dollar', 185,2.69, 1, GETUTCDATE(), 'C043DDEE-D0B1-48D8-9C3F-309A77F44781')
END
GO

IF NOT EXISTS (SELECT * FROM CurrencyTypes WHERE Code = 'SCR') 
BEGIN 
INSERT INTO CurrencyTypes (Code,Name,CountryId,ExchangeRate,IsEnabled,CreatedUtc,CreatedBy) VALUES ('SCR', 'Seychellois Rupee', 194,16.96, 1, GETUTCDATE(), 'C043DDEE-D0B1-48D8-9C3F-309A77F44781')
END
GO

IF NOT EXISTS (SELECT * FROM CurrencyTypes WHERE Code = 'USD') 
BEGIN 
INSERT INTO CurrencyTypes (Code,Name,CountryId,ExchangeRate,IsEnabled,CreatedUtc,CreatedBy) VALUES ('USD', 'US Dollar', 231,0, 1, GETUTCDATE(), 'C043DDEE-D0B1-48D8-9C3F-309A77F44781')
END
GO

IF NOT EXISTS (SELECT * FROM CurrencyTypes WHERE Code = 'UYU') 
BEGIN 
INSERT INTO CurrencyTypes (Code,Name,CountryId,ExchangeRate,IsEnabled,CreatedUtc,CreatedBy) VALUES ('UYU', 'Uruguayan Peso', 233,28.4, 1, GETUTCDATE(), 'C043DDEE-D0B1-48D8-9C3F-309A77F44781')
END
GO

