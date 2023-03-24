using System.IO;

namespace ComputerGraphics_TomogramVisualizer
{
    public static class Bin
    {
        //public static bool is_loaded = false;
        public static bool is_loaded { get; private set; }
        public static int X { get; private set; }
        public static int Y { get; private set; }
        public static int Z { get; private set; }
        public static short[]? array;

        public static void readBin(string path)
        {
            if (File.Exists(path))
            {
                BinaryReader reader = new BinaryReader(File.Open(path, FileMode.Open));
                X = reader.ReadInt32();
                Y = reader.ReadInt32();
                Z = reader.ReadInt32();
                int arraySize = X * Y * Z;
                array = new short[arraySize];
                for (int i = 0; i < arraySize; i++)
                {
                    array[i] = reader.ReadInt16();
                }
                is_loaded = true;
                reader.Close();
            }
        }
    }
}
