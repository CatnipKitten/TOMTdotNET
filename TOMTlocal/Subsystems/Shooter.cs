using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TOMTlocal.Subsystems
{
    public class Shooter
    {
        public Shooter()
        {
            ChangeSpeed += setSpeed;
        }

        public delegate void EventDelegate(double amt);
        public event EventDelegate ChangeSpeed;
        private double _RollerSpeed;
        public double RollerSpeed
        {
            get { return _RollerSpeed; }
            set
            {
                if (value != _RollerSpeed || value == _RollerSpeed)
                {
                    _RollerSpeed = RollerSpeed;
                    ChangeSpeed?.Invoke(value);
                }
            }
        }

        public static void setSpeed(double speed)
        {
            RobotMap.ShooterTalonS1.Set(-speed);
            RobotMap.ShooterTalonS2.Set(-speed);
        }

        public static void stop()
        {
            RobotMap.ShooterTalonS1.Set(0);
            RobotMap.ShooterTalonS2.Set(0);
        }
    }
    public class ShooterPiston
    {
        public ShooterPiston()
        {
            ChangeSolenoidState += set;
        }
        public delegate void EventDelegate(bool amt);
        public event EventDelegate ChangeSolenoidState;
        private bool _CurrentState;
        public bool CurrentState
        {
            get { return _CurrentState; }
            set
            {
                if (value != _CurrentState || value == _CurrentState)
                {
                    _CurrentState = CurrentState;
                    ChangeSolenoidState?.Invoke(value);
                }
            }
        }
        public static void set(bool state)
        {
            RobotMap.ShooterPistonFireSol.Set(state);
        }
        public static void toggle()
        {
            if (RobotMap.ShooterPistonFireSol.Get() == true)
                set(false);
            else set(true);
        }
    }
    public class ShooterArm
    {
        public ShooterArm()
        {
            ChangeArmSpeed += setArmSpeed;
        }

        public delegate void EventDelegate(double amt);
        public event EventDelegate ChangeArmSpeed;
        private double _ArmSpeed;
        public double ArmSpeed
        {
            get { return _ArmSpeed; }
            set
            {
                if (value != _ArmSpeed || value == _ArmSpeed)
                {
                    _ArmSpeed = ArmSpeed;
                    ChangeArmSpeed?.Invoke(value);
                }
            }
        }

        public static void setArmSpeed(double speed)
        {
            RobotMap.ArmTalonA1.Set(speed);
        }
    }
}
