namespace Modules.PlayerUnitModule.Scripts.Merge
{
    public class MergeUnitModel
    {
        public readonly int Id;
        public readonly MergeUnitComponent Component;

        public MergeUnitModel(MergeUnitComponent component, int id)
        {
            Id = id;
            Component = component;
        }
    }
}