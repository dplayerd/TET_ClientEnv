using System;

namespace BI.Shared.Models
{
    public class TET_ParametersModel
    {
        /// <summary> ID </summary>
        public Guid ID { get; set; }

        /// <summary> Type </summary>
        public string Type { get; set; }

        /// <summary> Item </summary>
        public string Item { get; set; }

        /// <summary> Seq </summary>
        public int Seq { get; set; }

        /// <summary> IsEnabled </summary>
        public bool IsEnable { get; set; }

        /// <summary> CreateUser </summary>
        public string CreateUser { get; set; }

        /// <summary> CreateDate </summary>
        public DateTime CreateDate { get; set; }

        /// <summary> ModifyUser </summary>
        public string ModifyUser { get; set; }

        /// <summary> ModifyDate </summary>
        public DateTime ModifyDate { get; set; }
    }
}
