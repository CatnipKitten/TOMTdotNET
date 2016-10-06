using System;
using WPILib;
using WPILib.Extras.NavX;

namespace TOMTlocal
{
    public class RobotMap
    {
        #region Robot Objects
        //Talons related to the drive train
        public static CANTalon DriveTrainTalonRl { get; private set; }
        public static CANTalon DriveTrainTalonRr { get; private set; }
        public static CANTalon DriveTrainTalonFr { get; private set; }
        public static CANTalon DriveTrainTalonFl { get; private set; }
        public static RobotDrive DriveTrainRobotDrive { get; private set; }

        //Gear box shifters
        public static Compressor GearBoxCompressor { get; private set; }
        public static Solenoid GearBoxGearShift { get; private set; }

        //PDP
        public static PowerDistributionPanel PDPPowerDistributionPanel { get; private set; }

        //Shooter talons
        public static CANTalon GrapplingTalonG1 { get; private set; }
        public static CANTalon GrapplingTalonG2 { get; private set; }
        public static CANTalon ShooterTalonS1 { get; private set; }
        public static CANTalon ShooterTalonS2 { get; private set; }
        public static CANTalon ArmTalonA1 { get; private set; }

        //Shooter solenoid
        public static Solenoid ShooterPistonFireSol { get; private set; }

        //Roller
        public static CANTalon RollerTalon { get; private set; }
        public static CANTalon RollerEndTalon { get; private set; }
        public static DigitalInput UpperLimitSwitch { get; private set; }
        public static DigitalInput LowerLimitSwitch { get; private set; }

        //Glorious NavX
        public static AHRS navX { get; private set; }

        //Spike for Flashlight
        public static Relay flashlightSpike { get; private set; }

        //Joysticks & Gamepad
        public static Joystick JoystickLeft { get; private set; }
        public static Joystick JoystickRight { get; private set; }
        public static Joystick Gamepad { get; private set; }
        #endregion

        //Delegates
        delegate void LiveWindowDelegate(string name, string channel, WPILib.LiveWindow.ILiveWindowSendable component);
        static LiveWindowDelegate AddComponent = (group, name, component) => { WPILib.LiveWindow.LiveWindow.AddActuator(group, name, component); };
        static LiveWindowDelegate AddSensor = (group, name, component) => { WPILib.LiveWindow.LiveWindow.AddSensor(group, name, component); };

        //Initializes each of the objects.
        public static void initializeObjects()
        {
            //Instantiates the drive train components and adds them to the LiveWindow (test mode)
            DriveTrainTalonRl = new CANTalon(RobotValues.TalonRL);
            AddComponent("Drive Train", "Rear Left Talon", DriveTrainTalonRl);
            DriveTrainTalonRr = new CANTalon(RobotValues.TalonRR);
            AddComponent("Drive Train", "Rear Right Talon", DriveTrainTalonRl);
            DriveTrainTalonFl = new CANTalon(RobotValues.TalonFL);
            AddComponent("Drive Train", "Front Left Talon", DriveTrainTalonFl);
            DriveTrainTalonFr = new CANTalon(RobotValues.TalonFR);
            AddComponent("Drive Train", "Front Right Talon", DriveTrainTalonFr);

            RollerTalon = new CANTalon(10);
            AddComponent("Roller", "Raise", RollerTalon);
            RollerEndTalon = new CANTalon(12);
            AddComponent("Roller", "End", RollerEndTalon);
            UpperLimitSwitch = new DigitalInput(1);
            AddComponent("Roller", "Lower Limit", UpperLimitSwitch);
            LowerLimitSwitch = new DigitalInput(0);
            AddComponent("Roller", "Upper Limit", LowerLimitSwitch);

            DriveTrainRobotDrive = new RobotDrive(DriveTrainTalonRl, DriveTrainTalonRr, DriveTrainTalonFl, DriveTrainTalonFr);
            DriveTrainRobotDrive.SafetyEnabled = false;
            DriveTrainRobotDrive.Expiration = 0.1;
            DriveTrainRobotDrive.MaxOutput = 1.0;

            PDPPowerDistributionPanel = new PowerDistributionPanel();
            AddSensor("PDP", "PDP", PDPPowerDistributionPanel);

            GearBoxCompressor = new Compressor(RobotValues.Compressor);
            GearBoxGearShift = new Solenoid(RobotValues.PCM, RobotValues.GearShift);
            AddComponent("Pneumatics", "Gear Box Shifter", GearBoxGearShift);

            ArmTalonA1 = new CANTalon(RobotValues.TalonA1);
            AddComponent("Shooter", "Arm Talon", ArmTalonA1);

            ShooterTalonS1 = new CANTalon(RobotValues.TalonS1);
            ShooterTalonS1.SafetyEnabled = false;
            AddComponent("Shooter", "Shooter Talon 1", ShooterTalonS1);
            ShooterTalonS2 = new CANTalon(RobotValues.TalonS2);
            ShooterTalonS2.SafetyEnabled = false;
            AddComponent("Shooter", "Shooter Talon 2", ShooterTalonS2);

            ShooterPistonFireSol = new Solenoid(RobotValues.PCM, RobotValues.Fire);

            GrapplingTalonG1 = new CANTalon(RobotValues.TalonG1);
            AddComponent("Grapple", "Grapple 1", GrapplingTalonG1);
            GrapplingTalonG2 = new CANTalon(RobotValues.TalonG2);
            AddComponent("Grapple", "Grapple 2", GrapplingTalonG2);

            JoystickLeft = new Joystick(RobotValues.JoystickLeft);
            JoystickRight = new Joystick(RobotValues.JoystickRight);
            Gamepad = new Joystick(RobotValues.Gamepad);

            flashlightSpike = new Relay(0);
            AddSensor("Relay", "Relay", flashlightSpike);

            try { navX = new AHRS(SPI.Port.MXP); }
            catch (Exception) { DriverStation.ReportError("Error instantiating NavX. Functionability will be disabled.", true); }
        }

        public static void StopAllMotors()
        {
            DriveTrainTalonFl.Set(0);
            DriveTrainTalonFr.Set(0);
            DriveTrainTalonRl.Set(0);
            DriveTrainTalonRr.Set(0);

            ArmTalonA1.Set(0);
            ShooterTalonS1.Set(0);
            ShooterTalonS2.Set(0);

            GrapplingTalonG1.Set(0);
            GrapplingTalonG2.Set(0);
        }

        public static void DriveLeftRight(double left, double right)
        {
            DriveTrainTalonFl.Set(left);
            DriveTrainTalonRl.Set(left);
            DriveTrainTalonFr.Set(right);
            DriveTrainTalonRr.Set(right);
        }
    }

    struct RobotValues
    {
        //AVAILABLE TALONS: 1 - 12

        //Drive train talons
        public const int TalonFR = 1;
        public const int TalonFL = 3;
        public const int TalonRR = 2;
        public const int TalonRL = 4;

        //Roller talons
        public const int RollerTalon = 15;
        public const int RollerEnd = 16;

        //Arm talon
        public const int TalonA1 = 9;

        //Shooter talons
        public const int TalonS1 = 6;
        public const int TalonS2 = 8;

        // Roller encoder
        public const int shooterEncoderChannelA = 2;
        public const int shooterEncoderChannelB = 3;

        //Grappling talons
        public const int TalonG1 = 5;
        public const int TalonG2 = 7;

        //PDP
        public const int PDP = 0;

        //Pneumatics
        public const int PCM = 13;
        public const int Compressor = 13;
        public const int GearShift = 0;
        public const int Fire = 1;

        //Camera
        public const String CameraID = "cam2";

        //Joysticks
        public const int JoystickLeft = 0;
        public const int JoystickRight = 1;
        public const int Gamepad = 2;

        //Joystick Buttons
        public const int ArmUpButton = 5;
        public const int ArmDownButton = 6;
        public const int GearShiftButton = 1;
        public const int RevUpButton = 4;
        public const int FireButton = 6;
        public const int GrappleButton = 8;
        public const int RevBackButton = 1;
        public const int TrackingSwitch = 9;
        public const int AutoConditional1 = 4;
        public const int AutoConditional2 = 6;
        public const int AutoConditionalTime = 12;
        public const int RevSlow = 3;
        public const int RollerArmDown = 9;

        //Joystick DIO
        public const int DONotify = 1;
        public const int DOWarning = 2;
    }

    struct RobotData
    {
        public static double YawOffset { private get; set; }
        public static double PitchOffset { private get; set; }
        public static double RollOffset { private get; set; }

        public static double Yaw => Math.Sin(YawOffset * (Math.PI / 180)) * RobotMap.navX.GetAngle();
        public static double Pitch => Math.Sin(PitchOffset * (Math.PI / 180)) * RobotMap.navX.GetPitch();
        public static double Roll => Math.Sin(RollOffset * (Math.PI / 180)) * RobotMap.navX.GetRoll();
    }
}
