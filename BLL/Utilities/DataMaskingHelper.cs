namespace BLL.Utilities;

/// <summary>
/// Kişisel verilerin maskelenmesi için yardımcı sınıf
/// KVKK uyumluluğu için hassas bilgilerin güvenli gösterimi
/// </summary>
public static class DataMaskingHelper
{
    /// <summary>
    /// TC Kimlik Numarasını maskeler - sadece son 4 hanesi görünür
    /// Örnek: 12345678901 -> ***8901
    /// </summary>
    /// <param name="tcKimlikNo">TC Kimlik Numarası</param>
    /// <returns>Maskelenmiş TC Kimlik Numarası</returns>
    public static string MaskTcKimlikNo(string? tcKimlikNo)
    {
        if (string.IsNullOrEmpty(tcKimlikNo) || tcKimlikNo.Length < 4)
            return "***";
        
        return "***" + tcKimlikNo.Substring(tcKimlikNo.Length - 4);
    }
    
    /// <summary>
    /// SSK Numarasını maskeler - sadece son 4 hanesi görünür
    /// Örnek: 123456789012 -> ***9012
    /// </summary>
    /// <param name="sskNo">SSK Numarası</param>
    /// <returns>Maskelenmiş SSK Numarası</returns>
    public static string MaskSskNo(string? sskNo)
    {
        if (string.IsNullOrEmpty(sskNo) || sskNo.Length < 4)
            return "***";
        
        return "***" + sskNo.Substring(sskNo.Length - 4);
    }
    
    /// <summary>
    /// Email adresini maskeler - @ işaretinden önceki kısmın ortasını gizler
    /// Örnek: john.doe@company.com -> jo***oe@company.com
    /// </summary>
    /// <param name="email">Email adresi</param>
    /// <returns>Maskelenmiş email adresi</returns>
    public static string MaskEmail(string? email)
    {
        if (string.IsNullOrEmpty(email) || !email.Contains('@'))
            return "***@***.***";
        
        var parts = email.Split('@');
        var localPart = parts[0];
        var domainPart = parts[1];
        
        if (localPart.Length <= 2)
            return "***@" + domainPart;
        
        var maskedLocal = localPart.Substring(0, 2) + "***" + localPart.Substring(localPart.Length - 2);
        return maskedLocal + "@" + domainPart;
    }
    
    /// <summary>
    /// Telefon numarasını maskeler - sadece son 4 hanesi görünür
    /// Örnek: 0555 123 45 67 -> ***5467
    /// </summary>
    /// <param name="phoneNumber">Telefon numarası</param>
    /// <returns>Maskelenmiş telefon numarası</returns>
    public static string MaskPhoneNumber(string? phoneNumber)
    {
        if (string.IsNullOrEmpty(phoneNumber))
            return "***";
        
        // Sadece rakamları al
        var digitsOnly = new string(phoneNumber.Where(char.IsDigit).ToArray());
        
        if (digitsOnly.Length < 4)
            return "***";
        
        return "***" + digitsOnly.Substring(digitsOnly.Length - 4);
    }
}
