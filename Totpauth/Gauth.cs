using System;
using System.Linq;
using System.Security.Cryptography;
using System.Windows.Threading;

namespace pGina.Plugin.Totpauth
{
    public class Gauth {

        private int _onetimepassword { get; set; }

        private int _secondstogo;

        private string _identity;

        private byte[] _secret;
       
        private byte[] _hmac { get; set; }

        private int _offset { get; set; }

        private string _secretbase32 {

            get => Base32.ToString(Secret);

            set {

                try {

                    Secret = Base32.ToBytes(value);

                } catch {

                }
            }
        }

        public int SecondsToGo {

            get => _secondstogo;

            private set {

                _secondstogo = value;

                if (SecondsToGo == 30) {

                    CalculateOneTimePassword();
                }
            }
        }

        public string Identity {

            get => _identity;

            set {

                _identity = value;

                CalculateOneTimePassword();
            }
        }

        public byte[] Secret {

            get => _secret;

            set {

                _secret = value;

                CalculateOneTimePassword();
            }
        }

        public string QrCodeUrl => GetQrCodeUrl();

        public long Timestamp { get; private set; }

        public string OneTimePassword { get; private set; }
        
        public Gauth() {

            var timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(500) };

            timer.Tick += (s, e) => SecondsToGo = 30 - Convert.ToInt32(GetUnixTimestamp() % 30);

            timer.IsEnabled = true;

            //Change this now. Now.
            Secret = new byte[] { 0x63, 0x68, 0x61, 0x6e, 0x67, 0x65, 0x74, 0x68, 0x69, 0x73, 0x6e, 0x6f, 0x77, 0x6e, 0x6f, 0x77 };

            Identity = "TOTP Authenticator";
        }

        private static long GetUnixTimestamp() {

            return Convert.ToInt64(Math.Round((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds));
        }

        private string GetQrCodeUrl() {

            return
                $"https://www.google.com/chart?chs=200x200&chld=M|0&cht=qr&chl=otpauth://totp/{this.Identity}%3Fsecret%3D{this._secretbase32}";
        }

        private void CalculateOneTimePassword() {

            // https://tools.ietf.org/html/rfc4226

            Timestamp = Convert.ToInt64(GetUnixTimestamp() / 30);

            byte[] data = BitConverter.GetBytes(Timestamp).Reverse().ToArray();

            _hmac = new HMACSHA1(Secret).ComputeHash(data);

            _offset = _hmac.Last() & 0x0F;

            _onetimepassword = (
                ((_hmac[_offset + 0] & 0x7f) << 24) |
                ((_hmac[_offset + 1] & 0xff) << 16) |
                ((_hmac[_offset + 2] & 0xff) << 8) |
                (_hmac[_offset + 3] & 0xff)) % 1000000;

            OneTimePassword = _onetimepassword.ToString("D6");
        }
    }
}
