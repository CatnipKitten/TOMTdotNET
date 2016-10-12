using System;
using System.Diagnostics;
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

        #region Modes
        private static void Forward()
        {
            // Robot.arm.setPower(-1, 1000);
            // Robot.arm.setPower(0);
            RobotMap.DriveLeftRight(-.85, -.85);
            Thread.Sleep(250);
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            while (RobotMap.navX.GetRawAccelX() > -0.25 && stopWatch.Elapsed.Seconds < 5)
            {
                RobotMap.DriveLeftRight(-.85, -.85);
            }
            if (stopWatch.Elapsed.Seconds < 5)
            {
                TOMTlocal.shooterArm.ArmSpeed = -1;
                Thread.Sleep(750);
                TOMTlocal.shooterArm.ArmSpeed = 0;
                stopWatch.Restart();
                while (RobotMap.navX.GetRawAccelX() > -0.25 && stopWatch.Elapsed.Seconds < 5)
                {
                    RobotMap.DriveLeftRight(-.85, -.85);
                }
            }
            RobotMap.StopAllMotors();
            // Robot.arm.setPower(0.35, 1000);
            // Robot.arm.setPower(0);
        }

        private static void Spy()
        {
            //Shoot the boulder into the tower (or audience :D (pls dont get mad))
            Shoot();
            //Turn away from the wall and align with the low goal
            Turn();
            //Drive forward a distance
            RobotMap.DriveLeftRight(-0.75, -0.75);
            Thread.Sleep(500);
            //Align to the low bar
            Align90();
            //First pass through low bar from courtyard
            DriveCourtyard();
            //Second pass through low bar from outer works, here's where the points are scored 8)
            DriveOuterworks();
            //Back through low bar from courtyard, ready to get ball back
            DriveCourtyard();
            //Stops all motors
            RobotMap.StopAllMotors();
        }

        #endregion
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
        public static void Turn()
        {
            //Motor speed variable
            double motorSpeed = 0.3;
            //Turn until our angle is 83.5 in the yaw
            while (RobotMap.navX.GetYaw() < 83.5)
            {
                //Desired velocity
                double velocity = 5.0;
                if(Math.Sqrt(Math.Pow(RobotMap.navX.GetVelocityX(), 2) + Math.Pow(RobotMap.navX.GetVelocityY(), 2)) < velocity)
                {
                    motorSpeed += 0.05;
                }
                RobotMap.DriveLeftRight(0, motorSpeed);
            }
        }
        public static void Align90()
        {
            double desiredAngle = 90;

            SmartDashboard.PutNumber("Angle Difference", desiredAngle - RobotMap.navX.GetYaw());
            while (RobotMap.navX.GetYaw() > desiredAngle)
            {
                SmartDashboard.PutNumber("Current Angle", RobotMap.navX.GetYaw());
                RobotMap.DriveLeftRight(0, 0.4);
            }
            RobotMap.DriveLeftRight(0, 0);
        }
        //Drives through the lowbar into the outerworks
        public static void DriveCourtyard()
        {
            while (RobotMap.navX.GetYaw() < -7)
            {
                RobotMap.DriveLeftRight(-0.7, -0.7);
                SmartDashboard.PutNumber("Roll", RobotMap.navX.GetYaw());
            }
            RobotMap.DriveLeftRight(-0.85, -0.85);
            SmartDashboard.PutNumber("Roll", RobotMap.navX.GetYaw());
            Thread.Sleep(50);
        }
        public static void DriveOuterworks()
        {
            while (RobotMap.navX.GetRoll() > -17)
            {
                SmartDashboard.PutNumber("Roll", RobotMap.navX.GetRoll());
                RobotMap.DriveLeftRight(0.7, 0.7);
            }
            RobotMap.DriveLeftRight(0.85, 0.85);
            SmartDashboard.PutNumber("Roll", RobotMap.navX.GetRoll());
            Thread.Sleep(100);
        }
        #endregion
    }
}
