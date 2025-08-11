using Kingmaker.Blueprints;
using System.Security.Cryptography;
using System.Text;


namespace RandomReinforcementsPerEncounter
{
    static class GuidUtil
    {
        private const string NAMESPACE = "RRE.enchants"; 

        public static BlueprintGuid FromString(string id)
        {
            using (var md5 = MD5.Create())
            {
                var bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(NAMESPACE + ":" + id));
                var g = new System.Guid(bytes);
                return BlueprintGuid.Parse(g.ToString("N")); // 32 hex, sin guiones
            }
        }
    }
}
