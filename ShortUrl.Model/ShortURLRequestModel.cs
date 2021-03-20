using System;
using System.ComponentModel.DataAnnotations;
using static ShortUrl.Model.Validation.CustomValidation;

namespace ShortUrl.Model
{
    public class ShortURLRequestModel
    {
        [Required]
        [CheckUrlValid(ErrorMessage = "Please enter a valid Url")]
        public string LongURL { get; set; }

        public DateTime TimeStamp
        {
            get
            {
                return DateTime.Now;
            }
        }
    }
}
