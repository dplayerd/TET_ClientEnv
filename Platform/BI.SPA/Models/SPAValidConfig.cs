﻿using Platform.AbstractionClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI.SPA.Models
{
    /// <summary> 供應商改版審核設定 </summary>
    public class SPAValidConfig : ValidateConfig
    {
        /// <summary> 是否允許編輯 </summary>
        public bool CanEdit { get; set; }
    }
}

