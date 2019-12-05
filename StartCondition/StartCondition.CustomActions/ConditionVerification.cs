// WixQ
// Copyright (C) 2019 Dust in the Wind
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using Microsoft.Deployment.WindowsInstaller;

namespace DustInTheWind.StartCondition.CustomActions
{
    public class SystemTimeVerification
    {
        [CustomAction("SystemTimeVerification")]
        public static ActionResult Execute(Session session)
        {
            try
            {
                session.Log("Begin SystemTimeVerification custom action");

                session["IS_CONDITION_TRUE"] = IsDateTimeEven() ? "yes" : "no";

                return ActionResult.Success;
            }
            finally
            {
                session.Log("End SystemTimeVerification custom action");
            }
        }

        private static bool IsDateTimeEven()
        {
            DateTime dateTime = DateTime.UtcNow;
            long ticks = dateTime.Ticks;
            return ticks % 2 == 0;
        }
    }
}