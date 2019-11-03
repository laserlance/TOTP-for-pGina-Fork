# TOTP for pGina Fork
A plugin for the pGina Fork that allows a Time-based One-Time Password for Windows using the Google Authenticator App. The specific pGina fork is this one: http://mutonufoai.github.io/pgina/index.html

# Change the Secret!
You need to change the secret that is in the Gauth.cs file. If this is not done then anyone will have the secret you used and will be able to login using their phone and Google Authenticator app. Perhaps in the future I'll make this configurable at runtime.

# Default Username
The default username is "Admin". If you want to change this, you can do so in the Totpauth.cs file.

