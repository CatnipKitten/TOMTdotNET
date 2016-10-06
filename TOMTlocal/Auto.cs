using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetworkTables;
using System.Threading;

namespace TOMTlocal
{
    static class Auto
    {
        static NetworkTable table = NetworkTable.GetTable("autonomous");
        public static void runAutoLowbar()
        {
            Thread thread = new Thread(updateNavX);
            thread.Start();


        }
        static void updateNavX()
        {
            while (true)
            {
                table.PutNumber("Yaw", RobotMap.navX.GetYaw());
                table.PutNumber("Pitch", RobotMap.navX.GetPitch());
                table.PutNumber("Roll", RobotMap.navX.GetRoll());

                table.PutNumber("Velocity X", RobotMap.navX.GetVelocityX());
                table.PutNumber("Velocity Y", RobotMap.navX.GetVelocityY());
                table.PutNumber("Velocity Z", RobotMap.navX.GetVelocityZ());
            }
        }
    }
}
