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
using System.Diagnostics;
using Microsoft.Deployment.WindowsInstaller;

namespace DustInTheWind.EventSource.CustomActions
{
    public static class WindowsEventsSetup
    {
        [CustomAction("WindowsEventsSetup")]
        public static ActionResult Execute(Session session)
        {
            try
            {
                string logName = session.CustomActionData["LogName"];
                string logEventSource = session.CustomActionData["LogEventSource"];

                EventLog eventLog = new EventLog(logName);
                eventLog.MaximumKilobytes = 4194240; // 4GB - 64KB
                eventLog.ModifyOverflowPolicy(OverflowAction.DoNotOverwrite, 0);

                return ActionResult.Success;
            }
            catch (Exception ex)
            {
                session.Log(ex.ToString());
                throw;
            }
        }
    }
}