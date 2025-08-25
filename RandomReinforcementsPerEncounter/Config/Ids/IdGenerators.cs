using Kingmaker.Blueprints;
using System;
using System.Security.Cryptography;
using System.Text;


namespace RandomReinforcementsPerEncounter.Config.Ids
{
    /// <summary>
    /// Deterministic GUID factory for the mod's content (enchants, features, weapons).
    /// </summary>
    /// <remarks>
    /// <para>
    /// This class generates <see cref="BlueprintGuid"/> values by hashing a stable textual ID with a
    /// category-specific namespace (salt) and converting the 16-byte MD5 digest directly into a .NET <see cref="Guid"/>.
    /// The resulting GUID is:
    /// </para>
    /// <list type="bullet">
    ///   <item><description><b>Deterministic</b>: same input → same GUID across runs, machines and builds.</description></item>
    ///   <item><description><b>Isolated per category</b>: different namespaces (enchants/features/weapons) prevent cross-collisions.</description></item>
    ///   <item><description><b>Save-safe</b>: as long as inputs and namespaces remain unchanged, GUIDs remain stable across updates.</description></item>
    /// </list>
    /// <para>
    /// ⚠️ <b>Do not change</b> the namespace constants or the algorithm after release: doing so will change all generated GUIDs and
    /// break references in existing saves/blueprints.
    /// </para>
    /// <para>
    /// About MD5: we use it for <i>deterministic ID derivation</i>, not for security. While MD5 is not collision-resistant
    /// cryptographically, the practical collision risk here is negligible for well-formed, unique textual IDs combined with a namespace.
    /// </para>
    /// <para>
    /// <b>Input hygiene:</b> IDs are treated as <i>case-sensitive</i> UTF-8 strings and used exactly as provided.
    /// Keep them normalized (e.g., all-lowercase, hyphen/underscore separated, no trailing spaces) to avoid accidental duplicates.
    /// </para>
    /// </remarks>
    static class IdGenerators
    {
        /// <summary>
        /// Category namespaces (salts) used to isolate ID spaces.
        /// Changing any of these after release will rewrite all derived GUIDs — don't do it.
        /// </summary>
        private const string NS_ENCH = "RRE.enchants";
        private const string NS_FEAT = "RRE.features";       
        private const string NS_WEAP = "RRE.weapon";

        /// <summary>
        /// Generates a deterministic <see cref="BlueprintGuid"/> for an enchant definition.
        /// </summary>
        /// <param name="id">Stable textual ID for the enchant (e.g., "flaming_t2", "onhit_bleed").</param>
        /// <returns>Deterministic <see cref="BlueprintGuid"/> derived from <c>RRE.enchants:id</c>.</returns>
        /// <example>
        /// <code>
        /// var gid = IdGenerators.EnchantId("flaming_t1");
        /// // gid will be the same across runs and machines
        /// </code>
        /// </example>
        /// <remarks>
        /// Implementation notes:
        /// <list type="number">
        ///   <item><description>Concatenate the namespace and the ID using a colon (e.g., "RRE.enchants:flaming_t1").</description></item>
        ///   <item><description>Compute MD5 over the UTF-8 bytes of that string (16-byte digest).</description></item>
        ///   <item><description>Create a <see cref="Guid"/> from the 16 bytes and format with "N" (32 hex chars, no hyphens).</description></item>
        ///   <item><description>Parse into <see cref="BlueprintGuid"/> using <c>BlueprintGuid.Parse</c>.</description></item>
        /// </list>
        /// </remarks>
        public static BlueprintGuid EnchantId(string id)
        {
            using (var md5 = MD5.Create())
            {
                var bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(NS_ENCH + ":" + id));
                var g = new Guid(bytes);
                return BlueprintGuid.Parse(g.ToString("N")); 
            }
        }

        /// <summary>
        /// Generates a deterministic <see cref="BlueprintGuid"/> for a feature definition.
        /// </summary>
        /// <param name="id">Stable textual ID for the feature (e.g., "rre_feat_dual_wielder").</param>
        /// <returns>Deterministic <see cref="BlueprintGuid"/> derived from <c>RRE.features:id</c>.</returns>
        /// <example>
        /// <code>
        /// var featGuid = IdGenerators.FeatureId("rre_feat_bonus_loot");
        /// </code>
        /// </example>
        public static BlueprintGuid FeatureId(string id)
        {
            using (var md5 = MD5.Create())
            {
                var bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(NS_FEAT + ":" + id));
                var g = new Guid(bytes);
                return BlueprintGuid.Parse(g.ToString("N")); 
            }
        }

        /// <summary>
        /// Generates a deterministic <see cref="BlueprintGuid"/> for a weapon blueprint or entry.
        /// </summary>
        /// <param name="id">Stable textual ID for the weapon (e.g., "longsword_frost_t3").</param>
        /// <returns>Deterministic <see cref="BlueprintGuid"/> derived from <c>RRE.weapon:id</c>.</returns>
        /// <example>
        /// <code>
        /// var weapGuid = IdGenerators.WeaponId("scimitar_flaming_t2");
        /// </code>
        /// </example>
        public static BlueprintGuid WeaponId(string id)
        {
            using (var md5 = MD5.Create())
            {
                var bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(NS_WEAP + ":" + id));
                var g = new Guid(bytes);
                return BlueprintGuid.Parse(g.ToString("N")); 
            }
        }
    }
}
