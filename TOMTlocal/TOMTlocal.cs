using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPILib;
using WPILib.SmartDashboard;

namespace TOMTlocal
{
    /**
 * This is a demo program showing the use of the RobotDrive class.
 * The SampleRobot class is the base of a robot application that will automatically call your
 * Autonomous and OperatorControl methods at the right time as controlled by the switches on
 * the driver station or the field controls.
 *
 * The VM is configured to automatically run this class, and to call the
 * functions corresponding to each mode, as described in the SampleRobot
 * documentation.
 *
 * WARNING: While it may look like a good choice to use for your code if you're inexperienced,
 * don't. Unless you know what you are doing, complex code will be much more difficult under
 * this system. Use IterativeRobot or Command-Based instead if you're new.
 */
    public class TOMTlocal : SampleRobot
    {

        public TOMTlocal()
        {

        }

        protected override void RobotInit()
        {
            RobotMap.initializeObjects();
            SmartDashboard.PutString("State", "Starting...");
        }

        // This autonomous (along with the sendable chooser above) shows how to select between
        // different autonomous modes using the dashboard. The senable chooser code works with
        // the Java SmartDashboard. If you prefer the LabVIEW Dashboard, remove all the chooser
        // code an uncomment the GetString code to get the uto name from the text box below
        // the gyro.
        // You can add additional auto modes by adding additional comparisons to the if-else
        // structure below with additional strings. If using the SendableChooser
        // be sure to add them to the chooser code above as well.
        public override void Autonomous()
        {
            AutonomousMode.Autonomous();
            RobotMap.StopAllMotors();
        }

        /**
         * Runs the motors with arcade steering.
         */
        public static Subsystems.Shooter shooter = new Subsystems.Shooter();
        public static Subsystems.ShooterPiston shooterPiston = new Subsystems.ShooterPiston();
        public static Subsystems.ShooterArm shooterArm = new Subsystems.ShooterArm();
        public static Subsystems.RollerArm rollerArm = new Subsystems.RollerArm();
        public static Subsystems.FlashlightSpike flashlight = new Subsystems.FlashlightSpike();
        public override void OperatorControl()
        {
            while (IsOperatorControl && IsEnabled)
            {
                //Joystick Control
                RobotMap.DriveLeftRight(RobotMap.JoystickLeft.GetY(), -RobotMap.JoystickRight.GetY());
                if (RobotMap.Gamepad.GetRawButton(2) ||
                    RobotMap.Gamepad.GetRawButton(4) ||
                    RobotMap.Gamepad.GetRawButton(3) ||
                    RobotMap.Gamepad.GetRawButton(1))
                {
                    //Intake
                    if (RobotMap.Gamepad.GetRawButton(1) || RobotMap.Gamepad.GetRawButton(4))
                        shooter.RollerSpeed = -0.8;
                    //Outtake slow
                    else if (RobotMap.Gamepad.GetRawButton(3))
                        shooter.RollerSpeed = 0.5;
                    //Outtake fast
                    else if (RobotMap.Gamepad.GetRawButton(2))
                        shooter.RollerSpeed = 1;
                }
                else
                    shooter.RollerSpeed = 0;
                //Fire
                if (RobotMap.Gamepad.GetRawButton(6))
                    shooterPiston.CurrentState = true;
                else
                    shooterPiston.CurrentState = false;
                //Arm Up
                if (-RobotMap.Gamepad.GetY() > 0.5 || -RobotMap.Gamepad.GetY() < -0.5)
                {
                    if (-RobotMap.Gamepad.GetY() > 0.5)
                    {
                        shooterArm.ArmSpeed = 0.625;
                    }
                    else
                        shooterArm.ArmSpeed = -0.625;
                } 



                else if (RobotMap.Gamepad.GetPOV(0) == 180)
                {
                    shooterArm.ArmSpeed = -0.10;
                }
                else
                    shooterArm.ArmSpeed = 0;
                //Flashlight
                int flip = 1;
                int count = 0;
                SmartDashboard.PutNumber("Flip status", flip);
                if ((RobotMap.JoystickLeft.GetRawButton(1) || RobotMap.JoystickRight.GetRawButton(1)))
                {
                    SmartDashboard.PutNumber("Enter Count", count++);
                    if (flip == 1)
                        flip = 2;
                    else if (flip == 2)
                        flip = 1;
                    
                    if (flip == 1)
                        RobotMap.flashlightSpike.Set(Relay.Value.Forward);
                    else if (flip == 2)
                        RobotMap.flashlightSpike.Set(Relay.Value.Off);
                    //cooldownState = false;
                    //timer.Interval = 1;
                    //timer.Elapsed += elapsed;
                    //timer.Start();
                }
                if (RobotMap.JoystickLeft.GetRawButton(3) || RobotMap.JoystickRight.GetRawButton(3))
                {

                }
            }
            RobotMap.StopAllMotors();
        }
        System.Timers.Timer timer = new System.Timers.Timer();
        private void elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            SmartDashboard.PutNumber("Timer", e.SignalTime.Millisecond);

            if (e.SignalTime.Millisecond >= 0.5)
            {
                cooldownState = true;
                timer.Stop();
                timer = new System.Timers.Timer();
            }
        }

        bool cooldownState = true;
    }
}
