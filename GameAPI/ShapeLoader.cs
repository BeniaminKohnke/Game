namespace GameAPI
{
    public class ShapeLoader
    {
        public Dictionary<Shapes, Dictionary<States, byte[][]>> Shapes = new();

        public void LoadShapes()
        {
            var mainDir = $@"{Directory.GetCurrentDirectory()}\Textures";
            foreach(var folder in Enum.GetValues(typeof(Shapes)))
            {
                var folderPath = $@"{mainDir}\{folder}";
                if(Directory.Exists(folderPath))
                {
                    Shapes[(Shapes)folder] = new();
                    foreach(var file in Enum.GetValues(typeof(States)))
                    {
                        var filePath = $@"{folderPath}\{file}.sm";
                        if(File.Exists(filePath))
                        {
                            Shapes[(Shapes)folder][(States)file] = 
                                File.ReadAllLines(filePath).Select(l => l.Split('\t').Select(p => byte.Parse(p)).ToArray()).ToArray();
                        }
                    }
                }
            }
        }
    }
}
