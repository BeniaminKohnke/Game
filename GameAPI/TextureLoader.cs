namespace GameAPI
{
    public class TextureLoader
    {
        public Dictionary<TexturesTypes, Dictionary<States, byte[][]>> Textures = new();

        public void LoadTextures()
        {
            var mainDir = $@"{Directory.GetCurrentDirectory()}\Textures";
            foreach(var folder in Enum.GetValues(typeof(TexturesTypes)))
            {
                var folderPath = $@"{mainDir}\{folder}";
                if(Directory.Exists(folderPath))
                {
                    Textures[(TexturesTypes)folder] = new();
                    foreach(var file in Enum.GetValues(typeof(States)))
                    {
                        var filePath = $@"{folderPath}\{file}.sm";
                        if(File.Exists(filePath))
                        {
                            Textures[(TexturesTypes)folder][(States)file] = 
                                File.ReadAllLines(filePath).Select(l => l.Split('\t').Select(p => byte.Parse(p)).ToArray()).ToArray();
                        }
                    }
                }
            }
        }
    }
}
