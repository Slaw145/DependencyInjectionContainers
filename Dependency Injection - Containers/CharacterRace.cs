﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dependency_Injection___Containers
{
    public interface ICharacterRace
    {
        ICharacterRace CreateCharacterRace(ICharacterRace characterrace);
    }
}
