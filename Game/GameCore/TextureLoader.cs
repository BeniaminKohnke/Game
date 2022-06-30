﻿using GameAPI;
using SFML.Graphics;

namespace Game.GameCore
{
    public class TextureLoader
    {
        public Dictionary<TexturesTypes, Dictionary<States, Texture>> Textures { get; private set; } = new();

        public void LoadTextures()
        {
            var rootDirectory = $@"{Directory.GetCurrentDirectory()}\Textures";

            foreach (var folder in Directory.GetDirectories(rootDirectory))
            {
                var name = folder.Replace($@"{rootDirectory}\", string.Empty);
                var folderId = (TexturesTypes)int.Parse(name);
                Textures.Add(folderId, new Dictionary<States, Texture>());

                foreach (var file in Directory.GetFiles(folder))
                {
                    name = file.Replace($@"{folder}\", string.Empty).Replace(".png", string.Empty);
                    var fileId = (States)int.Parse(name);
                    Textures[folderId][fileId] = new Texture(file);
                }
            }
        }
    }
}