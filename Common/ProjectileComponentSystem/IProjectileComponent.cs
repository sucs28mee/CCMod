﻿using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCMod.Common.ProjectileComponentSystem
{
    internal interface IProjectileComponent
    {
        public void RegisterHooks(ProjectileEntityHooks hooks);
    }
}
