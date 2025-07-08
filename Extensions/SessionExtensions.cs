using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace quiz_project.Extensions
{
    public static class SessionExtensions
    {
        public static void SetInt(this ISession session, SessionKeys key, int value)
            => session.SetInt32(key.ToString(), value);

        public static int? GetInt(this ISession session, SessionKeys key)
            => session.GetInt32(key.ToString());

        public static void SetString(this ISession session, SessionKeys key, string value)
            => session.SetString(key.ToString(), value);

        public static string? GetString(this ISession session, SessionKeys key)
            => session.GetString(key.ToString());

        public static void Remove(this ISession session, SessionKeys key)
            => session.Remove(key.ToString());
    }
}