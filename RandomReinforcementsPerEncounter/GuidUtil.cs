using Kingmaker.Blueprints;
using System;
using System.Security.Cryptography;
using System.Text;


namespace RandomReinforcementsPerEncounter
{
    static class GuidUtil
    {
        private const string NS_ROOT = "RRE.enchants";
        private const string NS_FEAT = "RRE.features";       //Base guid for features
        private const string NS_SAVE = "RRE.enchants.save"; //Base guid for ContextActionSavingThrow
        private const string NS_COND = "RRE.enchants.cond"; //Base guid for ContextActionConditionalSaved
        private const string NS_BUFF = "RRE.enchants.buff"; //Base guid for ContextActionApplyBuff

        /// <summary>
        /// Para Blueprints raíz (sin guiones). Igual que tenías.
        /// </summary>
        public static BlueprintGuid FromString(string id)
        {
            using (var md5 = MD5.Create())
            {
                var bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(NS_ROOT + ":" + id));
                var g = new Guid(bytes);
                return BlueprintGuid.Parse(g.ToString("N")); // 32 hex, sin guiones
            }
        }
        public static BlueprintGuid FeatureGuid(string id)
        {
            using (var md5 = MD5.Create())
            {
                var bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(NS_FEAT + ":" + id));
                var g = new Guid(bytes);
                return BlueprintGuid.Parse(g.ToString("N")); // sin guiones
            }
        }

        /// <summary>
        /// Para ContextActionSavingThrow u otras ST internas (con guiones).
        /// </summary>
        public static string SaveGuid(string id)
        {
            return MakeDashed(NS_SAVE, id);
        }

        /// <summary>
        /// Para ContextActionConditionalSaved y lógica/ActionList internas (con guiones).
        /// </summary>
        public static string CondGuid(string id)
        {
            return MakeDashed(NS_COND, id);
        }

        /// <summary>
        /// Para ContextActionApplyBuff, el Buff clonado y sus componentes (con guiones).
        /// </summary>
        public static string BuffGuid(string id)
        {
            return MakeDashed(NS_BUFF, id);
        }

        // --- Helper común ---
        private static string MakeDashed(string ns, string id)
        {
            using (var md5 = MD5.Create())
            {
                var bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(ns + ":" + id));
                var g = new Guid(bytes);
                return g.ToString("D"); // con guiones
            }
        }
    }
}
