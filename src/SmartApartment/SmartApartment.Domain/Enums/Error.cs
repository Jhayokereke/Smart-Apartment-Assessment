using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartApartment.Domain.Enums
{
    public enum Error
    {
        [Description("Request completed successfully")]
        NO_ERROR,
        [Description("An error occured while processing your request")]
        APPLICATION_ERROR,
        [Description("No resource was found that matches the request")]
        NOT_FOUND_ERROR,
        [Description("Resource already exists in the selected destination")]
        DUPLICATE_ERROR
    }
}
