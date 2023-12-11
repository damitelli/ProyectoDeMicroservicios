namespace Infrastructure.Services;

public class BsonSerializerRegisterer
{
    static BsonSerializerRegisterer()
    {
        BsonSerializer.RegisterSerializer(typeof(Guid), new GuidSerializer(BsonType.String));
        BsonSerializer.RegisterSerializer(typeof(DateTime), new DateTimeOffsetSerializer(BsonType.String));
    }

    public static void RegisterSerializer() { }
}