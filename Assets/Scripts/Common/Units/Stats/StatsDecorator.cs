using Common.Units.Stats.Interfaces;

namespace Common.Units.Stats
{
    public abstract class StatsDecorator : IUnitStatsProvider
    {
        protected readonly UnitStats wrappedEntity;

        protected StatsDecorator(UnitStats wrappedEntity)
        {
            this.wrappedEntity = wrappedEntity;
        }

        public UnitStats GetStats() => GetStatsInternal();

        protected abstract UnitStats GetStatsInternal();
    }
}