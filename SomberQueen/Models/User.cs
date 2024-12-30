public class User
{
    public Guid user_id { get; set; }  
    public string username { get; set; }
    public string role { get; set; }
    public byte[] decryption_key { get; set; }
    public DateTime created_at { get; set; }
} 