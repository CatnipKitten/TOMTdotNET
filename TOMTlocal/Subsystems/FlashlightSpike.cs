using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using WPILib;

namespace TOMTlocal.Subsystems
{
    public class FlashlightSpike
    {
        public FlashlightSpike()
        {
            ChangeSpeed += setValue;
        }

        System.Timers.Timer cooldown;
        bool cooldownState = true;

        public delegate void EventDelegate(bool amt);
        public event EventDelegate ChangeSpeed;
        private bool _State;
        public bool State
        {
            get { return _State; }
            set
            {
                if (value == State && cooldownState)
                {
                    
                    _State = State;
                    ChangeSpeed?.Invoke(value);
                    cooldownState = false;
                    cooldown = new System.Timers.Timer(250);
                    cooldown.Elapsed += async(sender, e) => await cooldownMethod();
                    cooldown.Start();
                }
            }
        }

        public Task cooldownMethod()
        {
            
            cooldownState = true;
            return null;
        }

        public static void setValue(bool value)
        {
            WPILib.Relay.Value state = WPILib.Relay.Value.Off;
            if (value == true)
                state = WPILib.Relay.Value.On;
            RobotMap.flashlightSpike.Set(state);
        }
    }
}
