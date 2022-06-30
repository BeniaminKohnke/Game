namespace GameAPI
{
    public class GameObject : Position
    {
        public uint ObjectId = 0;
        public States State { get; set; }
        public ObjectsTypes ObjectType { get; set; }
        public TexturesTypes TextureType { get; set; }
    }
}