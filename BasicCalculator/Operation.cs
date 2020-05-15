using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BasicCalculator
{
    /// <summary>
    /// Hold info about a single calc operation
    /// </summary>
    public class Operation
    {
        #region Public Properties
        public string LeftSide { get; set; }

        public string RightSide { get; set; }

        public OperationType OperationType { get; set; }

        public Operation InnerOperation { get; set; }

        #endregion

        #region Constructor
        public Operation()
        {
            this.LeftSide = string.Empty;
            this.RightSide = string.Empty;
        }

        #endregion
    }
}
