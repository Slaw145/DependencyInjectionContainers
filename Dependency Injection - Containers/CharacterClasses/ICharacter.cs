﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dependency_Injection___Containers
{
    public interface ICharacter
    {
        int Strength { get; set; }
        int Stamina { get; set; }

        int AdditionalStrength { get; set; }
        int AdditionalStamina { get; set; }
    }
}
