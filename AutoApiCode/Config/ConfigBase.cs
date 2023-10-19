namespace AutoApiCode.Config
{
    public abstract class ConfigBase
    {
        public void Save()
        {
            ConfigHelper.SaveConfg(this);
        }
    }
}
