using WebObrasci1.Models;

namespace WebObrasci1.Services
{
    public interface IPdfConverter
    {
        Stream ConvertToPdf(FormSubmission formSubmission);
    }
}
