using Kingmaker.Blueprints;
using System;
using System.Security.Cryptography;
using System.Text;


namespace RandomReinforcementsPerEncounter.Config.Ids
{
    static class IdGenerators
    {
        private const string NS_ENCH = "RRE.enchants";
        private const string NS_FEAT = "RRE.features";       //Base guid for features
        private const string NS_WEAP = "RRE.weapon";

        /// <summary>
        /// Para Blueprints raíz (sin guiones). Igual que tenías.
        /// </summary>
        public static BlueprintGuid EnchantId(string id)
        {
            using (var md5 = MD5.Create())
            {
                var bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(NS_ENCH + ":" + id));
                var g = new Guid(bytes);
                return BlueprintGuid.Parse(g.ToString("N")); // 32 hex, sin guiones
            }
        }

        public static BlueprintGuid FeatureId(string id)
        {
            using (var md5 = MD5.Create())
            {
                var bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(NS_FEAT + ":" + id));
                var g = new Guid(bytes);
                return BlueprintGuid.Parse(g.ToString("N")); // sin guiones
            }
        }

        public static BlueprintGuid WeaponId(string id)
        {
            using (var md5 = MD5.Create())
            {
                var bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(NS_WEAP + ":" + id));
                var g = new Guid(bytes);
                return BlueprintGuid.Parse(g.ToString("N")); // sin guiones
            }
        }
    }
}
