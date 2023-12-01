using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISF.FAF_RF.Domain.Common
{
    public class AppError
    {
        #region Class
        public string Message { get; private set; }

        private AppError(string message)
        {
            Message = message;
        }
        #endregion

        public static AppError UnhandleError { get => new("generic error with the system"); }
        public static AppError IdError { get => new("id of the entity was not found"); }
    }
}
