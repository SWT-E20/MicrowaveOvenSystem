using System;
using Microwave.Classes.Interfaces;

namespace Microwave.Classes.Boundary
{
    public class PowerTube : IPowerTube
    {
        private IOutput myOutput;

        private bool IsOn = false;

        public PowerTube(IOutput output)
        {
            myOutput = output;
        }
        //Her opstod en fjel da power var sæt til at være mellem 1 til 100, men den skal være mellem 50 og 700 bagge er inclusiv
        public void TurnOn(int power)
        {
            if (power < 50 || 700 < power)
            {
                throw new ArgumentOutOfRangeException("power", power, "Must be between 50 and 700 (incl.)");
            }

            if (IsOn)
            {
                throw new ApplicationException("PowerTube.TurnOn: is already on");
            }

            myOutput.OutputLine($"PowerTube works with {power}");
            IsOn = true;
        }

        public void TurnOff()
        {
            if (IsOn)
            {
                myOutput.OutputLine($"PowerTube turned off");
            }

            IsOn = false;
        }
    }
}