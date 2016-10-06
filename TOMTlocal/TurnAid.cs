using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TOMTlocal
{
    public enum TurnDirection
    {
        left,
        right
    }
    class TurnAid
    {
        public static int degrees = 90;
        public static bool isCalibrated { get; set; }
        public static List<int> increments;
        public static void TurnAssist(TurnDirection direction)
        {
            if (isCalibrated != true)
                calibrate();
            double currentAngle = RobotMap.navX.GetAngle();
            if(direction == TurnDirection.left)
            {

            }
        }
        private static void calibrate()
        {
            for(int x = 0; x <= 360; x += degrees)
            {

            }
        }
    }
}
