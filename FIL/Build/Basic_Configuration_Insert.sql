USE [KzConfiguration]
GO

INSERT INTO [dbo].[ConfigurationSets]
           ([Name]
           ,[CanMigrate]
           ,[ParentConfigurationSetId]
           ,[IsEnabled]
           ,[CreatedUtc]
           ,[UpdatedUtc]
           ,[CreatedBy]
           ,[UpdatedBy])
     VALUES
           ('DEFAULT'
           ,1
           ,NULL
           ,1
           ,GETUTCDATE()
           ,NULL
           ,'00000000-0000-0000-0000-000000000000' --system user
           ,NULL);

INSERT INTO [dbo].[ConfigurationKeys]
           ([Name]
           ,[Description]
           ,[CreatedUtc]
           ,[UpdatedUtc]
           ,[CreatedBy]
           ,[UpdatedBy])
     VALUES
           ('Api.Database.ReadOnlyConnectionString'
           ,'Read only API connection string.'
           ,GETUTCDATE()
           ,NULL
           ,'00000000-0000-0000-0000-000000000000'
           ,NULL);

INSERT INTO [dbo].[Configurations]
           ([ConfigurationKeyId]
           ,[ConfigurationSetId]
           ,[Value]
           ,[IsEnabled]
           ,[CreatedUtc]
           ,[UpdatedUtc]
           ,[CreatedBy]
           ,[UpdatedBy])
     VALUES
           (1
           ,1
           ,'Server=localhost;Database=KzOLTP;Integrated Security=True;Trusted_Connection=True;Min Pool Size=10;Max Pool Size=300;ApplicationIntent=ReadOnly;'
           ,1
           ,GETUTCDATE()
           ,NULL
           ,'00000000-0000-0000-0000-000000000000'
           ,NULL);

INSERT INTO [dbo].[ConfigurationKeys]
           ([Name]
           ,[Description]
           ,[CreatedUtc]
           ,[UpdatedUtc]
           ,[CreatedBy]
           ,[UpdatedBy])
     VALUES
           ('Api.Database.TransactionalConnectionString'
           ,'Write connection string for the API'
           ,GETUTCDATE()
           ,NULL
           ,'00000000-0000-0000-0000-000000000000'
           ,NULL);

INSERT INTO [dbo].[Configurations]
           ([ConfigurationKeyId]
           ,[ConfigurationSetId]
           ,[Value]
           ,[IsEnabled]
           ,[CreatedUtc]
           ,[UpdatedUtc]
           ,[CreatedBy]
           ,[UpdatedBy])
     VALUES
           (2
           ,1
           ,'Server=localhost;Database=KzOLTP;Integrated Security=True;Trusted_Connection=True;Min Pool Size=10;Max Pool Size=300;'
           ,1
           ,GETUTCDATE()
           ,NULL
           ,'00000000-0000-0000-0000-000000000000'
           ,NULL);

INSERT INTO [dbo].[ConfigurationKeys]
           ([Name]
           ,[Description]
           ,[CreatedUtc]
           ,[UpdatedUtc]
           ,[CreatedBy]
           ,[UpdatedBy])
     VALUES
           ('Foundation.Http.ApiEndpoint'
           ,'The API end point for foundation to use.'
           ,GETUTCDATE()
           ,NULL
           ,'00000000-0000-0000-0000-000000000000'
           ,NULL);

INSERT INTO [dbo].[Configurations]
           ([ConfigurationKeyId]
           ,[ConfigurationSetId]
           ,[Value]
           ,[IsEnabled]
           ,[CreatedUtc]
           ,[UpdatedUtc]
           ,[CreatedBy]
           ,[UpdatedBy])
     VALUES
           (3
           ,1
           ,'http://api.kz.local'
           ,1
           ,GETUTCDATE()
           ,NULL
           ,'00000000-0000-0000-0000-000000000000'
           ,NULL);

INSERT INTO [dbo].[ConfigurationKeys]
           ([Name]
           ,[Description]
           ,[CreatedUtc]
           ,[UpdatedUtc]
           ,[CreatedBy]
           ,[UpdatedBy])
     VALUES
           ('Redis.Enabled'
           ,'Whether or not Redis is turned on for caching.'
           ,GETUTCDATE()
           ,NULL
           ,'00000000-0000-0000-0000-000000000000'
           ,NULL);

INSERT INTO [dbo].[Configurations]
           ([ConfigurationKeyId]
           ,[ConfigurationSetId]
           ,[Value]
           ,[IsEnabled]
           ,[CreatedUtc]
           ,[UpdatedUtc]
           ,[CreatedBy]
           ,[UpdatedBy])
     VALUES
           (4
           ,1
           ,'false'
           ,1
           ,GETUTCDATE()
           ,NULL
           ,'00000000-0000-0000-0000-000000000000'
           ,NULL);

INSERT INTO [dbo].[ConfigurationKeys]
           ([Name]
           ,[Description]
           ,[CreatedUtc]
           ,[UpdatedUtc]
           ,[CreatedBy]
           ,[UpdatedBy])
     VALUES
           ('Api.HealthCheckFailFileName'
           ,'If this file exists, the api will fail healthcheck.'
           ,GETUTCDATE()
           ,NULL
           ,'00000000-0000-0000-0000-000000000000'
           ,NULL);

INSERT INTO [dbo].[Configurations]
           ([ConfigurationKeyId]
           ,[ConfigurationSetId]
           ,[Value]
           ,[IsEnabled]
           ,[CreatedUtc]
           ,[UpdatedUtc]
           ,[CreatedBy]
           ,[UpdatedBy])
     VALUES
           (5
           ,1
           ,'failhealthcheck.txt'
           ,1
           ,GETUTCDATE()
           ,NULL
           ,'00000000-0000-0000-0000-000000000000'
           ,NULL);

INSERT INTO [dbo].[ConfigurationSets]
           ([Name]
           ,[CanMigrate]
           ,[ParentConfigurationSetId]
           ,[IsEnabled]
           ,[CreatedUtc]
           ,[UpdatedUtc]
           ,[CreatedBy]
           ,[UpdatedBy])
     VALUES
           ('DEVELOPMENT'
           ,1
           ,NULL
           ,1
           ,GETUTCDATE()
           ,NULL
           ,'00000000-0000-0000-0000-000000000000' --system user
           ,NULL);


INSERT INTO [dbo].[Configurations]
           ([ConfigurationKeyId]
           ,[ConfigurationSetId]
           ,[Value]
           ,[IsEnabled]
           ,[CreatedUtc]
           ,[UpdatedUtc]
           ,[CreatedBy]
           ,[UpdatedBy])
     VALUES
           (1
           ,2
           ,'Server=kz-development.cduukfkxzylb.us-west-2.rds.amazonaws.com;Database=KzOLTP;User Id=root;Password=58!?SMrwgkSpcuY=;Min Pool Size=10;Max Pool Size=300;ApplicationIntent=ReadOnly;'
           ,1
           ,GETUTCDATE()
           ,NULL
           ,'00000000-0000-0000-0000-000000000000'
           ,NULL);


INSERT INTO [dbo].[Configurations]
           ([ConfigurationKeyId]
           ,[ConfigurationSetId]
           ,[Value]
           ,[IsEnabled]
           ,[CreatedUtc]
           ,[UpdatedUtc]
           ,[CreatedBy]
           ,[UpdatedBy])
     VALUES
           (2
           ,2
           ,'Server=kz-development.cduukfkxzylb.us-west-2.rds.amazonaws.com;Database=KzOLTP;User Id=root;Password=58!?SMrwgkSpcuY=;Min Pool Size=10;Max Pool Size=300;'
           ,1
           ,GETUTCDATE()
           ,NULL
           ,'00000000-0000-0000-0000-000000000000'
           ,NULL);

INSERT INTO [dbo].[Configurations]
           ([ConfigurationKeyId]
           ,[ConfigurationSetId]
           ,[Value]
           ,[IsEnabled]
           ,[CreatedUtc]
           ,[UpdatedUtc]
           ,[CreatedBy]
           ,[UpdatedBy])
     VALUES
           (3
           ,2
           ,'http://kzapi-dev.us-west-2.elasticbeanstalk.com'
           ,1
           ,GETUTCDATE()
           ,NULL
           ,'00000000-0000-0000-0000-000000000000'
           ,NULL);
GO




