using Kingmaker.Blueprints;

namespace RandomReinforcementsPerEncounter.Config.Ids
{
    // Guards against GUID collisions; builds a short log message if the GUID is taken.
    internal static class GuidCollisionGuard
    {
        public static bool Exists(BlueprintGuid guid, out SimpleBlueprint existing)
        {
            existing = ResourcesLibrary.TryGetBlueprint(guid);
            return existing != null;
        }

        public static bool TryDetectCollision(BlueprintGuid guid, string category, string id, out string message)
        {
            message = null;
            if (Exists(guid, out var existing))
            {
                var name = existing != null ? existing.name : "?";
                message = $"[RRE] GUID collision: {guid} already used by '{name}' (category={category}, id={id}).";
                return true;
            }
            return false;
        }
    }
}
