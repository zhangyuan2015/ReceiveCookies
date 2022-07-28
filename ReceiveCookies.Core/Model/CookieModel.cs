namespace ReceiveCookies.Core.Model
{
    /// <summary>
    /// 
    /// </summary>
    public class CookieModel
    {
        /*
domain: "github.com"
expirationDate: 1686661592.931744
hostOnly: true
httpOnly: true
name: "_device_id"
path: "/"
sameSite: "lax"
secure: true
session: false
storeId: "0"
value: "********"
         */

        /// <summary>
        /// "github.com"
        /// </summary>
        public string domain { get; set; }

        /// <summary>
        /// 1686661592.931744
        /// </summary>
        public double expirationDate { get; set; }

        /// <summary>
        /// true
        /// </summary>
        public bool hostOnly { get; set; }

        /// <summary>
        /// true
        /// </summary>
        public bool httpOnly { get; set; }

        /// <summary>
        /// "_device_id"
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// "/"
        /// </summary>
        public string path { get; set; }

        /// <summary>
        /// "strict" "lax" "none"
        /// </summary>
        public string sameSite { get; set; }

        /// <summary>
        /// true
        /// </summary>
        public bool secure { get; set; }

        /// <summary>
        /// false
        /// </summary>
        public bool session { get; set; }

        /// <summary>
        /// "0"
        /// </summary>
        public string storeId { get; set; }

        /// <summary>
        /// ********
        /// </summary>
        public string value { get; set; }
    }
}