namespace GameAPI
{
    public class GameObject : Position
    {
        private static uint _lastId = 0;
        public readonly uint ObjectId = _lastId++;
        public States State { get; set; }
        public ObjectsTypes ObjectType { get; set; }
        public TexturesTypes TextureType { get; set; }
    }
}