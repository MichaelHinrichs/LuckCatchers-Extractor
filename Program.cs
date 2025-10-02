//Written for LuckCatchers games.
//LuckCatchers https://store.steampowered.com/app/454540
//LuckCatchers 2 https://store.steampowered.com/app/2700970
using System.IO;

namespace LuckCatchers_Extractor
{
    class Program
    {
        public static BinaryReader br;
        static void Main(string[] args)
        {
            br = new(File.OpenRead(args[0]));
            if (new string(br.ReadChars(8)) != "VOLUME0\0")
                throw new System.Exception("This is not a LuckCatchers res file.");

            System.Collections.Generic.List<Subfile> subfiles = new()
            {
                new()
            };
            for(int i = 0; i < subfiles[0].size - 1; i++)
                subfiles.Add(new());

            br.ReadInt32();//unknown
            long dataStart = br.BaseStream.Position;
            string path = Path.GetDirectoryName(args[0]) + "//";

            foreach (Subfile file in subfiles)
            {
                if (file.isFolder == 1)
                    Directory.CreateDirectory(path + file.path);
                else
                {
                    br.BaseStream.Position = file.start + dataStart;
                    BinaryWriter bw = new(File.Create(path + file.path));
                    bw.Write(br.ReadBytes(file.size));
                    bw.Close();
                }
            }
        }

        class Subfile
        {
            public int size = br.ReadInt32();
            public byte isFolder = br.ReadByte();
            public string name = new string(br.ReadChars(br.ReadInt32())).TrimEnd('\0');
            public string path = new string(br.ReadChars(br.ReadInt32())).TrimEnd('\0');
            public int start = br.ReadInt32();
        }
    }
}
