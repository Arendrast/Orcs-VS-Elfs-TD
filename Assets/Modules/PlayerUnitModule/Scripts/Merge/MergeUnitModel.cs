namespace Modules.PlayerUnitModule.Scripts.Merge
{
    public class MergeUnitModel
    {
        public readonly int Id;
        public readonly MergeUnitComponent Component;

        public MergeUnitModel(int id, MergeUnitComponent component = null)
        {
            Id = id;
            Component = component;
        }
    }
}