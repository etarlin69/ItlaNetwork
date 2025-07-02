using System.ComponentModel.DataAnnotations;

public class ForgotPasswordViewModel
{
    [Required(ErrorMessage = "Debe colocar su correo electrónico")]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }

    public bool HasError { get; set; }
    public string Error { get; set; }
}