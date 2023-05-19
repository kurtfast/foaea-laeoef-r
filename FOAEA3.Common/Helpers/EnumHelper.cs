﻿using FOAEA3.Model.Enums;

namespace FOAEA3.Common.Helpers
{
    public static class EnumHelper
    {
        public static string AsString(this ApplicationState state)
        {
            return $"{(int)state} [{state}]";
        }
    }
}
