# Delivery Tracker (Event Sourcing)
An example delivery tracker domain making use of `IEvangelist.Azure.CosmosEventSourcing`.

## Run Locally

In order to run the project locally run the `DeliveryTracker.API` project. You will also need a connection string for a Cosmos DB database. This application demo uses the serverless option. The reason for this is cost you pay around `£0.230` per 1 million RUs so for demos this works great. Read more [here](https://azure.microsoft.com/en-us/pricing/details/cosmos-db/#purchase-options).

### Set `RepositoryOptions:CosmosConnectionString`

You need to set the connection string for a serverless cosmos account. There are two ways to do this. The first (advised method) is to use `dotnet user-secrets`. This needs to be set in the directory of the `DeliveryTracker.API` project. See the command below:

```shell
dotnet user-secrets set RepositoryOptions:CosmosConnectionString "<your-conn-string>"
```

The second option is to set this up in the `appsettings.Development.json` file. See adding this to the `DeliveryTracker.API/appsettings.Development.json` below:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning",
      "System.Net.Http": "Warning",
      "Microsoft.Azure.CosmosEventSourcing": "Debug",
      "Microsoft.Azure.CosmosRepository": "Debug"
    }
  },
  "RepositoryOptions": {
    "CosmosConnectionString": "<your-conn-string>"
  }
}
```

## Overview

This example of event sourcing uses the delivery tracking domain. The easiest way to think of this is a delivery driver get's a `Schedule` or maybe known as a route. This `Schedule` has a set of `Stop`'s. These are represented in the `DeliveryTracker.Domain` project. See the `ISchedule` for example shown below.

```csharp
public interface ISchedule : IAggregateRoot
{
    ScheduleId Id { get; }
    
    Driver Driver { get; }
    
    ScheduleStatus Status { get; }
    
    IReadOnlyList<IStop> Stops { get; }
    
    IReadOnlyList<IStop> CompletedStops { get; }
    
    IReadOnlyList<IStop> FailedStops { get; }
    
    IReadOnlyList<IStop> AbandonedStops { get; }
    
    IReadOnlyList<IStop> OutstandingStops { get; }

    double Progress { get; }

    void AddStop(Location location);

    void Start();
    
    void CompleteStop(Guid stopId);
}
```

Any changes to this `AggreateRoot` will result in an event being raised. This event is then stored in Cosmos DB. In the case of this app in a container called `delivery-schedule-events`. See the belows setup of the `IEvangelist.Azure.CosmosEventSourcing` package which aids this storage of events in Cosmos DB.

```csharp
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddCosmosEventSourcing(builder =>
        {
            builder.AddCosmosRepository(options =>
            {
                options.DatabaseId = "delivery-schedule-sample";

                options
                    .ContainerBuilder
                    .ConfigureEventSourceStore<ScheduleEventSource>(
                        "delivery-schedule-events",
                        c => 
                            c.WithServerlessThroughput())
                    .ConfigureProjectionStore<DriverSchedule>(
                        "projections",
                        containerOptionsBuilder: c => 
                            c.WithServerlessThroughput());
            });

            builder.AddEventSourceProjectionBuilder<ScheduleEventSource, DriverScheduleProjectionBuilder>();
            builder.AddAllPersistedEventsTypes(typeof(ISchedule).Assembly);
        });

        services.AddCosmosRepositoryChangeFeedHostedService();

        services.AddSingleton<IScheduleRepository, ScheduleRepository>();
        services.AddSingleton<IDriverScheduleReadService, DriverScheduleReadService>();

        return services;
    }
}
```

See some example events that are stored and make up part of a schedule

```json
[
    {
        "driverCode": "BILM",
        "stopId": null,
        "eventPayload": {
            "id": "BILM-060322-1271",
            "driverCode": "BILM",
            "driverFirstName": "Billy",
            "driverSecondName": "Mumby",
            "occuredUtc": "2022-03-06T15:34:07.383594Z",
            "eventName": "ScheduleCreated"
        },
        "partitionKey": "BILM-060322-1271",
        "eventName": "ScheduleCreated",
        "_etag": "\"f500baf8-0000-0700-0000-6224d46f0000\"",
        "timeToLive": null,
        "createdTimeUtc": "2022-03-06T15:34:07.717545Z",
        "id": "bfdcc0dd-d971-4804-89d0-3a088f42327e",
        "type": "ScheduleEventSource",
        "_rid": "StMaAPWvM4YZAAAAAAAAAA==",
        "_self": "dbs/StMaAA==/colls/StMaAPWvM4Y=/docs/StMaAPWvM4YZAAAAAAAAAA==/",
        "_attachments": "attachments/",
        "_ts": 1646580847
    },
    {
        "driverCode": "BILM",
        "stopId": "4c9d2ec0-1890-430f-96bb-76e9f3ac9cfd",
        "eventPayload": {
            "stopId": "4c9d2ec0-1890-430f-96bb-76e9f3ac9cfd",
            "houseNumber": 1,
            "addressLine": "Test Street 1",
            "postCode": "TS10 HUT",
            "occuredUtc": "2022-03-06T15:34:46.411462Z",
            "eventName": "StopScheduled"
        },
        "partitionKey": "BILM-060322-1271",
        "eventName": "StopScheduled",
        "_etag": "\"f500d1f8-0000-0700-0000-6224d4960000\"",
        "timeToLive": null,
        "createdTimeUtc": "2022-03-06T15:34:46.411951Z",
        "id": "dcf3f53f-9280-4056-913a-8e2732f8056d",
        "type": "ScheduleEventSource",
        "_rid": "StMaAPWvM4YaAAAAAAAAAA==",
        "_self": "dbs/StMaAA==/colls/StMaAPWvM4Y=/docs/StMaAPWvM4YaAAAAAAAAAA==/",
        "_attachments": "attachments/",
        "_ts": 1646580886
    },
    {
        "driverCode": "BILM",
        "stopId": "717efe12-0941-491c-97b0-6d0c5d1700ad",
        "eventPayload": {
            "stopId": "717efe12-0941-491c-97b0-6d0c5d1700ad",
            "houseNumber": 2,
            "addressLine": "Test Street 1",
            "postCode": "TS10 HUT",
            "occuredUtc": "2022-03-06T15:34:49.602421Z",
            "eventName": "StopScheduled"
        },
        "partitionKey": "BILM-060322-1271",
        "eventName": "StopScheduled",
        "_etag": "\"f500d6f8-0000-0700-0000-6224d4990000\"",
        "timeToLive": null,
        "createdTimeUtc": "2022-03-06T15:34:49.602467Z",
        "id": "8562a84e-050d-494e-b748-1ee8f80547fc",
        "type": "ScheduleEventSource",
        "_rid": "StMaAPWvM4YbAAAAAAAAAA==",
        "_self": "dbs/StMaAA==/colls/StMaAPWvM4Y=/docs/StMaAPWvM4YbAAAAAAAAAA==/",
        "_attachments": "attachments/",
        "_ts": 1646580889
    },
    {
        "driverCode": "BILM",
        "stopId": "4de1f2ae-1172-47c1-8ad3-99f31d58a66d",
        "eventPayload": {
            "stopId": "4de1f2ae-1172-47c1-8ad3-99f31d58a66d",
            "houseNumber": 3,
            "addressLine": "Test Street 1",
            "postCode": "TS10 HUT",
            "occuredUtc": "2022-03-06T15:34:52.45839Z",
            "eventName": "StopScheduled"
        },
        "partitionKey": "BILM-060322-1271",
        "eventName": "StopScheduled",
        "_etag": "\"f500dbf8-0000-0700-0000-6224d49c0000\"",
        "timeToLive": null,
        "createdTimeUtc": "2022-03-06T15:34:52.458405Z",
        "id": "734b97b3-64ef-4a13-bf2c-c29c9d1d2b08",
        "type": "ScheduleEventSource",
        "_rid": "StMaAPWvM4YcAAAAAAAAAA==",
        "_self": "dbs/StMaAA==/colls/StMaAPWvM4Y=/docs/StMaAPWvM4YcAAAAAAAAAA==/",
        "_attachments": "attachments/",
        "_ts": 1646580892
    },
    {
        "driverCode": "BILM",
        "stopId": null,
        "eventPayload": {
            "scheduleId": "BILM-060322-1271",
            "occuredUtc": "2022-03-06T15:36:30.001698Z",
            "eventName": "ScheduleStarted"
        },
        "partitionKey": "BILM-060322-1271",
        "eventName": "ScheduleStarted",
        "_etag": "\"f5004af9-0000-0700-0000-6224d4fe0000\"",
        "timeToLive": null,
        "createdTimeUtc": "2022-03-06T15:36:30.004355Z",
        "id": "d59e8aac-2bb7-446d-9701-e5e8722db0bd",
        "type": "ScheduleEventSource",
        "_rid": "StMaAPWvM4YdAAAAAAAAAA==",
        "_self": "dbs/StMaAA==/colls/StMaAPWvM4Y=/docs/StMaAPWvM4YdAAAAAAAAAA==/",
        "_attachments": "attachments/",
        "_ts": 1646580990
    },
    {
        "driverCode": "BILM",
        "stopId": "4c9d2ec0-1890-430f-96bb-76e9f3ac9cfd",
        "eventPayload": {
            "stopId": "4c9d2ec0-1890-430f-96bb-76e9f3ac9cfd",
            "occuredUtc": "2022-03-06T15:37:22.399866Z",
            "eventName": "StopCompleted"
        },
        "partitionKey": "BILM-060322-1271",
        "eventName": "StopCompleted",
        "_etag": "\"f5007cf9-0000-0700-0000-6224d5320000\"",
        "timeToLive": null,
        "createdTimeUtc": "2022-03-06T15:37:22.403797Z",
        "id": "41d33030-98cf-4092-936d-583df8784d33",
        "type": "ScheduleEventSource",
        "_rid": "StMaAPWvM4YeAAAAAAAAAA==",
        "_self": "dbs/StMaAA==/colls/StMaAPWvM4Y=/docs/StMaAPWvM4YeAAAAAAAAAA==/",
        "_attachments": "attachments/",
        "_ts": 1646581042
    },
    {
        "driverCode": "BILM",
        "stopId": "717efe12-0941-491c-97b0-6d0c5d1700ad",
        "eventPayload": {
            "stopId": "717efe12-0941-491c-97b0-6d0c5d1700ad",
            "occuredUtc": "2022-03-06T15:37:50.979952Z",
            "eventName": "StopCompleted"
        },
        "partitionKey": "BILM-060322-1271",
        "eventName": "StopCompleted",
        "_etag": "\"f5009ff9-0000-0700-0000-6224d54f0000\"",
        "timeToLive": null,
        "createdTimeUtc": "2022-03-06T15:37:50.979968Z",
        "id": "a0c85f3f-ecda-4e87-b66f-e6ba0655882f",
        "type": "ScheduleEventSource",
        "_rid": "StMaAPWvM4YfAAAAAAAAAA==",
        "_self": "dbs/StMaAA==/colls/StMaAPWvM4Y=/docs/StMaAPWvM4YfAAAAAAAAAA==/",
        "_attachments": "attachments/",
        "_ts": 1646581071
    }
]
```
