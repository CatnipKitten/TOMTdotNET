using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TOMTlocal.Subsystems
{
    public class RollerArm
    {
        public RollerArm()
        {
            ChangeRollerState += Run;
        }
        public delegate void EventDelegate(bool amt);
        public event EventDelegate ChangeRollerState;
        private bool _CurrentState;
        public bool CurrentState
        {
            get { return _CurrentState; }
            set
            {
                if (value != _CurrentState || value == _CurrentState)
                {
                    _CurrentState = CurrentState;
                    ChangeRollerState?.Invoke(value);
                }
            }
        }
        private void Run(bool value)
        {
            if (value == true && RobotMap.UpperLimitSwitch.Get() == false)
            {
                RobotMap.RollerTalon.Set(13);
                RobotMap.RollerEndTalon.Set(0);
            }
            else if (RobotMap.LowerLimitSwitch.Get() == false)
            {
                RobotMap.RollerTalon.Set(0);
                RobotMap.RollerEndTalon.Set(1);
            }
        }
    }
}
