using System;
using System.Diagnostics;
using log4net;
using pGina.Shared.Interfaces;
using pGina.Shared.Types;
using pGina.Shared.Settings;
using Newtonsoft.Json;

namespace pGina.Plugin.Totpauth{

    public sealed class Totpauth : IPluginConfiguration, IPluginAuthentication {

        private ILog _logger = LogManager.GetLogger("Totpauth");

        public static Guid Totpauthguid = new Guid("{DF106038-19F4-4CDD-BBB6-A6ADD12860B3}");

        private readonly string _defaultDescription = "Time-based One Time Password (TOTP) Authentication for pGina";

        private dynamic _settings = null;

        private readonly Gauth _gauth = new Gauth();
        
        public Totpauth() {

            using (Process me = Process.GetCurrentProcess()) {

                _settings = new pGinaDynamicSettings(Totpauthguid);

                _settings.SetDefault("ShowDescription", true);

                _settings.SetDefault("Description", _defaultDescription);

                _logger.DebugFormat("Plugin initialized on {0} in PID: {1} Session: {2}", Environment.MachineName, me.Id, me.SessionId);
            }
        }

        public string Name {

            get {

                return "TOTP Authenticator";
            }
        }

        public string Description {

            get {

                return _settings.Description;
            }
        }

        public string Version {

            get {

                return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }

        public Guid Uuid {

            get {

                return Totpauthguid;
            }
        }

        BooleanResult IPluginAuthentication.AuthenticateUser(SessionProperties properties) {

            try {

                _logger.InfoFormat("Properties: {0}", JsonConvert.SerializeObject(properties, Formatting.Indented));

                _logger.DebugFormat("AuthenticateUser({0})", properties.Id.ToString());

                UserInformation userInfo = properties.GetTrackedSingle<UserInformation>();

                _logger.DebugFormat("Found username: {0}", userInfo.Username);

                _gauth.Identity = "pGina TOTP Authenticator";

                if (userInfo.Password.ToString() == _gauth.OneTimePassword && (userInfo.Username == "Admin")) {

                    _logger.InfoFormat("Authenticated user: {0} using {1}", userInfo.Username, _gauth.OneTimePassword.ToString());

                    return new BooleanResult() { Success = true };

                } else {

                    _logger.ErrorFormat("Failed to authenticate user: {0} using {1}", userInfo.Username, _gauth.OneTimePassword.ToString());

                    return new BooleanResult() { Success = false, Message = string.Format("Error") };
                }
            } catch (Exception e) {

                _logger.ErrorFormat("AuthenticateUser exception: {0}", e);

                throw;
            }
        }

        public void Configure() {

            Configuration conf = new Configuration();

            conf.ShowDialog();
        }

        public void Starting() { }

        public void Stopping() { }
    }
}
