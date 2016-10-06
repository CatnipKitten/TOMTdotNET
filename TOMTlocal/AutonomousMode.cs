using System;
using System.Threading;
using WPILib.SmartDashboard;

namespace TOMTlocal
{
    class AutonomousMode
    {
        public static bool hasRun = false;
        public enum AutoModes
        {
            Forward,
            Spy
        }
        public static void Autonomous()
        {
            if (hasRun == true)
                return;
            AutoModes currentMode = CurrentMode();
            SmartDashboard.PutString("Autonomous Mode", AutoModes.Spy.ToString());
            if (currentMode == AutoModes.Forward)
                Forward();
            else if (currentMode == AutoModes.Spy)
                Spy();
        }

        public static AutoModes CurrentMode()
        {
            if (RobotMap.JoystickLeft.GetZ() >= 0.5 || RobotMap.JoystickRight.GetZ() >= 0.5)
                return AutoModes.Forward;
            else
                return AutoModes.Spy;
        }

        private static void Forward()
        {
            // Robot.arm.setPower(-1, 1000);
            // Robot.arm.setPower(0);
            RobotMap.DriveLeftRight(-.85, -.85);
            Thread.Sleep(3500);
            RobotMap.StopAllMotors();
            // Robot.arm.setPower(0.35, 1000);
            // Robot.arm.setPower(0);
        }

        private static void Spy()
        {
            Shoot();
        }

        #region Assisted Stuff
        private static void Shoot()
        {
            // Rev up motors
            TOMTlocal.shooter.RollerSpeed = 12;
            Thread.Sleep(300);
            // Fire the piston
            TOMTlocal.shooterPiston.CurrentState = true;
            Thread.Sleep(350);
            TOMTlocal.shooterPiston.CurrentState = false;
            // Bring arm down
            TOMTlocal.shooterArm.ArmSpeed = -0.5;
            TOMTlocal.shooter.RollerSpeed = 0;
            Thread.Sleep(200);
            TOMTlocal.shooterArm.ArmSpeed = 0;
        }
        #endregion
    }
}
