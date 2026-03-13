namespace Shared.Infrastructure.Persistence.Mongo;

using System.Reflection;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using Shared.Domain.Entities;

public static class MongoSerializerConfig
{
    private static bool _registered;
    private static readonly object Lock = new();

    public static void Register()
    {
        if (_registered) return;

        lock (Lock)
        {
            if (_registered) return;

            BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));
            BsonSerializer.RegisterSerializer(new DateTimeSerializer(DateTimeKind.Utc, BsonType.DateTime));
            BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(BsonType.DateTime));
            BsonSerializer.RegisterSerializer(new NullableSerializer<DateTimeOffset>(new DateTimeOffsetSerializer(BsonType.DateTime)));

            RegisterClassMap<User>();
            RegisterClassMap<Account>();
            RegisterClassMap<Company>();
            RegisterClassMap<Subscription>();
            RegisterClassMap<UserAccount>();
            RegisterClassMap<TransactionIngestionModel>();
            RegisterClassMap<Amount>();
            RegisterClassMap<Movement>();

            _registered = true;
        }
    }

    private static void RegisterClassMap<T>()
    {
        if (BsonClassMap.IsClassMapRegistered(typeof(T))) return;

        BsonClassMap.RegisterClassMap<T>(cm =>
        {
            cm.AutoMap();
            cm.SetIgnoreExtraElements(true);

            var noArgsCtor = typeof(T).GetConstructor(
                BindingFlags.NonPublic | BindingFlags.Instance,
                null, Type.EmptyTypes, null);

            if (noArgsCtor != null)
                cm.MapConstructor(noArgsCtor);
        });
    }
}
