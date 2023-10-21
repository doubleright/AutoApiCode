namespace AutoApiCode.Config
{
    public class MainConfig : ConfigBase
    {
        public string CodePath { get; set; }

        public GenCodeType CodeType { get; set; }

        public int Index { get; set; }
    }

    public enum GenCodeType
    {
        Client,
        Server
    }
}
