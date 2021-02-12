using System.Collections.Generic;

namespace Mono.Util
{
    public static class UuidService
    {
        public static string GetUuid()
        {
            string s = "";

            for (int i = 0; i < 5; i++)
            {
                s += UnityEngine.Random.Range(1, 9);
            }

            return s;
        }

        public static string GetUuid(List<string> existingUuids)
        {
            string uuid = GetUuid();

            while (existingUuids.Contains(uuid))
            {
                uuid = GetUuid();
            }

            return uuid;
        }

        public static int GetIntUuid()
        {
            return int.Parse(UuidService.GetUuid());
        }

    }
}